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
using TagLib.Flac;
using System.Globalization;
using System.Threading;

namespace QobuzDownloaderX
{
    public partial class searchForm : Form
    {
        public searchForm()
        {
            InitializeComponent();
        }

        public String appid { get; set; }
        public String userAuth { get; set; }

        static string DecodeEncodedNonAsciiCharacters(string value)
        {
            return Regex.Replace(
                value,
                @"\\u(?<Value>[a-zA-Z0-9]{4})",
                m => {
                    return ((char)int.Parse(m.Groups["Value"].Value, NumberStyles.HexNumber)).ToString();
                });
        }

        private void searchForm_Load(object sender, EventArgs e)
        {

        }

        private void searchButton_Click(object sender, EventArgs e)
        {
            #region Do Search
            WebRequest searchwr = WebRequest.Create("http://www.qobuz.com/api.json/0.2/album/search?app_id=" + appid + "&query=" + searchInput.Text + "&limit=15&user_auth_token=" + userAuth);

            WebResponse searchws = searchwr.GetResponse();
            StreamReader searchsr = new StreamReader(searchws.GetResponseStream());

            string searchRequest = searchsr.ReadToEnd();

            // Remove backslashes from the stream URL to have a proper URL.
            string resultpattern = "\"maximum_bit_depth\":(?<bitDepth>.*?),(?:.*?),\"artist\":(?:.*?)\"name\":\"(?<albumArtist>.*?)\",(?:.*?)\"title\":\"(?<albumTitle>.*?)\"(?:.*?),\"maximum_channel_count\":(?:.*?),\"id\":\"(?<albumID>.*?)\",\"maximum_sampling_rate\":(?<sampleRate>.*?),\"";
            string resultinput = searchRequest;
            RegexOptions resultoptions = RegexOptions.Multiline;

            resultTextbox.Invoke(new Action(() => resultTextbox.Text = String.Empty));

            foreach (Match mResult in Regex.Matches(resultinput, resultpattern, resultoptions))
            {
                resultTextbox.Invoke(new Action(() => resultTextbox.AppendText(string.Format("{0} - {1} [{2}bit/{3}kHz]\r\nhttps://play.qobuz.com/album/{4}\r\n\r\n", mResult.Groups["albumArtist"].Value, mResult.Groups["albumTitle"].Value, mResult.Groups["bitDepth"].Value, mResult.Groups["sampleRate"].Value, mResult.Groups["albumID"].Value))));

                // For converting unicode characters to ASCII
                string unicodeResult = resultTextbox.Text;
                string decodedResult = DecodeEncodedNonAsciiCharacters(unicodeResult);

                var fixedText = decodedResult.Replace(@"\/", "/");
                resultTextbox.Invoke(new Action(() => resultTextbox.Text = fixedText));
            }

            #endregion
        }

        private void searchForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            
        }

        private void searchInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                searchButton_Click(sender, e);
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }
    }
}
