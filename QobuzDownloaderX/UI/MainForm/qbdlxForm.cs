
using QobuzDownloaderX.Helpers;
using QobuzDownloaderX.Properties;
using QobuzDownloaderX.UI;
using QobuzDownloaderX.Win32;
using QopenAPI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZetaLongPaths;

namespace QobuzDownloaderX
{
    public partial class qbdlxForm : Form
    {
        // Create logger for this form
        public BufferedLogger logger { get; set; }

        // Create theme and language options
        public Theme theme { get; set; }
        public LanguageManager languageManager;

        public Service QoService = new Service();
        public User QoUser = new User();
        public Artist QoArtist = new Artist();
        public Album QoAlbum = new Album();
        public Item QoItem = new Item();
        public SearchAlbumResult QoAlbumSearch = new SearchAlbumResult();
        public SearchTrackResult QoTrackSearch = new SearchTrackResult();
        public Favorites QoFavorites = new Favorites();
        public Playlist QoPlaylist = new Playlist();
        public QopenAPI.Label QoLabel = new QopenAPI.Label();
        public QopenAPI.Stream QoStream = new QopenAPI.Stream();

        public bool downloadPanelActive = false;
        public bool aboutPanelActive = false;
        public bool settingsPanelActive = false;
        private bool firstLoadComplete = false;

        public string downloadLocation { get; set; }
        public string artistTemplate { get; set; }
        public string albumTemplate { get; set; }
        public string trackTemplate { get; set; }
        public string playlistTemplate { get; set; }
        public string favoritesTemplate { get; set; }

        public string app_id { get; set; }
        public string app_secret { get; set; }
        public string user_id { get; set; }
        public string user_auth_token { get; set; }
        public string user_display_name { get; set; }
        public string user_label { get; set; }
        public string user_avatar { get; set; }

        public string qobuz_id { get; set; }
        public string format_id { get; set; }
        public string audio_format { get; set; }

        public string embeddedArtSize { get; set; }
        public string savedArtSize { get; set; }
        public string themeName { get; set; }

        // Used to signal and manage 'getLinkTypeAsync' cancellation.
        private CancellationTokenSource abortTokenSource;

        // Local flag that indicates whether 'getLinkTypeAsync' is executing.
        public bool getLinkTypeIsBusy;

        // Global flag that indicates whether a batch download is running.
        public static bool isBatchDownloadRunning;

        // Global flag that indicates whether the current album download must be skipped in the current 'getLinkTypeAsync' execution.
        public static bool skipCurrentAlbum;

        // Global flag that keeps track of the last current taskbar progress value to restore it when minimizing and restoring the main form.
        public static int lastTaskBarProgressCurrentValue;

        // Global flag that keeps track of the last maximum taskbar progress value to restore it when minimizing and restoring the main form.
        public static int lastTaskBarProgressMaxValue;

        // Global flag that keeps track of the last taskbar progress state to restore it when minimizing and restoring the main form.
        public static TaskbarProgressState lastTaskBarProgressState;

        public static NotifyIconProgressBar ntfyProgressBar = new NotifyIconProgressBar { 
            Height = 8,
            BorderColor = Color.Black,
            BorderWidth = 1
        };

        // used for triple-click / select all text support.
        private int clickCount = 0;
        private DateTime lastClickTime = DateTime.MinValue;
        private readonly TimeSpan tripleClickThreshold = TimeSpan.FromMilliseconds(SystemInformation.DoubleClickTime);

        #region Language
        public string userInfoTextboxPlaceholder { get; set; }
        public string albumLabelPlaceholder { get; set; }
        public string artistLabelPlaceholder { get; set; }
        public string infoLabelPlaceholder { get; set; }
        public string inputTextboxPlaceholder { get; set; }
        public string searchTextboxPlaceholder { get; set; }
        public string downloadFolderPlaceholder { get; set; }
        public string downloadOutputWelcome { get; set; }
        public string downloadOutputExpired { get; set; }
        public string downloadOutputPath { get; set; }
        public string downloadOutputNoPath { get; set; }
        public string downloadOutputNoUrl { get; set; }
        public string downloadOutputAPIError { get; set; }
        public string downloadOutputNotImplemented { get; set; }
        public string downloadOutputCheckLink { get; set; }
        public string downloadOutputTrNotStream { get; set; }
        public string downloadOutputAlNotStream { get; set; }
        public string downloadOutputGoodyFound { get; set; }
        public string downloadOutputGoodyExists { get; set; }
        public string downloadOutputGoodyNoURL { get; set; }
        public string downloadOutputFileExists { get; set; }
        public string downloadOutputDownloading { get; set; }
        public string downloadOutputDone { get; set; }
        public string downloadOutputCompleted { get; set; }
        public string progressLabelInactive { get; set; }
        public string progressLabelActive { get; set; }
        public string formClosingWarning { get; set; }
        public string downloadAborted { get; set; }
        public string albumSkipped { get; set; }
        #endregion

        readonly GetInfo getInfo = new GetInfo();
        readonly RenameTemplates renameTemplates = new RenameTemplates();
        readonly DownloadAlbum downloadAlbum = new DownloadAlbum();
        readonly DownloadTrack downloadTrack = new DownloadTrack();
        readonly SearchPanelHelper searchPanelHelper = new SearchPanelHelper();

        readonly Regex qobuzStoreLinkRegex = new Regex(
            @"https:\/\/(?:.*?).qobuz.com\/(?<region>.*?)\/(?<type>.*?)\/(?<name>.*?)\/(?<id>.*?)$", RegexOptions.Compiled);

        readonly Regex qobuzLinkIdGrabRegex = new Regex(
            @"https:\/\/(?:.*?).qobuz.com\/(?<type>.*?)\/(?<id>.*?)$", RegexOptions.Compiled);

        private readonly Regex qobuzUrlRegEx = new Regex(
         @"^https?:\/\/(?!.*https?:\/\/)(?:[\w\-]+\.)?qobuz\.com\/[^\s]+$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        // Allows to minimize/restore the form by clicking on the taskbar icon.
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.Style |= Constants.WS_MINIMIZEBOX;
                cp.ClassStyle |= Constants.CS_DBLCLKS;
                return cp;
            }
        }

        public qbdlxForm()
        {
            // Create new log file
            Directory.CreateDirectory("logs");
            logger = new BufferedLogger("logs\\QobuzDLX " + DateTime.Now.ToString("yyyy⧸MM⧸dd HH꞉mm꞉ss") + ".log");
            logger.Debug("Logger started, QBDLX form initialized!");

            InitializeComponent();
            _qbdlxForm = this;
        }

        public static qbdlxForm _qbdlxForm;
        public readonly Theming _themeManager = new Theming();

        public void update(string text)
        {
            downloadOutput.Invoke(new Action(() => downloadOutput.Text = text));
        }

        public void updateTemplates()
        {
            logger.Debug("Updating templates");
            artistTemplate = artistTemplateTextbox.Text;
            albumTemplate = albumTemplateTextbox.Text;
            trackTemplate = trackTemplateTextbox.Text;
            playlistTemplate = playlistTemplateTextbox.Text;
            favoritesTemplate = favoritesTemplateTextbox.Text;
        }

        private void LoadSavedTemplates()
        {
            artistTemplateTextbox.Text = Settings.Default.savedArtistTemplate;
            albumTemplateTextbox.Text = Settings.Default.savedAlbumTemplate;
            trackTemplateTextbox.Text = Settings.Default.savedTrackTemplate;
            playlistTemplateTextbox.Text = Settings.Default.savedPlaylistTemplate;
            favoritesTemplateTextbox.Text = Settings.Default.savedFavoritesTemplate;
            updateTemplates();
        }

        private void LoadQualitySettings()
        {
            mp3Button2.Checked = Settings.Default.quality1;
            flacLowButton2.Checked = Settings.Default.quality2;
            flacMidButton2.Checked = Settings.Default.quality3;
            flacHighButton2.Checked = Settings.Default.quality4;
            format_id = Settings.Default.qualityFormat;
            audio_format = Settings.Default.audioType;
        }

        private void LoadTaggingSettings()
        {
            albumTitleCheckbox.Checked = Settings.Default.albumTag;
            albumArtistCheckbox.Checked = Settings.Default.albumArtistTag;
            trackArtistCheckbox.Checked = Settings.Default.artistTag;
            composerCheckbox.Checked = Settings.Default.composerTag;
            copyrightCheckbox.Checked = Settings.Default.copyrightTag;
            labelCheckbox.Checked = Settings.Default.labelTag;
            discNumberCheckbox.Checked = Settings.Default.discTag;
            discTotalCheckbox.Checked = Settings.Default.totalDiscsTag;
            genreCheckbox.Checked = Settings.Default.genreTag;
            isrcCheckbox.Checked = Settings.Default.isrcTag;
            urlCheckbox.Checked = Settings.Default.urlTag;
            mergeArtistNamesCheckbox.Checked = Settings.Default.mergeArtistNames;
            releaseTypeCheckbox.Checked = Settings.Default.typeTag;
            explicitCheckbox.Checked = Settings.Default.explicitTag;
            trackTitleCheckbox.Checked = Settings.Default.trackTitleTag;
            trackNumberCheckbox.Checked = Settings.Default.trackTag;
            trackTotalCheckbox.Checked = Settings.Default.totalTracksTag;
            upcCheckbox.Checked = Settings.Default.upcTag;
            releaseDateCheckbox.Checked = Settings.Default.yearTag;
            coverArtCheckbox.Checked = Settings.Default.imageTag;
            commentCheckbox.Checked = Settings.Default.commentTag;
            commentTextbox.Text = Settings.Default.commentText;
            embeddedArtSizeSelect.SelectedIndex = Settings.Default.savedEmbeddedArtSize;
            savedArtSizeSelect.SelectedIndex = Settings.Default.savedSavedArtSize;
        }
        private void LoadOtherSettings()
        {
            streamableCheckbox.Checked = Settings.Default.streamableCheck;
            useTLS13Checkbox.Checked = Settings.Default.useTLS13;
            fixMD5sCheckbox.Checked = Settings.Default.fixMD5s;
            downloadGoodiesCheckbox.Checked = Settings.Default.downloadGoodies;
            downloadArtistOtherCheckbox.Checked = Settings.Default.downloadArtistOther;
            downloadSpeedCheckbox.Checked = Settings.Default.showDownloadSpeed;
        }

        private void SetDownloadPath()
        {
            downloadLocation = Settings.Default.savedFolder;
            downloadFolderTextbox.Text = !string.IsNullOrEmpty(downloadLocation) ? downloadLocation : downloadFolderPlaceholder;
            folderBrowser.SelectedPath = downloadLocation;
            logger.Info("Saved download path: " + folderBrowser.SelectedPath);
        }

        private void InitializePanels()
        {
            // Set all panels to specific point
            var panelPosition = new Point(179, 0);
            downloaderPanel.Location = panelPosition;
            aboutPanel.Location = panelPosition;
            settingsPanel.Location = panelPosition;
            extraSettingsPanel.Location = panelPosition;
            searchPanel.Location = panelPosition;

            // Startup with downloadPanel active and visable, all others not visable
            downloaderPanel.Visible = true;
            aboutPanel.Visible = false;
            settingsPanel.Visible = false;
            downloadPanelActive = true;
            downloaderButton.BackColor = ColorTranslator.FromHtml(_themeManager._currentTheme.HighlightedButtonBackground);
        }

        private void InitializeTheme()
        {
            // Populate theme options in settings
            _themeManager.PopulateThemeOptions(this);

            // Set and load theme
            themeName = Settings.Default.currentTheme;
            if (!string.IsNullOrEmpty(themeName)) { themeComboBox.SelectedItem = themeName; }
            theme = _themeManager._currentTheme;
        }

        private void InitializeLanguage()
        {
            // Set saved language
            languageManager = new LanguageManager();
            languageManager.LoadLanguage($"languages/{Settings.Default.currentLanguage.ToLower()}.json");

            // Populate theme options in settings
            languageManager.PopulateLanguageComboBox(this);
            languageComboBox.SelectedItem = Settings.Default.currentLanguage.ToUpper();

            // Load theme
            UpdateUILanguage();
        }

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
            additionalSettingsButton.Text = languageManager.GetTranslation("additionalSettingsButton");
            aboutButton.Text = languageManager.GetTranslation("aboutButton");
            closeAdditionalButton.Text = languageManager.GetTranslation("closeAdditionalButton");
            downloadButton.Text = languageManager.GetTranslation("downloadButton");
            batchDownloadButton.Text = languageManager.GetTranslation("batchDownloadButton");
            abortButton.Text = languageManager.GetTranslation("abortButton");
            skipButton.Text = languageManager.GetTranslation("skipButton");
            downloaderButton.Text = languageManager.GetTranslation("downloaderButton");
            logoutButton.Text = languageManager.GetTranslation("logoutButton");
            openFolderButton.Text = languageManager.GetTranslation("openFolderButton");
            qualitySelectButton.Text = languageManager.GetTranslation("qualitySelectButton");
            resetTemplatesButton.Text = languageManager.GetTranslation("resetTemplatesButton");
            saveTemplatesButton.Text = languageManager.GetTranslation("saveTemplatesButton");
            searchButton.Text = languageManager.GetTranslation("searchButton");
            searchAlbumsButton.Text = languageManager.GetTranslation("searchAlbumsButton");
            searchTracksButton.Text = languageManager.GetTranslation("searchTracksButton");
            selectFolderButton.Text = languageManager.GetTranslation("selectFolderButton");
            settingsButton.Text = languageManager.GetTranslation("settingsButton");
            closeBatchDownloadbutton.Text = languageManager.GetTranslation("closeBatchDownloadbutton");
            getAllBatchDownloadButton.Text = languageManager.GetTranslation("getAllBatchDownloadButton");
            selectAllRowsButton.Text = languageManager.GetTranslation("selectAllRowsButton");
            deselectAllRowsButton.Text = languageManager.GetTranslation("deselectAllRowsButton");
            batchDownloadSelectedRowsButton.Text = languageManager.GetTranslation("batchDownloadRowsButton");

            /* Center additional settings button to center of panel */
            additionalSettingsButton.Location = new Point((settingsPanel.Width - additionalSettingsButton.Width) / 2, additionalSettingsButton.Location.Y);

            /* Center quality panel to center of quality button */
            qualitySelectPanel.Location = new Point(qualitySelectButton.Left + (qualitySelectButton.Width / 2) - (qualitySelectPanel.Width / 2), qualitySelectPanel.Location.Y);

            // Labels
            aboutLabel.Text = languageManager.GetTranslation("aboutButton") + "                                                                                                 ";
            advancedOptionsLabel.Text = languageManager.GetTranslation("advancedOptionsLabel");
            albumTemplateLabel.Text = languageManager.GetTranslation("albumTemplateLabel");
            artistTemplateLabel.Text = languageManager.GetTranslation("artistTemplateLabel");
            commentLabel.Text = languageManager.GetTranslation("commentLabel");
            downloadLabel.Text = languageManager.GetTranslation("downloaderButton") + "                                                                                         ";
            downloadFolderLabel.Text = languageManager.GetTranslation("downloadFolderLabel");
            downloadOptionsLabel.Text = languageManager.GetTranslation("downloadOptionsLabel");
            embeddedArtLabel.Text = languageManager.GetTranslation("embeddedArtLabel");
            extraSettingsLabel.Text = languageManager.GetTranslation("extraSettingsLabel");
            languageLabel.Text = languageManager.GetTranslation("languageLabel");
            playlistTemplateLabel.Text = languageManager.GetTranslation("playlistTemplateLabel");
            favoritesTemplateLabel.Text = languageManager.GetTranslation("favoritesTemplateLabel");
            savedArtLabel.Text = languageManager.GetTranslation("savedArtLabel");
            searchLabel.Text = languageManager.GetTranslation("searchButton") + "                                                                                               ";
            searchingLabel.Text = languageManager.GetTranslation("searchingLabel");
            settingsLabel.Text = languageManager.GetTranslation("settingsButton") + "                                                                                           ";
            taggingOptionsLabel.Text = languageManager.GetTranslation("taggingOptionsLabel");
            templatesLabel.Text = languageManager.GetTranslation("templatesLabel");
            templatesListLabel.Text = languageManager.GetTranslation("templatesListLabel");
            themeLabel.Text = languageManager.GetTranslation("themeLabel");
            themeSectionLabel.Text = languageManager.GetTranslation("themeSectionLabel");
            trackTemplateLabel.Text = languageManager.GetTranslation("trackTemplateLabel");
            userInfoLabel.Text = languageManager.GetTranslation("userInfoLabel");
            disclaimerLabel.Text = languageManager.GetTranslation("disclaimer");
            welcomeLabel.Text = languageManager.GetTranslation("welcomeLabel");
            batchDownloadLabel.Text = languageManager.GetTranslation("batchDownloadLabel");
            limitSearchResultsLabel.Text = languageManager.GetTranslation("limitSearchResultsLabel");
            searchSortingLabel.Text = languageManager.GetTranslation("searchSortingLabel");
            sortReleaseDateLabel.Text = languageManager.GetTranslation("sortReleaseDateLabel");
            sortGenreLabel.Text = languageManager.GetTranslation("sortGenreLabel");
            sortArtistNameLabel.Text = languageManager.GetTranslation("sortArtistNameLabel");
            sortAlbumTrackNameLabel.Text = languageManager.GetTranslation("sortAlbumTrackNameLabel");
            sortingSearchResultsLabel.Text = languageManager.GetTranslation("sortingSearchResultsLabel");

            // Checkboxes
            albumArtistCheckbox.Text = languageManager.GetTranslation("albumArtistCheckbox");
            albumTitleCheckbox.Text = languageManager.GetTranslation("albumTitleCheckbox");
            trackArtistCheckbox.Text = languageManager.GetTranslation("trackArtistCheckbox");
            trackTitleCheckbox.Text = languageManager.GetTranslation("trackTitleCheckbox");
            releaseDateCheckbox.Text = languageManager.GetTranslation("releaseDateCheckbox");
            releaseTypeCheckbox.Text = languageManager.GetTranslation("releaseTypeCheckbox");
            genreCheckbox.Text = languageManager.GetTranslation("genreCheckbox");
            trackNumberCheckbox.Text = languageManager.GetTranslation("trackNumberCheckbox");
            trackTotalCheckbox.Text = languageManager.GetTranslation("trackTotalCheckbox");
            discNumberCheckbox.Text = languageManager.GetTranslation("discNumberCheckbox");
            discTotalCheckbox.Text = languageManager.GetTranslation("discTotalCheckbox");
            composerCheckbox.Text = languageManager.GetTranslation("composerCheckbox");
            explicitCheckbox.Text = languageManager.GetTranslation("explicitCheckbox");
            coverArtCheckbox.Text = languageManager.GetTranslation("coverArtCheckbox");
            copyrightCheckbox.Text = languageManager.GetTranslation("copyrightCheckbox");
            labelCheckbox.Text = languageManager.GetTranslation("labelCheckbox");
            upcCheckbox.Text = languageManager.GetTranslation("upcCheckbox");
            isrcCheckbox.Text = languageManager.GetTranslation("isrcCheckbox");
            urlCheckbox.Text = languageManager.GetTranslation("urlCheckbox");
            mergeArtistNamesCheckbox.Text = languageManager.GetTranslation("mergeArtistNamesCheckbox");
            streamableCheckbox.Text = languageManager.GetTranslation("streamableCheckbox");
            fixMD5sCheckbox.Text = languageManager.GetTranslation("fixMD5sCheckbox");
            downloadSpeedCheckbox.Text = languageManager.GetTranslation("downloadSpeedCheckbox");
            sortAscendantCheckBox.Text = languageManager.GetTranslation("sortAscendantCheckBox");
            downloadGoodiesCheckbox.Text = languageManager.GetTranslation("downloadGoodiesCheckbox");
            downloadArtistOtherCheckbox.Text = languageManager.GetTranslation("downloadArtistOther");
            useTLS13Checkbox.Text = languageManager.GetTranslation("useTLS13Checkbox");

            /* Center certain checkboxes in panels */
            fixMD5sCheckbox.Location = new Point((extraSettingsPanel.Width - fixMD5sCheckbox.Width) / 2, fixMD5sCheckbox.Location.Y);
            downloadSpeedCheckbox.Location = new Point((extraSettingsPanel.Width - downloadSpeedCheckbox.Width) / 2, downloadSpeedCheckbox.Location.Y);
           
            streamableCheckbox.Location = new Point(fixMD5sCheckbox.Left - 100, streamableCheckbox.Location.Y);
            useTLS13Checkbox.Location = new Point(streamableCheckbox.Right + 16, streamableCheckbox.Location.Y);
            downloadGoodiesCheckbox.Location = new Point(useTLS13Checkbox.Right + 16, streamableCheckbox.Location.Y);
            downloadArtistOtherCheckbox.Location = new Point(downloadGoodiesCheckbox.Right + 16, streamableCheckbox.Location.Y);

            // Context menu items
            showWindowToolStripMenuItem.Text = languageManager.GetTranslation("showWindowCmItem");
            hideWindowToolStripMenuItem.Text = languageManager.GetTranslation("hideWindowCmItem");
            closeProgramToolStripMenuItem.Text = languageManager.GetTranslation("closeProgramCmItem");

            // Placeholders
            albumLabelPlaceholder = languageManager.GetTranslation("albumLabelPlaceholder");
            artistLabelPlaceholder = languageManager.GetTranslation("artistLabelPlaceholder");
            infoLabelPlaceholder = languageManager.GetTranslation("infoLabelPlaceholder");
            inputTextboxPlaceholder = languageManager.GetTranslation("inputTextboxPlaceholder");
            searchTextboxPlaceholder = languageManager.GetTranslation("searchTextboxPlaceholder");
            downloadFolderPlaceholder = languageManager.GetTranslation("downloadFolderPlaceholder");
            userInfoTextboxPlaceholder = languageManager.GetTranslation("userInfoTextboxPlaceholder");
            downloadOutputWelcome = languageManager.GetTranslation("downloadOutputWelcome");
            downloadOutputExpired = languageManager.GetTranslation("downloadOutputExpired");
            downloadOutputPath = languageManager.GetTranslation("downloadOutputPath");
            downloadOutputNoPath = languageManager.GetTranslation("downloadOutputNoPath");
            downloadOutputNoUrl = languageManager.GetTranslation("downloadOutputNoUrl");
            downloadOutputAPIError = languageManager.GetTranslation("downloadOutputAPIError");
            downloadOutputNotImplemented = languageManager.GetTranslation("downloadOutputNotImplemented");
            downloadOutputCheckLink = languageManager.GetTranslation("downloadOutputCheckLink");
            downloadOutputTrNotStream = languageManager.GetTranslation("downloadOutputTrNotStream");
            downloadOutputAlNotStream = languageManager.GetTranslation("downloadOutputAlNotStream");
            downloadOutputGoodyFound = languageManager.GetTranslation("downloadOutputGoodyFound");
            downloadOutputGoodyExists = languageManager.GetTranslation("downloadOutputGoodyExists");
            downloadOutputGoodyNoURL = languageManager.GetTranslation("downloadOutputGoodyNoURL");
            downloadOutputFileExists = languageManager.GetTranslation("downloadOutputFileExists");
            downloadOutputDownloading = languageManager.GetTranslation("downloadOutputDownloading");
            downloadOutputDone = languageManager.GetTranslation("downloadOutputDone");
            downloadOutputCompleted = languageManager.GetTranslation("downloadOutputCompleted");
            progressLabelInactive = languageManager.GetTranslation("progressLabelInactive");
            progressLabelActive = languageManager.GetTranslation("progressLabelActive");
            formClosingWarning = languageManager.GetTranslation("formClosingWarning");
            downloadAborted = languageManager.GetTranslation("downloadAborted");
            albumSkipped = languageManager.GetTranslation("albumSkipped");

            // Set the placeholders as needed
            inputTextbox.Text = inputTextboxPlaceholder;
            searchTextbox.Text = searchTextboxPlaceholder;
        }

        private void qbdlxForm_Load(object sender, EventArgs e)
        {
            logger.Debug("QBDLX form loading!");

            DeleteFilesFromTempFolder();

            this.DoubleBuffered = true;

            // Round corners of form
            Region = Region.FromHrgn(NativeMethods.CreateRoundRectRgn(0, 0, this.Width, this.Height, 20, 20));

            // Load settings / download location / theme / language / panels
            LoadSavedTemplates();
            LoadQualitySettings();
            LoadTaggingSettings();
            LoadOtherSettings();
            InitializeTheme();
            InitializePanels();
            InitializeLanguage();
            SetDownloadPath();

            // Fix hidden controls tab-order
            // --- SEARCH PAGE
            searchAlbumsButton.TabIndex = 0;
            searchTracksButton.TabIndex = 1;
            // --- SETTINGS PAGE
            downloadFolderTextbox.TabIndex = 0;
            openFolderButton.TabIndex = 1;
            selectFolderButton.TabIndex = 2;
            artistTemplateTextbox.TabIndex = 3;
            albumTemplateTextbox.TabIndex = 4;
            trackTemplateTextbox.TabIndex = 5;
            playlistTemplateTextbox.TabIndex = 6;
            favoritesTemplateTextbox.TabIndex = 7;
            saveTemplatesButton.TabIndex = 8;
            resetTemplatesButton.TabIndex = 9;
            templatesListTextbox.TabIndex = 10;

            // Get and display version number.
            versionNumber.Text = Assembly.GetExecutingAssembly().GetName().Version.ToString();

            // Set placeholders for downloader panel
            albumLabel.Text = albumLabelPlaceholder;
            artistLabel.Text = artistLabelPlaceholder;
            infoLabel.Text = string.Empty;
            progressLabel.Text = progressLabelInactive;

            // Set display_name to welcomeLabel
            welcomeLabel.Text = welcomeLabel.Text.Replace("{username}", user_display_name);

            // Get user account + subscription information for about panel
            downloadOutput.Text = downloadOutputWelcome.Replace("{user_display_name}", user_display_name);

            TextInfo textInfo = CultureInfo.InvariantCulture.TextInfo;
            var endDate = QoUser?.UserInfo?.Subscription?.EndDate ?? "N/A - Family account";
            var subscription = !string.IsNullOrEmpty(QoUser?.UserInfo?.Credential?.Label?.ToString())
                ? textInfo.ToTitleCase(QoUser?.UserInfo?.Credential?.Label?.ToString().ToLower().Replace("-", " ")).Replace("Hifi", "HiFi")
                : "N/A - Expired";

            userInfoTextbox.Text = userInfoTextboxPlaceholder
                .Replace("{user_id}", user_id)
                .Replace("{user_email}", QoUser?.UserInfo?.Email)
                .Replace("{user_country}", QoUser?.UserInfo?.Country)
                .Replace("{user_subscription}", subscription)
                .Replace("{user_subscription_expiration}", endDate);


            downloadOutput.AppendText(QoUser.UserInfo.Credential.Label == null
                ? $"\r\n\r\n{downloadOutputExpired}\r\n\r\n{downloadOutputPath}\r\n{folderBrowser.SelectedPath}"
                : $"\r\n\r\n{downloadOutputPath}\r\n{folderBrowser.SelectedPath}");

            firstLoadComplete = true;
            logger.Debug("QBDLX form loaded!");
        }

        private void qbdlxForm_Shown(object sender, EventArgs e)
        {
            this.notifyIcon1.Visible = true;
        }

        private async void qbdlxForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            logger.Debug($"Triggered form closing with reason: {e.CloseReason}");
            if (getLinkTypeIsBusy)
            {
                if (e.CloseReason == CloseReason.UserClosing)
                {
                    DialogResult dr = 
                        MessageBox.Show(this, formClosingWarning, Application.ProductName, 
                                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    if (dr == DialogResult.No)
                    {
                        e.Cancel = true;
                        logger.Debug($"Form closing was cancelled by user.");
                        return;
                    } else
                    {
                        if (getLinkTypeIsBusy)
                        {
                            e.Cancel = true; 
                            logger.Debug($"Form closing delayed/cancelled because {nameof(getLinkTypeIsBusy)} is {getLinkTypeIsBusy}");
                            abortButton.PerformClick();

                            // Short delay before exiting the application to try allow any current file download to finish/move safely.
                            int maxWaitMilliseconds = 3000;
                            int waitedMilliseconds = 0;
                            int stepMilliseconds = 100;

                            while (getLinkTypeIsBusy && (waitedMilliseconds < maxWaitMilliseconds))
                            {
                                await Task.Delay(stepMilliseconds);
                                waitedMilliseconds += stepMilliseconds;
                            }
                            await Task.Delay(100);
                        }
                    }
                } 
            }
            logger?.Dispose();
            Application.Exit(); // Triggers 'qbdlxForm_FormClosing' with CloseReason.ApplicationExitCall
        }

        private void qualitySelectButton_Click(object sender, EventArgs e)
        {
            qualitySelectPanel.Visible = !qualitySelectPanel.Visible;
        }

        private void qualitySelectPanel_VisibleChanged(object sender, EventArgs e)
        {
            if (!qualitySelectPanel.Visible) return;

            void handler(object s, MouseEventArgs args)
            {
                Point clickPos = qualitySelectPanel.Parent.PointToClient(Cursor.Position);

                if (!qualitySelectPanel.Bounds.Contains(clickPos))
                {
                    qualitySelectPanel.Visible = false;

                    void Unregister(Control c)
                    {
                        if (c == qualitySelectButton) return;
                        c.MouseDown -= handler;
                        foreach (Control child in c.Controls) Unregister(child);
                    }

                    Unregister(qualitySelectPanel.Parent);
                }
            }

            void Register(Control c)
            {
                if (c == qualitySelectButton) return;
                c.MouseDown += handler;
                foreach (Control child in c.Controls) Register(child);
            }

            Register(qualitySelectPanel.Parent);
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            logger.Debug("Exiting");
            this.Close(); // Triggers 'qbdlxForm_FormClosing' with CloseReason.UserClosing
        }

        private void minimizeButton_Click(object sender, EventArgs e)
        {
            logger.Debug("Minimizing");
            this.WindowState = FormWindowState.Minimized;
        }

        private void searchTextbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode.Equals(Keys.Enter))
            {
                e.SuppressKeyPress = true;
            }
        }

        private void searchTextbox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                qbdlxForm._qbdlxForm.searchSortingPanel.Enabled = false;
                qbdlxForm._qbdlxForm.searchSortingPanel.Update();
                searchAlbumsButton.PerformClick();
                qbdlxForm._qbdlxForm.searchSortingPanel.Enabled = true;
            }
        }
        private void downloadFolderTextbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode.Equals(Keys.Enter))
            {
                e.SuppressKeyPress = true;
            }
        }

        private void downloadFolderTextbox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string path = downloadFolderTextbox.Text?.TrimEnd('\\');

                if (ZlpIOHelper.DirectoryExists(path))
                {
                    Thread t = new Thread((ThreadStart)(() =>
                    {
                        // Save the selection
                        Settings.Default.savedFolder = path + @"\";
                        Settings.Default.Save();
                    }));

                    // Run your code from a thread that joins the STA Thread
                    t.SetApartmentState(ApartmentState.STA);
                    t.Start();
                    t.Join();

                    folderBrowser.SelectedPath = path + @"\";
                    downloadLocation = path + @"\";
                } else
                {
                    MessageBox.Show(this, languageManager.GetTranslation("downloadOutputDontExistMsg"), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    // Restore previous path text only if it's not null or empty.
                    if (!string.IsNullOrWhiteSpace(folderBrowser.SelectedPath))
                    {
                        downloadFolderTextbox.Text = folderBrowser.SelectedPath;
                    }
                    downloadFolderTextbox.SelectionStart = downloadFolderTextbox.Text.Length;
                    downloadFolderTextbox.SelectionLength = 0;
                }
            }
        }

        private void inputTextbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                if (downloadButton.Enabled) {
                    downloadButton.PerformClick();
                }
            }
        }

        private void inputTextbox_TextChanged(object sender, EventArgs e)
        {
            string text = inputTextbox.Text.TrimStart();
            downloadButton.Enabled = (text.StartsWith("http", StringComparison.OrdinalIgnoreCase));
        }

        private void abortButton_Click(object sender, EventArgs e)
        {
            abortButton.Enabled = false;
            skipButton.Enabled = false;
            if (getLinkTypeIsBusy && abortTokenSource != null)
            {
                logger.Debug("abortTokenSource cancel request by user.");
                abortTokenSource.Cancel();
            }
        }

        private void skipButton_Click(object sender, EventArgs e)
        {
            if (getLinkTypeIsBusy)
            {
                logger.Debug("skipCurrentAlbum request by user.");
                skipCurrentAlbum = true;
            }
        }

        private async void downloadButton_Click(object sender, EventArgs e)
        {
            await downloadButtonAsyncWork();
        }

        private async Task downloadButtonAsyncWork()
        {
            getLinkTypeIsBusy = true;
            abortTokenSource?.Dispose();
            abortTokenSource = null;
            abortTokenSource = new CancellationTokenSource();
            try
            {
                inputTextbox.Enabled = false;
                downloadButton.Enabled = false;
                batchDownloadButton.Enabled = false;
                abortButton.Enabled = true;
                if (!isBatchDownloadRunning)
                {
                    batchDownloadProgressCountLabel.Text = "";
                    batchDownloadProgressCountLabel.Visible = false;
                }
                await getLinkTypeAsync(abortTokenSource.Token);
            }
            catch (OperationCanceledException)
            {
                TaskbarHelper.SetProgressState(TaskbarProgressState.Error);
                logger.Debug("Download aborted by user.");
                downloadOutput.AppendText($"\r\n{downloadAborted}");
            }
            finally
            {
                skipButton.Enabled = false;
                abortButton.Enabled = false;
                inputTextbox.Enabled = true;
                downloadButton.Enabled = true;
                batchDownloadButton.Enabled = true;
                skipCurrentAlbum = false;
                getLinkTypeIsBusy = false;
            }
        }

        private async void batchDownloadButton_Click(object sender, EventArgs e)
        {
            Form batchDownloadDialog = new Form
            {
                FormBorderStyle = FormBorderStyle.FixedDialog,
                StartPosition = FormStartPosition.CenterParent,
                MinimizeBox = false,
                MaximizeBox = false,
                ClientSize = new Size((int)(this.ClientSize.Width / 1.5), 320),
                Text = languageManager.GetTranslation("batchDownloadDlgText"),
                ShowInTaskbar = false
            };

            batchDownloadDialog.Controls.Add(batchDownloadPanel);
            batchDownloadPanel.Dock = DockStyle.Fill;
            batchDownloadPanel.Visible = true;

            DialogResult result = batchDownloadDialog.ShowDialog(this);

            batchDownloadPanel.Visible = false;
            this.Controls.Add(batchDownloadPanel);
            batchDownloadDialog.Dispose();

            if (result == DialogResult.OK)
            {
                HashSet<string> batchUrls = new HashSet<string>(
                    batchDownloadTextBox.Lines
                                        .Select(l => l.Trim())
                                        .Where(l => !string.IsNullOrWhiteSpace(l)), 
                    StringComparer.OrdinalIgnoreCase
                );

                await DownloadBatchUrls(batchUrls);
            }
        }

        public async Task DownloadBatchUrls(HashSet<string> batchUrls)
        {
            abortTokenSource?.Dispose();
            abortTokenSource = null;

            int batchUrlsCount = batchUrls.Count;
            int batchUrlsCurrentIndex = 0;

            batchDownloadProgressCountLabel.Text = "";
            batchDownloadProgressCountLabel.Visible = true;
            TaskbarHelper.SetProgressState(TaskbarProgressState.Normal);
            TaskbarHelper.SetProgressValue(0, batchUrlsCount);
            batchDownloadProgressCountLabel.Text = $"{languageManager.GetTranslation("batchDownloadDlgText")} | {batchUrlsCurrentIndex} / {batchUrlsCount}";
            notifyIcon1.Text = $"QobuzDLX\r\n\r\n{languageManager.GetTranslation("batchDownloadDlgText")} | {batchUrlsCurrentIndex} / {batchUrlsCount}";
            isBatchDownloadRunning = true;
            foreach (string url in batchUrls)
            {
                batchUrlsCurrentIndex++;

                if (abortTokenSource != null && abortTokenSource.IsCancellationRequested)
                {
                    TaskbarHelper.SetProgressState(TaskbarProgressState.Error);
                    isBatchDownloadRunning = false;
                    break;
                }

                inputTextbox.Text = url;
                inputTextbox.ForeColor = Color.FromArgb(200, 200, 200);

                await downloadButtonAsyncWork();

                if (abortTokenSource != null && abortTokenSource.IsCancellationRequested)
                {
                    break;
                }
                batchDownloadProgressCountLabel.Text = $"{languageManager.GetTranslation("batchDownloadDlgText")} | {batchUrlsCurrentIndex} / {batchUrlsCount}";
                notifyIcon1.Text = $"QobuzDLX\r\n\r\n{languageManager.GetTranslation("batchDownloadDlgText")} | {batchUrlsCurrentIndex} / {batchUrlsCount}";
                TaskbarHelper.SetProgressValue(batchUrlsCurrentIndex, batchUrlsCount);
            }
            if (!this.Visible) notifyIcon1.ShowBalloonTip(5000, "QobuzDLX", languageManager.GetTranslation("batchDownloadFinished"), ToolTipIcon.Info);
            notifyIcon1.Text = $"QobuzDLX";
            isBatchDownloadRunning = false;

            batchDownloadSelectedRowsButton.Enabled =
                downloadButton.Enabled &&
                !getLinkTypeIsBusy &&
                SearchPanelHelper.selectedRowindices.Any();
        }

        private void closeBatchDownloadbutton_Click(object sender, EventArgs e)
        {
            Form parentForm = closeBatchDownloadbutton.FindForm();
            parentForm.DialogResult = DialogResult.Cancel;
        }

        private void getAllBatchDownloadButton_Click(object sender, EventArgs e)
        {
            Form parentForm = closeBatchDownloadbutton.FindForm();
            parentForm.DialogResult = DialogResult.OK;
        }

        private void batchDownloadTextBox_TextChanged(object sender, EventArgs e)
        {
            string text = batchDownloadTextBox.Text.TrimStart();
            getAllBatchDownloadButton.Enabled = text.IndexOf("http", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        public async Task getLinkTypeAsync(CancellationToken abortToken)
        {
            if (!isBatchDownloadRunning)
            {
                TaskbarHelper.SetProgressState(TaskbarProgressState.Normal);
                TaskbarHelper.SetProgressValue(0, progressBarDownload.Maximum);
            }

            IntPtr hwnd = Process.GetCurrentProcess().MainWindowHandle;
            var progress = new Progress<int>(value =>
            {
                progressBarDownload.Value = value;
                if (!isBatchDownloadRunning) TaskbarHelper.SetProgressValue(value, qbdlxForm.lastTaskBarProgressMaxValue);
            });

            downloadOutput.Focus();
            progressLabel.Invoke(new Action(() => progressLabel.Text = downloadOutputCheckLink));
            progressBarDownload.Invoke(new Action(() => progressBarDownload.Value = progressBarDownload.Minimum));

            // Check if there's no selected path.
            if (downloadLocation == null | downloadLocation == "" | downloadLocation == "no folder selected")
            {
                // If there is NOT a saved path.
                logger.Warning("No path has been set! Remember to Choose a Folder!");
                downloadOutput.Invoke(new Action(() => downloadOutput.Text = String.Empty));
                downloadOutput.Invoke(new Action(() => downloadOutput.AppendText($"{downloadOutputNoPath}\r\n")));
                progressLabel.Invoke(new Action(() => progressLabel.Text = progressLabelInactive));
                return;
            }

            string albumLink = inputTextbox.Text.Trim();

            bool isValidUrl = qobuzUrlRegEx.IsMatch(albumLink)
                              && Uri.TryCreate(albumLink, UriKind.Absolute, out Uri uriResult)
                              && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

            if (!isValidUrl)
            {
                string msg = string.Format(languageManager.GetTranslation("invalidUrl"), albumLink);
                logger.Error(msg);
                downloadOutput.Invoke(new Action(() => downloadOutput.Text= msg));
                progressLabel.Invoke(new Action(() => progressLabel.Text = msg));
                if (!isBatchDownloadRunning) TaskbarHelper.SetProgressState(TaskbarProgressState.Error);
                if (isBatchDownloadRunning) MessageBox.Show(this, msg, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var qobuzStoreLinkGrab = qobuzStoreLinkRegex.Match(albumLink).Groups;
            var linkRegion = qobuzStoreLinkGrab[1].Value;
            var storeLinkType = qobuzStoreLinkGrab[2].Value;
            var linkName = qobuzStoreLinkGrab[3].Value;
            var qobuzStoreLinkId = qobuzStoreLinkGrab[4].Value;

            if (linkRegion != null)
            {
                if (storeLinkType == "album")
                {
                    albumLink = "https://play.qobuz.com/album/" + qobuzStoreLinkId;
                }
                else if (storeLinkType == "interpreter")
                {
                    albumLink = "https://play.qobuz.com/artist/" + qobuzStoreLinkId;
                }
            }

            var qobuzLinkIdGrab = qobuzLinkIdGrabRegex.Match(albumLink).Groups;
            var linkType = qobuzLinkIdGrab[1].Value;
            var qobuzLinkId = qobuzLinkIdGrab[2].Value;

            qobuz_id = qobuzLinkId;

            downloadTrack.clearOutputText();
            getInfo.outputText = null;
            getInfo.updateDownloadOutput(downloadOutputCheckLink);

            progressItemsCountLabel.Text = "";
            progressItemsCountLabel.Visible = true;

            switch (linkType)
            {
                case "album":
                    skipButton.Enabled = true;
                    if (!isBatchDownloadRunning) TaskbarHelper.SetProgressValue(0, progressBarDownload.Maximum);
                    await Task.Run(() => getInfo.getAlbumInfoLabels(app_id, qobuz_id, user_auth_token));
                    QoAlbum = getInfo.QoAlbum;
                    if (QoAlbum == null)
                    {
                        getInfo.updateDownloadOutput($"{downloadOutputAPIError}");
                        progressLabel.Invoke(new Action(() => progressLabel.Text = progressLabelInactive));
                        break;
                    }
                    progressItemsCountLabel.Text = $"{languageManager.GetTranslation("album")} | {QoAlbum.TracksCount:N0} {languageManager.GetTranslation("tracks")}";
                    updateAlbumInfoLabels(QoAlbum);
                    await Task.Run(() => downloadAlbum.DownloadAlbumAsync(app_id, qobuz_id, format_id, audio_format, user_auth_token, app_secret, downloadLocation, artistTemplate, albumTemplate, trackTemplate, QoAlbum, progress, abortToken));
                    if (abortToken.IsCancellationRequested) { abortToken.ThrowIfCancellationRequested(); }
                    // Say the downloading is finished when it's completed.
                    getInfo.outputText = qbdlxForm._qbdlxForm.downloadOutput.Text;
                    getInfo.updateDownloadOutput("\r\n" + downloadOutputCompleted);
                    progressLabel.Invoke(new Action(() => progressLabel.Text = progressLabelInactive));
                    break;
                case "track":
                    skipButton.Enabled = false;
                    if (!isBatchDownloadRunning) TaskbarHelper.SetProgressValue(0, progressBarDownload.Maximum);
                    await Task.Run(() => getInfo.getTrackInfoLabels(app_id, qobuz_id, user_auth_token));
                    QoItem = getInfo.QoItem;
                    QoAlbum = getInfo.QoAlbum;
                    updateAlbumInfoLabels(QoAlbum);
                    progressItemsCountLabel.Text = $"{languageManager.GetTranslation("singleTrack")}";
                    await Task.Run(() => downloadTrack.DownloadTrackAsync("track", app_id, qobuz_id, format_id, audio_format, user_auth_token, app_secret, downloadLocation, artistTemplate, albumTemplate, trackTemplate, QoAlbum, QoItem, progress));
                    // Say the downloading is finished when it's completed.
                    if (!isBatchDownloadRunning) TaskbarHelper.SetProgressValue(progressBarDownload.Maximum, progressBarDownload.Maximum);
                    getInfo.outputText = qbdlxForm._qbdlxForm.downloadOutput.Text;
                    getInfo.updateDownloadOutput("\r\n" + downloadOutputCompleted);
                    progressLabel.Invoke(new Action(() => progressLabel.Text = progressLabelInactive));
                    progressBarDownload.Invoke(new Action(() => progressBarDownload.Value = progressBarDownload.Maximum));
                    break;
                case "playlist":
                    skipButton.Enabled = false;
                    if (!isBatchDownloadRunning) TaskbarHelper.SetProgressValue(0, progressBarDownload.Maximum);
                    await Task.Run(() => getInfo.getPlaylistInfoLabels(app_id, qobuz_id, user_auth_token));
                    QoPlaylist = getInfo.QoPlaylist;
                    updatePlaylistInfoLabels(QoPlaylist);
                    int totalTracksPlaylist = QoPlaylist.Tracks.Items.Count;
                    int trackIndexPlaylist = 0;
                    foreach (var item in QoPlaylist.Tracks.Items)
                    {
                        if (abortToken.IsCancellationRequested) { abortToken.ThrowIfCancellationRequested(); }
                        if (!isBatchDownloadRunning) TaskbarHelper.SetProgressValue(trackIndexPlaylist, totalTracksPlaylist);
                        progressItemsCountLabel.Text = $"{languageManager.GetTranslation("playlist")} | {trackIndexPlaylist:N0} / {totalTracksPlaylist:N0} {languageManager.GetTranslation("tracks")}";

                        trackIndexPlaylist++;
                        try
                        {
                            string track_id = item.Id.ToString();
                            await Task.Run(() => getInfo.getTrackInfoLabels(app_id, track_id, user_auth_token));
                            QoItem = item;
                            QoAlbum = getInfo.QoAlbum;
                            await Task.Run(() => downloadTrack.DownloadPlaylistTrackAsync(
                                app_id, format_id, audio_format, user_auth_token, app_secret,
                                downloadLocation, trackTemplate, playlistTemplate, QoAlbum, QoItem, QoPlaylist,
                                new Progress<int>(value =>
                                {
                                    double scaledValue = (trackIndexPlaylist - 1 + value / 100.0) / totalTracksPlaylist * 100.0;
                                    progressBarDownload.Invoke(new Action(() => progressBarDownload.Value = Math.Min(100, (int)Math.Round(scaledValue))));
                                })));
                        }
                        catch
                        {
                            continue;
                        }
                        if (!isBatchDownloadRunning) TaskbarHelper.SetProgressValue(trackIndexPlaylist, totalTracksPlaylist);
                        progressItemsCountLabel.Text = $"{languageManager.GetTranslation("playlist")} | {trackIndexPlaylist:N0} / {totalTracksPlaylist:N0} {languageManager.GetTranslation("tracks")}";

                    }
                    if (abortToken.IsCancellationRequested) { abortToken.ThrowIfCancellationRequested(); }
                    // Say the downloading is finished when it's completed.
                    getInfo.outputText = qbdlxForm._qbdlxForm.downloadOutput.Text;
                    getInfo.updateDownloadOutput("\r\n" + downloadOutputCompleted);
                    progressLabel.Invoke(new Action(() => progressLabel.Text = progressLabelInactive));
                    break;
                case "artist":
                    skipButton.Enabled = true;
                    if (!isBatchDownloadRunning) TaskbarHelper.SetProgressValue(0, progressBarDownload.Maximum);
                    await Task.Run(() => getInfo.getArtistInfo(app_id, qobuz_id, user_auth_token));
                    QoArtist = getInfo.QoArtist;
                    int totalAlbumsArtist = QoArtist.Albums.Items.Count;
                    int albumIndexArtist = 0;

                    if (totalAlbumsArtist == 0)
                    {
                        progressItemsCountLabel.Text = $"{languageManager.GetTranslation("artist")} | 0 {languageManager.GetTranslation("albums")}";
                    }

                    foreach (var item in QoArtist.Albums.Items)
                    {
                        if (abortToken.IsCancellationRequested) { abortToken.ThrowIfCancellationRequested(); }
                        if (!isBatchDownloadRunning) TaskbarHelper.SetProgressValue(albumIndexArtist, totalAlbumsArtist);
                        progressItemsCountLabel.Text = $"{languageManager.GetTranslation("artist")} | {albumIndexArtist:N0} / {totalAlbumsArtist:N0} {languageManager.GetTranslation("albums")}";
                       
                        albumIndexArtist++;
                        try
                        {
                            string album_id = item.Id.ToString();
                            await Task.Run(() => getInfo.getAlbumInfoLabels(app_id, album_id, user_auth_token));
                            QoAlbum = getInfo.QoAlbum;
                            updateAlbumInfoLabels(QoAlbum);
                            await Task.Run(() => downloadAlbum.DownloadAlbumAsync(
                               app_id, album_id, format_id, audio_format, user_auth_token, app_secret,
                               downloadLocation, artistTemplate, albumTemplate, trackTemplate, QoAlbum,
                               new Progress<int>(value =>
                               {
                                   double scaledValue = ((albumIndexArtist - 1) + value / 100.0) / totalAlbumsArtist * 100.0;
                                   progressBarDownload.Invoke(new Action(() => progressBarDownload.Value = Math.Min(100, (int)Math.Round(scaledValue))));
                               }), abortToken));
                        }
                        catch
                        {
                            continue;
                        }
                    }
                    if (abortToken.IsCancellationRequested) { abortToken.ThrowIfCancellationRequested(); }
                    // Say the downloading is finished when it's completed.
                    if (!isBatchDownloadRunning) TaskbarHelper.SetProgressValue(albumIndexArtist, totalAlbumsArtist);
                    progressItemsCountLabel.Text = $"{languageManager.GetTranslation("artist")} | {albumIndexArtist:N0} / {totalAlbumsArtist:N0} {languageManager.GetTranslation("albums")}";
                    getInfo.outputText = qbdlxForm._qbdlxForm.downloadOutput.Text;
                    getInfo.updateDownloadOutput("\r\n" + downloadOutputCompleted);
                    progressLabel.Invoke(new Action(() => progressLabel.Text = progressLabelInactive));
                    break;
                case "label":
                    skipButton.Enabled = true;
                    if (!isBatchDownloadRunning) TaskbarHelper.SetProgressValue(0, progressBarDownload.Maximum);
                    await Task.Run(() => getInfo.getLabelInfo(app_id, qobuz_id, user_auth_token));
                    QoLabel = getInfo.QoLabel;
                    int totalAlbumsLabel = QoLabel.Albums.Items.Count;
                    int albumIndexLabel = 0;

                    if (totalAlbumsLabel == 0)
                    {
                        progressItemsCountLabel.Text = $"{languageManager.GetTranslation("recordLabel")} | 0 {languageManager.GetTranslation("albums")}";
                    }

                    foreach (var item in QoLabel.Albums.Items)
                    {
                        if (abortToken.IsCancellationRequested) { abortToken.ThrowIfCancellationRequested(); }
                        if (!isBatchDownloadRunning) TaskbarHelper.SetProgressValue(albumIndexLabel, totalAlbumsLabel);
                        progressItemsCountLabel.Text = $"{languageManager.GetTranslation("recordLabel")} | {albumIndexLabel:N0} / {totalAlbumsLabel:N0} {languageManager.GetTranslation("albums")}";

                        albumIndexLabel++;
                        try
                        {
                            string album_id = item.Id.ToString();
                            await Task.Run(() => getInfo.getAlbumInfoLabels(app_id, album_id, user_auth_token));
                            QoAlbum = getInfo.QoAlbum;
                            updateAlbumInfoLabels(QoAlbum);
                            await Task.Run(() => downloadAlbum.DownloadAlbumAsync(
                                app_id, album_id, format_id, audio_format, user_auth_token, app_secret,
                                downloadLocation, artistTemplate, albumTemplate, trackTemplate, QoAlbum,
                                new Progress<int>(value =>
                                {
                                    double scaledValue = ((albumIndexLabel - 1) + value / 100.0) / totalAlbumsLabel * 100.0;
                                    progressBarDownload.Invoke(new Action(() => progressBarDownload.Value = Math.Min(100, (int)Math.Round(scaledValue))));
                                }), abortToken));
                        }
                        catch
                        {
                            continue;
                        }
                        if (!isBatchDownloadRunning) TaskbarHelper.SetProgressValue(albumIndexLabel, totalAlbumsLabel);
                        progressItemsCountLabel.Text = $"{languageManager.GetTranslation("recordLabel")} | {albumIndexLabel:N0} / {totalAlbumsLabel:N0} {languageManager.GetTranslation("albums")}";
                    }
                    if (abortToken.IsCancellationRequested) { abortToken.ThrowIfCancellationRequested(); }
                    // Say the downloading is finished when it's completed.
                    getInfo.outputText = qbdlxForm._qbdlxForm.downloadOutput.Text;
                    getInfo.updateDownloadOutput("\r\n" + downloadOutputCompleted);
                    progressLabel.Invoke(new Action(() => progressLabel.Text = progressLabelInactive));
                    break;
                case "user":
                    if (qobuzLinkId.Contains("albums"))
                    {
                        skipButton.Enabled = true;
                        if (!isBatchDownloadRunning) TaskbarHelper.SetProgressValue(0, progressBarDownload.Maximum);
                        await Task.Run(() => getInfo.getFavoritesInfo(app_id, user_id, "albums", user_auth_token));
                        QoFavorites = getInfo.QoFavorites;
                        int totalAlbumsUser = QoFavorites.Albums.Items.Count;
                        int albumIndexUser = 0;

                        if (totalAlbumsUser == 0)
                        {
                            progressItemsCountLabel.Text = $"{languageManager.GetTranslation("user")} | 0 {languageManager.GetTranslation("albums")}";
                        }

                        foreach (var item in QoFavorites.Albums.Items)
                        {
                            if (abortToken.IsCancellationRequested) { abortToken.ThrowIfCancellationRequested(); }
                            if (!isBatchDownloadRunning) TaskbarHelper.SetProgressValue(albumIndexUser, totalAlbumsUser);
                            progressItemsCountLabel.Text = $"{languageManager.GetTranslation("user")} | {albumIndexUser:N0} / {totalAlbumsUser:N0} {languageManager.GetTranslation("albums")}";
                            
                            albumIndexUser++;
                            try
                            {
                                string album_id = item.Id.ToString();
                                await Task.Run(() => getInfo.getAlbumInfoLabels(app_id, album_id, user_auth_token));
                                QoAlbum = getInfo.QoAlbum;
                                updateAlbumInfoLabels(QoAlbum);
                                await Task.Run(() => downloadAlbum.DownloadAlbumAsync(
                                    app_id, album_id, format_id, audio_format, user_auth_token, app_secret,
                                    downloadLocation, artistTemplate, albumTemplate, trackTemplate, QoAlbum,
                                    new Progress<int>(value =>
                                    {
                                        double scaledValue = ((albumIndexUser - 1) + value / 100.0) / totalAlbumsUser * 100.0;
                                        progressBarDownload.Invoke(new Action(() => progressBarDownload.Value = Math.Min(100, (int)Math.Round(scaledValue))));
                                        if (!isBatchDownloadRunning) TaskbarHelper.SetProgressValue(progressBarDownload.Value, progressBarDownload.Maximum);
                                    }), abortToken));
                            }
                            catch
                            {
                                continue;
                            }
                            progressItemsCountLabel.Text = $"{languageManager.GetTranslation("user")} | {albumIndexUser:N0} / {totalAlbumsUser:N0} {languageManager.GetTranslation("albums")}";
                        }
                    }
                    else if (qobuzLinkId.Contains("tracks"))
                    {
                        skipButton.Enabled = false;
                        if (!isBatchDownloadRunning) TaskbarHelper.SetProgressValue(0, progressBarDownload.Maximum);
                        await Task.Run(() => getInfo.getFavoritesInfo(app_id, user_id, "tracks", user_auth_token));
                        QoFavorites = getInfo.QoFavorites;
                        int totalTracksUser = QoFavorites.Tracks.Items.Count;
                        int trackIndexUser = 0;

                        if (totalTracksUser == 0)
                        {
                            progressItemsCountLabel.Text = $"{languageManager.GetTranslation("user")} | 0 {languageManager.GetTranslation("tracks")}";
                        }

                        foreach (var item in QoFavorites.Tracks.Items)
                        {
                            if (abortToken.IsCancellationRequested) { abortToken.ThrowIfCancellationRequested(); }
                            if (!isBatchDownloadRunning) TaskbarHelper.SetProgressValue(trackIndexUser, totalTracksUser);
                            progressItemsCountLabel.Text = $"{languageManager.GetTranslation("user")} | {trackIndexUser:N0} / {totalTracksUser:N0} {languageManager.GetTranslation("tracks")}";

                            trackIndexUser++;
                            try
                            {
                                string track_id = item.Id.ToString();
                                await Task.Run(() => getInfo.getTrackInfoLabels(app_id, track_id, user_auth_token));
                                QoItem = getInfo.QoItem;
                                QoAlbum = getInfo.QoAlbum;
                                updateAlbumInfoLabels(QoAlbum);
                                await Task.Run(() => downloadTrack.DownloadTrackAsync(
                                    "track", app_id, track_id, format_id, audio_format, user_auth_token, app_secret,
                                    downloadLocation, artistTemplate, albumTemplate, trackTemplate, QoAlbum, QoItem,
                                    new Progress<int>(value =>
                                    {
                                        double scaledValue = ((trackIndexUser - 1) + value / 100.0) / totalTracksUser * 100.0;
                                        progressBarDownload.Invoke(new Action(() => progressBarDownload.Value = Math.Min(100, (int)Math.Round(scaledValue))));
                                    })));
                            }
                            catch
                            {
                                continue;
                            }
                            if (!isBatchDownloadRunning) TaskbarHelper.SetProgressValue(trackIndexUser, totalTracksUser);
                            progressItemsCountLabel.Text = $"{languageManager.GetTranslation("user")} | {trackIndexUser:N0} / {totalTracksUser:N0} {languageManager.GetTranslation("tracks")}";
                        }
                    }
                    else if (qobuzLinkId.Contains("artists"))
                    {
                        skipButton.Enabled = true;
                        if (!isBatchDownloadRunning) TaskbarHelper.SetProgressValue(0, progressBarDownload.Maximum);
                        await Task.Run(() => getInfo.getFavoritesInfo(app_id, user_id, "artists", user_auth_token));
                        QoFavorites = getInfo.QoFavorites;

                        int totalAlbumsUserArtists = 0;
                        int totalArtists = QoFavorites.Artists.Items.Count;

                        if (totalAlbumsUserArtists == 0)
                        {
                            progressItemsCountLabel.Text = $"{languageManager.GetTranslation("user")} | 0 {languageManager.GetTranslation("artists")}";
                        }

                        foreach (var artist in QoFavorites.Artists.Items)
                        {
                            if (abortToken.IsCancellationRequested) { abortToken.ThrowIfCancellationRequested(); }
                            try
                            {
                                await Task.Run(() => getInfo.getArtistInfo(app_id, artist.Id.ToString(), user_auth_token));
                                QoArtist = getInfo.QoArtist;
                                totalAlbumsUserArtists += QoArtist.Albums.Items.Count;
                            }
                            catch
                            {
                                continue;
                            }
                        }

                        int albumIndexUserArtist = 0;
                        int indexUserArtist = 0;

                        foreach (var artist in QoFavorites.Artists.Items)
                        {
                            if (abortToken.IsCancellationRequested) { abortToken.ThrowIfCancellationRequested(); }
                            if (!isBatchDownloadRunning) TaskbarHelper.SetProgressValue(indexUserArtist, totalArtists);

                            indexUserArtist++;
                            try
                            {
                                string artist_id = artist.Id.ToString();
                                await Task.Run(() => getInfo.getArtistInfo(app_id, artist_id, user_auth_token));
                                QoArtist = getInfo.QoArtist;

                                foreach (var artistItem in QoArtist.Albums.Items)
                                {
                                    if (abortToken.IsCancellationRequested) { abortToken.ThrowIfCancellationRequested(); }
                                    progressItemsCountLabel.Text = $"{languageManager.GetTranslation("user")} | {indexUserArtist:N0} / {totalArtists:N0} {languageManager.GetTranslation("artists")} | {albumIndexUserArtist:N0} / {totalAlbumsUserArtists:N0} {languageManager.GetTranslation("albums")}";

                                    albumIndexUserArtist++;
                                    try
                                    {
                                        string album_id = artistItem.Id.ToString();
                                        await Task.Run(() => getInfo.getAlbumInfoLabels(app_id, album_id, user_auth_token));
                                        QoAlbum = getInfo.QoAlbum;
                                        updateAlbumInfoLabels(QoAlbum);

                                        await Task.Run(() => downloadAlbum.DownloadAlbumAsync(
                                            app_id, album_id, format_id, audio_format, user_auth_token, app_secret, downloadLocation,
                                            artistTemplate, albumTemplate, trackTemplate, QoAlbum,
                                            new Progress<int>(value =>
                                            {
                                                double scaledValue = ((albumIndexUserArtist - 1) + value / 100.0) / totalAlbumsUserArtists * 100.0;
                                                progressBarDownload.Invoke(new Action(() => progressBarDownload.Value = Math.Min(100, (int)Math.Round(scaledValue))));
                                                if (!isBatchDownloadRunning) TaskbarHelper.SetProgressValue(progressBarDownload.Value, progressBarDownload.Maximum);
                                            }), abortToken));
                                    }
                                    catch
                                    {
                                        continue;
                                    }
                                }
                                progressItemsCountLabel.Text = $"{languageManager.GetTranslation("user")} | {indexUserArtist:N0} / {totalArtists:N0} {languageManager.GetTranslation("artists")} | {albumIndexUserArtist:N0} / {totalAlbumsUserArtists:N0} {languageManager.GetTranslation("albums")}";
                            }
                            catch
                            {
                                continue;
                            }
                        }
                    }
                    else
                    {
                        // Say what isn't available at the moment.
                        downloadOutput.Invoke(new Action(() => downloadOutput.Text = String.Empty));
                        downloadOutput.Invoke(new Action(() => downloadOutput.AppendText(downloadOutputNotImplemented)));
                        progressLabel.Invoke(new Action(() => progressLabel.Text = downloadFolderPlaceholder));
                        return;
                    }
                    if (abortToken.IsCancellationRequested) { abortToken.ThrowIfCancellationRequested(); }
                    // Say the downloading is finished when it's completed.
                    getInfo.outputText = qbdlxForm._qbdlxForm.downloadOutput.Text;
                    getInfo.updateDownloadOutput("\r\n" + downloadOutputCompleted);
                    progressLabel.Invoke(new Action(() => progressLabel.Text = progressLabelInactive));
                    break;
                default:
                    // Say what isn't available at the moment.
                    downloadOutput.Invoke(new Action(() => downloadOutput.Text = String.Empty));
                    downloadOutput.Invoke(new Action(() => downloadOutput.AppendText(downloadOutputNotImplemented)));
                    progressLabel.Invoke(new Action(() => progressLabel.Text = progressLabelInactive));
                    return;
            }
        }

        public void updateAlbumInfoLabels(Album QoAlbum)
        {
            string trackOrTracks = "tracks";
            if (QoAlbum.TracksCount == 1) { trackOrTracks = "track"; }
            artistLabel.Text = renameTemplates.GetReleaseArtists(QoAlbum).Replace("&", "&&");
            if (QoAlbum.Version == null) { albumLabel.Text = QoAlbum.Title.Replace(@"&", @"&&"); } else { albumLabel.Text = QoAlbum.Title.Replace(@"&", @"&&").TrimEnd() + " (" + QoAlbum.Version + ")"; }
            infoLabel.Text = $"{infoLabelPlaceholder} {QoAlbum.ReleaseDateOriginal} • {QoAlbum.TracksCount} {trackOrTracks} • {QoAlbum.UPC}";
            try { albumPictureBox.ImageLocation = QoAlbum.Image?.Small; } catch { }
        }

        public void updatePlaylistInfoLabels(Playlist QoPlaylist)
        {
            artistLabel.Text = QoPlaylist.Owner.Name.Replace(@"&", @"&&") + "'s Playlist";
            albumLabel.Text = QoPlaylist.Name.Replace(@"&", @"&&");
            infoLabel.Text = "";
            try { albumPictureBox.ImageLocation = QoPlaylist.Images300[0]; } catch { }
        }

        private void SetPlaceholder(TextBox textBox, string placeholderText, bool isFocused)
        {
            if (isFocused)
            {
                if (textBox.Text == placeholderText)
                {
                    textBox.Text = null;
                    textBox.ForeColor = ColorTranslator.FromHtml(theme.TextBoxText);
                }
            }
            else
            {
                if (string.IsNullOrEmpty(textBox.Text))
                {
                    textBox.ForeColor = ColorTranslator.FromHtml(theme.PlaceholderTextBoxText);
                    textBox.Text = placeholderText;
                }
            }
        }

        private void inputTextbox_Click(object sender, EventArgs e)
        {
            SetPlaceholder(inputTextbox, inputTextboxPlaceholder, true);
        }

        private void inputTextbox_Leave(object sender, EventArgs e)
        {
            SetPlaceholder(inputTextbox, inputTextboxPlaceholder, false);
        }

        private void searchTextbox_Click(object sender, EventArgs e)
        {
            SetPlaceholder(searchTextbox, searchTextboxPlaceholder, true);
        }

        private void searchTextbox_Leave(object sender, EventArgs e)
        {
            SetPlaceholder(searchTextbox, searchTextboxPlaceholder, false);
        }

        private void openFolderButton_Click(object sender, EventArgs e)
        {
            // Open selected folder
            if (folderBrowser.SelectedPath == null | folderBrowser.SelectedPath == "")
            {
                // If there's no selected path.
                MessageBox.Show(this, downloadOutputNoPath, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            else
            {
                // If selected path doesn't exist, create it. (Will be ignored if it does)
                System.IO.Directory.CreateDirectory(folderBrowser.SelectedPath);
                // Open selcted folder
                Process.Start(folderBrowser.SelectedPath);
            }
        }

        private void selectFolderButton_Click(object sender, EventArgs e)
        {
            Thread t = new Thread((ThreadStart)(() =>
            {
                // Open Folder Browser to select path & Save the selection
                if (folderBrowser.ShowDialog() == DialogResult.OK)
                {
                    if (!string.IsNullOrWhiteSpace(folderBrowser.SelectedPath))
                    {
                        folderBrowser.SelectedPath = folderBrowser.SelectedPath.TrimEnd('\\') + @"\";
                        Settings.Default.savedFolder = folderBrowser.SelectedPath;
                        Settings.Default.Save();
                    }
                }
            }));

            // Run your code from a thread that joins the STA Thread
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
            t.Join();

            if (!string.IsNullOrWhiteSpace(folderBrowser.SelectedPath))
            { 
                downloadFolderTextbox.Text = folderBrowser.SelectedPath;
                downloadLocation = folderBrowser.SelectedPath;
            }
        }

        private void resetTemplatesButton_Click(object sender, EventArgs e)
        {
            artistTemplateTextbox.Text = "%ArtistName%";
            albumTemplateTextbox.Text = "%AlbumTitle% (%Year%) (%AlbumPA%) [UPC%UPC%]";
            trackTemplateTextbox.Text = "%TrackNumber%. %ArtistName% - %TrackTitle%";
            playlistTemplateTextbox.Text = "%PlaylistTitle% [ID%PlaylistID%]\\%ArtistName%";
            favoritesTemplateTextbox.Text = "- Favorites";
        }

        private void saveTemplatesButton_Click(object sender, EventArgs e)
        {
            Settings.Default.savedArtistTemplate = artistTemplateTextbox.Text;
            Settings.Default.savedAlbumTemplate = albumTemplateTextbox.Text;
            Settings.Default.savedTrackTemplate = trackTemplateTextbox.Text;
            Settings.Default.savedPlaylistTemplate = playlistTemplateTextbox.Text;
            Settings.Default.savedFavoritesTemplate = favoritesTemplateTextbox.Text;
            Settings.Default.Save();
            updateTemplates();
        }

        #region Quality Selection

        private void flacHighButton2_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.quality4 = flacHighButton2.Checked;
            Settings.Default.Save();

            if (flacHighButton2.Checked == true)
            {
                logger.Debug("Setting format ID to 27");
                format_id = "27";
                audio_format = ".flac";
                Settings.Default.qualityFormat = format_id;
                Settings.Default.audioType = audio_format;
            }
        }

        private void flacMidButton2_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.quality3 = flacMidButton2.Checked;
            Settings.Default.Save();

            if (flacMidButton2.Checked == true)
            {
                logger.Debug("Setting format ID to 7");
                format_id = "7";
                audio_format = ".flac";
                Settings.Default.qualityFormat = format_id;
                Settings.Default.audioType = audio_format;
            }
        }

        private void flacLowButton2_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.quality2 = flacLowButton2.Checked;
            Settings.Default.Save();

            if (flacLowButton2.Checked == true)
            {
                logger.Debug("Setting format ID to 6");
                format_id = "6";
                audio_format = ".flac";
                Settings.Default.qualityFormat = format_id;
                Settings.Default.audioType = audio_format;
            }
        }

        private void mp3Button2_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.quality1 = mp3Button2.Checked;
            Settings.Default.Save();

            if (mp3Button2.Checked == true)
            {
                logger.Debug("Setting format ID to 5");
                format_id = "5";
                audio_format = ".mp3";
                Settings.Default.qualityFormat = format_id;
                Settings.Default.audioType = audio_format;
            }
        }

        private void flacHighLabel2_Click(object sender, EventArgs e)
        {
            flacHighButton2.Checked = true;
        }

        private void flacMidLabel2_Click(object sender, EventArgs e)
        {
            flacMidButton2.Checked = true;
        }

        private void flacLowLabel2_Click(object sender, EventArgs e)
        {
            flacLowButton2.Checked = true;
        }

        private void mp3Label2_Click(object sender, EventArgs e)
        {
            mp3Button2.Checked = true;
        }

        #endregion

        #region Tagging Options
        private void albumTitleCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.albumTag = albumTitleCheckbox.Checked;
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

        private void trackArtistCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.artistTag = trackArtistCheckbox.Checked;
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

        private void releaseDateCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.yearTag = releaseDateCheckbox.Checked;
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

        private void labelCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.labelTag = labelCheckbox.Checked;
            Settings.Default.Save();
        }

        private void isrcCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.isrcTag = isrcCheckbox.Checked;
            Settings.Default.Save();
        }

        private void urlCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.urlTag = urlCheckbox.Checked;
            Settings.Default.Save();
        }

        private void mergeArtistNamesCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.mergeArtistNames = mergeArtistNamesCheckbox.Checked;
            Settings.Default.Save();
        }

        private void releaseTypeCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.typeTag = releaseTypeCheckbox.Checked;
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

        private void coverArtCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.imageTag = coverArtCheckbox.Checked;
            Settings.Default.Save();
        }
        
        private void commentCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.commentTag = commentCheckbox.Checked;
            Settings.Default.Save();
        }

        private void commentTextbox_TextChanged(object sender, EventArgs e)
        {
            Settings.Default.commentText = commentTextbox.Text;
            Settings.Default.Save();
        }

        private void embeddedArtSizeSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Set artSize to selected value, and save selected option to settings.
            embeddedArtSize = embeddedArtSizeSelect.Text;
            Settings.Default.savedEmbeddedArtSize = embeddedArtSizeSelect.SelectedIndex;
            Settings.Default.Save();
        }

        private void savedArtSizeSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Set artSize to selected value, and save selected option to settings.
            savedArtSize = savedArtSizeSelect.Text;
            Settings.Default.savedSavedArtSize = savedArtSizeSelect.SelectedIndex;
            Settings.Default.Save();
        }

        private void themeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Save the selected theme name to settings
            string selectedTheme = themeComboBox.SelectedItem.ToString();
            Settings.Default.currentTheme = selectedTheme;
            Settings.Default.Save();

            // Load and apply the selected theme
            _themeManager.LoadTheme(selectedTheme);
            _themeManager.ApplyTheme(this);
        }

        private void languageComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Save the selected language to settings
            string selectedLanguage = languageComboBox.SelectedItem.ToString();
            Settings.Default.currentLanguage = selectedLanguage;
            Settings.Default.Save();

            // Load the selected language file when the user changes selection
            string filePath = Path.Combine(languageManager.languagesDirectory, selectedLanguage.ToLower() + ".json");

            if (ZlpIOHelper.FileExists(filePath))
            {
                languageManager.LoadLanguage(filePath);
                if (!firstLoadComplete) return; // Ignore initial load
                                                // Could use some work, but this works.
                string exePath = Application.ExecutablePath;
                Process.Start(exePath);
                Application.Exit();
            }
            else
            {
                MessageBox.Show(this, qbdlxForm._qbdlxForm.languageManager.GetTranslation("selectedLangFileNotfoundMsg"), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Navigation Buttons
        private void logoutButton_Click(object sender, EventArgs e)
        {
            logger.Debug("Restarting program to logout");
            // Could use some work, but this works.
            string exePath = Application.ExecutablePath;
            Process.Start(exePath);
            Application.Exit();
        }

        private void aboutButton_Click(object sender, EventArgs e)
        {
            logger.Debug("Opening about panel");
            // Make other panels invisable, make about panel visible
            downloaderPanel.Visible = false;
            searchPanel.Visible = false;
            settingsPanel.Visible = false;
            extraSettingsPanel.Visible = false;
            aboutPanel.Visible = true;

            // Make this the active panel
            downloadPanelActive = false;
            settingsPanelActive = false;
            aboutPanelActive = true;

            // Change button colors
            downloaderButton.BackColor = ColorTranslator.FromHtml(_themeManager._currentTheme.ButtonBackground);
            settingsButton.BackColor = ColorTranslator.FromHtml(_themeManager._currentTheme.ButtonBackground);
            searchButton.BackColor = ColorTranslator.FromHtml(_themeManager._currentTheme.ButtonBackground);
            aboutButton.BackColor = ColorTranslator.FromHtml(_themeManager._currentTheme.HighlightedButtonBackground);
        }

        private void settingsButton_Click(object sender, EventArgs e)
        {
            logger.Debug("Opening settings panel");
            // Make other panels invisable, make settings panel visible
            downloaderPanel.Visible = false;
            searchPanel.Visible = false;
            aboutPanel.Visible = false;
            settingsPanel.Visible = true;

            // Make this the active panel
            downloadPanelActive = false;
            aboutPanelActive = false;
            extraSettingsPanel.Visible = false;
            settingsPanelActive = true;

            // Change button colors
            downloaderButton.BackColor = ColorTranslator.FromHtml(_themeManager._currentTheme.ButtonBackground);
            aboutButton.BackColor = ColorTranslator.FromHtml(_themeManager._currentTheme.ButtonBackground);
            searchButton.BackColor = ColorTranslator.FromHtml(_themeManager._currentTheme.ButtonBackground);
            settingsButton.BackColor = ColorTranslator.FromHtml(_themeManager._currentTheme.HighlightedButtonBackground);
        }

        public void downloaderButton_Click(object sender, EventArgs e)
        {
            logger.Debug("Opening download panel");
            // Make other panels invisable, make settings panel visible
            aboutPanel.Visible = false;
            searchPanel.Visible = false;
            settingsPanel.Visible = false;
            extraSettingsPanel.Visible = false;
            downloaderPanel.Visible = true;

            // Make this the active panel
            aboutPanelActive = false;
            settingsPanelActive = false;
            downloadPanelActive = true;

            // Change button colors
            aboutButton.BackColor = ColorTranslator.FromHtml(_themeManager._currentTheme.ButtonBackground);
            settingsButton.BackColor = ColorTranslator.FromHtml(_themeManager._currentTheme.ButtonBackground);
            searchButton.BackColor = ColorTranslator.FromHtml(_themeManager._currentTheme.ButtonBackground);
            downloaderButton.BackColor = ColorTranslator.FromHtml(_themeManager._currentTheme.HighlightedButtonBackground);
        }

        private void searchButton_Click(object sender, EventArgs e)
        {
            logger.Debug("Opening search panel");
            // Make other panels invisable, make settings panel visible
            aboutPanel.Visible = false;
            settingsPanel.Visible = false;
            extraSettingsPanel.Visible = false;
            downloaderPanel.Visible = false;
            searchPanel.Visible = true;

            // Make this the active panel
            aboutPanelActive = false;
            settingsPanelActive = false;
            downloadPanelActive = true;

            // Change button colors
            aboutButton.BackColor = ColorTranslator.FromHtml(_themeManager._currentTheme.ButtonBackground);
            settingsButton.BackColor = ColorTranslator.FromHtml(_themeManager._currentTheme.ButtonBackground);
            downloaderButton.BackColor = ColorTranslator.FromHtml(_themeManager._currentTheme.ButtonBackground);
            searchButton.BackColor = ColorTranslator.FromHtml(_themeManager._currentTheme.HighlightedButtonBackground);
        }

        private void additionalSettingsButton_Click(object sender, EventArgs e)
        {
            logger.Debug("Opening extra settings panel");
            // Make other panels invisable, make settings panel visible
            aboutPanel.Visible = false;
            searchPanel.Visible = false;
            downloaderPanel.Visible = false;
            settingsPanel.Visible = false;
            extraSettingsPanel.Visible = true;

            // Make settings the active panel
            aboutPanelActive = false;
            downloadPanelActive = false;
            settingsPanelActive = true;

            // Change button colors
            aboutButton.BackColor = ColorTranslator.FromHtml(_themeManager._currentTheme.ButtonBackground);
            downloaderButton.BackColor = ColorTranslator.FromHtml(_themeManager._currentTheme.ButtonBackground);
            searchButton.BackColor = ColorTranslator.FromHtml(_themeManager._currentTheme.ButtonBackground);
            settingsButton.BackColor = ColorTranslator.FromHtml(_themeManager._currentTheme.HighlightedButtonBackground);
        }

        private void closeAdditionalButton_Click(object sender, EventArgs e)
        {
            logger.Debug("Closing extra settings panel");
            // Make other panels invisable, make settings panel visible
            aboutPanel.Visible = false;
            searchPanel.Visible = false;
            downloaderPanel.Visible = false;
            extraSettingsPanel.Visible = false;
            settingsPanel.Visible = true;

            // Make settings the active panel
            aboutPanelActive = false;
            downloadPanelActive = false;
            settingsPanelActive = true;

            // Change button colors
            aboutButton.BackColor = ColorTranslator.FromHtml(_themeManager._currentTheme.ButtonBackground);
            downloaderButton.BackColor = ColorTranslator.FromHtml(_themeManager._currentTheme.ButtonBackground);
            searchButton.BackColor = ColorTranslator.FromHtml(_themeManager._currentTheme.ButtonBackground);
            settingsButton.BackColor = ColorTranslator.FromHtml(_themeManager._currentTheme.HighlightedButtonBackground);
        }

        #endregion

        private void userInfoTextbox_GotFocus(object sender, EventArgs e)
        {
            NativeMethods.HideCaret(this.userInfoTextbox.Handle);
        }

        private void userInfoTextbox_MouseDown(object sender, MouseEventArgs e)
        {
            NativeMethods.HideCaret(this.userInfoTextbox.Handle);
        }

        private void userInfoTextbox_MouseUp(object sender, MouseEventArgs e)
        {
            NativeMethods.HideCaret(this.userInfoTextbox.Handle);
        }

        private void streamableCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.streamableCheck = streamableCheckbox.Checked;
            Settings.Default.Save();
        }

        private void useTLS13Checkbox_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.useTLS13 = useTLS13Checkbox.Checked;
            Settings.Default.Save();
            SetTLSSetting();
        }

        private void downloadGoodiesCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.downloadGoodies = downloadGoodiesCheckbox.Checked;
            Settings.Default.Save();
        }

        private void downloadArtistOtherCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.downloadArtistOther = downloadArtistOtherCheckbox.Checked;
            Settings.Default.Save();
        }

        private void downloadSpeedCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.showDownloadSpeed = downloadSpeedCheckbox.Checked;
            Settings.Default.Save();
        }

        private void fixMD5sCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.fixMD5s = fixMD5sCheckbox.Checked;
            Settings.Default.Save();
        }

        private void downloadOutput_TextChanged(object sender, EventArgs e)
        {
            // Deferred to avoid UI update race conditions
            this.BeginInvoke((Action)(() =>
            {
                downloadOutput.SelectionStart = downloadOutput.Text.Length;
                downloadOutput.SelectionLength = 0;
                downloadOutput.ScrollToCaret();
            }));
        }

        #region Window Moving

        // For moving form with click and drag
        private void logoPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                NativeMethods.ReleaseCapture();
                NativeMethods.SendMessage(Handle, Constants.WM_NCLBUTTONDOWN, Constants.HT_CAPTION, 0);
            }
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                NativeMethods.ReleaseCapture();
                NativeMethods.SendMessage(Handle, Constants.WM_NCLBUTTONDOWN, Constants.HT_CAPTION, 0);
            }
        }

        private void movingLabel_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                NativeMethods.ReleaseCapture();
                NativeMethods.SendMessage(Handle, Constants.WM_NCLBUTTONDOWN, Constants.HT_CAPTION, 0);
            }
        }

        private void downloadLabel_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                NativeMethods.ReleaseCapture();
                NativeMethods.SendMessage(Handle, Constants.WM_NCLBUTTONDOWN, Constants.HT_CAPTION, 0);
            }
        }

        private void settingsLabel_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                NativeMethods.ReleaseCapture();
                NativeMethods.SendMessage(Handle, Constants.WM_NCLBUTTONDOWN, Constants.HT_CAPTION, 0);
            }
        }

        private void extraSettingsLabel_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                NativeMethods.ReleaseCapture();
                NativeMethods.SendMessage(Handle, Constants.WM_NCLBUTTONDOWN, Constants.HT_CAPTION, 0);
            }
        }

        private void aboutLabel_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                NativeMethods.ReleaseCapture();
                NativeMethods.SendMessage(Handle, Constants.WM_NCLBUTTONDOWN, Constants.HT_CAPTION, 0);
            }
        }

        private void searchLabel_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                NativeMethods.ReleaseCapture();
                NativeMethods.SendMessage(Handle, Constants.WM_NCLBUTTONDOWN, Constants.HT_CAPTION, 0);
            }
        }

        #endregion

        private async void searchAlbumsButton_Click(object sender, EventArgs e)
        {
            logger.Debug("Hiding search buttons");
            searchAlbumsButton.Visible = false;
            searchTracksButton.Visible = false;
            searchingLabel.Visible = true;
            searchResultsPanel.Hide();

            string searchQuery = searchTextbox.Text;

            if (string.IsNullOrEmpty(searchQuery) | searchQuery == searchTextboxPlaceholder)
            {
                logger.Debug("Search query was null, canceling");
                searchResultsPanel.Show();
                searchAlbumsButton.Visible = true;
                searchTracksButton.Visible = true;
                searchingLabel.Visible = false;
                return;
            }

            try
            {
                logger.Debug("Search for releases started");
                await Task.Run(() => searchPanelHelper.SearchInitiate("releases", app_id, searchQuery, user_auth_token));
            }
            catch (Exception ex)
            {
                logger.Error("Error occured during searchAlbumsButton_Click, error below:\r\n" + ex);
                searchResultsPanel.Show();
                searchAlbumsButton.Visible = true;
                searchTracksButton.Visible = true;
                searchingLabel.Visible = false;
                return;
            }
            logger.Debug("Search completed!");
            searchResultsPanel.Show();
            searchAlbumsButton.Visible = true;
            searchTracksButton.Visible = true;
            searchingLabel.Visible = false;
            return;
        }

        private async void searchTracksButton_Click(object sender, EventArgs e)
        {
            logger.Debug("Hiding search buttons");
            searchAlbumsButton.Visible = false;
            searchTracksButton.Visible = false;
            searchingLabel.Visible = true;
            searchingLabel.Update();
            searchResultsPanel.Hide();

            string searchQuery = searchTextbox.Text;

            if (string.IsNullOrEmpty(searchQuery) | searchQuery == searchTextboxPlaceholder)
            {
                logger.Debug("Search query was null, canceling");
                searchResultsPanel.Show();
                searchAlbumsButton.Visible = true;
                searchTracksButton.Visible = true;
                searchingLabel.Visible = false;
                searchingLabel.Update();
                return;
            }

            try
            {
                logger.Debug("Search for tracks started");
                await Task.Run(() => searchPanelHelper.SearchInitiate("tracks", app_id, searchQuery, user_auth_token));
            }
            catch (Exception ex)
            {
                logger.Error("Error occured during searchTracksButton_Click, error below:\r\n" + ex);
                searchResultsPanel.Show();
                searchAlbumsButton.Visible = true;
                searchTracksButton.Visible = true;
                searchingLabel.Visible = false;
                searchingLabel.Update();
                return;
            }
            logger.Debug("Search completed!");
            searchResultsPanel.Show();
            searchAlbumsButton.Visible = true;
            searchTracksButton.Visible = true;
            searchingLabel.Visible = false;
            searchingLabel.Update();
            return;
        }

        private async Task reorderSearchResultsAsync()
        {
            qbdlxForm._qbdlxForm.searchSortingPanel.Enabled = false;

            logger.Debug("Hiding search buttons (sorting)");
            searchAlbumsButton.Visible = false;
            searchTracksButton.Visible = false;
            sortingSearchResultsLabel.Visible = true;
            sortingSearchResultsLabel.Update();
            searchResultsPanel.Hide();

            try
            {
                logger.Debug("Sorting search results started");
                await Task.Run(() =>
                {
                    if (searchPanelHelper.lastSearchType == "releases")
                    {
                        if (searchPanelHelper.QoAlbumSearch?.Albums != null)
                        {
                            searchPanelHelper.QoAlbumSearch.Albums = searchPanelHelper.SortAlbums(searchPanelHelper.QoAlbumSearch.Albums);
                            searchPanelHelper.PopulateTableAlbums(this, searchPanelHelper.QoAlbumSearch);
                        }
                    }
                    else if (searchPanelHelper.lastSearchType == "tracks")
                    {
                        if (searchPanelHelper.QoTrackSearch?.Tracks != null)
                        {
                            searchPanelHelper.QoTrackSearch.Tracks = searchPanelHelper.SortTracks(searchPanelHelper.QoTrackSearch.Tracks);
                            searchPanelHelper.PopulateTableTracks(this, searchPanelHelper.QoTrackSearch);
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                logger.Error("Error occured during reorderSearchResultsAsync(), error below:\r\n" + ex);
                qbdlxForm._qbdlxForm.searchSortingPanel.Enabled = true;
                searchResultsPanel.Show();
                searchAlbumsButton.Visible = true;
                searchTracksButton.Visible = true;
                sortingSearchResultsLabel.Visible = false;
                sortingSearchResultsLabel.Update();
                return;
            }
            logger.Debug("Sorting completed!");
            qbdlxForm._qbdlxForm.searchSortingPanel.Enabled = true;
            searchResultsPanel.Show();
            searchAlbumsButton.Visible = true;
            searchTracksButton.Visible = true;
            sortingSearchResultsLabel.Visible = false;
            sortingSearchResultsLabel.Update();
            return;
        }

        private void sortArtistNameLabel_Click(object sender, EventArgs e)
        {
            sortArtistNameButton.Checked = true;
        }

        private void sortAlbumTrackNameLabel_Click(object sender, EventArgs e)
        {
            sortAlbumTrackNameButton.Checked = true;
        }

        private void sortReleaseDateLabel_Click(object sender, EventArgs e)
        {
            sortReleaseDateButton.Checked = true;
        }

        private void sortGenreLabel_Click(object sender, EventArgs e)
        {
            sortGenreButton.Checked = true;
        }

        private async void sortArtistNameButton_CheckedChanged(object sender, EventArgs e)
        {
            if (sortArtistNameButton.Checked)
            {
                await reorderSearchResultsAsync();
            }
        }

        private async void sortAlbumTrackNameButton_CheckedChanged(object sender, EventArgs e)
        {
            if (sortAlbumTrackNameButton.Checked)
            {
                await reorderSearchResultsAsync();
            }
        }

        private async void sortReleaseDateButton_CheckedChanged(object sender, EventArgs e)
        {
            if (sortReleaseDateButton.Checked)
            {
                await reorderSearchResultsAsync();
            }
        }

        private async void sortGenreButton_CheckedChanged(object sender, EventArgs e)
        {
            if (sortGenreButton.Checked)
            {
                await reorderSearchResultsAsync();
            }
        }


        private async void sortAscendantCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            await reorderSearchResultsAsync();
        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            ToggleMainFormVisibility();
        }

        private void sysTrayContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            this.showWindowToolStripMenuItem.Visible = !this.Visible;
            this.hideWindowToolStripMenuItem.Visible = this.Visible;
        }

        private void showWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!this.Visible)
            {
                ToggleMainFormVisibility();
            }
        }

        private void hideWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (this.Visible)
            {
                ToggleMainFormVisibility();
            }
        }

        private void closeProgramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        [DebuggerStepThrough]
        public void ToggleMainFormVisibility()
        {
            void act()
            {
                if (this.Visible)
                {
                    this.Hide(); 
                    if (lastTaskBarProgressMaxValue > 0)
                    {
                        using (Icon baseIcon = (Icon)Properties.Resources.QBDLX_Icon1.Clone())
                        {
                            NotifyIconHelper.RenderNotifyIconProgressBar(
                                notifyIcon1,
                                baseIcon,
                                ntfyProgressBar,
                                lastTaskBarProgressCurrentValue,
                                lastTaskBarProgressMaxValue);
                        }
                    }
                }
                else
                {
                    if (!this.Visible)
                    {
                        this.Show();
                        this.notifyIcon1.Icon = Properties.Resources.QBDLX_Icon1;
                    }

                    this.WindowState = FormWindowState.Normal;
                    Process pr = Process.GetCurrentProcess();
                    IntPtr hwnd = pr.MainWindowHandle;
                    if (NativeMethods.IsIconic(hwnd))
                        NativeMethods.ShowWindow(hwnd, Constants.SW_RESTORE);

                    this.BringToFront();
                    this.Activate();

                    TaskbarHelper.SetProgressValue(qbdlxForm.lastTaskBarProgressCurrentValue, qbdlxForm.lastTaskBarProgressMaxValue);
                    TaskbarHelper.SetProgressState(qbdlxForm.lastTaskBarProgressState);
                }
            }

            if (this.InvokeRequired)
            {
                this.Invoke((Action)(() => act()));
            }
            else
            {
                act();
            }
        }

        private void inputTextbox_MouseDown(object sender, MouseEventArgs e)
        {
            var now = DateTime.Now;

            if (now - lastClickTime < tripleClickThreshold)
                clickCount++;
            else
                clickCount = 1;

            lastClickTime = now;

            if (clickCount == 3)
            {
                inputTextbox.SelectAll();
                clickCount = 0;
            }
        }

        private void searchTextbox_MouseDown(object sender, MouseEventArgs e)
        {
            var now = DateTime.Now;

            if (now - lastClickTime < tripleClickThreshold)
                clickCount++;
            else
                clickCount = 1;

            lastClickTime = now;

            if (clickCount == 3)
            {
                searchTextbox.SelectAll();
                clickCount = 0;
            }
        }

        public static void DeleteFilesFromTempFolder()
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string folderPath = Path.Combine(basePath, "qbdlx-temp");

            if (!Directory.Exists(folderPath))
                return;

            try
            {
                foreach (string file in Directory.GetFiles(folderPath))
                {
                    try
                    {
                        File.Delete(file);
                    }
                    catch
                    {
                        // Ignore file deletion errors
                    }
                }
            }
            catch
            {
                // Ignore directory access errors
            }
        }

        private void selectAllRowsButton_Click(object sender, EventArgs e)
        {
            searchResultsTablePanel.SuspendLayout();
            foreach (Control rowControl in searchResultsTablePanel.Controls)
            {
                rowControl.SuspendLayout();
                if (rowControl is Panel rowPanel && rowPanel.Tag is RowInfo info)
                {
                    if (!info.Selected)
                    {
                        var method = typeof(Control).GetMethod("InvokeOnClick", BindingFlags.Instance | BindingFlags.NonPublic);
                        method.Invoke(rowPanel, new object[] { rowPanel, EventArgs.Empty });
                    }
                }
                rowControl.ResumeLayout();
            }
            searchResultsTablePanel.ResumeLayout();
        }

        private void deselectAllRowsButton_Click(object sender, EventArgs e)
        {
            searchResultsTablePanel.SuspendLayout();
            foreach (Control rowControl in searchResultsTablePanel.Controls)
            {
                rowControl.SuspendLayout();
                if (rowControl is Panel rowPanel && rowPanel.Tag is RowInfo info)
                {
                    if (info.Selected)
                    {
                        var method = typeof(Control).GetMethod("InvokeOnClick", BindingFlags.Instance | BindingFlags.NonPublic);
                        method.Invoke(rowPanel, new object[] { rowPanel, EventArgs.Empty });
                    }
                }
                rowControl.ResumeLayout();
            }
            searchResultsTablePanel.ResumeLayout();
        }

        private async void batchDownloadSelectedRowsButton_Click(object sender, EventArgs e)
        {
            if (getLinkTypeIsBusy)
            {
                // This should never trigger.
                return;
            }

            batchDownloadSelectedRowsButton.Enabled = false;

            var batchUrls = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (int rowIndex in SearchPanelHelper.selectedRowindices)
            {
                Panel rowPanel = searchResultsTablePanel
                    .GetControlFromPosition(0, rowIndex) as Panel;

                var selectButton = rowPanel.Controls
                    .OfType<TableLayoutPanel>()
                    .SelectMany(t => t.Controls.OfType<Button>())
                    .FirstOrDefault(b => b.Tag is string);

                if (selectButton?.Tag is string url && !string.IsNullOrWhiteSpace(url))
                {
                    batchUrls.Add(url);
                }
            }

            await DownloadBatchUrls(batchUrls);

            batchDownloadSelectedRowsButton.Enabled = SearchPanelHelper.selectedRowindices.Any();
        }

        private void downloadButton_EnabledChanged(object sender, EventArgs e)
        {
            batchDownloadSelectedRowsButton.Enabled =
                downloadButton.Enabled &&
                !getLinkTypeIsBusy &&
                SearchPanelHelper.selectedRowindices.Any();
        }

        public static void SetTLSSetting()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                                                 | SecurityProtocolType.Tls11
                                                 | SecurityProtocolType.Tls12;

            if (Settings.Default.useTLS13)
            {
                ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls13;
            }
        }

        private void copyToclipboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var item = sender as ToolStripItem;
            var cms = item?.Owner as ContextMenuStrip;
            var ctrl = cms?.SourceControl;

            string text = ctrl.Text;
            SafeSetClipboardText(text);
        }

        private void SafeSetClipboardText(string text)
        {
            var thread = new Thread(() =>
            {
                Clipboard.SetText(text);
            });

            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();
        }

        private void mainContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            if (searchPanel.Visible)
            {
                copyThisRowToClipboardToolStripMenuItem.Text = languageManager.GetTranslation("copyThisRowToClipboard");
                copySelectedRowsToClipboardToolStripMenuItem.Text = languageManager.GetTranslation("copySelectedRowsToClipboard");
                copyAllRowsToClipboardToolStripMenuItem.Text = languageManager.GetTranslation("copyAllRowsToClipboard");
                copyToClipboardToolStripMenuItem.Visible = false;
                copyThisRowToClipboardToolStripMenuItem.Visible = true;
                copyAllRowsToClipboardToolStripMenuItem.Visible = true;
                copySelectedRowsToClipboardToolStripMenuItem.Visible = SearchPanelHelper.selectedRowindices.Any();
            }
            else
            {
                copyToClipboardToolStripMenuItem.Text = languageManager.GetTranslation("copyToClipboard");
                copyToClipboardToolStripMenuItem.Visible = true;
                copyThisRowToClipboardToolStripMenuItem.Visible = false;
                copySelectedRowsToClipboardToolStripMenuItem.Visible = false;
                copyAllRowsToClipboardToolStripMenuItem.Visible = false;
            }
        }

        private void copyAllRowsToClipboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var sb = new StringBuilder();

            int rowCount = searchResultsTablePanel.RowCount;

            for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
            {
                if (!(searchResultsTablePanel.GetControlFromPosition(0, rowIndex) is Panel rowPanel))
                    continue;

                var innerTlp = rowPanel.Controls.OfType<TableLayoutPanel>().FirstOrDefault();
                if (innerTlp == null)
                    continue;

                foreach (Control c in innerTlp.Controls)
                {
                    switch (c)
                    {
                        case System.Windows.Forms.Label lbl:
                            if (!string.IsNullOrEmpty(lbl.Text))
                            {
                                string cleanText = lbl.Text.Replace("\r\n", " - ");
                                sb.Append(cleanText + "; ");
                            }
                            break;

                        case Button btn:
                            if (btn.Tag != null)
                                sb.AppendLine(btn.Tag.ToString());
                            break;
                    }
                }
            }

            SafeSetClipboardText(sb.ToString());
        }

        private void copySelectedRowsToClipboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var sb = new StringBuilder();

            foreach (int rowIndex in SearchPanelHelper.selectedRowindices)
            {
                if (!(searchResultsTablePanel.GetControlFromPosition(0, rowIndex) is Panel rowPanel)) continue;

                var innerTlp = rowPanel.Controls.OfType<TableLayoutPanel>().FirstOrDefault();
                if (innerTlp != null)
                {
                    foreach (Control c in innerTlp.Controls)
                    {
                        switch (c)
                        {
                            case System.Windows.Forms.Label lbl:
                                if (!string.IsNullOrEmpty(lbl.Text))
                                {
                                    string cleanText = lbl.Text.Replace("\r\n", " - ");
                                    sb.Append(cleanText + "; ");
                                }
                                break;

                            case Button btn:
                                if (btn.Tag != null)
                                    sb.AppendLine(btn.Tag.ToString());
                                break;
                        }
                    }
                }
            }

            SafeSetClipboardText(sb.ToString());
        }

        private void copyThisRowToClipboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var item = sender as ToolStripItem;
            var cms = item?.Owner as ContextMenuStrip;
            var sourceCtrl = cms?.SourceControl;

            if (sourceCtrl == null) return;

            Panel rowPanel = sourceCtrl.Parent as Panel;
            if (rowPanel == null) return;

            var innerTlp = rowPanel.Controls.OfType<TableLayoutPanel>().FirstOrDefault();
            if (innerTlp == null) return;

            var sb = new StringBuilder();

            foreach (Control c in innerTlp.Controls)
            {
                switch (c)
                {
                    case System.Windows.Forms.Label lbl:
                        if (!string.IsNullOrEmpty(lbl.Text))
                        {
                            string cleanText = lbl.Text.Replace("\r\n", " - ");
                            sb.Append(cleanText + "; ");
                        }
                        break;

                    case Button btn:
                        if (btn.Tag != null)
                            sb.AppendLine(btn.Tag.ToString());
                        break;
                }
            }

            SafeSetClipboardText(sb.ToString());
        }
    }

    // Upsides of this buffering approach:
    //
    //   - Thread-safe file writes.
    //
    //   - Flexibility in the write/flush calls, which:
    //       Minimizes application slowdown by performing less writes to large log files.
    //       Minimizes disk drive wear and tear by limiting unnecessary I/O (read/write) operations.
    //
    // Downsides of this buffering approach:
    //
    //   - For those who like to see log files updating in real time,
    //     log entries are buffered so will not appear in the file immediately.
    //
    //   - If the process is terminated abruptly (e.g., using Task Manager),
    //     any remaining log entries in '_buffer' cannot be written to log file. 
    public sealed class BufferedLogger : IDisposable
    {
        private const int flushThreshold = 1024 * 1024; // 1 MB
        private const long maxLogFileSize = 50L * 1024L * 1024L; // 50 MB

        private string _filePath;
        private StreamWriter _writer;
        private readonly StringBuilder _buffer;
        private bool _disposed = false; 
        private readonly object _lock = new object();

        public BufferedLogger(string filePath)
        {
            _filePath = filePath;
            _buffer = new StringBuilder();

            _writer = new StreamWriter(new FileStream(_filePath, FileMode.Append, FileAccess.Write, FileShare.Read), Encoding.UTF8)
            {
                AutoFlush = false
            };
        }

        private void WriteLog(string level, string message)
        {
            var logMessage = $"[{DateTime.Now}] [{level}] {message}";

            lock (_lock) // Thread-safety
            {
                if (_disposed)
                {
                    // Ignore writes after a 'Logger.Dispose()' call
                    return;
                }

                try
                {
                    this.RotateToNewLogFileIfNeeded();

                    // Write to console immediately
                    System.Diagnostics.Debug.WriteLine($"{level} | {message}");

                    // Add to buffer
                    _buffer.AppendLine(logMessage);

                    // If buffer exceeds threshold, flush to file
                    if (_buffer.Length >= BufferedLogger.flushThreshold)
                    {
                        _writer.Write(_buffer.ToString());
                        _buffer.Clear();
                        _writer.Flush();
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Somehow, the log failed to write, lol");
                    System.Diagnostics.Debug.WriteLine(logMessage);
                    System.Diagnostics.Debug.WriteLine(ex);
                }
            }
        }

        public void Debug(string message) => WriteLog("DEBUG", message);

        public void Info(string message) => WriteLog("INFO", message);

        public void Warning(string message) => WriteLog("WARNING", message);

        public void Error(string message) => WriteLog("ERROR", message);

        private void RotateToNewLogFileIfNeeded()
        {
            if (!ZlpIOHelper.FileExists(_filePath))
                return;

            long size = new FileInfo(_filePath).Length;

            if (size < BufferedLogger.maxLogFileSize)
                return;

            if (_buffer.Length > 0)
            {
                _writer.Write(_buffer.ToString());
                _buffer.Clear();
                _writer.Flush();
            }

            _writer.Dispose();

            string newName = this.GetUniqueFileName(_filePath);
            System.Diagnostics.Debug.WriteLine($"Rotating to new log file: {newName}");

            _writer = new StreamWriter(new FileStream(newName, FileMode.Append, FileAccess.Write, FileShare.None), Encoding.UTF8)
            {
                AutoFlush = false
            };

            _filePath = newName;
        }

        private string GetUniqueFileName(string fullPath)
        {
            string folder = Path.GetDirectoryName(fullPath) ?? throw new ArgumentException("Invalid path: " + fullPath);
            string file = Path.GetFileName(fullPath);
            string pszPathForApi = fullPath;

            const int MAX_PATH = 260;
            StringBuilder sb = new StringBuilder(MAX_PATH);

            bool ok = NativeMethods.PathYetAnotherMakeUniqueName(
                sb,
                pszPathForApi, // full path + name
                null,          // pszShort = null -> base on long name
                null           // optional pszFileSpec, can be left null
            );

            if (!ok)
            {
                // The function can return FALSE in case of truncation or other failure.
                throw new IOException("PathYetAnotherMakeUniqueName failed.");
            }

            string result = sb.ToString();

            // Extra safety: if the API returns exactly the same name
            // and the file already exists, use a manual fallback.
            if (string.Equals(result, fullPath, StringComparison.OrdinalIgnoreCase) && ZlpIOHelper.FileExists(fullPath))
            {
                string baseName = Path.GetFileNameWithoutExtension(file);
                string ext = Path.GetExtension(file);
                int count = 1;
                string candidate;
                do
                {
                    candidate = Path.Combine(folder, string.Format("{0} ({1}){2}", baseName, count, ext));
                    count++;
                } while (ZlpIOHelper.FileExists(candidate));
                return candidate;
            }

            return result;
        }

        public void Dispose()
        {
            lock (_lock)
            {
                if (!_disposed)
                {
                    try
                    {
                        // Flush any remaining buffered logs
                        if (_buffer.Length > 0)
                        {
                            _writer.Write(_buffer.ToString());
                            _buffer.Clear();
                            _writer.Flush();
                        }

                        _writer.Dispose();
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine("Failed to flush logs to log file on Logger.Dispose().");
                        System.Diagnostics.Debug.WriteLine(ex);
                    }

                    _disposed = true;
                }
            }
        }
    }
}
