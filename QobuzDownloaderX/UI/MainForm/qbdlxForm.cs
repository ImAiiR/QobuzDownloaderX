using QobuzDownloaderX.Helpers;
using QobuzDownloaderX.Helpers.QobuzDownloaderXMOD;
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
        internal BufferedLogger logger { get; set; }

        // Create theme and language options
        internal Theme theme { get; set; }
        internal LanguageManager languageManager;

        internal Service QoService = new Service();
        internal User QoUser = new User();
        internal Artist QoArtist = new Artist();
        internal Album QoAlbum = new Album();
        internal Item QoItem = new Item();
        internal SearchAlbumResult QoAlbumSearch = new SearchAlbumResult();
        internal SearchTrackResult QoTrackSearch = new SearchTrackResult();
        internal Favorites QoFavorites = new Favorites();
        internal Playlist QoPlaylist = new Playlist();
        internal QopenAPI.Label QoLabel = new QopenAPI.Label();
        internal QopenAPI.Stream QoStream = new QopenAPI.Stream();

        internal bool downloadPanelActive = false;
        internal bool aboutPanelActive = false;
        internal bool settingsPanelActive = false;
        private bool firstLoadComplete = false;

        internal string downloadLocation { get; set; }
        internal string artistTemplate { get; set; }
        internal string albumTemplate { get; set; }
        internal string trackTemplate { get; set; }
        internal string playlistTemplate { get; set; }
        internal string favoritesTemplate { get; set; }

        internal string app_id { get; set; }
        internal string app_secret { get; set; }
        internal string user_id { get; set; }
        internal string user_auth_token { get; set; }
        internal string user_display_name { get; set; }
        internal string user_label { get; set; }
        internal string user_avatar { get; set; }

        internal string qobuz_id { get; set; }
        internal string format_id { get; set; }
        internal string audio_format { get; set; }

        internal string embeddedArtSize { get; set; }
        internal string savedArtSize { get; set; }
        internal string themeName { get; set; }

        // Used to signal and manage 'getLinkTypeAsync' cancellation.
        internal CancellationTokenSource abortTokenSource;

        // Local flag that indicates whether 'getLinkTypeAsync' is executing.
        internal static bool getLinkTypeIsBusy;

        // Global flag that indicates whether a batch download is running.
        internal static bool isBatchDownloadRunning;

        // Global flag that indicates whether the current album download must be skipped in the current 'getLinkTypeAsync' execution.
        internal static bool skipCurrentAlbum;

        // Global flag that keeps track of the last current taskbar progress value to restore it when minimizing and restoring the main form.
        internal static int lastTaskBarProgressCurrentValue;

        // Global flag that keeps track of the last maximum taskbar progress value to restore it when minimizing and restoring the main form.
        internal static int lastTaskBarProgressMaxValue;

        // Global flag that keeps track of the last taskbar progress state to restore it when minimizing and restoring the main form.
        internal static TaskbarProgressState lastTaskBarProgressState;

        internal static NotifyIconProgressBar ntfyProgressBar = new NotifyIconProgressBar
        {
            Height = 8,
            BorderColor = Color.Black,
            BorderWidth = 1
        };

        // used for triple-click / select all text support.
        private int clickCount = 0;
        private DateTime lastClickTime = DateTime.MinValue;
        private readonly TimeSpan tripleClickThreshold = TimeSpan.FromMilliseconds(SystemInformation.DoubleClickTime);

        #region Language
        internal string userInfoTextboxPlaceholder { get; set; }
        internal string albumLabelPlaceholder { get; set; }
        internal string artistLabelPlaceholder { get; set; }
        internal string infoLabelPlaceholder { get; set; }
        internal string inputTextboxPlaceholder { get; set; }
        internal string searchTextboxPlaceholder { get; set; }
        internal string downloadFolderPlaceholder { get; set; }
        internal string downloadOutputWelcome { get; set; }
        internal string downloadOutputExpired { get; set; }
        internal string downloadOutputPath { get; set; }
        internal string downloadOutputNoPath { get; set; }
        internal string downloadOutputNoUrl { get; set; }
        internal string downloadOutputAPIError { get; set; }
        internal string downloadOutputNotImplemented { get; set; }
        internal string downloadOutputCheckLink { get; set; }
        internal string downloadOutputTrNotStream { get; set; }
        internal string downloadOutputAlNotStream { get; set; }
        internal string downloadOutputGoodyFound { get; set; }
        internal string downloadOutputGoodyExists { get; set; }
        internal string downloadOutputGoodyNoURL { get; set; }
        internal string downloadOutputFileExists { get; set; }
        internal string downloadOutputDownloading { get; set; }
        internal string downloadOutputDone { get; set; }
        internal string downloadOutputCompleted { get; set; }
        internal string progressLabelInactive { get; set; }
        internal string progressLabelActive { get; set; }
        internal string formClosingWarning { get; set; }
        internal string downloadAborted { get; set; }
        internal string albumSkipped { get; set; }
        #endregion

        internal int currentTipIndex = 0;
        internal string currentTipText = "";
        private string tipScroll = "";

        internal readonly GetInfo getInfo = new GetInfo();
        internal readonly RenameTemplates renameTemplates = new RenameTemplates();
        internal readonly DownloadAlbum downloadAlbum = new DownloadAlbum();
        internal readonly DownloadTrack downloadTrack = new DownloadTrack();
        internal readonly SearchPanelHelper searchPanelHelper = new SearchPanelHelper();

        internal static readonly Regex qobuzStoreLinkRegex = new Regex(
            @"https:\/\/(?:.*?).qobuz.com\/(?<region>.*?)\/(?<type>.*?)\/(?<name>.*?)\/(?<id>.*?)$", RegexOptions.Compiled);

        internal static readonly Regex qobuzLinkIdGrabRegex = new Regex(
            @"https:\/\/(?:.*?).qobuz.com\/(?<type>.*?)\/(?<id>.*?)$", RegexOptions.Compiled);

        internal static readonly Regex qobuzUrlRegEx = new Regex(
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

        internal qbdlxForm()
        {
            // Create new log file
            Directory.CreateDirectory("logs");

            logger = new BufferedLogger("logs\\QobuzDLX " + DateTime.Now.ToString("yyyy⧸MM⧸dd HH꞉mm꞉ss") + ".log");
            logger.Debug("Logger started, QBDLX form initialized!");

            InitializeComponent();
            _qbdlxForm = this;
        }

        internal static qbdlxForm _qbdlxForm;
        internal readonly Theming _themeManager = new Theming();

        private void qbdlxForm_Load(object sender, EventArgs e)
        {
            logger.Debug("QBDLX form loading!");

            Miscellaneous.ClearOldLogs();
            Miscellaneous.DeleteFilesFromTempFolder();

            this.DoubleBuffered = true;

            // Round corners of form
            Region = Region.FromHrgn(NativeMethods.CreateRoundRectRgn(0, 0, this.Width, this.Height, 20, 20));

            // Load settings / download location / theme / language / panels
            Miscellaneous.LoadSavedTemplates(this);
            Miscellaneous.LoadQualitySettings(this);
            Miscellaneous.LoadTaggingSettings(this);
            Miscellaneous.LoadOtherSettings(this);
            Miscellaneous.InitializeTheme(this);
            Miscellaneous.InitializePanels(this);
            Miscellaneous.InitializeLanguage(this);
            Miscellaneous.SetDownloadPath(this);

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
            Miscellaneous.InitTipTicker(this);
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
                    }
                    else
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
                }
                else
                {
                    MessageBox.Show(this, languageManager.GetTranslation("downloadOutputDontExistMsg"),
                        Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

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
                if (downloadButton.Enabled)
                {
                    downloadButton.PerformClick();
                }
            }
        }

        private void inputTextbox_TextChanged(object sender, EventArgs e)
        {
            string text = inputTextbox.Text.TrimStart();
            downloadButton.Enabled = (text.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
                                      text.StartsWith("https://", StringComparison.OrdinalIgnoreCase) ||
                                      text.StartsWith("www.", StringComparison.OrdinalIgnoreCase));
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
            await Miscellaneous.downloadButtonAsyncWork(this);
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

                await Miscellaneous.DownloadBatchUrls(this, batchUrls);
            }
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
            getAllBatchDownloadButton.Enabled =
                text.IndexOf("http://", StringComparison.OrdinalIgnoreCase) >= 0 ||
                text.IndexOf("https://", StringComparison.OrdinalIgnoreCase) >= 0 ||
                text.IndexOf("www.", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private void inputTextbox_Click(object sender, EventArgs e)
        {
            Miscellaneous.SetPlaceholder(this, inputTextbox, inputTextboxPlaceholder, true);
        }

        private void inputTextbox_Leave(object sender, EventArgs e)
        {
            Miscellaneous.SetPlaceholder(this, inputTextbox, inputTextboxPlaceholder, false);
        }

        private void searchTextbox_Click(object sender, EventArgs e)
        {
            Miscellaneous.SetPlaceholder(this, searchTextbox, searchTextboxPlaceholder, true);
        }

        private void searchTextbox_Leave(object sender, EventArgs e)
        {
            Miscellaneous.SetPlaceholder(this, searchTextbox, searchTextboxPlaceholder, false);
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
            Miscellaneous.updateTemplates(this);
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
                Miscellaneous.UpdateQualitySelectButtonText(this);
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
                Miscellaneous.UpdateQualitySelectButtonText(this);
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
                Miscellaneous.UpdateQualitySelectButtonText(this);
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
                Miscellaneous.UpdateQualitySelectButtonText(this);
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
            CheckBox cb = (CheckBox)sender;

            artistNamesSeparatorsPanel.Enabled = cb.Checked;
            Settings.Default.mergeArtistNames = cb.Checked;
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

        private void useItemPosInPlaylistCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.useItemPosInPlaylist = useItemPosInPlaylistCheckbox.Checked;
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
            if (Miscellaneous.ShowDownloadFromArtistWarningIfNeeded()) return;

            logger.Debug("Restarting program to logout");
            // Could use some work, but this works.
            string exePath = Application.ExecutablePath;
            Process.Start(exePath);
            Application.Exit();
        }

        private void aboutButton_Click(object sender, EventArgs e)
        {
            if (Miscellaneous.ShowDownloadFromArtistWarningIfNeeded()) return;

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
            if (extraSettingsPanel.Visible && Miscellaneous.ShowDownloadFromArtistWarningIfNeeded()) return;

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

        internal void downloaderButton_Click(object sender, EventArgs e)
        {
            if (Miscellaneous.ShowDownloadFromArtistWarningIfNeeded()) return;

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
            if (Miscellaneous.ShowDownloadFromArtistWarningIfNeeded()) return;

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
            if (Miscellaneous.ShowDownloadFromArtistWarningIfNeeded()) return;

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
            Miscellaneous.SetTLSSetting();
        }

        private void downloadGoodiesCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.downloadGoodies = downloadGoodiesCheckbox.Checked;
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

        private void clearOldLogsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.clearOldLogs = clearOldLogsCheckBox.Checked;
            Settings.Default.Save();
        }

        private void dontSaveArtworkToDiskCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            savedArtSizeSelect.Enabled = !dontSaveArtworkToDiskCheckBox.Checked;
            Settings.Default.dontSaveArtworkToDisk = dontSaveArtworkToDiskCheckBox.Checked;
            Settings.Default.Save();
        }

        private void downloadFromArtistListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Miscellaneous.SaveDownloadFromArtistSelectedIndices();
        }

        private void downloadAllFromArtistCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            var lb = downloadAllFromArtistCheckBox;
            downloadFromArtistListBox.Enabled = !lb.Checked;

            if (lb.Checked)
            {
                for (int i = 0; i < downloadFromArtistListBox.Items.Count; i++)
                {
                    downloadFromArtistListBox.SetItemChecked(i, true);
                }
            }
            Settings.Default.downloadAllFromArtist = lb.Checked;
            Settings.Default.Save();
        }

        private void primaryListSeparatorTextBox_Leave(object sender, EventArgs e)
        {
            TextBox tb = (TextBox)sender;

            if (string.IsNullOrEmpty(tb.Text))
            {
                tb.Text = ", ";
                tb.SelectionStart = tb.Text.Length;
            }

            ParsingHelper.primaryListSeparator = tb.Text;
            Settings.Default.primaryListSeparator = tb.Text;
            Settings.Default.Save();
        }

        private void listEndSeparatorTextBox_Leave(object sender, EventArgs e)
        {
            TextBox tb = (TextBox)sender;

            if (string.IsNullOrEmpty(tb.Text))
            {
                tb.Text = " & ";
                tb.SelectionStart = tb.Text.Length;
            }

            ParsingHelper.listEndSeparator = tb.Text;
            Settings.Default.listEndSeparator = tb.Text;
            Settings.Default.Save();
        }

        private void showTipsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.showTips = showTipsCheckBox.Checked;
            Settings.Default.Save();

            Miscellaneous.InitTipTicker(this);
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
                await Miscellaneous.reorderSearchResultsAsync(this);
            }
        }

        private async void sortAlbumTrackNameButton_CheckedChanged(object sender, EventArgs e)
        {
            if (sortAlbumTrackNameButton.Checked)
            {
                await Miscellaneous.reorderSearchResultsAsync(this);
            }
        }

        private async void sortReleaseDateButton_CheckedChanged(object sender, EventArgs e)
        {
            if (sortReleaseDateButton.Checked)
            {
                await Miscellaneous.reorderSearchResultsAsync(this);
            }
        }

        private async void sortGenreButton_CheckedChanged(object sender, EventArgs e)
        {
            if (sortGenreButton.Checked)
            {
                await Miscellaneous.reorderSearchResultsAsync(this);
            }
        }

        private async void sortAscendantCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            await Miscellaneous.reorderSearchResultsAsync(this);
        }

        private void sysTrayContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            this.showWindowToolStripMenuItem.Visible = !this.Visible;
            this.hideWindowToolStripMenuItem.Visible = this.Visible;
        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            Miscellaneous.ToggleMainFormVisibility(this);
        }

        private void showWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!this.Visible)
            {
                Miscellaneous.ToggleMainFormVisibility(this);
            }
        }

        private void hideWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (this.Visible)
            {
                Miscellaneous.ToggleMainFormVisibility(this);
            }
        }

        private void closeProgramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
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

            await Miscellaneous.DownloadBatchUrls(this, batchUrls);

            batchDownloadSelectedRowsButton.Enabled = SearchPanelHelper.selectedRowindices.Any();
        }

        private void downloadButton_EnabledChanged(object sender, EventArgs e)
        {
            batchDownloadSelectedRowsButton.Enabled =
                downloadButton.Enabled &&
                !getLinkTypeIsBusy &&
                SearchPanelHelper.selectedRowindices.Any();
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
                copySelectedRowsToClipboardToolStripMenuItem.Visible = SearchPanelHelper.selectedRowindices.Any();
                copyAllRowsToClipboardToolStripMenuItem.Visible = searchResultsTablePanel.Controls.Count > 1;
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

        private void copyToclipboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var item = sender as ToolStripItem;
            var cms = item?.Owner as ContextMenuStrip;
            var ctrl = cms?.SourceControl;

            string text = ctrl.Text;
            Miscellaneous.SafeSetClipboardText(text);
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

            Miscellaneous.SafeSetClipboardText(sb.ToString());
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

            Miscellaneous.SafeSetClipboardText(sb.ToString());
        }

        private void copyThisRowToClipboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var item = sender as ToolStripItem;
            var cms = item?.Owner as ContextMenuStrip;
            var sourceCtrl = cms?.SourceControl;

            if (sourceCtrl == null) return;

            if (!(sourceCtrl.Parent is Panel rowPanel)) return;

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

            Miscellaneous.SafeSetClipboardText(sb.ToString());
        }

        private void albumPictureBox_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;

            if (!string.IsNullOrEmpty(albumPictureBox.ImageLocation))
            {
                if ((string)albumPictureBox.Tag == "playlist")
                {
                    Miscellaneous.ShowFloatingImageFromUrl(QoPlaylist.ImageRectangle[0]);
                }
                else
                {
                    Miscellaneous.ShowFloatingImageFromUrl(QoAlbum.Image?.Large);
                }

                this.BringToFront();
            }
        }

        private void nextTipButton_Click(object sender, EventArgs e)
        {
            Miscellaneous.SetNextTip(this, forward: true);
            tipScroll = currentTipText;
        }

        private void prevTipButton_Click(object sender, EventArgs e)
        {
            Miscellaneous.SetNextTip(this, forward: false);
            tipScroll = currentTipText;
        }

        private void timerTip_Tick_1(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tipScroll))
            {
                Miscellaneous.SetNextTip(this, forward: true);
                tipScroll = currentTipText;
            }

            if (string.IsNullOrEmpty(tipScroll) || tipScroll.Length < 2)
            {
                tipLabel.Text = tipScroll;
                return;
            }

            tipScroll = tipScroll.Substring(1) + tipScroll[0];
            tipLabel.Text = tipScroll;
        }

    }
}
