using QobuzDownloaderX.Helpers;
using QobuzDownloaderX.Properties;
using QobuzDownloaderX.Win32;
using QopenAPI;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using ZetaLongPaths;

namespace QobuzDownloaderX
{
    public partial class LoginForm : Form
    {
        private readonly Theming themeManager = new Theming();
        private LanguageManager languageManager;
        readonly qbdlxForm qbdlx = new qbdlxForm();
        readonly BufferedLogger logger = qbdlxForm._qbdlxForm.logger;
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

            // TextBoxes
            customInfoTextBox.Text = languageManager.GetTranslation("customInfoTextBox");
            aboutTextBox.Text = languageManager.GetTranslation("aboutTextBox");

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
            // Email/usrname
            string savedEmail = Settings.Default.savedEmail ?? "";
            if (!string.IsNullOrEmpty(savedEmail) && savedEmail != emailPlaceholder)
            {
                try
                {
                    byte[] encryptedEmailBytes = Convert.FromBase64String(savedEmail);
                    byte[] decryptedEmailBytes = ProtectedData.Unprotect(encryptedEmailBytes, null, DataProtectionScope.CurrentUser);
                    string decryptedEmail = Encoding.UTF8.GetString(decryptedEmailBytes);
                   
                    username = decryptedEmail;
                    emailTextBox.Text = decryptedEmail;
                }
                catch (FormatException) // saved value is plain text or invalid Base64
                {
                    username = savedEmail;
                    emailTextBox.Text = username;
                }
                catch (CryptographicException) // cannot decrypt (different machine/user)
                {
                    username = savedEmail;
                    emailTextBox.Text = username;
                    // username = "";
                    // emailTextBox.Text = emailPlaceholder;
                }
            }
            else
            {
                emailTextBox.Text = emailPlaceholder;
                emailTextBox.ForeColor = ColorTranslator.FromHtml(themeManager._currentTheme.PlaceholderTextBoxText);
            }

            // Password
            string savedPassword = Settings.Default.savedPassword ?? "";
            if (!string.IsNullOrEmpty(savedPassword) &&
                savedPassword != passwordPlaceholder &&
                savedPassword != tokenPlaceholder)
            {
                try
                {
                    byte[] encryptedPasswordBytes = Convert.FromBase64String(savedPassword);
                    byte[] decryptedPasswordBytes = ProtectedData.Unprotect(encryptedPasswordBytes, null, DataProtectionScope.CurrentUser);
                    string decryptedPassword = Encoding.UTF8.GetString(decryptedPasswordBytes);

                    password = decryptedPassword;
                    passwordTextBox.Text = decryptedPassword;
                    passwordTextBox.PasswordChar = '*';
                    passwordTextBox.ForeColor = ColorTranslator.FromHtml(themeManager._currentTheme.TextBoxText);
                }
                catch (FormatException) // saved value is plain text or invalid Base64
                {
                    password = savedPassword;
                    passwordTextBox.Text = savedPassword;
                }
                catch (CryptographicException) // cannot decrypt (different machine/user)
                {
                    password = savedPassword;
                    passwordTextBox.Text = savedPassword;
                    // password = "";
                    // passwordTextBox.Text = passwordPlaceholder;
                }
            }
            else
            {
                passwordTextBox.Text = passwordPlaceholder;
                passwordTextBox.ForeColor = ColorTranslator.FromHtml(themeManager._currentTheme.PlaceholderTextBoxText);
                passwordTextBox.PasswordChar = '\0';
            }

            // App ID
            string savedAppID = Settings.Default.savedAppID ?? "";
            if (!string.IsNullOrEmpty(savedAppID))
            {
                try
                {
                    byte[] encryptedAppIDBytes = Convert.FromBase64String(savedAppID);
                    byte[] decryptedAppIDBytes = ProtectedData.Unprotect(encryptedAppIDBytes, null, DataProtectionScope.CurrentUser);
                    string decryptedAppID = Encoding.UTF8.GetString(decryptedAppIDBytes);

                    app_id = decryptedAppID;
                    appidTextBox.Text = decryptedAppID;
                }
                catch (FormatException) // saved value is plain text or invalid Base64
                {
                    
                    app_id = savedAppID;
                    appidTextBox.Text = app_id;
                }
                catch (CryptographicException) // cannot decrypt (different machine/user)
                {
                    app_id = savedAppID;
                    appidTextBox.Text = app_id;
                    // app_id = "";
                    // appidTextBox.Text = "";
                }
            }
            else
            {
                app_id = savedAppID;
                appidTextBox.Text = app_id;
            }

            // App Secret
            string savedAppSecret = Settings.Default.savedSecret ?? "";
            if (!string.IsNullOrEmpty(savedAppSecret))
            {
                try
                {
                    byte[] encryptedAppSecretBytes = Convert.FromBase64String(savedAppSecret);
                    string decryptedAppSecret = Encoding.UTF8.GetString(
                        ProtectedData.Unprotect(encryptedAppSecretBytes, null, DataProtectionScope.CurrentUser));

                    app_secret = decryptedAppSecret;
                    appSecretTextBox.Text = decryptedAppSecret;
                }
                catch (FormatException) // saved value is plain text or invalid Base64
                {
                    app_secret = savedAppSecret;
                    appSecretTextBox.Text = app_secret;
                }
                catch (CryptographicException) // cannot decrypt (different machine/user)
                {
                    app_secret = savedAppSecret;
                    appSecretTextBox.Text = app_secret;
                    // app_secret = "";
                    // appSecretTextBox.Text = "";
                }
            }
            else
            {
                app_secret = savedAppSecret;
                appSecretTextBox.Text = app_secret;
            }

            // Alt login handling
            if (Settings.Default.savedAltLoginValue)
            {
                emailIcon.Visible = false;
                emailPanel.Visible = false;
                emailTextBox.Visible = false;
                altLoginLabel.Text = altLoginLabelEmail;
            }
            else
            {
                altLoginLabel.Text = altLoginLabelToken;
            }

            // Final placeholder fixes
            if (emailTextBox.Text == null || emailTextBox.Text == "" || emailTextBox.Text == "\r\n")
            {
                emailTextBox.Text = emailPlaceholder;
                emailTextBox.ForeColor = ColorTranslator.FromHtml(themeManager._currentTheme.PlaceholderTextBoxText);
            }

            if (passwordTextBox.Text == null || passwordTextBox.Text == "" || passwordTextBox.Text == "\r\n")
            {
                passwordTextBox.Text = passwordPlaceholder;
                passwordTextBox.ForeColor = ColorTranslator.FromHtml(themeManager._currentTheme.PlaceholderTextBoxText);
                passwordTextBox.PasswordChar = '\0';
            }

#if DEBUG
            logger.Info("Currently saved username: " + username);
            logger.Info("Currently saved app ID: " + Settings.Default.savedAppID);
            logger.Info("Currently saved app secret: " + Settings.Default.savedSecret);
#endif
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
                var result = await VersionChecker.CheckForUpdate();
                if (result != default) 
                {
                    bool isUpdateAvailable = result.isUpdateAvailable;
                    string _newVersion = result.newVersion ?? "";
                    string _currentVersion = result.currentVersion ?? "";
                    string _changes = result.changes ?? "";

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
                else
                {
                    // result is null, just use defaults
                    changes = "";
                    newVersion = "";
                    currentVersion = "";
                    logger.Debug("Version check returned null.");
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

            // Load saved TLS negotiation settings
            Miscellaneous.SetTLSSetting();

            // Round corners of form
            Region = Region.FromHrgn(NativeMethods.CreateRoundRectRgn(0, 0, Width, Height, 20, 20));

            if (!ZlpIOHelper.FileExists(dllCheck))
            {
                logger.Error("taglib-sharp.dll is missing from folder. Exiting.");
                string exeName = Path.GetFileName(Application.ExecutablePath);
                MessageBox.Show(this, qbdlxForm._qbdlxForm.languageManager.GetTranslation("tagLibSharpMissingMsg").Replace("{exeName}", exeName), Application.ProductName,
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
            aboutTextBox.Text = aboutTextBox.Text.Replace("{version}", Assembly.GetExecutingAssembly().GetName().Version.ToString());

            this.BeginInvoke((Action)(() => loginButton.Focus()));

            // Check for language updates
            await TranslationUpdater.CheckAndUpdateLanguageFiles();
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            logger.Debug("Exiting.");
            Application.Exit();
        }

        private void emailTextBox_Click(object sender, EventArgs e)
        {
            if (emailTextBox.Text == emailPlaceholder)
            {
                emailTextBox.Text = null;
                emailTextBox.ForeColor = ColorTranslator.FromHtml(themeManager._currentTheme.TextBoxText);
            }
        }

        private void emailTextBox_Leave(object sender, EventArgs e)
        {
            if (emailTextBox.Text == null | emailTextBox.Text == "")
            {
                emailTextBox.ForeColor = ColorTranslator.FromHtml(themeManager._currentTheme.PlaceholderTextBoxText);
                if (Settings.Default.savedAltLoginValue == false)
                {
                    emailTextBox.Text = emailPlaceholder;
                }
            }
        }

        private void passwordTextBox_Click(object sender, EventArgs e)
        {
            if (passwordTextBox.Text == passwordPlaceholder | passwordTextBox.Text == tokenPlaceholder)
            {
                passwordTextBox.Text = null;
                passwordTextBox.PasswordChar = '*';
                passwordTextBox.UseSystemPasswordChar = false;
                passwordTextBox.ForeColor = ColorTranslator.FromHtml(themeManager._currentTheme.TextBoxText);
            }
        }

        private void passwordTextBox_Leave(object sender, EventArgs e)
        {
            if (passwordTextBox.Text == null | passwordTextBox.Text == "")
            {
                passwordTextBox.PasswordChar = '\0';
                passwordTextBox.ForeColor = ColorTranslator.FromHtml(themeManager._currentTheme.PlaceholderTextBoxText);
                if (Settings.Default.savedAltLoginValue == false)
                {
                    passwordTextBox.Text = passwordPlaceholder;
                }
                else
                {
                    passwordTextBox.Text = tokenPlaceholder;
                }

            }
        }

        private void visableCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (visableCheckBox.Checked == true)
            {
                passwordTextBox.PasswordChar = '\0';
            }
            else
            {
                passwordTextBox.PasswordChar = '*';
            }
        }

        private void emailTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                loginButton_Click(this, new EventArgs());
                e.SuppressKeyPress = true;
            }
        }

        private void passwordTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                loginButton_Click(this, new EventArgs());
                e.SuppressKeyPress = true;
            }
        }

        private void loginButton_Click(object sender, EventArgs e)
        {
            logger.Debug("Logging in…");

            #region Check if textboxes are valid
            if (emailTextBox.Text == emailPlaceholder || emailTextBox.Text == null || emailTextBox.Text == "")
            {
                // If there's no email typed in. Ignore if using token to login.
                logger.Warning("emailTextBox does not contain proper values for logging in.");
                Settings.Default.savedEmail = "";
                Settings.Default.Save();

                if (!altLoginLabel.Text.Contains(altLoginLabelEmail))
                {
                    loginText.Invoke(new Action(() => loginText.Text = loginTextNoEmail));
                    return;
                }
            }

            if (passwordTextBox.Text == passwordPlaceholder || passwordTextBox.Text == tokenPlaceholder)
            {
                // If there's no password typed in.
                logger.Warning("passwordTextBox does not contain proper values for logging in.");
                Settings.Default.savedPassword = "";
                Settings.Default.Save();
                loginText.Invoke(new Action(() => loginText.Text = loginTextNoPassword));
                return;
            }
            #endregion

            // Assign values from textboxes
            username = emailTextBox.Text;
            password = passwordTextBox.Text;

            // Encrypt username
            try
            {
                byte[] emailBytes = Encoding.UTF8.GetBytes(username);
                byte[] protectedBytes = ProtectedData.Protect(emailBytes, null, DataProtectionScope.CurrentUser);

                Settings.Default.savedEmail = Convert.ToBase64String(protectedBytes);
            }
            catch
            {
                // Fallback: store plain text
                Settings.Default.savedEmail = username;
                logger.Warning("Email encryption failed, storing it as plain text.");
            }

            // Encrypt password
            try
            {
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                byte[] protectedBytes = ProtectedData.Protect(passwordBytes, null, DataProtectionScope.CurrentUser);

                Settings.Default.savedPassword = Convert.ToBase64String(protectedBytes);
            }
            catch
            {
                // Fallback: store plain text
                Settings.Default.savedPassword = password;
                logger.Warning("Password encryption failed, storing it as plain text.");
            }

            // Save to settings
            Settings.Default.Save();

            logger.Debug("Starting loginBackground…");
            loginBackground.RunWorkerAsync();
        }

        private void loginBackground_DoWork(object sender, DoWorkEventArgs e)
        {
            // Set up a StringWriter and attach it to Debug listener.
            // This allows capturing all debug/log output from the third-party DLL Q(Open)API, 
            // so we can inspect errors and messages that would normally only appear in the Immediate Window.
            StringWriter sw = new StringWriter();
            var twListener = new TextWriterTraceListener(sw);
            Debug.Listeners.Add(twListener);
            TextWriter originalConsoleOut = Console.Out;

            try
            {
                loginText.Invoke(new Action(() => loginText.Text = loginTextStart));
                loginButton.Invoke(new Action(() => loginButton.Enabled = false));

                if (appidTextBox.Text == null | appidTextBox.Text == "" | appSecretTextBox.Text == null | appSecretTextBox.Text == "")
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

                    // Encrypt and save App ID and App Secret.
                    SaveAppIdAndSecretCredentials(app_id, app_secret);

                    // Update UI
                    appidTextBox.Invoke(new Action(() => appidTextBox.Text = app_id));
                    appSecretTextBox.Invoke(new Action(() => appSecretTextBox.Text = app_secret));
                }
                else
                {
                    logger.Debug("Using saved/custom app ID and secret");
                    // Use user-provided app_id & login
                    app_id = appidTextBox.Text;
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
                    app_secret = appSecretTextBox.Text;
                    logger.Info("App secret: " + app_secret);

                    // Re-enable login button, and send app_id & app_secret to QBDLX
                    loginButton.Invoke(new Action(() => loginButton.Enabled = true));
                    qbdlx.app_id = app_id;
                    qbdlx.app_secret = app_secret;
                    qbdlx.user_auth_token = user_auth_token;
                    qbdlx.QoUser = QoUser;

                    // Encrypt and save App ID and App Secret.
                    SaveAppIdAndSecretCredentials(app_id, app_secret);

                    // Update UI
                    appidTextBox.Invoke(new Action(() => appidTextBox.Text = app_id));
                    appSecretTextBox.Invoke(new Action(() => appSecretTextBox.Text = app_secret));
                }

                // Hide this window & open QBDLX
                logger.Debug("Login successful! Hiding this form, and launching main form.");
                this.Invoke(new Action(() => this.Hide()));
                Application.Run(qbdlx);
            }
            catch (Exception ex)
            {
                // Flush listeners and gather what was captured
                try { twListener.Flush(); Trace.Flush(); Debug.Flush(); } catch { /* ignore */ }
                string captured = "";
                try { captured = sw.ToString(); } catch { captured = ""; }

                // Remove consecutive duplicate lines and filter out unwanted "shit aint work" line
                string deduped = "";
                try
                {
                    var lines = captured
                        .Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries)
                        .Where(line => line.ToLowerInvariant().Trim() != "shit aint work") // ignore this line
                        .ToArray();

                    var sb = new StringBuilder();
                    string prev = null;
                    foreach (var line in lines)
                    {
                        if (line == prev) continue;
                        if (sb.Length > 0) sb.AppendLine();
                        sb.Append(line);
                        prev = line;
                    }
                    deduped = sb.ToString();
                }
                catch
                {
                    deduped = captured; // fallback
                }

                string loginError = ex.ToString();
                logger.Error("Login failed, error listed below.");
                logger.Error("Error:\r\n" + ex);
                if (!string.IsNullOrWhiteSpace(deduped))
                    logger.Error("Captured output:\r\n" + deduped);

                try
                {
                    File.WriteAllText(errorLog, loginError + "\r\nDLL output:\r\n" + deduped);
                }
                catch
                {
                    /* ignore */
                }

                // Restore listeners and console BEFORE touching the UI
                try
                {
                    if (Debug.Listeners.Contains(twListener)) Debug.Listeners.Remove(twListener);
                    Console.SetOut(originalConsoleOut);
                }
                catch { /* ignore */ }

                // Try to parse the last line as JSON to extract error & message
                string messageToShow = loginError; // default fallback
                if (!string.IsNullOrWhiteSpace(deduped))
                {
                    var lines = deduped.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
                    string lastLine = lines.LastOrDefault()?.Trim();

                    if (!string.IsNullOrEmpty(lastLine))
                    {
                        try
                        {
                            var json = Newtonsoft.Json.Linq.JObject.Parse(lastLine);
                            string status = json["status"]?.ToString();
                            string code = json["code"]?.ToString();
                            string message = json["message"]?.ToString();

                            if (!string.IsNullOrEmpty(status) && !string.IsNullOrEmpty(message))
                            {
                                messageToShow = $"Error {code}\n\n{message}";
                            }
                            else
                            {
                                messageToShow = deduped; // fallback to captured output
                            }
                        }
                        catch
                        {
                            // not JSON, just show captured output
                            messageToShow = deduped;
                        }
                    }
                }

                // Update UI and show MessageBox on the UI thread (safe)
                if (this.IsHandleCreated && this.InvokeRequired)
                {
                    this.Invoke((Action)(() =>
                    {
                        loginText.Text = loginTextError;
                        loginButton.Enabled = true;
                        MessageBox.Show(this, messageToShow, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }));
                }
                else
                {
                    loginText.Text = loginTextError;
                    loginButton.Enabled = true;
                    MessageBox.Show(this, messageToShow, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                return;
            }
            finally
            {
                try
                {
                    if (Debug.Listeners.Contains(twListener)) Debug.Listeners.Remove(twListener);
                    if (Trace.Listeners.Contains(twListener)) Trace.Listeners.Remove(twListener);
                    Console.SetOut(originalConsoleOut);
                }
                catch { /* ignore */ }
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
                emailTextBox.Visible = false;

                passwordTextBox.Text = null;
                passwordTextBox_Leave(this, new EventArgs());
            }
            else
            {
                logger.Debug("Swapping login method to e-mail and password");
                Settings.Default.savedAltLoginValue = false;
                altLoginLabel.Text = altLoginLabelToken;

                emailIcon.Visible = true;
                emailPanel.Visible = true;
                emailTextBox.Visible = true;

                passwordTextBox.Text = null;
                passwordTextBox_Leave(this, new EventArgs());
                emailTextBox.Text = null;
                emailTextBox_Leave(this, new EventArgs());
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
            loginAboutPanel.BringToFront();
        }

        private void closeAboutButton_Click(object sender, EventArgs e)
        {
            logger.Debug("Hiding about panel");
            loginAboutPanel.Enabled = false;
            loginAboutPanel.Visible = false;
        }

        private void customSaveButton_Click(object sender, EventArgs e)
        {
            logger.Debug("Saving custom app ID and secret…");

            // Encrypt and save custom AppID & Secret
            SaveAppIdAndSecretCredentials(appidTextBox.Text, appSecretTextBox.Text);

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
            DialogResult dialogResult = MessageBox.Show(this, updateNotification.Replace("{currentVersion}", currentVersion).Replace("{newVersion}", newVersion).Replace("{changelog}", changes.Replace("\\r\\n", "\r\n")), updateNotificationTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
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

        private void SaveAppIdAndSecretCredentials(string appId, string appSecret)
        {
            try
            {
                // App ID
                if (string.IsNullOrWhiteSpace(appId))
                {
                    Settings.Default.savedAppID = string.Empty;
                }
                else
                {
                    try
                    {
                        byte[] appIdBytes = Encoding.UTF8.GetBytes(appId);
                        byte[] protectedBytes = ProtectedData.Protect(appIdBytes, null, DataProtectionScope.CurrentUser);

                        Settings.Default.savedAppID = Convert.ToBase64String(protectedBytes);
                    }
                    catch
                    {
                        // Fallback: store plain text
                        Settings.Default.savedAppID = appId;
                        logger.Warning("App ID encryption failed, storing it as plain text.");
                    }
                }

                // App Secret
                if (string.IsNullOrWhiteSpace(appSecret))
                {
                    Settings.Default.savedSecret = string.Empty;
                }
                else
                {
                    try
                    {
                        byte[] appSecretBytes = Encoding.UTF8.GetBytes(appSecret);
                        byte[] protectedBytes = ProtectedData.Protect(appSecretBytes, null, DataProtectionScope.CurrentUser);

                        Settings.Default.savedSecret = Convert.ToBase64String(protectedBytes);
                    }
                    catch
                    {
                        // Fallback: store plain text
                        Settings.Default.savedSecret = appSecret;
                        logger.Warning("App secret encryption failed, storing it as plain text.");
                    }
                }

                Settings.Default.Save();
                logger.Info("AppId and AppSecret credentials saved successfully.");
            }
            catch (Exception ex)
            {
                // This should *never* happen unless Settings.Save() explodes
                logger.Error($"Failed to persist app credentials: {ex}");
            }
        }
    }
}
