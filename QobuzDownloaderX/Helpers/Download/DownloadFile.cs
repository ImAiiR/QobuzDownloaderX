using System;
using System.Net;
using System.Threading.Tasks;
using QopenAPI;
using QobuzDownloaderX.Properties;
using System.Text.RegularExpressions;
using ZetaLongPaths;

namespace QobuzDownloaderX
{
    class DownloadFile
    {
        TagFile tagFile = new TagFile();
        RenameTemplates renameTemplates = new RenameTemplates();
        PaddingNumbers paddingNumbers = new PaddingNumbers();
        GetInfo getInfo = new GetInfo();
        FixMD5 fixMD5 = new FixMD5();

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
                    string playlistTemplateConverted = renameTemplates.renameTemplates(playlistTemplate, paddedTrackLength, paddedDiscLength, qbdlxForm._qbdlxForm.audio_format, null, null, QoPlaylist);
                    downloadPath = ZlpPathHelper.Combine(downloadLocation, playlistTemplateConverted.TrimEnd(ZlpPathHelper.DirectorySeparatorChar) + ZlpPathHelper.DirectorySeparatorChar);
                    downloadPath = Regex.Replace(downloadPath, @"\s+", " "); // Remove double spaces
                }
                return downloadPath;
            });
        }

        public async Task DownloadStream(string streamUrl, string downloadPath, string filePath, string audio_format, Album QoAlbum, Item QoItem)
        {
            qbdlxForm._qbdlxForm.logger.Debug("Writing temp file to qbdlx-temp/qbdlx_downloading-" + QoItem.Id.ToString() + audio_format);

            // Create a temp directory inside the exe location, to download files to.
            string tempFile = ZlpPathHelper.Combine(@"qbdlx-temp", "qbdlx_downloading-" + QoItem.Id.ToString() + audio_format);
            ZlpIOHelper.CreateDirectory(@"qbdlx-temp");

            using (var client = new WebClient())
            {
                // Set path for downloaded artwork.
                artworkPath = downloadPath + qbdlxForm._qbdlxForm.embeddedArtSize + @".jpg";
                qbdlxForm._qbdlxForm.logger.Debug("Artwork path: " + artworkPath);

                // Use secure connection
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                
                // Create a TaskCompletionSource to handle asynchronous waiting
                var tcs = new TaskCompletionSource<bool>();
                
                // Fields to track previous progress and time for speed calculation
                long previousBytesReceived = 0;
                DateTime lastUpdateTime = DateTime.Now;

                // Subscribe to progress changed event
                client.DownloadProgressChanged += (sender, e) =>
                {
                    int progressPercentage = e.ProgressPercentage;
                    long bytesReceived = e.BytesReceived;
                    long totalBytesToReceive = e.TotalBytesToReceive;

                    if (qbdlxForm._qbdlxForm.downloadSpeedCheckbox.Checked)
                    {
                        // Calculate download speed in bytes per second
                        DateTime currentTime = DateTime.Now;
                        double timeDiff = (currentTime - lastUpdateTime).TotalSeconds;

                        if (timeDiff > 0)
                        {
                            long bytesDiff = bytesReceived - previousBytesReceived;
                            double speed = bytesDiff / timeDiff; // bytes per second
                            string speedText = speed > 1024 * 1024
                                ? $"{speed / (1024 * 1024):F2} MB/s"
                                : $"{speed / 1024:F2} KB/s";

                            qbdlxForm._qbdlxForm.BeginInvoke(new Action(() => { qbdlxForm._qbdlxForm.progressLabel.Text = $"{qbdlxForm._qbdlxForm.progressLabelActive} - {progressPercentage}% [{speedText}]"; }));
                        }
                    }
                    else
                    {
                        qbdlxForm._qbdlxForm.BeginInvoke(new Action(() => { qbdlxForm._qbdlxForm.progressLabel.Text = $"{qbdlxForm._qbdlxForm.progressLabelActive} - {progressPercentage}%"; }));
                    }
                };

                // Handle completion of the download
                client.DownloadFileCompleted += (sender, e) =>
                {
                    if (e.Error != null)
                    {
                        qbdlxForm._qbdlxForm.logger.Error("Download failed: " + e.Error.Message);
                        tcs.SetException(e.Error);
                        return;
                    }

                    qbdlxForm._qbdlxForm.logger.Debug("Download complete.");

                    if (Settings.Default.fixMD5s && audio_format.Contains("flac"))
                    {
                        qbdlxForm._qbdlxForm.logger.Debug("Attempting to fix unset MD5s...");
                        fixMD5.fixMD5(tempFile, "flac");
                    }

                    qbdlxForm._qbdlxForm.logger.Debug("Starting file metadata tagging");
                    TagFile.WriteToFile(tempFile, artworkPath, QoAlbum, QoItem);

                    // Move the file with the full name (Zeta Long Paths to avoid MAX_PATH error)
                    qbdlxForm._qbdlxForm.logger.Debug("Moving temp file to - " + filePath);
                    ZlpIOHelper.MoveFile(tempFile, filePath);
                    
                    // Signal the TaskCompletionSource that the task is complete
                    tcs.SetResult(true);
                };
                
                // Start the asynchronous download
                qbdlxForm._qbdlxForm.logger.Debug("Downloading to temp file...");
                if (QoAlbum.MediaCount > 1)
                {
                    qbdlxForm._qbdlxForm.logger.Debug("More than 1 volume, using subfolders for each volume");
                    ZlpIOHelper.CreateDirectory(ZlpPathHelper.GetDirectoryPathNameFromFilePath(downloadPath + "CD " + QoItem.MediaNumber.ToString().PadLeft(paddingNumbers.padDiscs(QoAlbum), '0') + ZlpPathHelper.DirectorySeparatorChar));
                }
                else
                {
                    ZlpIOHelper.CreateDirectory(ZlpPathHelper.GetDirectoryPathNameFromFilePath(downloadPath));
                }

                client.DownloadFileAsync(new Uri(streamUrl), tempFile);

                // Await the TaskCompletionSource to wait until download completes
                await tcs.Task;
            }
        }

        public async Task DownloadArtwork(string downloadPath, Album QoAlbum)
        {
            using (var client = new WebClient())
            {
                // Use secure connection
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

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

        public async Task DownloadGoody(string downloadPath, Album QoAlbum, Goody QoGoody)
        {
            using (var client = new WebClient())
            {
                // Use secure connection
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

                // Download goody to the download path
                ZlpIOHelper.CreateDirectory(ZlpPathHelper.GetDirectoryPathNameFromFilePath(downloadPath));
                await client.DownloadFileTaskAsync(QoGoody.Url, downloadPath + renameTemplates.GetSafeFilename(QoAlbum.Title) + " (" + QoGoody.Id + @").pdf");
            }
        }
    }
}
