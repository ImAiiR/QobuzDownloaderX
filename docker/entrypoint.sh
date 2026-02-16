#!/bin/sh
set -e

# Create directories if they don't exist
mkdir -p "${QBDLX_CONFIG_DIR}" "${QBDLX_DOWNLOAD_DIR}"

echo "============================================"
echo "  QobuzDownloaderX Web UI"
echo "  Port: ${QBDLX_PORT}"
echo "  Config: ${QBDLX_CONFIG_DIR}"
echo "  Downloads: ${QBDLX_DOWNLOAD_DIR}"
echo "============================================"

# Start gunicorn with the Flask app
exec gunicorn \
    --bind "${QBDLX_HOST}:${QBDLX_PORT}" \
    --workers 2 \
    --threads 4 \
    --timeout 600 \
    --access-logfile - \
    --error-logfile - \
    "app.main:app"
