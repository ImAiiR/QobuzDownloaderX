using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

using Newtonsoft.Json;
using ZetaLongPaths;

using QobuzDownloaderX.Properties;
using QobuzDownloaderX.UserControls;

namespace QobuzDownloaderX.Helpers
{
    public class ThemeSettings
    {
        public Dictionary<string, Theme> Themes { get; set; }
        private readonly Dictionary<string, Image> originalImages = new Dictionary<string, Image>();
    }

    public class Theme
    {
        public string FormBackground { get; set; }
        public string MainPanelBackground { get; set; }
        public string HighlightedButtonBackground { get; set; }
        public string SidePanelBackground { get; set; }
        public string ButtonBackground { get; set; }
        public string ClickedButtonBackground { get; set; }
        public string TextBoxBackground { get; set; }
        public string PlaceholderTextBoxText { get; set; }
        public string TextBoxText { get; set; }
        public string LabelText { get; set; }
        public string ButtonText { get; set; }
        public string NavBarIconColor { get; set; }
        public string HiResLabelText { get; set; }
        public string LogoReplaceURL { get; set; }
        public bool InvertLogo { get; set; }
    }

    public class Theming
    {
        public Theme _currentTheme;

        public void LoadTheme(string themeName)
        {
            string themesFilePath = ZlpPathHelper.GetDirectoryPathNameFromFilePath(Application.ExecutablePath) + "\\themes.json";
            if (ZlpIOHelper.FileExists(themesFilePath))
            {
                string json = ZlpIOHelper.ReadAllText(themesFilePath);
                var themeSettings = JsonConvert.DeserializeObject<ThemeSettings>(json);

                if (themeSettings.Themes.TryGetValue(themeName, out var theme))
                {
                    _currentTheme = theme;
                }
                else
                {
                    MessageBox.Show($"Theme '{themeName}' not found.");
                }
            }
            else
            {
                MessageBox.Show("Theme file not found.");
            }
        }
        public void PopulateThemeOptions(qbdlxForm mainForm)
        {
            string themesFilePath = ZlpPathHelper.GetDirectoryPathNameFromFilePath(Application.ExecutablePath) + "\\themes.json";
            if (ZlpIOHelper.FileExists(themesFilePath))
            {
                string json = ZlpIOHelper.ReadAllText(themesFilePath);
                var themeSettings = JsonConvert.DeserializeObject<ThemeSettings>(json);

                mainForm.Invoke((MethodInvoker)delegate ()
                {
                    ComboBox themeComboBox = mainForm.themeComboBox;

                    // Clear existing items (if any)
                    themeComboBox.Items.Clear();

                    // Add theme names to the ComboBox
                    foreach (var theme in themeSettings.Themes)
                    {
                        themeComboBox.Items.Add(theme.Key);
                    }
                });
            }
            else
            {
                MessageBox.Show("Theme file not found.");
            }
        }

        public void ApplyTheme(Control parent)
        {
            if (_currentTheme == null) return;

            // Set theme for the form itself if `parent` is the form
            if (parent is Form form)
            {
                qbdlxForm._qbdlxForm.logger.Info("Setting theme for form: " + form.Name);
                form.BackColor = ColorTranslator.FromHtml(_currentTheme.FormBackground);
            }

            // Loop through all child controls of the parent
            foreach (Control control in parent.Controls)
            {
                if (control is Button button)
                {
                    button.ForeColor = ColorTranslator.FromHtml(_currentTheme.ButtonText);
                    button.FlatAppearance.MouseOverBackColor = ColorTranslator.FromHtml(_currentTheme.HighlightedButtonBackground);
                    button.FlatAppearance.MouseDownBackColor = ColorTranslator.FromHtml(_currentTheme.ClickedButtonBackground);

                    if (button.Name == "updateButton") { continue; } // Don't need to change background of this

                    if (button.Image != null)
                    {
                        // Change the button's image color
                        button.Image = ChangeImageColor((Bitmap)button.Image, ColorTranslator.FromHtml(_currentTheme.NavBarIconColor));
                    }

                    // Apply specific colors for specific panels if needed
                    button.BackColor = button.Name == "qualitySelectButton"
                        ? ColorTranslator.FromHtml(_currentTheme.TextBoxBackground)
                        : ColorTranslator.FromHtml(_currentTheme.ButtonBackground);
                }
                else if (control is Label label)
                {
                    if (label.Name == "versionNumber") { if (parent.Name == "LoginForm") { label.BackColor = ColorTranslator.FromHtml(_currentTheme.MainPanelBackground); } else { label.BackColor = ColorTranslator.FromHtml(_currentTheme.SidePanelBackground); } }

                    // Apply specific colors for specific panels if needed
                    label.ForeColor = label.Name == "movingLabel"
                        ? ColorTranslator.FromHtml(_currentTheme.MainPanelBackground)
                        : ColorTranslator.FromHtml(_currentTheme.LabelText);
                }
                else if (control is TextBox textBox)
                {
                    textBox.ForeColor = ColorTranslator.FromHtml(_currentTheme.TextBoxText);

                    // Apply specific colors for specific panels if needed
                    textBox.BackColor = (textBox.Name == "userInfoTextbox" || textBox.Name == "emailTextbox" || textBox.Name == "passwordTextbox" || textBox.Name == "appidTextbox" || textBox.Name == "appSecretTextbox")
                        ? ColorTranslator.FromHtml(_currentTheme.MainPanelBackground)
                        : ColorTranslator.FromHtml(_currentTheme.TextBoxBackground);

                    textBox.ForeColor = (textBox.Text == "Paste a Qobuz URL..." || textBox.Text == "Input your search..." || textBox.Text == "e-mail" || textBox.Text == "password" || textBox.Text == "token")
                        ? ColorTranslator.FromHtml(_currentTheme.PlaceholderTextBoxText)
                        : ColorTranslator.FromHtml(_currentTheme.TextBoxText);
                }
                else if (control is CustomProgressBar progressBar)
                {
                    progressBar.BackgroundColor = ColorTranslator.FromHtml(_currentTheme.TextBoxBackground);
                    progressBar.ForeColor = ColorTranslator.FromHtml(_currentTheme.TextBoxText);
                    progressBar.BorderColor = progressBar.BackgroundColor;
                    progressBar.FillColor = Color.RoyalBlue; // Fits good with all themes.

                }
                else if (control is PictureBox pictureBox)
                {
                    // Apply specific colors for specific panels if needed
                    pictureBox.BackColor = pictureBox.Name == "logoPictureBox"
                        ? ColorTranslator.FromHtml(_currentTheme.SidePanelBackground)
                        : ColorTranslator.FromHtml(_currentTheme.MainPanelBackground);

                    if (_currentTheme.InvertLogo)
                    {
                        // Use custom image URL or reset to default logo, and then invert
                        if (_currentTheme.LogoReplaceURL != null)
                        {
                            if (pictureBox.Name == "logoPictureBox" || pictureBox.Name == "qbdlxPictureBox") { try { pictureBox.ImageLocation = _currentTheme.LogoReplaceURL; } catch { } }
                        }
                        else
                        {
                            if (pictureBox.Name == "logoPictureBox" || pictureBox.Name == "qbdlxPictureBox") { pictureBox.Image = Resources.qbdlx_new; }
                        }

                        if (pictureBox.Name == "logoPictureBox" || pictureBox.Name == "qbdlxPictureBox" || pictureBox.Name == "albumPictureBox") { pictureBox.Image = InvertImage(pictureBox.Image); }
                    }
                    else
                    {
                        // Use custom image URL or reset to default logo, do not invert
                        if (_currentTheme.LogoReplaceURL != null)
                        {
                            if (pictureBox.Name == "logoPictureBox" || pictureBox.Name == "qbdlxPictureBox") { try { pictureBox.ImageLocation = _currentTheme.LogoReplaceURL; } catch { } }
                        }
                        else
                        {
                            if (pictureBox.Name == "logoPictureBox" || pictureBox.Name == "qbdlxPictureBox") { pictureBox.Image = Resources.qbdlx_new; }
                        }
                    }
                }
                else if (control is Panel panel)
                {
                    if (panel.Name == "emailPanel" || panel.Name == "passwordPanel") { continue; }

                    // Apply specific colors for specific panels if needed
                    panel.BackColor = (panel.Name == "navigationPanel" || panel.Name == "logoPanel")
                        ? ColorTranslator.FromHtml(_currentTheme.SidePanelBackground)
                        : ColorTranslator.FromHtml(_currentTheme.MainPanelBackground);

                    if (panel.Name == "qualitySelectPanel" || panel.Name == "loginAboutPanel" || panel.Name == "customPanel" || panel.Name == "searchResultsPanel" || panel.Name == "searchResultsTablePanel") { panel.BackColor = ColorTranslator.FromHtml(_currentTheme.TextBoxBackground); }
                }

                // Recursive call to apply theme to any nested child controls, regardless of control type
                if (control.HasChildren)
                {
                    ApplyTheme(control);
                }
            }
        }

        private Image InvertImage(Image image)
        {
            if (image == null)
                return null;

            // Create a new bitmap with the same dimensions
            Bitmap invertedImage = new Bitmap(image.Width, image.Height);

            // Loop through each pixel
            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    // Get the pixel color
                    Color pixelColor = ((Bitmap)image).GetPixel(x, y);

                    // Check if the pixel is not fully transparent
                    if (pixelColor.A > 0)
                    {
                        // Invert the RGB components while preserving alpha
                        Color invertedColor = Color.FromArgb(pixelColor.A, 255 - pixelColor.R, 255 - pixelColor.G, 255 - pixelColor.B);
                        invertedImage.SetPixel(x, y, invertedColor);
                    }
                    else
                    {
                        // Preserve the transparent pixel
                        invertedImage.SetPixel(x, y, Color.Transparent);
                    }
                }
            }

            return invertedImage;
        }

        private Bitmap ChangeImageColor(Bitmap originalImage, Color newColor)
        {
            Bitmap newImage = new Bitmap(originalImage.Width, originalImage.Height);

            for (int x = 0; x < originalImage.Width; x++)
            {
                for (int y = 0; y < originalImage.Height; y++)
                {
                    Color pixelColor = originalImage.GetPixel(x, y);

                    // Preserve transparency
                    if (pixelColor.A == 0)
                    {
                        newImage.SetPixel(x, y, Color.Transparent);
                    }
                    else
                    {
                        // Change the color to the specified new color, keeping the alpha
                        newImage.SetPixel(x, y, Color.FromArgb(pixelColor.A, newColor.R, newColor.G, newColor.B));
                    }
                }
            }

            return newImage;
        }
    }

    public class LanguageManager
    {
        private Dictionary<string, string> languageDictionary;
        public string languagesDirectory = "languages";

        // Default English translation if no files are avaialble
        public string defaultLanguage = @"{
	""TranslationCredit"":			""AiiR"",
	""TranslationUpdatedOn"":		""December 4, 2025, 12:38PM EST"",
	""TranslationFont"":			""Nirmala UI"",

	""_SECTION1_"":					""=================== MAIN FORM BUTTONS ==================="",
	""additionalSettingsButton"":	""Additional Settings"",
	""aboutButton"": 				""ABOUT..."",
	""closeAdditionalButton"":		""Back to Settings"",
	""downloadButton"":				""GET"",
	""batchDownloadButton"":		""GET BATCH"",
	""abortButton"": 				""ABORT"",
	""skipButton"": 				""SKIP"",
	""downloaderButton"": 			""DOWNLOADER"",
	""logoutButton"": 				""LOGOUT"",
	""openFolderButton"":			""Open Folder"",
	""qualitySelectButton"":		""Quality Selector"",
	""saveTemplatesButton"":		""Save"",
	""searchButton"": 				""SEARCH"",
	""searchAlbumsButton"":			""RELEASES"",
	""searchTracksButton"":			""TRACKS"",
	""selectFolderButton"":			""Select Folder"",
	""settingsButton"": 			""SETTINGS"",

	""_SECTION2_"":					""=================== MAIN FORM LABELS ==================="",
	""advancedOptionsLabel"":		""ADVANCED OPTIONS"",
	""albumTemplateLabel"":			""ALBUM TEMPLATE"",
	""artistTemplateLabel"":		""ARTIST TEMPLATE"",
	""commentLabel"":				""Custom Comment"",
	""downloadFolderLabel"":		""DOWNLOAD FOLDER"",
	""downloadOptionsLabel"":		""DOWNLOAD OPTIONS"",
	""embeddedArtLabel"":			""Embedded Artwork Size"",
	""extraSettingsLabel"":			""ADDITIONAL SETTINGS"",
	""languageLabel"":				""Current Language"",
	""playlistTemplateLabel"":		""PLAYLIST TEMPLATE"",
	""favoritesTemplateLabel"":		""FAVORITES TEMPLATE"",
	""savedArtLabel"":				""Saved Artwork Size"",
	""searchingLabel"":				""Searching..."",
	""taggingOptionsLabel"":		""TAGGING OPTIONS"",
	""templatesLabel"":				""TEMPLATES"",
	""templatesListLabel"":			""TEMPLATES LIST"",
	""themeLabel"":					""Current Theme"",
	""themeSectionLabel"":			""THEMING OPTIONS"",
	""trackTemplateLabel"":			""TRACK TEMPLATE"",
	""userInfoLabel"": 				""USER INFO"",
	""welcomeLabel"": 				""Welcome\r\n{username}"",
	""limitSearchResultsLabel"":	""Results Limit:"",
	""searchSortingLabel"":			""Sort By:"",
	""sortReleaseDateLabel"":		""Release Date"",
	""sortArtistNameLabel"":		""Artist Name"",
	""sortAlbumTrackNameLabel"":	""Album / Track Name"",
	""sortingSearchResultsLabel"":	""Sorting..."",
	""searchResultsCountLabel"":    ""results"",

	""_SECTION3_"":					""=================== MAIN FORM CHECKBOXES ==================="",
	""albumArtistCheckbox"":		""Album Artist"",
	""albumTitleCheckbox"":			""Album Title"",
	""trackArtistCheckbox"":		""Track Artist"",
	""trackTitleCheckbox"":			""Track Title"",
	""releaseDateCheckbox"":		""Release Date"",
	""releaseTypeCheckbox"":		""Release Type"",
	""genreCheckbox"":				""Genre"",
	""trackNumberCheckbox"":		""Track Number"",
	""trackTotalCheckbox"":			""Total Tracks"",
	""discNumberCheckbox"":			""Disc Number"",
	""discTotalCheckbox"":			""Total Discs"",
	""composerCheckbox"":			""Composer"",
	""explicitCheckbox"":			""Explicit Advisory"",
	""coverArtCheckbox"":			""Cover Art"",
	""copyrightCheckbox"":			""Copyright"",
	""labelCheckbox"":				""Label"",
	""upcCheckbox"":				""UPC / Barcode"",
	""isrcCheckbox"":				""ISRC"",
	""urlCheckbox"":				""URL"",
	""mergeArtistNamesCheckbox"":   ""Merge Artist Names"",
	""streamableCheckbox"":			""Streamable Check"",
	""fixMD5sCheckbox"":			""Auto-Fix Unset MD5s (must have FLAC in PATH variables)"",
	""downloadSpeedCheckbox"":		""Print Download Speed"",
	""sortAscendantCheckBox"":	    ""Ascendant"",

	""_SECTION4_"":					""=================== MAIN FORM PLACEHOLDERS ==================="",
	""albumLabelPlaceholder"":		""Welcome to QBDLX!"",
	""artistLabelPlaceholder"":		""Input your Qobuz link and hit GET!"",
	""infoLabelPlaceholder"":		""Released"",
	""inputTextboxPlaceholder"":	""Paste a Qobuz URL..."",
	""searchTextboxPlaceholder"":	""Input your search..."",
	""downloadFolderPlaceholder"":	""No folder selected"",
	""userInfoTextboxPlaceholder"":	""User ID = {user_id}\r\nE-mail = {user_email}\r\nCountry = {user_country}\r\nSubscription = {user_subscription}\r\nExpires = {user_subscription_expiration}"",
	""downloadOutputWelcome"":		""Welcome {user_display_name}!"",
	""downloadOutputExpired"": 		""YOUR SUBSCRIPTION HAS EXPIRED, DOWNLOADS WILL BE LIMITED TO 30 SECOND SNIPPETS!"",
	""downloadOutputPath"": 		""Download Path:"",
	""downloadOutputNoPath"":		""No path has been set! Remember to Choose a Folder!"",
	""downloadOutputNoUrl"": 		""Track {TrackNumber} is not available for download. Skipping."",
	""downloadOutputDontExist"":	""The specified path does not exist."",
	""downloadOutputAPIError"": 	""Qobuz API error. Maybe release isn't available in this account region?"",
	""downloadOutputNotImplemented"": ""Not implemented yet or the URL is not understood. Is there a typo?"",
	""downloadOutputCheckLink"": 	""Checking Link..."",
	""downloadOutputTrNotStream"": 	""Track {TrackNumber} is not available for streaming. Skipping."",
	""downloadOutputAlNotStream"": 	""Release is not available for streaming."",
	""downloadOutputGoodyFound"": 	""Goody found, downloading..."",
	""downloadOutputGoodyExists"": 	""File for goody already exists"",
	""downloadOutputGoodyNoURL"": 	""No download URL found for goody, skipping"",
	""downloadOutputFileExists"": 	""File for track {TrackNumber} already exists, skipping."",
	""downloadOutputDownloading"": 	""Downloading"",
	""downloadOutputDone"": 		""DONE"",
	""downloadOutputCompleted"": 	""DOWNLOAD COMPLETE"",
	""downloadAborted"": 			""DOWNLOAD ABORTED BY USER."",
	""albumSkipped"": 			    ""ALBUM SKIPPED BY USER."",
	""progressLabelInactive"": 		""No download active"",
	""progressLabelActive"": 		""Download progress"",
	""formClosingWarning"": 		""A download is in progress, do you really want to quit?."",
	""artist"": 					""Artist"", // First char in upper-case.
	""artists"": 					""artists"",
	""album"": 						""Album"",  // First char in upper-case.
	""albums"": 					""albums"",
	""tracks"": 					""tracks"",
	""singleTrack"": 				""Single track"",  // First char in upper-case.
	""playlist"": 					""Playlist"",      // First char in upper-case.
	""recordLabel"": 				""Record label"",  // First char in upper-case.
	""user"": 						""User"",          // First char in upper-case.

	""_SECTION5_"":					""=================== LOGIN FORM BUTTONS ==================="",
	""closeAboutButton"":			""CLOSE"",
	""customSaveButton"":			""SAVE"",
	""exitButton"":					""EXIT"",
	""loginButton"":				""LOGIN"",

	""_SECTION6_"":					""=================== LOGIN FORM LABELS ==================="",
	""appidLabel"":					""App ID"",
	""appSecretLabel"":				""App Secret"",
	""customLabel"":				""USE CUSTOM APP ID + SECRET"",

	""_SECTION7_"":					""=================== LOGIN FORM TEXTBOXES ==================="",
	""customInfoTextbox"":			""Leave values blank if you would like to automatically grab the values!"",
	""aboutTextbox"":				""Version - {version}\r\nCreated by AiiR\r\n\r\nInspired By Qo-DL\r\n(Created by Sorrow and DashLt)\r\n\r\nThanks to the users on Github and Telegram for offering bug reports and ideas! And huge shoutout to DJDoubleD for keeping the original running since I've been busy!"",

	""_SECTION8_"":					""=================== LOGIN FORM PLACEHOLDERS ==================="",
	""emailPlaceholder"":			""e-mail"",
	""passwordPlaceholder"":		""password"",
	""tokenPlaceholder"":			""token"",
	""altLoginLabelToken"":			""LOGIN WITH TOKEN"",
	""altLoginLabelEmail"":			""LOGIN WITH E-MAIL AND PASSWORD"",
	""loginTextWaiting"":			""waiting for login..."",
	""loginTextStart"":				""logging in..."",
	""loginTextError"":				""login failed, error log saved"",
	""loginTextNoEmail"":			""no e-mail in input"",
	""loginTextNoPassword"":		""no password/token in input"",
	""updateNotification"":			""New version of QBDLX is available!\r\n\r\nInstalled version - {currentVersion}\r\nLatest version - {newVersion}\r\n\r\nChangelog Below\r\n==============\r\n{changelog}\r\n==============\r\n\r\nWould you like to update?"",
	""updateNotificationTitle"":	""QBDLX | Update Available"",
	
	""_SECTION9_"":					""=================== BATCH DOWNLOAD DIALOG CONTROLS ==================="",
	""batchDownloadDlgText"": 		""Batch Download"",
	""batchDownloadLabel"": 		""Paste one or more Qobuz URLs..."",
	""closeBatchDownloadbutton"": 	""CLOSE / CANCEL"",
	""getAllBatchDownloadButton"":	""GET ALL"",

	""_SECTION10_"":				""=================== CONTEXT MENU ITEMS ==================="",
	""showWindowCmItem"": 			""Show window"",
	""hideWindowCmItem"": 			""Hide window"",
	""closeProgramCmItem"": 		""Close program""
}
";

        public void PopulateLanguageComboBox(qbdlxForm mainForm)
        {
            ComboBox languageComboBox = mainForm.languageComboBox;

            if (!ZlpIOHelper.DirectoryExists(languagesDirectory))
            {
                qbdlxForm._qbdlxForm.logger.Warning("Language directory not found.");
                return;
            }

            // Get all .json files in the languages folder
            var languageFiles = ZlpIOHelper.GetFiles(languagesDirectory, "*.json");

            foreach (var filePath in languageFiles)
            {
                // Extract the language code from file name (e.g., "en" from "en.json")
                var fileName = filePath.NameWithoutExtension;
                languageComboBox.Items.Add(fileName.ToUpper()); // Add language code to combo box
            }
        }

        public void LoadLanguage(string filePath)
        {
            try
            {
                qbdlxForm._qbdlxForm.logger.Info("Loading language from: " + filePath);
                var json = ZlpIOHelper.ReadAllText(filePath);
                languageDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            }
            catch (Exception ex)
            {
                qbdlxForm._qbdlxForm.logger.Error($"Error loading language file: {ex.Message}");
                Debug.WriteLine($"Error loading language file: {ex.Message}");
                languageDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(defaultLanguage);
            }
        }

        public string GetTranslation(string key)
        {
            return languageDictionary.ContainsKey(key) ? languageDictionary[key] : key;
        }

        public void UpdateControlFont(Control.ControlCollection controls, string fontName)
        {
            foreach (Control control in controls)
            {
                if (control is Label || control is Button || control is TextBox)
                {
                    // Keep the original font size
                    float originalSize = control.Font.Size;
                    FontStyle originalStyle = control.Font.Style;

                    control.Font = new Font(fontName, originalSize, originalStyle);
                }

                // Recursively update child controls
                if (control.HasChildren)
                {
                    UpdateControlFont(control.Controls, fontName);
                }
            }
        }
    }
}
