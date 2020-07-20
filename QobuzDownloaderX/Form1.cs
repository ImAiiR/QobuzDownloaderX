using QobuzDownloaderX.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Drawing.Imaging;
using TagLib;
using TagLib.Flac;
using TagLib.Id3v2;
using Newtonsoft.Json.Linq;
using System.Globalization;
using System.Threading;

namespace QobuzDownloaderX
{
    public partial class QobuzDownloaderX : Form
    {
        public QobuzDownloaderX()
        {
            InitializeComponent();
        }

        AboutForm about = new AboutForm();

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        public string eMail { get; set; }
        public string appid { get; set; }
        public string password { get; set; }
        public string userAuth { get; set; }
        public string profilePic { get; set; }
        public string displayName { get; set; }
        public string userID { get; set; }
        public string accountType { get; set; }
        public string appSecret { get; set; }
        public string formatIdString { get; set; }
        public string audioFileType { get; set; }
        public string trackRequest { get; set; }
        public string artSize { get; set; }
        public string finalTrackNamePath { get; set; }
        public string finalTrackNameVersionPath { get; set; }
        public int MaxLength { get; set; }
        public int devClickEggThingValue { get; set; }

        // Important strings
        public string loc { get; set; }
        public string fullLoc { get; set; }
        public string fullLocVersion { get; set; }
        public string qualityPath { get; set; }
        public string stream { get; set; }
        public string type { get; set; }
        public string userAgent { get; set; }
        public string path1Full { get; set; }
        public string path2Full { get; set; }
        public string path3Full { get; set; }
        public string path4Full { get; set; }
        public string path5Full { get; set; }
        public string path6Full { get; set; }


        // Info / Tagging strings
        public string albumId { get; set; }
        public string trackIdString { get; set; }
        public string versionName { get; set; }
        public string advisory { get; set; }
        public string albumArtist { get; set; }
        public string albumName { get; set; }
        public string performerName { get; set; }
        public string composerName { get; set; }
        public string trackName { get; set; }
        public string copyright { get; set; }
        public string genre { get; set; }
        public string releaseDate { get; set; }
        public string isrc { get; set; }
        public string upc { get; set; }
        public string frontCoverImg { get; set; }
        public string frontCoverImgBox { get; set; }
        public string goodiesPDF { get; set; }

        // Info strings for creating paths
        public string albumArtistPath { get; set; }
        public string albumNamePath { get; set; }
        public string performerNamePath { get; set; }
        public string trackNamePath { get; set; }
        public string versionNamePath { get; set; }

        // Info / Tagging ints
        public int discNumber { get; set; }
        public int discTotal { get; set; }
        public int trackNumber { get; set; }
        public int trackTotal { get; set; }

        searchForm searchF = new searchForm();

        private void Form1_Load(object sender, EventArgs e)
        {
            MaxLength = 36;

            // Set main form size on launch and bring to center.
            this.Height = 533;
            this.CenterToScreen();

            // Welcome the user after successful login.
            output.Invoke(new Action(() => output.Text = String.Empty));
            output.Invoke(new Action(() => output.AppendText("Welcome " + displayName + "!\r\n")));

            // Show account type if user logged in normally.
            if (accountType == null | accountType == "")
            {
                output.Invoke(new Action(() => output.AppendText("\r\n")));
            }
            else
            {
                output.Invoke(new Action(() => output.AppendText("Qobuz Account Type - " + accountType + "\r\n\r\n")));
            }

            output.Invoke(new Action(() => output.AppendText("Your user_auth_token has been set for this session!")));

            // Get and display version number.
            verNumLabel.Text = Assembly.GetExecutingAssembly().GetName().Version.ToString();

            // Set user agent for web requests (HttpClient, etc)
            userAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:67.0) Gecko/20100101 Firefox/67.0";

            // Set app_id & auth_token for the Search Form
            searchF.appid = appid;
            searchF.userAuth = userAuth;

            // Set a placeholder image for Cover Art box.
            albumArtPicBox.ImageLocation = "https://static.qobuz.com/images/covers/01/00/2013072600001_150.jpg";
            profilePictureBox.ImageLocation = profilePic;

            // Change account info for logout button
            string oldText = logoutLabel.Text;
            logoutLabel.Text = oldText.Replace("%name%", displayName);

            #region Load Saved Settings
            // Set saved settings to correct places.
            folderBrowserDialog.SelectedPath = Settings.Default.savedFolder.ToString();
            albumCheckbox.Checked = Settings.Default.albumTag;
            albumArtistCheckbox.Checked = Settings.Default.albumArtistTag;
            artistCheckbox.Checked = Settings.Default.artistTag;
            commentCheckbox.Checked = Settings.Default.commentTag;
            commentTextbox.Text = Settings.Default.commentText;
            composerCheckbox.Checked = Settings.Default.composerTag;
            copyrightCheckbox.Checked = Settings.Default.copyrightTag;
            discNumberCheckbox.Checked = Settings.Default.discTag;
            discTotalCheckbox.Checked = Settings.Default.totalDiscsTag;
            genreCheckbox.Checked = Settings.Default.genreTag;
            isrcCheckbox.Checked = Settings.Default.isrcTag;
            typeCheckbox.Checked = Settings.Default.typeTag;
            explicitCheckbox.Checked = Settings.Default.explicitTag;
            trackTitleCheckbox.Checked = Settings.Default.trackTitleTag;
            trackNumberCheckbox.Checked = Settings.Default.trackTag;
            trackTotalCheckbox.Checked = Settings.Default.totalTracksTag;
            upcCheckbox.Checked = Settings.Default.upcTag;
            releaseCheckbox.Checked = Settings.Default.yearTag;
            imageCheckbox.Checked = Settings.Default.imageTag;
            mp3Checkbox.Checked = Settings.Default.quality1;
            flacLowCheckbox.Checked = Settings.Default.quality2;
            flacMidCheckbox.Checked = Settings.Default.quality3;
            flacHighCheckbox.Checked = Settings.Default.quality4;
            formatIdString = Settings.Default.qualityFormat;
            audioFileType = Settings.Default.audioType;
            artSizeSelect.SelectedIndex = Settings.Default.savedArtSize;
            artSize = artSizeSelect.Text;
            #endregion

            // Check if there's no selected path saved.
            if (folderBrowserDialog.SelectedPath == null | folderBrowserDialog.SelectedPath == "")
            {
                // If there is NOT a saved path.
                output.Invoke(new Action(() => output.AppendText("\r\n\r\n")));
                output.Invoke(new Action(() => output.AppendText("No default path has been set! Remember to Choose a Folder!\r\n")));
            }
            else
            {
                // If there is a saved path.
                output.Invoke(new Action(() => output.AppendText("\r\n\r\n")));
                output.Invoke(new Action(() => output.AppendText("Using the last folder you've selected as your selected path!\r\n")));
                output.Invoke(new Action(() => output.AppendText("\r\n")));
                output.Invoke(new Action(() => output.AppendText("Default Folder:\r\n")));
                output.Invoke(new Action(() => output.AppendText(folderBrowserDialog.SelectedPath + "\r\n")));
            }

            // Run anything put into the debug events (For Testing)
            debuggingEvents(sender, e);
        }

        private void debuggingEvents(object sender, EventArgs e)
        {
            #region Debug Events, For Testing

            devClickEggThingValue = 0;

            // Show app_secret value.
            //output.Invoke(new Action(() => output.AppendText("\r\n\r\napp_secret = " + appSecret)));

            // Show format_id value.
            //output.Invoke(new Action(() => output.AppendText("\r\n\r\nformat_id = " + formatIdString)));

            #endregion
        }

        //// Set DateTime for new date formatting.
        //System.DateTime dateTime = new System.DateTime(1970, 1, 1, 0, 0, 0, 0);

        //public string DateConvertion(string Input)
        //{
        //    var date = DateTime.ParseExact(Input, "M/d/yyyy hh:mm:ss tt",
        //                                    CultureInfo.InvariantCulture);

        //    return date.ToString("yyyy-MM-dd");
        //}
        
        static string DecodeEncodedNonAsciiCharacters(string value)
        {
            if (value != null)
            {
                return Regex.Replace(
                value,
                @"\\u(?<Value>[a-zA-Z0-9]{4})",
                m =>
                {
                    return ((char)int.Parse(m.Groups["Value"].Value, NumberStyles.HexNumber)).ToString();
                });
            }
            else
            {
                return null;
            }
        }

        static string GetMd5Hash(MD5 md5Hash, string input)
        {
            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        // Verify a hash against a string.
        static bool VerifyMd5Hash(MD5 md5Hash, string input, string hash)
        {
            // Hash the input.
            string hashOfInput = GetMd5Hash(md5Hash, input);

            // Create a StringComparer an compare the hashes.
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            if (0 == comparer.Compare(hashOfInput, hash))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // Create a WebClient named "wc" to be used anywhere.
        WebClient wc = new WebClient();

        private void openSearch_Click(object sender, EventArgs e)
        {
            searchF.ShowDialog();
        }

        private void downloadButton_Click(object sender, EventArgs e)
        {
            getLinkTypeBG.RunWorkerAsync();
        }

        private void albumUrl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                getLinkTypeBG.RunWorkerAsync();
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void createURL(object sender, EventArgs e)
        {
            // Create unix timestamp for "request_ts=" and hashing to make request signature.
            Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            string time = unixTimestamp.ToString();


            // Generate the string that will be hashed using MD5 (utf-8). Example string - "trackgetFileUrlformat_id27intentstreamtrack_id6891469115724574501b4d2f1aca8d4c8ef4z07984c5aa6712" (example shows a fake app_secret)
            string md5HashMe = "trackgetFileUrlformat_id" + formatIdString + "intentstreamtrack_id" + trackIdString + time + appSecret;

            // Generate the MD5 hash using the string created above.
            using (MD5 md5Hash = MD5.Create())
            {
                string requestSignature = GetMd5Hash(md5Hash, md5HashMe);

                if (VerifyMd5Hash(md5Hash, md5HashMe, requestSignature))
                {
                    // If the MD5 hash is verified, proceed to get the streaming URL.
                    WebRequest wrGetFile = WebRequest.Create("https://www.qobuz.com/api.json/0.2/track/getFileUrl?request_ts=" + time + "&request_sig=" + requestSignature + "&track_id=" + trackIdString + "&format_id=" + formatIdString + "&intent=stream&app_id=" + appid + "&user_auth_token=" + userAuth);

                    try
                    {
                        // Grab response from API when grabbing the streaming URL.
                        WebResponse ws = wrGetFile.GetResponse();
                        StreamReader sr = new StreamReader(ws.GetResponseStream());

                        string getFileRequest = sr.ReadToEnd();
                        string text = getFileRequest;

                        // Grab stream URL.
                        var streamUrlLog = Regex.Match(getFileRequest, "url\":\"(?<streamUrl>[^\"]+)").Groups;
                        var streamUrl = streamUrlLog[1].Value;

                        // Remove backslashes from the stream URL to have a proper URL.
                        string pattern = @"(?<streamUrlFix>[^\\]+)";
                        string input = streamUrl;
                        RegexOptions options = RegexOptions.Multiline;

                        // Place proper stream URL into the stream URL textbox.
                        testURLBox.Invoke(new Action(() => testURLBox.Text = String.Empty));
                        foreach (Match m in Regex.Matches(input, pattern, options))
                        {
                            testURLBox.Invoke(new Action(() => testURLBox.AppendText(string.Format("{0}", m.Value))));
                        }

                        // Set stream string to streaming URL
                        stream = testURLBox.Text;
                    }
                    catch (Exception ex)
                    {
                        // If connection to API fails, or something is incorrect, show error info.
                        string getError = ex.ToString();
                        output.Invoke(new Action(() => output.Text = String.Empty));
                        output.Invoke(new Action(() => output.AppendText("Failed to get streaming URL. Error information below.\r\n\r\n")));
                        output.Invoke(new Action(() => output.AppendText(getError)));
                        enableBoxes(sender, e);
                        return;
                    }
                }
                else
                {
                    // If the hash can't be verified.
                    output.Invoke(new Action(() => output.AppendText("The hash can't be verified. Please retry.\r\n")));
                    enableBoxes(sender, e);
                    return;
                }
            }
        }

        private void disableBoxes(object sender, EventArgs e)
        {
            mp3Checkbox.Invoke(new Action(() => mp3Checkbox.Visible = false));
            flacLowCheckbox.Invoke(new Action(() => flacLowCheckbox.Visible = false));
            flacMidCheckbox.Invoke(new Action(() => flacMidCheckbox.Visible = false));
            flacHighCheckbox.Invoke(new Action(() => flacHighCheckbox.Visible = false));
            downloadButton.Invoke(new Action(() => downloadButton.Enabled = false));
        }

        private void enableBoxes(object sender, EventArgs e)
        {
            mp3Checkbox.Invoke(new Action(() => mp3Checkbox.Visible = true));
            flacLowCheckbox.Invoke(new Action(() => flacLowCheckbox.Visible = true));
            flacMidCheckbox.Invoke(new Action(() => flacMidCheckbox.Visible = true));
            flacHighCheckbox.Invoke(new Action(() => flacHighCheckbox.Visible = true));
            downloadButton.Invoke(new Action(() => downloadButton.Enabled = true));
        }

        #region Choosing / Opening folder
        private void selectFolder_Click(object sender, EventArgs e)
        {
            Thread t = new Thread((ThreadStart)(() =>
            {
                // Open Folder Browser to select path & Save the selection
                folderBrowserDialog.ShowDialog();
                Settings.Default.savedFolder = folderBrowserDialog.SelectedPath;
                Settings.Default.Save();
            }));

            // Run your code from a thread that joins the STA Thread
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
            t.Join();
        }

        private void openFolderButton_Click(object sender, EventArgs e)
        {
            // Open selcted folder
            if (folderBrowserDialog.SelectedPath == null | folderBrowserDialog.SelectedPath == "")
            {
                // If there's no selected path.
                MessageBox.Show("No path selected!", "ERROR",
                MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                mp3Checkbox.Invoke(new Action(() => mp3Checkbox.Visible = true));
                flacLowCheckbox.Invoke(new Action(() => flacLowCheckbox.Visible = true));
                flacMidCheckbox.Invoke(new Action(() => flacMidCheckbox.Visible = true));
                flacHighCheckbox.Invoke(new Action(() => flacHighCheckbox.Visible = true));
                downloadButton.Invoke(new Action(() => downloadButton.Enabled = true));
                return;
            }
            else
            {
                // If selected path doesn't exist, create it. (Will be ignored if it does)
                System.IO.Directory.CreateDirectory(folderBrowserDialog.SelectedPath);
                // Open selcted folder
                Process.Start(@folderBrowserDialog.SelectedPath);
            }
        }
        #endregion

        #region Getting Type of URL
        private void getLinkTypeBG_DoWork(object sender, DoWorkEventArgs e)
        {
            disableBoxes(sender, e);

            // Check if there's no selected path.
            if (folderBrowserDialog.SelectedPath == null | folderBrowserDialog.SelectedPath == "")
            {
                // If there is NOT a saved path.
                output.Invoke(new Action(() => output.Text = String.Empty));
                output.Invoke(new Action(() => output.AppendText("No path has been set! Remember to Choose a Folder!\r\n")));
                enableBoxes(sender, e);
                return;
            }

            string albumLink = albumUrl.Text;

            var albumLinkIdGrab = Regex.Match(albumLink, "https:\\/\\/(?:.*?).qobuz.com\\/(?<type>.*?)\\/(?<id>.*?)$").Groups;
            var linkType = albumLinkIdGrab[1].Value;
            var albumLinkId = albumLinkIdGrab[2].Value;

            albumId = albumLinkId;

            if (linkType == "album")
            {
                downloadAlbumBG.RunWorkerAsync();
            }
            else if (linkType == "track")
            {
                downloadTrackBG.RunWorkerAsync();
            }
            else if (linkType == "artist")
            {
                downloadDiscogBG.RunWorkerAsync();
            }
            else if (linkType == "label")
            {
                downloadLabelBG.RunWorkerAsync();
            }
            else if (linkType == "user")
            {
                if (albumId == @"library/favorites/albums")
                {
                    downloadFaveAlbumsBG.RunWorkerAsync();
                }
                //else if (albumId == @"library/favorites/artists")
                //{
                //    downloadFaveArtistsBG.RunWorkerAsync();
                //}
                else
                {
                    output.Invoke(new Action(() => output.Text = String.Empty));
                    output.Invoke(new Action(() => output.AppendText("Downloading favorites only works on favorite albums at the moment. More options will be added in the future.\r\n\r\nIf you'd like to go ahead and grab your favorite albums, paste this link in the URL section - https://play.qobuz.com/user/library/favorites/albums")));
                    enableBoxes(sender, e);
                    return;
                }
            }
            else if (linkType == "playlist")
            {
                // Say what isn't available at the moment.
                output.Invoke(new Action(() => output.Text = String.Empty));
                output.Invoke(new Action(() => output.AppendText("Downloading playlists is not available right now. Maybe in the future. Sorry.")));
                enableBoxes(sender, e);
                return;
            }
            else
            {
                // Say what isn't available at the moment.
                output.Invoke(new Action(() => output.Text = String.Empty));
                output.Invoke(new Action(() => output.AppendText("URL not understood. Is there a typo?")));
                enableBoxes(sender, e);
                return;
            }
        }
        #endregion

        #region Downloading Based on URL
        // For downloading "artist" links [MOSTLY WORKING]
        private async void downloadDiscogBG_DoWork(object sender, DoWorkEventArgs e)
        {
            #region If URL has "artist"
            // Set "loc" as the selected path.
            String loc = folderBrowserDialog.SelectedPath;

            // Create HttpClient to grab Favorites ID
            var artistClient = new HttpClient();
            // Run through TLS to allow secure connection.
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            // Set user-agent to Firefox.
            artistClient.DefaultRequestHeaders.Add("User-Agent", userAgent);
            // Set referer to localhost that mora qualitas uses
            artistClient.DefaultRequestHeaders.Add("Referer", "https://play.qobuz.com/album/" + albumId);

            // Empty output, then say Grabbing IDs.
            output.Invoke(new Action(() => output.Text = String.Empty));
            output.Invoke(new Action(() => output.AppendText("Grabbing Album IDs...\r\n\r\n")));

            try
            {
                // Grab response from Qobuz to get Track IDs from Album response.
                var artistUrl = "https://www.qobuz.com/api.json/0.2/artist/get?artist_id=" + albumId + "&extra=albums%2Cfocus&offset=0&limit=9999999999&sort=release_desc&app_id=" + appid + "&user_auth_token=" + userAuth;
                var artistResponse = await artistClient.GetAsync(artistUrl);
                string artistResponseString = artistResponse.Content.ReadAsStringAsync().Result;

                // Grab all Album IDs listed on the API.
                string artistAlbumIDspattern = ",\"maximum_channel_count\":(?<notUsed>.*?),\"id\":\"(?<albumIds>.*?)\",";
                string artistAlbumIDsInput = artistResponseString;
                RegexOptions artistAlbumIDsOptions = RegexOptions.Multiline;

                foreach (Match m in Regex.Matches(artistAlbumIDsInput, artistAlbumIDspattern, artistAlbumIDsOptions))
                {
                    // Make sure buttons are disabled during downloads.
                    disableBoxes(sender, e);

                    string albumIDArtist = string.Format("{0}", m.Groups["albumIds"].Value);

                    // Create HttpClient to grab Album ID
                    var albumIDClient = new HttpClient();
                    // Run through TLS to allow secure connection.
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                    // Set user-agent to Firefox.
                    albumIDClient.DefaultRequestHeaders.Add("User-Agent", userAgent);
                    // Set referer to localhost that mora qualitas uses
                    albumIDClient.DefaultRequestHeaders.Add("Referer", "https://play.qobuz.com/album/" + albumIDArtist);

                    // Empty output, then say Starting Downloads.
                    output.Invoke(new Action(() => output.Text = String.Empty));
                    output.Invoke(new Action(() => output.AppendText("Starting Downloads...\r\n\r\n")));

                    try
                    {
                        // Grab response from Qobuz to get Track IDs from Album response.
                        var albumIDUrl = "https://www.qobuz.com/api.json/0.2/album/get?album_id=" + albumIDArtist + "&app_id=" + appid + "&user_auth_token=" + userAuth;
                        var albumIDResponse = await albumIDClient.GetAsync(albumIDUrl);
                        string albumIDResponseString = albumIDResponse.Content.ReadAsStringAsync().Result;

                        // Grab metadata from API JSON response
                        JObject joAlbumResponse = JObject.Parse(albumIDResponseString);

                        #region Availability Check (Valid Link?)
                        // Check if available at all.
                        string errorCheckAlbum = (string)joAlbumResponse["code"];
                        string errorMessageCheck = (string)joAlbumResponse["message"];

                        switch (errorCheckAlbum)
                        {
                            case "404":
                                output.Invoke(new Action(() => output.Text = String.Empty));
                                output.Invoke(new Action(() => output.AppendText("ERROR: 404\r\n")));
                                output.Invoke(new Action(() => output.AppendText("Error message is \"" + errorMessageCheck + "\"\r\n")));
                                output.Invoke(new Action(() => output.AppendText("This usually means the link is invalid, or isn't available in the region your account is from.")));
                                enableBoxes(sender, e);
                                return;
                        }
                        #endregion

                        #region Quality Info (Bitrate & Sample Rate)
                        // Grab sample rate and bit depth for album track is from.
                        var bitDepth = (string)joAlbumResponse["maximum_bit_depth"];
                        var sampleRate = (string)joAlbumResponse["maximum_sampling_rate"];

                        var quality = "FLAC (" + bitDepth + "bit/" + sampleRate + "kHz)";
                        var qualityPath = quality.Replace(@"\", "-").Replace(@"/", "-");

                        switch (formatIdString)
                        {
                            case "5":
                                quality = "MP3 320kbps CBR";
                                qualityPath = "MP3";
                                break;
                            case "6":
                                quality = "FLAC (16bit/44.1kHz)";
                                qualityPath = "FLAC (16bit-44.1kHz)";
                                break;
                            case "7":
                                if (quality == "FLAC (24bit/192kHz)")
                                {
                                    quality = "FLAC (24bit/96kHz)";
                                    qualityPath = "FLAC (24bit-96kHz)";
                                }
                                break;
                        }

                        // Display album quality in quality textbox.
                        qualityTextbox.Invoke(new Action(() => qualityTextbox.Text = quality));
                        #endregion

                        #region Cover Art URL
                        // Grab cover art link
                        frontCoverImg = (string)joAlbumResponse["image"]["large"];
                        // Get 150x150 artwork for cover art box
                        frontCoverImgBox = frontCoverImg.Replace("_600.jpg", "_150.jpg");
                        // Get max sized artwork
                        frontCoverImg = frontCoverImg.Replace("_600.jpg", "_max.jpg");

                        albumArtPicBox.Invoke(new Action(() => albumArtPicBox.ImageLocation = frontCoverImgBox));
                        #endregion

                        #region "Goodies" URL (Digital Booklets)
                        // Look for "Goodies" (digital booklet)
                        var goodiesLog = Regex.Match(albumIDResponseString, "\"goodies\":\\[{(?<notUsed>.*?),\"url\":\"(?<booklet>.*?)\",").Groups;
                        var goodiesPDF = goodiesLog[2].Value.Replace(@"\/", "/");
                        #endregion

                        // Grab all Track IDs listed on the API.
                        string trackIDsPattern = "\"version\":(?:.*?),\"id\":(?<trackId>.*?),";
                        string trackIDsInput = albumIDResponseString;
                        RegexOptions trackIDsOptions = RegexOptions.Multiline;

                        foreach (Match m2 in Regex.Matches(trackIDsInput, trackIDsPattern, trackIDsOptions))
                        {
                            // Grab matches for Track IDs
                            trackIdString = string.Format("{0}", m2.Groups["trackId"].Value);

                            // Create HttpClient to grab Track ID
                            var trackIDClient = new HttpClient();
                            // Run through TLS to allow secure connection.
                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                            // Set user-agent to Firefox.
                            trackIDClient.DefaultRequestHeaders.Add("User-Agent", userAgent);
                            // Set referer to localhost that mora qualitas uses
                            trackIDClient.DefaultRequestHeaders.Add("Referer", "https://play.qobuz.com/album/" + albumIDArtist);

                            // Grab response from Qobuz to get info using Track IDs.
                            var trackIDUrl = "https://www.qobuz.com/api.json/0.2/track/get?track_id=" + trackIdString + "&app_id=" + appid + "&user_auth_token=" + userAuth;
                            var trackIDResponse = await trackIDClient.GetAsync(trackIDUrl);
                            string trackIDResponseString = trackIDResponse.Content.ReadAsStringAsync().Result;

                            // Grab metadata from API JSON response
                            JObject joTrackResponse = JObject.Parse(trackIDResponseString);

                            #region Get Information (Tags, Titles, etc.)
                            // Grab tag strings
                            albumArtist = (string)joTrackResponse["album"]["artist"]["name"]; albumArtist = DecodeEncodedNonAsciiCharacters(albumArtist);
                            albumArtistPath = GetSafeFilename(albumArtist);
                            albumArtistTextBox.Invoke(new Action(() => albumArtistTextBox.Text = albumArtist));

                            try
                            {
                                performerName = (string)joTrackResponse["performer"]["name"]; performerName = DecodeEncodedNonAsciiCharacters(performerName);
                                performerNamePath = GetSafeFilename(performerName);
                            }
                            catch { performerName = null; performerNamePath = null; /*Set to null and Ignore if fails*/ }

                            try { composerName = (string)joTrackResponse["composer"]["name"]; composerName = DecodeEncodedNonAsciiCharacters(composerName); } catch { /*Ignore if fails*/ }

                            advisory = (string)joTrackResponse["parental_warning"];

                            albumName = (string)joTrackResponse["album"]["title"]; albumName = DecodeEncodedNonAsciiCharacters(albumName);
                            albumNamePath = GetSafeFilename(albumName);
                            albumTextBox.Invoke(new Action(() => albumTextBox.Text = albumName));

                            trackName = (string)joTrackResponse["title"]; trackName = trackName.Trim(); trackName = DecodeEncodedNonAsciiCharacters(trackName);
                            trackNamePath = GetSafeFilename(trackName);

                            versionName = (string)joTrackResponse["version"];
                            if (versionName != null)
                            {
                                versionName = DecodeEncodedNonAsciiCharacters(versionName);
                                versionNamePath = GetSafeFilename(versionName);
                            }

                            genre = (string)joTrackResponse["album"]["genre"]["name"]; genre = DecodeEncodedNonAsciiCharacters(genre);

                            releaseDate = (string)joTrackResponse["album"]["release_date_stream"];
                            releaseDateTextBox.Invoke(new Action(() => releaseDateTextBox.Text = releaseDate));

                            copyright = (string)joTrackResponse["copyright"]; copyright = DecodeEncodedNonAsciiCharacters(copyright);

                            upc = (string)joTrackResponse["album"]["upc"];
                            upcTextBox.Invoke(new Action(() => upcTextBox.Text = upc));

                            isrc = (string)joTrackResponse["isrc"];

                            type = (string)joTrackResponse["album"]["release_type"];

                            // Grab tag ints
                            trackNumber = (int)joTrackResponse["track_number"];

                            trackTotal = (int)joTrackResponse["album"]["tracks_count"];
                            totalTracksTextbox.Invoke(new Action(() => totalTracksTextbox.Text = trackTotal.ToString()));

                            discNumber = (int)joTrackResponse["media_number"];

                            discTotal = (int)joTrackResponse["album"]["media_count"];

                            // Debug output to make sure values are grabbed properly
                            //output.Invoke(new Action(() => output.AppendText("Tags found, listed below...\r\n")));
                            //output.Invoke(new Action(() => output.AppendText("  Album Artist - " + albumArtist + "\r\n")));
                            //output.Invoke(new Action(() => output.AppendText("  Track Artist - " + performerName + "\r\n")));
                            //output.Invoke(new Action(() => output.AppendText("  Composer - " + composerName + "\r\n")));
                            //output.Invoke(new Action(() => output.AppendText("  Advisory - " + advisory + "\r\n")));
                            //output.Invoke(new Action(() => output.AppendText("  Album Name - " + albumName + "\r\n")));
                            //output.Invoke(new Action(() => output.AppendText("  Track Name - " + trackName + "\r\n")));
                            //output.Invoke(new Action(() => output.AppendText("  Track Version - " + versionName + "\r\n")));
                            //output.Invoke(new Action(() => output.AppendText("  Genre - " + genre + "\r\n")));
                            //output.Invoke(new Action(() => output.AppendText("  Release Date - " + releaseDate + "\r\n")));
                            //output.Invoke(new Action(() => output.AppendText("  Copyright - " + copyright + "\r\n")));
                            //output.Invoke(new Action(() => output.AppendText("  UPC - " + upc + "\r\n")));
                            //output.Invoke(new Action(() => output.AppendText("  ISRC - " + isrc + "\r\n")));
                            //output.Invoke(new Action(() => output.AppendText("  Media Type - " + type + "\r\n")));
                            //output.Invoke(new Action(() => output.AppendText("  Track Number - " + trackNumber.ToString() + "\r\n")));
                            //output.Invoke(new Action(() => output.AppendText("  Track Total - " + trackTotal.ToString() + "\r\n")));
                            //output.Invoke(new Action(() => output.AppendText("  Disc Number - " + discNumber.ToString() + "\r\n")));
                            //output.Invoke(new Action(() => output.AppendText("  Disc Total - " + discTotal.ToString() + "\r\n")));

                            #region Availability Check (Streamable?)
                            // Check if available for streaming.
                            string streamCheck = (string)joTrackResponse["streamable"];

                            switch (streamCheck.ToLower())
                            {
                                case "true":
                                    break;
                                default:
                                    switch (streamableCheckbox.Checked)
                                    {
                                        case true:
                                            output.Invoke(new Action(() => output.AppendText("Track " + trackNumber.ToString() + " is not available for streaming. Unable to download.\r\n")));
                                            System.Threading.Thread.Sleep(100);
                                            enableBoxes(sender, e);
                                            continue;
                                        default:
                                            output.Invoke(new Action(() => output.AppendText("Track is not available for streaming. But stremable check is being ignored for debugging, or messed up releases. Attempting to download...\r\n")));
                                            break;
                                    }
                                    break;
                            }
                            #endregion

                            #endregion

                            #region Filename Number Padding
                            // Set default track number padding length
                            var paddingLength = 2;

                            // Prepare track number padding in filename.
                            string paddingLog = Math.Floor(Math.Log10(trackTotal) + 1).ToString();

                            switch (paddingLog)
                            {
                                case "1":
                                    paddingLength = 2;
                                    break;
                                default:
                                    paddingLength = (int)Math.Floor(Math.Log10(trackTotal) + 1);
                                    break;
                            }

                            // Set default disc number padding length
                            var paddingDiscLength = 2;

                            // Prepare disc number padding in filename.
                            string paddingDiscLog = Math.Floor(Math.Log10(discTotal) + 1).ToString();

                            switch (paddingDiscLog)
                            {
                                case "1":
                                    paddingDiscLength = 2;
                                    break;
                                default:
                                    paddingDiscLength = (int)Math.Floor(Math.Log10(trackTotal) + 1);
                                    break;
                            }
                            #endregion

                            #region Create Shortened Strings
                            // If name goes over 36 characters, limit it to 36
                            if (albumArtistPath.Length > MaxLength)
                            {
                                albumArtistPath = albumArtistPath.Substring(0, MaxLength).TrimEnd();
                            }

                            if (performerName != null)
                            {
                                if (performerNamePath.Length > MaxLength)
                                {
                                    performerNamePath = performerNamePath.Substring(0, MaxLength).TrimEnd();
                                }
                            }

                            if (albumNamePath.Length > MaxLength)
                            {
                                albumNamePath = albumNamePath.Substring(0, MaxLength).TrimEnd();
                            }
                            #endregion

                            #region Create Directories
                            // Create strings for disc folders
                            string discFolder = null;

                            // If more than 1 disc, create folders for discs. Otherwise, strings will remain null.
                            if (discTotal > 1)
                            {
                                discFolder = "CD " + discNumber.ToString().PadLeft(paddingDiscLength, '0');
                            }

                            // Create directories
                            string[] path1 = { loc, albumArtistPath };
                            path1Full = Path.Combine(path1);
                            string[] path2 = { loc, albumArtistPath, albumNamePath + " [" + albumIDArtist + "]" };
                            path2Full = Path.Combine(path2);
                            string[] path3 = { loc, albumArtistPath, albumNamePath + " [" + albumIDArtist + "]", qualityPath };
                            path3Full = Path.Combine(path3);

                            switch (discTotal)
                            {
                                case 1:
                                    path4Full = path3Full;
                                    break;
                                default:
                                    string[] path4 = { loc, albumArtistPath, albumNamePath + " [" + albumIDArtist + "]", qualityPath, discFolder };
                                    path4Full = Path.Combine(path4);
                                    break;
                            }

                            System.IO.Directory.CreateDirectory(path1Full);
                            System.IO.Directory.CreateDirectory(path2Full);
                            System.IO.Directory.CreateDirectory(path3Full);
                            System.IO.Directory.CreateDirectory(path4Full);

                            // Set albumPath to the created directories.
                            string trackPath = path4Full;
                            #endregion

                            #region Create Shortened Strings (Again)
                            // Create final shortened track file names to avoid errors with file names being too long.
                            switch (versionName)
                            {
                                case null:
                                    if ((trackNumber.ToString().PadLeft(paddingLength, '0') + " " + trackNamePath).Length > MaxLength)
                                    {
                                        finalTrackNamePath = (trackNumber.ToString().PadLeft(paddingLength, '0') + " " + trackNamePath).Substring(0, MaxLength).TrimEnd();
                                    }
                                    else
                                    {
                                        finalTrackNamePath = (trackNumber.ToString().PadLeft(paddingLength, '0') + " " + trackNamePath).TrimEnd();
                                    }
                                    break;
                                default:
                                    if ((trackNumber.ToString().PadLeft(paddingLength, '0') + " " + trackNamePath + " (" + versionNamePath + ")").Length > MaxLength)
                                    {
                                        finalTrackNameVersionPath = (trackNumber.ToString().PadLeft(paddingLength, '0') + " " + trackNamePath + " (" + versionNamePath + ")").Substring(0, MaxLength).TrimEnd();
                                    }
                                    else
                                    {
                                        finalTrackNameVersionPath = (trackNumber.ToString().PadLeft(paddingLength, '0') + " " + trackNamePath + " (" + versionNamePath + ")").TrimEnd();
                                    }
                                    break;
                            }
                            #endregion

                            #region Check if File Exists
                            // Check if there is a version name.
                            switch (versionName)
                            {
                                case null:
                                    string[] path6 = { trackPath, finalTrackNamePath + audioFileType };
                                    string checkFile = Path.Combine(path6);

                                    if (System.IO.File.Exists(checkFile))
                                    {
                                        output.Invoke(new Action(() => output.AppendText("File for \"" + trackNumber.ToString().PadLeft(paddingLength, '0') + " " + trackName + "\" already exists. Skipping.\r\n")));
                                        System.Threading.Thread.Sleep(100);
                                        continue;
                                    }
                                    break;
                                default:
                                    string[] path6Version = { trackPath, finalTrackNameVersionPath + audioFileType };
                                    string checkFileVersion = Path.Combine(path6Version);

                                    if (System.IO.File.Exists(checkFileVersion))
                                    {
                                        output.Invoke(new Action(() => output.AppendText("File for \"" + trackNumber.ToString().PadLeft(paddingLength, '0') + " " + trackName + " (" + versionName + ")" + "\" already exists. Skipping.\r\n")));
                                        System.Threading.Thread.Sleep(100);
                                        continue;
                                    }
                                    break;
                            }
                            #endregion

                            // Create streaming URL.
                            createURL(sender, e);

                            try
                            {
                                #region Downloading
                                // Check if there is a version name.
                                switch (versionName)
                                {
                                    case null:
                                        output.Invoke(new Action(() => output.AppendText("Downloading - " + trackNumber.ToString().PadLeft(paddingLength, '0') + " - " + trackName + " ......")));
                                        break;
                                    default:
                                        output.Invoke(new Action(() => output.AppendText("Downloading - " + trackNumber.ToString().PadLeft(paddingLength, '0') + " - " + trackName + " (" + versionName + ")" + " ......")));
                                        break;
                                }

                                // Save streamed file from link
                                using (HttpClient streamClient = new HttpClient())
                                {
                                    // Set "range" header to nearly unlimited.
                                    streamClient.DefaultRequestHeaders.Range = new RangeHeaderValue(0, 999999999999);
                                    // Set user-agent to Firefox.
                                    streamClient.DefaultRequestHeaders.Add("User-Agent", userAgent);
                                    // Set referer URL to album ID.
                                    streamClient.DefaultRequestHeaders.Add("Referer", "https://play.qobuz.com/album/" + albumId);

                                    using (HttpResponseMessage streamResponse = await streamClient.GetAsync(stream, HttpCompletionOption.ResponseHeadersRead))
                                    using (Stream streamToReadFrom = await streamResponse.Content.ReadAsStreamAsync())
                                    {
                                        string fileName = Path.GetTempFileName();
                                        using (Stream streamToWriteTo = System.IO.File.Open(fileName, FileMode.Create))
                                        {
                                            await streamToReadFrom.CopyToAsync(streamToWriteTo);
                                        }

                                        switch (versionName)
                                        {
                                            case null:
                                                string[] path6 = { trackPath, finalTrackNamePath + audioFileType };
                                                string filePath = Path.Combine(path6);

                                                System.IO.File.Move(fileName, filePath);
                                                break;
                                            default:
                                                string[] path6Version = { trackPath, finalTrackNameVersionPath + audioFileType };
                                                string filePathVersion = Path.Combine(path6Version);

                                                System.IO.File.Move(fileName, filePathVersion);
                                                break;
                                        }
                                    }
                                }
                                #endregion

                                #region Cover Art Saving
                                string[] path7 = { loc, albumArtistPath, albumNamePath + " [" + albumIDArtist + "]", qualityPath, "Cover.jpg" };
                                string coverArtPath = Path.Combine(path7);
                                string[] path7Tag = { loc, albumArtistPath, albumNamePath + " [" + albumIDArtist + "]", qualityPath, artSize + ".jpg" };
                                string coverArtTagPath = Path.Combine(path7Tag);

                                if (System.IO.File.Exists(coverArtPath))
                                {
                                    try
                                    {
                                        // Skip, don't re-download.

                                        // Download selected cover art size for tagging files (Currently happens every time a track is downloaded).
                                        using (WebClient imgClient = new WebClient())
                                        {
                                            imgClient.DownloadFile(new Uri(frontCoverImg.Replace("_max", "_" + artSize)), coverArtTagPath);
                                        }
                                    }
                                    catch
                                    {
                                        // Ignore, Qobuz servers throw a 404 as if the image doesn't exist.
                                    }
                                }
                                else
                                {
                                    try
                                    {
                                        // Save cover art to selected path.
                                        using (WebClient imgClient = new WebClient())
                                        {
                                            // Download max quality Cover Art to "Cover.jpg" file in chosen path. 
                                            imgClient.DownloadFile(new Uri(frontCoverImg), coverArtPath);

                                            // Download selected cover art size for tagging files (Currently happens every time a track is downloaded).
                                            imgClient.DownloadFile(new Uri(frontCoverImg.Replace("_max", "_" + artSize)), coverArtTagPath);
                                        }
                                    }
                                    catch
                                    {
                                        // Ignore, Qobuz servers throw a 404 as if the image doesn't exist.
                                    }
                                }
                                #endregion

                                #region Tagging
                                switch (versionName)
                                {
                                    case null:
                                        break;
                                    default:
                                        finalTrackNamePath = finalTrackNameVersionPath;
                                        break;
                                }

                                string[] path8 = { trackPath, finalTrackNamePath + audioFileType };
                                string tagFilePath = Path.Combine(path8);
                                string[] path9 = { loc, albumArtistPath, albumNamePath + " [" + albumIDArtist + "]", qualityPath, artSize + ".jpg" };
                                string tagCoverArtFilePath = Path.Combine(path9);

                                // Set file to tag
                                var tfile = TagLib.File.Create(tagFilePath);

                                switch (audioFileType)
                                {
                                    case ".mp3":
                                        #region MP3 Tagging
                                        // For custom / troublesome tags.
                                        TagLib.Id3v2.Tag t = (TagLib.Id3v2.Tag)tfile.GetTag(TagLib.TagTypes.Id3v2);

                                        // Saving cover art to file(s)
                                        if (imageCheckbox.Checked == true)
                                        {
                                            try
                                            {
                                                // Define cover art to use for MP3 file(s)
                                                TagLib.Id3v2.AttachedPictureFrame pic = new TagLib.Id3v2.AttachedPictureFrame();
                                                pic.TextEncoding = TagLib.StringType.Latin1;
                                                pic.MimeType = System.Net.Mime.MediaTypeNames.Image.Jpeg;
                                                pic.Type = TagLib.PictureType.FrontCover;
                                                pic.Data = TagLib.ByteVector.FromPath(tagCoverArtFilePath);

                                                // Save cover art to MP3 file.
                                                tfile.Tag.Pictures = new TagLib.IPicture[1] { pic };
                                                tfile.Save();
                                            }
                                            catch
                                            {
                                                output.Invoke(new Action(() => output.AppendText("Cover art tag fail, .jpg still exists?...")));
                                            }
                                        }

                                        // Track Title tag
                                        if (trackTitleCheckbox.Checked == true)
                                        {
                                            switch (versionName)
                                            {
                                                case null:
                                                    tfile.Tag.Title = trackName;
                                                    break;
                                                default:
                                                    tfile.Tag.Title = trackName + " (" + versionName + ")";
                                                    break;
                                            }

                                        }

                                        // Album Title tag
                                        if (albumCheckbox.Checked == true) { tfile.Tag.Album = albumName; }

                                        // Album Artits tag
                                        if (albumArtistCheckbox.Checked == true) { tfile.Tag.AlbumArtists = new string[] { albumArtist }; }

                                        // Track Artist tag
                                        if (artistCheckbox.Checked == true) { tfile.Tag.Performers = new string[] { performerName }; }

                                        // Composer tag
                                        if (composerCheckbox.Checked == true) { tfile.Tag.Composers = new string[] { composerName }; }

                                        // Release Date tag
                                        if (releaseCheckbox.Checked == true) { releaseDate = releaseDate.Substring(0, 4); tfile.Tag.Year = UInt32.Parse(releaseDate); }

                                        // Genre tag
                                        if (genreCheckbox.Checked == true) { tfile.Tag.Genres = new string[] { genre }; }

                                        // Track Number tag
                                        if (trackNumberCheckbox.Checked == true) { tfile.Tag.Track = Convert.ToUInt32(trackNumber); }

                                        // Disc Number tag
                                        if (discNumberCheckbox.Checked == true) { tfile.Tag.Disc = Convert.ToUInt32(discNumber); }

                                        // Total Discs tag
                                        if (discTotalCheckbox.Checked == true) { tfile.Tag.DiscCount = Convert.ToUInt32(discTotal); }

                                        // Total Tracks tag
                                        if (trackTotalCheckbox.Checked == true) { tfile.Tag.TrackCount = Convert.ToUInt32(trackTotal); }

                                        // Comment tag
                                        if (commentCheckbox.Checked == true) { tfile.Tag.Comment = commentTextbox.Text; }

                                        // Copyright tag
                                        if (copyrightCheckbox.Checked == true) { tfile.Tag.Copyright = copyright; }

                                        // ISRC tag
                                        if (isrcCheckbox.Checked == true) { TagLib.Id3v2.Tag tag = (TagLib.Id3v2.Tag)tfile.GetTag(TagTypes.Id3v2, true); tag.SetTextFrame("TSRC", isrc); }

                                        // Release Type tag
                                        if (type != null)
                                        {
                                            if (typeCheckbox.Checked == true) { TagLib.Id3v2.Tag tag = (TagLib.Id3v2.Tag)tfile.GetTag(TagTypes.Id3v2, true); tag.SetTextFrame("TMED", type); }
                                        }

                                        // Save all selected tags to file
                                        tfile.Save();
                                        #endregion
                                        break;
                                    case ".flac":
                                        #region FLAC Tagging
                                        // For custom / troublesome tags.
                                        var custom = (TagLib.Ogg.XiphComment)tfile.GetTag(TagLib.TagTypes.Xiph);

                                        // Saving cover art to file(s)
                                        if (imageCheckbox.Checked == true)
                                        {
                                            try
                                            {
                                                // Define cover art to use for FLAC file(s)
                                                TagLib.Id3v2.AttachedPictureFrame pic = new TagLib.Id3v2.AttachedPictureFrame();
                                                pic.TextEncoding = TagLib.StringType.Latin1;
                                                pic.MimeType = System.Net.Mime.MediaTypeNames.Image.Jpeg;
                                                pic.Type = TagLib.PictureType.FrontCover;
                                                pic.Data = TagLib.ByteVector.FromPath(tagCoverArtFilePath);

                                                // Save cover art to FLAC file.
                                                tfile.Tag.Pictures = new TagLib.IPicture[1] { pic };
                                                tfile.Save();
                                            }
                                            catch
                                            {
                                                output.Invoke(new Action(() => output.AppendText("Cover art tag fail, .jpg still exists?...")));
                                            }
                                        }

                                        // Track Title tag
                                        if (trackTitleCheckbox.Checked == true)
                                        {
                                            switch (versionName)
                                            {
                                                case null:
                                                    tfile.Tag.Title = trackName;
                                                    break;
                                                default:
                                                    tfile.Tag.Title = trackName + " (" + versionName + ")";
                                                    break;
                                            }
                                        }

                                        // Album Title tag
                                        if (albumCheckbox.Checked == true) { tfile.Tag.Album = albumName; }

                                        // Album Artits tag
                                        if (albumArtistCheckbox.Checked == true) { custom.SetField("ALBUMARTIST", new string[] { albumArtist }); }

                                        // Track Artist tag
                                        if (artistCheckbox.Checked == true) { custom.SetField("ARTIST", new string[] { performerName }); }

                                        // Composer tag
                                        if (composerCheckbox.Checked == true) { custom.SetField("COMPOSER", new string[] { composerName }); }

                                        // Release Date tag
                                        if (releaseCheckbox.Checked == true) { custom.SetField("YEAR", new string[] { releaseDate }); }

                                        // Genre tag
                                        if (genreCheckbox.Checked == true) { custom.SetField("GENRE", new string[] { genre }); }

                                        // Track Number tag
                                        if (trackNumberCheckbox.Checked == true) { tfile.Tag.Track = Convert.ToUInt32(trackNumber); }

                                        // Disc Number tag
                                        if (discNumberCheckbox.Checked == true) { tfile.Tag.Disc = Convert.ToUInt32(discNumber); }

                                        // Total Discs tag
                                        if (discTotalCheckbox.Checked == true) { tfile.Tag.DiscCount = Convert.ToUInt32(discTotal); }

                                        // Total Tracks tag
                                        if (trackTotalCheckbox.Checked == true) { tfile.Tag.TrackCount = Convert.ToUInt32(trackTotal); }

                                        // Comment tag
                                        if (commentCheckbox.Checked == true) { custom.SetField("COMMENT", new string[] { commentTextbox.Text }); }

                                        // Copyright tag
                                        if (copyrightCheckbox.Checked == true) { custom.SetField("COPYRIGHT", new string[] { copyright }); }
                                        // UPC tag
                                        if (upcCheckbox.Checked == true) { custom.SetField("UPC", new string[] { upc }); }

                                        // ISRC tag
                                        if (isrcCheckbox.Checked == true) { custom.SetField("ISRC", new string[] { isrc }); }

                                        // Release Type tag
                                        if (type != null)
                                        {
                                            if (typeCheckbox.Checked == true) { custom.SetField("MEDIATYPE", new string[] { type }); }
                                        }

                                        // Explicit tag
                                        if (explicitCheckbox.Checked == true)
                                        {
                                            if (advisory == "false") { custom.SetField("ITUNESADVISORY", new string[] { "0" }); } else { custom.SetField("ITUNESADVISORY", new string[] { "1" }); }
                                        }

                                        // Save all selected tags to file
                                        tfile.Save();
                                        #endregion
                                        break;
                                }
                                #endregion
                            }
                            catch (Exception downloadError)
                            {
                                // If there is an issue trying to, or during the download, show error info.
                                string error = downloadError.ToString();
                                output.Invoke(new Action(() => output.AppendText("\r\n\r\n")));
                                output.Invoke(new Action(() => output.AppendText("Track Download ERROR. Information below.\r\n\r\n")));
                                output.Invoke(new Action(() => output.AppendText(error)));
                                enableBoxes(sender, e);
                                return;
                            }

                            // Delete image file used for tagging
                            string[] path11 = { loc, albumArtistPath, albumNamePath + " [" + albumIDArtist + "]", qualityPath, artSize + ".jpg" };
                            string coverArtTagDelete = Path.Combine(path11);

                            if (System.IO.File.Exists(coverArtTagDelete))
                            {
                                System.IO.File.Delete(coverArtTagDelete);
                            }

                            // Say when a track is done downloading, then wait for the next track / end.
                            output.Invoke(new Action(() => output.AppendText("Track Download Done!\r\n")));
                            System.Threading.Thread.Sleep(100);
                        }

                        #region Digital Booklet
                        string[] path12 = { loc, albumArtistPath, albumNamePath + " [" + albumIDArtist + "]", qualityPath, "Digital Booklet.pdf" };
                        string goodiesPath = Path.Combine(path12);
                        // If a booklet was found, save it.
                        if (goodiesPDF == null | goodiesPDF == "")
                        {
                            // No need to download something that doesn't exist.
                        }
                        else
                        {
                            if (System.IO.File.Exists(goodiesPath))
                            {
                                // Skip, don't re-download.
                            }
                            else
                            {
                                // Save digital booklet to selected path
                                output.Invoke(new Action(() => output.AppendText("Goodies found, downloading...")));
                                using (WebClient bookClient = new WebClient())
                                {
                                    // Download max quality Cover Art to "Cover.jpg" file in chosen path. 
                                    bookClient.DownloadFile(new Uri(goodiesPDF), goodiesPath);
                                }
                            }
                        }
                        #endregion

                        // Say that downloading is completed.
                        output.Invoke(new Action(() => output.AppendText("\r\n\r\n")));
                        output.Invoke(new Action(() => output.AppendText("Downloading job completed! All downloaded files will be located in your chosen path.")));
                        enableBoxes(sender, e);
                    }
                    catch (Exception ex)
                    {
                        string error = ex.ToString();
                        //output.Invoke(new Action(() => output.Text = String.Empty));
                        output.Invoke(new Action(() => output.AppendText("Failed to download (First Phase). Error information below.\r\n\r\n")));
                        output.Invoke(new Action(() => output.AppendText(error)));
                        enableBoxes(sender, e);
                        return;
                    }
                }
            }
            catch (Exception downloadError)
            {
                // If there is an issue trying to, or during the download, show error info.
                string error = downloadError.ToString();
                output.Invoke(new Action(() => output.Text = String.Empty));
                output.Invoke(new Action(() => output.AppendText("Artist Download ERROR. Information below.\r\n\r\n")));
                output.Invoke(new Action(() => output.AppendText(error)));
                enableBoxes(sender, e);
                return;
            }
            #endregion
        }

        // For downloading "label" links [IN DEV]
        private async void downloadLabelBG_DoWork(object sender, DoWorkEventArgs e)
        {
            #region If URL has "label"
            // Set "loc" as the selected path.
            String loc = folderBrowserDialog.SelectedPath;

            // Create HttpClient to grab Favorites ID
            var labelClient = new HttpClient();
            // Run through TLS to allow secure connection.
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            // Set user-agent to Firefox.
            labelClient.DefaultRequestHeaders.Add("User-Agent", userAgent);
            // Set referer to localhost that mora qualitas uses
            labelClient.DefaultRequestHeaders.Add("Referer", "https://play.qobuz.com/album/" + albumId);

            // Empty output, then say Grabbing IDs.
            output.Invoke(new Action(() => output.Text = String.Empty));
            output.Invoke(new Action(() => output.AppendText("Grabbing Album IDs...\r\n\r\n")));

            try
            {
                // Grab response from Qobuz to get Track IDs from Album response.
                var labelUrl = "https://www.qobuz.com/api.json/0.2/label/get?label_id=" + albumId + "&extra=albums%2Cfocus&offset=0&limit=999999999999&app_id=" + appid + "&user_auth_token=" + userAuth;
                var labelResponse = await labelClient.GetAsync(labelUrl);
                string labelResponseString = labelResponse.Content.ReadAsStringAsync().Result;

                // Grab metadata from API JSON response
                JObject joLabelResponse = JObject.Parse(labelResponseString);

                // Grab Label Name
                string labelName = (string)joLabelResponse["name"];
                labelName = labelName.Replace("\\\"", "\"").Replace(@"\\/", @"/").Replace(@"\\", @"\").Replace(@"\/", @"/");
                var labelNamePath = labelName;


                // Grab all Album IDs listed on the API.
                string labelAlbumIDspattern = ",\"maximum_channel_count\":(?<notUsed>.*?),\"id\":\"(?<albumIds>.*?)\",";
                string labelAlbumIDsInput = labelResponseString;
                RegexOptions labelAlbumIDsOptions = RegexOptions.Multiline;

                foreach (Match m in Regex.Matches(labelAlbumIDsInput, labelAlbumIDspattern, labelAlbumIDsOptions))
                {
                    // Make sure buttons are disabled during downloads.
                    disableBoxes(sender, e);

                    string albumIDLabel = string.Format("{0}", m.Groups["albumIds"].Value);

                    // Create HttpClient to grab Album ID
                    var albumIDClient = new HttpClient();
                    // Run through TLS to allow secure connection.
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                    // Set user-agent to Firefox.
                    albumIDClient.DefaultRequestHeaders.Add("User-Agent", userAgent);
                    // Set referer to localhost that mora qualitas uses
                    albumIDClient.DefaultRequestHeaders.Add("Referer", "https://play.qobuz.com/album/" + albumIDLabel);

                    // Empty output, then say Starting Downloads.
                    output.Invoke(new Action(() => output.Text = String.Empty));
                    output.Invoke(new Action(() => output.AppendText("Starting Downloads...\r\n\r\n")));

                    try
                    {
                        // Grab response from Qobuz to get Track IDs from Album response.
                        var albumIDUrl = "https://www.qobuz.com/api.json/0.2/album/get?album_id=" + albumIDLabel + "&app_id=" + appid + "&user_auth_token=" + userAuth;
                        var albumIDResponse = await albumIDClient.GetAsync(albumIDUrl);
                        string albumIDResponseString = albumIDResponse.Content.ReadAsStringAsync().Result;

                        // Grab metadata from API JSON response
                        JObject joAlbumResponse = JObject.Parse(albumIDResponseString);

                        #region Availability Check (Valid Link?)
                        // Check if available at all.
                        string errorCheckAlbum = (string)joAlbumResponse["code"];
                        string errorMessageCheck = (string)joAlbumResponse["message"];

                        switch (errorCheckAlbum)
                        {
                            case "404":
                                output.Invoke(new Action(() => output.Text = String.Empty));
                                output.Invoke(new Action(() => output.AppendText("ERROR: 404\r\n")));
                                output.Invoke(new Action(() => output.AppendText("Error message is \"" + errorMessageCheck + "\"\r\n")));
                                output.Invoke(new Action(() => output.AppendText("This usually means the link is invalid, or isn't available in the region your account is from.")));
                                enableBoxes(sender, e);
                                continue;
                        }
                        #endregion

                        #region Quality Info (Bitrate & Sample Rate)
                        // Grab sample rate and bit depth for album track is from.
                        var bitDepth = (string)joAlbumResponse["maximum_bit_depth"];
                        var sampleRate = (string)joAlbumResponse["maximum_sampling_rate"];

                        var quality = "FLAC (" + bitDepth + "bit/" + sampleRate + "kHz)";
                        var qualityPath = quality.Replace(@"\", "-").Replace(@"/", "-");

                        switch (formatIdString)
                        {
                            case "5":
                                quality = "MP3 320kbps CBR";
                                qualityPath = "MP3";
                                break;
                            case "6":
                                quality = "FLAC (16bit/44.1kHz)";
                                qualityPath = "FLAC (16bit-44.1kHz)";
                                break;
                            case "7":
                                if (quality == "FLAC (24bit/192kHz)")
                                {
                                    quality = "FLAC (24bit/96kHz)";
                                    qualityPath = "FLAC (24bit-96kHz)";
                                }
                                break;
                        }

                        // Display album quality in quality textbox.
                        qualityTextbox.Invoke(new Action(() => qualityTextbox.Text = quality));
                        #endregion

                        #region Cover Art URL
                        // Grab cover art link
                        frontCoverImg = (string)joAlbumResponse["image"]["large"];
                        // Get 150x150 artwork for cover art box
                        frontCoverImgBox = frontCoverImg.Replace("_600.jpg", "_150.jpg");
                        // Get max sized artwork
                        frontCoverImg = frontCoverImg.Replace("_600.jpg", "_max.jpg");

                        albumArtPicBox.Invoke(new Action(() => albumArtPicBox.ImageLocation = frontCoverImgBox));
                        #endregion

                        #region "Goodies" URL (Digital Booklets)
                        // Look for "Goodies" (digital booklet)
                        var goodiesLog = Regex.Match(albumIDResponseString, "\"goodies\":\\[{(?<notUsed>.*?),\"url\":\"(?<booklet>.*?)\",").Groups;
                        var goodiesPDF = goodiesLog[2].Value.Replace(@"\/", "/");
                        #endregion

                        // Grab all Track IDs listed on the API.
                        string trackIDsPattern = "\"version\":(?:.*?),\"id\":(?<trackId>.*?),";
                        string trackIDsInput = albumIDResponseString;
                        RegexOptions trackIDsOptions = RegexOptions.Multiline;

                        foreach (Match m2 in Regex.Matches(trackIDsInput, trackIDsPattern, trackIDsOptions))
                        {
                            // Grab matches for Track IDs
                            trackIdString = string.Format("{0}", m2.Groups["trackId"].Value);

                            // Create HttpClient to grab Track ID
                            var trackIDClient = new HttpClient();
                            // Run through TLS to allow secure connection.
                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                            // Set user-agent to Firefox.
                            trackIDClient.DefaultRequestHeaders.Add("User-Agent", userAgent);
                            // Set referer to localhost that mora qualitas uses
                            trackIDClient.DefaultRequestHeaders.Add("Referer", "https://play.qobuz.com/album/" + albumId);

                            // Grab response from Qobuz to get info using Track IDs.
                            var trackIDUrl = "https://www.qobuz.com/api.json/0.2/track/get?track_id=" + trackIdString + "&app_id=" + appid + "&user_auth_token=" + userAuth;
                            var trackIDResponse = await trackIDClient.GetAsync(trackIDUrl);
                            string trackIDResponseString = trackIDResponse.Content.ReadAsStringAsync().Result;

                            // Grab metadata from API JSON response
                            JObject joTrackResponse = JObject.Parse(trackIDResponseString);

                            #region Get Information (Tags, Titles, etc.)
                            // Grab tag strings
                            albumArtist = (string)joTrackResponse["album"]["artist"]["name"]; albumArtist = DecodeEncodedNonAsciiCharacters(albumArtist);
                            albumArtistPath = GetSafeFilename(albumArtist);
                            albumArtistTextBox.Invoke(new Action(() => albumArtistTextBox.Text = albumArtist));

                            try
                            {
                                performerName = (string)joTrackResponse["performer"]["name"]; performerName = DecodeEncodedNonAsciiCharacters(performerName);
                                performerNamePath = GetSafeFilename(performerName);
                            }
                            catch { performerName = null; performerNamePath = null; /*Set to null and Ignore if fails*/ }

                            try { composerName = (string)joTrackResponse["composer"]["name"]; composerName = DecodeEncodedNonAsciiCharacters(composerName); } catch { /*Ignore if fails*/ }

                            advisory = (string)joTrackResponse["parental_warning"];

                            albumName = (string)joTrackResponse["album"]["title"]; albumName = DecodeEncodedNonAsciiCharacters(albumName);
                            albumNamePath = GetSafeFilename(albumName);
                            albumTextBox.Invoke(new Action(() => albumTextBox.Text = albumName));

                            trackName = (string)joTrackResponse["title"]; trackName = trackName.Trim(); trackName = DecodeEncodedNonAsciiCharacters(trackName);
                            trackNamePath = GetSafeFilename(trackName);

                            versionName = (string)joTrackResponse["version"];
                            if (versionName != null)
                            {
                                versionName = DecodeEncodedNonAsciiCharacters(versionName);
                                versionNamePath = GetSafeFilename(versionName);
                            }

                            genre = (string)joTrackResponse["album"]["genre"]["name"]; genre = DecodeEncodedNonAsciiCharacters(genre);

                            releaseDate = (string)joTrackResponse["album"]["release_date_stream"];
                            releaseDateTextBox.Invoke(new Action(() => releaseDateTextBox.Text = releaseDate));

                            copyright = (string)joTrackResponse["copyright"]; copyright = DecodeEncodedNonAsciiCharacters(copyright);

                            upc = (string)joTrackResponse["album"]["upc"];
                            upcTextBox.Invoke(new Action(() => upcTextBox.Text = upc));

                            isrc = (string)joTrackResponse["isrc"];

                            type = (string)joTrackResponse["album"]["release_type"];

                            // Grab tag ints
                            trackNumber = (int)joTrackResponse["track_number"];

                            trackTotal = (int)joTrackResponse["album"]["tracks_count"];
                            totalTracksTextbox.Invoke(new Action(() => totalTracksTextbox.Text = trackTotal.ToString()));

                            discNumber = (int)joTrackResponse["media_number"];

                            discTotal = (int)joTrackResponse["album"]["media_count"];

                            // Debug output to make sure values are grabbed properly
                            //output.Invoke(new Action(() => output.AppendText("Tags found, listed below...\r\n")));
                            //output.Invoke(new Action(() => output.AppendText("  Album Artist - " + albumArtist + "\r\n")));
                            //output.Invoke(new Action(() => output.AppendText("  Track Artist - " + performerName + "\r\n")));
                            //output.Invoke(new Action(() => output.AppendText("  Composer - " + composerName + "\r\n")));
                            //output.Invoke(new Action(() => output.AppendText("  Advisory - " + advisory + "\r\n")));
                            //output.Invoke(new Action(() => output.AppendText("  Album Name - " + albumName + "\r\n")));
                            //output.Invoke(new Action(() => output.AppendText("  Track Name - " + trackName + "\r\n")));
                            //output.Invoke(new Action(() => output.AppendText("  Track Version - " + versionName + "\r\n")));
                            //output.Invoke(new Action(() => output.AppendText("  Genre - " + genre + "\r\n")));
                            //output.Invoke(new Action(() => output.AppendText("  Release Date - " + releaseDate + "\r\n")));
                            //output.Invoke(new Action(() => output.AppendText("  Copyright - " + copyright + "\r\n")));
                            //output.Invoke(new Action(() => output.AppendText("  UPC - " + upc + "\r\n")));
                            //output.Invoke(new Action(() => output.AppendText("  ISRC - " + isrc + "\r\n")));
                            //output.Invoke(new Action(() => output.AppendText("  Media Type - " + type + "\r\n")));
                            //output.Invoke(new Action(() => output.AppendText("  Track Number - " + trackNumber.ToString() + "\r\n")));
                            //output.Invoke(new Action(() => output.AppendText("  Track Total - " + trackTotal.ToString() + "\r\n")));
                            //output.Invoke(new Action(() => output.AppendText("  Disc Number - " + discNumber.ToString() + "\r\n")));
                            //output.Invoke(new Action(() => output.AppendText("  Disc Total - " + discTotal.ToString() + "\r\n")));

                            #region Availability Check (Streamable?)
                            // Check if available for streaming.
                            string streamCheck = (string)joTrackResponse["streamable"];

                            switch (streamCheck.ToLower())
                            {
                                case "true":
                                    break;
                                default:
                                    switch (streamableCheckbox.Checked)
                                    {
                                        case true:
                                            output.Invoke(new Action(() => output.AppendText("Track " + trackNumber.ToString() + " is not available for streaming. Unable to download.\r\n")));
                                            System.Threading.Thread.Sleep(100);
                                            enableBoxes(sender, e);
                                            continue;
                                        default:
                                            output.Invoke(new Action(() => output.AppendText("Track is not available for streaming. But stremable check is being ignored for debugging, or messed up releases. Attempting to download...\r\n")));
                                            break;
                                    }
                                    break;
                            }
                            #endregion

                            #endregion

                            #region Filename Number Padding
                            // Set default track number padding length
                            var paddingLength = 2;

                            // Prepare track number padding in filename.
                            string paddingLog = Math.Floor(Math.Log10(trackTotal) + 1).ToString();

                            switch (paddingLog)
                            {
                                case "1":
                                    paddingLength = 2;
                                    break;
                                default:
                                    paddingLength = (int)Math.Floor(Math.Log10(trackTotal) + 1);
                                    break;
                            }

                            // Set default disc number padding length
                            var paddingDiscLength = 2;

                            // Prepare disc number padding in filename.
                            string paddingDiscLog = Math.Floor(Math.Log10(discTotal) + 1).ToString();

                            switch (paddingDiscLog)
                            {
                                case "1":
                                    paddingDiscLength = 2;
                                    break;
                                default:
                                    paddingDiscLength = (int)Math.Floor(Math.Log10(trackTotal) + 1);
                                    break;
                            }
                            #endregion

                            #region Create Shortened Strings
                            // If name goes over 36 characters, limit it to 36
                            if (albumArtistPath.Length > MaxLength)
                            {
                                albumArtistPath = albumArtistPath.Substring(0, MaxLength).TrimEnd();
                            }

                            if (performerName != null)
                            {
                                if (performerNamePath.Length > MaxLength)
                                {
                                    performerNamePath = performerNamePath.Substring(0, MaxLength).TrimEnd();
                                }
                            }

                            if (albumNamePath.Length > MaxLength)
                            {
                                albumNamePath = albumNamePath.Substring(0, MaxLength).TrimEnd();
                            }
                            #endregion

                            #region Create Directories
                            // Create strings for disc folders
                            string discFolder = null;

                            // If more than 1 disc, create folders for discs. Otherwise, strings will remain null.
                            if (discTotal > 1)
                            {
                                discFolder = "CD " + discNumber.ToString().PadLeft(paddingDiscLength, '0');
                            }

                            // Create directories
                            string[] path1 = { loc, "- Labels" };
                            path1Full = Path.Combine(path1);
                            string[] path2 = { loc, "- Labels", labelNamePath };
                            path2Full = Path.Combine(path2);
                            string[] path3 = { loc, "- Labels", labelNamePath, albumArtistPath };
                            path3Full = Path.Combine(path3);
                            string[] path4 = { loc, "- Labels", labelNamePath, albumArtistPath, albumNamePath + " [" + albumIDLabel + "]" };
                            path4Full = Path.Combine(path4);
                            string[] path5 = { loc, "- Labels", labelNamePath, albumArtistPath, albumNamePath + " [" + albumIDLabel + "]", qualityPath };
                            path5Full = Path.Combine(path5);

                            switch (discTotal)
                            {
                                case 1:
                                    path6Full = path5Full;
                                    break;
                                default:
                                    string[] path6 = { loc, "- Labels", labelNamePath, albumArtistPath, albumNamePath + " [" + albumIDLabel + "]", qualityPath, discFolder };
                                    path6Full = Path.Combine(path6);
                                    break;
                            }

                            System.IO.Directory.CreateDirectory(path1Full);
                            System.IO.Directory.CreateDirectory(path2Full);
                            System.IO.Directory.CreateDirectory(path3Full);
                            System.IO.Directory.CreateDirectory(path4Full);
                            System.IO.Directory.CreateDirectory(path5Full);
                            System.IO.Directory.CreateDirectory(path6Full);

                            // Set albumPath to the created directories.
                            string trackPath = path6Full;
                            #endregion

                            #region Create Shortened Strings (Again)
                            // Create final shortened track file names to avoid errors with file names being too long.
                            switch (versionName)
                            {
                                case null:
                                    if ((trackNumber.ToString().PadLeft(paddingLength, '0') + " " + trackNamePath).Length > MaxLength)
                                    {
                                        finalTrackNamePath = (trackNumber.ToString().PadLeft(paddingLength, '0') + " " + trackNamePath).Substring(0, MaxLength).TrimEnd();
                                    }
                                    else
                                    {
                                        finalTrackNamePath = (trackNumber.ToString().PadLeft(paddingLength, '0') + " " + trackNamePath).TrimEnd();
                                    }
                                    break;
                                default:
                                    if ((trackNumber.ToString().PadLeft(paddingLength, '0') + " " + trackNamePath + " (" + versionNamePath + ")").Length > MaxLength)
                                    {
                                        finalTrackNameVersionPath = (trackNumber.ToString().PadLeft(paddingLength, '0') + " " + trackNamePath + " (" + versionNamePath + ")").Substring(0, MaxLength).TrimEnd();
                                    }
                                    else
                                    {
                                        finalTrackNameVersionPath = (trackNumber.ToString().PadLeft(paddingLength, '0') + " " + trackNamePath + " (" + versionNamePath + ")").TrimEnd();
                                    }
                                    break;
                            }
                            #endregion

                            #region Check if File Exists
                            // Check if there is a version name.
                            switch (versionName)
                            {
                                case null:
                                    string[] path7 = { trackPath, finalTrackNamePath + audioFileType };
                                    string checkFile = Path.Combine(path7);

                                    if (System.IO.File.Exists(checkFile))
                                    {
                                        output.Invoke(new Action(() => output.AppendText("File for \"" + trackNumber.ToString().PadLeft(paddingLength, '0') + " " + trackName + "\" already exists. Skipping.\r\n")));
                                        System.Threading.Thread.Sleep(100);
                                        continue;
                                    }
                                    break;
                                default:
                                    string[] path7Version = { trackPath, finalTrackNameVersionPath + audioFileType };
                                    string checkFileVersion = Path.Combine(path7Version);

                                    if (System.IO.File.Exists(checkFileVersion))
                                    {
                                        output.Invoke(new Action(() => output.AppendText("File for \"" + trackNumber.ToString().PadLeft(paddingLength, '0') + " " + trackName + " (" + versionName + ")" + "\" already exists. Skipping.\r\n")));
                                        System.Threading.Thread.Sleep(100);
                                        continue;
                                    }
                                    break;
                            }
                            #endregion

                            // Create streaming URL.
                            createURL(sender, e);

                            try
                            {
                                #region Downloading
                                // Check if there is a version name.
                                switch (versionName)
                                {
                                    case null:
                                        output.Invoke(new Action(() => output.AppendText("Downloading - " + trackNumber.ToString().PadLeft(paddingLength, '0') + " - " + trackName + " ......")));
                                        break;
                                    default:
                                        output.Invoke(new Action(() => output.AppendText("Downloading - " + trackNumber.ToString().PadLeft(paddingLength, '0') + " - " + trackName + " (" + versionName + ")" + " ......")));
                                        break;
                                }

                                // Save streamed file from link
                                using (HttpClient streamClient = new HttpClient())
                                {
                                    // Set "range" header to nearly unlimited.
                                    streamClient.DefaultRequestHeaders.Range = new RangeHeaderValue(0, 999999999999);
                                    // Set user-agent to Firefox.
                                    streamClient.DefaultRequestHeaders.Add("User-Agent", userAgent);
                                    // Set referer URL to album ID.
                                    streamClient.DefaultRequestHeaders.Add("Referer", "https://play.qobuz.com/album/" + albumId);

                                    using (HttpResponseMessage streamResponse = await streamClient.GetAsync(stream, HttpCompletionOption.ResponseHeadersRead))
                                    using (Stream streamToReadFrom = await streamResponse.Content.ReadAsStreamAsync())
                                    {
                                        string fileName = Path.GetTempFileName();
                                        using (Stream streamToWriteTo = System.IO.File.Open(fileName, FileMode.Create))
                                        {
                                            await streamToReadFrom.CopyToAsync(streamToWriteTo);
                                        }

                                        switch (versionName)
                                        {
                                            case null:
                                                string[] path8 = { trackPath, finalTrackNamePath + audioFileType };
                                                string filePath = Path.Combine(path8);

                                                System.IO.File.Move(fileName, filePath);
                                                break;
                                            default:
                                                string[] path8Version = { trackPath, finalTrackNameVersionPath + audioFileType };
                                                string filePathVersion = Path.Combine(path8Version);

                                                System.IO.File.Move(fileName, filePathVersion);
                                                break;
                                        }
                                    }
                                }
                                #endregion

                                #region Cover Art Saving
                                string[] path9 = { loc, "- Labels", labelNamePath, albumArtistPath, albumNamePath + " [" + albumIDLabel + "]", qualityPath, "Cover.jpg" };
                                string coverArtPath = Path.Combine(path9);
                                string[] path9Tag = { loc, "- Labels", labelNamePath, albumArtistPath, albumNamePath + " [" + albumIDLabel + "]", qualityPath, artSize + ".jpg" };
                                string coverArtTagPath = Path.Combine(path9Tag);

                                if (System.IO.File.Exists(coverArtPath))
                                {
                                    try
                                    {
                                        // Skip, don't re-download.

                                        // Download selected cover art size for tagging files (Currently happens every time a track is downloaded).
                                        using (WebClient imgClient = new WebClient())
                                        {
                                            imgClient.DownloadFile(new Uri(frontCoverImg.Replace("_max", "_" + artSize)), coverArtTagPath);
                                        }
                                    }
                                    catch
                                    {
                                        // Ignore, Qobuz servers throw a 404 as if the image doesn't exist.
                                    }
                                }
                                else
                                {
                                    try
                                    {
                                        // Save cover art to selected path.
                                        using (WebClient imgClient = new WebClient())
                                        {
                                            // Download max quality Cover Art to "Cover.jpg" file in chosen path. 
                                            imgClient.DownloadFile(new Uri(frontCoverImg), coverArtPath);

                                            // Download selected cover art size for tagging files (Currently happens every time a track is downloaded).
                                            imgClient.DownloadFile(new Uri(frontCoverImg.Replace("_max", "_" + artSize)), coverArtTagPath);
                                        }
                                    }
                                    catch
                                    {
                                        // Ignore, Qobuz servers throw a 404 as if the image doesn't exist.
                                    }
                                }
                                #endregion

                                #region Tagging
                                switch (versionName)
                                {
                                    case null:
                                        break;
                                    default:
                                        finalTrackNamePath = finalTrackNameVersionPath;
                                        break;
                                }

                                string[] path10 = { trackPath, finalTrackNamePath + audioFileType };
                                string tagFilePath = Path.Combine(path10);
                                string[] path11 = { loc, "- Labels", labelNamePath, albumArtistPath, albumNamePath + " [" + albumIDLabel + "]", qualityPath, artSize + ".jpg" };
                                string tagCoverArtFilePath = Path.Combine(path11);

                                // Set file to tag
                                var tfile = TagLib.File.Create(tagFilePath);

                                switch (audioFileType)
                                {
                                    case ".mp3":
                                        #region MP3 Tagging
                                        // For custom / troublesome tags.
                                        TagLib.Id3v2.Tag t = (TagLib.Id3v2.Tag)tfile.GetTag(TagLib.TagTypes.Id3v2);

                                        // Saving cover art to file(s)
                                        if (imageCheckbox.Checked == true)
                                        {
                                            try
                                            {
                                                // Define cover art to use for MP3 file(s)
                                                TagLib.Id3v2.AttachedPictureFrame pic = new TagLib.Id3v2.AttachedPictureFrame();
                                                pic.TextEncoding = TagLib.StringType.Latin1;
                                                pic.MimeType = System.Net.Mime.MediaTypeNames.Image.Jpeg;
                                                pic.Type = TagLib.PictureType.FrontCover;
                                                pic.Data = TagLib.ByteVector.FromPath(tagCoverArtFilePath);

                                                // Save cover art to MP3 file.
                                                tfile.Tag.Pictures = new TagLib.IPicture[1] { pic };
                                                tfile.Save();
                                            }
                                            catch
                                            {
                                                output.Invoke(new Action(() => output.AppendText("Cover art tag fail, .jpg still exists?...")));
                                            }
                                        }

                                        // Track Title tag
                                        if (trackTitleCheckbox.Checked == true)
                                        {
                                            switch (versionName)
                                            {
                                                case null:
                                                    tfile.Tag.Title = trackName;
                                                    break;
                                                default:
                                                    tfile.Tag.Title = trackName + " (" + versionName + ")";
                                                    break;
                                            }

                                        }

                                        // Album Title tag
                                        if (albumCheckbox.Checked == true) { tfile.Tag.Album = albumName; }

                                        // Album Artits tag
                                        if (albumArtistCheckbox.Checked == true) { tfile.Tag.AlbumArtists = new string[] { albumArtist }; }

                                        // Track Artist tag
                                        if (artistCheckbox.Checked == true) { tfile.Tag.Performers = new string[] { performerName }; }

                                        // Composer tag
                                        if (composerCheckbox.Checked == true) { tfile.Tag.Composers = new string[] { composerName }; }

                                        // Release Date tag
                                        if (releaseCheckbox.Checked == true) { releaseDate = releaseDate.Substring(0, 4); tfile.Tag.Year = UInt32.Parse(releaseDate); }

                                        // Genre tag
                                        if (genreCheckbox.Checked == true) { tfile.Tag.Genres = new string[] { genre }; }

                                        // Track Number tag
                                        if (trackNumberCheckbox.Checked == true) { tfile.Tag.Track = Convert.ToUInt32(trackNumber); }

                                        // Disc Number tag
                                        if (discNumberCheckbox.Checked == true) { tfile.Tag.Disc = Convert.ToUInt32(discNumber); }

                                        // Total Discs tag
                                        if (discTotalCheckbox.Checked == true) { tfile.Tag.DiscCount = Convert.ToUInt32(discTotal); }

                                        // Total Tracks tag
                                        if (trackTotalCheckbox.Checked == true) { tfile.Tag.TrackCount = Convert.ToUInt32(trackTotal); }

                                        // Comment tag
                                        if (commentCheckbox.Checked == true) { tfile.Tag.Comment = commentTextbox.Text; }

                                        // Copyright tag
                                        if (copyrightCheckbox.Checked == true) { tfile.Tag.Copyright = copyright; }

                                        // ISRC tag
                                        if (isrcCheckbox.Checked == true) { TagLib.Id3v2.Tag tag = (TagLib.Id3v2.Tag)tfile.GetTag(TagTypes.Id3v2, true); tag.SetTextFrame("TSRC", isrc); }

                                        // Release Type tag
                                        if (type != null)
                                        {
                                            if (typeCheckbox.Checked == true) { TagLib.Id3v2.Tag tag = (TagLib.Id3v2.Tag)tfile.GetTag(TagTypes.Id3v2, true); tag.SetTextFrame("TMED", type); }
                                        }

                                        // Save all selected tags to file
                                        tfile.Save();
                                        #endregion
                                        break;
                                    case ".flac":
                                        #region FLAC Tagging
                                        // For custom / troublesome tags.
                                        var custom = (TagLib.Ogg.XiphComment)tfile.GetTag(TagLib.TagTypes.Xiph);

                                        // Saving cover art to file(s)
                                        if (imageCheckbox.Checked == true)
                                        {
                                            try
                                            {
                                                // Define cover art to use for FLAC file(s)
                                                TagLib.Id3v2.AttachedPictureFrame pic = new TagLib.Id3v2.AttachedPictureFrame();
                                                pic.TextEncoding = TagLib.StringType.Latin1;
                                                pic.MimeType = System.Net.Mime.MediaTypeNames.Image.Jpeg;
                                                pic.Type = TagLib.PictureType.FrontCover;
                                                pic.Data = TagLib.ByteVector.FromPath(tagCoverArtFilePath);

                                                // Save cover art to FLAC file.
                                                tfile.Tag.Pictures = new TagLib.IPicture[1] { pic };
                                                tfile.Save();
                                            }
                                            catch
                                            {
                                                output.Invoke(new Action(() => output.AppendText("Cover art tag fail, .jpg still exists?...")));
                                            }
                                        }

                                        // Track Title tag
                                        if (trackTitleCheckbox.Checked == true)
                                        {
                                            switch (versionName)
                                            {
                                                case null:
                                                    tfile.Tag.Title = trackName;
                                                    break;
                                                default:
                                                    tfile.Tag.Title = trackName + " (" + versionName + ")";
                                                    break;
                                            }
                                        }

                                        // Album Title tag
                                        if (albumCheckbox.Checked == true) { tfile.Tag.Album = albumName; }

                                        // Album Artits tag
                                        if (albumArtistCheckbox.Checked == true) { custom.SetField("ALBUMARTIST", new string[] { albumArtist }); }

                                        // Track Artist tag
                                        if (artistCheckbox.Checked == true) { custom.SetField("ARTIST", new string[] { performerName }); }

                                        // Composer tag
                                        if (composerCheckbox.Checked == true) { custom.SetField("COMPOSER", new string[] { composerName }); }

                                        // Release Date tag
                                        if (releaseCheckbox.Checked == true) { custom.SetField("YEAR", new string[] { releaseDate }); }

                                        // Genre tag
                                        if (genreCheckbox.Checked == true) { custom.SetField("GENRE", new string[] { genre }); }

                                        // Track Number tag
                                        if (trackNumberCheckbox.Checked == true) { tfile.Tag.Track = Convert.ToUInt32(trackNumber); }

                                        // Disc Number tag
                                        if (discNumberCheckbox.Checked == true) { tfile.Tag.Disc = Convert.ToUInt32(discNumber); }

                                        // Total Discs tag
                                        if (discTotalCheckbox.Checked == true) { tfile.Tag.DiscCount = Convert.ToUInt32(discTotal); }

                                        // Total Tracks tag
                                        if (trackTotalCheckbox.Checked == true) { tfile.Tag.TrackCount = Convert.ToUInt32(trackTotal); }

                                        // Comment tag
                                        if (commentCheckbox.Checked == true) { custom.SetField("COMMENT", new string[] { commentTextbox.Text }); }

                                        // Copyright tag
                                        if (copyrightCheckbox.Checked == true) { custom.SetField("COPYRIGHT", new string[] { copyright }); }
                                        // UPC tag
                                        if (upcCheckbox.Checked == true) { custom.SetField("UPC", new string[] { upc }); }

                                        // ISRC tag
                                        if (isrcCheckbox.Checked == true) { custom.SetField("ISRC", new string[] { isrc }); }

                                        // Release Type tag
                                        if (type != null)
                                        {
                                            if (typeCheckbox.Checked == true) { custom.SetField("MEDIATYPE", new string[] { type }); }
                                        }

                                        // Explicit tag
                                        if (explicitCheckbox.Checked == true)
                                        {
                                            if (advisory == "false") { custom.SetField("ITUNESADVISORY", new string[] { "0" }); } else { custom.SetField("ITUNESADVISORY", new string[] { "1" }); }
                                        }

                                        // Save all selected tags to file
                                        tfile.Save();
                                        #endregion
                                        break;
                                }
                                #endregion
                            }
                            catch (Exception downloadError)
                            {
                                // If there is an issue trying to, or during the download, show error info.
                                string error = downloadError.ToString();
                                output.Invoke(new Action(() => output.AppendText("\r\n\r\n")));
                                output.Invoke(new Action(() => output.AppendText("Track Download ERROR. Information below.\r\n\r\n")));
                                output.Invoke(new Action(() => output.AppendText(error)));
                                enableBoxes(sender, e);
                                return;
                            }

                            // Delete image file used for tagging
                            string[] path13 = { loc, "- Labels", labelNamePath, albumArtistPath, albumNamePath + " [" + albumIDLabel + "]", qualityPath, artSize + ".jpg" };
                            string coverArtTagDelete = Path.Combine(path13);

                            if (System.IO.File.Exists(coverArtTagDelete))
                            {
                                System.IO.File.Delete(coverArtTagDelete);
                            }

                            // Say when a track is done downloading, then wait for the next track / end.
                            output.Invoke(new Action(() => output.AppendText("Track Download Done!\r\n")));
                            System.Threading.Thread.Sleep(100);
                        }

                        #region Digital Booklet
                        string[] path12 = { loc, "- Labels", labelNamePath, albumArtistPath, albumNamePath + " [" + albumIDLabel + "]", qualityPath, "Digital Booklet.pdf" };
                        string goodiesPath = Path.Combine(path12);
                        // If a booklet was found, save it.
                        if (goodiesPDF == null | goodiesPDF == "")
                        {
                            // No need to download something that doesn't exist.
                        }
                        else
                        {
                            if (System.IO.File.Exists(goodiesPath))
                            {
                                // Skip, don't re-download.
                            }
                            else
                            {
                                // Save digital booklet to selected path
                                output.Invoke(new Action(() => output.AppendText("Goodies found, downloading...")));
                                using (WebClient bookClient = new WebClient())
                                {
                                    // Download max quality Cover Art to "Cover.jpg" file in chosen path. 
                                    bookClient.DownloadFile(new Uri(goodiesPDF), goodiesPath);
                                }
                            }
                        }
                        #endregion

                        // Say that downloading is completed.
                        output.Invoke(new Action(() => output.AppendText("\r\n\r\n")));
                        output.Invoke(new Action(() => output.AppendText("Downloading job completed! All downloaded files will be located in your chosen path.")));
                        enableBoxes(sender, e);
                    }
                    catch (Exception ex)
                    {
                        string error = ex.ToString();
                        //output.Invoke(new Action(() => output.Text = String.Empty));
                        output.Invoke(new Action(() => output.AppendText("Failed to download (First Phase). Error information below.\r\n\r\n")));
                        output.Invoke(new Action(() => output.AppendText(error)));
                        enableBoxes(sender, e);
                        return;
                    }
                }
            }
            catch (Exception downloadError)
            {
                // If there is an issue trying to, or during the download, show error info.
                string error = downloadError.ToString();
                output.Invoke(new Action(() => output.Text = String.Empty));
                output.Invoke(new Action(() => output.AppendText("Label Download ERROR. Information below.\r\n\r\n")));
                output.Invoke(new Action(() => output.AppendText(error)));
                enableBoxes(sender, e);
                return;
            }
            #endregion
        }

        // For downloading "favorites" (Albums only at the moment) [IN DEV]

        #region If URL is for "favorites"

        // Favorite Albums
        private async void downloadFaveAlbumsBG_DoWork(object sender, DoWorkEventArgs e)
        {
            #region Albums
            // Set "loc" as the selected path.
            String loc = folderBrowserDialog.SelectedPath;

            // Create HttpClient to grab Favorites ID
            var faveClient = new HttpClient();
            // Run through TLS to allow secure connection.
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            // Set user-agent to Firefox.
            faveClient.DefaultRequestHeaders.Add("User-Agent", userAgent);
            // Set referer to localhost that mora qualitas uses
            faveClient.DefaultRequestHeaders.Add("Referer", "https://play.qobuz.com/album/" + albumId);

            // Empty output, then say Grabbing IDs.
            output.Invoke(new Action(() => output.Text = String.Empty));
            output.Invoke(new Action(() => output.AppendText("Grabbing Album IDs...\r\n\r\n")));

            try
            {
                // Grab response from Qobuz to get Track IDs from Album response.
                var faveUrl = "https://www.qobuz.com/api.json/0.2/favorite/getUserFavorites?type=albums&limit=9999999999&user_id=" + userID + "&app_id=" + appid + "&user_auth_token=" + userAuth;
                var faveResponse = await faveClient.GetAsync(faveUrl);
                string faveResponseString = faveResponse.Content.ReadAsStringAsync().Result;

                // Grab all Album IDs listed on the API.
                string faveAlbumIDspattern = ",\"maximum_channel_count\":(?<notUsed>.*?),\"id\":\"(?<albumIds>.*?)\",";
                string faveAlbumIDsInput = faveResponseString;
                RegexOptions faveAlbumIDsOptions = RegexOptions.Multiline;

                foreach (Match m in Regex.Matches(faveAlbumIDsInput, faveAlbumIDspattern, faveAlbumIDsOptions))
                {
                    // Make sure buttons are disabled during downloads.
                    disableBoxes(sender, e);

                    string albumIDFave = string.Format("{0}", m.Groups["albumIds"].Value);

                    // Create HttpClient to grab Album ID
                    var albumIDClient = new HttpClient();
                    // Run through TLS to allow secure connection.
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                    // Set user-agent to Firefox.
                    albumIDClient.DefaultRequestHeaders.Add("User-Agent", userAgent);
                    // Set referer to localhost that mora qualitas uses
                    albumIDClient.DefaultRequestHeaders.Add("Referer", "https://play.qobuz.com/album/" + albumIDFave);

                    // Empty output, then say Starting Downloads.
                    output.Invoke(new Action(() => output.Text = String.Empty));
                    output.Invoke(new Action(() => output.AppendText("Starting Downloads...\r\n\r\n")));

                    try
                    {
                        // Grab response from Qobuz to get Track IDs from Album response.
                        var albumIDUrl = "https://www.qobuz.com/api.json/0.2/album/get?album_id=" + albumIDFave + "&app_id=" + appid + "&user_auth_token=" + userAuth;
                        var albumIDResponse = await albumIDClient.GetAsync(albumIDUrl);
                        string albumIDResponseString = albumIDResponse.Content.ReadAsStringAsync().Result;

                        // Grab metadata from API JSON response
                        JObject joAlbumResponse = JObject.Parse(albumIDResponseString);

                        #region Availability Check (Valid Link?)
                        // Check if available at all.
                        string errorCheckAlbum = (string)joAlbumResponse["code"];
                        string errorMessageCheck = (string)joAlbumResponse["message"];

                        switch (errorCheckAlbum)
                        {
                            case "404":
                                output.Invoke(new Action(() => output.Text = String.Empty));
                                output.Invoke(new Action(() => output.AppendText("ERROR: 404\r\n")));
                                output.Invoke(new Action(() => output.AppendText("Error message is \"" + errorMessageCheck + "\"\r\n")));
                                output.Invoke(new Action(() => output.AppendText("This usually means the link is invalid, or isn't available in the region your account is from.")));
                                enableBoxes(sender, e);
                                return;
                        }
                        #endregion

                        #region Quality Info (Bitrate & Sample Rate)
                        // Grab sample rate and bit depth for album track is from.
                        var bitDepth = (string)joAlbumResponse["maximum_bit_depth"];
                        var sampleRate = (string)joAlbumResponse["maximum_sampling_rate"];

                        var quality = "FLAC (" + bitDepth + "bit/" + sampleRate + "kHz)";
                        var qualityPath = quality.Replace(@"\", "-").Replace(@"/", "-");

                        switch (formatIdString)
                        {
                            case "5":
                                quality = "MP3 320kbps CBR";
                                qualityPath = "MP3";
                                break;
                            case "6":
                                quality = "FLAC (16bit/44.1kHz)";
                                qualityPath = "FLAC (16bit-44.1kHz)";
                                break;
                            case "7":
                                if (quality == "FLAC (24bit/192kHz)")
                                {
                                    quality = "FLAC (24bit/96kHz)";
                                    qualityPath = "FLAC (24bit-96kHz)";
                                }
                                break;
                        }

                        // Display album quality in quality textbox.
                        qualityTextbox.Invoke(new Action(() => qualityTextbox.Text = quality));
                        #endregion

                        #region Cover Art URL
                        // Grab cover art link
                        frontCoverImg = (string)joAlbumResponse["image"]["large"];
                        // Get 150x150 artwork for cover art box
                        frontCoverImgBox = frontCoverImg.Replace("_600.jpg", "_150.jpg");
                        // Get max sized artwork
                        frontCoverImg = frontCoverImg.Replace("_600.jpg", "_max.jpg");

                        albumArtPicBox.Invoke(new Action(() => albumArtPicBox.ImageLocation = frontCoverImgBox));
                        #endregion

                        #region "Goodies" URL (Digital Booklets)
                        // Look for "Goodies" (digital booklet)
                        var goodiesLog = Regex.Match(albumIDResponseString, "\"goodies\":\\[{(?<notUsed>.*?),\"url\":\"(?<booklet>.*?)\",").Groups;
                        var goodiesPDF = goodiesLog[2].Value.Replace(@"\/", "/");
                        #endregion

                        // Grab all Track IDs listed on the API.
                        string trackIDsPattern = "\"version\":(?:.*?),\"id\":(?<trackId>.*?),";
                        string trackIDsInput = albumIDResponseString;
                        RegexOptions trackIDsOptions = RegexOptions.Multiline;

                        foreach (Match m2 in Regex.Matches(trackIDsInput, trackIDsPattern, trackIDsOptions))
                        {
                            // Grab matches for Track IDs
                            trackIdString = string.Format("{0}", m2.Groups["trackId"].Value);

                            // Create HttpClient to grab Track ID
                            var trackIDClient = new HttpClient();
                            // Run through TLS to allow secure connection.
                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                            // Set user-agent to Firefox.
                            trackIDClient.DefaultRequestHeaders.Add("User-Agent", userAgent);
                            // Set referer to localhost that mora qualitas uses
                            trackIDClient.DefaultRequestHeaders.Add("Referer", "https://play.qobuz.com/album/" + albumId);

                            // Grab response from Qobuz to get info using Track IDs.
                            var trackIDUrl = "https://www.qobuz.com/api.json/0.2/track/get?track_id=" + trackIdString + "&app_id=" + appid + "&user_auth_token=" + userAuth;
                            var trackIDResponse = await trackIDClient.GetAsync(trackIDUrl);
                            string trackIDResponseString = trackIDResponse.Content.ReadAsStringAsync().Result;

                            // Grab metadata from API JSON response
                            JObject joTrackResponse = JObject.Parse(trackIDResponseString);

                            #region Get Information (Tags, Titles, etc.)
                            // Grab tag strings
                            albumArtist = (string)joTrackResponse["album"]["artist"]["name"]; albumArtist = DecodeEncodedNonAsciiCharacters(albumArtist);
                            albumArtistPath = GetSafeFilename(albumArtist);
                            albumArtistTextBox.Invoke(new Action(() => albumArtistTextBox.Text = albumArtist));

                            try
                            {
                                performerName = (string)joTrackResponse["performer"]["name"]; performerName = DecodeEncodedNonAsciiCharacters(performerName);
                                performerNamePath = GetSafeFilename(performerName);
                            }
                            catch { performerName = null; performerNamePath = null; /*Set to null and Ignore if fails*/ }

                            try { composerName = (string)joTrackResponse["composer"]["name"]; composerName = DecodeEncodedNonAsciiCharacters(composerName); } catch { /*Ignore if fails*/ }

                            advisory = (string)joTrackResponse["parental_warning"];

                            albumName = (string)joTrackResponse["album"]["title"]; albumName = DecodeEncodedNonAsciiCharacters(albumName);
                            albumNamePath = GetSafeFilename(albumName);
                            albumTextBox.Invoke(new Action(() => albumTextBox.Text = albumName));

                            trackName = (string)joTrackResponse["title"]; trackName = trackName.Trim(); trackName = DecodeEncodedNonAsciiCharacters(trackName);
                            trackNamePath = GetSafeFilename(trackName);

                            versionName = (string)joTrackResponse["version"];
                            if (versionName != null)
                            {
                                versionName = DecodeEncodedNonAsciiCharacters(versionName);
                                versionNamePath = GetSafeFilename(versionName);
                            }

                            genre = (string)joTrackResponse["album"]["genre"]["name"]; genre = DecodeEncodedNonAsciiCharacters(genre);

                            releaseDate = (string)joTrackResponse["album"]["release_date_stream"];
                            releaseDateTextBox.Invoke(new Action(() => releaseDateTextBox.Text = releaseDate));

                            copyright = (string)joTrackResponse["copyright"]; copyright = DecodeEncodedNonAsciiCharacters(copyright);

                            upc = (string)joTrackResponse["album"]["upc"];
                            upcTextBox.Invoke(new Action(() => upcTextBox.Text = upc));

                            isrc = (string)joTrackResponse["isrc"];

                            type = (string)joTrackResponse["album"]["release_type"];

                            // Grab tag ints
                            trackNumber = (int)joTrackResponse["track_number"];

                            trackTotal = (int)joTrackResponse["album"]["tracks_count"];
                            totalTracksTextbox.Invoke(new Action(() => totalTracksTextbox.Text = trackTotal.ToString()));

                            discNumber = (int)joTrackResponse["media_number"];

                            discTotal = (int)joTrackResponse["album"]["media_count"];

                            // Debug output to make sure values are grabbed properly
                            //output.Invoke(new Action(() => output.AppendText("Tags found, listed below...\r\n")));
                            //output.Invoke(new Action(() => output.AppendText("  Album Artist - " + albumArtist + "\r\n")));
                            //output.Invoke(new Action(() => output.AppendText("  Track Artist - " + performerName + "\r\n")));
                            //output.Invoke(new Action(() => output.AppendText("  Composer - " + composerName + "\r\n")));
                            //output.Invoke(new Action(() => output.AppendText("  Advisory - " + advisory + "\r\n")));
                            //output.Invoke(new Action(() => output.AppendText("  Album Name - " + albumName + "\r\n")));
                            //output.Invoke(new Action(() => output.AppendText("  Track Name - " + trackName + "\r\n")));
                            //output.Invoke(new Action(() => output.AppendText("  Track Version - " + versionName + "\r\n")));
                            //output.Invoke(new Action(() => output.AppendText("  Genre - " + genre + "\r\n")));
                            //output.Invoke(new Action(() => output.AppendText("  Release Date - " + releaseDate + "\r\n")));
                            //output.Invoke(new Action(() => output.AppendText("  Copyright - " + copyright + "\r\n")));
                            //output.Invoke(new Action(() => output.AppendText("  UPC - " + upc + "\r\n")));
                            //output.Invoke(new Action(() => output.AppendText("  ISRC - " + isrc + "\r\n")));
                            //output.Invoke(new Action(() => output.AppendText("  Media Type - " + type + "\r\n")));
                            //output.Invoke(new Action(() => output.AppendText("  Track Number - " + trackNumber.ToString() + "\r\n")));
                            //output.Invoke(new Action(() => output.AppendText("  Track Total - " + trackTotal.ToString() + "\r\n")));
                            //output.Invoke(new Action(() => output.AppendText("  Disc Number - " + discNumber.ToString() + "\r\n")));
                            //output.Invoke(new Action(() => output.AppendText("  Disc Total - " + discTotal.ToString() + "\r\n")));

                            #region Availability Check (Streamable?)
                            // Check if available for streaming.
                            string streamCheck = (string)joTrackResponse["streamable"];

                            switch (streamCheck.ToLower())
                            {
                                case "true":
                                    break;
                                default:
                                    switch (streamableCheckbox.Checked)
                                    {
                                        case true:
                                            output.Invoke(new Action(() => output.AppendText("Track " + trackNumber.ToString() + " is not available for streaming. Unable to download.\r\n")));
                                            System.Threading.Thread.Sleep(100);
                                            enableBoxes(sender, e);
                                            continue;
                                        default:
                                            output.Invoke(new Action(() => output.AppendText("Track is not available for streaming. But stremable check is being ignored for debugging, or messed up releases. Attempting to download...\r\n")));
                                            break;
                                    }
                                    break;
                            }
                            #endregion

                            #endregion

                            #region Filename Number Padding
                            // Set default track number padding length
                            var paddingLength = 2;

                            // Prepare track number padding in filename.
                            string paddingLog = Math.Floor(Math.Log10(trackTotal) + 1).ToString();

                            switch (paddingLog)
                            {
                                case "1":
                                    paddingLength = 2;
                                    break;
                                default:
                                    paddingLength = (int)Math.Floor(Math.Log10(trackTotal) + 1);
                                    break;
                            }

                            // Set default disc number padding length
                            var paddingDiscLength = 2;

                            // Prepare disc number padding in filename.
                            string paddingDiscLog = Math.Floor(Math.Log10(discTotal) + 1).ToString();

                            switch (paddingDiscLog)
                            {
                                case "1":
                                    paddingDiscLength = 2;
                                    break;
                                default:
                                    paddingDiscLength = (int)Math.Floor(Math.Log10(trackTotal) + 1);
                                    break;
                            }
                            #endregion

                            #region Create Shortened Strings
                            // If name goes over 36 characters, limit it to 36
                            if (albumArtistPath.Length > MaxLength)
                            {
                                albumArtistPath = albumArtistPath.Substring(0, MaxLength).TrimEnd();
                            }

                            if (performerName != null)
                            {
                                if (performerNamePath.Length > MaxLength)
                                {
                                    performerNamePath = performerNamePath.Substring(0, MaxLength).TrimEnd();
                                }
                            }

                            if (albumNamePath.Length > MaxLength)
                            {
                                albumNamePath = albumNamePath.Substring(0, MaxLength).TrimEnd();
                            }
                            #endregion
                            
                            #region Create Directories
                            // Create strings for disc folders
                            string discFolder = null;

                            // If more than 1 disc, create folders for discs. Otherwise, strings will remain null.
                            if (discTotal > 1)
                            {
                                discFolder = "CD " + discNumber.ToString().PadLeft(paddingDiscLength, '0');
                            }

                            // Create directories
                            string[] path1 = { loc, "- Favorites" };
                            path1Full = Path.Combine(path1);
                            string[] path2 = { loc, "- Favorites", albumArtistPath };
                            path2Full = Path.Combine(path2);
                            string[] path3 = { loc, "- Favorites", albumArtistPath, albumNamePath + " [" + albumIDFave + "]" };
                            path3Full = Path.Combine(path3);
                            string[] path4 = { loc, "- Favorites", albumArtistPath, albumNamePath + " [" + albumIDFave + "]", qualityPath };
                            path4Full = Path.Combine(path4);

                            switch (discTotal)
                            {
                                case 1:
                                    path5Full = path4Full;
                                    break;
                                default:
                                    string[] path5 = { loc, "- Favorites", albumArtistPath, albumNamePath + " [" + albumIDFave + "]", qualityPath, discFolder };
                                    path5Full = Path.Combine(path5);
                                    break;
                            }

                            System.IO.Directory.CreateDirectory(path1Full);
                            System.IO.Directory.CreateDirectory(path2Full);
                            System.IO.Directory.CreateDirectory(path3Full);
                            System.IO.Directory.CreateDirectory(path4Full);
                            System.IO.Directory.CreateDirectory(path5Full);

                            // Set albumPath to the created directories.
                            string trackPath = path5Full;
                            #endregion

                            #region Create Shortened Strings (Again)
                            // Create final shortened track file names to avoid errors with file names being too long.
                            switch (versionName)
                            {
                                case null:
                                    if ((trackNumber.ToString().PadLeft(paddingLength, '0') + " " + trackNamePath).Length > MaxLength)
                                    {
                                        finalTrackNamePath = (trackNumber.ToString().PadLeft(paddingLength, '0') + " " + trackNamePath).Substring(0, MaxLength).TrimEnd();
                                    }
                                    else
                                    {
                                        finalTrackNamePath = (trackNumber.ToString().PadLeft(paddingLength, '0') + " " + trackNamePath).TrimEnd();
                                    }
                                    break;
                                default:
                                    if ((trackNumber.ToString().PadLeft(paddingLength, '0') + " " + trackNamePath + " (" + versionNamePath + ")").Length > MaxLength)
                                    {
                                        finalTrackNameVersionPath = (trackNumber.ToString().PadLeft(paddingLength, '0') + " " + trackNamePath + " (" + versionNamePath + ")").Substring(0, MaxLength).TrimEnd();
                                    }
                                    else
                                    {
                                        finalTrackNameVersionPath = (trackNumber.ToString().PadLeft(paddingLength, '0') + " " + trackNamePath + " (" + versionNamePath + ")").TrimEnd();
                                    }
                                    break;
                            }
                            #endregion

                            #region Check if File Exists
                            // Check if there is a version name.
                            switch (versionName)
                            {
                                case null:
                                    string[] path6 = { trackPath, finalTrackNamePath + audioFileType };
                                    string checkFile = Path.Combine(path6);

                                    if (System.IO.File.Exists(checkFile))
                                    {
                                        output.Invoke(new Action(() => output.AppendText("File for \"" + trackNumber.ToString().PadLeft(paddingLength, '0') + " " + trackName + "\" already exists. Skipping.\r\n")));
                                        System.Threading.Thread.Sleep(100);
                                        continue;
                                    }
                                    break;
                                default:
                                    string[] path6Version = { trackPath, finalTrackNameVersionPath + audioFileType };
                                    string checkFileVersion = Path.Combine(path6Version);

                                    if (System.IO.File.Exists(checkFileVersion))
                                    {
                                        output.Invoke(new Action(() => output.AppendText("File for \"" + trackNumber.ToString().PadLeft(paddingLength, '0') + " " + trackName + " (" + versionName + ")" + "\" already exists. Skipping.\r\n")));
                                        System.Threading.Thread.Sleep(100);
                                        continue;
                                    }
                                    break;
                            }
                            #endregion

                            // Create streaming URL.
                            createURL(sender, e);

                            try
                            {
                                #region Downloading
                                // Check if there is a version name.
                                switch (versionName)
                                {
                                    case null:
                                        output.Invoke(new Action(() => output.AppendText("Downloading - " + trackNumber.ToString().PadLeft(paddingLength, '0') + " - " + trackName + " ......")));
                                        break;
                                    default:
                                        output.Invoke(new Action(() => output.AppendText("Downloading - " + trackNumber.ToString().PadLeft(paddingLength, '0') + " - " + trackName + " (" + versionName + ")" + " ......")));
                                        break;
                                }

                                // Save streamed file from link
                                using (HttpClient streamClient = new HttpClient())
                                {
                                    // Set "range" header to nearly unlimited.
                                    streamClient.DefaultRequestHeaders.Range = new RangeHeaderValue(0, 999999999999);
                                    // Set user-agent to Firefox.
                                    streamClient.DefaultRequestHeaders.Add("User-Agent", userAgent);
                                    // Set referer URL to album ID.
                                    streamClient.DefaultRequestHeaders.Add("Referer", "https://play.qobuz.com/album/" + albumId);

                                    using (HttpResponseMessage streamResponse = await streamClient.GetAsync(stream, HttpCompletionOption.ResponseHeadersRead))
                                    using (Stream streamToReadFrom = await streamResponse.Content.ReadAsStreamAsync())
                                    {
                                        string fileName = Path.GetTempFileName();
                                        using (Stream streamToWriteTo = System.IO.File.Open(fileName, FileMode.Create))
                                        {
                                            await streamToReadFrom.CopyToAsync(streamToWriteTo);
                                        }

                                        switch (versionName)
                                        {
                                            case null:
                                                string[] path7 = { trackPath, finalTrackNamePath + audioFileType };
                                                string filePath = Path.Combine(path7);

                                                System.IO.File.Move(fileName, filePath);
                                                break;
                                            default:
                                                string[] path7Version = { trackPath, finalTrackNameVersionPath + audioFileType };
                                                string filePathVersion = Path.Combine(path7Version);

                                                System.IO.File.Move(fileName, filePathVersion);
                                                break;
                                        }
                                    }
                                }
                                #endregion

                                #region Cover Art Saving
                                string[] path8 = { loc, "- Favorites", albumArtistPath, albumNamePath + " [" + albumIDFave + "]", qualityPath, "Cover.jpg" };
                                string coverArtPath = Path.Combine(path8);
                                string[] path8Tag = { loc, "- Favorites", albumArtistPath, albumNamePath + " [" + albumIDFave + "]", qualityPath, artSize + ".jpg" };
                                string coverArtTagPath = Path.Combine(path8Tag);

                                if (System.IO.File.Exists(coverArtPath))
                                {
                                    try
                                    {
                                        // Skip, don't re-download.

                                        // Download selected cover art size for tagging files (Currently happens every time a track is downloaded).
                                        using (WebClient imgClient = new WebClient())
                                        {
                                            imgClient.DownloadFile(new Uri(frontCoverImg.Replace("_max", "_" + artSize)), coverArtTagPath);
                                        }
                                    }
                                    catch
                                    {
                                        // Ignore, Qobuz servers throw a 404 as if the image doesn't exist.
                                    }
                                }
                                else
                                {
                                    try
                                    {
                                        // Save cover art to selected path.
                                        using (WebClient imgClient = new WebClient())
                                        {
                                            // Download max quality Cover Art to "Cover.jpg" file in chosen path. 
                                            imgClient.DownloadFile(new Uri(frontCoverImg), coverArtPath);

                                            // Download selected cover art size for tagging files (Currently happens every time a track is downloaded).
                                            imgClient.DownloadFile(new Uri(frontCoverImg.Replace("_max", "_" + artSize)), coverArtTagPath);
                                        }
                                    }
                                    catch
                                    {
                                        // Ignore, Qobuz servers throw a 404 as if the image doesn't exist.
                                    }
                                }
                                #endregion

                                #region Tagging
                                switch (versionName)
                                {
                                    case null:
                                        break;
                                    default:
                                        finalTrackNamePath = finalTrackNameVersionPath;
                                        break;
                                }

                                string[] path9 = { trackPath, finalTrackNamePath + audioFileType };
                                string tagFilePath = Path.Combine(path9);
                                string[] path10 = { loc, "- Favorites", albumArtistPath, albumNamePath + " [" + albumIDFave + "]", qualityPath, artSize + ".jpg" };
                                string tagCoverArtFilePath = Path.Combine(path10);

                                // Set file to tag
                                var tfile = TagLib.File.Create(tagFilePath);

                                switch (audioFileType)
                                {
                                    case ".mp3":
                                        #region MP3 Tagging
                                        // For custom / troublesome tags.
                                        TagLib.Id3v2.Tag t = (TagLib.Id3v2.Tag)tfile.GetTag(TagLib.TagTypes.Id3v2);

                                        // Saving cover art to file(s)
                                        if (imageCheckbox.Checked == true)
                                        {
                                            try
                                            {
                                                // Define cover art to use for MP3 file(s)
                                                TagLib.Id3v2.AttachedPictureFrame pic = new TagLib.Id3v2.AttachedPictureFrame();
                                                pic.TextEncoding = TagLib.StringType.Latin1;
                                                pic.MimeType = System.Net.Mime.MediaTypeNames.Image.Jpeg;
                                                pic.Type = TagLib.PictureType.FrontCover;
                                                pic.Data = TagLib.ByteVector.FromPath(tagCoverArtFilePath);

                                                // Save cover art to MP3 file.
                                                tfile.Tag.Pictures = new TagLib.IPicture[1] { pic };
                                                tfile.Save();
                                            }
                                            catch
                                            {
                                                output.Invoke(new Action(() => output.AppendText("Cover art tag fail, .jpg still exists?...")));
                                            }
                                        }

                                        // Track Title tag
                                        if (trackTitleCheckbox.Checked == true)
                                        {
                                            switch (versionName)
                                            {
                                                case null:
                                                    tfile.Tag.Title = trackName;
                                                    break;
                                                default:
                                                    tfile.Tag.Title = trackName + " (" + versionName + ")";
                                                    break;
                                            }

                                        }

                                        // Album Title tag
                                        if (albumCheckbox.Checked == true) { tfile.Tag.Album = albumName; }

                                        // Album Artits tag
                                        if (albumArtistCheckbox.Checked == true) { tfile.Tag.AlbumArtists = new string[] { albumArtist }; }

                                        // Track Artist tag
                                        if (artistCheckbox.Checked == true) { tfile.Tag.Performers = new string[] { performerName }; }

                                        // Composer tag
                                        if (composerCheckbox.Checked == true) { tfile.Tag.Composers = new string[] { composerName }; }

                                        // Release Date tag
                                        if (releaseCheckbox.Checked == true) { releaseDate = releaseDate.Substring(0, 4); tfile.Tag.Year = UInt32.Parse(releaseDate); }

                                        // Genre tag
                                        if (genreCheckbox.Checked == true) { tfile.Tag.Genres = new string[] { genre }; }

                                        // Track Number tag
                                        if (trackNumberCheckbox.Checked == true) { tfile.Tag.Track = Convert.ToUInt32(trackNumber); }

                                        // Disc Number tag
                                        if (discNumberCheckbox.Checked == true) { tfile.Tag.Disc = Convert.ToUInt32(discNumber); }

                                        // Total Discs tag
                                        if (discTotalCheckbox.Checked == true) { tfile.Tag.DiscCount = Convert.ToUInt32(discTotal); }

                                        // Total Tracks tag
                                        if (trackTotalCheckbox.Checked == true) { tfile.Tag.TrackCount = Convert.ToUInt32(trackTotal); }

                                        // Comment tag
                                        if (commentCheckbox.Checked == true) { tfile.Tag.Comment = commentTextbox.Text; }

                                        // Copyright tag
                                        if (copyrightCheckbox.Checked == true) { tfile.Tag.Copyright = copyright; }

                                        // ISRC tag
                                        if (isrcCheckbox.Checked == true) { TagLib.Id3v2.Tag tag = (TagLib.Id3v2.Tag)tfile.GetTag(TagTypes.Id3v2, true); tag.SetTextFrame("TSRC", isrc); }

                                        // Release Type tag
                                        if (type != null)
                                        {
                                            if (typeCheckbox.Checked == true) { TagLib.Id3v2.Tag tag = (TagLib.Id3v2.Tag)tfile.GetTag(TagTypes.Id3v2, true); tag.SetTextFrame("TMED", type); }
                                        }

                                        // Save all selected tags to file
                                        tfile.Save();
                                        #endregion
                                        break;
                                    case ".flac":
                                        #region FLAC Tagging
                                        // For custom / troublesome tags.
                                        var custom = (TagLib.Ogg.XiphComment)tfile.GetTag(TagLib.TagTypes.Xiph);

                                        // Saving cover art to file(s)
                                        if (imageCheckbox.Checked == true)
                                        {
                                            try
                                            {
                                                // Define cover art to use for FLAC file(s)
                                                TagLib.Id3v2.AttachedPictureFrame pic = new TagLib.Id3v2.AttachedPictureFrame();
                                                pic.TextEncoding = TagLib.StringType.Latin1;
                                                pic.MimeType = System.Net.Mime.MediaTypeNames.Image.Jpeg;
                                                pic.Type = TagLib.PictureType.FrontCover;
                                                pic.Data = TagLib.ByteVector.FromPath(tagCoverArtFilePath);

                                                // Save cover art to FLAC file.
                                                tfile.Tag.Pictures = new TagLib.IPicture[1] { pic };
                                                tfile.Save();
                                            }
                                            catch
                                            {
                                                output.Invoke(new Action(() => output.AppendText("Cover art tag fail, .jpg still exists?...")));
                                            }
                                        }

                                        // Track Title tag
                                        if (trackTitleCheckbox.Checked == true)
                                        {
                                            switch (versionName)
                                            {
                                                case null:
                                                    tfile.Tag.Title = trackName;
                                                    break;
                                                default:
                                                    tfile.Tag.Title = trackName + " (" + versionName + ")";
                                                    break;
                                            }
                                        }

                                        // Album Title tag
                                        if (albumCheckbox.Checked == true) { tfile.Tag.Album = albumName; }

                                        // Album Artits tag
                                        if (albumArtistCheckbox.Checked == true) { custom.SetField("ALBUMARTIST", new string[] { albumArtist }); }

                                        // Track Artist tag
                                        if (artistCheckbox.Checked == true) { custom.SetField("ARTIST", new string[] { performerName }); }

                                        // Composer tag
                                        if (composerCheckbox.Checked == true) { custom.SetField("COMPOSER", new string[] { composerName }); }

                                        // Release Date tag
                                        if (releaseCheckbox.Checked == true) { custom.SetField("YEAR", new string[] { releaseDate }); }

                                        // Genre tag
                                        if (genreCheckbox.Checked == true) { custom.SetField("GENRE", new string[] { genre }); }

                                        // Track Number tag
                                        if (trackNumberCheckbox.Checked == true) { tfile.Tag.Track = Convert.ToUInt32(trackNumber); }

                                        // Disc Number tag
                                        if (discNumberCheckbox.Checked == true) { tfile.Tag.Disc = Convert.ToUInt32(discNumber); }

                                        // Total Discs tag
                                        if (discTotalCheckbox.Checked == true) { tfile.Tag.DiscCount = Convert.ToUInt32(discTotal); }

                                        // Total Tracks tag
                                        if (trackTotalCheckbox.Checked == true) { tfile.Tag.TrackCount = Convert.ToUInt32(trackTotal); }

                                        // Comment tag
                                        if (commentCheckbox.Checked == true) { custom.SetField("COMMENT", new string[] { commentTextbox.Text }); }

                                        // Copyright tag
                                        if (copyrightCheckbox.Checked == true) { custom.SetField("COPYRIGHT", new string[] { copyright }); }
                                        // UPC tag
                                        if (upcCheckbox.Checked == true) { custom.SetField("UPC", new string[] { upc }); }

                                        // ISRC tag
                                        if (isrcCheckbox.Checked == true) { custom.SetField("ISRC", new string[] { isrc }); }

                                        // Release Type tag
                                        if (type != null)
                                        {
                                            if (typeCheckbox.Checked == true) { custom.SetField("MEDIATYPE", new string[] { type }); }
                                        }

                                        // Explicit tag
                                        if (explicitCheckbox.Checked == true)
                                        {
                                            if (advisory == "false") { custom.SetField("ITUNESADVISORY", new string[] { "0" }); } else { custom.SetField("ITUNESADVISORY", new string[] { "1" }); }
                                        }

                                        // Save all selected tags to file
                                        tfile.Save();
                                        #endregion
                                        break;
                                }
                                #endregion
                            }
                            catch (Exception downloadError)
                            {
                                // If there is an issue trying to, or during the download, show error info.
                                string error = downloadError.ToString();
                                output.Invoke(new Action(() => output.AppendText("\r\n\r\n")));
                                output.Invoke(new Action(() => output.AppendText("Track Download ERROR. Information below.\r\n\r\n")));
                                output.Invoke(new Action(() => output.AppendText(error)));
                                enableBoxes(sender, e);
                                return;
                            }

                            // Delete image file used for tagging
                            string[] path12 = { loc, "- Favorites", albumArtistPath, albumNamePath + " [" + albumIDFave + "]", qualityPath, artSize + ".jpg" };
                            string coverArtTagDelete = Path.Combine(path12);

                            if (System.IO.File.Exists(coverArtTagDelete))
                            {
                                System.IO.File.Delete(coverArtTagDelete);
                            }

                            // Say when a track is done downloading, then wait for the next track / end.
                            output.Invoke(new Action(() => output.AppendText("Track Download Done!\r\n")));
                            System.Threading.Thread.Sleep(100);
                        }

                        #region Digital Booklet
                        string[] path11 = { loc, "- Favorites", albumArtistPath, albumNamePath + " [" + albumIDFave + "]", qualityPath, "Digital Booklet.pdf" };
                        string goodiesPath = Path.Combine(path11);
                        // If a booklet was found, save it.
                        if (goodiesPDF == null | goodiesPDF == "")
                        {
                            // No need to download something that doesn't exist.
                        }
                        else
                        {
                            if (System.IO.File.Exists(goodiesPath))
                            {
                                // Skip, don't re-download.
                            }
                            else
                            {
                                // Save digital booklet to selected path
                                output.Invoke(new Action(() => output.AppendText("Goodies found, downloading...")));
                                using (WebClient bookClient = new WebClient())
                                {
                                    // Download max quality Cover Art to "Cover.jpg" file in chosen path. 
                                    bookClient.DownloadFile(new Uri(goodiesPDF), goodiesPath);
                                }
                            }
                        }
                        #endregion

                        // Say that downloading is completed.
                        output.Invoke(new Action(() => output.AppendText("\r\n\r\n")));
                        output.Invoke(new Action(() => output.AppendText("Downloading job completed! All downloaded files will be located in your chosen path.")));
                        enableBoxes(sender, e);
                    }
                    catch (Exception ex)
                    {
                        string error = ex.ToString();
                        //output.Invoke(new Action(() => output.Text = String.Empty));
                        output.Invoke(new Action(() => output.AppendText("Failed to download (First Phase). Error information below.\r\n\r\n")));
                        output.Invoke(new Action(() => output.AppendText(error)));
                        enableBoxes(sender, e);
                        return;
                    }
                }
            }
            catch (Exception downloadError)
            {
                // If there is an issue trying to, or during the download, show error info.
                string error = downloadError.ToString();
                output.Invoke(new Action(() => output.Text = String.Empty));
                output.Invoke(new Action(() => output.AppendText("Favorites Download ERROR. Information below.\r\n\r\n")));
                output.Invoke(new Action(() => output.AppendText(error)));
                enableBoxes(sender, e);
                return;
            }
            #endregion
        }

        // Favorite Artists
        private async void downloadFaveArtistsBG_DoWork(object sender, DoWorkEventArgs e)
        {
            /* This hasn't been worked on yet */
        }

        #endregion

        // For downloading "album" links
        private async void downloadAlbumBG_DoWork(object sender, DoWorkEventArgs e)
        {
            #region If URL has "album"
            // Set "loc" as the selected path.
            String loc = folderBrowserDialog.SelectedPath;
            
            // Create HttpClient to grab Album ID
            var albumIDClient = new HttpClient();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            // Set user-agent to Firefox.
            albumIDClient.DefaultRequestHeaders.Add("User-Agent", userAgent);
            // Set referer to localhost that mora qualitas uses
            albumIDClient.DefaultRequestHeaders.Add("Referer", "https://play.qobuz.com/album/" + albumId);

            // Empty output, then say Starting Downloads.
            output.Invoke(new Action(() => output.Text = String.Empty));
            output.Invoke(new Action(() => output.AppendText("Starting Downloads...\r\n\r\n")));

            try
            {
                // Grab response from Qobuz to get Track IDs from Album response.
                var albumIDUrl = "https://www.qobuz.com/api.json/0.2/album/get?album_id=" + albumId + "&app_id=" + appid + "&user_auth_token=" + userAuth;
                var albumIDResponse = await albumIDClient.GetAsync(albumIDUrl);
                string albumIDResponseString = albumIDResponse.Content.ReadAsStringAsync().Result;

                // Grab metadata from API JSON response
                JObject joAlbumResponse = JObject.Parse(albumIDResponseString);

                #region Availability Check (Valid Link?)
                // Check if available at all.
                string errorCheckAlbum = (string)joAlbumResponse["code"];
                string errorMessageCheck = (string)joAlbumResponse["message"];

                switch (errorCheckAlbum)
                {
                    case "404":
                        output.Invoke(new Action(() => output.Text = String.Empty));
                        output.Invoke(new Action(() => output.AppendText("ERROR: 404\r\n")));
                        output.Invoke(new Action(() => output.AppendText("Error message is \"" + errorMessageCheck + "\"\r\n")));
                        output.Invoke(new Action(() => output.AppendText("This usually means the link is invalid, or isn't available in the region your account is from.")));
                        enableBoxes(sender, e);
                        return;
                }
                #endregion

                #region Quality Info (Bitrate & Sample Rate)
                // Grab sample rate and bit depth for album track is from.
                var bitDepth = (string)joAlbumResponse["maximum_bit_depth"];
                var sampleRate = (string)joAlbumResponse["maximum_sampling_rate"];

                var quality = "FLAC (" + bitDepth + "bit/" + sampleRate + "kHz)";
                var qualityPath = quality.Replace(@"\", "-").Replace(@"/", "-");

                switch (formatIdString)
                {
                    case "5":
                        quality = "MP3 320kbps CBR";
                        qualityPath = "MP3";
                        break;
                    case "6":
                        quality = "FLAC (16bit/44.1kHz)";
                        qualityPath = "FLAC (16bit-44.1kHz)";
                        break;
                    case "7":
                        if (quality == "FLAC (24bit/192kHz)")
                        {
                            quality = "FLAC (24bit/96kHz)";
                            qualityPath = "FLAC (24bit-96kHz)";
                        }
                        break;
                }

                // Display album quality in quality textbox.
                qualityTextbox.Invoke(new Action(() => qualityTextbox.Text = quality));
                #endregion

                #region Cover Art URL
                // Grab cover art link
                frontCoverImg = (string)joAlbumResponse["image"]["large"];
                // Get 150x150 artwork for cover art box
                frontCoverImgBox = frontCoverImg.Replace("_600.jpg", "_150.jpg");
                // Get max sized artwork
                frontCoverImg = frontCoverImg.Replace("_600.jpg", "_max.jpg");

                albumArtPicBox.Invoke(new Action(() => albumArtPicBox.ImageLocation = frontCoverImgBox));
                #endregion

                #region "Goodies" URL (Digital Booklets)
                // Look for "Goodies" (digital booklet)
                var goodiesLog = Regex.Match(albumIDResponseString, "\"goodies\":\\[{(?<notUsed>.*?),\"url\":\"(?<booklet>.*?)\",").Groups;
                var goodiesPDF = goodiesLog[2].Value.Replace(@"\/", "/");
                #endregion

                // Grab all Track IDs listed on the API.
                string trackIDsPattern = "\"version\":(?:.*?),\"id\":(?<trackId>.*?),";
                string trackIDsInput = albumIDResponseString;
                RegexOptions trackIDsOptions = RegexOptions.Multiline;

                foreach (Match m in Regex.Matches(trackIDsInput, trackIDsPattern, trackIDsOptions))
                {
                    // Grab matches for Track IDs
                    trackIdString = string.Format("{0}", m.Groups["trackId"].Value);

                    // Create HttpClient to grab Track ID
                    var trackIDClient = new HttpClient();
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                    // Set user-agent to Firefox.
                    trackIDClient.DefaultRequestHeaders.Add("User-Agent", userAgent);
                    // Set referer to localhost that mora qualitas uses
                    trackIDClient.DefaultRequestHeaders.Add("Referer", "https://play.qobuz.com/album/" + albumId);

                    // Grab response from Qobuz to get info using Track IDs.
                    var trackIDUrl = "https://www.qobuz.com/api.json/0.2/track/get?track_id=" + trackIdString + "&app_id=" + appid + "&user_auth_token=" + userAuth;
                    var trackIDResponse = await trackIDClient.GetAsync(trackIDUrl);
                    string trackIDResponseString = trackIDResponse.Content.ReadAsStringAsync().Result;

                    // Grab metadata from API JSON response
                    JObject joTrackResponse = JObject.Parse(trackIDResponseString);

                    #region Get Information (Tags, Titles, etc.)
                    // Grab tag strings
                    albumArtist = (string)joTrackResponse["album"]["artist"]["name"]; albumArtist = DecodeEncodedNonAsciiCharacters(albumArtist);
                    albumArtistPath = GetSafeFilename(albumArtist);
                    albumArtistTextBox.Invoke(new Action(() => albumArtistTextBox.Text = albumArtist));

                    try
                    {
                        performerName = (string)joTrackResponse["performer"]["name"]; performerName = DecodeEncodedNonAsciiCharacters(performerName);
                        performerNamePath = GetSafeFilename(performerName);
                    }
                    catch { performerName = null; performerNamePath = null; /*Set to null and Ignore if fails*/ }

                    try { composerName = (string)joTrackResponse["composer"]["name"]; composerName = DecodeEncodedNonAsciiCharacters(composerName); } catch { composerName = null; /*Set to null and Ignore if fails*/ }

                    advisory = (string)joTrackResponse["parental_warning"];

                    albumName = (string)joTrackResponse["album"]["title"]; albumName = DecodeEncodedNonAsciiCharacters(albumName);
                    albumNamePath = GetSafeFilename(albumName);
                    albumTextBox.Invoke(new Action(() => albumTextBox.Text = albumName));

                    trackName = (string)joTrackResponse["title"]; trackName = trackName.Trim(); trackName = DecodeEncodedNonAsciiCharacters(trackName);
                    trackNamePath = GetSafeFilename(trackName);

                    versionName = (string)joTrackResponse["version"];
                    if (versionName != null)
                    {
                        versionName = DecodeEncodedNonAsciiCharacters(versionName);
                        versionNamePath = GetSafeFilename(versionName);
                    }

                    genre = (string)joTrackResponse["album"]["genre"]["name"]; genre = DecodeEncodedNonAsciiCharacters(genre);

                    releaseDate = (string)joTrackResponse["album"]["release_date_stream"];
                    releaseDateTextBox.Invoke(new Action(() => releaseDateTextBox.Text = releaseDate));

                    copyright = (string)joTrackResponse["copyright"]; copyright = DecodeEncodedNonAsciiCharacters(copyright);

                    upc = (string)joTrackResponse["album"]["upc"];
                    upcTextBox.Invoke(new Action(() => upcTextBox.Text = upc));

                    isrc = (string)joTrackResponse["isrc"];

                    type = (string)joTrackResponse["album"]["release_type"];

                    // Grab tag ints
                    trackNumber = (int)joTrackResponse["track_number"];

                    trackTotal = (int)joTrackResponse["album"]["tracks_count"];
                    totalTracksTextbox.Invoke(new Action(() => totalTracksTextbox.Text = trackTotal.ToString()));

                    discNumber = (int)joTrackResponse["media_number"];

                    discTotal = (int)joTrackResponse["album"]["media_count"];

                    // Debug output to make sure values are grabbed properly
                    //output.Invoke(new Action(() => output.AppendText("Tags found, listed below...\r\n")));
                    //output.Invoke(new Action(() => output.AppendText("  Album Artist - " + albumArtist + "\r\n")));
                    //output.Invoke(new Action(() => output.AppendText("  Track Artist - " + performerName + "\r\n")));
                    //output.Invoke(new Action(() => output.AppendText("  Composer - " + composerName + "\r\n")));
                    //output.Invoke(new Action(() => output.AppendText("  Advisory - " + advisory + "\r\n")));
                    //output.Invoke(new Action(() => output.AppendText("  Album Name - " + albumName + "\r\n")));
                    //output.Invoke(new Action(() => output.AppendText("  Track Name - " + trackName + "\r\n")));
                    //output.Invoke(new Action(() => output.AppendText("  Track Version - " + versionName + "\r\n")));
                    //output.Invoke(new Action(() => output.AppendText("  Genre - " + genre + "\r\n")));
                    //output.Invoke(new Action(() => output.AppendText("  Release Date - " + releaseDate + "\r\n")));
                    //output.Invoke(new Action(() => output.AppendText("  Copyright - " + copyright + "\r\n")));
                    //output.Invoke(new Action(() => output.AppendText("  UPC - " + upc + "\r\n")));
                    //output.Invoke(new Action(() => output.AppendText("  ISRC - " + isrc + "\r\n")));
                    //output.Invoke(new Action(() => output.AppendText("  Media Type - " + type + "\r\n")));
                    //output.Invoke(new Action(() => output.AppendText("  Track Number - " + trackNumber.ToString() + "\r\n")));
                    //output.Invoke(new Action(() => output.AppendText("  Track Total - " + trackTotal.ToString() + "\r\n")));
                    //output.Invoke(new Action(() => output.AppendText("  Disc Number - " + discNumber.ToString() + "\r\n")));
                    //output.Invoke(new Action(() => output.AppendText("  Disc Total - " + discTotal.ToString() + "\r\n")));

                    #region Availability Check (Streamable?)
                    // Check if available for streaming.
                    string streamCheck = (string)joTrackResponse["streamable"];

                    switch (streamCheck.ToLower())
                    {
                        case "true":
                            break;
                        default:
                            switch (streamableCheckbox.Checked)
                            {
                                case true:
                                    output.Invoke(new Action(() => output.AppendText("Track " + trackNumber.ToString() + " is not available for streaming. Unable to download.\r\n")));
                                    System.Threading.Thread.Sleep(100);
                                    enableBoxes(sender, e);
                                    continue;
                                default:
                                    output.Invoke(new Action(() => output.AppendText("Track " + trackNumber.ToString() + "is not available for streaming. But stremable check is being ignored for debugging, or messed up releases. Attempting to download...\r\n")));
                                    break;
                            }
                            break;
                    }
                    #endregion

                    #endregion

                    #region Filename Number Padding
                    // Set default track number padding length
                    var paddingLength = 2;

                    // Prepare track number padding in filename.
                    string paddingLog = Math.Floor(Math.Log10(trackTotal) + 1).ToString();

                    switch (paddingLog)
                    {
                        case "1":
                            paddingLength = 2;
                            break;
                        default:
                            paddingLength = (int)Math.Floor(Math.Log10(trackTotal) + 1);
                            break;
                    }

                    // Set default disc number padding length
                    var paddingDiscLength = 2;

                    // Prepare disc number padding in filename.
                    string paddingDiscLog = Math.Floor(Math.Log10(discTotal) + 1).ToString();

                    switch (paddingDiscLog)
                    {
                        case "1":
                            paddingDiscLength = 2;
                            break;
                        default:
                            paddingDiscLength = (int)Math.Floor(Math.Log10(trackTotal) + 1);
                            break;
                    }
                    #endregion

                    #region Create Shortened Strings
                    // If name goes over 36 characters, limit it to 36
                    if (albumArtistPath.Length > MaxLength)
                    {
                        albumArtistPath = albumArtistPath.Substring(0, MaxLength).TrimEnd();
                    }

                    if (performerName != null)
                    {
                        if (performerNamePath.Length > MaxLength)
                        {
                            performerNamePath = performerNamePath.Substring(0, MaxLength).TrimEnd();
                        }
                    }

                    if (albumNamePath.Length > MaxLength)
                    {
                        albumNamePath = albumNamePath.Substring(0, MaxLength).TrimEnd();
                    }
                    #endregion

                    #region Create Directories
                    // Create strings for disc folders
                    string discFolder = null;

                    // If more than 1 disc, create folders for discs. Otherwise, strings will remain null.
                    if (discTotal > 1)
                    {
                        discFolder = "CD " + discNumber.ToString().PadLeft(paddingDiscLength, '0');
                    }

                    // Create directories
                    string[] path1 = { loc, albumArtistPath };
                    path1Full = Path.Combine(path1);
                    string[] path2 = { loc, albumArtistPath, albumNamePath };
                    path2Full = Path.Combine(path2);
                    string[] path3 = { loc, albumArtistPath, albumNamePath, qualityPath };
                    path3Full = Path.Combine(path3);

                    switch (discTotal)
                    {
                        case 1:
                            path4Full = path3Full;
                            break;
                        default:
                            string[] path4 = { loc, albumArtistPath, albumNamePath, qualityPath, discFolder };
                            path4Full = Path.Combine(path4);
                            break;
                    }

                    System.IO.Directory.CreateDirectory(path1Full);
                    System.IO.Directory.CreateDirectory(path2Full);
                    System.IO.Directory.CreateDirectory(path3Full);
                    System.IO.Directory.CreateDirectory(path4Full);

                    // Set albumPath to the created directories.
                    string trackPath = path4Full;
                    #endregion

                    #region Create Shortened Strings (Again)
                    // Create final shortened track file names to avoid errors with file names being too long.
                    switch (versionName)
                    {
                        case null:
                            if ((trackNumber.ToString().PadLeft(paddingLength, '0') + " " + trackNamePath).Length > MaxLength)
                            {
                                finalTrackNamePath = (trackNumber.ToString().PadLeft(paddingLength, '0') + " " + trackNamePath).Substring(0, MaxLength).TrimEnd();
                            }
                            else
                            {
                                finalTrackNamePath = (trackNumber.ToString().PadLeft(paddingLength, '0') + " " + trackNamePath).TrimEnd();
                            }
                            break;
                        default:
                            if ((trackNumber.ToString().PadLeft(paddingLength, '0') + " " + trackNamePath + " (" + versionNamePath + ")").Length > MaxLength)
                            {
                                finalTrackNameVersionPath = (trackNumber.ToString().PadLeft(paddingLength, '0') + " " + trackNamePath + " (" + versionNamePath + ")").Substring(0, MaxLength).TrimEnd();
                            }
                            else
                            {
                                finalTrackNameVersionPath = (trackNumber.ToString().PadLeft(paddingLength, '0') + " " + trackNamePath + " (" + versionNamePath + ")").TrimEnd();
                            }
                            break;
                    }
                    #endregion

                    #region Check if File Exists
                    // Check if there is a version name.
                    switch (versionName)
                    {
                        case null:
                            string[] path5 = { trackPath, finalTrackNamePath + audioFileType };
                            string checkFile = Path.Combine(path5);

                            if (System.IO.File.Exists(checkFile))
                            {
                                output.Invoke(new Action(() => output.AppendText("File for \"" + trackNumber.ToString().PadLeft(paddingLength, '0') + " " + trackName + "\" already exists. Skipping.\r\n")));
                                System.Threading.Thread.Sleep(100);
                                continue;
                            }
                            break;
                        default:
                            string[] path5Version = { trackPath, finalTrackNameVersionPath + audioFileType };
                            string checkFileVersion = Path.Combine(path5Version);

                            if (System.IO.File.Exists(checkFileVersion))
                            {
                                output.Invoke(new Action(() => output.AppendText("File for \"" + trackNumber.ToString().PadLeft(paddingLength, '0') + " " + trackName + " (" + versionName + ")" + "\" already exists. Skipping.\r\n")));
                                System.Threading.Thread.Sleep(100);
                                continue;
                            }
                            break;
                    }
                    #endregion

                    // Create streaming URL.
                    createURL(sender, e);

                    try
                    {
                        #region Downloading
                        // Check if there is a version name.
                        switch (versionName)
                        {
                            case null:
                                output.Invoke(new Action(() => output.AppendText("Downloading - " + trackNumber.ToString().PadLeft(paddingLength, '0') + " - " + trackName + " ......")));
                                break;
                            default:
                                output.Invoke(new Action(() => output.AppendText("Downloading - " + trackNumber.ToString().PadLeft(paddingLength, '0') + " - " + trackName + " (" + versionName + ")" + " ......")));
                                break;
                        }

                        // Save streamed file from link
                        using (HttpClient streamClient = new HttpClient())
                        {
                            // Set "range" header to nearly unlimited.
                            streamClient.DefaultRequestHeaders.Range = new RangeHeaderValue(0, 999999999999);
                            // Set user-agent to Firefox.
                            streamClient.DefaultRequestHeaders.Add("User-Agent", userAgent);
                            // Set referer URL to album ID.
                            streamClient.DefaultRequestHeaders.Add("Referer", "https://play.qobuz.com/album/" + albumId);

                            using (HttpResponseMessage streamResponse = await streamClient.GetAsync(stream, HttpCompletionOption.ResponseHeadersRead))
                            using (Stream streamToReadFrom = await streamResponse.Content.ReadAsStreamAsync())
                            {
                                string fileName = Path.GetTempFileName();
                                using (Stream streamToWriteTo = System.IO.File.Open(fileName, FileMode.Create))
                                {
                                    await streamToReadFrom.CopyToAsync(streamToWriteTo);
                                }

                                switch (versionName)
                                {
                                    case null:
                                        string[] path6 = { trackPath, finalTrackNamePath + audioFileType };
                                        string filePath = Path.Combine(path6);

                                        System.IO.File.Move(fileName, filePath);
                                        break;
                                    default:
                                        string[] path6Version = { trackPath, finalTrackNameVersionPath + audioFileType };
                                        string filePathVersion = Path.Combine(path6Version);

                                        System.IO.File.Move(fileName, filePathVersion);
                                        break;
                                }
                            }
                        }
                        #endregion

                        #region Cover Art Saving
                        string[] path7 = { loc, albumArtistPath, albumNamePath, qualityPath, "Cover.jpg" };
                        string coverArtPath = Path.Combine(path7);
                        string[] path7Tag = { loc, albumArtistPath, albumNamePath, qualityPath, artSize + ".jpg" };
                        string coverArtTagPath = Path.Combine(path7Tag);

                        if (System.IO.File.Exists(coverArtPath))
                        {
                            try
                            {
                                // Skip, don't re-download.

                                // Download selected cover art size for tagging files (Currently happens every time a track is downloaded).
                                using (WebClient imgClient = new WebClient())
                                {
                                    imgClient.DownloadFile(new Uri(frontCoverImg.Replace("_max", "_" + artSize)), coverArtTagPath);
                                }
                            }
                            catch
                            {
                                // Ignore, Qobuz servers throw a 404 as if the image doesn't exist.
                            }
                        }
                        else
                        {
                            try
                            {
                                // Save cover art to selected path.
                                using (WebClient imgClient = new WebClient())
                                {
                                    // Download max quality Cover Art to "Cover.jpg" file in chosen path. 
                                    imgClient.DownloadFile(new Uri(frontCoverImg), coverArtPath);

                                    // Download selected cover art size for tagging files (Currently happens every time a track is downloaded).
                                    imgClient.DownloadFile(new Uri(frontCoverImg.Replace("_max", "_" + artSize)), coverArtTagPath);
                                }
                            }
                            catch
                            {
                                // Ignore, Qobuz servers throw a 404 as if the image doesn't exist.
                            }
                        }
                        #endregion

                        #region Tagging
                        switch (versionName)
                        {
                            case null:
                                break;
                            default:
                                finalTrackNamePath = finalTrackNameVersionPath;
                                break;
                        }

                        string[] path8 = { trackPath, finalTrackNamePath + audioFileType };
                        string tagFilePath = Path.Combine(path8);
                        string[] path9 = { loc, albumArtistPath, albumNamePath, qualityPath, artSize + ".jpg" };
                        string tagCoverArtFilePath = Path.Combine(path9);

                        // Set file to tag
                        var tfile = TagLib.File.Create(tagFilePath);

                        switch (audioFileType)
                        {
                            case ".mp3":
                                #region MP3 Tagging
                                // For custom / troublesome tags.
                                TagLib.Id3v2.Tag t = (TagLib.Id3v2.Tag)tfile.GetTag(TagLib.TagTypes.Id3v2);

                                // Saving cover art to file(s)
                                if (imageCheckbox.Checked == true)
                                {
                                    try
                                    {
                                        // Define cover art to use for MP3 file(s)
                                        TagLib.Id3v2.AttachedPictureFrame pic = new TagLib.Id3v2.AttachedPictureFrame();
                                        pic.TextEncoding = TagLib.StringType.Latin1;
                                        pic.MimeType = System.Net.Mime.MediaTypeNames.Image.Jpeg;
                                        pic.Type = TagLib.PictureType.FrontCover;
                                        pic.Data = TagLib.ByteVector.FromPath(tagCoverArtFilePath);

                                        // Save cover art to MP3 file.
                                        tfile.Tag.Pictures = new TagLib.IPicture[1] { pic };
                                        tfile.Save();
                                    }
                                    catch
                                    {
                                        output.Invoke(new Action(() => output.AppendText("Cover art tag fail, .jpg still exists?...")));
                                    }
                                }

                                // Track Title tag
                                if (trackTitleCheckbox.Checked == true)
                                {
                                    switch (versionName)
                                    {
                                        case null:
                                            tfile.Tag.Title = trackName;
                                            break;
                                        default:
                                            tfile.Tag.Title = trackName + " (" + versionName + ")";
                                            break;
                                    }

                                }

                                // Album Title tag
                                if (albumCheckbox.Checked == true) { tfile.Tag.Album = albumName; }

                                // Album Artits tag
                                if (albumArtistCheckbox.Checked == true) { tfile.Tag.AlbumArtists = new string[] { albumArtist }; }

                                // Track Artist tag
                                if (artistCheckbox.Checked == true) { tfile.Tag.Performers = new string[] { performerName }; }

                                // Composer tag
                                if (composerCheckbox.Checked == true) { tfile.Tag.Composers = new string[] { composerName }; }

                                // Release Date tag
                                if (releaseCheckbox.Checked == true) { releaseDate = releaseDate.Substring(0, 4); tfile.Tag.Year = UInt32.Parse(releaseDate); }

                                // Genre tag
                                if (genreCheckbox.Checked == true) { tfile.Tag.Genres = new string[] { genre }; }

                                // Track Number tag
                                if (trackNumberCheckbox.Checked == true) { tfile.Tag.Track = Convert.ToUInt32(trackNumber); }

                                // Disc Number tag
                                if (discNumberCheckbox.Checked == true) { tfile.Tag.Disc = Convert.ToUInt32(discNumber); }

                                // Total Discs tag
                                if (discTotalCheckbox.Checked == true) { tfile.Tag.DiscCount = Convert.ToUInt32(discTotal); }

                                // Total Tracks tag
                                if (trackTotalCheckbox.Checked == true) { tfile.Tag.TrackCount = Convert.ToUInt32(trackTotal); }

                                // Comment tag
                                if (commentCheckbox.Checked == true) { tfile.Tag.Comment = commentTextbox.Text; }

                                // Copyright tag
                                if (copyrightCheckbox.Checked == true) { tfile.Tag.Copyright = copyright; }

                                // ISRC tag
                                if (isrcCheckbox.Checked == true) { TagLib.Id3v2.Tag tag = (TagLib.Id3v2.Tag)tfile.GetTag(TagTypes.Id3v2, true); tag.SetTextFrame("TSRC", isrc); }

                                // Release Type tag
                                if (type != null)
                                {
                                    if (typeCheckbox.Checked == true) { TagLib.Id3v2.Tag tag = (TagLib.Id3v2.Tag)tfile.GetTag(TagTypes.Id3v2, true); tag.SetTextFrame("TMED", type); }
                                }

                                // Save all selected tags to file
                                tfile.Save();
                                #endregion
                                break;
                            case ".flac":
                                #region FLAC Tagging
                                // For custom / troublesome tags.
                                var custom = (TagLib.Ogg.XiphComment)tfile.GetTag(TagLib.TagTypes.Xiph);

                                // Saving cover art to file(s)
                                if (imageCheckbox.Checked == true)
                                {
                                    try
                                    {
                                        // Define cover art to use for FLAC file(s)
                                        TagLib.Id3v2.AttachedPictureFrame pic = new TagLib.Id3v2.AttachedPictureFrame();
                                        pic.TextEncoding = TagLib.StringType.Latin1;
                                        pic.MimeType = System.Net.Mime.MediaTypeNames.Image.Jpeg;
                                        pic.Type = TagLib.PictureType.FrontCover;
                                        pic.Data = TagLib.ByteVector.FromPath(tagCoverArtFilePath);

                                        // Save cover art to FLAC file.
                                        tfile.Tag.Pictures = new TagLib.IPicture[1] { pic };
                                        tfile.Save();
                                    }
                                    catch
                                    {
                                        output.Invoke(new Action(() => output.AppendText("Cover art tag fail, .jpg still exists?...")));
                                    }
                                }

                                // Track Title tag
                                if (trackTitleCheckbox.Checked == true)
                                {
                                    switch (versionName)
                                    {
                                        case null:
                                            tfile.Tag.Title = trackName;
                                            break;
                                        default:
                                            tfile.Tag.Title = trackName + " (" + versionName + ")";
                                            break;
                                    }
                                }

                                // Album Title tag
                                if (albumCheckbox.Checked == true) { tfile.Tag.Album = albumName; }

                                // Album Artits tag
                                if (albumArtistCheckbox.Checked == true) { custom.SetField("ALBUMARTIST", new string[] { albumArtist }); }

                                // Track Artist tag
                                if (artistCheckbox.Checked == true) { custom.SetField("ARTIST", new string[] { performerName }); }

                                // Composer tag
                                if (composerCheckbox.Checked == true) { custom.SetField("COMPOSER", new string[] { composerName }); }

                                // Release Date tag
                                if (releaseCheckbox.Checked == true) { custom.SetField("YEAR", new string[] { releaseDate }); }

                                // Genre tag
                                if (genreCheckbox.Checked == true) { custom.SetField("GENRE", new string[] { genre }); }

                                // Track Number tag
                                if (trackNumberCheckbox.Checked == true) { tfile.Tag.Track = Convert.ToUInt32(trackNumber); }

                                // Disc Number tag
                                if (discNumberCheckbox.Checked == true) { tfile.Tag.Disc = Convert.ToUInt32(discNumber); }

                                // Total Discs tag
                                if (discTotalCheckbox.Checked == true) { tfile.Tag.DiscCount = Convert.ToUInt32(discTotal); }

                                // Total Tracks tag
                                if (trackTotalCheckbox.Checked == true) { tfile.Tag.TrackCount = Convert.ToUInt32(trackTotal); }

                                // Comment tag
                                if (commentCheckbox.Checked == true) { custom.SetField("COMMENT", new string[] { commentTextbox.Text }); }

                                // Copyright tag
                                if (copyrightCheckbox.Checked == true) { custom.SetField("COPYRIGHT", new string[] { copyright }); }
                                // UPC tag
                                if (upcCheckbox.Checked == true) { custom.SetField("UPC", new string[] { upc }); }

                                // ISRC tag
                                if (isrcCheckbox.Checked == true) { custom.SetField("ISRC", new string[] { isrc }); }

                                // Release Type tag
                                if (type != null)
                                {
                                    if (typeCheckbox.Checked == true) { custom.SetField("MEDIATYPE", new string[] { type }); }
                                }

                                // Explicit tag
                                if (explicitCheckbox.Checked == true)
                                {
                                    if (advisory == "false") { custom.SetField("ITUNESADVISORY", new string[] { "0" }); } else { custom.SetField("ITUNESADVISORY", new string[] { "1" }); }
                                }

                                // Save all selected tags to file
                                tfile.Save();
                                #endregion
                                break;
                        }
                        #endregion
                    }
                    catch (Exception downloadError)
                    {
                        // If there is an issue trying to, or during the download, show error info.
                        string error = downloadError.ToString();
                        output.Invoke(new Action(() => output.AppendText("\r\n\r\n")));
                        output.Invoke(new Action(() => output.AppendText("Track Download ERROR. Information below.\r\n\r\n")));
                        output.Invoke(new Action(() => output.AppendText(error)));
                        enableBoxes(sender, e);
                        return;
                    }

                    // Delete image file used for tagging
                    string[] path12 = { loc, albumArtistPath, albumNamePath, qualityPath, artSize + ".jpg" };
                    string coverArtTagDelete = Path.Combine(path12);

                    if (System.IO.File.Exists(coverArtTagDelete))
                    {
                        System.IO.File.Delete(coverArtTagDelete);
                    }

                    // Say when a track is done downloading, then wait for the next track / end.
                    output.Invoke(new Action(() => output.AppendText("Track Download Done!\r\n")));
                    System.Threading.Thread.Sleep(100);
                }

                #region Digital Booklet
                string[] path11 = { loc, albumArtistPath, albumNamePath, qualityPath, "Digital Booklet.pdf" };
                string goodiesPath = Path.Combine(path11);
                // If a booklet was found, save it.
                if (goodiesPDF == null | goodiesPDF == "")
                {
                    // No need to download something that doesn't exist.
                }
                else
                {
                    if (System.IO.File.Exists(goodiesPath))
                    {
                        // Skip, don't re-download.
                    }
                    else
                    {
                        // Save digital booklet to selected path
                        output.Invoke(new Action(() => output.AppendText("Goodies found, downloading...")));
                        using (WebClient bookClient = new WebClient())
                        {
                            // Download max quality Cover Art to "Cover.jpg" file in chosen path. 
                            bookClient.DownloadFile(new Uri(goodiesPDF), goodiesPath);
                        }
                    }
                }
                #endregion

                // Say that downloading is completed.
                output.Invoke(new Action(() => output.AppendText("\r\n\r\n")));
                output.Invoke(new Action(() => output.AppendText("Downloading job completed! All downloaded files will be located in your chosen path.")));
                mp3Checkbox.Invoke(new Action(() => mp3Checkbox.Visible = true));
                enableBoxes(sender, e);
            }
            catch (Exception ex)
            {
                string error = ex.ToString();
                //output.Invoke(new Action(() => output.Text = String.Empty));
                output.Invoke(new Action(() => output.AppendText("Failed to download (First Phase). Error information below.\r\n\r\n")));
                output.Invoke(new Action(() => output.AppendText(error)));
                enableBoxes(sender, e);
                return;
            }
            #endregion
        }

        // For downloading "track" links (Re-work started July 5, 2020)
        private async void downloadTrackBG_DoWork(object sender, DoWorkEventArgs e)
        {
            #region If URL has "track"
            // Set "loc" as the selected path.
            String loc = folderBrowserDialog.SelectedPath;
            
            // Set Track ID to the ID in the provided Qobuz link.
            trackIdString = albumId;

            // Create HttpClient to grab Track ID
            var trackIDClient = new HttpClient();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            // Set user-agent to Firefox.
            trackIDClient.DefaultRequestHeaders.Add("User-Agent", userAgent);
            // Set referer to localhost that mora qualitas uses
            trackIDClient.DefaultRequestHeaders.Add("Referer", "https://play.qobuz.com/album/" + albumId);

            // Empty output, then say Starting Downloads.
            output.Invoke(new Action(() => output.Text = String.Empty));
            output.Invoke(new Action(() => output.AppendText("Starting Downloads...\r\n\r\n")));

            // Grab response from Qobuz to get Track IDs from Album response.
            var trackIDUrl = "https://www.qobuz.com/api.json/0.2/track/get?track_id=" + albumId + "&app_id=" + appid + "&user_auth_token=" + userAuth;
            var trackIDResponse = await trackIDClient.GetAsync(trackIDUrl);
            string trackIDResponseString = trackIDResponse.Content.ReadAsStringAsync().Result;

            // Grab metadata from API JSON response
            JObject joResponse2 = JObject.Parse(trackIDResponseString);

            #region Availability Check (Valid Link?)
            // Check if available at all.
            string errorCheck = (string)joResponse2["code"];
            string errorMessageCheck = (string)joResponse2["message"];

            switch (errorCheck)
            {
                case "404":
                    output.Invoke(new Action(() => output.Text = String.Empty));
                    output.Invoke(new Action(() => output.AppendText("ERROR: 404\r\n")));
                    output.Invoke(new Action(() => output.AppendText("Error message is \"" + errorMessageCheck + "\"\r\n")));
                    output.Invoke(new Action(() => output.AppendText("This usually means the link is invalid, or isn't available in the region your account is from.")));
                    enableBoxes(sender, e);
                    return;
            }
            #endregion

            #region Quality Info (Bitrate & Sample Rate)
            // Grab sample rate and bit depth for album track is from.
            var bitDepth = (string)joResponse2["album"]["maximum_bit_depth"];
            var sampleRate = (string)joResponse2["album"]["maximum_sampling_rate"];

            var quality = "FLAC (" + bitDepth + "bit/" + sampleRate + "kHz)";
            var qualityPath = quality.Replace(@"\", "-").Replace(@"/", "-");

            switch (formatIdString)
            {
                case "5":
                    quality = "MP3 320kbps CBR";
                    qualityPath = "MP3";
                    break;
                case "6":
                    quality = "FLAC (16bit/44.1kHz)";
                    qualityPath = "FLAC (16bit-44.1kHz)";
                    break;
                case "7":
                    if (quality == "FLAC (24bit/192kHz)")
                    {
                        quality = "FLAC (24bit/96kHz)";
                        qualityPath = "FLAC (24bit-96kHz)";
                    }
                    break;
            }

            // Display album quality in quality textbox.
            qualityTextbox.Invoke(new Action(() => qualityTextbox.Text = quality));
            #endregion

            #region Cover Art URL
            // Grab cover art link
            frontCoverImg = (string)joResponse2["album"]["image"]["large"];
            // Get 150x150 artwork for cover art box
            frontCoverImgBox = frontCoverImg.Replace("_600.jpg", "_150.jpg");
            // Get max sized artwork
            frontCoverImg = frontCoverImg.Replace("_600.jpg", "_max.jpg");

            albumArtPicBox.Invoke(new Action(() => albumArtPicBox.ImageLocation = frontCoverImgBox));
            #endregion

            #region Get Information (Tags, Titles, etc.)
            // Grab tag strings
            albumArtist = (string)joResponse2["album"]["artist"]["name"]; albumArtist = DecodeEncodedNonAsciiCharacters(albumArtist);
            albumArtistPath = GetSafeFilename(albumArtist);
            albumArtistTextBox.Invoke(new Action(() => albumArtistTextBox.Text = albumArtist));

            try
            {
                performerName = (string)joResponse2["performer"]["name"]; performerName = DecodeEncodedNonAsciiCharacters(performerName);
                performerNamePath = GetSafeFilename(performerName);
            }
            catch { performerName = null; performerNamePath = null; /*Set to null and Ignore if fails*/ }

            try { composerName = (string)joResponse2["composer"]["name"]; composerName = DecodeEncodedNonAsciiCharacters(composerName); } catch { composerName = null; /*Set to null and Ignore if fails*/ }

            advisory = (string)joResponse2["parental_warning"];

            albumName = (string)joResponse2["album"]["title"]; albumName = DecodeEncodedNonAsciiCharacters(albumName);
            albumNamePath = GetSafeFilename(albumName);
            albumTextBox.Invoke(new Action(() => albumTextBox.Text = albumName));

            trackName = (string)joResponse2["title"]; trackName = trackName.Trim(); trackName = DecodeEncodedNonAsciiCharacters(trackName);
            trackNamePath = GetSafeFilename(trackName);

            versionName = (string)joResponse2["version"];
            if (versionName != null)
            {
                versionName = DecodeEncodedNonAsciiCharacters(versionName);
                versionNamePath = GetSafeFilename(versionName);
            }

            genre = (string)joResponse2["album"]["genre"]["name"]; genre = DecodeEncodedNonAsciiCharacters(genre);

            releaseDate = (string)joResponse2["album"]["release_date_stream"];
            releaseDateTextBox.Invoke(new Action(() => releaseDateTextBox.Text = releaseDate));

            copyright = (string)joResponse2["copyright"]; copyright = DecodeEncodedNonAsciiCharacters(copyright);

            upc = (string)joResponse2["album"]["upc"];
            upcTextBox.Invoke(new Action(() => upcTextBox.Text = upc));

            isrc = (string)joResponse2["isrc"];

            type = (string)joResponse2["album"]["release_type"];

            // Grab tag ints
            trackNumber = (int)joResponse2["track_number"];

            trackTotal = (int)joResponse2["album"]["tracks_count"];
            totalTracksTextbox.Invoke(new Action(() => totalTracksTextbox.Text = trackTotal.ToString()));

            discNumber = (int)joResponse2["media_number"];

            discTotal = (int)joResponse2["album"]["media_count"];

            // Debug output to make sure values are grabbed properly
            //output.Invoke(new Action(() => output.AppendText("Tags found, listed below...\r\n")));
            //output.Invoke(new Action(() => output.AppendText("  Album Artist - " + albumArtist + "\r\n")));
            //output.Invoke(new Action(() => output.AppendText("  Track Artist - " + performerName + "\r\n")));
            //output.Invoke(new Action(() => output.AppendText("  Composer - " + composerName + "\r\n")));
            //output.Invoke(new Action(() => output.AppendText("  Advisory - " + advisory + "\r\n")));
            //output.Invoke(new Action(() => output.AppendText("  Album Name - " + albumName + "\r\n")));
            //output.Invoke(new Action(() => output.AppendText("  Track Name - " + trackName + "\r\n")));
            //output.Invoke(new Action(() => output.AppendText("  Track Version - " + versionName + "\r\n")));
            //output.Invoke(new Action(() => output.AppendText("  Genre - " + genre + "\r\n")));
            //output.Invoke(new Action(() => output.AppendText("  Release Date - " + releaseDate + "\r\n")));
            //output.Invoke(new Action(() => output.AppendText("  Copyright - " + copyright + "\r\n")));
            //output.Invoke(new Action(() => output.AppendText("  UPC - " + upc + "\r\n")));
            //output.Invoke(new Action(() => output.AppendText("  ISRC - " + isrc + "\r\n")));
            //output.Invoke(new Action(() => output.AppendText("  Media Type - " + type + "\r\n")));
            //output.Invoke(new Action(() => output.AppendText("  Track Number - " + trackNumber.ToString() + "\r\n")));
            //output.Invoke(new Action(() => output.AppendText("  Track Total - " + trackTotal.ToString() + "\r\n")));
            //output.Invoke(new Action(() => output.AppendText("  Disc Number - " + discNumber.ToString() + "\r\n")));
            //output.Invoke(new Action(() => output.AppendText("  Disc Total - " + discTotal.ToString() + "\r\n")));

            #region Availability Check (Streamable?)
            // Check if available for streaming.
            string streamCheck = (string)joResponse2["streamable"];

            switch (streamCheck.ToLower())
            {
                case "true":
                    break;
                default:
                    switch (streamableCheckbox.Checked)
                    {
                        case true:
                            output.Invoke(new Action(() => output.AppendText("Track is not available for streaming. Unable to download.\r\n")));
                            System.Threading.Thread.Sleep(100);
                            enableBoxes(sender, e);
                            return;
                        default:
                            output.Invoke(new Action(() => output.AppendText("Track is not available for streaming. But stremable check is being ignored for debugging, or messed up releases. Attempting to download...\r\n")));
                            break;
                    }
                    break;
            }
            #endregion

            #endregion
            
            #region Filename Number Padding
            // Set default track number padding length
            var paddingLength = 2;

            // Prepare track number padding in filename.
            string paddingLog = Math.Floor(Math.Log10(trackTotal) + 1).ToString();

            switch (paddingLog)
            {
                case "1":
                    paddingLength = 2;
                    break;
                default:
                    paddingLength = (int)Math.Floor(Math.Log10(trackTotal) + 1);
                    break;
            }

            // Set default disc number padding length
            var paddingDiscLength = 2;

            // Prepare disc number padding in filename.
            string paddingDiscLog = Math.Floor(Math.Log10(discTotal) + 1).ToString();

            switch (paddingDiscLog)
            {
                case "1":
                    paddingDiscLength = 2;
                    break;
                default:
                    paddingDiscLength = (int)Math.Floor(Math.Log10(trackTotal) + 1);
                    break;
            }
            #endregion

            #region Create Shortened Strings
            // If name goes over 36 characters, limit it to 36
            if (albumArtistPath.Length > MaxLength)
            {
                albumArtistPath = albumArtistPath.Substring(0, MaxLength).TrimEnd();
            }

            if (performerName != null)
            {
                if (performerNamePath.Length > MaxLength)
                {
                    performerNamePath = performerNamePath.Substring(0, MaxLength).TrimEnd();
                }
            }

            if (albumNamePath.Length > MaxLength)
            {
                albumNamePath = albumNamePath.Substring(0, MaxLength).TrimEnd();
            }
            #endregion

            #region Create Directories
            // Create strings for disc folders
            string discFolder = null;

            // If more than 1 disc, create folders for discs. Otherwise, strings will remain null.
            if (discTotal > 1)
            {
                discFolder = "CD " + discNumber.ToString().PadLeft(paddingDiscLength, '0');
            }

            // Create directories
            string[] path1 = { loc, albumArtistPath };
            path1Full = Path.Combine(path1);
            string[] path2 = { loc, albumArtistPath, albumNamePath };
            path2Full = Path.Combine(path2);
            string[] path3 = { loc, albumArtistPath, albumNamePath, qualityPath };
            path3Full = Path.Combine(path3);

            switch (discTotal)
            {
                case 1:
                    path4Full = path3Full;
                    break;
                default:
                    string[] path4 = { loc, albumArtistPath, albumNamePath, qualityPath, discFolder };
                    path4Full = Path.Combine(path4);
                    break;
            }

            System.IO.Directory.CreateDirectory(path1Full);
            System.IO.Directory.CreateDirectory(path2Full);
            System.IO.Directory.CreateDirectory(path3Full);
            System.IO.Directory.CreateDirectory(path4Full);

            // Set albumPath to the created directories.
            string trackPath = path4Full;
            #endregion

            #region Create Shortened Strings (Again)
            // Create final shortened track file names to avoid errors with file names being too long.
            switch (versionName)
            {
                case null:
                    if ((trackNumber.ToString().PadLeft(paddingLength, '0') + " " + trackNamePath).Length > MaxLength)
                    {
                        finalTrackNamePath = (trackNumber.ToString().PadLeft(paddingLength, '0') + " " + trackNamePath).Substring(0, MaxLength).TrimEnd();
                    }
                    else
                    {
                        finalTrackNamePath = (trackNumber.ToString().PadLeft(paddingLength, '0') + " " + trackNamePath).TrimEnd();
                    }
                    break;
                default:
                    if ((trackNumber.ToString().PadLeft(paddingLength, '0') + " " + trackNamePath + " (" + versionNamePath + ")").Length > MaxLength)
                    {
                        finalTrackNameVersionPath = (trackNumber.ToString().PadLeft(paddingLength, '0') + " " + trackNamePath + " (" + versionNamePath + ")").Substring(0, MaxLength).TrimEnd();
                    }
                    else
                    {
                        finalTrackNameVersionPath = (trackNumber.ToString().PadLeft(paddingLength, '0') + " " + trackNamePath + " (" + versionNamePath + ")").TrimEnd();
                    }
                    break;
            }
            #endregion

            #region Check if File Exists
            // Check if there is a version name.
            switch (versionName)
            {
                case null:
                    string[] path5 = { trackPath, finalTrackNamePath + audioFileType };
                    string checkFile = Path.Combine(path5);

                    if (System.IO.File.Exists(checkFile))
                    {
                        output.Invoke(new Action(() => output.AppendText("File for \"" + trackNumber.ToString().PadLeft(paddingLength, '0') + " " + trackName + "\" already exists. Skipping.\r\n")));
                        System.Threading.Thread.Sleep(100);
                        enableBoxes(sender, e);
                        return;
                    }
                    break;
                default:
                    string[] path5Version = { trackPath, finalTrackNameVersionPath + audioFileType };
                    string checkFileVersion = Path.Combine(path5Version);

                    if (System.IO.File.Exists(checkFileVersion))
                    {
                        output.Invoke(new Action(() => output.AppendText("File for \"" + trackNumber.ToString().PadLeft(paddingLength, '0') + " " + trackName + " (" + versionName + ")" + "\" already exists. Skipping.\r\n")));
                        System.Threading.Thread.Sleep(100);
                        enableBoxes(sender, e);
                        return;
                    }
                    break;
            }
            #endregion
            
            // Create streaming URL.
            createURL(sender, e);

            try
            {
                #region Downloading
                // Check if there is a version name.
                switch (versionName)
                {
                    case null:
                        output.Invoke(new Action(() => output.AppendText("Downloading - " + trackNumber.ToString().PadLeft(paddingLength, '0') + " - " + trackName + " ......")));
                        break;
                    default:
                        output.Invoke(new Action(() => output.AppendText("Downloading - " + trackNumber.ToString().PadLeft(paddingLength, '0') + " - " + trackName + " (" + versionName + ")" + " ......")));
                        break;
                }

                // Save streamed file from link
                using (HttpClient streamClient = new HttpClient())
                {
                    // Set "range" header to nearly unlimited.
                    streamClient.DefaultRequestHeaders.Range = new RangeHeaderValue(0, 999999999999);
                    // Set user-agent to Firefox.
                    streamClient.DefaultRequestHeaders.Add("User-Agent", userAgent);
                    // Set referer URL to album ID.
                    streamClient.DefaultRequestHeaders.Add("Referer", "https://play.qobuz.com/album/" + albumId);

                    using (HttpResponseMessage streamResponse = await streamClient.GetAsync(stream, HttpCompletionOption.ResponseHeadersRead))
                    using (Stream streamToReadFrom = await streamResponse.Content.ReadAsStreamAsync())
                    {
                        string fileName = Path.GetTempFileName();
                        using (Stream streamToWriteTo = System.IO.File.Open(fileName, FileMode.Create))
                        {
                            await streamToReadFrom.CopyToAsync(streamToWriteTo);
                        }

                        switch (versionName)
                        {
                            case null:
                                string[] path6 = { trackPath, finalTrackNamePath + audioFileType };
                                string filePath = Path.Combine(path6);

                                System.IO.File.Move(fileName, filePath);
                                break;
                            default:
                                string[] path6Version = { trackPath, finalTrackNameVersionPath + audioFileType };
                                string filePathVersion = Path.Combine(path6Version);

                                System.IO.File.Move(fileName, filePathVersion);
                                break;
                        }
                    }
                }
                #endregion

                #region Cover Art Saving
                string[] path7 = { loc, albumArtistPath, albumNamePath, qualityPath, "Cover.jpg" };
                string coverArtPath = Path.Combine(path7);
                string[] path7Tag = { loc, albumArtistPath, albumNamePath, qualityPath, artSize + ".jpg" };
                string coverArtTagPath = Path.Combine(path7Tag);

                if (System.IO.File.Exists(coverArtPath))
                {
                    try
                    {
                        // Skip, don't re-download.

                        // Download selected cover art size for tagging files (Currently happens every time a track is downloaded).
                        using (WebClient imgClient = new WebClient())
                        {
                            imgClient.DownloadFile(new Uri(frontCoverImg.Replace("_max", "_" + artSize)), coverArtTagPath);
                        }
                    }
                    catch
                    {
                        // Ignore, Qobuz servers throw a 404 as if the image doesn't exist.
                    }
                }
                else
                {
                    try
                    {
                        // Save cover art to selected path.
                        using (WebClient imgClient = new WebClient())
                        {
                            // Download max quality Cover Art to "Cover.jpg" file in chosen path. 
                            imgClient.DownloadFile(new Uri(frontCoverImg), coverArtPath);

                            // Download selected cover art size for tagging files (Currently happens every time a track is downloaded).
                            imgClient.DownloadFile(new Uri(frontCoverImg.Replace("_max", "_" + artSize)), coverArtTagPath);
                        }
                    }
                    catch
                    {
                        // Ignore, Qobuz servers throw a 404 as if the image doesn't exist.
                    }
                }
                #endregion

                #region Tagging
                switch (versionName)
                {
                    case null:
                        break;
                    default:
                        finalTrackNamePath = finalTrackNameVersionPath;
                        break;
                }

                string[] path8 = { trackPath, finalTrackNamePath + audioFileType };
                string tagFilePath = Path.Combine(path8);
                string[] path9 = { loc, albumArtistPath, albumNamePath, qualityPath, artSize + ".jpg" };
                string tagCoverArtFilePath = Path.Combine(path9);

                // Set file to tag
                var tfile = TagLib.File.Create(tagFilePath);

                switch (audioFileType)
                {
                    case ".mp3":
                        #region MP3 Tagging
                        // For custom / troublesome tags.
                        TagLib.Id3v2.Tag t = (TagLib.Id3v2.Tag)tfile.GetTag(TagLib.TagTypes.Id3v2);

                        // Saving cover art to file(s)
                        if (imageCheckbox.Checked == true)
                        {
                            try
                            {
                                // Define cover art to use for MP3 file(s)
                                TagLib.Id3v2.AttachedPictureFrame pic = new TagLib.Id3v2.AttachedPictureFrame();
                                pic.TextEncoding = TagLib.StringType.Latin1;
                                pic.MimeType = System.Net.Mime.MediaTypeNames.Image.Jpeg;
                                pic.Type = TagLib.PictureType.FrontCover;
                                pic.Data = TagLib.ByteVector.FromPath(tagCoverArtFilePath);

                                // Save cover art to MP3 file.
                                tfile.Tag.Pictures = new TagLib.IPicture[1] { pic };
                                tfile.Save();
                            }
                            catch
                            {
                                output.Invoke(new Action(() => output.AppendText("Cover art tag fail, .jpg still exists?...")));
                            }
                        }

                        // Track Title tag
                        if (trackTitleCheckbox.Checked == true)
                        {
                            switch (versionName)
                            {
                                case null:
                                    tfile.Tag.Title = trackName;
                                    break;
                                default:
                                    tfile.Tag.Title = trackName + " (" + versionName + ")";
                                    break;
                            }
                            
                        }

                        // Album Title tag
                        if (albumCheckbox.Checked == true) { tfile.Tag.Album = albumName; }

                        // Album Artits tag
                        if (albumArtistCheckbox.Checked == true) { tfile.Tag.AlbumArtists = new string[] { albumArtist }; }

                        // Track Artist tag
                        if (artistCheckbox.Checked == true) { tfile.Tag.Performers = new string[] { performerName }; }

                        // Composer tag
                        if (composerCheckbox.Checked == true) { tfile.Tag.Composers = new string[] { composerName }; }

                        // Release Date tag
                        if (releaseCheckbox.Checked == true) { releaseDate = releaseDate.Substring(0, 4); tfile.Tag.Year = UInt32.Parse(releaseDate); }

                        // Genre tag
                        if (genreCheckbox.Checked == true) { tfile.Tag.Genres = new string[] { genre }; }

                        // Track Number tag
                        if (trackNumberCheckbox.Checked == true) { tfile.Tag.Track = Convert.ToUInt32(trackNumber); }

                        // Disc Number tag
                        if (discNumberCheckbox.Checked == true) { tfile.Tag.Disc = Convert.ToUInt32(discNumber); }

                        // Total Discs tag
                        if (discTotalCheckbox.Checked == true) { tfile.Tag.DiscCount = Convert.ToUInt32(discTotal); }

                        // Total Tracks tag
                        if (trackTotalCheckbox.Checked == true) { tfile.Tag.TrackCount = Convert.ToUInt32(trackTotal); }

                        // Comment tag
                        if (commentCheckbox.Checked == true) { tfile.Tag.Comment = commentTextbox.Text; }

                        // Copyright tag
                        if (copyrightCheckbox.Checked == true) { tfile.Tag.Copyright = copyright; }

                        // ISRC tag
                        if (isrcCheckbox.Checked == true) { TagLib.Id3v2.Tag tag = (TagLib.Id3v2.Tag)tfile.GetTag(TagTypes.Id3v2, true); tag.SetTextFrame("TSRC", isrc); }

                        // Release Type tag
                        if (type != null)
                        {
                            if (typeCheckbox.Checked == true) { TagLib.Id3v2.Tag tag = (TagLib.Id3v2.Tag)tfile.GetTag(TagTypes.Id3v2, true); tag.SetTextFrame("TMED", type); }
                        }

                        // Save all selected tags to file
                        tfile.Save();
                        #endregion
                        break;
                    case ".flac":
                        #region FLAC Tagging
                        // For custom / troublesome tags.
                        var custom = (TagLib.Ogg.XiphComment)tfile.GetTag(TagLib.TagTypes.Xiph);

                        // Saving cover art to file(s)
                        if (imageCheckbox.Checked == true)
                        {
                            try
                            {
                                // Define cover art to use for FLAC file(s)
                                TagLib.Id3v2.AttachedPictureFrame pic = new TagLib.Id3v2.AttachedPictureFrame();
                                pic.TextEncoding = TagLib.StringType.Latin1;
                                pic.MimeType = System.Net.Mime.MediaTypeNames.Image.Jpeg;
                                pic.Type = TagLib.PictureType.FrontCover;
                                pic.Data = TagLib.ByteVector.FromPath(tagCoverArtFilePath);

                                // Save cover art to FLAC file.
                                tfile.Tag.Pictures = new TagLib.IPicture[1] { pic };
                                tfile.Save();
                            }
                            catch
                            {
                                output.Invoke(new Action(() => output.AppendText("Cover art tag fail, .jpg still exists?...")));
                            }
                        }

                        // Track Title tag
                        if (trackTitleCheckbox.Checked == true)
                        {
                            switch (versionName)
                            {
                                case null:
                                    tfile.Tag.Title = trackName;
                                    break;
                                default:
                                    tfile.Tag.Title = trackName + " (" + versionName + ")";
                                    break;
                            }
                        }

                        // Album Title tag
                        if (albumCheckbox.Checked == true) { tfile.Tag.Album = albumName; }

                        // Album Artits tag
                        if (albumArtistCheckbox.Checked == true) { custom.SetField("ALBUMARTIST", new string[] { albumArtist }); }

                        // Track Artist tag
                        if (artistCheckbox.Checked == true) { custom.SetField("ARTIST", new string[] { performerName }); }

                        // Composer tag
                        if (composerCheckbox.Checked == true) { custom.SetField("COMPOSER", new string[] { composerName }); }

                        // Release Date tag
                        if (releaseCheckbox.Checked == true) { custom.SetField("YEAR", new string[] { releaseDate }); }

                        // Genre tag
                        if (genreCheckbox.Checked == true) { custom.SetField("GENRE", new string[] { genre }); }

                        // Track Number tag
                        if (trackNumberCheckbox.Checked == true) { tfile.Tag.Track = Convert.ToUInt32(trackNumber); }

                        // Disc Number tag
                        if (discNumberCheckbox.Checked == true) { tfile.Tag.Disc = Convert.ToUInt32(discNumber); }

                        // Total Discs tag
                        if (discTotalCheckbox.Checked == true) { tfile.Tag.DiscCount = Convert.ToUInt32(discTotal); }

                        // Total Tracks tag
                        if (trackTotalCheckbox.Checked == true) { tfile.Tag.TrackCount = Convert.ToUInt32(trackTotal); }

                        // Comment tag
                        if (commentCheckbox.Checked == true) { custom.SetField("COMMENT", new string[] { commentTextbox.Text }); }

                        // Copyright tag
                        if (copyrightCheckbox.Checked == true) { custom.SetField("COPYRIGHT", new string[] { copyright }); }
                        // UPC tag
                        if (upcCheckbox.Checked == true) { custom.SetField("UPC", new string[] { upc }); }

                        // ISRC tag
                        if (isrcCheckbox.Checked == true) { custom.SetField("ISRC", new string[] { isrc }); }

                        // Release Type tag
                        if (type != null)
                        {
                            if (typeCheckbox.Checked == true) { custom.SetField("MEDIATYPE", new string[] { type }); }
                        }

                        // Explicit tag
                        if (explicitCheckbox.Checked == true)
                        {
                            if (advisory == "false") { custom.SetField("ITUNESADVISORY", new string[] { "0" }); } else { custom.SetField("ITUNESADVISORY", new string[] { "1" }); }
                        }

                        // Save all selected tags to file
                        tfile.Save();
                        #endregion
                        break;
                }
                #endregion
            }
            catch (Exception downloadError)
            {
                // If there is an issue trying to, or during the download, show error info.
                string error = downloadError.ToString();
                output.Invoke(new Action(() => output.AppendText("\r\n\r\n")));
                output.Invoke(new Action(() => output.AppendText("Track Download ERROR. Information below.\r\n\r\n")));
                output.Invoke(new Action(() => output.AppendText(error)));
                enableBoxes(sender, e);
                return;
            }

            // Delete image file used for tagging
            string[] path11 = { loc, albumArtistPath, albumNamePath, qualityPath, artSize + ".jpg" };
            string coverArtTagDelete = Path.Combine(path11);

            if (System.IO.File.Exists(coverArtTagDelete))
            {
                System.IO.File.Delete(coverArtTagDelete);
            }

            // Say that downloading is completed.
            output.Invoke(new Action(() => output.AppendText("Track Download Done!\r\n\r\n")));
            output.Invoke(new Action(() => output.AppendText("File will be located in your selected path.")));
            enableBoxes(sender, e);
            #endregion
        }
        #endregion

        #region Tagging Options
        private void tagsLabel_Click(object sender, EventArgs e)
        {
            if (this.Height == 533)
            {
                //New Height
                this.Height = 733;
                tagsLabel.Text = "🠉 Choose which tags to save (click me) 🠉";
            }
            else if (this.Height == 733)
            {
                //New Height
                this.Height = 533;
                tagsLabel.Text = "🠋 Choose which tags to save (click me) 🠋";
            }

        }

        private void albumCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.albumTag = albumCheckbox.Checked;
            Settings.Default.Save();
        }

        private void albumArtistCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.albumArtistTag = albumArtistCheckbox.Checked;
            Settings.Default.Save();
        }

        private void trackTitleCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.trackTitleTag = trackTitleCheckbox.Checked;
            Settings.Default.Save();
        }

        private void artistCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.artistTag = artistCheckbox.Checked;
            Settings.Default.Save();
        }

        private void trackNumberCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.trackTag = trackTitleCheckbox.Checked;
            Settings.Default.Save();
        }

        private void trackTotalCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.totalTracksTag = trackTotalCheckbox.Checked;
            Settings.Default.Save();
        }

        private void discNumberCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.discTag = discNumberCheckbox.Checked;
            Settings.Default.Save();
        }

        private void discTotalCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.totalDiscsTag = discTotalCheckbox.Checked;
            Settings.Default.Save();
        }

        private void releaseCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.yearTag = releaseCheckbox.Checked;
            Settings.Default.Save();
        }

        private void genreCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.genreTag = genreCheckbox.Checked;
            Settings.Default.Save();
        }

        private void composerCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.composerTag = composerCheckbox.Checked;
            Settings.Default.Save();
        }

        private void copyrightCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.copyrightTag = copyrightCheckbox.Checked;
            Settings.Default.Save();
        }

        private void isrcCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.isrcTag = isrcCheckbox.Checked;
            Settings.Default.Save();
        }

        private void typeCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.typeTag = typeCheckbox.Checked;
            Settings.Default.Save();
        }

        private void upcCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.upcTag = upcCheckbox.Checked;
            Settings.Default.Save();
        }

        private void explicitCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.explicitTag = explicitCheckbox.Checked;
            Settings.Default.Save();
        }

        private void commentCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.commentTag = commentCheckbox.Checked;
            Settings.Default.Save();
        }

        private void imageCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.imageTag = imageCheckbox.Checked;
            Settings.Default.Save();
        }

        private void commentTextbox_TextChanged(object sender, EventArgs e)
        {
            Settings.Default.commentText = commentTextbox.Text;
            Settings.Default.Save();
        }
        #endregion

        #region Quality Options
        private void flacHighCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.quality4 = flacHighCheckbox.Checked;
            Settings.Default.Save();

            if (flacHighCheckbox.Checked == true)
            {
                formatIdString = "27";
                audioFileType = ".flac";
                Settings.Default.qualityFormat = formatIdString;
                Settings.Default.audioType = audioFileType;
                downloadButton.Enabled = true;
                flacMidCheckbox.Checked = false;
                flacLowCheckbox.Checked = false;
                mp3Checkbox.Checked = false;
            }
            else
            {
                if (flacMidCheckbox.Checked == false & flacLowCheckbox.Checked == false & mp3Checkbox.Checked == false)
                {
                    downloadButton.Enabled = false;
                }
            }
        }

        private void flacMidCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.quality3 = flacMidCheckbox.Checked;
            Settings.Default.Save();

            if (flacMidCheckbox.Checked == true)
            {
                formatIdString = "7";
                audioFileType = ".flac";
                Settings.Default.qualityFormat = formatIdString;
                Settings.Default.audioType = audioFileType;
                downloadButton.Enabled = true;
                flacHighCheckbox.Checked = false;
                flacLowCheckbox.Checked = false;
                mp3Checkbox.Checked = false;
            }
            else
            {
                if (flacHighCheckbox.Checked == false & flacLowCheckbox.Checked == false & mp3Checkbox.Checked == false)
                {
                    downloadButton.Enabled = false;
                }
            }
        }

        private void flacLowCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.quality2 = flacLowCheckbox.Checked;
            Settings.Default.Save();

            if (flacLowCheckbox.Checked == true)
            {
                formatIdString = "6";
                audioFileType = ".flac";
                Settings.Default.qualityFormat = formatIdString;
                Settings.Default.audioType = audioFileType;
                downloadButton.Enabled = true;
                flacHighCheckbox.Checked = false;
                flacMidCheckbox.Checked = false;
                mp3Checkbox.Checked = false;
            }
            else
            {
                if (flacHighCheckbox.Checked == false & flacMidCheckbox.Checked == false & mp3Checkbox.Checked == false)
                {
                    downloadButton.Enabled = false;
                }
            }
        }

        private void mp3Checkbox_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.quality1 = mp3Checkbox.Checked;
            Settings.Default.Save();

            if (mp3Checkbox.Checked == true)
            {
                formatIdString = "5";
                audioFileType = ".mp3";
                Settings.Default.qualityFormat = formatIdString;
                Settings.Default.audioType = audioFileType;
                downloadButton.Enabled = true;
                flacHighCheckbox.Checked = false;
                flacMidCheckbox.Checked = false;
                flacLowCheckbox.Checked = false;
            }
            else
            {
                if (flacHighCheckbox.Checked == false & flacMidCheckbox.Checked == false & flacLowCheckbox.Checked == false)
                {
                    downloadButton.Enabled = false;
                }
            }
        }
        #endregion

        #region Form moving, closing, minimizing, etc.
        private void exitLabel_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void minimizeLabel_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void minimizeLabel_MouseHover(object sender, EventArgs e)
        {
            minimizeLabel.ForeColor = Color.FromArgb(0, 112, 239);
        }

        private void minimizeLabel_MouseLeave(object sender, EventArgs e)
        {
            minimizeLabel.ForeColor = Color.White;
        }

        private void aboutLabel_Click(object sender, EventArgs e)
        {
            about.Show();
        }

        private void aboutLabel_MouseHover(object sender, EventArgs e)
        {
            aboutLabel.ForeColor = Color.FromArgb(0, 112, 239);
        }

        private void aboutLabel_MouseLeave(object sender, EventArgs e)
        {
            aboutLabel.ForeColor = Color.White;
        }

        private void exitLabel_MouseHover(object sender, EventArgs e)
        {
            exitLabel.ForeColor = Color.FromArgb(0, 112, 239);
        }

        private void exitLabel_MouseLeave(object sender, EventArgs e)
        {
            exitLabel.ForeColor = Color.White;
        }

        private void QobuzDownloaderX_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void QobuzDownloaderX_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
        #endregion

        private void logoBox_Click(object sender, EventArgs e)
        {
            devClickEggThingValue = devClickEggThingValue + 1;

            if (devClickEggThingValue >= 3)
            {
                streamableCheckbox.Visible = true;
                displaySecretButton.Visible = true;
                secretTextbox.Visible = true;
                hiddenTextPanel.Visible = true;
            }
            else
            {
                streamableCheckbox.Visible = false;
                displaySecretButton.Visible = false;
                secretTextbox.Visible = false;
                hiddenTextPanel.Visible = false;
            }
        }

        private void displaySecretButton_Click(object sender, EventArgs e)
        {
            secretTextbox.Text = appSecret;
        }

        private void logoutLabel_MouseHover(object sender, EventArgs e)
        {
            logoutLabel.ForeColor = Color.FromArgb(0, 112, 239);
        }

        private void logoutLabel_MouseLeave(object sender, EventArgs e)
        {
            logoutLabel.ForeColor = Color.FromArgb(88, 92, 102);
        }

        private void logoutLabel_Click(object sender, EventArgs e)
        {
            // Could use some work, but this works.
            Process.Start("QobuzDownloaderX.exe");
            Application.Exit();
        }

        private void artSizeSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Set artSize to selected value, and save selected option to settings.
            artSize = artSizeSelect.Text;
            Settings.Default.savedArtSize = artSizeSelect.SelectedIndex;
            Settings.Default.Save();
        }

        // For converting illegal filename characters to an underscore.
        public string GetSafeFilename(string filename)
        {
            return string.Join("_", filename.Split(Path.GetInvalidFileNameChars()));
        }
    }
}
