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

        private readonly Theming _themeManager = new Theming();
        qbdlxForm qbdlx = new qbdlxForm();
        Service QoService = new Service();
        User QoUser;

        public string currentVersion { get; set; }
        public string newVersion { get; set; }
        public string changes { get; set; }

        // Create logger for this form
        public Logger logger { get; set; }

        public string username { get; set; }
        public string password { get; set; }
        public string user_auth_token { get; set; }
        public string app_id { get; set; }
        public string app_secret { get; set; }

        public string user_id { get; set; }
        public string user_display_name { get; set; }
        public string user_label { get; set; }

        public string latestWebResponse { get; set; }

        public LoginForm()
        {
            InitializeComponent();
        }

        string errorLog = Path.GetDirectoryName(Application.ExecutablePath) + "\\Latest_Error.log";
        string dllCheck = Path.GetDirectoryName(Application.ExecutablePath) + "\\taglib-sharp.dll";

        private async void LoginForm_Load(object sender, EventArgs e)
        {
            // Create new log file
            Directory.CreateDirectory("logs");
            logger = new Logger("logs\\loginForm_log-" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".txt");
            logger.Debug("Logger started, login form loaded!");

            // Round corners of form
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));

            // Get and display version number.
            logger.Info("QobuzDownlaoderX | Version " + Assembly.GetExecutingAssembly().GetName().Version.ToString());
            versionNumber.Text = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            aboutTextbox.Text = aboutTextbox.Text.Replace("%version%", Assembly.GetExecutingAssembly().GetName().Version.ToString());

            if (!System.IO.File.Exists(dllCheck))
            {
                logger.Error("taglib-sharp.dll is missing from folder. Exiting.");
                MessageBox.Show("taglib-sharp.dll missing from folder!\r\nPlease Make sure the DLL is in the same folder as QobuzDownloaderX.exe!", "ERROR",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
            // Bring to center of screen.
            CenterToScreen();

            // Set and load theme
            _themeManager.LoadTheme(Settings.Default.currentTheme);
            _themeManager.ApplyTheme(this);

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

            logger.Info("Currently saved username: " + username);
            logger.Info("Currently saved app ID: " + Settings.Default.savedAppID.ToString());
            logger.Info("Currently saved app secret: " + Settings.Default.savedSecret.ToString());

            string emailPlaceholder = "e-mail";
            string passwordPlaceholder = "password";

            if (Settings.Default.savedAltLoginValue == true)
            {
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
                emailTextbox.ForeColor = ColorTranslator.FromHtml(_themeManager._currentTheme.TextBoxText);
            }
            if (emailTextbox.Text == null | emailTextbox.Text == "" | emailTextbox.Text == "\r\n")
            {
                emailTextbox.ForeColor = ColorTranslator.FromHtml(_themeManager._currentTheme.PlaceholderTextBoxText);
                emailTextbox.Text = emailPlaceholder;
            }

            // Set values for password textbox.
            if (passwordTextbox.Text != "password" | passwordTextbox.Text != "token")
            {
                passwordTextbox.PasswordChar = '*';
                passwordTextbox.ForeColor = ColorTranslator.FromHtml(_themeManager._currentTheme.TextBoxText);
            }
            if (passwordTextbox.Text == null | passwordTextbox.Text == "" | passwordTextbox.Text == "\r\n")
            {
                passwordTextbox.PasswordChar = '\0';
                passwordTextbox.ForeColor = ColorTranslator.FromHtml(_themeManager._currentTheme.PlaceholderTextBoxText);
                passwordTextbox.Text = passwordPlaceholder;
            }

            try
            {
                // Create HttpClient to grab version number from Github
                var versionURLClient = new HttpClient();
                logger.Debug("versionURLClient initialized");
                // Run through TLS to allow secure connection.
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                // Set user-agent to Firefox.
                versionURLClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:67.0) Gecko/20100101 Firefox/67.0");

                // Grab response from Github to get version number.
                logger.Debug("Starting request for latest GitHub version");
                var versionURL = "https://api.github.com/repos/ImAiiR/QobuzDownloaderX/releases/latest";
                var versionURLResponse = await versionURLClient.GetAsync(versionURL);
                string versionURLResponseString = versionURLResponse.Content.ReadAsStringAsync().Result;
                latestWebResponse = versionURLResponseString;

                // Grab metadata from API JSON response
                JObject joVersionResponse = JObject.Parse(versionURLResponseString);

                // Grab latest version number
                string version = (string)joVersionResponse["tag_name"];
                logger.Debug("Recieved version: " + version);
                // Grab changelog
                changes = (string)joVersionResponse["body"];

                currentVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
                newVersion = version;

                if (currentVersion.Contains(newVersion))
                {
                    // Do nothing. All is good.
                    logger.Debug("Current version and new version match!");
                }
                else
                {
                    logger.Warning("Current version and new version do not match!");
                    logger.Debug("Enabling update button");
                    updateButton.Enabled = true;
                    updateButton.Visible = true;
                }
            }
            catch
            {
                logger.Error("Connection to GitHub failed, unable to grab latest version.");
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
            logger.Debug("Exiting.");
            Application.Exit();
        }

        private void emailTextbox_Click(object sender, EventArgs e)
        {
            if (emailTextbox.Text == "e-mail" | emailTextbox.Text == "id")
            {
                emailTextbox.Text = null;
                emailTextbox.ForeColor = ColorTranslator.FromHtml(_themeManager._currentTheme.TextBoxText);
            }
        }

        private void emailTextbox_Leave(object sender, EventArgs e)
        {
            if (emailTextbox.Text == null | emailTextbox.Text == "")
            {
                emailTextbox.ForeColor = ColorTranslator.FromHtml(_themeManager._currentTheme.PlaceholderTextBoxText);
                if (Settings.Default.savedAltLoginValue == false)
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
                passwordTextbox.ForeColor = ColorTranslator.FromHtml(_themeManager._currentTheme.TextBoxText);
            }
        }

        private void passwordTextbox_Leave(object sender, EventArgs e)
        {
            if (passwordTextbox.Text == null | passwordTextbox.Text == "")
            {
                passwordTextbox.PasswordChar = '\0';
                passwordTextbox.ForeColor = ColorTranslator.FromHtml(_themeManager._currentTheme.PlaceholderTextBoxText);
                if (Settings.Default.savedAltLoginValue == false)
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
            logger.Debug("Logging in...");
            #region Check if textboxes are valid
            if (emailTextbox.Text == "e-mail" | emailTextbox.Text == null | emailTextbox.Text == "id" | emailTextbox.Text == "")
            {
                // If there's no email typed in. Ignore if using token to login.
                logger.Warning("emailTextbox does not contain proper values for logging in.");
                if (altLoginLabel.Text.Contains("PASSWORD") == false)
                {
                    loginText.Invoke(new Action(() => loginText.Text = "no e-mail or id, please input email first"));
                    return;
                }
            }

            if (passwordTextbox.Text == "password" | passwordTextbox.Text == "token")
            {
                // If there's no password typed in.
                logger.Warning("passwordTextbox does not contain proper values for logging in.");
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

            logger.Debug("Starting loginBackground...");
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
                    logger.Debug("No saved/custom app ID given, will get a new ID from Qobuz");
                    // Grab app_id & login
                    app_id = QoService.GetAppID().App_ID;
                    logger.Info("App ID: " + app_id);

                    if (Settings.Default.savedAltLoginValue == false)
                    {
                        logger.Debug("Logging in with e-mail and password");
                        QoUser = QoService.Login(app_id, username, password, null);
                    }
                    else
                    {
                        logger.Debug("Logging in with token");
                        QoUser = QoService.Login(app_id, null, null, password);
                    }

                    user_auth_token = QoUser.UserAuthToken;
                    user_id = QoUser.UserInfo.Id.ToString();
                    user_display_name = QoUser.UserInfo.DisplayName;

                    logger.Info("User ID: " + user_id);
                    logger.Info("User display name: " + user_display_name);


                    // Grab user details & send to QBDLX
                    logger.Debug("Sending values to main form");
                    qbdlx.user_id = user_id;
                    qbdlx.user_display_name = user_display_name;
                    try { qbdlx.user_label = QoUser.UserInfo.Credential.Parameters.ShortLabel; } catch { logger.Warning("Attempt to grab user's short label from API has failed. Continuing."); }

                    // Grab profile image
                    try { qbdlx.user_avatar = QoUser.UserInfo.Avatar.Replace(@"\", null).Replace("s=50", "s=20"); } catch { logger.Warning("Attempt to grab user's avatar from API has failed. Continuing."); }

                    // Set app_secret
                    app_secret = QoService.GetAppSecret(app_id, user_auth_token).App_Secret;
                    logger.Info("App secret: " + app_secret);

                    // Re-enable login button, and send app_id & app_secret to QBDLX
                    loginButton.Invoke(new Action(() => loginButton.Enabled = true));
                    qbdlx.app_id = app_id;
                    qbdlx.app_secret = app_secret;
                    qbdlx.user_auth_token = user_auth_token;
                    qbdlx.QoUser = QoUser;

                    // Save App ID and Secret to use later on
                    loginText.Invoke(new Action(() => appidTextbox.Text = app_id));
                    loginText.Invoke(new Action(() => appSecretTextbox.Text = app_secret));
                    Settings.Default.savedAppID = app_id;
                    Settings.Default.savedSecret = app_secret;
                }
                else
                {
                    logger.Debug("Using saved/custom app ID and secret");
                    // Use user-provided app_id & login
                    app_id = appidTextbox.Text;
                    logger.Info("App ID: " + app_id);

                    if (Settings.Default.savedAltLoginValue == false)
                    {
                        logger.Debug("Logging in with e-mail and password");
                        QoUser = QoService.Login(app_id, username, password, null);
                    }
                    else
                    {
                        logger.Debug("Logging in with token");
                        QoUser = QoService.Login(app_id, null, null, password);
                    }

                    user_auth_token = QoUser.UserAuthToken;
                    user_id = QoUser.UserInfo.Id.ToString();
                    user_display_name = QoUser.UserInfo.DisplayName;

                    logger.Info("User ID: " + user_id);
                    logger.Info("User display name: " + user_display_name);


                    // Grab user details & send to QBDLX
                    logger.Debug("Sending values to main form");
                    qbdlx.user_id = user_id;
                    qbdlx.user_display_name = user_display_name;
                    try { qbdlx.user_label = QoUser.UserInfo.Credential.Parameters.ShortLabel; } catch { logger.Warning("Attempt to grab user's short label from API has failed. Continuing."); }

                    // Grab profile image
                    try { qbdlx.user_avatar = QoUser.UserInfo.Avatar.Replace(@"\", null).Replace("s=50", "s=20"); } catch { logger.Warning("Attempt to grab user's avatar from API has failed. Continuing."); }

                    // Set user-provided app_secret
                    app_secret = appSecretTextbox.Text;
                    logger.Info("App secret: " + app_secret);

                    // Re-enable login button, and send app_id & app_secret to QBDLX
                    loginButton.Invoke(new Action(() => loginButton.Enabled = true));
                    qbdlx.app_id = app_id;
                    qbdlx.app_secret = app_secret;
                    qbdlx.user_auth_token = user_auth_token;
                    qbdlx.QoUser = QoUser;

                    // Save App ID and Secret to use later on
                    loginText.Invoke(new Action(() => appidTextbox.Text = app_id));
                    loginText.Invoke(new Action(() => appSecretTextbox.Text = app_secret));
                    Settings.Default.savedAppID = app_id;
                    Settings.Default.savedSecret = app_secret;
                }

                // Hide this window & open QBDLX
                logger.Debug("Login successful! Hiding this form, and launching main form.");
                this.Invoke(new Action(() => this.Hide()));
                Application.Run(qbdlx);
            }
            catch (Exception loginException)
            {
                // If obtaining bundle.js info fails, show error info.
                string loginError = loginException.ToString();
                logger.Error("Login failed, error listed below.");
                logger.Error("Error:\r\n" + loginException);
                loginText.Invoke(new Action(() => loginText.Text = "login failed, error log saved"));
                System.IO.File.WriteAllText(errorLog, loginError);
                loginButton.Invoke(new Action(() => loginButton.Enabled = true));
                return;
            }
        }

        private void cusotmLabel_Click(object sender, EventArgs e)
        {
            logger.Debug("Opening custom app ID and secret panel");
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
                logger.Debug("Swapping login method to token");
                Settings.Default.savedAltLoginValue = true;
                altLoginLabel.Text = "LOGIN WITH E-MAIL AND PASSWORD";
                altLoginLabel.Location = new Point(48, 306);

                emailIcon.Visible = false;
                emailPanel.Visible = false;
                emailTextbox.Visible = false;

                passwordTextbox.Text = null;
                passwordTextbox_Leave(this, new EventArgs());
            }
            else
            {
                logger.Debug("Swapping login method to e-mail and password");
                Settings.Default.savedAltLoginValue = false;
                altLoginLabel.Text = "LOGIN WITH TOKEN";
                altLoginLabel.Location = new Point(93, 306);

                emailIcon.Visible = true;
                emailPanel.Visible = true;
                emailTextbox.Visible = true;

                passwordTextbox.Text = null;
                passwordTextbox_Leave(this, new EventArgs());
                emailTextbox.Text = null;
                emailTextbox_Leave(this, new EventArgs());
            }
            
            // Save choice locally to be used on next launch.
            Settings.Default.Save();
        }

        private void aboutButton_Click(object sender, EventArgs e)
        {
            logger.Debug("Opening about panel");
            loginAboutPanel.Location = new Point(12, 82);
            loginAboutPanel.Enabled = true;
            loginAboutPanel.Visible = true;
        }

        private void closeAboutButton_Click(object sender, EventArgs e)
        {
            logger.Debug("Hiding about panel");
            loginAboutPanel.Enabled = false;
            loginAboutPanel.Visible = false;
        }

        private void customSaveButton_Click(object sender, EventArgs e)
        {
            logger.Debug("Saving custom app ID and secret...");
            Settings.Default.savedAppID = appidTextbox.Text;
            Settings.Default.savedSecret = appSecretTextbox.Text;
            logger.Debug("Custom app ID and secret saved! Hiding custom values panel");
            customPanel.Enabled = false;
            customPanel.Visible = false;
        }

        private void altLoginLabel_MouseEnter(object sender, EventArgs e)
        {
            altLoginLabel.ForeColor = ColorTranslator.FromHtml(_themeManager._currentTheme.PlaceholderTextBoxText);
        }

        private void altLoginLabel_MouseLeave(object sender, EventArgs e)
        {
            altLoginLabel.ForeColor = ColorTranslator.FromHtml(_themeManager._currentTheme.LabelText);
        }

        private void cusotmLabel_MouseEnter(object sender, EventArgs e)
        {
            customLabel.ForeColor = ColorTranslator.FromHtml(_themeManager._currentTheme.PlaceholderTextBoxText);
        }

        private void cusotmLabel_MouseLeave(object sender, EventArgs e)
        {
            customLabel.ForeColor = ColorTranslator.FromHtml(_themeManager._currentTheme.LabelText);
        }

        private void updateButton_Click(object sender, EventArgs e)
        {
            logger.Debug("Opening update information dialog");
            DialogResult dialogResult = MessageBox.Show("New version of QBDLX is available!\r\n\r\nInstalled version - " + currentVersion + "\r\nLatest version - " + newVersion + "\r\n\r\nChangelog Below\r\n==============\r\n" + changes.Replace("\\r\\n", "\r\n") + "\r\n==============\r\n\r\nWould you like to update?", "QBDLX | Update Available", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                // If "Yes" is clicked, open GitHub page and close QBDLX.
                logger.Debug("Opening GitHub page for latest update");
                Process.Start("https://github.com/ImAiiR/QobuzDownloaderX/releases/latest");
                logger.Debug("Exiting");
                Application.Exit();
            }
            else if (dialogResult == DialogResult.No)
            {
                // Ignore the update
                logger.Info("Update ignored");
            }
        }

        // For moving form with click and drag
        private void qbdlxPictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void topPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }
    }
}
