using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using ZetaLongPaths;
using QobuzDownloaderX;
using QopenAPI;
using QobuzDownloaderX.Properties;
using QobuzDownloaderX.Download;

namespace QobuzDownloaderX
{
    class DownloadFile
    {
        TagFile tagFile = new TagFile();
        RenameTemplates renameTemplates = new RenameTemplates();
        PaddingNumbers paddingNumbers = new PaddingNumbers();
        GetInfo getInfo = new GetInfo();
        FixMD5 fixMD5 = new FixMD5();

        public string artistTemplateConverted { get; set; }
        public string albumTemplateConverted { get; set; }
        public string trackTemplateConverted { get; set; }
        public string playlistTemplateConverted { get; set; }
        public string downloadPath { get; set; }
        public string artworkPath { get; set; }

        public string createPath(string downloadLocation, string artistTemplate, string albumTemplate, string trackTemplate, string playlistTemplate, string favoritesTemplate, int paddedTrackLength, int paddedDiscLength, Album QoAlbum, Item QoItem, Playlist QoPlaylist)
        {
            if (QoPlaylist == null)
            {
                qbdlxForm._qbdlxForm.logger.Debug("Using non-playlist path");
                artistTemplateConverted = renameTemplates.renameTemplates(artistTemplate, paddedTrackLength, paddedDiscLength, qbdlxForm._qbdlxForm.audio_format, QoAlbum, null, null);
                albumTemplateConverted = renameTemplates.renameTemplates(albumTemplate, paddedTrackLength, paddedDiscLength, qbdlxForm._qbdlxForm.audio_format, QoAlbum, null, null);
                downloadPath = Path.Combine(downloadLocation, artistTemplateConverted, albumTemplateConverted.TrimEnd(Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar);
                
                return downloadPath;
            }
            else
            {
                qbdlxForm._qbdlxForm.logger.Debug("Using playlist path");
                playlistTemplateConverted = renameTemplates.renameTemplates(playlistTemplate, paddedTrackLength, paddedDiscLength, qbdlxForm._qbdlxForm.audio_format, null, null, QoPlaylist);
                downloadPath = Path.Combine(downloadLocation, playlistTemplateConverted.TrimEnd(Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar);

                return downloadPath;
            }
        }

        public void downloadStream(string streamUrl, string downloadPath, string filePath, string audio_format, Album QoAlbum, Item QoItem)
        {
            qbdlxForm._qbdlxForm.logger.Debug("Writing temp file to qbdlx-temp/qbdlx_downloading-" + QoItem.Id.ToString() + audio_format);
            // Create a temp directory inside the exe location, to download files to.
            string tempFile = Path.Combine(@"qbdlx-temp", "qbdlx_downloading-" + QoItem.Id.ToString() + audio_format);
            Directory.CreateDirectory(@"qbdlx-temp");

            using (var client = new WebClient())
            {
                // Set path for downloaded artwork.
                artworkPath = downloadPath + qbdlxForm._qbdlxForm.embeddedArtSize + @".jpg";
                qbdlxForm._qbdlxForm.logger.Debug("Artwork path: " + artworkPath);

                // Use secure connection
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

                // Download to the temp directory that was made, and tag the file
                qbdlxForm._qbdlxForm.logger.Debug("Downloading to temp file...");
                Console.WriteLine(filePath);
                if (QoAlbum.MediaCount > 1)
                {
                    qbdlxForm._qbdlxForm.logger.Debug("More than 1 volume, using subfolders for each volume");
                    Directory.CreateDirectory(Path.GetDirectoryName(downloadPath + "CD " + QoItem.MediaNumber.ToString().PadLeft(paddingNumbers.padDiscs(QoAlbum), '0') + Path.DirectorySeparatorChar));
                }
                else
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(downloadPath));
                }
                client.DownloadFile(streamUrl, tempFile);

                if (Settings.Default.fixMD5s == true)
                {
                    if (audio_format.Contains("flac") == true)
                    {
                        //getInfo.updateDownloadOutput("\r\nAttempting to fix unset MD5s...");
                        fixMD5.fixMD5(tempFile, "flac");
                    }
                }

                qbdlxForm._qbdlxForm.logger.Debug("Starting file metadata tagging");
                TagFile.WriteToFile(tempFile, artworkPath, QoAlbum, QoItem);

                // Move the file with the full name (Zeta Long Paths to avoid MAX_PATH error)
                qbdlxForm._qbdlxForm.logger.Debug("Moving temp file to - " + filePath);
                ZetaLongPaths.ZlpIOHelper.MoveFile(tempFile, filePath);
            }
        }

        public void downloadArtwork(string downloadPath, Album QoAlbum)
        {
            using (var client = new WebClient())
            {
                // Use secure connection
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

                // Download cover art (600x600) to the download path
                qbdlxForm._qbdlxForm.logger.Debug("Downloading Cover Art");
                Directory.CreateDirectory(Path.GetDirectoryName(downloadPath));

                if (File.Exists(downloadPath + @"Cover.jpg") == false)
                {
                    qbdlxForm._qbdlxForm.logger.Debug("Saved artwork Cover.jpg not found, downloading");
                    try { client.DownloadFile(QoAlbum.Image.Large.Replace("_600", "_" + qbdlxForm._qbdlxForm.savedArtSize), downloadPath + @"Cover.jpg"); } catch { Console.WriteLine("Failed to Download Cover Art"); }
                }
                if (File.Exists(downloadPath + qbdlxForm._qbdlxForm.embeddedArtSize + @".jpg") == false)
                {
                    qbdlxForm._qbdlxForm.logger.Debug("Saved artwork for embedding not found, downloading");
                    try { client.DownloadFile(QoAlbum.Image.Large.Replace("_600", "_" + qbdlxForm._qbdlxForm.embeddedArtSize), downloadPath + qbdlxForm._qbdlxForm.embeddedArtSize + @".jpg"); } catch { Console.WriteLine("Failed to Download Cover Art"); }
                }
            }
        }

        public void downloadGoody(string downloadPath, Album QoAlbum, Goody QoGoody)
        {
            using (var client = new WebClient())
            {
                // Use secure connection
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

                // Download goody to the download path
                Directory.CreateDirectory(Path.GetDirectoryName(downloadPath));
                client.DownloadFile(QoGoody.Url, downloadPath + QoAlbum.Title + " (" + QoGoody.Id + @").pdf");
            }
        }
    }
}
