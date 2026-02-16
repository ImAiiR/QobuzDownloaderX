"""Qobuz API client - Python reimplementation of QopenAPI logic.

Signature format from the original C# Form1.cs (line 364):
  "trackgetFileUrlformat_id" + formatIdString + "intentstreamtrack_id"
  + trackIdString + time + appSecret

API call format from Form1.cs (line 374):
  All params (app_id, user_auth_token, request_ts, request_sig, track_id,
  format_id, intent) are passed as query parameters, NOT headers.
"""

import base64
import hashlib
import logging
import re
import time

import requests

logger = logging.getLogger(__name__)

QOBUZ_BASE_URL = "https://www.qobuz.com/api.json/0.2"


class QobuzAPI:
    def __init__(self):
        self.app_id = None
        self.app_secret = None
        self.user_auth_token = None
        self.user_id = None
        self.user_display_name = None
        self.user_label = None
        self.user_avatar = None
        self.session = requests.Session()
        self.session.headers.update({
            "User-Agent": "Mozilla/5.0 (Windows NT 10.0; Win64; x64) "
                          "AppleWebKit/537.36 (KHTML, like Gecko) "
                          "Chrome/120.0.0.0 Safari/537.36",
        })

    def _get(self, endpoint, params=None):
        """Make authenticated GET request using headers for auth."""
        headers = {}
        if self.app_id:
            headers["X-App-Id"] = self.app_id
        if self.user_auth_token:
            headers["X-User-Auth-Token"] = self.user_auth_token
        resp = self.session.get(
            f"{QOBUZ_BASE_URL}/{endpoint}",
            params=params,
            headers=headers,
            timeout=30,
        )
        resp.raise_for_status()
        return resp.json()

    # ------------------------------------------------------------------
    # App ID & Secret extraction from Qobuz web player JS bundles
    # ------------------------------------------------------------------

    def get_app_id_and_secret(self):
        """Extract app_id and app_secret from Qobuz web player JS bundles.

        The web player loads multiple JS files. The app_id is typically in
        the main bundle, while the app_secret is split across seed values
        that must be reassembled.
        """
        login_page = self.session.get(
            "https://play.qobuz.com/login", timeout=15
        )
        login_page.raise_for_status()
        page_html = login_page.text

        # Find all script src URLs
        script_urls = re.findall(
            r'<script[^>]+src="([^"]+)"', page_html
        )
        if not script_urls:
            raise ValueError("No script tags found on Qobuz login page")

        logger.info(f"Found {len(script_urls)} script URLs")

        bundle_contents = []
        for script_url in script_urls:
            if not script_url.startswith("http"):
                script_url = "https://play.qobuz.com" + script_url
            try:
                resp = self.session.get(script_url, timeout=30)
                resp.raise_for_status()
                bundle_contents.append(resp.text)
            except Exception as e:
                logger.debug(f"Failed to fetch {script_url}: {e}")

        all_js = "\n".join(bundle_contents)

        # --- Extract app_id ---
        self._extract_app_id(all_js)
        if not self.app_id:
            raise ValueError("Could not extract app_id from Qobuz JS bundles")
        logger.info(f"Extracted app_id: {self.app_id}")

        # --- Extract app_secret ---
        self._extract_app_secret(all_js)
        if self.app_secret:
            logger.info(f"Extracted app_secret (length {len(self.app_secret)})")
        else:
            logger.warning("Could not extract app_secret from JS bundles")

        return self.app_id

    def _extract_app_id(self, js_content):
        """Try multiple patterns to find app_id in JS content."""
        patterns = [
            # production:{api:{appId:"XXXXX",appSecret:"YYYYY"
            r'production\s*:\s*\{\s*api\s*:\s*\{\s*appId\s*:\s*"(\d+)"\s*,\s*appSecret\s*:\s*"(\w+)"',
            # {app_id:"XXXXX" or app_id: "XXXXX"
            r'app_id\s*[:=]\s*["\'](\d{9,})["\']',
            # appId:"XXXXX"
            r'appId\s*[:=]\s*["\'](\d{9,})["\']',
        ]

        for pattern in patterns:
            match = re.search(pattern, js_content)
            if match:
                self.app_id = match.group(1)
                # First pattern also has the secret directly
                if match.lastindex and match.lastindex >= 2:
                    candidate = match.group(2)
                    if len(candidate) > 20:
                        self.app_secret = candidate
                return

    def _extract_app_secret(self, js_content):
        """Extract app_secret from seed/timezone pairs in JS bundles.

        The Qobuz web player splits the app_secret into multiple seed strings,
        each associated with a timezone. The seeds must be sorted by timezone
        offset and concatenated to recover the full secret.
        """
        if self.app_secret and len(self.app_secret) > 20:
            return  # Already have it from direct extraction

        # Pattern 1: initialSeed("seedvalue", window.utimezone.timezone_name)
        seed_patterns = [
            r'[a-z]\.initialSeed\(\s*"([\w=+/]+)"\s*,\s*window\.utimezone\.(\w+)\s*\)',
            r'initialSeed\(\s*"([\w=+/]+)"\s*,\s*window\.utimezone\.(\w+)\s*\)',
        ]

        seeds = []
        for pattern in seed_patterns:
            found = re.findall(pattern, js_content)
            if found:
                seeds = found
                break

        if not seeds:
            logger.warning("No seed patterns found in JS bundles")
            return

        logger.info(f"Found {len(seeds)} seed/timezone pairs")

        # Find timezone-to-offset mapping
        # Pattern: {timezone_name: offset, ...} or "timezone_name": offset
        tz_patterns = [
            r'utimezone\s*[=:]\s*\{([^}]+)\}',
            r'exports\s*=\s*\{([^}]+)\}',
        ]

        timezones = {}
        for pattern in tz_patterns:
            tz_matches = re.findall(pattern, js_content)
            for tz_block in tz_matches:
                pairs = re.findall(r'["\']?(\w+)["\']?\s*:\s*(-?\d+)', tz_block)
                if pairs:
                    for name, offset in pairs:
                        timezones[name] = int(offset)
                    if len(timezones) >= len(seeds):
                        break
            if timezones:
                break

        if not timezones:
            # Broader search for timezone values
            all_pairs = re.findall(
                r'["\'](\w+)["\']:\s*(-?\d+)', js_content
            )
            for name, offset in all_pairs:
                timezones[name] = int(offset)

        if not timezones:
            logger.warning("No timezone mappings found")
            return

        # Match seeds to timezone offsets and sort
        seed_offset_pairs = []
        for seed_val, tz_name in seeds:
            if tz_name in timezones:
                seed_offset_pairs.append((seed_val, timezones[tz_name]))
            else:
                logger.warning(f"Timezone '{tz_name}' not found in mappings")

        if not seed_offset_pairs:
            logger.warning("Could not match any seeds to timezone offsets")
            return

        # Sort by timezone offset and concatenate seeds
        seed_offset_pairs.sort(key=lambda x: x[1])
        self.app_secret = "".join(s for s, _ in seed_offset_pairs)
        logger.info(
            f"Assembled app_secret from {len(seed_offset_pairs)} seeds "
            f"(length: {len(self.app_secret)})"
        )

    def get_app_id(self):
        """Extract app_id (and app_secret) from Qobuz web player."""
        return self.get_app_id_and_secret()

    # ------------------------------------------------------------------
    # Authentication
    # ------------------------------------------------------------------

    def login(self, email=None, password=None, token=None, app_id=None):
        """Login to Qobuz using email/password or auth token."""
        if app_id:
            self.app_id = app_id

        if not self.app_id:
            self.get_app_id_and_secret()

        if token:
            self.user_auth_token = token
            data = self._get("user/get", params={
                "user_auth_token": token,
            })
            user_data = data.get("user", data)
            self.user_id = str(user_data.get("id", ""))
            self.user_display_name = user_data.get("display_name", "")
            try:
                self.user_label = user_data["credential"]["parameters"]["short_label"]
            except (KeyError, TypeError):
                self.user_label = ""
            try:
                avatar = user_data.get("avatar", "")
                if avatar:
                    self.user_avatar = avatar.replace("s=50", "s=20")
            except (KeyError, TypeError):
                self.user_avatar = ""
        else:
            params = {
                "email": email,
                "password": password,
                "app_id": self.app_id,
            }
            data = self._get("user/login", params=params)
            self.user_auth_token = data.get("user_auth_token", "")
            user_info = data.get("user", {})
            self.user_id = str(user_info.get("id", ""))
            self.user_display_name = user_info.get("display_name", "")
            try:
                self.user_label = user_info["credential"]["parameters"]["short_label"]
            except (KeyError, TypeError):
                self.user_label = ""
            try:
                avatar = user_info.get("avatar", "")
                if avatar:
                    self.user_avatar = avatar.replace("s=50", "s=20")
            except (KeyError, TypeError):
                self.user_avatar = ""

        # If we don't have app_secret yet, try to get it now
        if not self.app_secret:
            try:
                self.get_app_id_and_secret()
            except Exception as e:
                logger.warning(f"Could not extract app_secret after login: {e}")

        return {
            "user_id": self.user_id,
            "display_name": self.user_display_name,
            "label": self.user_label,
            "avatar": self.user_avatar,
            "auth_token": self.user_auth_token,
        }

    def get_app_secret(self):
        """Get app_secret, extracting from JS bundles if needed.

        The original C# app calls QoService.GetAppSecret(app_id, user_auth_token)
        which internally extracts secrets from the Qobuz web player JS bundles
        and validates them by testing against the API.
        """
        if self.app_secret:
            return self.app_secret

        # Re-extract from JS bundles
        self.get_app_id_and_secret()

        if not self.app_secret:
            raise ValueError(
                "Could not determine app_secret. "
                "You may need to provide it manually."
            )
        return self.app_secret

    # ------------------------------------------------------------------
    # Stream URL (the critical part that was failing)
    # ------------------------------------------------------------------

    def get_stream_url(self, track_id, format_id="27"):
        """Get the streaming URL for a track.

        Reimplements the C# createURL method from Form1.cs.
        Key difference from other API calls: ALL parameters including
        app_id and user_auth_token are passed as QUERY PARAMETERS,
        not as HTTP headers.

        Signature string format (Form1.cs line 363-364):
            "trackgetFileUrlformat_id" + formatIdString
            + "intentstreamtrack_id" + trackIdString + time + appSecret

        URL format (Form1.cs line 374):
            /track/getFileUrl?request_ts=...&request_sig=...
            &track_id=...&format_id=...&intent=stream
            &app_id=...&user_auth_token=...
        """
        if not self.app_secret:
            raise ValueError("app_secret is required to get stream URLs")

        unix_ts = str(int(time.time()))

        # Build the signature string exactly as in Form1.cs
        sig_string = (
            "trackgetFileUrlformat_id" + str(format_id)
            + "intentstreamtrack_id" + str(track_id)
            + unix_ts + self.app_secret
        )
        request_sig = hashlib.md5(sig_string.encode("utf-8")).hexdigest()

        # ALL parameters as query string - matching Form1.cs line 374
        # Do NOT use _get() here since it sends auth via headers
        params = {
            "request_ts": unix_ts,
            "request_sig": request_sig,
            "track_id": str(track_id),
            "format_id": str(format_id),
            "intent": "stream",
            "app_id": self.app_id,
            "user_auth_token": self.user_auth_token,
        }

        logger.debug(
            f"getFileUrl: track_id={track_id}, format_id={format_id}, "
            f"ts={unix_ts}, sig={request_sig}"
        )

        resp = self.session.get(
            f"{QOBUZ_BASE_URL}/track/getFileUrl",
            params=params,
            timeout=30,
        )
        resp.raise_for_status()
        return resp.json()

    def test_secret(self, track_id="5966783"):
        """Validate the current app_secret by attempting a getFileUrl call.

        Returns True if the secret works, False otherwise.
        Similar to what the QopenAPI DLL does internally in GetAppSecret.
        """
        if not self.app_secret or not self.app_id or not self.user_auth_token:
            return False

        try:
            result = self.get_stream_url(track_id, "5")  # MP3 320 for testing
            return bool(result.get("url"))
        except Exception as e:
            logger.debug(f"Secret test failed: {e}")
            return False

    # ------------------------------------------------------------------
    # Search & Browse
    # ------------------------------------------------------------------

    def search(self, query, search_type="albums", limit=50, offset=0):
        """Search Qobuz for albums, tracks, or artists."""
        if search_type not in ("albums", "tracks", "artists", "playlists"):
            search_type = "albums"

        # Qobuz search endpoint: {type_singular}/search
        data = self._get(f"{search_type.rstrip('s')}/search", params={
            "query": query,
            "limit": limit,
            "offset": offset,
        })
        return data

    def get_album(self, album_id, limit=500, offset=0):
        """Get album info with all tracks (handles pagination)."""
        data = self._get("album/get", params={
            "album_id": album_id,
            "limit": limit,
            "offset": offset,
        })

        if data.get("tracks") and data["tracks"].get("items"):
            all_items = list(data["tracks"]["items"])
            total = data["tracks"].get("total", len(all_items))
            current_offset = offset + limit

            while len(all_items) < total:
                page = self._get("album/get", params={
                    "album_id": album_id,
                    "limit": limit,
                    "offset": current_offset,
                })
                page_items = page.get("tracks", {}).get("items", [])
                if not page_items:
                    break
                all_items.extend(page_items)
                current_offset += limit
                if current_offset > 100000:
                    break

            data["tracks"]["items"] = all_items

        return data

    def get_track(self, track_id):
        """Get track info."""
        return self._get("track/get", params={"track_id": track_id})

    def get_artist(self, artist_id, limit=500, offset=0):
        """Get artist info with albums (handles pagination)."""
        data = self._get("artist/get", params={
            "artist_id": artist_id,
            "extra": "albums",
            "limit": limit,
            "offset": offset,
        })

        if data.get("albums") and data["albums"].get("items"):
            all_items = list(data["albums"]["items"])
            total = data["albums"].get("total", len(all_items))
            current_offset = offset + limit

            while len(all_items) < total:
                page = self._get("artist/get", params={
                    "artist_id": artist_id,
                    "extra": "albums",
                    "limit": limit,
                    "offset": current_offset,
                })
                page_items = page.get("albums", {}).get("items", [])
                if not page_items:
                    break
                all_items.extend(page_items)
                current_offset += limit
                if current_offset > 100000:
                    break

            data["albums"]["items"] = all_items

        return data

    def get_playlist(self, playlist_id, limit=500, offset=0):
        """Get playlist info with tracks (handles pagination)."""
        data = self._get("playlist/get", params={
            "playlist_id": playlist_id,
            "extra": "tracks",
            "limit": limit,
            "offset": offset,
        })

        if data.get("tracks") and data["tracks"].get("items"):
            all_items = list(data["tracks"]["items"])
            total = data["tracks"].get("total", len(all_items))
            current_offset = offset + limit

            while len(all_items) < total:
                page = self._get("playlist/get", params={
                    "playlist_id": playlist_id,
                    "extra": "tracks",
                    "limit": limit,
                    "offset": current_offset,
                })
                page_items = page.get("tracks", {}).get("items", [])
                if not page_items:
                    break
                all_items.extend(page_items)
                current_offset += limit
                if current_offset > 100000:
                    break

            data["tracks"]["items"] = all_items

        return data

    def get_favorites(self, fav_type="albums", limit=500, offset=0):
        """Get user favorites."""
        return self._get("favorite/getUserFavorites", params={
            "user_id": self.user_id,
            "type": fav_type,
            "limit": limit,
            "offset": offset,
        })

    # ------------------------------------------------------------------
    # Session helpers
    # ------------------------------------------------------------------

    def is_logged_in(self):
        return bool(self.user_auth_token and self.app_id)

    def get_session_info(self):
        if not self.is_logged_in():
            return None
        return {
            "user_id": self.user_id,
            "display_name": self.user_display_name,
            "label": self.user_label,
            "avatar": self.user_avatar,
            "app_id": self.app_id,
            "has_secret": bool(self.app_secret),
        }
