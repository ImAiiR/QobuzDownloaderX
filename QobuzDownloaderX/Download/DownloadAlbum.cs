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
using QobuzDownloaderX.Properties;
using QobuzDownloaderX.Download;

namespace QobuzDownloaderX
{
    public static class StringExt
    {
        public static string Truncate(this string value, int maxLength)
        {
            // This is currently unused, but leaving this here in case it needs to be used in the future.
            if (string.IsNullOrEmpty(value)) return value;
            return value.Length <= maxLength ? value : value.Substring(0, maxLength);
        }
    }

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

        PaddingNumbers padNumber = new PaddingNumbers();
        DownloadTrack downloadTrack = new DownloadTrack();
        DownloadFile downloadFile = new DownloadFile();
        RenameTemplates renameTemplates = new RenameTemplates();
        GetInfo getInfo = new GetInfo();
        FixMD5 fixMD5 = new FixMD5();

        public Item getTrackInfoLabels(string app_id, string track_id, string user_auth_token)
        {
            try
            {
                // Grab album info with auth
                getInfo.outputText = null;
                getInfo.updateDownloadOutput("Getting Track Info...");
                QoItem = QoService.TrackGetWithAuth(app_id, track_id, user_auth_token);
                string album_id = QoItem.Album.Id;
                QoAlbum = QoService.AlbumGetWithAuth(app_id, album_id, user_auth_token);
                return QoItem;
            }
            catch (Exception getAlbumInfoLabelsEx)
            {
                getInfo.updateDownloadOutput("\r\n" + getAlbumInfoLabelsEx.ToString());
                Console.WriteLine(getAlbumInfoLabelsEx);
                return null;
            }
        }

        public void downloadAlbum(string app_id, string album_id, string format_id, string audio_format, string user_auth_token, string app_secret, string downloadLocation, string artistTemplate, string albumTemplate, string trackTemplate, Album QoAlbum)
        {
            // Clear output text from DownloadTrack to avoid text from previous downloads sticking around.
            downloadTrack.clearOutputText();
            getInfo.outputText = null;

            if (QoAlbum.Streamable == false)
            {
                if (Settings.Default.streamableCheck == true)
                {
                    getInfo.updateDownloadOutput("Release is not available for streaming.");
                    getInfo.updateDownloadOutput("\r\n" + "DOWNLOAD COMPLETE");
                    return;
                }
                else { }
            }

            try
            {
                paddedTrackLength = padNumber.padTracks(QoAlbum);
                paddedDiscLength = padNumber.padTracks(QoAlbum);

                downloadPath = downloadFile.createPath(downloadLocation, artistTemplate, albumTemplate, trackTemplate, null, null, paddedTrackLength, paddedDiscLength, QoAlbum, null, null);

                try
                {
                    downloadFile.downloadArtwork(downloadPath, QoAlbum);
                }
                catch {
                    Console.WriteLine("Unable to download artwork");
                }


                foreach (var item in QoAlbum.Tracks.Items) // For each track ID
                {
                    try
                    {
                        downloadTrack.downloadTrack(app_id, album_id, format_id, audio_format, user_auth_token, app_secret, downloadLocation, artistTemplate, albumTemplate, trackTemplate, QoAlbum, QoService.TrackGetWithAuth(app_id, item.Id.ToString(), user_auth_token));
                    }
                    catch (Exception downloadAlbumEx)
                    {
                        getInfo.updateDownloadOutput("\r\n" + downloadAlbumEx);
                        Console.WriteLine(downloadAlbumEx);
                        return;
                    }
                }

                // Delete image used for embedded artwork
                try
                {
                    System.IO.File.Delete(downloadPath + qbdlxForm._qbdlxForm.embeddedArtSize + @".jpg");
                }
                catch
                {
                    Console.WriteLine("Unable to delete artwork");
                }

                try
                {
                    foreach (var goody in QoAlbum.Goodies)
                    {
                        getInfo.updateDownloadOutput("Goodies found, attempting to download...");
                        if (goody.Url == null)
                        {
                            Console.WriteLine("No goody URL, skipping");
                            getInfo.updateDownloadOutput(" No download URL found, skipping" + "\r\n");
                        }
                        else
                        {
                            Console.WriteLine("Downloading goody");
                            downloadFile.downloadGoody(downloadPath, QoAlbum, goody);
                            getInfo.updateDownloadOutput(" DONE" + "\r\n");
                        }
                    }
                }
                catch
                {

                }

                //if (Settings.Default.fixMD5s == true)
                //{
                //    if (audio_format.Contains("flac") == true)
                //    {
                //        //getInfo.updateDownloadOutput("\r\nAttempting to fix unset MD5s...");
                //        fixMD5.fixMD5(downloadPath, null, "flac");
                //    }
                //}

                // Say the downloading is finished when it's completed.
                getInfo.outputText = qbdlxForm._qbdlxForm.downloadOutput.Text;
                getInfo.updateDownloadOutput("\r\n" + "DOWNLOAD COMPLETE");

                // JAM.S Post Template Export, not really useful for normal users.
                var templateDate = DateTime.Parse(QoAlbum.ReleaseDateOriginal).ToString("MMMM d, yyyy");
                File.WriteAllText("post_template.txt", String.Empty);
                using (StreamWriter sw = File.AppendText("post_template.txt"))
                {
                    sw.WriteLine("[center]");
                    sw.WriteLine("[CoverArt]" + QoAlbum.Image.Large + "[/CoverArt]");
                    sw.WriteLine("[b]Release Date:[/b] " + templateDate);
                    sw.WriteLine("[b]Genre:[/b] " + QoAlbum.Genre.Name.Replace("Alternativ und Indie", "Alternative").Replace("Hörbücher", "Comedy/Other"));
                    sw.WriteLine("");
                    sw.WriteLine("[b]TRACKLIST[/b]");
                    sw.WriteLine("-----------------------------------");

                    foreach (var item in QoAlbum.Tracks.Items) // For each track ID
                    {
                        if (item.Version != null)
                        {
                            sw.WriteLine(item.TrackNumber.ToString() + ". " + item.Title.TrimEnd() + " (" + item.Version + ")");
                        }
                        else
                        {
                            sw.WriteLine(item.TrackNumber.ToString() + ". " + item.Title.TrimEnd());
                        }
                    }

                    sw.WriteLine("-----------------------------------");
                    sw.WriteLine("");
                    sw.WriteLine("[b]DOWNLOADS[/b]");
                    sw.WriteLine("-----------------------------------");
                    sw.WriteLine("[spoiler=" + QoAlbum.Label.Name + " / " + QoAlbum.UPC + " / WEB]");
                    sw.WriteLine("[format=FLAC / Lossless (24bit/??kHz) / WEB]");
                    sw.WriteLine("Uploaded by [USER=2]@AiiR[/USER]");
                    sw.WriteLine("DOWNLOAD");
                    sw.WriteLine("REPLACE THIS WITH URL");
                    sw.WriteLine("[/format]");
                    sw.WriteLine("[format=FLAC / Lossless / WEB]");
                    sw.WriteLine("Uploaded by [USER=2]@AiiR[/USER]");
                    sw.WriteLine("DOWNLOAD");
                    sw.WriteLine("REPLACE THIS WITH URL");
                    sw.WriteLine("[/format]");
                    sw.WriteLine("[format=MP3 / 320 / WEB]");
                    sw.WriteLine("Uploaded by [USER=2]@AiiR[/USER]");
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
            catch (Exception downloadAlbumEx)
            {
                Console.WriteLine(downloadAlbumEx);
                return;
            }
        }
    }
}
