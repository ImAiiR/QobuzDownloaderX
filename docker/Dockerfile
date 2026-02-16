FROM python:3.12-slim

LABEL maintainer="QobuzDownloaderX Docker"
LABEL description="QobuzDownloaderX Web UI - Qobuz music downloader in Docker"

# Prevent Python from writing .pyc files and enable unbuffered output
ENV PYTHONDONTWRITEBYTECODE=1 \
    PYTHONUNBUFFERED=1 \
    QBDLX_CONFIG_DIR=/config \
    QBDLX_DOWNLOAD_DIR=/downloads \
    QBDLX_PORT=6595 \
    QBDLX_HOST=0.0.0.0

WORKDIR /app

# Install system dependencies
RUN apt-get update && \
    apt-get install -y --no-install-recommends \
        tini \
    && rm -rf /var/lib/apt/lists/*

# Install Python dependencies
COPY requirements.txt .
RUN pip install --no-cache-dir -r requirements.txt

# Copy application
COPY app/ ./app/
COPY entrypoint.sh .
RUN chmod +x entrypoint.sh

# Create directories for config and downloads
RUN mkdir -p /config /downloads

# Expose web UI port
EXPOSE 6595

# Volumes for persistent data
VOLUME ["/config", "/downloads"]

# Use tini as init system
ENTRYPOINT ["tini", "--"]

# Start the application
CMD ["./entrypoint.sh"]
