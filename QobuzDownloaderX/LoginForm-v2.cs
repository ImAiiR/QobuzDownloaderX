using QobuzDownloaderX.Properties;
using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Drawing.Imaging;
using TagLib.Flac;
using QobuzDownloaderX;

namespace QobuzDownloaderX
{
    public partial class LoginFrm : Form
    {
        public LoginFrm()
        {
            InitializeComponent();
        }

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        private void QobuzDownloaderX_FormClosing(Object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        QobuzDownloaderX qbdlx = new QobuzDownloaderX();

        public string appSecret { get; set; }

        string errorLog = Path.GetDirectoryName(Application.ExecutablePath) + "\\Latest_Error.log";
        string dllCheck = Path.GetDirectoryName(Application.ExecutablePath) + "\\taglib-sharp.dll";

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

        private void LoginFrm_Load(object sender, EventArgs e)
        {
            try
            {
                WebClient versionURLClient = new WebClient();
                // Run through TLS to allow secure connection.
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                string versionHTML = versionURLClient.DownloadString("https://github.com/ImAiiR/QobuzDownloaderX/releases");

                // Grab link to bundle.js
                var versionLog = Regex.Match(versionHTML, "<span class=\"css-truncate-target\" style=\"max-width: 125px\">(?<latestVersion>.*?)<\\/span>").Groups;
                var version = versionLog[1].Value;

                string currentVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
                string newVersion = version;

                if (currentVersion.Contains(newVersion))
                {
                    // Do nothing. All is good.
                }
                else
                {
                    DialogResult dialogResult = MessageBox.Show("New version of QBDLX is available!\r\n\r\nInstalled version - " + currentVersion + "\r\nLatest version - "+ newVersion + "\r\n\r\nWould you like to update?", "QBDLX | Update Available", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        // If "Yes" is clicked, open GitHub page and close QBDLX.
                        Process.Start("https://github.com/ImAiiR/QobuzDownloaderX/releases/latest");
                        Application.Exit();
                    }
                    else if (dialogResult == DialogResult.No)
                    {
                        // Ignore the update until next open.
                    }
                }
            }
            catch
            {
                DialogResult dialogResult = MessageBox.Show("Connection to GitHub to check for an update has failed.\r\nWould you like to check for an update manually?\r\n\r\nYour current version is " + Assembly.GetExecutingAssembly().GetName().Version.ToString(), "QBDLX | GitHub Connection Failed", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    // If "Yes" is clicked, open GitHub page and close QBDLX.
                    Process.Start("https://github.com/ImAiiR/QobuzDownloaderX/releases/latest");
                    Application.Exit();
                }
                else if (dialogResult == DialogResult.No)
                {
                    // Ignore the update until next open.
                }
            }

            // Get and display version number.
            verNumLabel2.Text = Assembly.GetExecutingAssembly().GetName().Version.ToString();

            // Check for taglib-sharp.dll
            if (!System.IO.File.Exists(dllCheck))
            {
                MessageBox.Show("taglib-sharp.dll missing from folder!\r\nPlease Make sure the DLL is in the same folder as QobuzDownloaderX.exe!", "ERROR",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }

            // Bring to center of screen.
            CenterToScreen();

            if (Properties.Settings.Default.UpgradeRequired)
            {
                Properties.Settings.Default.Upgrade();
                Properties.Settings.Default.UpgradeRequired = false;
                Properties.Settings.Default.Save();
            }

            // Set saved settings to correct places.
            appidTextbox.Text = Settings.Default.savedAppID.ToString();
            emailTextbox.Text = Settings.Default.savedEmail.ToString();
            passwordTextbox.Text = Settings.Default.savedPassword.ToString();

            if (appidTextbox.Text != "app_id")
            {
                appidTextbox.ForeColor = Color.FromArgb(186, 186, 186);
            }

            if (appidTextbox.Text == null | appidTextbox.Text == "")
            {
                appidTextbox.ForeColor = Color.FromArgb(88, 92, 102);
                appidTextbox.Text = "app_id";
            }

            if (emailTextbox.Text != "Email")
            {
                emailTextbox.ForeColor = Color.FromArgb(186, 186, 186);
            }

            if (emailTextbox.Text == null | emailTextbox.Text == "")
            {
                emailTextbox.ForeColor = Color.FromArgb(88, 92, 102);
                emailTextbox.Text = "Email";
            }

            if (passwordTextbox.Text != "Password")
            {
                passwordTextbox.PasswordChar = '*';
                passwordTextbox.UseSystemPasswordChar = false;
                passwordTextbox.ForeColor = Color.FromArgb(186, 186, 186);
            }

            if (passwordTextbox.Text == null | passwordTextbox.Text == "")
            {
                passwordTextbox.ForeColor = Color.FromArgb(88, 92, 102);
                passwordTextbox.UseSystemPasswordChar = true;
                passwordTextbox.Text = "Password";
            }
        }

        private void loginButton_Click(object sender, EventArgs e)
        {
            if (appidTextbox.Text == "app_id" | appidTextbox.Text == null | appidTextbox.Text == "")
            {
                // If there's no app_id typed in.
                loginText.Invoke(new Action(() => loginText.Text = "No app_id, please input app_id first."));
                return;
            }
            else if (emailTextbox.Text == "Email" | emailTextbox.Text == null | emailTextbox.Text == "")
            {
                // If there's no email typed in.
                loginText.Invoke(new Action(() => loginText.Text = "No email, please input email first."));
                return;
            }

            var passMD5CheckLog = Regex.Match(passwordTextbox.Text, "(?<md5Test>^[0-9a-f]{32}$)").Groups;
            var passMD5Check = passMD5CheckLog[1].Value;

            if (passMD5Check == null | passMD5Check == "")
            {
                loginText.Text = "Password not MD5! Hit \"MD5\" before logging in!";
                return;
            }

            // Save info locally to be used on next launch.
            Settings.Default.savedEmail = emailTextbox.Text;
            Settings.Default.savedAppID = appidTextbox.Text;
            Settings.Default.savedPassword = passwordTextbox.Text;
            Settings.Default.Save();

            loginText.Text = "Logging in + obtaining app_secret...";
            loginButton.Enabled = false;
            loginBG.RunWorkerAsync();
        }

        #region Textbox Focous & Text Change

        #region app_id Textbox
        private void appIdTextbox_Click(object sender, EventArgs e)
        {
            if (appidTextbox.Text == "app_id")
            {
                appidTextbox.Text = null;
                appidTextbox.ForeColor = Color.FromArgb(186, 186, 186);
            }
        }

        private void panel2_Click(object sender, EventArgs e)
        {
            appidTextbox.Focus();

            if (appidTextbox.Text == "app_id")
            {
                appidTextbox.Text = null;
                appidTextbox.ForeColor = Color.FromArgb(186, 186, 186);
            }
        }

        private void appIdTextbox_Leave(object sender, EventArgs e)
        {
            if (appidTextbox.Text == null | appidTextbox.Text == "")
            {
                appidTextbox.ForeColor = Color.FromArgb(88, 92, 102);
                appidTextbox.Text = "app_id";
            }
        }
        #endregion

        #region Email Textbox
        private void emailTextbox_Click(object sender, EventArgs e)
        {
            if (emailTextbox.Text == "Email")
            {
                emailTextbox.Text = null;
                emailTextbox.ForeColor = Color.FromArgb(186, 186, 186);
            }
        }

        private void panel3_Click(object sender, EventArgs e)
        {
            emailTextbox.Focus();

            if (emailTextbox.Text == "Email")
            {
                emailTextbox.Text = null;
                emailTextbox.ForeColor = Color.FromArgb(186, 186, 186);
            }
        }

        private void emailTextbox_Leave(object sender, EventArgs e)
        {
            if (emailTextbox.Text == null | emailTextbox.Text == "")
            {
                emailTextbox.ForeColor = Color.FromArgb(88, 92, 102);
                emailTextbox.Text = "Email";
            }
        }
        #endregion

        #region Password Textbox
        private void passwordTextbox_Click(object sender, EventArgs e)
        {
            if (passwordTextbox.Text == "Password")
            {
                passwordTextbox.Text = null;
                passwordTextbox.PasswordChar = '*';
                passwordTextbox.UseSystemPasswordChar = false;
                passwordTextbox.ForeColor = Color.FromArgb(186, 186, 186);
            }
        }

        private void panel4_Click(object sender, EventArgs e)
        {
            passwordTextbox.Focus();

            if (passwordTextbox.Text == "Password")
            {
                passwordTextbox.Text = null;
                passwordTextbox.PasswordChar = '*';
                passwordTextbox.UseSystemPasswordChar = false;
                passwordTextbox.ForeColor = Color.FromArgb(186, 186, 186);
            }
        }

        private void passwordTextbox_Leave(object sender, EventArgs e)
        {
            if (passwordTextbox.Text == null | passwordTextbox.Text == "")
            {
                passwordTextbox.ForeColor = Color.FromArgb(88, 92, 102);
                passwordTextbox.UseSystemPasswordChar = true;
                passwordTextbox.Text = "Password";
            }
        }

        #endregion

        #endregion

        private void exitLabel_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
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

        private void verNumLabel2_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void md5Button_Click(object sender, EventArgs e)
        {
            if (passwordTextbox.Text == "Password")
            {
                // If there's no password typed in.
                loginText.Invoke(new Action(() => loginText.Text = "No password typed, please input password first."));
                return;
            }

            string plainTextPW = passwordTextbox.Text;

            // Generate the MD5 hash using the string created above.
            using (MD5 md5PassHash = MD5.Create())
            {
                string hashedPW = GetMd5Hash(md5PassHash, plainTextPW);

                if (VerifyMd5Hash(md5PassHash, plainTextPW, hashedPW))
                {
                    // If the MD5 hash is verified, proceed to get the streaming URL.
                    passwordTextbox.Text = hashedPW;
                }
                else
                {
                    // If the hash can't be verified.
                    loginText.Invoke(new Action(() => loginText.Text = "Hashing failed. Please retry."));
                    return;
                }
            }
        }

        private void loginBG_DoWork(object sender, DoWorkEventArgs e)
        {
            loginBG.WorkerSupportsCancellation = true;

            // Create WebRequest to login using login information from input textboxes.
            WebRequest wr = WebRequest.Create("https://www.qobuz.com/api.json/0.2/user/login?email=" + emailTextbox.Text + "&password=" + passwordTextbox.Text + "&app_id=" + appidTextbox.Text);

            try
            {
                // Grab info to be displayed and used.
                WebResponse ws = wr.GetResponse();
                StreamReader sr = new StreamReader(ws.GetResponseStream());

                string loginRequest = sr.ReadToEnd();
                string text = loginRequest;

                // Grab display name
                var displayNameLog = Regex.Match(loginRequest, "\"display_name\":\"(?<displayName>.*?)\",\\\"").Groups;
                var displayName = displayNameLog[1].Value;
                qbdlx.displayName = displayName;

                // Grab account type
                var accountTypeLog = Regex.Match(loginRequest, "short_label\":\"(?<accountType>\\w+)").Groups;
                var accountType = accountTypeLog[1].Value;
                qbdlx.accountType = accountType;

                // Grab authentication token
                var userAuth = Regex.Match(loginRequest, "\"user_auth_token\":\"(?<userAuth>.*?)\\\"}").Groups;
                var userAuthToken = userAuth[1].Value;

                // Set user_auth_token
                qbdlx.userAuth = userAuthToken;
                loginText.Invoke(new Action(() => loginText.Text = "Login Successful! Getting app_secret..."));
            }
            catch (Exception ex)
            {
                // If connection to API fails, show error info.
                string error = ex.ToString();
                loginText.Invoke(new Action(() => loginText.Text = "Login Failed. Error Log saved"));
                System.IO.File.WriteAllText(errorLog, error);
                wr.Abort();
                loginButton.Invoke(new Action(() => loginButton.Enabled = true));
                return;
            }

            wr.Abort();
            getSecretBG.RunWorkerAsync();
            loginBG.CancelAsync();
        }

        private void getSecretBG_DoWork(object sender, DoWorkEventArgs e)
        {
            getSecretBG.WorkerSupportsCancellation = true;

            WebClient bundleURLClient = new WebClient();
            string bundleHTML = bundleURLClient.DownloadString("https://play.qobuz.com/");

            // Grab link to bundle.js
            var bundleLog = Regex.Match(bundleHTML, "<script src=\"(?<bundleJS>\\/resources\\/\\d+\\.\\d+\\.\\d+-[a-z]\\d{3}\\/bundle\\.js)").Groups;
            var bundleSuffix = bundleLog[1].Value;
            var bundleURL = "https://play.qobuz.com" + bundleSuffix;

            WebRequest bundleWR = WebRequest.Create(bundleURL);

            try
            {
                WebResponse bundleWS = bundleWR.GetResponse();
                StreamReader bundleSR = new StreamReader(bundleWS.GetResponseStream());

                string getBundleRequest = bundleSR.ReadToEnd();
                string text = getBundleRequest;

                // Grab "info" and "extras"
                var bundleLog1 = Regex.Match(getBundleRequest, "{offset:\"(?<notUsed>.*?)\",name:\"Europe\\/Berlin\",info:\"(?<info>.*?)\",extras:\"(?<extras>.*?)\"}").Groups;
                var bundleInfo = bundleLog1[2].Value;
                var bundleExtras = bundleLog1[3].Value;

                // Grab "seed"
                var bundleLog2 = Regex.Match(getBundleRequest, "window.utimezone.paris\\):h.initialSeed\\(\"(?<seed>.*?)\",window.utimezone.berlin\\)").Groups;
                var bundleSeed = bundleLog2[1].Value;

                // Step 1 of getting the app_secret
                string B64step1 = bundleSeed + bundleInfo + bundleExtras;
                B64step1 = B64step1.Remove(B64step1.Length - 44, 44);
                byte[] step1Bytes = Encoding.UTF8.GetBytes(B64step1);
                B64step1 = Convert.ToBase64String(step1Bytes);

                // Step 2 of getting the app_secret
                byte[] step2Data = Convert.FromBase64String(B64step1);
                string B64step2 = Encoding.UTF8.GetString(step2Data);

                // Step 3 of getting the app_secret
                byte[] step3Data = Convert.FromBase64String(B64step2);

                // Set app_secret
                appSecret = Encoding.UTF8.GetString(step3Data);
                loginText.Invoke(new Action(() => loginText.Text = "app_secret Obtained! Launching QBDLX..."));
                System.Threading.Thread.Sleep(1000);
            }
            catch (Exception bundleEx)
            {
                // If obtaining bundle.js info fails, show error info.
                string bundleError = bundleEx.ToString();
                loginText.Invoke(new Action(() => loginText.Text = "Couldn't obtain app_secret. Error Log saved"));
                System.IO.File.WriteAllText(errorLog, bundleError);
                bundleWR.Abort();
                loginButton.Invoke(new Action(() => loginButton.Enabled = true));
                return;
            }

            bundleWR.Abort();
            finishLogin(sender, e);
            getSecretBG.CancelAsync();
        }

        private void finishLogin(object sender, EventArgs e)
        {
            loginButton.Invoke(new Action(() => loginButton.Enabled = true));
            // If info is legit, go to the main form.
            qbdlx.appid = appidTextbox.Text;
            qbdlx.eMail = emailTextbox.Text;
            qbdlx.password = passwordTextbox.Text;
            qbdlx.appSecret = appSecret;
            this.Invoke(new Action(() => this.Hide()));
            Application.Run(qbdlx);
        }
    }
}
