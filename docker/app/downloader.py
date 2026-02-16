"""Download manager - handles downloading and tagging of audio files."""

import hashlib
import logging
import os
import re
import threading
import time
import uuid
from datetime import datetime

import mutagen
from mutagen.flac import FLAC, Picture
from mutagen.id3 import (APIC, TALB, TBPM, TCOM, TCON, TCOP, TDRL, TIT2,
                          TPE1, TPE2, TPOS, TPUB, TRCK, TSRC, TXXX, ID3)
from mutagen.mp3 import MP3
import requests

from .config import DOWNLOAD_DIR, load_config

logger = logging.getLogger(__name__)

INVALID_CHARS_RE = re.compile(r'[<>:"/\\|?*]')
MULTI_SPACES_RE = re.compile(r'\s+')
PERCENT_RE = re.compile(r'%(.*?)%')


class DownloadManager:
    def __init__(self, qobuz_api):
        self.api = qobuz_api
        self.active_downloads = {}
        self.download_history = []
        self._lock = threading.Lock()

    def get_status(self):
        with self._lock:
            active = []
            for did, info in self.active_downloads.items():
                active.append({
                    "id": did,
                    "title": info.get("title", ""),
                    "artist": info.get("artist", ""),
                    "type": info.get("type", ""),
                    "progress": info.get("progress", 0),
                    "status": info.get("status", "pending"),
                    "current_track": info.get("current_track", ""),
                    "total_tracks": info.get("total_tracks", 0),
                    "completed_tracks": info.get("completed_tracks", 0),
                    "cover": info.get("cover", ""),
                })
            history = list(self.download_history[-50:])
        return {"active": active, "history": history}

    def download_album(self, album_id):
        download_id = str(uuid.uuid4())[:8]
        thread = threading.Thread(
            target=self._download_album_worker,
            args=(download_id, album_id),
            daemon=True,
        )
        thread.start()
        return download_id

    def download_track(self, track_id):
        download_id = str(uuid.uuid4())[:8]
        thread = threading.Thread(
            target=self._download_track_worker,
            args=(download_id, track_id),
            daemon=True,
        )
        thread.start()
        return download_id

    def download_playlist(self, playlist_id):
        download_id = str(uuid.uuid4())[:8]
        thread = threading.Thread(
            target=self._download_playlist_worker,
            args=(download_id, playlist_id),
            daemon=True,
        )
        thread.start()
        return download_id

    def _update_download(self, download_id, **kwargs):
        with self._lock:
            if download_id in self.active_downloads:
                self.active_downloads[download_id].update(kwargs)

    def _finish_download(self, download_id, status="completed"):
        with self._lock:
            if download_id in self.active_downloads:
                info = self.active_downloads.pop(download_id)
                info["status"] = status
                info["finished_at"] = datetime.now().isoformat()
                self.download_history.append({
                    "id": download_id,
                    "title": info.get("title", ""),
                    "artist": info.get("artist", ""),
                    "type": info.get("type", ""),
                    "status": status,
                    "finished_at": info["finished_at"],
                    "cover": info.get("cover", ""),
                })

    def _download_album_worker(self, download_id, album_id):
        config = load_config()
        try:
            album = self.api.get_album(album_id)
            artist_name = album.get("artist", {}).get("name", "Unknown Artist")
            album_title = album.get("title", "Unknown Album")
            version = album.get("version")
            if version:
                album_title = f"{album_title.rstrip()} ({version})"
            cover_url = album.get("image", {}).get("large", "")

            tracks = album.get("tracks", {}).get("items", [])
            total_tracks = len(tracks)

            with self._lock:
                self.active_downloads[download_id] = {
                    "title": album_title,
                    "artist": artist_name,
                    "type": "album",
                    "progress": 0,
                    "status": "downloading",
                    "current_track": "",
                    "total_tracks": total_tracks,
                    "completed_tracks": 0,
                    "cover": cover_url,
                }

            download_path = self._build_album_path(config, album)
            os.makedirs(download_path, exist_ok=True)

            self._download_artwork(download_path, album, config)

            for idx, track in enumerate(tracks):
                track_id = str(track.get("id", ""))
                track_title = track.get("title", f"Track {idx + 1}")
                track_version = track.get("version")
                if track_version:
                    track_title = f"{track_title.rstrip()} ({track_version})"

                self._update_download(
                    download_id,
                    current_track=track_title,
                    completed_tracks=idx,
                    progress=int(idx / total_tracks * 100) if total_tracks else 0,
                )

                try:
                    full_track = self.api.get_track(track_id)
                    self._download_single_track(
                        config, album, full_track, download_path
                    )
                except Exception as e:
                    logger.error(f"Failed to download track {track_id}: {e}")

            self._update_download(
                download_id,
                progress=100,
                completed_tracks=total_tracks,
            )
            self._finish_download(download_id, "completed")

        except Exception as e:
            logger.error(f"Album download {download_id} failed: {e}")
            self._update_download(download_id, status="error")
            self._finish_download(download_id, "error")

    def _download_track_worker(self, download_id, track_id):
        config = load_config()
        try:
            track = self.api.get_track(track_id)
            album_data = track.get("album", {})
            album_id = album_data.get("id", "")

            album = self.api.get_album(album_id) if album_id else album_data

            artist_name = track.get("performer", {}).get("name", "Unknown Artist")
            track_title = track.get("title", "Unknown Track")
            version = track.get("version")
            if version:
                track_title = f"{track_title.rstrip()} ({version})"
            cover_url = album.get("image", {}).get("large", "")

            with self._lock:
                self.active_downloads[download_id] = {
                    "title": track_title,
                    "artist": artist_name,
                    "type": "track",
                    "progress": 0,
                    "status": "downloading",
                    "current_track": track_title,
                    "total_tracks": 1,
                    "completed_tracks": 0,
                    "cover": cover_url,
                }

            download_path = self._build_album_path(config, album)
            os.makedirs(download_path, exist_ok=True)

            self._download_artwork(download_path, album, config)
            self._download_single_track(config, album, track, download_path)

            self._update_download(download_id, progress=100, completed_tracks=1)
            self._finish_download(download_id, "completed")

        except Exception as e:
            logger.error(f"Track download {download_id} failed: {e}")
            self._update_download(download_id, status="error")
            self._finish_download(download_id, "error")

    def _download_playlist_worker(self, download_id, playlist_id):
        config = load_config()
        try:
            playlist = self.api.get_playlist(playlist_id)
            playlist_name = playlist.get("name", "Unknown Playlist")
            tracks = playlist.get("tracks", {}).get("items", [])
            total_tracks = len(tracks)

            with self._lock:
                self.active_downloads[download_id] = {
                    "title": playlist_name,
                    "artist": "",
                    "type": "playlist",
                    "progress": 0,
                    "status": "downloading",
                    "current_track": "",
                    "total_tracks": total_tracks,
                    "completed_tracks": 0,
                    "cover": "",
                }

            safe_name = _safe_filename(playlist_name)
            download_path = os.path.join(DOWNLOAD_DIR, safe_name)
            os.makedirs(download_path, exist_ok=True)

            for idx, track in enumerate(tracks):
                track_id = str(track.get("id", ""))
                track_title = track.get("title", f"Track {idx + 1}")
                track_version = track.get("version")
                if track_version:
                    track_title = f"{track_title.rstrip()} ({track_version})"

                self._update_download(
                    download_id,
                    current_track=track_title,
                    completed_tracks=idx,
                    progress=int(idx / total_tracks * 100) if total_tracks else 0,
                )

                try:
                    full_track = self.api.get_track(track_id)
                    album_data = full_track.get("album", {})
                    album_id = album_data.get("id", "")
                    album = self.api.get_album(album_id) if album_id else album_data

                    self._download_single_track(
                        config, album, full_track, download_path,
                        track_num_override=idx + 1,
                        padded_length=len(str(total_tracks)),
                    )
                except Exception as e:
                    logger.error(f"Failed to download playlist track {track_id}: {e}")

            self._update_download(
                download_id,
                progress=100,
                completed_tracks=total_tracks,
            )
            self._finish_download(download_id, "completed")

        except Exception as e:
            logger.error(f"Playlist download {download_id} failed: {e}")
            self._update_download(download_id, status="error")
            self._finish_download(download_id, "error")

    def _build_album_path(self, config, album):
        artist_name = album.get("artist", {}).get("name", "Unknown Artist")
        album_title = album.get("title", "Unknown Album")
        version = album.get("version")
        if version:
            album_title = f"{album_title.rstrip()} ({version})"

        audio_format = config.get("audio_format", ".flac").upper().lstrip(".")

        artist_template = config.get("artist_template", "%artistname%")
        album_template = config.get("album_template", "%albumtitle% [%format%]")

        artist_dir = _apply_template(artist_template, album=album, format_str=audio_format)
        album_dir = _apply_template(album_template, album=album, format_str=audio_format)

        return os.path.join(DOWNLOAD_DIR, _safe_filename(artist_dir), _safe_filename(album_dir))

    def _download_artwork(self, download_path, album, config):
        if not config.get("save_artwork", True):
            return
        cover_path = os.path.join(download_path, "Cover.jpg")
        if os.path.exists(cover_path):
            return
        try:
            art_size = config.get("artwork_size", "600")
            image_url = album.get("image", {}).get("large", "")
            if not image_url:
                return
            image_url = image_url.replace("_600", f"_{art_size}")
            resp = requests.get(image_url, timeout=60)
            resp.raise_for_status()
            with open(cover_path, "wb") as f:
                f.write(resp.content)
        except Exception as e:
            logger.error(f"Artwork download failed: {e}")

    def _download_single_track(self, config, album, track, download_path,
                               track_num_override=None, padded_length=None):
        track_id = str(track.get("id", ""))
        format_id = config.get("quality", "27")
        audio_format = config.get("audio_format", ".flac")

        streamable = track.get("streamable", True)
        if not streamable and config.get("streamable_check", True):
            logger.warning(f"Track {track_id} is not streamable, skipping")
            return

        try:
            stream_data = self.api.get_stream_url(track_id, format_id)
        except Exception as e:
            logger.error(f"Failed to get stream URL for track {track_id}: {e}")
            return

        stream_url = stream_data.get("url")
        if not stream_url:
            logger.error(f"No stream URL for track {track_id}")
            return

        actual_format_id = str(stream_data.get("format_id", format_id))
        if actual_format_id in ("5",):
            audio_format = ".mp3"
        elif actual_format_id in ("6", "7", "27"):
            audio_format = ".flac"

        track_num = track_num_override or track.get("track_number", 1)
        total_tracks = album.get("tracks_count", 0)
        if padded_length is None:
            padded_length = len(str(total_tracks)) if total_tracks else 2

        track_template = config.get("track_template", "%tracknumber%. %tracktitle%")
        filename = _apply_template(
            track_template, album=album, track=track,
            format_str=audio_format.upper().lstrip("."),
            padded_length=padded_length,
            track_num_override=track_num_override,
        )
        filename = _safe_filename(filename) + audio_format
        file_path = os.path.join(download_path, filename)

        if os.path.exists(file_path):
            logger.info(f"File already exists, skipping: {file_path}")
            return

        temp_path = file_path + ".tmp"

        try:
            resp = requests.get(stream_url, stream=True, timeout=600)
            resp.raise_for_status()

            total_size = int(resp.headers.get("content-length", 0))
            downloaded = 0

            with open(temp_path, "wb") as f:
                for chunk in resp.iter_content(chunk_size=81920):
                    if chunk:
                        f.write(chunk)
                        downloaded += len(chunk)

            self._tag_file(config, temp_path, album, track, audio_format)

            os.makedirs(os.path.dirname(file_path), exist_ok=True)
            os.rename(temp_path, file_path)
            logger.info(f"Downloaded: {file_path}")

        except Exception as e:
            logger.error(f"Download failed for {track_id}: {e}")
            if os.path.exists(temp_path):
                try:
                    os.remove(temp_path)
                except OSError:
                    pass
            raise

    def _tag_file(self, config, file_path, album, track, audio_format):
        """Tag the downloaded audio file with metadata."""
        try:
            if audio_format == ".flac":
                self._tag_flac(config, file_path, album, track)
            elif audio_format == ".mp3":
                self._tag_mp3(config, file_path, album, track)
        except Exception as e:
            logger.error(f"Tagging failed for {file_path}: {e}")

    def _tag_flac(self, config, file_path, album, track):
        audio = FLAC(file_path)

        if config.get("tag_track_title"):
            title = _format_title(track)
            audio["TITLE"] = title

        if config.get("tag_artist"):
            audio["ARTIST"] = [track.get("performer", {}).get("name", "")]

        if config.get("tag_album"):
            album_title = album.get("title", "")
            version = album.get("version")
            if version:
                album_title = f"{album_title.rstrip()} ({version})"
            audio["ALBUM"] = [album_title]

        if config.get("tag_album_artist"):
            audio["ALBUMARTIST"] = [album.get("artist", {}).get("name", "")]

        if config.get("tag_track_number"):
            audio["TRACKNUMBER"] = [str(track.get("track_number", ""))]

        if config.get("tag_total_tracks"):
            audio["TRACKTOTAL"] = [str(album.get("tracks_count", ""))]

        if config.get("tag_disc"):
            audio["DISCNUMBER"] = [str(track.get("media_number", 1))]

        if config.get("tag_total_discs"):
            audio["DISCTOTAL"] = [str(album.get("media_count", 1))]

        if config.get("tag_genre"):
            genre = album.get("genre", {}).get("name", "")
            if genre:
                audio["GENRE"] = [genre]

        if config.get("tag_year"):
            release_date = track.get("release_date_original") or album.get("release_date_original", "")
            if release_date and len(release_date) >= 4:
                audio["YEAR"] = [release_date[:4]]

        if config.get("tag_release_date"):
            release_date = track.get("release_date_original") or album.get("release_date_original", "")
            if release_date:
                audio["DATE"] = [release_date.strip()]

        if config.get("tag_composer"):
            composer = track.get("composer", {})
            if composer and composer.get("name"):
                audio["COMPOSER"] = [composer["name"]]

        if config.get("tag_copyright"):
            copyright_text = album.get("copyright", "")
            if copyright_text:
                audio["COPYRIGHT"] = [copyright_text]

        if config.get("tag_isrc"):
            isrc = track.get("isrc", "")
            if isrc:
                audio["ISRC"] = [isrc]

        if config.get("tag_upc"):
            upc = album.get("upc", "")
            if upc:
                audio["BARCODE"] = [upc]

        if config.get("tag_label"):
            label = album.get("label", {}).get("name", "")
            if label:
                audio["LABEL"] = [MULTI_SPACES_RE.sub(" ", label)]

        if config.get("tag_explicit"):
            audio["ITUNESADVISORY"] = ["1" if track.get("parental_warning", False) else "0"]

        if config.get("tag_image"):
            self._embed_artwork_flac(audio, album, config)

        audio.save()

    def _tag_mp3(self, config, file_path, album, track):
        try:
            audio = MP3(file_path, ID3=ID3)
        except mutagen.MutagenError:
            audio = MP3(file_path)
        if audio.tags is None:
            audio.add_tags()

        if config.get("tag_track_title"):
            audio.tags.add(TIT2(encoding=3, text=[_format_title(track)]))

        if config.get("tag_artist"):
            audio.tags.add(TPE1(encoding=3, text=[track.get("performer", {}).get("name", "")]))

        if config.get("tag_album"):
            album_title = album.get("title", "")
            version = album.get("version")
            if version:
                album_title = f"{album_title.rstrip()} ({version})"
            audio.tags.add(TALB(encoding=3, text=[album_title]))

        if config.get("tag_album_artist"):
            audio.tags.add(TPE2(encoding=3, text=[album.get("artist", {}).get("name", "")]))

        if config.get("tag_track_number"):
            total = album.get("tracks_count", 0)
            tn = track.get("track_number", 1)
            audio.tags.add(TRCK(encoding=3, text=[f"{tn}/{total}" if total else str(tn)]))

        if config.get("tag_disc"):
            mn = track.get("media_number", 1)
            mc = album.get("media_count", 1)
            audio.tags.add(TPOS(encoding=3, text=[f"{mn}/{mc}"]))

        if config.get("tag_genre"):
            genre = album.get("genre", {}).get("name", "")
            if genre:
                audio.tags.add(TCON(encoding=3, text=[genre]))

        if config.get("tag_release_date"):
            release_date = track.get("release_date_original") or album.get("release_date_original", "")
            if release_date:
                audio.tags.add(TDRL(encoding=3, text=[release_date.strip()]))

        if config.get("tag_composer"):
            composer = track.get("composer", {})
            if composer and composer.get("name"):
                audio.tags.add(TCOM(encoding=3, text=[composer["name"]]))

        if config.get("tag_copyright"):
            copyright_text = album.get("copyright", "")
            if copyright_text:
                audio.tags.add(TCOP(encoding=3, text=[copyright_text]))

        if config.get("tag_isrc"):
            isrc = track.get("isrc", "")
            if isrc:
                audio.tags.add(TSRC(encoding=3, text=[isrc]))

        if config.get("tag_upc"):
            upc = album.get("upc", "")
            if upc:
                audio.tags.add(TXXX(encoding=3, desc="BARCODE", text=[upc]))

        if config.get("tag_label"):
            label = album.get("label", {}).get("name", "")
            if label:
                audio.tags.add(TPUB(encoding=3, text=[MULTI_SPACES_RE.sub(" ", label)]))

        if config.get("tag_explicit"):
            audio.tags.add(TXXX(
                encoding=3, desc="ITUNESADVISORY",
                text=["1" if track.get("parental_warning", False) else "0"],
            ))

        if config.get("tag_image"):
            self._embed_artwork_mp3(audio, album, config)

        audio.save()

    def _embed_artwork_flac(self, audio, album, config):
        try:
            art_size = config.get("embedded_art_size", "600")
            image_url = album.get("image", {}).get("large", "")
            if not image_url:
                return
            image_url = image_url.replace("_600", f"_{art_size}")
            resp = requests.get(image_url, timeout=60)
            resp.raise_for_status()

            pic = Picture()
            pic.type = 3  # Front cover
            pic.mime = "image/jpeg"
            pic.data = resp.content
            audio.clear_pictures()
            audio.add_picture(pic)
        except Exception as e:
            logger.error(f"Failed to embed artwork (FLAC): {e}")

    def _embed_artwork_mp3(self, audio, album, config):
        try:
            art_size = config.get("embedded_art_size", "600")
            image_url = album.get("image", {}).get("large", "")
            if not image_url:
                return
            image_url = image_url.replace("_600", f"_{art_size}")
            resp = requests.get(image_url, timeout=60)
            resp.raise_for_status()

            audio.tags.add(APIC(
                encoding=3,
                mime="image/jpeg",
                type=3,
                desc="Front Cover",
                data=resp.content,
            ))
        except Exception as e:
            logger.error(f"Failed to embed artwork (MP3): {e}")


def _format_title(track):
    title = track.get("title", "")
    version = track.get("version")
    if version:
        title = f"{title.rstrip()} ({version})"
    return title


def _safe_filename(name):
    if not name:
        return "Unknown"
    name = INVALID_CHARS_RE.sub("_", name)
    name = name.strip(". ")
    name = MULTI_SPACES_RE.sub(" ", name)
    if len(name) > 200:
        name = name[:200]
    return name or "Unknown"


def _apply_template(template, album=None, track=None, format_str="FLAC",
                    padded_length=2, track_num_override=None):
    template = PERCENT_RE.sub(lambda m: m.group(0).lower(), template)

    if album:
        artist_name = album.get("artist", {}).get("name", "Unknown Artist")
        album_title = album.get("title", "Unknown Album")
        version = album.get("version")
        if version:
            album_title = f"{album_title.rstrip()} ({version})"

        template = template.replace("%artistname%", artist_name)
        template = template.replace("%albumtitle%", album_title)
        template = template.replace("%albumid%", str(album.get("id", "")))
        template = template.replace("%albumgenre%", album.get("genre", {}).get("name", ""))
        template = template.replace("%label%", MULTI_SPACES_RE.sub(" ", album.get("label", {}).get("name", "")))
        template = template.replace("%upc%", album.get("upc", ""))
        template = template.replace("%copyright%", album.get("copyright", ""))
        template = template.replace("%format%", format_str)
        template = template.replace("%bitdepth%", str(album.get("maximum_bit_depth", "")))
        template = template.replace("%samplerate%", str(album.get("maximum_sampling_rate", "")))

        release_date = album.get("release_date_original", "")
        template = template.replace("%releasedate%", release_date.strip() if release_date else "")
        if release_date and len(release_date) >= 4:
            template = template.replace("%year%", release_date[:4])
        else:
            template = template.replace("%year%", "")

        product_type = album.get("product_type", "album")
        if product_type:
            template = template.replace("%releasetype%", product_type.capitalize())

    if track:
        track_num = track_num_override or track.get("track_number", 1)
        track_title = track.get("title", "")
        track_version = track.get("version")
        if track_version:
            track_title = f"{track_title.rstrip()} ({track_version})"

        template = template.replace("%tracknumber%", str(track_num).zfill(padded_length))
        template = template.replace("%tracktitle%", track_title)
        template = template.replace("%trackartist%", track.get("performer", {}).get("name", ""))
        template = template.replace("%trackid%", str(track.get("id", "")))
        template = template.replace("%isrc%", track.get("isrc", ""))
        template = template.replace("%trackcomposer%", track.get("composer", {}).get("name", "") if track.get("composer") else "")
        template = template.replace("%trackbitdepth%", str(track.get("maximum_bit_depth", "")))
        template = template.replace("%tracksamplerate%", str(track.get("maximum_sampling_rate", "")))

        if not album:
            template = template.replace("%artistname%", track.get("performer", {}).get("name", ""))

    return template
