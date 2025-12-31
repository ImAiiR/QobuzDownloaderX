using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using QopenAPI;
using ZetaLongPaths;

using QobuzDownloaderX.Helpers;
using QobuzDownloaderX.Properties;

namespace QobuzDownloaderX
{
    // UNUSED
    // ======
    //
    //public static class StringExt
    //{
    //    public static string Truncate(this string value, int maxLength)
    //    {
    //        // This is currently unused, but leaving this here in case it needs to be used in the future.
    //        if (string.IsNullOrEmpty(value)) return value;
    //        return value.Length <= maxLength ? value : value.Substring(0, maxLength);
    //    }
    //}

    class DownloadAlbum
    {
        public Service QoService = new Service();
        public User QoUser = new User();
        public Album QoAlbum = new Album();
        public Item QoItem = new Item();
        public QopenAPI.Stream QoStream = new QopenAPI.Stream();

        public int paddedTrackLength { get; set; }
        public int paddedDiscLength { get; set; }

        public string downloadPath { get; set; }

        readonly PaddingNumbers padNumber = new PaddingNumbers();
        readonly DownloadTrack downloadTrack = new DownloadTrack();
        readonly DownloadFile downloadFile = new DownloadFile();
        readonly GetInfo getInfo = new GetInfo();

        private async Task DownloadArtwork(string downloadPath, Album album)
        {
            try
            {
                qbdlxForm._qbdlxForm.logger.Debug("Downloading artwork…");
                await downloadFile.DownloadArtwork(downloadPath, album);
                qbdlxForm._qbdlxForm.logger.Debug("Artwork download complete");
            }
            catch (Exception ex)
            {
                qbdlxForm._qbdlxForm.logger.Error($"Artwork download failed: {ex}");
                Debug.WriteLine("Unable to download artwork");
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

        private async Task DownloadGoodiesAsync(string downloadPath, Album album)
        {
            foreach (var goody in album.Goodies)
            {
                try
                {
                if (goody.FileFormatId == 52) continue; // Clip video, skip.

                if (goody.Url == null)
                {
                    qbdlxForm._qbdlxForm.logger.Warning("No URL found for the goody, skipping.");
                    getInfo.updateDownloadOutput($"\r\n{qbdlxForm._qbdlxForm.downloadOutputGoodyNoURL}\r\n");
                    continue;
                }

                await downloadFile.DownloadGoody(downloadPath, album, goody, getInfo);
                }
                catch
                {
                    qbdlxForm._qbdlxForm.logger.Warning("No goodies found or failed to download");
                }
            }
        }

        public async Task DownloadTracksAsync(string app_id, string album_id, string format_id, string audio_format, string user_auth_token, string app_secret, string downloadLocation, string artistTemplate, string albumTemplate, string trackTemplate, Album album, IProgress<int> progress, CancellationToken abortToken)
        {
            int totalTracks = album.Tracks.Items.Count;
            int trackIndex = 0;

            foreach (var item in album.Tracks.Items)
            {
                if (abortToken.IsCancellationRequested) { abortToken.ThrowIfCancellationRequested(); }
                if (qbdlxForm.skipCurrentAlbum) {
                    qbdlxForm.skipCurrentAlbum = false;
                    qbdlxForm._qbdlxForm.logger.Debug("Skipping current album by user");
                    getInfo.updateDownloadOutput(qbdlxForm._qbdlxForm.albumSkipped);
                    break; }

                trackIndex++;
                var trackProgress = new Progress<int>(value =>
                {
                    double trackPortion = 100.0 / totalTracks;
                    double scaledValue = (trackIndex - 1) * trackPortion + (value / 100.0) * trackPortion;
                    progress?.Report((int)Math.Round(scaledValue));
                });

                try
                {
                    var trackInfo = QoService.TrackGetWithAuth(app_id, item.Id.ToString(), user_auth_token);
                    await downloadTrack.DownloadTrackAsync(
                        "album", app_id, album_id, format_id, audio_format, 
                        user_auth_token, app_secret, downloadLocation, artistTemplate, albumTemplate, 
                        trackTemplate, album, trackInfo, trackProgress);
                }
                catch (Exception ex)
                {
                    qbdlxForm._qbdlxForm.logger.Error($"Track {item.Id} failed: {ex}");
                    getInfo.updateDownloadOutput($"\r\n{ex}");
                }
            }
            qbdlxForm.skipCurrentAlbum = false;
        }

        public async Task DownloadAlbumAsync(string app_id, string album_id, string format_id, string audio_format, string user_auth_token, string app_secret, string downloadLocation, string artistTemplate, string albumTemplate, string trackTemplate, Album QoAlbum, IProgress<int> progress, CancellationToken abortToken)
        {
            qbdlxForm._qbdlxForm.logger.Debug("Starting album download (downloadAlbum)");

            // Clear output text from DownloadTrack to avoid text from previous downloads sticking around.
            qbdlxForm._qbdlxForm.logger.Debug("Clearing output text");
            downloadTrack.clearOutputText();

            getInfo.outputText = null;

            if (QoAlbum.Streamable == false)
            {
                if (Settings.Default.streamableCheck == true)
                {
                    qbdlxForm._qbdlxForm.logger.Debug("Streamable tag is set to false on Qobuz, and streamable check is enabled, skipping download");
                    getInfo.updateDownloadOutput(qbdlxForm._qbdlxForm.downloadOutputAlNotStream);
                    getInfo.updateDownloadOutput("\r\n" + qbdlxForm._qbdlxForm.downloadOutputCompleted);
                    progress?.Report(100);
                    return;
                }
                else
                {
                    qbdlxForm._qbdlxForm.logger.Debug("Streamable tag is set to false on Qobuz, but streamable check is disabled, attempting download");
                }
            }

            if (abortToken.IsCancellationRequested) {abortToken.ThrowIfCancellationRequested();}
            try
            {
                // Find the how many characters are needed for padding
                paddedTrackLength = padNumber.padTracks(QoAlbum);
                paddedDiscLength = padNumber.padTracks(QoAlbum);

                // Set download path
                downloadPath = await downloadFile.createPath(downloadLocation, artistTemplate, albumTemplate, trackTemplate, null, null, paddedTrackLength, paddedDiscLength, QoAlbum, null, null);
                qbdlxForm._qbdlxForm.logger.Debug("Download path: " + downloadPath);

                // Download artwork
                await DownloadArtwork(downloadPath, QoAlbum);
                if (abortToken.IsCancellationRequested) { abortToken.ThrowIfCancellationRequested(); }

                // Download tracks
                await DownloadTracksAsync(
                    app_id, album_id, format_id, audio_format, user_auth_token, app_secret, downloadLocation, 
                    artistTemplate, albumTemplate, trackTemplate, QoAlbum, progress, abortToken);

                // Delete image used for embedding artwork
                if (abortToken.IsCancellationRequested) { abortToken.ThrowIfCancellationRequested(); }
                DeleteEmbeddedArtwork(downloadPath);

                // Set current output text
                getInfo.outputText = qbdlxForm._qbdlxForm.downloadOutput.Text;

                // Download goodies
                if (abortToken.IsCancellationRequested) { 
                    abortToken.ThrowIfCancellationRequested(); 
                } else
                {
                    if (Settings.Default.downloadGoodies)
                    {
                        await DownloadGoodiesAsync(downloadPath, QoAlbum);
                    }
                }

                progress?.Report(100);

                // Tell user that download is completed
                qbdlxForm._qbdlxForm.logger.Debug("All downloads completed!");

                // Write post template
                WritePostTemplate(QoAlbum);
            }
            catch (Exception downloadAlbumEx)
            {
                qbdlxForm._qbdlxForm.logger.Error("Error occured during downloadAlbum, error below:\r\n" + downloadAlbumEx);
                Debug.WriteLine(downloadAlbumEx);
                return;
            }
        }

        private void WritePostTemplate(Album album)
        {
            try
            {
                // Not useful at all for normal users, but I use it so… yeah
                qbdlxForm._qbdlxForm.logger.Debug("Writing post template…");
                var templateDate = DateTime.Parse(album.ReleaseDateOriginal).ToString("MMMM d, yyyy");
                ZlpIOHelper.WriteAllText("post_template.txt", String.Empty);

                using (StreamWriter sw = File.AppendText("post_template.txt"))
                {
                    sw.WriteLine("[center]");
                    sw.WriteLine($"[CoverArt]{album.Image.Large}[/CoverArt]");
                    sw.WriteLine($"[b]Release Date:[/b] {templateDate}");
                    sw.WriteLine($"[b]Genre:[/b] {album.Genre.Name.Replace("Alternativ und Indie", "Alternative").Replace("Hörbücher", "Comedy/Other")}");
                    sw.WriteLine("");
                    sw.WriteLine("[b]TRACKLIST[/b]");
                    sw.WriteLine("-----------------------------------");

                    foreach (var item in album.Tracks.Items)
                    {
                        sw.WriteLine(item.Version != null
                            ? $"{item.TrackNumber}. {item.Title.TrimEnd()} ({item.Version})"
                            : $"{item.TrackNumber}. {item.Title.TrimEnd()}");
                    }

                    sw.WriteLine("-----------------------------------");
                    sw.WriteLine("");
                    sw.WriteLine("[b]DOWNLOADS[/b]");
                    sw.WriteLine("-----------------------------------");
                    sw.WriteLine($"[spoiler={RenameTemplates.spacesRegex.Replace(album.Label.Name, " ")} / {album.UPC} / WEB]");
                    if (album.MaximumBitDepth > 16)
                    {
                        sw.WriteLine($"[format=FLAC / Lossless ({album.MaximumBitDepth}bit/{album.MaximumSamplingRate}kHz) / WEB]");
                        sw.WriteLine($"Uploaded by @{Settings.Default.postTemplateUsername}");
                        sw.WriteLine("");
                        sw.WriteLine("DOWNLOAD");
                        sw.WriteLine("REPLACE THIS WITH URL");
                        sw.WriteLine("[/format]");
                    }
                    sw.WriteLine("[format=FLAC / Lossless / WEB]");
                    sw.WriteLine($"Uploaded by @{Settings.Default.postTemplateUsername}");
                    sw.WriteLine("");
                    sw.WriteLine("DOWNLOAD");
                    sw.WriteLine("REPLACE THIS WITH URL");
                    sw.WriteLine("[/format]");
                    sw.WriteLine("[format=MP3 / 320 / WEB]");
                    sw.WriteLine($"Uploaded by @{Settings.Default.postTemplateUsername}");
                    sw.WriteLine("");
                    sw.WriteLine("DOWNLOAD");
                    sw.WriteLine("REPLACE THIS WITH URL");
                    sw.WriteLine("[/format]");
                    sw.WriteLine("[/spoiler]");
                    sw.WriteLine("-----------------------------------");
                    sw.WriteLine("");
                    sw.WriteLine("REPLACE WITH STREAM LINK");
                    sw.WriteLine("");
                    sw.WriteLine("[/center]");
                }
            }
            catch (Exception ex)
            {
                qbdlxForm._qbdlxForm.logger.Error($"Error writing post template: {ex}");
            }
        }
    }
}