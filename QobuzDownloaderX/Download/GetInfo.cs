using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QobuzDownloaderX;
using QopenAPI;
using System.Net;
using System.IO;
using System.Diagnostics;

namespace QobuzDownloaderX
{
    class GetInfo
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
                qbdlxForm._qbdlxForm.update(null);
                outputText = qbdlxForm._qbdlxForm.downloadOutput.Text;
                qbdlxForm._qbdlxForm.update(text);
                outputText = qbdlxForm._qbdlxForm.downloadOutput.Text;
            }
            else if (text == null)
            {
                qbdlxForm._qbdlxForm.update(null);
                outputText = qbdlxForm._qbdlxForm.downloadOutput.Text;
            }
            else
            {
                qbdlxForm._qbdlxForm.update(outputText + text);
                outputText = qbdlxForm._qbdlxForm.downloadOutput.Text;
            }

        }

        public Artist getArtistInfo(string app_id, string artist_id, string user_auth_token)
        {
            try
            {
                // Grab album info with auth
                outputText = null;
                Console.WriteLine("Getting Artist Info...");
                QoArtist = QoService.ArtistGetWithAuth(app_id, artist_id, user_auth_token);
                return QoArtist;
            }
            catch (Exception getArtistInfoEx)
            {
                updateDownloadOutput("\r\n" + getArtistInfoEx.ToString());
                Console.WriteLine(getArtistInfoEx);
                return null;
            }
        }

        public QopenAPI.Label getLabelInfo(string app_id, string label_id, string user_auth_token)
        {
            try
            {
                // Grab album info with auth
                outputText = null;
                Console.WriteLine("Getting Label Info...");
                QoLabel = QoService.LabelGetWithAuth(app_id, label_id, "albums", 500, 0, user_auth_token);
                return QoLabel;
            }
            catch (Exception getLabelInfoEx)
            {
                updateDownloadOutput("\r\n" + getLabelInfoEx.ToString());
                Console.WriteLine(getLabelInfoEx);
                return null;
            }
        }

        public Favorites getFavoritesInfo(string app_id, string user_id, string type, string user_auth_token)
        {
            try
            {
                // Grab album info with auth
                outputText = null;
                Console.WriteLine("Getting Favorites Info...");
                QoFavorites = QoService.FavoriteGetUserFavoritesWithAuth(app_id, user_id, type, 500, 0, user_auth_token);
                return QoFavorites;
            }
            catch (Exception getFavoritesInfoEx)
            {
                updateDownloadOutput("\r\n" + getFavoritesInfoEx.ToString());
                Console.WriteLine(getFavoritesInfoEx);
                return null;
            }
        }

        public Item getTrackInfoLabels(string app_id, string track_id, string user_auth_token)
        {
            try
            {
                // Grab album info with auth
                outputText = null;
                Console.WriteLine("Getting Track Info...");
                QoItem = QoService.TrackGetWithAuth(app_id, track_id, user_auth_token);
                string album_id = QoItem.Album.Id;
                QoAlbum = QoService.AlbumGetWithAuth(app_id, album_id, user_auth_token);
                return QoItem;
            }
            catch (Exception getTrackInfoLabelsEx)
            {
                updateDownloadOutput("\r\n" + getTrackInfoLabelsEx.ToString());
                Console.WriteLine(getTrackInfoLabelsEx);
                return null;
            }
        }

        public Album getAlbumInfoLabels(string app_id, string album_id, string user_auth_token)
        {
            try
            {
                // Grab album info with auth
                outputText = null;
                Console.WriteLine("Getting Album Info...");
                QoAlbum = QoService.AlbumGetWithAuth(app_id, album_id, user_auth_token);
                return QoAlbum;
            }
            catch (Exception getAlbumInfoLabelsEx)
            {
                updateDownloadOutput("\r\n" + getAlbumInfoLabelsEx.ToString());
                Console.WriteLine(getAlbumInfoLabelsEx);
                return null;
            }
        }

        public Playlist getPlaylistInfoLabels(string app_id, string playlist_id, string user_auth_token)
        {
            try
            {
                // Grab album info with auth
                outputText = null;
                Console.WriteLine("Getting Playlist Info...");
                QoPlaylist = QoService.PlaylistGetWithAuth(app_id, playlist_id, "tracks", 500, 0, user_auth_token);
                return QoPlaylist;
            }
            catch (Exception getPlaylistInfoLabelsEx)
            {
                updateDownloadOutput("\r\n" + getPlaylistInfoLabelsEx.ToString());
                Console.WriteLine(getPlaylistInfoLabelsEx);
                return null;
            }
        }
    }
}
