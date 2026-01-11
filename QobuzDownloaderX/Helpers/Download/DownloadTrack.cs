using QobuzDownloaderX.Helpers;
using QobuzDownloaderX.Properties;
using QopenAPI;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using ZetaLongPaths;

namespace QobuzDownloaderX
{
    internal sealed class DownloadTrack
    {
        public Service QoService = new Service();
        public User QoUser = new User();
        public Item QoItem = new Item();
        public QopenAPI.Stream QoStream = new QopenAPI.Stream();

        public int paddedTrackLength { get; set; }
        public int paddedDiscLength { get; set; }

        public string downloadPath { get; set; }
        public string filePath { get; set; }

        readonly PaddingNumbers padNumber = new PaddingNumbers();
        readonly DownloadFile downloadFile = new DownloadFile();
        readonly RenameTemplates renameTemplates = new RenameTemplates();
        readonly GetInfo getInfo = new GetInfo();
        // readonly FixMD5 fixMD5 = new FixMD5(); // UNUSED

        public void clearOutputText() => getInfo.outputText = null;

        private bool VerifyStreamable(Item QoItem, int paddedTrackLength)
        {
            if (QoItem.Streamable || !Settings.Default.streamableCheck) return true;

            getInfo.updateDownloadOutput($"{qbdlxForm._qbdlxForm.downloadOutputTrNotStream.Replace("{TrackNumber}", QoItem.TrackNumber.ToString().PadLeft(paddedTrackLength, '0'))}\r\n");
            return false;
        }

        private bool CheckForExistingFile(string filePath, int paddedTrackLength, Item QoItem)
        {
            if (ZlpIOHelper.FileExists(filePath))
            {
                getInfo.updateDownloadOutput($"{qbdlxForm._qbdlxForm.downloadOutputFileExists.Replace("{TrackNumber}", QoItem.TrackNumber.ToString().PadLeft(paddedTrackLength, '0'))}\r\n");
                return true;
            }
            return false;
        }

        private void CleanupArtwork()
        {
            string artworkPath = downloadFile.artworkPath;
            if (ZlpIOHelper.FileExists(artworkPath))
            {
                ZlpIOHelper.DeleteFile(artworkPath);
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

        private async Task DownloadAndSaveTrack(string downloadType, string app_id, string format_id, string user_auth_token, string app_secret, Album QoAlbum, Item QoItem, Playlist QoPlaylist, string downloadPath, string filePath, string audio_format, int paddedTrackLength, DownloadStats stats, CancellationToken abortToken)
        {
            var QoStream = QoService.TrackGetFileUrl(QoItem.Id.ToString(), format_id, app_id, user_auth_token, app_secret);
            string streamURL = QoStream?.StreamURL;
            if (string.IsNullOrWhiteSpace(streamURL)) {
                getInfo.updateDownloadOutput($"{qbdlxForm._qbdlxForm.downloadOutputNoUrl.Replace("{TrackNumber}", QoItem.TrackNumber.ToString().PadLeft(paddedTrackLength, '0'))}\r\n");
                return;
            }

            // Display download status (depending on track number or playlist position number)
            string trackNameFormatted = QoItem.Version == null
                                        ? QoItem.Title
                                        : $"{QoItem.Title.TrimEnd()} ({QoItem.Version})";
            trackNameFormatted = RenameTemplates.repeatedParenthesesRegex.Replace(trackNameFormatted, "($1)");

            if (QoPlaylist == null) { getInfo.updateDownloadOutput($"{qbdlxForm._qbdlxForm.downloadOutputDownloading} - {QoItem.TrackNumber.ToString().PadLeft(paddedTrackLength, '0')} {trackNameFormatted}…"); }
            else { getInfo.updateDownloadOutput($"{qbdlxForm._qbdlxForm.downloadOutputDownloading} - {QoItem.Position.ToString().PadLeft(paddedTrackLength, '0')} {trackNameFormatted}…"); }

            if (abortToken.IsCancellationRequested) { abortToken.ThrowIfCancellationRequested(); }

            // Download stream
            await downloadFile.DownloadStream(downloadType, streamURL, downloadPath, filePath, audio_format, QoAlbum, QoItem, getInfo, abortToken, stats);
        }

        public async Task DownloadTrackAsync(string downloadType, string app_id, string album_id, string format_id, string audio_format, string user_auth_token, string app_secret, string downloadLocation, string artistTemplate, string albumTemplate, string trackTemplate, Album QoAlbum, Item QoItem, IProgress<int> progress, DownloadStats stats, CancellationToken abortToken)
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

                    // Check if QoItem is still null. If so, skip.
                    if (QoItem == null)
                    {
                        getInfo.updateDownloadOutput($"{qbdlxForm._qbdlxForm.downloadOutputAPIError}\r\n");
                        progress?.Report(100);
                        return;
                    }

                    // Verify Streamable
                    try { if (!VerifyStreamable(QoItem, paddedTrackLength)) { progress?.Report(100); return; } }
                    catch (Exception ex) { qbdlxForm._qbdlxForm.logger.Error($"Unable to verify if track is streamable. Error below:\r\n{ex}"); }

                    // Setting up download and file path
                    downloadPath = await downloadFile.createPath(downloadLocation, artistTemplate, albumTemplate, trackTemplate, null, null, paddedTrackLength, paddedDiscLength, QoAlbum, QoItem, null);

                    string trackTemplateConverted = renameTemplates.renameTemplates(trackTemplate, paddedTrackLength, paddedDiscLength, audio_format, QoAlbum, QoItem, null);

                    if (trackTemplateConverted.Contains(@"\"))
                    {
                        downloadPath = downloadPath + trackTemplateConverted.Substring(0, trackTemplateConverted.LastIndexOf(@"\")) + @"\";
                        trackTemplateConverted = trackTemplateConverted.Substring(trackTemplateConverted.LastIndexOf(@"\") + 1);
                        Debug.WriteLine(downloadPath);
                    }

                    // Create subfolders for multi-volume releases
                    if (!(downloadType == "track") && QoAlbum.MediaCount > 1)
                    {
                        filePath = downloadPath + "CD " + QoItem.MediaNumber.ToString().PadLeft(paddedDiscLength, '0') + ZlpPathHelper.DirectorySeparatorChar + trackTemplateConverted.TrimEnd() + audio_format;
                    }
                    else
                    {
                        filePath = downloadPath + trackTemplateConverted.TrimEnd() + audio_format;
                    }

                    // Download cover art
                    try { await downloadFile.DownloadArtwork(downloadPath, QoAlbum); } catch (Exception ex) { qbdlxForm._qbdlxForm.logger.Error($"Failed to Download Cover Art. Error below:\r\n{ex}"); }

                    // Check for Existing File
                    if (CheckForExistingFile(filePath, paddedTrackLength, QoItem))
                    {
                        progress?.Report(100);
                        return;
                    }

                    // Download and Save Track
                    await DownloadAndSaveTrack(downloadType, app_id, format_id, user_auth_token, app_secret, QoAlbum, QoItem, null, downloadPath, filePath, audio_format, paddedTrackLength, stats, abortToken);

                    // Delete image used for embedded artwork
                    if (abortToken.IsCancellationRequested) { abortToken.ThrowIfCancellationRequested(); }
                    DeleteEmbeddedArtwork(downloadPath);

                    progress?.Report(100);
                }
                catch (OperationCanceledException ex)
                {
                    if (!abortToken.IsCancellationRequested)
                    {
                        qbdlxForm._qbdlxForm.logger.Error("Download canceled: " + ex.Message);
                        throw;
                    }
                }
                catch (Exception downloadAlbumEx)
                {
                    getInfo.updateDownloadOutput("\r\n\r\n" + downloadAlbumEx + "\r\n\r\n");
                    Debug.WriteLine(downloadAlbumEx);
                    return;
                }
            }
            catch (Exception downloadAlbumEx)
            {
                Debug.WriteLine(downloadAlbumEx);
                return;
            }
        }

        public async Task DownloadPlaylistTrackAsync(string downloadType, string app_id, string format_id, string audio_format, string user_auth_token, string app_secret, string downloadLocation, string trackTemplate, string playlistTemplate, Album QoAlbum, Item QoItem, Playlist QoPlaylist, IProgress<int> progress, DownloadStats stats, CancellationToken abortToken)
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
                        Debug.WriteLine(downloadPath);
                    }

                    filePath = downloadPath + trackTemplateConverted.TrimEnd() + audio_format;

                    // Check for Existing File
                    if (CheckForExistingFile(filePath, paddedTrackLength, QoItem))
                    {
                        progress?.Report(100);
                        return;
                    }

                    // Download cover art
                    try
                    {
                        await downloadFile.DownloadArtwork(downloadPath, QoAlbum);
                    }
                    catch { }

                    // Download and Save Track
                    await DownloadAndSaveTrack(downloadType, app_id, format_id, user_auth_token, app_secret, QoAlbum, QoItem, QoPlaylist, downloadPath, filePath, audio_format, paddedTrackLength, stats, abortToken);

                    // Delete image used for embedded artwork
                    CleanupArtwork();
                    progress?.Report(100);
                }
                catch (Exception downloadAlbumEx)
                {
                    getInfo.updateDownloadOutput("\r\n\r\n" + downloadAlbumEx + "\r\n\r\n");
                    Debug.WriteLine(downloadAlbumEx);
                    return;
                }

            }
            catch (Exception downloadAlbumEx)
            {
                Debug.WriteLine(downloadAlbumEx);
                return;
            }
        }

        private void DeleteEmbeddedArtwork(string downloadPath)
        {
            try
            {
                qbdlxForm._qbdlxForm.logger.Debug("Deleting embedded artwork…");
                ZlpIOHelper.DeleteFile($"{downloadPath}{qbdlxForm._qbdlxForm.embeddedArtSize}.jpg");
            }
            catch
            {
                qbdlxForm._qbdlxForm.logger.Warning("Unable to delete embedded artwork");
            }
        }
    }

}
