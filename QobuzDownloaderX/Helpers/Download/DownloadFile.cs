using QobuzDownloaderX.Helpers;
using QobuzDownloaderX.Properties;
using QopenAPI;
using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZetaLongPaths;

namespace QobuzDownloaderX
{
    internal sealed class DownloadFile
    {
        private readonly RenameTemplates renameTemplates = new RenameTemplates();
        private readonly PaddingNumbers paddingNumbers = new PaddingNumbers();
        private readonly FixMD5 fixMD5 = new FixMD5();

        private readonly TimeSpan artworkDownloadCompletionTimeout = TimeSpan.FromMinutes(2);
        private readonly TimeSpan trackDownloadCompletionTimeout = TimeSpan.FromMinutes(10);
        private readonly TimeSpan goodyDownloadCompletionTimeout = TimeSpan.FromMinutes(5);
        private readonly TimeSpan dataReceiveTimeout = TimeSpan.FromMinutes(1);

        // Static flag to ensure temp directory check runs only once per application run
        private static bool tempDirChecked = false;

        public string embeddedArtworkPath { get; set; }

        [SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "I don’t feel like changing this and it doesn’t matter")]
        public async Task<string> createPath(string downloadLocation, string artistTemplate, string albumTemplate, string trackTemplate, string playlistTemplate, string favoritesTemplate, int paddedTrackLength, int paddedDiscLength, Album QoAlbum, Item QoItem, Playlist QoPlaylist)
        {
            return await Task.Run(() =>
            {
                string downloadPath;
                if (QoPlaylist == null)
                {
                    qbdlxForm._qbdlxForm.logger.Debug("Using non-playlist path");
                    string artistTemplateConverted = renameTemplates.renameTemplates(artistTemplate, paddedTrackLength, paddedDiscLength, qbdlxForm._qbdlxForm.audio_format, QoAlbum, null, null);
                    string albumTemplateConverted = renameTemplates.renameTemplates(albumTemplate, paddedTrackLength, paddedDiscLength, qbdlxForm._qbdlxForm.audio_format, QoAlbum, null, null);
                    downloadPath = ZlpPathHelper.Combine(downloadLocation, artistTemplateConverted, albumTemplateConverted.TrimEnd(ZlpPathHelper.DirectorySeparatorChar) + ZlpPathHelper.DirectorySeparatorChar);                    
                }
                else
                {
                    qbdlxForm._qbdlxForm.logger.Debug("Using playlist path");
                    string playlistTemplateConverted = renameTemplates.renameTemplates(playlistTemplate, paddedTrackLength, paddedDiscLength, qbdlxForm._qbdlxForm.audio_format, QoAlbum, QoItem, QoPlaylist);
                    downloadPath = ZlpPathHelper.Combine(downloadLocation, playlistTemplateConverted.TrimEnd(ZlpPathHelper.DirectorySeparatorChar) + ZlpPathHelper.DirectorySeparatorChar);
                }
                downloadPath = RenameTemplates.spacesRegex.Replace(downloadPath, " "); // Remove double spaces
                return downloadPath;
            });
        }

        public async Task DownloadStream(string downloadType, string streamUrl, string downloadPath, string filePath, string audio_format, Album QoAlbum, Item QoItem, GetInfo getInfo, CancellationToken abortToken, DownloadStats stats)
        {
            const string tempDir = @"qbdlx-temp";
            string tempFile = ZlpPathHelper.Combine(tempDir, $"qbdlx_downloading-{QoItem.Id}{audio_format}");

            // Ensure temp directory exists
            if (!ZlpIOHelper.DirectoryExists(tempDir))
            {
                qbdlxForm._qbdlxForm.logger.Debug($"[DownloadStream] Temp directory does not exist, creating: {tempDir}");
                ZlpIOHelper.CreateDirectory(tempDir);
            }

            // Check write permission -only once per session- by creating an empty test file
            if (!tempDirChecked)
            {
                string testFile = ZlpPathHelper.Combine(tempDir, "test.tmp");
                try
                {
                    File.WriteAllText(testFile, "test");
                    File.Delete(testFile);
                    tempDirChecked = true;
                    qbdlxForm._qbdlxForm.logger.Debug("Temp directory is writable.");
                }
                catch (Exception ex)
                {
                    qbdlxForm._qbdlxForm.logger.Error("Temp directory is not writable: " + ex.Message);
                    getInfo.updateDownloadOutput($"ERROR: Temp directory '{tempDir}' is not writable: {ex.Message}\r\n");
                    throw;
                }
            }

            qbdlxForm._qbdlxForm.logger.Debug($"Writing temp file to {tempFile}");

            if (qbdlxForm._qbdlxForm.downloadSpeedCheckbox.Checked && stats?.SpeedWatch != null && !string.IsNullOrEmpty(stats.LastSpeedText))
            {
                qbdlxForm._qbdlxForm.BeginInvoke(new Action(() => { qbdlxForm._qbdlxForm.progressLabel.Text = $"{qbdlxForm._qbdlxForm.progressLabelActive} - 0% [{stats.LastSpeedText}]"; }));
            }
            else
            {
                qbdlxForm._qbdlxForm.BeginInvoke(new Action(() => { qbdlxForm._qbdlxForm.progressLabel.Text = $"{qbdlxForm._qbdlxForm.progressLabelActive} - 0%"; }));
            }

            // Set path for downloaded embedded artwork
            embeddedArtworkPath = Path.Combine(Path.GetTempPath(), qbdlxForm._qbdlxForm.embeddedArtSize + ".jpg");
            qbdlxForm._qbdlxForm.logger.Debug("Embedded artwork path: " + embeddedArtworkPath);

            // Handle subfolders if more than 1 volume
            string finalDownloadPath = downloadPath;
            if (QoItem.PlaylistTrackId is null && !(downloadType=="track") && QoAlbum.MediaCount > 1)
            {
                qbdlxForm._qbdlxForm.logger.Debug("More than 1 volume, using subfolders for each volume");
                finalDownloadPath = ZlpPathHelper.Combine(downloadPath, "CD " + QoItem.MediaNumber.ToString().PadLeft(paddingNumbers.padDiscs(QoAlbum), '0'));
                ZlpIOHelper.CreateDirectory(finalDownloadPath);
            }
            else
            {
                ZlpIOHelper.CreateDirectory(ZlpPathHelper.GetDirectoryPathNameFromFilePath(downloadPath));
            }

            try
            {
                qbdlxForm._qbdlxForm.logger.Debug("Stream URL: " + streamUrl);
                using (var httpClient = new HttpClient { Timeout = trackDownloadCompletionTimeout })
                using (var response = await httpClient.GetAsync(streamUrl, HttpCompletionOption.ResponseHeadersRead))
                {
                    response.EnsureSuccessStatusCode();

                    long totalBytes = response.Content.Headers.ContentLength ?? -1;
                    qbdlxForm._qbdlxForm.logger.Debug($"HTTP Status Code: {(int)response.StatusCode} ({response.StatusCode})");
                    qbdlxForm._qbdlxForm.logger.Debug($"Responde Headers: {string.Join("; ", response.Content.Headers.Select(h => $"{h.Key}={string.Join(",", h.Value)}"))}");

                    if (totalBytes < 1)
                    {
                        throw new WebException($"response.Content.Headers.ContentLength: {totalBytes}");
                    }

                    long totalBytesRead = 0;
                    int bufferLength = 81920; // 80 kb - Stream.cs: const int DefaultCopyBufferSize = 81920
                    byte[] buffer = new byte[bufferLength];

                    // Open file stream for writing and get the HTTP response stream
                    using (var fs = new FileStream(tempFile, FileMode.Create, FileAccess.Write, FileShare.Read, bufferLength, useAsync: true))
                    using (var stream = await response.Content.ReadAsStreamAsync())
                    using (var downloadTimeoutCts = new CancellationTokenSource(trackDownloadCompletionTimeout)) // Total download timeout
                    {
                        if (stats?.SpeedWatch != null && !stats.SpeedWatch.IsRunning) stats.SpeedWatch.Start();
                       
                        int bytesRead;
                        while (true)
                        {
                            if (abortToken.IsCancellationRequested) { abortToken.ThrowIfCancellationRequested(); }

                            using (var readTimeoutCts = new CancellationTokenSource(dataReceiveTimeout)) // Timeout if no data is received
                            using (var readTimeoutLinkedCts = CancellationTokenSource.CreateLinkedTokenSource(downloadTimeoutCts.Token, readTimeoutCts.Token))
                            {
                                // Read from the stream with the linked token (total + inactivity timeout)
                                stats?.SpeedWatch?.Stop();
                                bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length, readTimeoutLinkedCts.Token);
                                if (bytesRead == 0) break; // End of stream

                                if (stats?.SpeedWatch != null && !stats.SpeedWatch.IsRunning) stats.SpeedWatch.Start();

                                // Write the bytes to the file stream using the same linked token
                                await fs.WriteAsync(buffer, 0, bytesRead, readTimeoutLinkedCts.Token);
                                totalBytesRead += bytesRead;

                                if (stats != null)
                                {

                                    // accumulate across multiple DownloadStream calls
                                    stats.CumulativeBytesRead += bytesRead;
                                }

                                int progressPercentage = (int)(totalBytesRead * 100L / totalBytes);

                                if (qbdlxForm._qbdlxForm.downloadSpeedCheckbox.Checked && stats?.SpeedWatch != null && stats.SpeedWatch.IsRunning)
                                {
                                    long elapsedMs = stats.SpeedWatch.ElapsedMilliseconds;

                                    if (elapsedMs - stats.LastUiTimeMs >= 250)
                                    {
                                        long bytesDelta = stats.CumulativeBytesRead - stats.LastUiBytes;
                                        double seconds = (elapsedMs - stats.LastUiTimeMs) / 1000.0;
                                        double speed = seconds > 0 ? bytesDelta / seconds : 0.0;

                                        string speedText = speed >= 1024 * 1024
                                            ? $"{speed / (1024 * 1024):F2} MB/s"
                                            : $"{speed / 1024:F2} KB/s";

                                        stats.LastUiBytes = stats.CumulativeBytesRead;
                                        stats.LastUiTimeMs = elapsedMs;

                                        stats.LastSpeedText = speedText;
                                        qbdlxForm._qbdlxForm.BeginInvoke(new Action(() =>
                                            qbdlxForm._qbdlxForm.progressLabel.Text = $"{qbdlxForm._qbdlxForm.progressLabelActive} - {progressPercentage}% [{stats.LastSpeedText}]"));
                                    }
                                    else
                                    {
                                        qbdlxForm._qbdlxForm.BeginInvoke(new Action(() =>
                                            qbdlxForm._qbdlxForm.progressLabel.Text =
                                                $"{qbdlxForm._qbdlxForm.progressLabelActive} - {progressPercentage}% [{stats.LastSpeedText}]"));
                                    }
                                }
                                else
                                {
                                    qbdlxForm._qbdlxForm.BeginInvoke(new Action(() =>
                                        qbdlxForm._qbdlxForm.progressLabel.Text =
                                            $"{qbdlxForm._qbdlxForm.progressLabelActive} - {progressPercentage}%"));
                                }
                            } 
                        }
                        stats?.SpeedWatch?.Stop();
                    }
                }

                if (Settings.Default.fixMD5s && audio_format.Contains("flac"))
                {
                    qbdlxForm._qbdlxForm.logger.Debug("Attempting to fix unset MD5s…");
                    fixMD5.fixMD5(tempFile, "flac");
                }

                qbdlxForm._qbdlxForm.logger.Debug("Starting file metadata tagging");
                TagFile.WriteToFile(tempFile, embeddedArtworkPath, QoAlbum, QoItem);

                // Move the file to final destination
                qbdlxForm._qbdlxForm.logger.Debug("Moving temp file to - " + filePath);
                ZlpIOHelper.MoveFile(tempFile, filePath);

                qbdlxForm._qbdlxForm.logger.Debug("DownloadStream complete.");
                getInfo.updateDownloadOutput($" {qbdlxForm._qbdlxForm.downloadOutputDone}\r\n");
            }
            catch (TaskCanceledException ex)
            {
                qbdlxForm._qbdlxForm.logger.Error("DownloadStream timed out (TaskCanceledException): " + ex.Message);
                throw;

            }
            catch (OperationCanceledException ex)
            {
                if (!abortToken.IsCancellationRequested)
                {
                    qbdlxForm._qbdlxForm.logger.Error("DownloadStream canceled (OperationCanceledException): " + ex.Message);
                    throw;
                }
            }
            catch (WebException webEx)
            {
                getInfo.updateDownloadOutput($"DownloadStream failed (WebException): {webEx.Message}\r\n");
                qbdlxForm._qbdlxForm.logger.Error("WebException caught: " + webEx.Message);
            }
            catch (Exception ex) when (
                  (ex is IOException ioEx && ioEx.HResult == unchecked((int)0x80070070)) ||
                  (ex is Win32Exception winEx && winEx.HResult == unchecked((int)0x80004005))) // Error 112
            {
                string selectedPath = !ZlpIOHelper.FileExists(filePath) ? filePath : tempFile;
                string pathRoot = Path.GetPathRoot(selectedPath);
                MessageBox.Show(qbdlxForm._qbdlxForm.languageManager.GetTranslation("notEnoughFreeSpaceMsg").Replace("{pathRoot}", pathRoot), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                qbdlxForm._qbdlxForm.logger.Error($"Not enough free space on drive '{pathRoot}': " + ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                qbdlxForm._qbdlxForm.logger.Error("DownloadStream failed (Exception): " + ex.Message);
                throw;
            }
        }

        public async Task DownloadArtwork(string downloadPath, Album QoAlbum)
        {
            // Download cover art to the download path
            using (var httpClient = new HttpClient { Timeout = artworkDownloadCompletionTimeout })
            using (var downloadTimeoutCts = new CancellationTokenSource(artworkDownloadCompletionTimeout))
            {
                qbdlxForm._qbdlxForm.logger.Debug("Downloading Cover Art");
                ZlpIOHelper.CreateDirectory(ZlpPathHelper.GetDirectoryPathNameFromFilePath(downloadPath));

                // Helper local function for downloading with dual timeouts
                async Task DownloadWithTimeoutAsync(string url, string destinationPath)
                {
                    try
                    {
                        using (var response = await httpClient.GetAsync(
                            url,
                            HttpCompletionOption.ResponseHeadersRead,
                            downloadTimeoutCts.Token))
                        {
                            response.EnsureSuccessStatusCode();

                            byte[] buffer = new byte[81920]; // 80 kb - Stream.cs: const int DefaultCopyBufferSize = 81920
                            using (var httpStream = await response.Content.ReadAsStreamAsync())
                            using (var fs = new FileStream(destinationPath, FileMode.Create, FileAccess.Write, FileShare.Read))
                            {
                                int bytesRead;

                                while (true)
                                {
                                    using (var readTimeoutCts = new CancellationTokenSource(dataReceiveTimeout))
                                    using (var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(
                                        downloadTimeoutCts.Token,
                                        readTimeoutCts.Token))
                                    {
                                        bytesRead = await httpStream.ReadAsync(buffer, 0, buffer.Length, linkedCts.Token);
                                        if (bytesRead <= 0)
                                            break;

                                        await fs.WriteAsync(buffer, 0, bytesRead, linkedCts.Token);
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        qbdlxForm._qbdlxForm.logger.Error($"Error downloading cover art ({url}). Error:\r\n{ex}");
                        throw;
                    }
                }

                if (!Settings.Default.dontSaveArtworkToDisk)
                {
                    // Cover.jpg
                    if (!ZlpIOHelper.FileExists(downloadPath + @"Cover.jpg"))
                    {
                        qbdlxForm._qbdlxForm.logger.Debug("Saved artwork Cover.jpg not found, downloading");
                        string url = QoAlbum.Image.Large.Replace("_600", "_" + qbdlxForm._qbdlxForm.savedArtSize);
                        string dest = ZlpPathHelper.GetFullPath(downloadPath + @"Cover.jpg");
                        await DownloadWithTimeoutAsync(url, dest);
                    }
                }

                // Embedded art (TEMP directory)
                embeddedArtworkPath = Path.Combine(Path.GetTempPath(), qbdlxForm._qbdlxForm.embeddedArtSize + ".jpg");
                if (!File.Exists(embeddedArtworkPath))
                {
                    qbdlxForm._qbdlxForm.logger.Debug("Embedded artwork not found in temp directory, downloading");

                    string url = QoAlbum.Image.Large.Replace("_600", "_" + qbdlxForm._qbdlxForm.embeddedArtSize);
                    await DownloadWithTimeoutAsync(url, embeddedArtworkPath);
                }
            }
        }

        public async Task DownloadGoody(string downloadPath, Album QoAlbum, Goody QoGoody, GetInfo getInfo, CancellationToken abortToken)
        {
            ZlpIOHelper.CreateDirectory(ZlpPathHelper.GetDirectoryPathNameFromFilePath(downloadPath));

            string fileName = downloadPath
                              + renameTemplates.GetSafeFilename(QoGoody.Description ?? QoAlbum.Title)
                              + " (" + QoGoody.Id + $"){Path.GetExtension(QoGoody.Url)}";

            if (ZlpIOHelper.FileExists(fileName))
            {
                qbdlxForm._qbdlxForm.logger.Debug("Goody file already exists: " + fileName);
                getInfo.updateDownloadOutput($"{qbdlxForm._qbdlxForm.languageManager.GetTranslation("downloadOutputGoodyExists")}\r\n");
                return;
            }

            try
            {
                using (var httpClient = new HttpClient { Timeout = goodyDownloadCompletionTimeout })
                {
                    // Global download timeout (total operation)
                    using (var downloadTimeoutCts = new CancellationTokenSource(goodyDownloadCompletionTimeout))
                    {
                        getInfo.updateDownloadOutput($"{qbdlxForm._qbdlxForm.downloadOutputGoodyFound} ");
                        qbdlxForm._qbdlxForm.logger.Debug("Downloading goody…");

                        int bufferLength = 81920; // 80 kb - Stream.cs: const int DefaultCopyBufferSize = 81920
                        var buffer = new byte[bufferLength];

                        using (var response = await httpClient.GetAsync(
                            QoGoody.Url,
                            HttpCompletionOption.ResponseHeadersRead,
                            downloadTimeoutCts.Token))
                        {
                            response.EnsureSuccessStatusCode();

                            using (var httpStream = await response.Content.ReadAsStreamAsync())
                            using (var fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.Read))
                            {
                                int bytesRead;

                                while (true)
                                {
                                    // Per-read inactivity timeout (no data received)
                                    using (var readTimeoutCts = new CancellationTokenSource(dataReceiveTimeout))
                                    using (var readTimeoutLinkedCts = CancellationTokenSource.CreateLinkedTokenSource(
                                        downloadTimeoutCts.Token,
                                        readTimeoutCts.Token))
                                    {
                                        bytesRead = await httpStream.ReadAsync(buffer, 0, buffer.Length, readTimeoutLinkedCts.Token);
                                        if (bytesRead <= 0)
                                            break;

                                        await fileStream.WriteAsync(buffer, 0, bytesRead, readTimeoutLinkedCts.Token);
                                    }
                                }
                            }
                        }
                    }
                }

                qbdlxForm._qbdlxForm.logger.Debug("Goody download complete");
                getInfo.updateDownloadOutput($"{qbdlxForm._qbdlxForm.downloadOutputDone}\r\n");
            }
            catch (TaskCanceledException ex)
            {
                getInfo.updateDownloadOutput($"Download has timed out (TaskCanceledException).");
                qbdlxForm._qbdlxForm.logger.Error("Download has timed out (TaskCanceledException): " + ex.Message);
                throw;

            }
            catch (OperationCanceledException ex)
            {
                if (!abortToken.IsCancellationRequested)
                {
                    getInfo.updateDownloadOutput($"ERROR: {ex.Message}\r\n");
                    qbdlxForm._qbdlxForm.logger.Error("Error downloading goody (OperationCanceledException): " + ex.Message);
                    throw;
                }
            }
            catch (WebException webEx)
            {
                getInfo.updateDownloadOutput($"ERROR: {webEx.Message}\r\n");
                qbdlxForm._qbdlxForm.logger.Error("Error downloading goody (WebException): " + webEx.Message);
            }
            catch (Exception ex)
            {
                getInfo.updateDownloadOutput("ERROR: " + ex.Message + "\r\n");
                qbdlxForm._qbdlxForm.logger.Error($"Error downloading goody (Exception): {ex.Message}");
            }
        }
    }
}
