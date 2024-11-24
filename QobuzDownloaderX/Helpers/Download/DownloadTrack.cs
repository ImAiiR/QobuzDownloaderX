using System;
using System.Threading.Tasks;
using QopenAPI;
using System.IO;
using QobuzDownloaderX.Properties;

namespace QobuzDownloaderX
{
    class DownloadTrack
    {
        public Service QoService = new Service();
        public User QoUser = new User();
        public Item QoItem = new Item();
        public QopenAPI.Stream QoStream = new QopenAPI.Stream();

        public int paddedTrackLength { get; set; }
        public int paddedDiscLength { get; set; }

        public string downloadPath { get; set; }
        public string filePath { get; set; }

        PaddingNumbers padNumber = new PaddingNumbers();
        DownloadFile downloadFile = new DownloadFile();
        RenameTemplates renameTemplates = new RenameTemplates();
        GetInfo getInfo = new GetInfo();
        FixMD5 fixMD5 = new FixMD5();

        public void clearOutputText() => getInfo.outputText = null;

        private bool VerifyStreamable(Item QoItem, int paddedTrackLength)
        {
            if (QoItem.Streamable || !Settings.Default.streamableCheck) return true;

            getInfo.updateDownloadOutput($"{qbdlxForm._qbdlxForm.downloadOutputTrNotStream.Replace("{TrackNumber}", QoItem.TrackNumber.ToString().PadLeft(paddedTrackLength, '0'))}\r\n");
            return false;
        }

        private bool CheckForExistingFile(string filePath, int paddedTrackLength, Item QoItem)
        {
            if (File.Exists(filePath))
            {
                getInfo.updateDownloadOutput($"{qbdlxForm._qbdlxForm.downloadOutputFileExists.Replace("{TrackNumber}", QoItem.TrackNumber.ToString().PadLeft(paddedTrackLength, '0'))}\r\n");
                return true;
            }
            return false;
        }

        private void CleanupArtwork(string downloadPath)
        {
            string artworkPath = downloadFile.artworkPath;
            if (File.Exists(artworkPath))
            {
                File.Delete(artworkPath);
            }
        }

        private void CreatePadding(Album QoAlbum, Playlist QoPlaylist)
        {
            if (QoPlaylist != null)
            {
                paddedTrackLength = padNumber.padPlaylistTracks(QoPlaylist);
                paddedDiscLength = 2;
            }
            else
            {
                paddedTrackLength = padNumber.padTracks(QoAlbum);
                paddedDiscLength = padNumber.padDiscs(QoAlbum);
            }
        }

        private async Task DownloadAndSaveTrack(string app_id, string format_id, string user_auth_token, string app_secret, Album QoAlbum, Item QoItem, Playlist QoPlaylist, string downloadPath, string filePath, string audio_format, int paddedTrackLength)
        {
            var QoStream = QoService.TrackGetFileUrl(QoItem.Id.ToString(), format_id, app_id, user_auth_token, app_secret);
            string streamURL = QoStream.StreamURL;

            // Display download status (depending on track number or playlist position number)
            var trackName = QoItem.Version == null ? QoItem.Title : $"{QoItem.Title.Trim()} ({QoItem.Version})";
            if (QoPlaylist == null) { getInfo.updateDownloadOutput($"{qbdlxForm._qbdlxForm.downloadOutputDownloading} - {QoItem.TrackNumber.ToString().PadLeft(paddedTrackLength, '0')} {trackName}..."); }
            else { getInfo.updateDownloadOutput($"{qbdlxForm._qbdlxForm.downloadOutputDownloading} - {QoItem.Position.ToString().PadLeft(paddedTrackLength, '0')} {trackName}..."); }

            // Download stream
            await downloadFile.DownloadStream(streamURL, downloadPath, filePath, audio_format, QoAlbum, QoItem);
            getInfo.updateDownloadOutput($" {qbdlxForm._qbdlxForm.downloadOutputDone}\r\n");
        }

        public async Task DownloadTrackAsync(string downloadType, string app_id, string album_id, string format_id, string audio_format, string user_auth_token, string app_secret, string downloadLocation, string artistTemplate, string albumTemplate, string trackTemplate, Album QoAlbum, Item QoItem)
        {
            // Empty output on main form if individual track download
            if (downloadType == "track") { getInfo.outputText = null; }
            
            try
            {
                // Create padding for tracks and possible multi-volume releases
                CreatePadding(QoAlbum, null);

                try
                {
                    // Get track info with auth if not there already
                    if (QoItem == null) { QoItem = QoService.TrackGetWithAuth(app_id, album_id, user_auth_token); }

                    // Verify Streamable
                    if (!VerifyStreamable(QoItem, paddedTrackLength)) return;

                    // Setting up download and file path
                    downloadPath = await downloadFile.createPath(downloadLocation, artistTemplate, albumTemplate, trackTemplate, null, null, paddedTrackLength, paddedDiscLength, QoAlbum, QoItem, null);
                    string trackTemplateConverted = renameTemplates.renameTemplates(trackTemplate, paddedTrackLength, paddedDiscLength, audio_format, QoAlbum, QoItem, null);

                    if (trackTemplateConverted.Contains(@"\"))
                    {
                        downloadPath = downloadPath + trackTemplateConverted.Substring(0, trackTemplateConverted.LastIndexOf(@"\")) + @"\";
                        trackTemplateConverted = trackTemplateConverted.Substring(trackTemplateConverted.LastIndexOf(@"\") + 1);
                        Console.WriteLine(downloadPath);
                    }

                    // Create subfolders for multi-volume releases
                    if (QoAlbum.MediaCount > 1)
                    {
                        filePath = downloadPath + "CD " + QoItem.MediaNumber.ToString().PadLeft(paddedDiscLength, '0') + Path.DirectorySeparatorChar + trackTemplateConverted.TrimEnd() + audio_format;
                    }
                    else
                    {
                        filePath = downloadPath + trackTemplateConverted.TrimEnd() + audio_format;
                    }

                    // Download cover art
                    try { await downloadFile.DownloadArtwork(downloadPath, QoAlbum); } catch { qbdlxForm._qbdlxForm.logger.Error("Failed to Download Cover Art"); }

                    // Check for Existing File
                    if (CheckForExistingFile(filePath, paddedTrackLength, QoItem)) return;

                    // Download and Save Track
                    await DownloadAndSaveTrack(app_id, format_id, user_auth_token, app_secret, QoAlbum, QoItem, null, downloadPath, filePath, audio_format, paddedTrackLength);
                }
                catch (Exception downloadAlbumEx)
                {
                    getInfo.updateDownloadOutput("\r\n\r\n" + downloadAlbumEx + "\r\n\r\n");
                    Console.WriteLine(downloadAlbumEx);
                    return;
                }
            }
            catch (Exception downloadAlbumEx)
            {
                Console.WriteLine(downloadAlbumEx);
                return;
            }
        }

        public async Task DownloadPlaylistTrackAsync(string app_id, string album_id, string format_id, string audio_format, string user_auth_token, string app_secret, string downloadLocation, string artistTemplate, string albumTemplate, string trackTemplate, string playlistTemplate, Album QoAlbum, Item QoItem, Playlist QoPlaylist)
        {
            try
            {
                // Create padding for tracks
                CreatePadding(null, QoPlaylist);

                // Verify Streamable
                if (!VerifyStreamable(QoItem, paddedTrackLength)) return;

                try
                {
                    // Setting up download and file path
                    downloadPath = await downloadFile.createPath(downloadLocation, null, null, trackTemplate, playlistTemplate, null, paddedTrackLength, paddedDiscLength, QoAlbum, QoItem, QoPlaylist);
                    string trackTemplateConverted = renameTemplates.renameTemplates(trackTemplate, paddedTrackLength, paddedDiscLength, audio_format, QoAlbum, QoItem, QoPlaylist);

                    if (trackTemplateConverted.Contains(@"\")) 
                    { 
                        downloadPath = downloadPath + trackTemplateConverted.Substring(0, trackTemplateConverted.LastIndexOf(@"\")) + @"\";
                        trackTemplateConverted = trackTemplateConverted.Substring(trackTemplateConverted.LastIndexOf(@"\") + 1);
                        Console.WriteLine(downloadPath);
                    }

                    filePath = downloadPath + trackTemplateConverted.TrimEnd() + audio_format;

                    // Check for Existing File
                    if (CheckForExistingFile(filePath, paddedTrackLength, QoItem)) return;

                    // Download cover art
                    try { await downloadFile.DownloadArtwork(downloadPath, QoAlbum); } catch { Console.WriteLine("Failed to Download Cover Art"); }

                    // Download and Save Track
                    await DownloadAndSaveTrack(app_id, format_id, user_auth_token, app_secret, QoAlbum, QoItem, QoPlaylist, downloadPath, filePath, audio_format, paddedTrackLength);

                    // Delete image used for embedded artwork
                    CleanupArtwork(downloadPath);
                }
                catch (Exception downloadAlbumEx)
                {
                    getInfo.updateDownloadOutput("\r\n\r\n" + downloadAlbumEx + "\r\n\r\n");
                    Console.WriteLine(downloadAlbumEx);
                    return;
                }

            }
            catch (Exception downloadAlbumEx)
            {
                Console.WriteLine(downloadAlbumEx);
                return;
            }
        }
    }
}
