using Newtonsoft.Json;
using QobuzDownloaderX.Properties;
using QobuzDownloaderX.UserControls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using ZetaLongPaths;

namespace QobuzDownloaderX.Helpers
{
    internal sealed class ThemeSettings
    {
        public Dictionary<string, Theme> Themes { get; set; }
        private readonly Dictionary<string, Image> originalImages = new Dictionary<string, Image>();
    }

    internal sealed class Theme
    {
        public string FormBackground { get; set; }
        public string MainPanelBackground { get; set; }
        public string HighlightedButtonBackground { get; set; }
        public string ProgressBarFillColor { get; set; }
        public string SelectedRowBackground { get; set; }
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

    internal sealed class Theming
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
                    MessageBox.Show(qbdlxForm._qbdlxForm.languageManager.GetTranslation("themeNameNotFoundMsg").Replace("{themeName}", themeName), Application.ProductName);
                }
            }
            else
            {
                MessageBox.Show(qbdlxForm._qbdlxForm.languageManager.GetTranslation("themeFileNotFoundMsg"), Application.ProductName);
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
                MessageBox.Show(qbdlxForm._qbdlxForm.languageManager.GetTranslation("themeFileNotFoundMsg"), Application.ProductName);
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
                else if (control is CheckedListBox clb)
                {
                    clb.BackColor = ColorTranslator.FromHtml(_currentTheme.FormBackground);
                    clb.ForeColor = ColorTranslator.FromHtml(_currentTheme.LabelText);
                }
                else if (control is TextBox textBox)
                {
                    textBox.ForeColor = ColorTranslator.FromHtml(_currentTheme.TextBoxText);

                    // Apply specific colors for specific panels if needed
                    textBox.BackColor = (textBox.Name == "userInfoTextbox" || textBox.Name == "emailTextbox" || textBox.Name == "passwordTextbox" || textBox.Name == "appidTextbox" || textBox.Name == "appSecretTextbox")
                        ? ColorTranslator.FromHtml(_currentTheme.MainPanelBackground)
                        : ColorTranslator.FromHtml(_currentTheme.TextBoxBackground);

                    textBox.ForeColor = (textBox.Text == "Paste a Qobuz URL…" || textBox.Text == "Input your search…" || textBox.Text == "e-mail" || textBox.Text == "password" || textBox.Text == "token")
                        ? ColorTranslator.FromHtml(_currentTheme.PlaceholderTextBoxText)
                        : ColorTranslator.FromHtml(_currentTheme.TextBoxText);
                }
                else if (control is CustomProgressBar progressBar)
                {
                    progressBar.BackgroundColor = ColorTranslator.FromHtml(_currentTheme.TextBoxBackground);
                    progressBar.ForeColor = ColorTranslator.FromHtml(_currentTheme.TextBoxText);
                    progressBar.BorderColor = progressBar.BackgroundColor;
                    progressBar.FillColor = ColorTranslator.FromHtml(_currentTheme.ProgressBarFillColor);

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
                    if (panel.Tag is RowInfo)
                    {
                        // Get the inner table (innerRow)
                        foreach (Control child in panel.Controls)
                        {
                            if (child is TableLayoutPanel innerRow)
                            {
                                foreach (Control innerChild in innerRow.Controls)
                                {
                                    if (innerChild is Label lbl)
                                    {
                                        lbl.ForeColor = ColorTranslator.FromHtml(_currentTheme.LabelText);
                                    }
                                    else if (innerChild is Button btn)
                                    {
                                        btn.ForeColor = ColorTranslator.FromHtml(_currentTheme.ButtonText);
                                        btn.BackColor = ColorTranslator.FromHtml(_currentTheme.ButtonBackground);

                                        btn.FlatStyle = FlatStyle.Flat;
                                        btn.FlatAppearance.BorderSize = 0;
                                        btn.FlatAppearance.MouseOverBackColor =
                                            ColorTranslator.FromHtml(_currentTheme.HighlightedButtonBackground);
                                        btn.FlatAppearance.MouseDownBackColor =
                                            ColorTranslator.FromHtml(_currentTheme.ClickedButtonBackground);
                                    }
                                }
                            }
                        }

                        // no ApplyTheme recursion for RowPanels
                        continue;
                    }

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

    internal sealed class LanguageManager
    {
        private Dictionary<string, string> languageDictionary;
        public string languagesDirectory = "languages";

        // Default English translation if no files are avaialble
        public const string defaultLanguage = @"{
    ""TranslationCredit"":            ""AiiR"",
    ""TranslationUpdatedOn"":         ""January 06, 2026, 09:22AM EST"",
    ""TranslationFont"":              ""Nirmala UI"",

    ""_SECTION1_"":                   ""=================== MAIN FORM BUTTONS ==================="",
    ""additionalSettingsButton"":     ""Additional Settings"",
    ""aboutButton"":                  ""ABOUT…"",
    ""closeAdditionalButton"":        ""Back to Settings"",
    ""downloadButton"":               ""GET"",
    ""batchDownloadButton"":          ""GET BATCH"",
    ""abortButton"":                  ""ABORT"",
    ""skipButton"":                   ""SKIP"",
    ""downloaderButton"":             ""DOWNLOADER"",
    ""logoutButton"":                 ""LOGOUT"",
    ""openFolderButton"":             ""Open Folder"",
    ""qualitySelectButton"":          ""Quality Selector"",
    ""saveTemplatesButton"":          ""Save"",
    ""resetTemplatesButton"":         ""Reset"",
    ""searchButton"":                 ""SEARCHER"",
    ""searchAlbumsButton"":           ""RELEASES"",
    ""searchTracksButton"":           ""TRACKS"",
    ""selectFolderButton"":           ""Select Folder"",
    ""settingsButton"":               ""SETTINGS"",
    ""selectAllRowsButton"":          ""Select all rows"",
    ""deselectAllRowsButton"":        ""Deselect all rows"",
    ""batchDownloadRowsButton"":      ""BATCH DOWNLOAD SELECTED ROWS"",

    ""_SECTION2_"":                   ""=================== MAIN FORM LABELS ==================="",
    ""advancedOptionsLabel"":         ""ADVANCED OPTIONS"",
    ""albumTemplateLabel"":           ""ALBUM TEMPLATE"",
    ""artistTemplateLabel"":          ""ARTIST TEMPLATE"",
    ""commentLabel"":                 ""Custom Comment"",
    ""downloadFolderLabel"":          ""DOWNLOAD FOLDER"",
    ""downloadOptionsLabel"":         ""DOWNLOAD OPTIONS"",
    ""embeddedArtLabel"":             ""Embedded Artwork Size"",
    ""extraSettingsLabel"":           ""ADDITIONAL SETTINGS"",
    ""languageLabel"":                ""Current Language"",
    ""playlistTemplateLabel"":        ""PLAYLIST TEMPLATE"",
    ""favoritesTemplateLabel"":       ""FAVORITES TEMPLATE"",
    ""savedArtLabel"":                ""Saved Artwork Size"",
    ""searchingLabel"":               ""Searching…"",
    ""taggingOptionsLabel"":          ""TAGGING OPTIONS"",
    ""templatesLabel"":               ""TEMPLATES"",
    ""templatesListLabel"":           ""TEMPLATES LIST"",
    ""themeLabel"":                   ""Current Theme"",
    ""themeSectionLabel"":            ""THEMING OPTIONS"",
    ""trackTemplateLabel"":           ""TRACK TEMPLATE"",
    ""userInfoLabel"":                ""USER INFO"",
    ""welcomeLabel"":                 ""Welcome\r\n{username}"",
    ""limitSearchResultsLabel"":      ""Results Limit:"",
    ""searchSortingLabel"":           ""Sort By:"",
    ""sortReleaseDateLabel"":         ""Release Date"",
    ""sortGenreLabel"":               ""Genre"",
    ""sortArtistNameLabel"":          ""Artist Name"",
    ""sortAlbumTrackNameLabel"":      ""Album / Track Name"",
    ""sortingSearchResultsLabel"":    ""Sorting…"",
    ""searchResultsCountLabel"":      ""results"",
    ""selectedRowsCountLabel"":       ""{0} selected rows"",
    ""disclaimer"":                   ""DISCLAIMER / LEGAL NOTICE\n\nThis application uses the Qobuz API but is not certified by Qobuz.\n\nThe software and its authors are not affiliated with, endorsed by, or officially connected to Qobuz in any way. Use of this application is at your own risk. The authors make no warranties regarding the accuracy, reliability, or availability of the service, and will not be held liable for any damages or data loss resulting from its use.\n\nBy using this application, you acknowledge and accept that it is provided \""as-is\"", without any express or implied warranty of any kind, including but not limited to warranties of merchantability, fitness for a particular purpose, or non-infringement.\n\nUsers are responsible for complying with all applicable laws, terms of service, and usage restrictions when accessing the Qobuz API or any other third-party services.\n\nFor official Qobuz services and support, please refer to Qobuz's official website: https://www.qobuz.com/"",
    ""downloadFromArtistLabel"":      ""DOWNLOAD FROM ARTIST"",
    
    ""_SECTION3_"":                   ""=================== MAIN FORM CHECKBOXES ==================="",
    ""albumArtistCheckbox"":          ""Album Artist"",
    ""albumTitleCheckbox"":           ""Album Title"",
    ""trackArtistCheckbox"":          ""Track Artist"",
    ""trackTitleCheckbox"":           ""Track Title"",
    ""releaseDateCheckbox"":          ""Release Date"",
    ""releaseTypeCheckbox"":          ""Release Type"",
    ""genreCheckbox"":                ""Genre"",
    ""trackNumberCheckbox"":          ""Track Number"",
    ""trackTotalCheckbox"":           ""Total Tracks"",
    ""discNumberCheckbox"":           ""Disc Number"",
    ""discTotalCheckbox"":            ""Total Discs"",
    ""composerCheckbox"":             ""Composer"",
    ""explicitCheckbox"":             ""Explicit Advisory"",
    ""coverArtCheckbox"":             ""Cover Art"",
    ""copyrightCheckbox"":            ""Copyright"",
    ""labelCheckbox"":                ""Label"",
    ""upcCheckbox"":                  ""UPC / Barcode"",
    ""isrcCheckbox"":                 ""ISRC"",
    ""urlCheckbox"":                  ""URL"",
    ""mergeArtistNamesCheckbox"":     ""Merge track artist names.\nExample:\nArtist1 && Artist2 Feat. Artist3 - Title.mp3"",
    ""streamableCheckbox"":           ""Streamable Check"",
    ""fixMD5sCheckbox"":              ""Auto-Fix Unset MD5s (must have FLAC in PATH variables)"",
    ""downloadSpeedCheckbox"":        ""Print Download Speed"",
    ""sortAscendantCheckBox"":        ""Ascendant"",
    ""downloadGoodiesCheckbox"":      ""Download goodies"",
    ""useTLS13Checkbox"":             ""Enable TLS 1.3"",
    ""downloadArtistOtherCheckBox"":  ""Download artist - Other / covers"",
    ""clearOldLogsCheckBox"":         ""Clear old log files on startup"",
    ""dontSaveArtworkToDiskCheckBox"":""Don't save artwork to disk"",
    ""downloadFromArtistListBox"":    ""Album, EP/Single, Live, Compilation, Download, Other"",
    ""downloadAllFromArtistCheckBox"":""Download all from artist (it can include more)"",

    ""_SECTION4_"":                   ""=================== MAIN FORM PLACEHOLDERS ==================="",
    ""albumLabelPlaceholder"":        ""Welcome to QBDLX!"",
    ""artistLabelPlaceholder"":       ""Input your Qobuz link and hit GET!"",
    ""infoLabelPlaceholder"":         ""Released"",
    ""inputTextboxPlaceholder"":      ""Paste a Qobuz URL…"",
    ""searchTextboxPlaceholder"":     ""Input your search…"",
    ""downloadFolderPlaceholder"":    ""No folder selected"",
    ""userInfoTextboxPlaceholder"":   ""User ID = {user_id}\r\nE-mail = {user_email}\r\nCountry = {user_country}\r\nSubscription = {user_subscription}\r\nExpires = {user_subscription_expiration}"",
    ""downloadOutputWelcome"":        ""Welcome {user_display_name}!"",
    ""downloadOutputExpired"":        ""YOUR SUBSCRIPTION HAS EXPIRED, DOWNLOADS WILL BE LIMITED TO 30 SECOND SNIPPETS!"",
    ""downloadOutputPath"":           ""Download Path:"",
    ""downloadOutputNoPath"":         ""No path has been set! Remember to Choose a Folder!"",
    ""downloadOutputNoUrl"":          ""Track {TrackNumber} is not available for download. Skipping."",
    ""downloadOutputAPIError"":       ""Qobuz API error. Maybe release isn't available in this account region?"",
    ""downloadOutputNotImplemented"": ""Not implemented yet or the URL is not understood. Is there a typo?"",
    ""downloadOutputCheckLink"":      ""Checking Link…"",
    ""downloadOutputTrNotStream"":    ""Track {TrackNumber} is not available for streaming. Skipping."",
    ""downloadOutputAlNotStream"":    ""Release is not available for streaming."",
    ""downloadOutputGoodyFound"":     ""Goody found, downloading…"",
    ""downloadOutputGoodyExists"":    ""File for goody already exists"",
    ""downloadOutputGoodyNoURL"":     ""No download URL found for goody, skipping"",
    ""downloadOutputFileExists"":     ""File for track {TrackNumber} already exists, skipping."",
    ""downloadOutputDownloading"":    ""Downloading"",
    ""downloadOutputDone"":           ""DONE"",
    ""downloadOutputCompleted"":      ""DOWNLOAD COMPLETE"",
    ""downloadAborted"":              ""DOWNLOAD ABORTED BY USER."",
    ""albumSkipped"":                 ""ALBUM SKIPPED BY USER."",
    ""progressLabelInactive"":        ""No download active"",
    ""progressLabelActive"":          ""Download progress"",
    ""formClosingWarning"":           ""A download is in progress, do you really want to quit?."",
    ""artist"":                       ""Artist"",
    ""artists"":                      ""artists"",
    ""album"":                        ""Album"",
    ""albums"":                       ""albums"",
    ""tracks"":                       ""tracks"",
    ""singleTrack"":                  ""Single track"",
    ""playlist"":                     ""Playlist"",
    ""recordLabel"":                  ""Record label"",
    ""user"":                         ""User"",
    ""invalidUrl"":                   ""Invalid URL: {0}"",
    ""copyToClipboard"":              ""Copy to clipboard"",
    ""copyThisRowToClipboard"":       ""Copy this row to clipboard"",
    ""copySelectedRowsToClipboard"":  ""Copy selected rows to clipboard"",
    ""copyAllRowsToClipboard"":       ""Copy all rows to clipboard"",
    ""downloadFromArtistWarning"":    ""Please, select at least one download type for artists"",

    ""_SECTION5_"":                   ""=================== LOGIN FORM BUTTONS ==================="",
    ""closeAboutButton"":             ""CLOSE"",
    ""customSaveButton"":             ""SAVE"",
    ""exitButton"":                   ""EXIT"",
    ""loginButton"":                  ""LOGIN"",

    ""_SECTION6_"":                   ""=================== LOGIN FORM LABELS ==================="",
    ""appidLabel"":                   ""App ID"",
    ""appSecretLabel"":               ""App Secret"",
    ""customLabel"":                  ""USE CUSTOM APP ID + SECRET"",

    ""_SECTION7_"":                   ""=================== LOGIN FORM TEXTBOXES ==================="",
    ""customInfoTextbox"":            ""Leave values blank if you would like to automatically grab the values!"",
    ""aboutTextbox"":                 ""Version - {version}\r\nCreated by AiiR\r\n\r\nInspired By Qo-DL\r\n(Created by Sorrow and DashLt)\r\n\r\nThanks to the users on Github and Telegram for offering bug reports and ideas! And huge shoutout to DJDoubleD for keeping the original running since I've been busy!"",

    ""_SECTION8_"":                   ""=================== LOGIN FORM PLACEHOLDERS ==================="",
    ""emailPlaceholder"":             ""e-mail"",
    ""passwordPlaceholder"":          ""password"",
    ""tokenPlaceholder"":             ""token"",
    ""altLoginLabelToken"":           ""LOGIN WITH TOKEN"",
    ""altLoginLabelEmail"":           ""LOGIN WITH E-MAIL AND PASSWORD"",
    ""loginTextWaiting"":             ""waiting for login…"",
    ""loginTextStart"":               ""logging in…"",
    ""loginTextError"":               ""login failed, error log saved"",
    ""loginTextNoEmail"":             ""no e-mail in input"",
    ""loginTextNoPassword"":          ""no password/token in input"",
    ""updateNotification"":           ""New version of QBDLX is available!\r\n\r\nInstalled version - {currentVersion}\r\nLatest version - {newVersion}\r\n\r\nChangelog Below\r\n==============\r\n{changelog}\r\n==============\r\n\r\nWould you like to update?"",
    ""updateNotificationTitle"":      ""QBDLX | Update Available"",

    ""_SECTION9_"":                   ""=================== BATCH DOWNLOAD DIALOG CONTROLS ==================="",
    ""batchDownloadDlgText"":         ""Batch Download"",
    ""batchDownloadLabel"":           ""Paste one or more Qobuz URLs…"",
    ""closeBatchDownloadbutton"":     ""CLOSE / CANCEL"",
    ""getAllBatchDownloadButton"":    ""GET ALL"",

    ""_SECTION10_"":                  ""=================== SYSTRAY ICON ==================="",
    ""showWindowCmItem"":             ""Show window"",
    ""hideWindowCmItem"":             ""Hide window"",
    ""closeProgramCmItem"":           ""Close program"",
    ""batchDownloadFinished"":        ""Batch download finished!"",

    ""_SECTION11_"":                  ""=================== MESSAGE BOXES ==================="",
    ""tagLibSharpMissingMsg"":        ""taglib-sharp.dll missing from folder!\\r\\nPlease Make sure the DLL is in the same folder as {exeName}!"",
    ""themeFileNotFoundMsg"":         ""Visual theme file not found."",
    ""themeNameNotFoundMsg"":         ""Visual theme '{themeName}' not found."",
    ""selectedLangFileNotfoundMsg"":  ""Selected language file not found."",
    ""downloadOutputDontExistMsg"":   ""The specified path does not exist."",
    ""notEnoughFreeSpaceMsg"":        ""Not enough free space on drive: '{pathRoot}'""
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
