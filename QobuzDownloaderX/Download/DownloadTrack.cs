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
using QobuzDownloaderX.Properties;
using QobuzDownloaderX.Download;

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
        public string fileName { get; set; }

        PaddingNumbers padNumber = new PaddingNumbers();
        DownloadFile downloadFile = new DownloadFile();
        RenameTemplates renameTemplates = new RenameTemplates();
        GetInfo getInfo = new GetInfo();
        FixMD5 fixMD5 = new FixMD5();

        public void clearOutputText()
        {
            getInfo.outputText = null;
        }

        public void downloadIndividualTrack(string app_id, string album_id, string format_id, string audio_format, string user_auth_token, string app_secret, string downloadLocation, string artistTemplate, string albumTemplate, string trackTemplate, Album QoAlbum)
        {
            getInfo.outputText = null;

            try
            {
                paddedTrackLength = padNumber.padTracks(QoAlbum);
                paddedDiscLength = padNumber.padDiscs(QoAlbum);

                try
                {
                    // Get track info with auth
                    QoItem = QoService.TrackGetWithAuth(app_id, album_id, user_auth_token);

                    if (QoItem.Streamable == false)
                    {
                        if (Settings.Default.streamableCheck == true)
                        {
                            getInfo.updateDownloadOutput("Track " + QoItem.TrackNumber.ToString().PadLeft(paddedTrackLength, '0') + " is not available for streaming. Skipping.\r\n");

                            // Say the downloading is finished as no downloads happened.
                            getInfo.updateDownloadOutput("\r\n" + "DOWNLOAD COMPLETE");
                            return;
                        }
                        else { }
                    }

                    downloadPath = downloadFile.createPath(downloadLocation, artistTemplate, albumTemplate, trackTemplate, null, null, paddedTrackLength, paddedDiscLength, QoAlbum, QoItem, null);

                    downloadFile.downloadArtwork(downloadPath, QoAlbum);

                    string trackTemplateConverted = renameTemplates.renameTemplates(trackTemplate, paddedTrackLength, paddedDiscLength, audio_format, QoAlbum, QoItem, null);

                    if (QoAlbum.MediaCount > 1)
                    {
                        filePath = downloadPath + "CD " + QoItem.MediaNumber.ToString().PadLeft(paddedDiscLength, '0') + Path.DirectorySeparatorChar + trackTemplateConverted.TrimEnd() + audio_format;
                    }
                    else
                    {
                        filePath = downloadPath + trackTemplateConverted.TrimEnd() + audio_format;
                    }
                    

                    if (System.IO.File.Exists(filePath))
                    {
                        getInfo.updateDownloadOutput("File for track " + QoItem.TrackNumber.ToString().PadLeft(paddedTrackLength, '0') + " already exists, skipping.\r\n");
                        System.Threading.Thread.Sleep(100);

                        // Say the downloading is finished as no downloads happened.
                        getInfo.updateDownloadOutput("\r\n" + "DOWNLOAD COMPLETE");
                        return;
                    }

                    // Get stream URL
                    Console.WriteLine("Getting Stream Link");
                    QoStream = QoService.TrackGetFileUrl(QoItem.Id.ToString(), format_id, app_id, user_auth_token, app_secret);
                    string streamURL = QoStream.StreamURL;

                    // Send file to DownloadFile.cs to download from stream URL.
                    if (QoItem.Version == null)
                    {
                        getInfo.updateDownloadOutput("Downloading - " + QoItem.TrackNumber.ToString().PadLeft(paddedTrackLength, '0') + " " + QoItem.Title + "...");
                    }
                    else
                    {
                        getInfo.updateDownloadOutput("Downloading - " + QoItem.TrackNumber.ToString().PadLeft(paddedTrackLength, '0') + " " + QoItem.Title.TrimEnd() + " (" + QoItem.Version + ")" + "...");
                    }
                    downloadFile.downloadStream(streamURL, downloadPath, filePath, audio_format, QoAlbum, QoItem);

                    //if(Settings.Default.fixMD5s == true)
                    //{
                    //    if (audio_format.Contains("flac") == true)
                    //    {
                    //        getInfo.updateDownloadOutput("\r\nAttempting to fix unset MD5s...");
                    //        fixMD5.fixMD5(downloadPath, null, "flac");
                    //    }
                    //}

                    // Say DONE when finished downloading.
                    getInfo.updateDownloadOutput(" DONE" + "\r\n");

                    // Delete image used for embedded artwork
                    if (System.IO.File.Exists(downloadFile.artworkPath))
                    {
                        System.IO.File.Delete(downloadFile.artworkPath);
                    }
                }
                catch (Exception downloadAlbumEx)
                {
                    getInfo.updateDownloadOutput("\r\n\r\n" + downloadAlbumEx + "\r\n\r\n");
                    Console.WriteLine(downloadAlbumEx);
                    return;
                }

                // Say the downloading is finished when it's completed.
                getInfo.updateDownloadOutput("\r\n" + "DOWNLOAD COMPLETE");

            }
            catch (Exception downloadAlbumEx)
            {
                Console.WriteLine(downloadAlbumEx);
                return;
            }
        }

        public void downloadTrack(string app_id, string album_id, string format_id, string audio_format, string user_auth_token, string app_secret, string downloadLocation, string artistTemplate, string albumTemplate, string trackTemplate, Album QoAlbum, Item QoItem)
        {
            try
            {
                paddedTrackLength = padNumber.padTracks(QoAlbum);
                paddedDiscLength = padNumber.padDiscs(QoAlbum);
                
                if (QoItem.Streamable == false)
                {
                    if (Settings.Default.streamableCheck == true)
                    {
                        getInfo.updateDownloadOutput("Track " + QoItem.TrackNumber.ToString().PadLeft(paddedTrackLength, '0') + " is not available for streaming. Skipping.\r\n");
                        return;
                    }
                    else { }
                }

                try
                {
                    downloadPath = downloadFile.createPath(downloadLocation, artistTemplate, albumTemplate, trackTemplate, null, null, paddedTrackLength, paddedDiscLength, QoAlbum, QoItem, null);

                    try { downloadFile.downloadArtwork(downloadPath, QoAlbum); } catch { Console.WriteLine("Failed to Download Cover Art"); }

                    string trackTemplateConverted = renameTemplates.renameTemplates(trackTemplate, paddedTrackLength, paddedDiscLength, audio_format, QoAlbum, QoItem, null);

                    if (QoAlbum.MediaCount > 1)
                    {
                        filePath = downloadPath + "CD " + QoItem.MediaNumber.ToString().PadLeft(paddedDiscLength, '0') + Path.DirectorySeparatorChar + trackTemplateConverted.TrimEnd() + audio_format;
                    }
                    else
                    {
                        filePath = downloadPath + trackTemplateConverted.TrimEnd() + audio_format;
                    }


                    if (System.IO.File.Exists(filePath))
                    {
                        getInfo.updateDownloadOutput("File for track " + QoItem.TrackNumber.ToString().PadLeft(paddedTrackLength, '0') + " already exists, skipping.\r\n");
                        System.Threading.Thread.Sleep(100);
                        return;
                    }

                    // Get stream URL
                    Console.WriteLine("Getting Stream Link");
                    QoStream = QoService.TrackGetFileUrl(QoItem.Id.ToString(), format_id, app_id, user_auth_token, app_secret);
                    string streamURL = QoStream.StreamURL;

                    // Send file to DownloadFile.cs to download from stream URL.
                    if (QoItem.Version == null)
                    {
                        getInfo.updateDownloadOutput("Downloading - " + QoItem.TrackNumber.ToString().PadLeft(paddedTrackLength, '0') + " " + QoItem.Title + "...");
                    }
                    else
                    {
                        getInfo.updateDownloadOutput("Downloading - " + QoItem.TrackNumber.ToString().PadLeft(paddedTrackLength, '0') + " " + QoItem.Title.TrimEnd() + " (" + QoItem.Version + ")" + "...");
                    }
                    downloadFile.downloadStream(streamURL, downloadPath, filePath, audio_format, QoAlbum, QoItem);

                    // Say DONE when finished downloading.
                    getInfo.updateDownloadOutput(" DONE" + "\r\n");
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

        public void downloadPlaylistTrack(string app_id, string album_id, string format_id, string audio_format, string user_auth_token, string app_secret, string downloadLocation, string artistTemplate, string albumTemplate, string trackTemplate, string playlistTemplate, Album QoAlbum, Item QoItem, Playlist QoPlaylist)
        {
            try
            {
                paddedTrackLength = padNumber.padPlaylistTracks(QoPlaylist);
                paddedDiscLength = 2;

                if (QoItem.Streamable == false)
                {
                    if (Settings.Default.streamableCheck == true)
                    {
                        getInfo.updateDownloadOutput("Track " + QoItem.TrackNumber.ToString().PadLeft(paddedTrackLength, '0') + " is not available for streaming. Skipping.\r\n");
                        return;
                    }
                    else { }
                }

                try
                {
                    downloadPath = downloadFile.createPath(downloadLocation, null, null, trackTemplate, playlistTemplate, null, paddedTrackLength, paddedDiscLength, QoAlbum, QoItem, QoPlaylist);

                    string trackTemplateConverted = renameTemplates.renameTemplates(trackTemplate, paddedTrackLength, paddedDiscLength, audio_format, QoAlbum, QoItem, QoPlaylist);

                    filePath = downloadPath + trackTemplateConverted.TrimEnd() + audio_format;


                    if (System.IO.File.Exists(filePath))
                    {
                        getInfo.updateDownloadOutput("File for track " + QoItem.Position.ToString().PadLeft(paddedTrackLength, '0') + " already exists, skipping.\r\n");
                        System.Threading.Thread.Sleep(100);
                        return;
                    }

                    try { downloadFile.downloadArtwork(downloadPath, QoAlbum); } catch { Console.WriteLine("Failed to Download Cover Art"); }

                    // Get stream URL
                    Console.WriteLine("Getting Stream Link");
                    QoStream = QoService.TrackGetFileUrl(QoItem.Id.ToString(), format_id, app_id, user_auth_token, app_secret);
                    string streamURL = QoStream.StreamURL;

                    // Send file to DownloadFile.cs to download from stream URL.
                    if (QoItem.Version == null)
                    {
                        getInfo.updateDownloadOutput("Downloading - " + QoItem.Position.ToString().PadLeft(paddedTrackLength, '0') + " " + QoItem.Title + "...");
                    }
                    else
                    {
                        getInfo.updateDownloadOutput("Downloading - " + QoItem.Position.ToString().PadLeft(paddedTrackLength, '0') + " " + QoItem.Title.TrimEnd() + " (" + QoItem.Version + ")" + "...");
                    }
                    downloadFile.downloadStream(streamURL, downloadPath, filePath, audio_format, QoAlbum, QoItem);

                    // Say DONE when finished downloading.
                    getInfo.updateDownloadOutput(" DONE" + "\r\n");

                    // Delete image used for embedded artwork
                    if (System.IO.File.Exists(downloadFile.artworkPath))
                    {
                        System.IO.File.Delete(downloadFile.artworkPath);
                    }
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
