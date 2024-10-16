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
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Drawing.Imaging;
using QopenAPI;
using QobuzDownloaderX;
using Newtonsoft.Json.Linq;

namespace QobuzDownloaderX
{
    public partial class LoginForm : Form
    {
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
        (
            int nLeftRect,     // x-coordinate of upper-left corner
            int nTopRect,      // y-coordinate of upper-left corner
            int nRightRect,    // x-coordinate of lower-right corner
            int nBottomRect,   // y-coordinate of lower-right corner
            int nWidthEllipse, // width of ellipse
            int nHeightEllipse // height of ellipse
        );

        private void QobuzDownloaderX_FormClosing(Object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        qbdlxForm qbdlx = new qbdlxForm();
        AboutForm about = new AboutForm();
        Service QoService = new Service();
        User QoUser;
        AppID QoAppID;
        AppSecret QoAppSecret;

        public string username { get; set; }
        public string password { get; set; }
        public string user_auth_token { get; set; }
        public string app_id { get; set; }
        public string app_secret { get; set; }

        public string user_id { get; set; }
        public string user_display_name { get; set; }
        public string user_label { get; set; }

        public LoginForm()
        {
            InitializeComponent();
        }

        string errorLog = Path.GetDirectoryName(Application.ExecutablePath) + "\\Latest_Error.log";
        string dllCheck = Path.GetDirectoryName(Application.ExecutablePath) + "\\taglib-sharp.dll";

        private async void LoginForm_Load(object sender, EventArgs e)
        {
            // Round corners of form
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));

            // Get and display version number.
            versionNumber.Text = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            aboutTextbox.Text = aboutTextbox.Text.Replace("%version%", Assembly.GetExecutingAssembly().GetName().Version.ToString());

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
            username = Settings.Default.savedEmail.ToString();
            password = Settings.Default.savedPassword.ToString();
            emailTextbox.Text = username;
            passwordTextbox.Text = password;
            appidTextbox.Text = Settings.Default.savedAppID.ToString();
            appSecretTextbox.Text = Settings.Default.savedSecret.ToString();

            string emailPlaceholder = "e-mail";
            string passwordPlaceholder = "password";
            //resetPasswordLabel.Visible = true;

            if (Settings.Default.savedAltLoginValue == true)
            {
                //resetPasswordLabel.Visible = false;
                emailPlaceholder = "id";
                passwordPlaceholder = "token";

                emailIcon.Visible = false;
                emailPanel.Visible = false;
                emailTextbox.Visible = false;

                altLoginLabel.Text = "LOGIN WITH E-MAIL AND PASSWORD";
                altLoginLabel.Location = new Point(48, 306);
            }

            // Set values for email textbox.
            if (emailTextbox.Text != "e-mail" | emailTextbox.Text != "id")
            {
                emailTextbox.ForeColor = Color.FromArgb(186, 186, 186);
            }
            if (emailTextbox.Text == null | emailTextbox.Text == "" | emailTextbox.Text == "\r\n")
            {
                emailTextbox.ForeColor = Color.FromArgb(88, 92, 102);
                emailTextbox.Text = emailPlaceholder;
            }

            // Set values for password textbox.
            if (passwordTextbox.Text != "password" | passwordTextbox.Text != "token")
            {
                passwordTextbox.PasswordChar = '*';
                passwordTextbox.ForeColor = Color.FromArgb(186, 186, 186);
            }
            if (passwordTextbox.Text == null | passwordTextbox.Text == "" | passwordTextbox.Text == "\r\n")
            {
                passwordTextbox.PasswordChar = '\0';
                passwordTextbox.ForeColor = Color.FromArgb(88, 92, 102);
                passwordTextbox.Text = passwordPlaceholder;
            }

            try
            {
                // Create HttpClient to grab version number from Github
                var versionURLClient = new HttpClient();
                // Run through TLS to allow secure connection.
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                // Set user-agent to Firefox.
                versionURLClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:67.0) Gecko/20100101 Firefox/67.0");

                // Grab response from Github to get Track IDs from Album response.
                var versionURL = "https://api.github.com/repos/ImAiiR/QobuzDownloaderX/releases/latest";
                var versionURLResponse = await versionURLClient.GetAsync(versionURL);
                string versionURLResponseString = versionURLResponse.Content.ReadAsStringAsync().Result;

                // Grab metadata from API JSON response
                JObject joVersionResponse = JObject.Parse(versionURLResponseString);

                // Grab latest version number
                string version = (string)joVersionResponse["tag_name"];
                // Grab changelog
                string changes = (string)joVersionResponse["body"];

                string currentVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
                string newVersion = version;

                if (currentVersion.Contains(newVersion))
                {
                    // Do nothing. All is good.
                }
                else
                {
                    DialogResult dialogResult = MessageBox.Show("New version of QBDLX is available!\r\n\r\nInstalled version - " + currentVersion + "\r\nLatest version - " + newVersion + "\r\n\r\nChangelog Below\r\n==============\r\n" + changes.Replace("\\r\\n", "\r\n") + "\r\n==============\r\n\r\nWould you like to update?", "QBDLX | Update Available", MessageBoxButtons.YesNo);
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
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void emailTextbox_Click(object sender, EventArgs e)
        {
            if (emailTextbox.Text == "e-mail" | emailTextbox.Text == "id")
            {
                emailTextbox.Text = null;
                emailTextbox.ForeColor = Color.FromArgb(186, 186, 186);
            }
        }

        private void emailTextbox_Leave(object sender, EventArgs e)
        {
            if (emailTextbox.Text == null | emailTextbox.Text == "")
            {
                emailTextbox.ForeColor = Color.FromArgb(50, 50, 50);
                if (Settings.Default.savedAltLoginValue.ToString() == "0")
                {
                    emailTextbox.Text = "e-mail";
                }
                else
                {
                    emailTextbox.Text = "id";
                }
            }
        }

        private void passwordTextbox_Click(object sender, EventArgs e)
        {
            if (passwordTextbox.Text == "password" | passwordTextbox.Text == "token")
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
                passwordTextbox.PasswordChar = '\0';
                passwordTextbox.ForeColor = Color.FromArgb(50, 50, 50);
                if (Settings.Default.savedAltLoginValue.ToString() == "0")
                {
                    passwordTextbox.Text = "password";
                }
                else
                {
                    passwordTextbox.Text = "token";
                }
                
            }
        }

        private void visableCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (visableCheckbox.Checked == true)
            {
                passwordTextbox.PasswordChar = '\0';
            }
            else
            {
                passwordTextbox.PasswordChar = '*';
            }
        }

        private void emailTextbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                loginButton_Click(this, new EventArgs());
                e.SuppressKeyPress = true;
            }
        }

        private void passwordTextbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                loginButton_Click(this, new EventArgs());
                e.SuppressKeyPress = true;
            }
        }

        private void loginButton_Click(object sender, EventArgs e)
        {
            #region Check if textboxes are valid
            if (emailTextbox.Text == "e-mail" | emailTextbox.Text == null | emailTextbox.Text == "id" | emailTextbox.Text == "")
            {
                // If there's no email typed in.
                loginText.Invoke(new Action(() => loginText.Text = "no e-mail or id, please input email first"));
                return;
            }

            if (passwordTextbox.Text == "password" | passwordTextbox.Text == "token")
            {
                // If there's no password typed in.
                loginText.Invoke(new Action(() => loginText.Text = "no password or token typed, please input password first"));
                return;
            }
            #endregion

            username = emailTextbox.Text;
            password = passwordTextbox.Text;

            // Save info locally to be used on next launch.
            Settings.Default.savedEmail = username;
            Settings.Default.savedPassword = password;
            Settings.Default.Save();

            loginBackground.RunWorkerAsync();
        }

        private void loginBackground_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                loginText.Invoke(new Action(() => loginText.Text = "logging in..."));
                loginButton.Invoke(new Action(() => loginButton.Enabled = false));

                if (appidTextbox.Text == null | appidTextbox.Text == "" | appSecretTextbox.Text == null | appSecretTextbox.Text == "")
                {
                    // Grab app_id & login
                    app_id = QoService.GetAppID().App_ID;

                    if (Settings.Default.savedAltLoginValue == false)
                    {
                        QoUser = QoService.Login(app_id, username, password, null);
                    }
                    else
                    {
                        QoUser = QoService.Login(app_id, null, null, password);
                    }

                    user_auth_token = QoUser.UserAuthToken;
                    user_id = QoUser.UserInfo.Id.ToString();
                    user_display_name = QoUser.UserInfo.DisplayName;


                    // Grab user details & send to QBDLX
                    qbdlx.user_id = user_id;
                    qbdlx.user_display_name = user_display_name;
                    try { qbdlx.user_label = QoUser.UserInfo.Credential.Parameters.ShortLabel; } catch { }

                    // Grab profile image
                    try { qbdlx.user_avatar = QoUser.UserInfo.Avatar.Replace(@"\", null).Replace("s=50", "s=20"); } catch { }

                    // Set app_secret
                    app_secret = QoService.GetAppSecret(app_id, user_auth_token).App_Secret;

                    // Re-enable login button, and send app_id & app_secret to QBDLX
                    loginButton.Invoke(new Action(() => loginButton.Enabled = true));
                    qbdlx.app_id = app_id;
                    qbdlx.app_secret = app_secret;
                    qbdlx.user_auth_token = user_auth_token;
                    qbdlx.user_display_name = user_display_name;
                    qbdlx.user_id = user_id;
                    qbdlx.QoUser = QoUser;

                    // Save App ID and Secret to use later on
                    loginText.Invoke(new Action(() => appidTextbox.Text = app_id));
                    loginText.Invoke(new Action(() => appSecretTextbox.Text = app_secret));
                    Settings.Default.savedAppID = app_id;
                    Settings.Default.savedSecret = app_secret;
                }
                else
                {
                    // Use user-provided app_id & login
                    app_id = appidTextbox.Text;

                    if (Settings.Default.savedAltLoginValue == false)
                    {
                        QoUser = QoService.Login(app_id, username, password, null);
                    }
                    else
                    {
                        QoUser = QoService.Login(app_id, null, null, password);
                    }

                    user_auth_token = QoUser.UserAuthToken;
                    user_id = QoUser.UserInfo.Id.ToString();
                    user_display_name = QoUser.UserInfo.DisplayName;


                    // Grab user details & send to QBDLX
                    qbdlx.user_id = user_id;
                    qbdlx.user_display_name = user_display_name;
                    try { qbdlx.user_label = QoUser.UserInfo.Credential.Parameters.ShortLabel; } catch { }

                    // Grab profile image
                    try { qbdlx.user_avatar = QoUser.UserInfo.Avatar.Replace(@"\", null).Replace("s=50", "s=20"); } catch { }

                    // Set user-provided app_secret
                    app_secret = appSecretTextbox.Text;

                    // Re-enable login button, and send app_id & app_secret to QBDLX
                    loginButton.Invoke(new Action(() => loginButton.Enabled = true));
                    qbdlx.app_id = app_id;
                    qbdlx.app_secret = app_secret;
                    qbdlx.user_auth_token = user_auth_token;
                    qbdlx.user_display_name = user_display_name;
                    qbdlx.user_id = user_id;
                    qbdlx.QoUser = QoUser;

                    // Save App ID and Secret to use later on
                    loginText.Invoke(new Action(() => appidTextbox.Text = app_id));
                    loginText.Invoke(new Action(() => appSecretTextbox.Text = app_secret));
                    Settings.Default.savedAppID = app_id;
                    Settings.Default.savedSecret = app_secret;
                }

                // Hide this window & open QBDLX
                this.Invoke(new Action(() => this.Hide()));
                Application.Run(qbdlx);
            }
            catch (Exception loginException)
            {
                // If obtaining bundle.js info fails, show error info.
                string loginError = loginException.ToString();
                loginText.Invoke(new Action(() => loginText.Text = "login failed, error log saved"));
                System.IO.File.WriteAllText(errorLog, loginError);
                loginButton.Invoke(new Action(() => loginButton.Enabled = true));
                return;
            }
        }

        private void cusotmLabel_Click(object sender, EventArgs e)
        {
            customPanel.Location = new Point(12, 82);
            customPanel.Enabled = true;
            customPanel.Visible = true;
        }

        private void resetBackground_DoWork(object sender, DoWorkEventArgs e)
        {
            // CURRENTLY NOT WORKING
            #region Check if e-mail textbox is valid
            if (emailTextbox.Text == "e-mail" | emailTextbox.Text == null | emailTextbox.Text == "")
            {
                // If there's no email typed in.
                loginText.Invoke(new Action(() => loginText.Text = "no e-mail, please input email first"));
                return;
            }
            #endregion

            username = emailTextbox.Text;

            if (username.Contains("@") == false)
            {
                loginText.Invoke(new Action(() => loginText.Text = "No e-mail in e-mail box"));
                return;
            }

            // Grab app_id
            app_id = QoService.GetAppID().App_ID;
            loginText.Invoke(new Action(() => loginText.Text = "sending reset request..."));
            System.Threading.Thread.Sleep(500);
            try
            {
                // Send reset request through API
                QoUser = QoService.ResetPassword(app_id, username);
                if (QoUser.Status == "success")
                {
                    loginText.Invoke(new Action(() => loginText.Text = "request sent, check your e-mail"));
                    return;
                }
                else
                {
                    loginText.Invoke(new Action(() => loginText.Text = "request failed, try again"));
                    return;
                }
            }
            catch
            {
                loginText.Invoke(new Action(() => loginText.Text = "sending failed, try again"));
                return;
            }
        }

        private void altLoginLabel_Click(object sender, EventArgs e)
        {
            if (altLoginLabel.Text.Contains("TOKEN"))
            {
                Settings.Default.savedAltLoginValue = true;
                //resetPasswordLabel.Visible = false;
                altLoginLabel.Text = "LOGIN WITH E-MAIL AND PASSWORD";
                altLoginLabel.Location = new Point(48, 306);

                emailIcon.Visible = false;
                emailPanel.Visible = false;
                emailTextbox.Visible = false;

                if (passwordTextbox.Text == "password")
                {
                    passwordTextbox.Text = "token";
                }
            }
            else
            {
                Settings.Default.savedAltLoginValue = false;
                //resetPasswordLabel.Visible = true;
                altLoginLabel.Text = "LOGIN WITH TOKEN";
                altLoginLabel.Location = new Point(93, 306);

                emailIcon.Visible = true;
                emailPanel.Visible = true;
                emailTextbox.Visible = true;

                if (passwordTextbox.Text == "token")
                {
                    passwordTextbox.Text = "password";
                }
            }
            
            // Save info locally to be used on next launch.
            Settings.Default.savedEmail = username;
            Settings.Default.savedPassword = password;
            Settings.Default.Save();
        }

        private void aboutButton_Click(object sender, EventArgs e)
        {
            aboutPanel.Location = new Point(12, 82);
            aboutPanel.Enabled = true;
            aboutPanel.Visible = true;
        }

        private void closeAboutButton_Click(object sender, EventArgs e)
        {
            aboutPanel.Enabled = false;
            aboutPanel.Visible = false;
        }

        private void altLoginLabel_MouseEnter(object sender, EventArgs e)
        {
            altLoginLabel.ForeColor = Color.FromArgb(140, 140, 140);
        }

        private void altLoginLabel_MouseLeave(object sender, EventArgs e)
        {
            altLoginLabel.ForeColor = Color.FromArgb(100, 100, 100);
        }

        private void cusotmLabel_MouseEnter(object sender, EventArgs e)
        {
            customLabel.ForeColor = Color.FromArgb(140, 140, 140);
        }

        private void cusotmLabel_MouseLeave(object sender, EventArgs e)
        {
            customLabel.ForeColor = Color.FromArgb(100, 100, 100);
        }

        private void customSaveButton_Click(object sender, EventArgs e)
        {
            Settings.Default.savedAppID = appidTextbox.Text;
            Settings.Default.savedSecret = appSecretTextbox.Text;
            customPanel.Enabled = false;
            customPanel.Visible = false;
        }
    }
}
