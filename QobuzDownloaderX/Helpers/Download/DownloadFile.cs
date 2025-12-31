using QobuzDownloaderX.Helpers;
using QobuzDownloaderX.Properties;
using QopenAPI;
using System;
using System.ComponentModel;
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
    class DownloadFile
    {
        readonly RenameTemplates renameTemplates = new RenameTemplates();
        readonly PaddingNumbers paddingNumbers = new PaddingNumbers();
        readonly FixMD5 fixMD5 = new FixMD5();

        // Static flag to ensure temp directory check runs only once per application run
        private static bool tempDirChecked = false;

        public string artworkPath { get; set; }

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

        public async Task DownloadStream(string streamUrl, string downloadPath, string filePath, string audio_format, Album QoAlbum, Item QoItem, GetInfo getInfo)
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

            qbdlxForm._qbdlxForm.BeginInvoke(new Action(() => { qbdlxForm._qbdlxForm.progressLabel.Text = $"{qbdlxForm._qbdlxForm.progressLabelActive} - {0}%"; }));

            // Set path for downloaded artwork
            artworkPath = downloadPath + qbdlxForm._qbdlxForm.embeddedArtSize + @".jpg";
            qbdlxForm._qbdlxForm.logger.Debug("Artwork path: " + artworkPath);

            // Handle subfolders if more than 1 volume
            string finalDownloadPath = downloadPath;
            if (QoAlbum.MediaCount > 1)
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
                using (var httpClient = new HttpClient { Timeout = TimeSpan.FromMinutes(5) })
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
                    int bufferLength = 81920; // 80 kb. Stream.cs: const int DefaultCopyBufferSize = 81920
                    byte[] buffer = new byte[bufferLength];
                    DateTime lastUpdateTime = DateTime.Now;
                    long previousBytesRead = 0;

                    using (var fs = new FileStream(tempFile, FileMode.Create, FileAccess.Write, FileShare.Read, bufferLength, useAsync:true))
                    using (var stream = await response.Content.ReadAsStreamAsync())
                    using (var cts = new CancellationTokenSource(TimeSpan.FromSeconds(60)))
                    {
                        int bytesRead;
                        while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length, cts.Token)) > 0)
                        {
                            await fs.WriteAsync(buffer, 0, bytesRead);
                            totalBytesRead += bytesRead;

                            int progressPercentage = totalBytes > 0 ? (int)(totalBytesRead * 100L / totalBytes) : -1;

                            if (qbdlxForm._qbdlxForm.downloadSpeedCheckbox.Checked)
                            {
                                DateTime currentTime = DateTime.Now;
                                double timeDiff = (currentTime - lastUpdateTime).TotalSeconds;

                                if (timeDiff > 0)
                                {
                                    long bytesDiff = totalBytesRead - previousBytesRead;
                                    double speed = bytesDiff / timeDiff;
                                    string speedText = speed > 1024 * 1024
                                        ? $"{speed / (1024 * 1024):F2} MB/s"
                                        : $"{speed / 1024:F2} KB/s";

                                    qbdlxForm._qbdlxForm.BeginInvoke(new Action(() =>
                                        qbdlxForm._qbdlxForm.progressLabel.Text = $"{qbdlxForm._qbdlxForm.progressLabelActive} - {progressPercentage}% [{speedText}]"));

                                    previousBytesRead = totalBytesRead;
                                    lastUpdateTime = currentTime;
                                }
                            }
                            else
                            {
                                qbdlxForm._qbdlxForm.BeginInvoke(new Action(() =>
                                    qbdlxForm._qbdlxForm.progressLabel.Text = $"{qbdlxForm._qbdlxForm.progressLabelActive} - {progressPercentage}%"));
                            }
                        }
                    }
                }

                if (Settings.Default.fixMD5s && audio_format.Contains("flac"))
                {
                    qbdlxForm._qbdlxForm.logger.Debug("Attempting to fix unset MD5s…");
                    fixMD5.fixMD5(tempFile, "flac");
                }

                qbdlxForm._qbdlxForm.logger.Debug("Starting file metadata tagging");
                TagFile.WriteToFile(tempFile, artworkPath, QoAlbum, QoItem);

                // Move the file to final destination
                qbdlxForm._qbdlxForm.logger.Debug("Moving temp file to - " + filePath);
                ZlpIOHelper.MoveFile(tempFile, filePath);

                qbdlxForm._qbdlxForm.logger.Debug("Download complete.");
                getInfo.updateDownloadOutput($" {qbdlxForm._qbdlxForm.downloadOutputDone}\r\n");
            }
            catch (TaskCanceledException ex)
            {
                qbdlxForm._qbdlxForm.logger.Error("Download timed out or cancelled: " + ex.Message);
                throw;
            }
            catch (WebException webEx)
            {
                getInfo.updateDownloadOutput($" Download failed: {webEx.Message}\r\n");
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
                qbdlxForm._qbdlxForm.logger.Error("Download failed: " + ex.Message);
                throw;
            }
        }

        public async Task DownloadArtwork(string downloadPath, Album QoAlbum)
        {
            if (qbdlxForm._qbdlxForm.savedArtSize == "0") return; 

            using (var client = new WebClient())
            {
                // Download cover art (600x600) to the download path
                qbdlxForm._qbdlxForm.logger.Debug("Downloading Cover Art");
                ZlpIOHelper.CreateDirectory(ZlpPathHelper.GetDirectoryPathNameFromFilePath(downloadPath));

                if (!ZlpIOHelper.FileExists(downloadPath + @"Cover.jpg"))
                {
                    qbdlxForm._qbdlxForm.logger.Debug("Saved artwork Cover.jpg not found, downloading");
                    try { await client.DownloadFileTaskAsync(QoAlbum.Image.Large.Replace("_600", "_" + qbdlxForm._qbdlxForm.savedArtSize), ZlpPathHelper.GetFullPath(downloadPath + @"Cover.jpg")); } catch (Exception ex) { qbdlxForm._qbdlxForm.logger.Error($"Failed to download cover art. Error below:\r\n{ex}"); }
                }
                if (!ZlpIOHelper.FileExists(downloadPath + qbdlxForm._qbdlxForm.embeddedArtSize + @".jpg"))
                {
                    qbdlxForm._qbdlxForm.logger.Debug("Saved artwork for embedding not found, downloading");
                    try { await client.DownloadFileTaskAsync(QoAlbum.Image.Large.Replace("_600", "_" + qbdlxForm._qbdlxForm.embeddedArtSize), ZlpPathHelper.GetFullPath(downloadPath + qbdlxForm._qbdlxForm.embeddedArtSize + @".jpg")); } catch (Exception ex) { qbdlxForm._qbdlxForm.logger.Error($"Failed to download cover art. Error below:\r\n{ex}"); }
                }
            }
        }

        public async Task DownloadGoody(string downloadPath, Album QoAlbum, Goody QoGoody, GetInfo getInfo)
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
                using (var http = new HttpClient() { Timeout = TimeSpan.FromMinutes(5) })
                {
                    getInfo.updateDownloadOutput($"{qbdlxForm._qbdlxForm.downloadOutputGoodyFound} ");
                    qbdlxForm._qbdlxForm.logger.Debug("Downloading goody…");

                    int bufferLength = 81920; // 80 kb. Stream.cs: const int DefaultCopyBufferSize = 81920
                    using (var response = await http.GetAsync(QoGoody.Url, HttpCompletionOption.ResponseHeadersRead))
                    {
                        response.EnsureSuccessStatusCode();

                        using (var httpStream = await response.Content.ReadAsStreamAsync())
                        using (var fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.Read))
                        using (var cts = new CancellationTokenSource(TimeSpan.FromSeconds(50)))
                        {
                            await httpStream.CopyToAsync(fileStream, bufferLength, cts.Token);
                        }
                    }
                }

                qbdlxForm._qbdlxForm.logger.Debug("Goody download complete");
                getInfo.updateDownloadOutput($"{qbdlxForm._qbdlxForm.downloadOutputDone}\r\n");
            }
            catch (Exception ex)
            {
                qbdlxForm._qbdlxForm.logger.Error($"Error downloading goody: {ex.Message}");
                getInfo.updateDownloadOutput("ERROR: " + ex.Message + "\r\n");
            }
        }
    }
}
