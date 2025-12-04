using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

using QopenAPI;

using QobuzDownloaderX.Helpers;
using QobuzDownloaderX.Properties;

namespace QobuzDownloaderX
{
    public partial class LoginForm : Form
    {


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

        private readonly Theming themeManager = new Theming();
        private LanguageManager languageManager;
        readonly qbdlxForm qbdlx = new qbdlxForm();
        readonly Logger logger = qbdlxForm._qbdlxForm.logger;
        readonly Service QoService = new Service();
        User QoUser;

        public string currentVersion { get; set; }
        public string newVersion { get; set; }
        public string changes { get; set; }

        public string username { get; set; }
        public string password { get; set; }
        public string user_auth_token { get; set; }
        public string app_id { get; set; }
        public string app_secret { get; set; }

        public string user_id { get; set; }
        public string user_display_name { get; set; }

        public string latestWebResponse { get; set; }

        // Create language options
        public string emailPlaceholder { get; set; }
        public string passwordPlaceholder { get; set; }
        public string tokenPlaceholder { get; set; }
        public string altLoginLabelToken { get; set; }
        public string altLoginLabelEmail { get; set; }
        public string loginTextWaiting { get; set; }
        public string loginTextStart { get; set; }
        public string loginTextError { get; set; }
        public string loginTextNoEmail { get; set; }
        public string loginTextNoPassword { get; set; }
        public string updateNotification { get; set; }
        public string updateNotificationTitle { get; set; }

        public LoginForm()
        {
            InitializeComponent();
        }

        readonly string errorLog = Path.GetDirectoryName(Application.ExecutablePath) + "\\Latest_Error.log";
        readonly string dllCheck = Path.GetDirectoryName(Application.ExecutablePath) + "\\taglib-sharp.dll";

        private void UpdateUILanguage()
        {
            // Load the font name from the translation file
            string fontName = languageManager.GetTranslation("TranslationFont");

            if (!string.IsNullOrEmpty(fontName))
            {
                // Call method to update fonts
                languageManager.UpdateControlFont(this.Controls, fontName);
            }

            /* Update labels, buttons, textboxes, etc., based on the loaded language */

            // Buttons
            closeAboutButton.Text = languageManager.GetTranslation("closeAboutButton");
            customSaveButton.Text = languageManager.GetTranslation("customSaveButton");
            exitButton.Text = languageManager.GetTranslation("exitButton");
            loginButton.Text = languageManager.GetTranslation("loginButton");
            aboutButton.Text = languageManager.GetTranslation("aboutButton");

            // Labels
            appidLabel.Text = languageManager.GetTranslation("appidLabel");
            appSecretLabel.Text = languageManager.GetTranslation("appSecretLabel");
            customLabel.Text = languageManager.GetTranslation("customLabel");
            loginText.Text = languageManager.GetTranslation("loginTextWaiting");

            // Textboxes
            customInfoTextbox.Text = languageManager.GetTranslation("customInfoTextbox");
            aboutTextbox.Text = languageManager.GetTranslation("aboutTextbox");

            // Placeholders
            emailPlaceholder = languageManager.GetTranslation("emailPlaceholder");
            passwordPlaceholder = languageManager.GetTranslation("passwordPlaceholder");
            tokenPlaceholder = languageManager.GetTranslation("tokenPlaceholder");
            altLoginLabelToken = languageManager.GetTranslation("altLoginLabelToken");
            altLoginLabelEmail = languageManager.GetTranslation("altLoginLabelEmail");
            loginTextWaiting = languageManager.GetTranslation("loginTextWaiting");
            loginTextStart = languageManager.GetTranslation("loginTextStart");
            loginTextError = languageManager.GetTranslation("loginTextError");
            loginTextNoEmail = languageManager.GetTranslation("loginTextNoEmail");
            loginTextNoPassword = languageManager.GetTranslation("loginTextNoPassword");
            updateNotification = languageManager.GetTranslation("updateNotification");
            updateNotificationTitle = languageManager.GetTranslation("updateNotificationTitle");

            // Set placeholders
            altLoginLabel.Text = altLoginLabelToken;
        }

        private void SetSavedValues()
        {
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

            if (Settings.Default.savedAltLoginValue == true)
            {
                emailIcon.Visible = false;
                emailPanel.Visible = false;
                emailTextbox.Visible = false;

                altLoginLabel.Text = altLoginLabelEmail;
            }

            // Set values for email textbox.
            if (emailTextbox.Text != emailPlaceholder)
            {
                emailTextbox.ForeColor = ColorTranslator.FromHtml(themeManager._currentTheme.TextBoxText);
            }
            if (emailTextbox.Text == null | emailTextbox.Text == "" | emailTextbox.Text == "\r\n")
            {
                emailTextbox.ForeColor = ColorTranslator.FromHtml(themeManager._currentTheme.PlaceholderTextBoxText);
                emailTextbox.Text = emailPlaceholder;
            }

            // Set values for password textbox.
            if (passwordTextbox.Text != passwordPlaceholder | passwordTextbox.Text != tokenPlaceholder)
            {
                passwordTextbox.PasswordChar = '*';
                passwordTextbox.ForeColor = ColorTranslator.FromHtml(themeManager._currentTheme.TextBoxText);
            }
            if (passwordTextbox.Text == null | passwordTextbox.Text == "" | passwordTextbox.Text == "\r\n")
            {
                passwordTextbox.PasswordChar = '\0';
                passwordTextbox.ForeColor = ColorTranslator.FromHtml(themeManager._currentTheme.PlaceholderTextBoxText);
                passwordTextbox.Text = passwordPlaceholder;
            }
        }

        private void InitializeTheme()
        {
            themeManager.LoadTheme(Settings.Default.currentTheme);
            themeManager.ApplyTheme(this);
        }

        private void InitializeLanguage()
        {
            languageManager = new LanguageManager();
            languageManager.LoadLanguage($"languages/{Settings.Default.currentLanguage.ToLower()}.json");
            UpdateUILanguage();
        }

        private async void CheckForNewVersion()
        {
            try
            {
                var (isUpdateAvailable, _newVersion, _currentVersion, _changes) = await VersionChecker.CheckForUpdate();

                changes = _changes;
                newVersion = _newVersion;
                currentVersion = _currentVersion;

                if (isUpdateAvailable)
                {
                    logger.Warning("An update is available.");
                    updateButton.Enabled = true;
                    updateButton.Visible = true;
                }
                else
                {
                    logger.Debug("No update needed.");
                }
            }
            catch (Exception ex)
            {
                logger.Error($"Connection to GitHub to check for an update has failed: {ex.Message}");
            }
        }

        private async void LoginForm_Load(object sender, EventArgs e)
        {
            // Avoids annoying image transition visuals.
            this.qbdlxPictureBox.InitialImage = null;
            this.qbdlxPictureBox.Image = null;

            // Upgrade previous settings to current version
            if (Properties.Settings.Default.UpgradeRequired)
            {
                Properties.Settings.Default.Upgrade();
                Properties.Settings.Default.UpgradeRequired = false;
                Properties.Settings.Default.Save();
            }

            // Round corners of form
            Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));

            if (!System.IO.File.Exists(dllCheck))
            {
                logger.Error("taglib-sharp.dll is missing from folder. Exiting.");
                string exeName = Path.GetFileName(Application.ExecutablePath);
                MessageBox.Show($"taglib-sharp.dll missing from folder!\r\nPlease Make sure the DLL is in the same folder as {exeName}!", "ERROR",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }

            // Center program + Set theme, language, saved values + Check for update on GitHub
            CenterToScreen();
            InitializeTheme(); 
            InitializeLanguage();
            SetSavedValues();
            CheckForNewVersion();

            // Get and display version number.
            logger.Info("QobuzDownlaoderX | Version " + Assembly.GetExecutingAssembly().GetName().Version.ToString());
            versionNumber.Text = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            aboutTextbox.Text = aboutTextbox.Text.Replace("{version}", Assembly.GetExecutingAssembly().GetName().Version.ToString());

            this.BeginInvoke((Action)(() => loginButton.Focus()));

            // Check for language updates
            await TranslationUpdater.CheckAndUpdateLanguageFiles();

        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            logger.Debug("Exiting.");
            Application.Exit();
        }

        private void emailTextbox_Click(object sender, EventArgs e)
        {
            if (emailTextbox.Text == emailPlaceholder)
            {
                emailTextbox.Text = null;
                emailTextbox.ForeColor = ColorTranslator.FromHtml(themeManager._currentTheme.TextBoxText);
            }
        }

        private void emailTextbox_Leave(object sender, EventArgs e)
        {
            if (emailTextbox.Text == null | emailTextbox.Text == "")
            {
                emailTextbox.ForeColor = ColorTranslator.FromHtml(themeManager._currentTheme.PlaceholderTextBoxText);
                if (Settings.Default.savedAltLoginValue == false)
                {
                    emailTextbox.Text = emailPlaceholder;
                }
            }
        }

        private void passwordTextbox_Click(object sender, EventArgs e)
        {
            if (passwordTextbox.Text == passwordPlaceholder | passwordTextbox.Text == tokenPlaceholder)
            {
                passwordTextbox.Text = null;
                passwordTextbox.PasswordChar = '*';
                passwordTextbox.UseSystemPasswordChar = false;
                passwordTextbox.ForeColor = ColorTranslator.FromHtml(themeManager._currentTheme.TextBoxText);
            }
        }

        private void passwordTextbox_Leave(object sender, EventArgs e)
        {
            if (passwordTextbox.Text == null | passwordTextbox.Text == "")
            {
                passwordTextbox.PasswordChar = '\0';
                passwordTextbox.ForeColor = ColorTranslator.FromHtml(themeManager._currentTheme.PlaceholderTextBoxText);
                if (Settings.Default.savedAltLoginValue == false)
                {
                    passwordTextbox.Text = passwordPlaceholder;
                }
                else
                {
                    passwordTextbox.Text = tokenPlaceholder;
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
            if (emailTextbox.Text == emailPlaceholder | emailTextbox.Text == null | emailTextbox.Text == "")
            {
                // If there's no email typed in. Ignore if using token to login.
                logger.Warning("emailTextbox does not contain proper values for logging in.");
                if (!altLoginLabel.Text.Contains(altLoginLabelEmail))
                {
                    loginText.Invoke(new Action(() => loginText.Text = loginTextNoEmail));
                    return;
                }
            }

            if (passwordTextbox.Text == passwordPlaceholder | passwordTextbox.Text == tokenPlaceholder)
            {
                // If there's no password typed in.
                logger.Warning("passwordTextbox does not contain proper values for logging in.");
                loginText.Invoke(new Action(() => loginText.Text = loginTextNoPassword));
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
                loginText.Invoke(new Action(() => loginText.Text = loginTextStart));
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
                    appidTextbox.Invoke(new Action(() => appidTextbox.Text = app_id));
                    appSecretTextbox.Invoke(new Action(() => appSecretTextbox.Text = app_secret));
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
                    appidTextbox.Invoke(new Action(() => appidTextbox.Text = app_id));
                    appSecretTextbox.Invoke(new Action(() => appSecretTextbox.Text = app_secret));
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
                loginText.Invoke(new Action(() => loginText.Text = loginTextError));
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

        private void altLoginLabel_Click(object sender, EventArgs e)
        {
            if (altLoginLabel.Text.Contains(altLoginLabelToken))
            {
                logger.Debug("Swapping login method to token");
                Settings.Default.savedAltLoginValue = true;
                altLoginLabel.Text = altLoginLabelEmail;

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
                altLoginLabel.Text = altLoginLabelToken;

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
            altLoginLabel.ForeColor = ColorTranslator.FromHtml(themeManager._currentTheme.PlaceholderTextBoxText);
        }

        private void altLoginLabel_MouseLeave(object sender, EventArgs e)
        {
            altLoginLabel.ForeColor = ColorTranslator.FromHtml(themeManager._currentTheme.LabelText);
        }

        private void cusotmLabel_MouseEnter(object sender, EventArgs e)
        {
            customLabel.ForeColor = ColorTranslator.FromHtml(themeManager._currentTheme.PlaceholderTextBoxText);
        }

        private void cusotmLabel_MouseLeave(object sender, EventArgs e)
        {
            customLabel.ForeColor = ColorTranslator.FromHtml(themeManager._currentTheme.LabelText);
        }

        private void updateButton_Click(object sender, EventArgs e)
        {
            logger.Debug("Opening update information dialog");
            DialogResult dialogResult = MessageBox.Show(updateNotification.Replace("{currentVersion}", currentVersion).Replace("{newVersion}", newVersion).Replace("{changelog}", changes.Replace("\\r\\n", "\r\n")), updateNotificationTitle, MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                // If "Yes" is clicked, open GitHub page and close QBDLX.
                logger.Debug("Opening GitHub page for latest update");
                Process.Start("https://github.com/ImAiiR/QobuzDownloaderX/releases/latest");
                logger.Debug("Exiting");
                Application.Exit();
            }
            else
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
                NativeMethods.ReleaseCapture();
                NativeMethods.SendMessage(Handle, Win32.Constants.WM_NCLBUTTONDOWN, Win32.Constants.HT_CAPTION, 0);
            }
        }

        private void topPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                NativeMethods.ReleaseCapture();
                NativeMethods.SendMessage(Handle, Win32.Constants.WM_NCLBUTTONDOWN, Win32.Constants.HT_CAPTION, 0);
            }
        }

    }
}
