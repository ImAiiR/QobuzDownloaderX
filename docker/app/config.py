import os
import json

CONFIG_DIR = os.environ.get("QBDLX_CONFIG_DIR", "/config")
CONFIG_FILE = os.path.join(CONFIG_DIR, "settings.json")
DOWNLOAD_DIR = os.environ.get("QBDLX_DOWNLOAD_DIR", "/downloads")

DEFAULT_CONFIG = {
    "quality": "27",
    "audio_format": ".flac",
    "artist_template": "%artistname%",
    "album_template": "%albumtitle% [%format%]",
    "track_template": "%tracknumber%. %tracktitle%",
    "playlist_template": "%playlisttitle%",
    "embed_artwork": True,
    "save_artwork": True,
    "artwork_size": "600",
    "embedded_art_size": "600",
    "tag_album": True,
    "tag_artist": True,
    "tag_album_artist": True,
    "tag_track_title": True,
    "tag_track_number": True,
    "tag_disc": True,
    "tag_genre": True,
    "tag_year": True,
    "tag_release_date": True,
    "tag_composer": True,
    "tag_copyright": True,
    "tag_isrc": True,
    "tag_upc": True,
    "tag_label": True,
    "tag_image": True,
    "tag_total_tracks": True,
    "tag_total_discs": True,
    "tag_explicit": True,
    "fix_md5": True,
    "streamable_check": True,
    "download_goodies": False,
}


def load_config():
    os.makedirs(CONFIG_DIR, exist_ok=True)
    if os.path.exists(CONFIG_FILE):
        with open(CONFIG_FILE, "r") as f:
            saved = json.load(f)
        merged = {**DEFAULT_CONFIG, **saved}
        return merged
    return dict(DEFAULT_CONFIG)


def save_config(config):
    os.makedirs(CONFIG_DIR, exist_ok=True)
    with open(CONFIG_FILE, "w") as f:
        json.dump(config, f, indent=2)
