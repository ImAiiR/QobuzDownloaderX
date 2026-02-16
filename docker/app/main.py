"""QobuzDownloaderX Web UI - Flask application."""

import logging
import os

from flask import Flask, jsonify, render_template, request

from .config import load_config, save_config
from .downloader import DownloadManager
from .qobuz_api import QobuzAPI

logging.basicConfig(
    level=logging.INFO,
    format="%(asctime)s [%(levelname)s] %(name)s: %(message)s",
)
logger = logging.getLogger(__name__)

app = Flask(__name__)
qobuz = QobuzAPI()
download_manager = DownloadManager(qobuz)


@app.route("/")
def index():
    return render_template("index.html")


@app.route("/api/login", methods=["POST"])
def api_login():
    data = request.get_json()
    email = data.get("email")
    password = data.get("password")
    token = data.get("token")
    app_id = data.get("app_id")
    app_secret = data.get("app_secret")

    try:
        # Set custom app_id/secret if provided
        if app_id:
            qobuz.app_id = app_id
        if app_secret:
            qobuz.app_secret = app_secret

        if token:
            result = qobuz.login(token=token, app_id=app_id or None)
        elif email and password:
            result = qobuz.login(email=email, password=password, app_id=app_id or None)
        else:
            return jsonify({"error": "Email/password or token required"}), 400

        # Ensure we have app_secret (critical for downloads)
        if not qobuz.app_secret:
            try:
                qobuz.get_app_secret()
            except Exception as e:
                logger.warning(f"Could not get app_secret: {e}")

        logger.info(
            f"Login successful: user={qobuz.user_display_name}, "
            f"app_id={qobuz.app_id}, has_secret={bool(qobuz.app_secret)}"
        )

        return jsonify({"success": True, "user": result})

    except Exception as e:
        logger.error(f"Login failed: {e}")
        return jsonify({"error": str(e)}), 401


@app.route("/api/session")
def api_session():
    info = qobuz.get_session_info()
    if info:
        return jsonify({"logged_in": True, "user": info})
    return jsonify({"logged_in": False})


@app.route("/api/logout", methods=["POST"])
def api_logout():
    qobuz.user_auth_token = None
    qobuz.user_id = None
    qobuz.user_display_name = None
    return jsonify({"success": True})


@app.route("/api/search")
def api_search():
    if not qobuz.is_logged_in():
        return jsonify({"error": "Not logged in"}), 401

    query = request.args.get("q", "")
    search_type = request.args.get("type", "albums")
    limit = int(request.args.get("limit", 50))
    offset = int(request.args.get("offset", 0))

    if not query:
        return jsonify({"error": "Query required"}), 400

    try:
        results = qobuz.search(query, search_type, limit, offset)
        return jsonify(results)
    except Exception as e:
        logger.error(f"Search failed: {e}")
        return jsonify({"error": str(e)}), 500


@app.route("/api/album/<album_id>")
def api_album(album_id):
    if not qobuz.is_logged_in():
        return jsonify({"error": "Not logged in"}), 401
    try:
        data = qobuz.get_album(album_id)
        return jsonify(data)
    except Exception as e:
        return jsonify({"error": str(e)}), 500


@app.route("/api/track/<track_id>")
def api_track(track_id):
    if not qobuz.is_logged_in():
        return jsonify({"error": "Not logged in"}), 401
    try:
        data = qobuz.get_track(track_id)
        return jsonify(data)
    except Exception as e:
        return jsonify({"error": str(e)}), 500


@app.route("/api/artist/<artist_id>")
def api_artist(artist_id):
    if not qobuz.is_logged_in():
        return jsonify({"error": "Not logged in"}), 401
    try:
        data = qobuz.get_artist(artist_id)
        return jsonify(data)
    except Exception as e:
        return jsonify({"error": str(e)}), 500


@app.route("/api/download/album", methods=["POST"])
def api_download_album():
    if not qobuz.is_logged_in():
        return jsonify({"error": "Not logged in"}), 401

    data = request.get_json()
    album_id = data.get("album_id")
    if not album_id:
        return jsonify({"error": "album_id required"}), 400

    download_id = download_manager.download_album(str(album_id))
    return jsonify({"success": True, "download_id": download_id})


@app.route("/api/download/track", methods=["POST"])
def api_download_track():
    if not qobuz.is_logged_in():
        return jsonify({"error": "Not logged in"}), 401

    data = request.get_json()
    track_id = data.get("track_id")
    if not track_id:
        return jsonify({"error": "track_id required"}), 400

    download_id = download_manager.download_track(str(track_id))
    return jsonify({"success": True, "download_id": download_id})


@app.route("/api/download/playlist", methods=["POST"])
def api_download_playlist():
    if not qobuz.is_logged_in():
        return jsonify({"error": "Not logged in"}), 401

    data = request.get_json()
    playlist_id = data.get("playlist_id")
    if not playlist_id:
        return jsonify({"error": "playlist_id required"}), 400

    download_id = download_manager.download_playlist(str(playlist_id))
    return jsonify({"success": True, "download_id": download_id})


@app.route("/api/downloads")
def api_downloads():
    status = download_manager.get_status()
    return jsonify(status)


@app.route("/api/test-secret")
def api_test_secret():
    """Test if the current app_secret works for stream URLs."""
    if not qobuz.is_logged_in():
        return jsonify({"error": "Not logged in"}), 401

    has_secret = bool(qobuz.app_secret)
    secret_length = len(qobuz.app_secret) if qobuz.app_secret else 0

    result = {
        "has_secret": has_secret,
        "secret_length": secret_length,
        "app_id": qobuz.app_id,
    }

    if has_secret:
        works = qobuz.test_secret()
        result["secret_works"] = works
    else:
        result["secret_works"] = False

    return jsonify(result)


@app.route("/api/config", methods=["GET"])
def api_get_config():
    config = load_config()
    return jsonify(config)


@app.route("/api/config", methods=["POST"])
def api_set_config():
    data = request.get_json()
    config = load_config()
    config.update(data)
    save_config(config)
    return jsonify({"success": True, "config": config})


def main():
    port = int(os.environ.get("QBDLX_PORT", 6595))
    host = os.environ.get("QBDLX_HOST", "0.0.0.0")
    debug = os.environ.get("QBDLX_DEBUG", "false").lower() == "true"

    logger.info(f"Starting QobuzDownloaderX Web UI on {host}:{port}")
    app.run(host=host, port=port, debug=debug)


if __name__ == "__main__":
    main()
