using QobuzDownloaderX.Properties;
using QopenAPI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QobuzDownloaderX.Helpers
{
    internal sealed class GetInfo
    {
        public Service QoService = new Service();
        public User QoUser = new User();
        public Artist QoArtist = new Artist();
        public Album QoAlbum = new Album();
        public Item QoItem = new Item();
        public Favorites QoFavorites = new Favorites();
        public Playlist QoPlaylist = new Playlist();
        public QopenAPI.Label QoLabel = new QopenAPI.Label();

        public string outputText { get; set; }

        public void updateDownloadOutput(string text)
        {
            if (outputText == "Test String" | outputText == null)
            {
                Miscellaneous.update(qbdlxForm._qbdlxForm, null);
                outputText = qbdlxForm._qbdlxForm.downloadOutput.Text;
                Miscellaneous.update(qbdlxForm._qbdlxForm, text);
                outputText = qbdlxForm._qbdlxForm.downloadOutput.Text;
            }
            else if (text == null)
            {
                Miscellaneous.update(qbdlxForm._qbdlxForm, null);
                outputText = qbdlxForm._qbdlxForm.downloadOutput.Text;
            }
            else
            {
                Miscellaneous.update(qbdlxForm._qbdlxForm, outputText + text);
                outputText = qbdlxForm._qbdlxForm.downloadOutput.Text;
            }
        }

        public Artist getArtistInfo(string app_id, string artist_id, string user_auth_token)
        {
            try
            {
                // Grab artist info with auth
                outputText = null;
                qbdlxForm._qbdlxForm.logger.Debug("Getting artist Info…");

                int limit = 500;
                int offset = 0;

                // 1) First request (initial page)
                QoArtist = QoService.ArtistGetWithAuth(app_id, artist_id, user_auth_token, limit, offset);

                if (QoArtist == null || QoArtist.Albums == null || QoArtist.Albums.Items == null)
                {
                    qbdlxForm._qbdlxForm.logger.Warning("Artist has no albums or retrieval failed.");
                    return QoArtist; // return what we have (possibly null)
                }

                var allItems = QoArtist.Albums.Items.Cast<object>().ToList();
                int total = QoArtist.Albums.Total;

                offset += limit;

                // 2) Pagination loop - keep requesting pages until all albums are collected
                while (allItems.Count < total)
                {
                    var page = QoService.ArtistGetWithAuth(app_id, artist_id, user_auth_token, limit, offset);

                    if (page == null || page.Albums == null || page.Albums.Items == null || page.Albums.Items.Count == 0)
                        break;

                    allItems.AddRange(page.Albums.Items.Cast<object>());
                    offset += limit;

                    // Safety break to prevent infinite loop
                    if (offset > 100000) break;
                }

                if (!Settings.Default.downloadArtistOther)
                {
                    QoArtist.Albums.Items = allItems.Cast<Item>()
                                                    .Where(a => a.Artist?.Id.ToString() == artist_id)
                                                    .ToList();
                } else
                {
                    QoArtist.Albums.Items = allItems.Cast<Item>()
                                                    .OrderByDescending(a => a.Artist?.Id.ToString() == artist_id)
                                                    .ToList();
                }

                return QoArtist;
            }
            catch (Exception getArtistInfoEx)
            {
                updateDownloadOutput("\r\n" + getArtistInfoEx.ToString());
                qbdlxForm._qbdlxForm.logger.Error("Failed to get artist info, error below:\r\n" + getArtistInfoEx);
                return null;
            }
        }

        public QopenAPI.Label getLabelInfo(string app_id, string label_id, string user_auth_token)
        {
            try
            {
                qbdlxForm._qbdlxForm.logger.Debug("Getting label info…");
                outputText = null;

                int limit = 500;
                int offset = 0;

                // 1) First request (initial page)
                QoLabel = QoService.LabelGetWithAuth(app_id, label_id, "albums", user_auth_token, limit, offset);

                if (QoLabel == null || QoLabel.Albums == null || QoLabel.Albums.Items == null)
                    return QoLabel;

                // Store all collected items here
                var allItems = QoLabel.Albums.Items.Cast<object>().ToList();

                int total = 0;
                try { total = QoLabel.Albums.Total; } catch { }

                offset += limit;

                // 2) Pagination loop - keep requesting pages until all items are collected
                while ((total == 0 && QoLabel.Albums.Items.Count > 0)
                    || (total > 0 && allItems.Count < total))
                {
                    var page = QoService.LabelGetWithAuth(app_id, label_id, "albums", user_auth_token, limit, offset);

                    if (page == null || page.Albums == null || page.Albums.Items == null || page.Albums.Items.Count == 0)
                        break;

                    // Add items from the page
                    allItems.AddRange(page.Albums.Items.Cast<object>());

                    offset += limit;

                    if (offset > 100000) break; // safety cutoff
                }

                QoLabel.Albums.Items = allItems.Cast<Item>().ToList();
                return QoLabel;
            }
            catch (Exception getLabelInfoEx)
            {
                updateDownloadOutput("\r\n" + getLabelInfoEx.ToString());
                qbdlxForm._qbdlxForm.logger.Error("Failed to get label info, error below:\r\n" + getLabelInfoEx);
                return null;
            }
        }

        public Favorites getFavoritesInfo(string app_id, string user_id, string type, string user_auth_token)
        {
            try
            {
                qbdlxForm._qbdlxForm.logger.Debug("Getting favorites Info…");
                outputText = null;

                int limit = 500;
                int offset = 0;

                // 1) First request (initial page)
                QoFavorites = QoService.FavoriteGetUserFavoritesWithAuth(
                    app_id, user_id, type, user_auth_token, limit, offset);

                if (QoFavorites == null)
                    return QoFavorites;

                // Detect the correct list depending on type
                List<object> allItems;

                if (type == "albums")
                    allItems = QoFavorites.Albums.Items.Cast<object>().ToList();
                else if (type == "tracks")
                    allItems = QoFavorites.Tracks.Items.Cast<object>().ToList();
                else if (type == "artists")
                    allItems = QoFavorites.Artists.Items.Cast<object>().ToList();
                else
                    return QoFavorites;

                int total = 0;
                try
                {
                    if (type == "albums") total = QoFavorites.Albums.Total;
                    if (type == "tracks") total = QoFavorites.Tracks.Total;
                    if (type == "artists") total = QoFavorites.Artists.Total;
                }
                catch { }

                offset += limit;

                // 2) Pagination loop - keep requesting pages until all items are collected
                while (total == 0 || allItems.Count < total)
                {
                    var page = QoService.FavoriteGetUserFavoritesWithAuth(
                        app_id, user_id, type, user_auth_token, limit, offset);

                    if (page == null)
                        break;

                    List<object> pageItems;

                    if (type == "albums")
                        pageItems = page.Albums.Items.Cast<object>().ToList();
                    else if (type == "tracks")
                        pageItems = page.Tracks.Items.Cast<object>().ToList();
                    else // artists
                        pageItems = page.Artists.Items.Cast<object>().ToList();

                    if (pageItems.Count == 0)
                        break;

                    allItems.AddRange(pageItems);

                    offset += limit;
                    if (offset > 1000000) break;
                }

                if (type == "albums")
                    QoFavorites.Albums.Items = allItems.Cast<Item>().ToList();
                else if (type == "tracks")
                    QoFavorites.Tracks.Items = allItems.Cast<Item>().ToList();
                else
                    QoFavorites.Artists.Items = allItems.Cast<Item>().ToList();

                return QoFavorites;
            }
            catch (Exception getFavoritesInfoEx)
            {
                updateDownloadOutput("\r\n" + getFavoritesInfoEx.ToString());
                qbdlxForm._qbdlxForm.logger.Error("Failed to get favorites info, error below:\r\n" + getFavoritesInfoEx);
                return null;
            }
        }

        public Item getTrackInfoLabels(string app_id, string track_id, string user_auth_token)
        {
            try
            {
                // Grab track info with auth
                outputText = null;
                qbdlxForm._qbdlxForm.logger.Debug("Getting track Info…");
                QoItem = QoService.TrackGetWithAuth(app_id, track_id, user_auth_token);

                if (QoItem != null)
                {
                    if (QoItem.Album != null)
                    {
                        string album_id = QoItem.Album.Id;

                        // Pagination variables
                        int limit = 500;
                        int offset = 0;

                        // 1) First request (initial page)
                        QoAlbum = QoService.AlbumGetWithAuth(app_id, album_id, user_auth_token, limit, offset);

                        if (QoAlbum == null || QoAlbum.Tracks == null || QoAlbum.Tracks.Items == null)
                        {
                            QoAlbum = null;
                            qbdlxForm._qbdlxForm.logger.Warning("Album has no associated tracks or album retrieval failed.");
                        }
                        else
                        {
                            var allItems = QoAlbum.Tracks.Items.Cast<object>().ToList();
                            int total = QoAlbum.Tracks.Total;

                            offset += limit;

                            // 2) Pagination loop
                            while (allItems.Count < total)
                            {
                                var page = QoService.AlbumGetWithAuth(app_id, album_id, user_auth_token, limit, offset);

                                if (page == null || page.Tracks == null || page.Tracks.Items == null || page.Tracks.Items.Count == 0)
                                    break;

                                allItems.AddRange(page.Tracks.Items.Cast<object>());
                                offset += limit;

                                // Safety break to prevent infinite loop
                                if (offset > 100000) break;
                            }

                            QoAlbum.Tracks.Items = allItems.Cast<Item>().ToList();
                        }
                    }
                    else
                    {
                        QoAlbum = null;
                        qbdlxForm._qbdlxForm.logger.Warning("Track has no associated album.");
                    }
                }
                else
                {
                    QoAlbum = null;
                    qbdlxForm._qbdlxForm.logger.Warning("No track information was retrieved.");
                }

                return QoItem;
            }
            catch (Exception getTrackInfoLabelsEx)
            {
                updateDownloadOutput("\r\n" + getTrackInfoLabelsEx.ToString());
                qbdlxForm._qbdlxForm.logger.Error("Failed to get track info, error below:\r\n" + getTrackInfoLabelsEx);
                return null;
            }
        }

        public Album getAlbumInfoLabels(string app_id, string album_id, string user_auth_token)
        {
            try
            {
                // Grab album info with auth
                outputText = null;
                qbdlxForm._qbdlxForm.logger.Debug("Getting album Info…");

                // Pagination variables
                int limit = 500;
                int offset = 0;

                // 1) First request (initial page)
                QoAlbum = QoService.AlbumGetWithAuth(app_id, album_id, user_auth_token, limit, offset);

                if (QoAlbum == null || QoAlbum.Tracks == null || QoAlbum.Tracks.Items == null)
                {
                    qbdlxForm._qbdlxForm.logger.Warning("Album has no tracks or retrieval failed.");
                    return QoAlbum; // Return whatever we got, possibly null
                }

                var allItems = QoAlbum.Tracks.Items.Cast<object>().ToList();
                int total = QoAlbum.Tracks.Total;

                offset += limit;

                // 2) Pagination loop - keep requesting pages until all tracks are collected
                while (allItems.Count < total)
                {
                    var page = QoService.AlbumGetWithAuth(app_id, album_id, user_auth_token, limit, offset);

                    if (page == null || page.Tracks == null || page.Tracks.Items == null || page.Tracks.Items.Count == 0)
                        break;

                    allItems.AddRange(page.Tracks.Items.Cast<object>());
                    offset += limit;

                    // Safety break to prevent infinite loop
                    if (offset > 100000) break;
                }

                QoAlbum.Tracks.Items = allItems.Cast<Item>().ToList();
                return QoAlbum;
            }
            catch (Exception getAlbumInfoLabelsEx)
            {
                updateDownloadOutput("\r\n" + getAlbumInfoLabelsEx.ToString());
                qbdlxForm._qbdlxForm.logger.Error("Failed to get album info, error below:\r\n" + getAlbumInfoLabelsEx);
                return null;
            }
        }

        public Playlist getPlaylistInfoLabels(string app_id, string playlist_id, string user_auth_token)
        {
            try
            {
                qbdlxForm._qbdlxForm.logger.Debug("Getting playlist Info…");
                outputText = null;

                int limit = 500;
                int offset = 0;

                // 1) First request (initial page)
                QoPlaylist = QoService.PlaylistGetWithAuth(app_id, user_auth_token, playlist_id, "tracks", limit, offset);

                if (QoPlaylist == null || QoPlaylist.Tracks == null || QoPlaylist.Tracks.Items == null)
                    return QoPlaylist;

                var allItems = QoPlaylist.Tracks.Items.Cast<object>().ToList();

                int total = QoPlaylist.Tracks.Total;

                offset += limit;

                // 2) Pagination loop - keep requesting pages until all items are collected
                while (total == 0 || allItems.Count < total)
                {
                    var page = QoService.PlaylistGetWithAuth(app_id, user_auth_token, playlist_id, "tracks", limit, offset);

                    if (page == null || page.Tracks == null || page.Tracks.Items == null)
                        break;

                    if (page.Tracks.Items.Count == 0)
                        break;

                    allItems.AddRange(page.Tracks.Items.Cast<object>());

                    offset += limit;
                    if (offset > 1000000) break;
                }

                QoPlaylist.Tracks.Items = allItems.Cast<Item>().ToList();
                return QoPlaylist;
            }
            catch (Exception getPlaylistInfoLabelsEx)
            {
                updateDownloadOutput("\r\n" + getPlaylistInfoLabelsEx.ToString());
                qbdlxForm._qbdlxForm.logger.Error("Failed to get playlist info, error below:\r\n" + getPlaylistInfoLabelsEx);
                return null;
            }
        }
    }
}
