"""Qobuz API client - Python reimplementation of QopenAPI logic."""

import hashlib
import time
import re
import requests

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

    def get_app_id(self):
        """Extract app_id from Qobuz web player JavaScript bundle."""
        try:
            resp = self.session.get("https://play.qobuz.com/login", timeout=15)
            resp.raise_for_status()
            bundle_re = re.compile(
                r'<script\s+src="(/resources/\d+\.\d+\.\d+-[a-z]\d+/bundle\.js)"'
            )
            match = bundle_re.search(resp.text)
            if not match:
                raise ValueError("Could not find bundle.js URL in Qobuz login page")

            bundle_url = "https://play.qobuz.com" + match.group(1)
            resp = self.session.get(bundle_url, timeout=30)
            resp.raise_for_status()
            bundle_js = resp.text

            app_id_re = re.compile(r'production:\{api:\{appId:"(\d+)",appSecret:"(\w+)"')
            m = app_id_re.search(bundle_js)
            if m:
                self.app_id = m.group(1)
                self.app_secret = m.group(2)
                return self.app_id

            app_id_re2 = re.compile(r'"app_id"\s*:\s*"(\d+)"')
            m2 = app_id_re2.search(bundle_js)
            if m2:
                self.app_id = m2.group(1)

            seed_re = re.compile(
                r'[a-z]\.initialSeed\("([\w=]+)",window\.utimezone\.(.+?)\)'
            )
            seeds = seed_re.findall(bundle_js)
            timezone_re = re.compile(
                r'["\'](\w+)["\']:\s*(-?\d+)'
            )
            timezones = dict(timezone_re.findall(bundle_js))

            if seeds and timezones:
                info_extras = []
                for seed, tz_key in seeds:
                    if tz_key in timezones:
                        info_extras.append((seed, timezones[tz_key]))
                if info_extras:
                    secret_parts = []
                    for seed, tz_val in sorted(info_extras, key=lambda x: x[1]):
                        secret_parts.append(seed)
                    self.app_secret = "".join(secret_parts)

            if not self.app_id:
                raise ValueError("Could not extract app_id from Qobuz bundle")
            return self.app_id

        except Exception:
            raise

    def login(self, email=None, password=None, token=None, app_id=None):
        """Login to Qobuz using email/password or auth token."""
        if app_id:
            self.app_id = app_id

        if not self.app_id:
            self.get_app_id()

        if token:
            self.user_auth_token = token
            data = self._get("user/get", params={
                "user_auth_token": token,
            })
            self.user_id = str(data.get("user", data).get("id", ""))
            self.user_display_name = data.get("user", data).get("display_name", "")
            try:
                self.user_label = data.get("user", data)["credential"]["parameters"]["short_label"]
            except (KeyError, TypeError):
                self.user_label = ""
            try:
                avatar = data.get("user", data).get("avatar", "")
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

        return {
            "user_id": self.user_id,
            "display_name": self.user_display_name,
            "label": self.user_label,
            "avatar": self.user_avatar,
            "auth_token": self.user_auth_token,
        }

    def get_app_secret(self):
        """Get app_secret if not already extracted."""
        if self.app_secret:
            return self.app_secret

        if not self.app_id or not self.user_auth_token:
            raise ValueError("Must be logged in to get app_secret")

        try:
            self._get("track/get", params={
                "track_id": "1",
            })
        except Exception:
            pass

        if not self.app_secret:
            raise ValueError("Could not determine app_secret")
        return self.app_secret

    def search(self, query, search_type="albums", limit=50, offset=0):
        """Search Qobuz for albums or tracks."""
        if search_type not in ("albums", "tracks", "artists", "playlists"):
            search_type = "albums"

        data = self._get(f"{search_type.rstrip('s')}/search", params={
            "query": query,
            "limit": limit,
            "offset": offset,
        })
        return data

    def get_album(self, album_id, limit=500, offset=0):
        """Get album info with tracks."""
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
        data = self._get("track/get", params={
            "track_id": track_id,
        })
        return data

    def get_artist(self, artist_id, limit=500, offset=0):
        """Get artist info with albums."""
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
        """Get playlist info with tracks."""
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
        data = self._get("favorite/getUserFavorites", params={
            "user_id": self.user_id,
            "type": fav_type,
            "limit": limit,
            "offset": offset,
        })
        return data

    def get_stream_url(self, track_id, format_id="27"):
        """Get the streaming URL for a track."""
        if not self.app_secret:
            raise ValueError("app_secret is required to get stream URLs")

        unix_ts = str(int(time.time()))
        r_sig = f"trackgetFileUrlformat_id{format_id}intentstreamtrack_id{track_id}{unix_ts}{self.app_secret}"
        r_sig_hash = hashlib.md5(r_sig.encode("utf-8")).hexdigest()

        data = self._get("track/getFileUrl", params={
            "track_id": track_id,
            "format_id": format_id,
            "intent": "stream",
            "request_ts": unix_ts,
            "request_sig": r_sig_hash,
        })
        return data

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
        }
