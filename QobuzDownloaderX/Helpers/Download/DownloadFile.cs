using QobuzDownloaderX.Properties;
using QopenAPI;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using ZetaLongPaths;

using QobuzDownloaderX.Helpers;

namespace QobuzDownloaderX
{
    class DownloadFile
    {
        readonly RenameTemplates renameTemplates = new RenameTemplates();
        readonly PaddingNumbers paddingNumbers = new PaddingNumbers();
        readonly FixMD5 fixMD5 = new FixMD5();

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
                    downloadPath = Regex.Replace(downloadPath, @"\s+", " "); // Remove double spaces
                    
                }
                else
                {
                    qbdlxForm._qbdlxForm.logger.Debug("Using playlist path");
                    string playlistTemplateConverted = renameTemplates.renameTemplates(playlistTemplate, paddedTrackLength, paddedDiscLength, qbdlxForm._qbdlxForm.audio_format, QoAlbum, QoItem, QoPlaylist);
                    downloadPath = ZlpPathHelper.Combine(downloadLocation, playlistTemplateConverted.TrimEnd(ZlpPathHelper.DirectorySeparatorChar) + ZlpPathHelper.DirectorySeparatorChar);
                    downloadPath = Regex.Replace(downloadPath, @"\s+", " "); // Remove double spaces
                }
                return downloadPath;
            });
        }

        public async Task DownloadStream(string streamUrl, string downloadPath, string filePath, string audio_format, Album QoAlbum, Item QoItem)
        {
            qbdlxForm._qbdlxForm.logger.Debug("Writing temp file to qbdlx-temp/qbdlx_downloading-" + QoItem.Id.ToString() + audio_format);

            // Create a temp directory inside the exe location
            string tempFile = ZlpPathHelper.Combine(@"qbdlx-temp", "qbdlx_downloading-" + QoItem.Id.ToString() + audio_format);
            ZlpIOHelper.CreateDirectory(@"qbdlx-temp");

            qbdlxForm._qbdlxForm.BeginInvoke(new Action(() => { qbdlxForm._qbdlxForm.progressLabel.Text = $"{qbdlxForm._qbdlxForm.progressLabelActive} - {0}%"; }));

            // Set path for downloaded artwork
            artworkPath = downloadPath + qbdlxForm._qbdlxForm.embeddedArtSize + @".jpg";
            qbdlxForm._qbdlxForm.logger.Debug("Artwork path: " + artworkPath);

            // Use secure connection
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls13;

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
                using (var httpClient = new HttpClient { Timeout = TimeSpan.FromMinutes(5) })
                using (var response = await httpClient.GetAsync(streamUrl, HttpCompletionOption.ResponseHeadersRead))
                {
                    response.EnsureSuccessStatusCode();

                    long totalBytes = response.Content.Headers.ContentLength ?? -1;
                    long totalBytesRead = 0;
                    int bufferLength = 81920; // 80 kb. Stream.cs: const int DefaultCopyBufferSize = 81920
                    byte[] buffer = new byte[bufferLength];
                    DateTime lastUpdateTime = DateTime.Now;
                    long previousBytesRead = 0;

                    using (var fs = new FileStream(tempFile, FileMode.Create, FileAccess.Write, FileShare.Read, bufferLength, useAsync:true))
                    using (var stream = await response.Content.ReadAsStreamAsync())
                    using (var cts = new CancellationTokenSource(TimeSpan.FromSeconds(50)))
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
                    qbdlxForm._qbdlxForm.logger.Debug("Attempting to fix unset MD5s...");
                    fixMD5.fixMD5(tempFile, "flac");
                }

                qbdlxForm._qbdlxForm.logger.Debug("Starting file metadata tagging");
                TagFile.WriteToFile(tempFile, artworkPath, QoAlbum, QoItem);

                // Move the file to final destination
                qbdlxForm._qbdlxForm.logger.Debug("Moving temp file to - " + filePath);
                ZlpIOHelper.MoveFile(tempFile, filePath);

                qbdlxForm._qbdlxForm.logger.Debug("Download complete.");
            }
            catch (TaskCanceledException ex)
            {
                qbdlxForm._qbdlxForm.logger.Error("Download timed out or cancelled: " + ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                qbdlxForm._qbdlxForm.logger.Error("Download failed: " + ex.Message);
                throw;
            }
        }

        // PREVIOUS WORKING IMPLEMENTATION USING OBSOLETE WEBCLIENT CLASS AND WITHOUT A CONNECTION TIMEOUT,
        // HANGING FOREVER THE APPLICATION IN SOME CASES. AND WITH SLOWER OVERALL TRACKS DOWNLOAD TIME.
        // ================================================================================================
        //
        //[Obsolete("WebClient is obsolete, and you shouldn't use it for development. Use HttpClient instead. https://learn.microsoft.com/en-us/dotnet/api/system.net.webclient", false)]
        //public async Task DownloadStream(string streamUrl, string downloadPath, string filePath, string audio_format, Album QoAlbum, Item QoItem)
        //{
        //    qbdlxForm._qbdlxForm.logger.Debug("Writing temp file to qbdlx-temp/qbdlx_downloading-" + QoItem.Id.ToString() + audio_format);
        //
        //    // Create a temp directory inside the exe location, to download files to.
        //    string tempFile = ZlpPathHelper.Combine(@"qbdlx-temp", "qbdlx_downloading-" + QoItem.Id.ToString() + audio_format);
        //    ZlpIOHelper.CreateDirectory(@"qbdlx-temp");
        //
        //    qbdlxForm._qbdlxForm.BeginInvoke(new Action(() => { qbdlxForm._qbdlxForm.progressLabel.Text = $"{qbdlxForm._qbdlxForm.progressLabelActive} - {0}%"; }));
        //
        //    using (var client = new WebClient())
        //    {
        //        // Set path for downloaded artwork.
        //        artworkPath = downloadPath + qbdlxForm._qbdlxForm.embeddedArtSize + @".jpg";
        //        qbdlxForm._qbdlxForm.logger.Debug("Artwork path: " + artworkPath);
        //
        //        // Use secure connection
        //        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls13;
        //
        //        // Create a TaskCompletionSource to handle asynchronous waiting
        //        var tcs = new TaskCompletionSource<bool>();
        //
        //        // Fields to track previous progress and time for speed calculation
        //        long previousBytesReceived = 0;
        //        DateTime lastUpdateTime = DateTime.Now;
        //
        //        // Subscribe to progress changed event
        //        client.DownloadProgressChanged += (sender, e) =>
        //        {
        //            int progressPercentage = e.ProgressPercentage;
        //            long bytesReceived = e.BytesReceived;
        //            long totalBytesToReceive = e.TotalBytesToReceive;
        //
        //            if (qbdlxForm._qbdlxForm.downloadSpeedCheckbox.Checked)
        //            {
        //                // Calculate download speed in bytes per second
        //                DateTime currentTime = DateTime.Now;
        //                double timeDiff = (currentTime - lastUpdateTime).TotalSeconds;
        //
        //                if (timeDiff > 0)
        //                {
        //                    long bytesDiff = bytesReceived - previousBytesReceived;
        //                    double speed = bytesDiff / timeDiff; // bytes per second
        //                    string speedText = speed > 1024 * 1024
        //                        ? $"{speed / (1024 * 1024):F2} MB/s"
        //                        : $"{speed / 1024:F2} KB/s";
        //
        //                    qbdlxForm._qbdlxForm.BeginInvoke(new Action(() => { qbdlxForm._qbdlxForm.progressLabel.Text = $"{qbdlxForm._qbdlxForm.progressLabelActive} - {progressPercentage}% [{speedText}]"; }));
        //                }
        //            }
        //            else
        //            {
        //                qbdlxForm._qbdlxForm.BeginInvoke(new Action(() => { qbdlxForm._qbdlxForm.progressLabel.Text = $"{qbdlxForm._qbdlxForm.progressLabelActive} - {progressPercentage}%"; }));
        //            }
        //        };
        //
        //        // Handle completion of the download
        //        client.DownloadFileCompleted += (sender, e) =>
        //        {
        //            if (e.Error != null)
        //            {
        //                qbdlxForm._qbdlxForm.logger.Error("Download failed: " + e.Error.Message);
        //                tcs.SetException(e.Error);
        //                return;
        //            }
        //
        //            qbdlxForm._qbdlxForm.logger.Debug("Download complete.");
        //
        //            if (Settings.Default.fixMD5s && audio_format.Contains("flac"))
        //            {
        //                qbdlxForm._qbdlxForm.logger.Debug("Attempting to fix unset MD5s...");
        //                fixMD5.fixMD5(tempFile, "flac");
        //            }
        //
        //            qbdlxForm._qbdlxForm.logger.Debug("Starting file metadata tagging");
        //            TagFile.WriteToFile(tempFile, artworkPath, QoAlbum, QoItem);
        //
        //            // Move the file with the full name (Zeta Long Paths to avoid MAX_PATH error)
        //            qbdlxForm._qbdlxForm.logger.Debug("Moving temp file to - " + filePath);
        //            ZlpIOHelper.MoveFile(tempFile, filePath);
        //
        //            // Signal the TaskCompletionSource that the task is complete
        //            tcs.SetResult(true);
        //        };
        //
        //        // Start the asynchronous download
        //        qbdlxForm._qbdlxForm.logger.Debug("Downloading to temp file...");
        //        if (QoAlbum.MediaCount > 1)
        //        {
        //            qbdlxForm._qbdlxForm.logger.Debug("More than 1 volume, using subfolders for each volume");
        //            ZlpIOHelper.CreateDirectory(ZlpPathHelper.GetDirectoryPathNameFromFilePath(downloadPath + "CD " + QoItem.MediaNumber.ToString().PadLeft(paddingNumbers.padDiscs(QoAlbum), '0') + ZlpPathHelper.DirectorySeparatorChar));
        //        }
        //        else
        //        {
        //            ZlpIOHelper.CreateDirectory(ZlpPathHelper.GetDirectoryPathNameFromFilePath(downloadPath));
        //        }
        //        client.DownloadFileAsync(new Uri(streamUrl), tempFile);
        //
        //        // Await the TaskCompletionSource to wait until download completes
        //        await tcs.Task;
        //    }
        //}

        public async Task DownloadArtwork(string downloadPath, Album QoAlbum)
        {
            if (qbdlxForm._qbdlxForm.savedArtSize == "0") return; 

            using (var client = new WebClient())
            {
                // Use secure connection
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls13;

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
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                                                 | SecurityProtocolType.Tls11
                                                 | SecurityProtocolType.Tls12
                                                 | SecurityProtocolType.Tls13;

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
                    qbdlxForm._qbdlxForm.logger.Debug("Downloading goody...");

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

        // PREVIOUS WORKING IMPLEMENTATION USING OBSOLETE WEBCLIENT CLASS.
        // It was not reporting http status code errors.
        // ===============================================================
        //public async Task DownloadGoody(string downloadPath, Album QoAlbum, Goody QoGoody, GetInfo getInfo)
        //{
        //    using (var client = new WebClient())
        //    {
        //        // Use secure connection
        //        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
        //                                             | SecurityProtocolType.Tls11
        //                                             | SecurityProtocolType.Tls12
        //                                             | SecurityProtocolType.Tls13;
        //
        //        // Ensure directory exists
        //        ZlpIOHelper.CreateDirectory(ZlpPathHelper.GetDirectoryPathNameFromFilePath(downloadPath));
        //
        //        // Build the file name
        //        string fileName = downloadPath + renameTemplates.GetSafeFilename($"{QoAlbum.Title} - {QoGoody.Description}")
        //                          + " (" + QoGoody.Id + $").{Path.GetExtension(QoGoody.Url)}";
        //
        //        // Check if the file already exists
        //        if (!ZlpIOHelper.FileExists(fileName))
        //        {
        //            getInfo.updateDownloadOutput($"{qbdlxForm._qbdlxForm.downloadOutputGoodyFound} ");
        //            qbdlxForm._qbdlxForm.logger.Debug("Downloading goody...");
        //            await client.DownloadFileTaskAsync(QoGoody.Url, fileName);
        //            qbdlxForm._qbdlxForm.logger.Debug("Goody download complete");
        //            getInfo.updateDownloadOutput($"{qbdlxForm._qbdlxForm.downloadOutputDone}\r\n");
        //        }
        //        else
        //        {
        //            qbdlxForm._qbdlxForm.logger.Debug("Goody file already exists: " + fileName);
        //            System.Diagnostics.Debug.WriteLine("Goody file already exists: " + fileName);
        //            getInfo.updateDownloadOutput($"{qbdlxForm._qbdlxForm.languageManager.GetTranslation("downloadOutputGoodyExists")}\r\n");
        //        }
        //    }
        //}
    }
}
