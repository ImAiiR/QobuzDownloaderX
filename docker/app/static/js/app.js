/* QobuzDownloaderX Web UI */

const API = {
    async post(url, data) {
        const resp = await fetch(url, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(data),
        });
        return resp.json();
    },
    async get(url) {
        const resp = await fetch(url);
        return resp.json();
    },
};

/* State */
let pollInterval = null;

/* Init */
document.addEventListener("DOMContentLoaded", async () => {
    setupLoginTabs();
    setupLoginForm();
    setupNavigation();
    setupSearch();
    setupSettings();
    setupModal();
    setupLogout();

    // Check existing session
    const session = await API.get("/api/session");
    if (session.logged_in) {
        showMainScreen(session.user);
    }
});

/* Login */
function setupLoginTabs() {
    document.querySelectorAll(".tab").forEach((tab) => {
        tab.addEventListener("click", () => {
            document.querySelectorAll(".tab").forEach((t) => t.classList.remove("active"));
            tab.classList.add("active");
            const mode = tab.dataset.tab;
            document.getElementById("email-fields").style.display = mode === "email" ? "block" : "none";
            document.getElementById("token-fields").style.display = mode === "token" ? "block" : "none";
        });
    });
}

function setupLoginForm() {
    document.getElementById("login-form").addEventListener("submit", async (e) => {
        e.preventDefault();
        const btn = document.getElementById("login-btn");
        const errorEl = document.getElementById("login-error");
        errorEl.style.display = "none";
        btn.disabled = true;
        btn.textContent = "Connecting...";

        const activeTab = document.querySelector(".tab.active").dataset.tab;
        const payload = {
            app_id: document.getElementById("app-id").value || undefined,
            app_secret: document.getElementById("app-secret").value || undefined,
        };

        if (activeTab === "email") {
            payload.email = document.getElementById("email").value;
            payload.password = document.getElementById("password").value;
        } else {
            payload.token = document.getElementById("token").value;
        }

        try {
            const result = await API.post("/api/login", payload);
            if (result.success) {
                showMainScreen(result.user);
            } else {
                errorEl.textContent = result.error || "Login failed";
                errorEl.style.display = "block";
            }
        } catch (err) {
            errorEl.textContent = "Connection error";
            errorEl.style.display = "block";
        } finally {
            btn.disabled = false;
            btn.textContent = "Login";
        }
    });
}

function showMainScreen(user) {
    document.getElementById("login-screen").classList.remove("active");
    document.getElementById("main-screen").classList.add("active");
    document.getElementById("user-name").textContent = user.display_name || user.user_id || "";
    loadSettings();
    startDownloadPolling();
}

function showLoginScreen() {
    document.getElementById("main-screen").classList.remove("active");
    document.getElementById("login-screen").classList.add("active");
    stopDownloadPolling();
}

/* Logout */
function setupLogout() {
    document.getElementById("logout-btn").addEventListener("click", async () => {
        await API.post("/api/logout");
        showLoginScreen();
    });
}

/* Navigation */
function setupNavigation() {
    document.querySelectorAll(".nav-btn").forEach((btn) => {
        btn.addEventListener("click", () => {
            document.querySelectorAll(".nav-btn").forEach((b) => b.classList.remove("active"));
            document.querySelectorAll(".page").forEach((p) => p.classList.remove("active"));
            btn.classList.add("active");
            document.getElementById("page-" + btn.dataset.page).classList.add("active");
        });
    });
}

/* Search */
function setupSearch() {
    const input = document.getElementById("search-input");
    const btn = document.getElementById("search-btn");

    btn.addEventListener("click", doSearch);
    input.addEventListener("keydown", (e) => {
        if (e.key === "Enter") doSearch();
    });
}

async function doSearch() {
    const query = document.getElementById("search-input").value.trim();
    if (!query) return;

    const type = document.getElementById("search-type").value;
    const resultsEl = document.getElementById("search-results");
    const loadingEl = document.getElementById("search-loading");

    resultsEl.innerHTML = "";
    loadingEl.style.display = "flex";

    try {
        const data = await API.get(`/api/search?q=${encodeURIComponent(query)}&type=${type}`);
        loadingEl.style.display = "none";
        renderSearchResults(data, type);
    } catch (err) {
        loadingEl.style.display = "none";
        resultsEl.innerHTML = '<div class="empty-state">Search failed</div>';
    }
}

function renderSearchResults(data, type) {
    const el = document.getElementById("search-results");
    el.innerHTML = "";

    let items = [];
    if (type === "albums") items = data.albums?.items || [];
    else if (type === "tracks") items = data.tracks?.items || [];
    else if (type === "artists") items = data.artists?.items || [];

    if (!items.length) {
        el.innerHTML = '<div class="empty-state">No results found</div>';
        return;
    }

    items.forEach((item) => {
        if (type === "albums") el.appendChild(createAlbumCard(item));
        else if (type === "tracks") el.appendChild(createTrackCard(item));
        else if (type === "artists") el.appendChild(createArtistCard(item));
    });
}

function createAlbumCard(album) {
    const card = document.createElement("div");
    card.className = "result-card";

    const imgUrl = album.image?.large || album.image?.small || "";
    const title = album.title || "";
    const artist = album.artist?.name || "";
    const year = album.release_date_original ? album.release_date_original.substring(0, 4) : "";
    const quality = album.maximum_bit_depth && album.maximum_sampling_rate
        ? `${album.maximum_bit_depth}bit/${album.maximum_sampling_rate}kHz`
        : "";

    card.innerHTML = `
        <img src="${imgUrl}" alt="${title}" loading="lazy">
        <div class="card-info">
            <div class="card-title" title="${title}">${title}</div>
            <div class="card-artist">${artist}</div>
            <div class="card-meta">${year} ${quality ? "- " + quality : ""}</div>
        </div>
        <div class="card-actions">
            <button class="btn btn-download" onclick="event.stopPropagation(); downloadAlbum('${album.id}')">Download</button>
        </div>
    `;
    card.addEventListener("click", () => showAlbumDetail(album.id));
    return card;
}

function createTrackCard(track) {
    const card = document.createElement("div");
    card.className = "result-card";

    const imgUrl = track.album?.image?.large || track.album?.image?.small || "";
    const title = track.title || "";
    const artist = track.performer?.name || track.artist?.name || "";
    const album = track.album?.title || "";
    const duration = formatDuration(track.duration);

    card.innerHTML = `
        <img src="${imgUrl}" alt="${title}" loading="lazy">
        <div class="card-info">
            <div class="card-title" title="${title}">${title}</div>
            <div class="card-artist">${artist}</div>
            <div class="card-meta">${album} - ${duration}</div>
        </div>
        <div class="card-actions">
            <button class="btn btn-download" onclick="event.stopPropagation(); downloadTrack('${track.id}')">Download</button>
        </div>
    `;
    if (track.album?.id) {
        card.addEventListener("click", () => showAlbumDetail(track.album.id));
    }
    return card;
}

function createArtistCard(artist) {
    const card = document.createElement("div");
    card.className = "result-card artist-card";

    const imgUrl = artist.image?.large || artist.image?.small || artist.picture || "";
    const name = artist.name || "";
    const albums = artist.albums_count || 0;

    card.innerHTML = `
        <img src="${imgUrl}" alt="${name}" loading="lazy">
        <div class="card-info">
            <div class="card-title" title="${name}">${name}</div>
            <div class="card-meta">${albums} albums</div>
        </div>
    `;
    card.addEventListener("click", () => browseArtist(artist.id));
    return card;
}

/* Album Detail Modal */
function setupModal() {
    document.querySelector(".modal-close").addEventListener("click", closeModal);
    document.getElementById("album-modal").addEventListener("click", (e) => {
        if (e.target === e.currentTarget) closeModal();
    });
}

function closeModal() {
    document.getElementById("album-modal").style.display = "none";
}

async function showAlbumDetail(albumId) {
    const modal = document.getElementById("album-modal");
    const detail = document.getElementById("album-detail");
    modal.style.display = "flex";
    detail.innerHTML = '<div class="loading"><div class="spinner"></div><span>Loading...</span></div>';

    try {
        const album = await API.get(`/api/album/${albumId}`);
        renderAlbumDetail(album);
    } catch (err) {
        detail.innerHTML = '<div class="empty-state">Failed to load album</div>';
    }
}

function renderAlbumDetail(album) {
    const detail = document.getElementById("album-detail");
    const imgUrl = album.image?.large || "";
    const title = album.version ? `${album.title} (${album.version})` : album.title;
    const artist = album.artist?.name || "";
    const year = album.release_date_original ? album.release_date_original.substring(0, 4) : "";
    const genre = album.genre?.name || "";
    const label = album.label?.name || "";
    const quality = `${album.maximum_bit_depth}bit / ${album.maximum_sampling_rate}kHz`;
    const tracks = album.tracks?.items || [];
    const totalDuration = tracks.reduce((sum, t) => sum + (t.duration || 0), 0);

    let html = `
        <div class="album-header">
            <img src="${imgUrl}" alt="${title}">
            <div class="album-meta">
                <h2>${title}</h2>
                <div class="album-artist">${artist}</div>
                <div class="album-info">${year} - ${genre} - ${label}</div>
                <div class="album-quality">${quality} - ${tracks.length} tracks - ${formatDuration(totalDuration)}</div>
                <button class="btn btn-download" onclick="downloadAlbum('${album.id}')">Download Album</button>
            </div>
        </div>
        <ul class="track-list">
    `;

    let currentDisc = 0;
    tracks.forEach((track) => {
        if (album.media_count > 1 && track.media_number !== currentDisc) {
            currentDisc = track.media_number;
            html += `<li class="track-item" style="color:var(--text-muted);font-weight:600;padding-top:1rem">Disc ${currentDisc}</li>`;
        }
        const dur = formatDuration(track.duration);
        const trackTitle = track.version ? `${track.title} (${track.version})` : track.title;
        html += `
            <li class="track-item">
                <span class="track-num">${track.track_number}</span>
                <span class="track-title">${trackTitle}</span>
                <span class="track-duration">${dur}</span>
                <button class="track-dl-btn" onclick="downloadTrack('${track.id}')">DL</button>
            </li>
        `;
    });

    html += "</ul>";
    detail.innerHTML = html;
}

async function browseArtist(artistId) {
    const resultsEl = document.getElementById("search-results");
    resultsEl.innerHTML = '<div class="loading"><div class="spinner"></div><span>Loading artist...</span></div>';

    try {
        const artist = await API.get(`/api/artist/${artistId}`);
        resultsEl.innerHTML = "";
        const albums = artist.albums?.items || [];
        if (!albums.length) {
            resultsEl.innerHTML = '<div class="empty-state">No albums found</div>';
            return;
        }
        albums.forEach((album) => {
            resultsEl.appendChild(createAlbumCard(album));
        });
    } catch (err) {
        resultsEl.innerHTML = '<div class="empty-state">Failed to load artist</div>';
    }
}

/* Downloads */
async function downloadAlbum(albumId) {
    try {
        await API.post("/api/download/album", { album_id: albumId });
        switchToPage("downloads");
    } catch (err) {
        alert("Download failed: " + err.message);
    }
}

async function downloadTrack(trackId) {
    try {
        await API.post("/api/download/track", { track_id: trackId });
        switchToPage("downloads");
    } catch (err) {
        alert("Download failed: " + err.message);
    }
}

function switchToPage(page) {
    document.querySelectorAll(".nav-btn").forEach((b) => b.classList.remove("active"));
    document.querySelectorAll(".page").forEach((p) => p.classList.remove("active"));
    document.querySelector(`.nav-btn[data-page="${page}"]`).classList.add("active");
    document.getElementById("page-" + page).classList.add("active");
}

function startDownloadPolling() {
    if (pollInterval) return;
    updateDownloads();
    pollInterval = setInterval(updateDownloads, 2000);
}

function stopDownloadPolling() {
    if (pollInterval) {
        clearInterval(pollInterval);
        pollInterval = null;
    }
}

async function updateDownloads() {
    try {
        const data = await API.get("/api/downloads");
        renderActiveDownloads(data.active || []);
        renderDownloadHistory(data.history || []);
    } catch (err) {
        // Silently fail
    }
}

function renderActiveDownloads(downloads) {
    const el = document.getElementById("active-downloads");
    if (!downloads.length) {
        el.innerHTML = '<div class="empty-state">No active downloads</div>';
        return;
    }

    el.innerHTML = downloads.map((dl) => `
        <div class="download-item">
            ${dl.cover ? `<img src="${dl.cover}" alt="">` : ""}
            <div class="download-info">
                <div class="dl-title">${dl.title}</div>
                <div class="dl-artist">${dl.artist}</div>
                <div class="dl-track">${dl.current_track || ""} (${dl.completed_tracks}/${dl.total_tracks})</div>
                <div class="progress-bar"><div class="progress-fill" style="width:${dl.progress}%"></div></div>
            </div>
            <span class="download-status status-${dl.status}">${dl.status} ${dl.progress}%</span>
        </div>
    `).join("");
}

function renderDownloadHistory(history) {
    const el = document.getElementById("download-history");
    if (!history.length) {
        el.innerHTML = '<div class="empty-state">No download history</div>';
        return;
    }

    el.innerHTML = history.reverse().map((dl) => `
        <div class="download-item">
            ${dl.cover ? `<img src="${dl.cover}" alt="">` : ""}
            <div class="download-info">
                <div class="dl-title">${dl.title}</div>
                <div class="dl-artist">${dl.artist || dl.type}</div>
            </div>
            <span class="download-status status-${dl.status}">${dl.status}</span>
        </div>
    `).join("");
}

/* Settings */
const SETTINGS_MAP = {
    "cfg-quality": "quality",
    "cfg-format": "audio_format",
    "cfg-art-size": "artwork_size",
    "cfg-embed-art-size": "embedded_art_size",
    "cfg-artist-tpl": "artist_template",
    "cfg-album-tpl": "album_template",
    "cfg-track-tpl": "track_template",
    "cfg-playlist-tpl": "playlist_template",
    "cfg-tag-album": "tag_album",
    "cfg-tag-artist": "tag_artist",
    "cfg-tag-album-artist": "tag_album_artist",
    "cfg-tag-title": "tag_track_title",
    "cfg-tag-track": "tag_track_number",
    "cfg-tag-disc": "tag_disc",
    "cfg-tag-genre": "tag_genre",
    "cfg-tag-year": "tag_year",
    "cfg-tag-date": "tag_release_date",
    "cfg-tag-composer": "tag_composer",
    "cfg-tag-copyright": "tag_copyright",
    "cfg-tag-isrc": "tag_isrc",
    "cfg-tag-upc": "tag_upc",
    "cfg-tag-label": "tag_label",
    "cfg-tag-image": "tag_image",
    "cfg-tag-total-tracks": "tag_total_tracks",
    "cfg-tag-total-discs": "tag_total_discs",
    "cfg-tag-explicit": "tag_explicit",
    "cfg-save-artwork": "save_artwork",
    "cfg-embed-artwork": "embed_artwork",
    "cfg-fix-md5": "fix_md5",
    "cfg-streamable-check": "streamable_check",
    "cfg-download-goodies": "download_goodies",
};

async function loadSettings() {
    try {
        const config = await API.get("/api/config");
        for (const [elemId, key] of Object.entries(SETTINGS_MAP)) {
            const el = document.getElementById(elemId);
            if (!el) continue;
            if (el.type === "checkbox") {
                el.checked = !!config[key];
            } else {
                el.value = config[key] || "";
            }
        }
    } catch (err) {
        console.error("Failed to load settings:", err);
    }
}

function setupSettings() {
    document.getElementById("save-settings-btn").addEventListener("click", async () => {
        const payload = {};
        for (const [elemId, key] of Object.entries(SETTINGS_MAP)) {
            const el = document.getElementById(elemId);
            if (!el) continue;
            payload[key] = el.type === "checkbox" ? el.checked : el.value;
        }

        try {
            await API.post("/api/config", payload);
            const msg = document.getElementById("settings-saved");
            msg.style.display = "block";
            setTimeout(() => { msg.style.display = "none"; }, 2000);
        } catch (err) {
            alert("Failed to save settings");
        }
    });
}

/* Helpers */
function formatDuration(seconds) {
    if (!seconds) return "0:00";
    const m = Math.floor(seconds / 60);
    const s = Math.floor(seconds % 60);
    return `${m}:${s.toString().padStart(2, "0")}`;
}
