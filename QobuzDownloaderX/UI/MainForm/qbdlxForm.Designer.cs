using QobuzDownloaderX.UserControls;
using QobuzDownloaderX.Win32;
using System.Drawing;
using System.Windows.Forms;

namespace QobuzDownloaderX
{
    partial class qbdlxForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(qbdlxForm));
            this.navigationPanel = new System.Windows.Forms.Panel();
            this.searchButton = new System.Windows.Forms.Button();
            this.welcomeLabel = new System.Windows.Forms.Label();
            this.settingsButton = new System.Windows.Forms.Button();
            this.logoutButton = new System.Windows.Forms.Button();
            this.aboutButton = new System.Windows.Forms.Button();
            this.downloaderButton = new System.Windows.Forms.Button();
            this.logoPanel = new System.Windows.Forms.Panel();
            this.versionNumber = new System.Windows.Forms.Label();
            this.logoPictureBox = new System.Windows.Forms.PictureBox();
            this.downloaderPanel = new System.Windows.Forms.Panel();
            this.batchDownloadProgressCountLabel = new System.Windows.Forms.Label();
            this.progressItemsCountLabel = new System.Windows.Forms.Label();
            this.batchDownloadPanel = new System.Windows.Forms.Panel();
            this.batchDownloadTextBox = new System.Windows.Forms.TextBox();
            this.batchDownloadLabel = new System.Windows.Forms.Label();
            this.getAllBatchDownloadButton = new System.Windows.Forms.Button();
            this.closeBatchDownloadbutton = new System.Windows.Forms.Button();
            this.batchDownloadButton = new System.Windows.Forms.Button();
            this.skipButton = new System.Windows.Forms.Button();
            this.abortButton = new System.Windows.Forms.Button();
            this.progressLabel = new System.Windows.Forms.Label();
            this.downloadButton = new System.Windows.Forms.Button();
            this.infoLabel = new System.Windows.Forms.Label();
            this.albumLabel = new System.Windows.Forms.Label();
            this.artistLabel = new System.Windows.Forms.Label();
            this.downloadOutput = new System.Windows.Forms.TextBox();
            this.albumPictureBox = new System.Windows.Forms.PictureBox();
            this.inputTextbox = new System.Windows.Forms.TextBox();
            this.downloadLabel = new System.Windows.Forms.Label();
            this.settingsPanel = new System.Windows.Forms.Panel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.resetTemplatesButton = new System.Windows.Forms.Button();
            this.saveTemplatesButton = new System.Windows.Forms.Button();
            this.folderButtonsPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.selectFolderButton = new System.Windows.Forms.Button();
            this.openFolderButton = new System.Windows.Forms.Button();
            this.templatesListLabel = new System.Windows.Forms.Label();
            this.templatesListTextbox = new System.Windows.Forms.TextBox();
            this.additionalSettingsButton = new System.Windows.Forms.Button();
            this.trackTemplateTextbox = new System.Windows.Forms.TextBox();
            this.trackTemplateLabel = new System.Windows.Forms.Label();
            this.playlistTemplateTextbox = new System.Windows.Forms.TextBox();
            this.playlistTemplateLabel = new System.Windows.Forms.Label();
            this.artistTemplateTextbox = new System.Windows.Forms.TextBox();
            this.favoritesTemplateTextbox = new System.Windows.Forms.TextBox();
            this.albumTemplateTextbox = new System.Windows.Forms.TextBox();
            this.downloadFolderTextbox = new System.Windows.Forms.TextBox();
            this.artistTemplateLabel = new System.Windows.Forms.Label();
            this.favoritesTemplateLabel = new System.Windows.Forms.Label();
            this.albumTemplateLabel = new System.Windows.Forms.Label();
            this.downloadFolderLabel = new System.Windows.Forms.Label();
            this.downloadOptionsLabel = new System.Windows.Forms.Label();
            this.templatesLabel = new System.Windows.Forms.Label();
            this.settingsLabel = new System.Windows.Forms.Label();
            this.userInfoTextbox = new System.Windows.Forms.TextBox();
            this.mainContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copyToClipboardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyThisRowToClipboardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copySelectedRowsToClipboardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyAllRowsToClipboardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.userInfoLabel = new System.Windows.Forms.Label();
            this.disclaimerLabel = new System.Windows.Forms.Label();
            this.exitButton = new System.Windows.Forms.Button();
            this.minimizeButton = new System.Windows.Forms.Button();
            this.aboutPanel = new System.Windows.Forms.Panel();
            this.aboutLabel = new System.Windows.Forms.Label();
            this.folderBrowser = new Ookii.Dialogs.WinForms.VistaFolderBrowserDialog();
            this.extraSettingsPanel = new System.Windows.Forms.Panel();
            this.advancedOptionsPanelRight = new System.Windows.Forms.FlowLayoutPanel();
            this.downloadGoodiesCheckbox = new System.Windows.Forms.CheckBox();
            this.downloadArtistOtherCheckbox = new System.Windows.Forms.CheckBox();
            this.fixMD5sCheckbox = new System.Windows.Forms.CheckBox();
            this.clearOldLogsCheckBox = new System.Windows.Forms.CheckBox();
            this.advancedOptionsPanelLeft = new System.Windows.Forms.FlowLayoutPanel();
            this.useTLS13Checkbox = new System.Windows.Forms.CheckBox();
            this.streamableCheckbox = new System.Windows.Forms.CheckBox();
            this.downloadSpeedCheckbox = new System.Windows.Forms.CheckBox();
            this.mergeArtistNamesCheckbox = new System.Windows.Forms.CheckBox();
            this.commentLabel = new System.Windows.Forms.Label();
            this.dontSaveArtworkToDiskCheckBox = new System.Windows.Forms.CheckBox();
            this.taggingOptionsPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.albumArtistCheckbox = new System.Windows.Forms.CheckBox();
            this.albumTitleCheckbox = new System.Windows.Forms.CheckBox();
            this.trackArtistCheckbox = new System.Windows.Forms.CheckBox();
            this.trackTitleCheckbox = new System.Windows.Forms.CheckBox();
            this.releaseDateCheckbox = new System.Windows.Forms.CheckBox();
            this.releaseTypeCheckbox = new System.Windows.Forms.CheckBox();
            this.genreCheckbox = new System.Windows.Forms.CheckBox();
            this.trackNumberCheckbox = new System.Windows.Forms.CheckBox();
            this.trackTotalCheckbox = new System.Windows.Forms.CheckBox();
            this.discNumberCheckbox = new System.Windows.Forms.CheckBox();
            this.discTotalCheckbox = new System.Windows.Forms.CheckBox();
            this.composerCheckbox = new System.Windows.Forms.CheckBox();
            this.explicitCheckbox = new System.Windows.Forms.CheckBox();
            this.coverArtCheckbox = new System.Windows.Forms.CheckBox();
            this.copyrightCheckbox = new System.Windows.Forms.CheckBox();
            this.labelCheckbox = new System.Windows.Forms.CheckBox();
            this.upcCheckbox = new System.Windows.Forms.CheckBox();
            this.isrcCheckbox = new System.Windows.Forms.CheckBox();
            this.urlCheckbox = new System.Windows.Forms.CheckBox();
            this.languageLabel = new System.Windows.Forms.Label();
            this.languageComboBox = new System.Windows.Forms.ComboBox();
            this.themeLabel = new System.Windows.Forms.Label();
            this.themeComboBox = new System.Windows.Forms.ComboBox();
            this.themeSectionLabel = new System.Windows.Forms.Label();
            this.commentCheckbox = new System.Windows.Forms.CheckBox();
            this.commentTextbox = new System.Windows.Forms.TextBox();
            this.advancedOptionsLabel = new System.Windows.Forms.Label();
            this.closeAdditionalButton = new System.Windows.Forms.Button();
            this.savedArtLabel = new System.Windows.Forms.Label();
            this.savedArtSizeSelect = new System.Windows.Forms.ComboBox();
            this.taggingOptionsLabel = new System.Windows.Forms.Label();
            this.embeddedArtLabel = new System.Windows.Forms.Label();
            this.embeddedArtSizeSelect = new System.Windows.Forms.ComboBox();
            this.extraSettingsLabel = new System.Windows.Forms.Label();
            this.qualitySelectButton = new System.Windows.Forms.Button();
            this.qualitySelectPanel = new System.Windows.Forms.Panel();
            this.mp3Button2 = new System.Windows.Forms.RadioButton();
            this.flacLowButton2 = new System.Windows.Forms.RadioButton();
            this.flacMidButton2 = new System.Windows.Forms.RadioButton();
            this.flacHighButton2 = new System.Windows.Forms.RadioButton();
            this.mp3Label2 = new System.Windows.Forms.Label();
            this.flacLowLabel2 = new System.Windows.Forms.Label();
            this.flacMidLabel2 = new System.Windows.Forms.Label();
            this.flacHighLabel2 = new System.Windows.Forms.Label();
            this.movingLabel = new System.Windows.Forms.Label();
            this.searchPanel = new System.Windows.Forms.Panel();
            this.deselectAllRowsButton = new System.Windows.Forms.Button();
            this.selectAllRowsButton = new System.Windows.Forms.Button();
            this.batchDownloadSelectedRowsButton = new System.Windows.Forms.Button();
            this.selectedRowsCountLabel = new System.Windows.Forms.Label();
            this.limitSearchResultsLabel = new System.Windows.Forms.Label();
            this.searchResultsCountLabel = new System.Windows.Forms.Label();
            this.searchSortingLabel = new System.Windows.Forms.Label();
            this.searchSortingPanel = new System.Windows.Forms.Panel();
            this.sortGenreLabel = new System.Windows.Forms.Label();
            this.sortGenreButton = new System.Windows.Forms.RadioButton();
            this.sortAlbumTrackNameLabel = new System.Windows.Forms.Label();
            this.sortArtistNameLabel = new System.Windows.Forms.Label();
            this.sortAlbumTrackNameButton = new System.Windows.Forms.RadioButton();
            this.sortArtistNameButton = new System.Windows.Forms.RadioButton();
            this.sortReleaseDateButton = new System.Windows.Forms.RadioButton();
            this.sortReleaseDateLabel = new System.Windows.Forms.Label();
            this.limitSearchResultsNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.searchResultsPanel = new System.Windows.Forms.Panel();
            this.searchResultsTablePanel = new System.Windows.Forms.TableLayoutPanel();
            this.searchAlbumsButton = new System.Windows.Forms.Button();
            this.searchTracksButton = new System.Windows.Forms.Button();
            this.searchTextbox = new System.Windows.Forms.TextBox();
            this.searchLabel = new System.Windows.Forms.Label();
            this.searchingLabel = new System.Windows.Forms.Label();
            this.sortingSearchResultsLabel = new System.Windows.Forms.Label();
            this.sortAscendantCheckBox = new System.Windows.Forms.CheckBox();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.sysTrayContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.showWindowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hideWindowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeProgramToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.progressBarDownload = new QobuzDownloaderX.UserControls.CustomProgressBar();
            this.navigationPanel.SuspendLayout();
            this.logoPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.logoPictureBox)).BeginInit();
            this.downloaderPanel.SuspendLayout();
            this.batchDownloadPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.albumPictureBox)).BeginInit();
            this.settingsPanel.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.folderButtonsPanel.SuspendLayout();
            this.mainContextMenuStrip.SuspendLayout();
            this.aboutPanel.SuspendLayout();
            this.extraSettingsPanel.SuspendLayout();
            this.advancedOptionsPanelRight.SuspendLayout();
            this.advancedOptionsPanelLeft.SuspendLayout();
            this.taggingOptionsPanel.SuspendLayout();
            this.qualitySelectPanel.SuspendLayout();
            this.searchPanel.SuspendLayout();
            this.searchSortingPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.limitSearchResultsNumericUpDown)).BeginInit();
            this.searchResultsPanel.SuspendLayout();
            this.sysTrayContextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // navigationPanel
            // 
            this.navigationPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(13)))), ((int)(((byte)(13)))), ((int)(((byte)(13)))));
            this.navigationPanel.Controls.Add(this.searchButton);
            this.navigationPanel.Controls.Add(this.welcomeLabel);
            this.navigationPanel.Controls.Add(this.settingsButton);
            this.navigationPanel.Controls.Add(this.logoutButton);
            this.navigationPanel.Controls.Add(this.aboutButton);
            this.navigationPanel.Controls.Add(this.downloaderButton);
            this.navigationPanel.Controls.Add(this.logoPanel);
            this.navigationPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.navigationPanel.Location = new System.Drawing.Point(0, 0);
            this.navigationPanel.Name = "navigationPanel";
            this.navigationPanel.Size = new System.Drawing.Size(180, 615);
            this.navigationPanel.TabIndex = 0;
            // 
            // searchButton
            // 
            this.searchButton.FlatAppearance.BorderSize = 0;
            this.searchButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
            this.searchButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(18)))), ((int)(((byte)(18)))));
            this.searchButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.searchButton.Font = new System.Drawing.Font("Nirmala UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.searchButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.searchButton.Image = global::QobuzDownloaderX.Properties.Resources.search;
            this.searchButton.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.searchButton.Location = new System.Drawing.Point(0, 166);
            this.searchButton.Name = "searchButton";
            this.searchButton.Size = new System.Drawing.Size(180, 66);
            this.searchButton.TabIndex = 2;
            this.searchButton.Text = "SEARCH";
            this.searchButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.searchButton.UseVisualStyleBackColor = true;
            this.searchButton.Click += new System.EventHandler(this.searchButton_Click);
            // 
            // welcomeLabel
            // 
            this.welcomeLabel.BackColor = System.Drawing.Color.Transparent;
            this.welcomeLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.welcomeLabel.Location = new System.Drawing.Point(0, 403);
            this.welcomeLabel.Name = "welcomeLabel";
            this.welcomeLabel.Size = new System.Drawing.Size(180, 36);
            this.welcomeLabel.TabIndex = 4;
            this.welcomeLabel.Text = "Welcome\r\n{username}";
            this.welcomeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // settingsButton
            // 
            this.settingsButton.FlatAppearance.BorderSize = 0;
            this.settingsButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
            this.settingsButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(18)))), ((int)(((byte)(18)))));
            this.settingsButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.settingsButton.Font = new System.Drawing.Font("Nirmala UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.settingsButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.settingsButton.Image = global::QobuzDownloaderX.Properties.Resources.settings;
            this.settingsButton.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.settingsButton.Location = new System.Drawing.Point(0, 442);
            this.settingsButton.Name = "settingsButton";
            this.settingsButton.Size = new System.Drawing.Size(180, 66);
            this.settingsButton.TabIndex = 5;
            this.settingsButton.Text = "SETTINGS";
            this.settingsButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.settingsButton.UseVisualStyleBackColor = true;
            this.settingsButton.Click += new System.EventHandler(this.settingsButton_Click);
            // 
            // logoutButton
            // 
            this.logoutButton.FlatAppearance.BorderSize = 0;
            this.logoutButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
            this.logoutButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(18)))), ((int)(((byte)(18)))));
            this.logoutButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.logoutButton.Font = new System.Drawing.Font("Nirmala UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.logoutButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.logoutButton.Image = global::QobuzDownloaderX.Properties.Resources.logout;
            this.logoutButton.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.logoutButton.Location = new System.Drawing.Point(0, 508);
            this.logoutButton.Name = "logoutButton";
            this.logoutButton.Size = new System.Drawing.Size(180, 66);
            this.logoutButton.TabIndex = 6;
            this.logoutButton.Text = "LOGOUT";
            this.logoutButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.logoutButton.UseVisualStyleBackColor = true;
            this.logoutButton.Click += new System.EventHandler(this.logoutButton_Click);
            // 
            // aboutButton
            // 
            this.aboutButton.FlatAppearance.BorderSize = 0;
            this.aboutButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
            this.aboutButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(18)))), ((int)(((byte)(18)))));
            this.aboutButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.aboutButton.Font = new System.Drawing.Font("Nirmala UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.aboutButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.aboutButton.Image = global::QobuzDownloaderX.Properties.Resources.info;
            this.aboutButton.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.aboutButton.Location = new System.Drawing.Point(0, 232);
            this.aboutButton.Name = "aboutButton";
            this.aboutButton.Size = new System.Drawing.Size(180, 66);
            this.aboutButton.TabIndex = 3;
            this.aboutButton.Text = "ABOUT";
            this.aboutButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.aboutButton.UseVisualStyleBackColor = true;
            this.aboutButton.Click += new System.EventHandler(this.aboutButton_Click);
            // 
            // downloaderButton
            // 
            this.downloaderButton.FlatAppearance.BorderSize = 0;
            this.downloaderButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
            this.downloaderButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(18)))), ((int)(((byte)(18)))));
            this.downloaderButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.downloaderButton.Font = new System.Drawing.Font("Nirmala UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.downloaderButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.downloaderButton.Image = ((System.Drawing.Image)(resources.GetObject("downloaderButton.Image")));
            this.downloaderButton.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.downloaderButton.Location = new System.Drawing.Point(0, 100);
            this.downloaderButton.Name = "downloaderButton";
            this.downloaderButton.Size = new System.Drawing.Size(180, 66);
            this.downloaderButton.TabIndex = 1;
            this.downloaderButton.Text = "DOWNLOADER";
            this.downloaderButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.downloaderButton.UseVisualStyleBackColor = true;
            this.downloaderButton.Click += new System.EventHandler(this.downloaderButton_Click);
            // 
            // logoPanel
            // 
            this.logoPanel.Controls.Add(this.versionNumber);
            this.logoPanel.Controls.Add(this.logoPictureBox);
            this.logoPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.logoPanel.Location = new System.Drawing.Point(0, 0);
            this.logoPanel.Name = "logoPanel";
            this.logoPanel.Size = new System.Drawing.Size(180, 100);
            this.logoPanel.TabIndex = 0;
            this.logoPanel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.logoPanel_MouseMove);
            // 
            // versionNumber
            // 
            this.versionNumber.BackColor = System.Drawing.Color.Transparent;
            this.versionNumber.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.versionNumber.Location = new System.Drawing.Point(119, 79);
            this.versionNumber.Name = "versionNumber";
            this.versionNumber.Size = new System.Drawing.Size(58, 18);
            this.versionNumber.TabIndex = 0;
            this.versionNumber.Text = "#.#.#.#";
            this.versionNumber.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // logoPictureBox
            // 
            this.logoPictureBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(13)))), ((int)(((byte)(13)))), ((int)(((byte)(13)))));
            this.logoPictureBox.Image = global::QobuzDownloaderX.Properties.Resources.qbdlx_new;
            this.logoPictureBox.Location = new System.Drawing.Point(9, 3);
            this.logoPictureBox.Name = "logoPictureBox";
            this.logoPictureBox.Size = new System.Drawing.Size(161, 94);
            this.logoPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.logoPictureBox.TabIndex = 0;
            this.logoPictureBox.TabStop = false;
            this.logoPictureBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseMove);
            // 
            // downloaderPanel
            // 
            this.downloaderPanel.Controls.Add(this.batchDownloadProgressCountLabel);
            this.downloaderPanel.Controls.Add(this.progressItemsCountLabel);
            this.downloaderPanel.Controls.Add(this.batchDownloadPanel);
            this.downloaderPanel.Controls.Add(this.batchDownloadButton);
            this.downloaderPanel.Controls.Add(this.skipButton);
            this.downloaderPanel.Controls.Add(this.abortButton);
            this.downloaderPanel.Controls.Add(this.progressBarDownload);
            this.downloaderPanel.Controls.Add(this.progressLabel);
            this.downloaderPanel.Controls.Add(this.downloadButton);
            this.downloaderPanel.Controls.Add(this.infoLabel);
            this.downloaderPanel.Controls.Add(this.albumLabel);
            this.downloaderPanel.Controls.Add(this.artistLabel);
            this.downloaderPanel.Controls.Add(this.downloadOutput);
            this.downloaderPanel.Controls.Add(this.albumPictureBox);
            this.downloaderPanel.Controls.Add(this.inputTextbox);
            this.downloaderPanel.Controls.Add(this.downloadLabel);
            this.downloaderPanel.Location = new System.Drawing.Point(205, 83);
            this.downloaderPanel.Name = "downloaderPanel";
            this.downloaderPanel.Size = new System.Drawing.Size(771, 577);
            this.downloaderPanel.TabIndex = 5;
            // 
            // batchDownloadProgressCountLabel
            // 
            this.batchDownloadProgressCountLabel.AutoSize = true;
            this.batchDownloadProgressCountLabel.Font = new System.Drawing.Font("Nirmala UI Semilight", 12F);
            this.batchDownloadProgressCountLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.batchDownloadProgressCountLabel.Location = new System.Drawing.Point(185, 143);
            this.batchDownloadProgressCountLabel.Name = "batchDownloadProgressCountLabel";
            this.batchDownloadProgressCountLabel.Size = new System.Drawing.Size(21, 21);
            this.batchDownloadProgressCountLabel.TabIndex = 13;
            this.batchDownloadProgressCountLabel.Text = "…";
            this.batchDownloadProgressCountLabel.Visible = false;
            // 
            // progressItemsCountLabel
            // 
            this.progressItemsCountLabel.AutoSize = true;
            this.progressItemsCountLabel.Font = new System.Drawing.Font("Nirmala UI Semilight", 12F);
            this.progressItemsCountLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.progressItemsCountLabel.Location = new System.Drawing.Point(185, 118);
            this.progressItemsCountLabel.Name = "progressItemsCountLabel";
            this.progressItemsCountLabel.Size = new System.Drawing.Size(21, 21);
            this.progressItemsCountLabel.TabIndex = 12;
            this.progressItemsCountLabel.Text = "…";
            this.progressItemsCountLabel.Visible = false;
            // 
            // batchDownloadPanel
            // 
            this.batchDownloadPanel.BackColor = System.Drawing.Color.DarkRed;
            this.batchDownloadPanel.Controls.Add(this.batchDownloadTextBox);
            this.batchDownloadPanel.Controls.Add(this.batchDownloadLabel);
            this.batchDownloadPanel.Controls.Add(this.getAllBatchDownloadButton);
            this.batchDownloadPanel.Controls.Add(this.closeBatchDownloadbutton);
            this.batchDownloadPanel.Location = new System.Drawing.Point(213, 118);
            this.batchDownloadPanel.Name = "batchDownloadPanel";
            this.batchDownloadPanel.Size = new System.Drawing.Size(533, 23);
            this.batchDownloadPanel.TabIndex = 11;
            this.batchDownloadPanel.Visible = false;
            // 
            // batchDownloadTextBox
            // 
            this.batchDownloadTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.batchDownloadTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.batchDownloadTextBox.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.batchDownloadTextBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.batchDownloadTextBox.Font = new System.Drawing.Font("Nirmala UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.batchDownloadTextBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.batchDownloadTextBox.Location = new System.Drawing.Point(0, 25);
            this.batchDownloadTextBox.MaxLength = 0;
            this.batchDownloadTextBox.Multiline = true;
            this.batchDownloadTextBox.Name = "batchDownloadTextBox";
            this.batchDownloadTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.batchDownloadTextBox.Size = new System.Drawing.Size(533, 248);
            this.batchDownloadTextBox.TabIndex = 1;
            this.batchDownloadTextBox.WordWrap = false;
            this.batchDownloadTextBox.TextChanged += new System.EventHandler(this.batchDownloadTextBox_TextChanged);
            // 
            // batchDownloadLabel
            // 
            this.batchDownloadLabel.AutoEllipsis = true;
            this.batchDownloadLabel.AutoSize = true;
            this.batchDownloadLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.batchDownloadLabel.Font = new System.Drawing.Font("Nirmala UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.batchDownloadLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.batchDownloadLabel.Location = new System.Drawing.Point(0, 0);
            this.batchDownloadLabel.Name = "batchDownloadLabel";
            this.batchDownloadLabel.Size = new System.Drawing.Size(286, 25);
            this.batchDownloadLabel.TabIndex = 0;
            this.batchDownloadLabel.Text = "Paste one or more Qobuz URLs…";
            // 
            // getAllBatchDownloadButton
            // 
            this.getAllBatchDownloadButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.getAllBatchDownloadButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(13)))), ((int)(((byte)(13)))), ((int)(((byte)(13)))));
            this.getAllBatchDownloadButton.Enabled = false;
            this.getAllBatchDownloadButton.FlatAppearance.BorderSize = 0;
            this.getAllBatchDownloadButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
            this.getAllBatchDownloadButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(18)))), ((int)(((byte)(18)))));
            this.getAllBatchDownloadButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.getAllBatchDownloadButton.Font = new System.Drawing.Font("Nirmala UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.getAllBatchDownloadButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.getAllBatchDownloadButton.Location = new System.Drawing.Point(393, 282);
            this.getAllBatchDownloadButton.Name = "getAllBatchDownloadButton";
            this.getAllBatchDownloadButton.Size = new System.Drawing.Size(130, 31);
            this.getAllBatchDownloadButton.TabIndex = 3;
            this.getAllBatchDownloadButton.Text = "DESCARGAR TODO";
            this.getAllBatchDownloadButton.UseVisualStyleBackColor = false;
            this.getAllBatchDownloadButton.Click += new System.EventHandler(this.getAllBatchDownloadButton_Click);
            // 
            // closeBatchDownloadbutton
            // 
            this.closeBatchDownloadbutton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(13)))), ((int)(((byte)(13)))), ((int)(((byte)(13)))));
            this.closeBatchDownloadbutton.FlatAppearance.BorderSize = 0;
            this.closeBatchDownloadbutton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
            this.closeBatchDownloadbutton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(18)))), ((int)(((byte)(18)))));
            this.closeBatchDownloadbutton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.closeBatchDownloadbutton.Font = new System.Drawing.Font("Nirmala UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.closeBatchDownloadbutton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.closeBatchDownloadbutton.Location = new System.Drawing.Point(8, 282);
            this.closeBatchDownloadbutton.Name = "closeBatchDownloadbutton";
            this.closeBatchDownloadbutton.Size = new System.Drawing.Size(130, 31);
            this.closeBatchDownloadbutton.TabIndex = 2;
            this.closeBatchDownloadbutton.Text = "CLOSE / CANCEL";
            this.closeBatchDownloadbutton.UseVisualStyleBackColor = false;
            this.closeBatchDownloadbutton.Click += new System.EventHandler(this.closeBatchDownloadbutton_Click);
            // 
            // batchDownloadButton
            // 
            this.batchDownloadButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(13)))), ((int)(((byte)(13)))), ((int)(((byte)(13)))));
            this.batchDownloadButton.FlatAppearance.BorderSize = 0;
            this.batchDownloadButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
            this.batchDownloadButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(18)))), ((int)(((byte)(18)))));
            this.batchDownloadButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.batchDownloadButton.Font = new System.Drawing.Font("Nirmala UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.batchDownloadButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.batchDownloadButton.Location = new System.Drawing.Point(638, 48);
            this.batchDownloadButton.Name = "batchDownloadButton";
            this.batchDownloadButton.Size = new System.Drawing.Size(110, 31);
            this.batchDownloadButton.TabIndex = 3;
            this.batchDownloadButton.Text = "GET BATCH";
            this.batchDownloadButton.UseVisualStyleBackColor = false;
            this.batchDownloadButton.Click += new System.EventHandler(this.batchDownloadButton_Click);
            // 
            // skipButton
            // 
            this.skipButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(13)))), ((int)(((byte)(13)))), ((int)(((byte)(13)))));
            this.skipButton.Enabled = false;
            this.skipButton.FlatAppearance.BorderSize = 0;
            this.skipButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
            this.skipButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(18)))), ((int)(((byte)(18)))));
            this.skipButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.skipButton.Font = new System.Drawing.Font("Nirmala UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.skipButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.skipButton.Location = new System.Drawing.Point(638, 81);
            this.skipButton.Name = "skipButton";
            this.skipButton.Size = new System.Drawing.Size(110, 31);
            this.skipButton.TabIndex = 9;
            this.skipButton.Text = "SKIP";
            this.skipButton.UseVisualStyleBackColor = false;
            this.skipButton.Click += new System.EventHandler(this.skipButton_Click);
            // 
            // abortButton
            // 
            this.abortButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(13)))), ((int)(((byte)(13)))), ((int)(((byte)(13)))));
            this.abortButton.Enabled = false;
            this.abortButton.FlatAppearance.BorderSize = 0;
            this.abortButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
            this.abortButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(18)))), ((int)(((byte)(18)))));
            this.abortButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.abortButton.Font = new System.Drawing.Font("Nirmala UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.abortButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.abortButton.Location = new System.Drawing.Point(522, 81);
            this.abortButton.Name = "abortButton";
            this.abortButton.Size = new System.Drawing.Size(110, 31);
            this.abortButton.TabIndex = 8;
            this.abortButton.Text = "ABORT";
            this.abortButton.UseVisualStyleBackColor = false;
            this.abortButton.Click += new System.EventHandler(this.abortButton_Click);
            // 
            // progressLabel
            // 
            this.progressLabel.AutoSize = true;
            this.progressLabel.Font = new System.Drawing.Font("Nirmala UI", 10F);
            this.progressLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.progressLabel.Location = new System.Drawing.Point(14, 553);
            this.progressLabel.Name = "progressLabel";
            this.progressLabel.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.progressLabel.Size = new System.Drawing.Size(130, 19);
            this.progressLabel.TabIndex = 7;
            this.progressLabel.Text = "No download active";
            // 
            // downloadButton
            // 
            this.downloadButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(13)))), ((int)(((byte)(13)))), ((int)(((byte)(13)))));
            this.downloadButton.Enabled = false;
            this.downloadButton.FlatAppearance.BorderSize = 0;
            this.downloadButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
            this.downloadButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(18)))), ((int)(((byte)(18)))));
            this.downloadButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.downloadButton.Font = new System.Drawing.Font("Nirmala UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.downloadButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.downloadButton.Location = new System.Drawing.Point(522, 48);
            this.downloadButton.Name = "downloadButton";
            this.downloadButton.Size = new System.Drawing.Size(110, 31);
            this.downloadButton.TabIndex = 2;
            this.downloadButton.Text = "GET";
            this.downloadButton.UseVisualStyleBackColor = false;
            this.downloadButton.EnabledChanged += new System.EventHandler(this.downloadButton_EnabledChanged);
            this.downloadButton.Click += new System.EventHandler(this.downloadButton_Click);
            // 
            // infoLabel
            // 
            this.infoLabel.AutoEllipsis = true;
            this.infoLabel.Font = new System.Drawing.Font("Nirmala UI Semilight", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.infoLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.infoLabel.Location = new System.Drawing.Point(186, 222);
            this.infoLabel.Name = "infoLabel";
            this.infoLabel.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.infoLabel.Size = new System.Drawing.Size(560, 21);
            this.infoLabel.TabIndex = 6;
            this.infoLabel.Text = "Released xxxx-xx-xx • xx Tracks • UPC";
            // 
            // albumLabel
            // 
            this.albumLabel.AutoEllipsis = true;
            this.albumLabel.Font = new System.Drawing.Font("Nirmala UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.albumLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.albumLabel.Location = new System.Drawing.Point(184, 169);
            this.albumLabel.Name = "albumLabel";
            this.albumLabel.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.albumLabel.Size = new System.Drawing.Size(562, 32);
            this.albumLabel.TabIndex = 4;
            this.albumLabel.Text = "Placeholder Album Name";
            // 
            // artistLabel
            // 
            this.artistLabel.AutoEllipsis = true;
            this.artistLabel.Font = new System.Drawing.Font("Nirmala UI Semilight", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.artistLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.artistLabel.Location = new System.Drawing.Point(186, 201);
            this.artistLabel.Name = "artistLabel";
            this.artistLabel.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.artistLabel.Size = new System.Drawing.Size(560, 21);
            this.artistLabel.TabIndex = 5;
            this.artistLabel.Text = "Placeholder Artist Name";
            // 
            // downloadOutput
            // 
            this.downloadOutput.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.downloadOutput.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.downloadOutput.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.downloadOutput.Font = new System.Drawing.Font("Nirmala UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.downloadOutput.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.downloadOutput.Location = new System.Drawing.Point(18, 252);
            this.downloadOutput.MaxLength = 0;
            this.downloadOutput.Multiline = true;
            this.downloadOutput.Name = "downloadOutput";
            this.downloadOutput.ReadOnly = true;
            this.downloadOutput.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.downloadOutput.Size = new System.Drawing.Size(733, 300);
            this.downloadOutput.TabIndex = 7;
            this.downloadOutput.Text = "Test String";
            this.downloadOutput.TextChanged += new System.EventHandler(this.downloadOutput_TextChanged);
            // 
            // albumPictureBox
            // 
            this.albumPictureBox.Cursor = System.Windows.Forms.Cursors.Default;
            this.albumPictureBox.Image = ((System.Drawing.Image)(resources.GetObject("albumPictureBox.Image")));
            this.albumPictureBox.Location = new System.Drawing.Point(18, 83);
            this.albumPictureBox.Name = "albumPictureBox";
            this.albumPictureBox.Size = new System.Drawing.Size(160, 160);
            this.albumPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.albumPictureBox.TabIndex = 2;
            this.albumPictureBox.TabStop = false;
            this.albumPictureBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.albumPictureBox_MouseClick);
            // 
            // inputTextbox
            // 
            this.inputTextbox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.inputTextbox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.inputTextbox.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.inputTextbox.Font = new System.Drawing.Font("Nirmala UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.inputTextbox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.inputTextbox.Location = new System.Drawing.Point(18, 48);
            this.inputTextbox.MaxLength = 1000;
            this.inputTextbox.Name = "inputTextbox";
            this.inputTextbox.Size = new System.Drawing.Size(498, 26);
            this.inputTextbox.TabIndex = 1;
            this.inputTextbox.Text = "Paste a Qobuz URL…";
            this.inputTextbox.WordWrap = false;
            this.inputTextbox.Click += new System.EventHandler(this.inputTextbox_Click);
            this.inputTextbox.TextChanged += new System.EventHandler(this.inputTextbox_TextChanged);
            this.inputTextbox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.inputTextbox_KeyDown);
            this.inputTextbox.Leave += new System.EventHandler(this.inputTextbox_Leave);
            this.inputTextbox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.inputTextbox_MouseDown);
            // 
            // downloadLabel
            // 
            this.downloadLabel.AutoSize = true;
            this.downloadLabel.Font = new System.Drawing.Font("Nirmala UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.downloadLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.downloadLabel.Location = new System.Drawing.Point(13, 10);
            this.downloadLabel.Name = "downloadLabel";
            this.downloadLabel.Size = new System.Drawing.Size(139, 25);
            this.downloadLabel.TabIndex = 0;
            this.downloadLabel.Text = "DOWNLOADER";
            this.downloadLabel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.downloadLabel_MouseMove);
            // 
            // settingsPanel
            // 
            this.settingsPanel.Controls.Add(this.flowLayoutPanel1);
            this.settingsPanel.Controls.Add(this.folderButtonsPanel);
            this.settingsPanel.Controls.Add(this.templatesListLabel);
            this.settingsPanel.Controls.Add(this.templatesListTextbox);
            this.settingsPanel.Controls.Add(this.additionalSettingsButton);
            this.settingsPanel.Controls.Add(this.trackTemplateTextbox);
            this.settingsPanel.Controls.Add(this.trackTemplateLabel);
            this.settingsPanel.Controls.Add(this.playlistTemplateTextbox);
            this.settingsPanel.Controls.Add(this.playlistTemplateLabel);
            this.settingsPanel.Controls.Add(this.artistTemplateTextbox);
            this.settingsPanel.Controls.Add(this.favoritesTemplateTextbox);
            this.settingsPanel.Controls.Add(this.albumTemplateTextbox);
            this.settingsPanel.Controls.Add(this.downloadFolderTextbox);
            this.settingsPanel.Controls.Add(this.artistTemplateLabel);
            this.settingsPanel.Controls.Add(this.favoritesTemplateLabel);
            this.settingsPanel.Controls.Add(this.albumTemplateLabel);
            this.settingsPanel.Controls.Add(this.downloadFolderLabel);
            this.settingsPanel.Controls.Add(this.downloadOptionsLabel);
            this.settingsPanel.Controls.Add(this.templatesLabel);
            this.settingsPanel.Controls.Add(this.settingsLabel);
            this.settingsPanel.Location = new System.Drawing.Point(482, 26);
            this.settingsPanel.Name = "settingsPanel";
            this.settingsPanel.Size = new System.Drawing.Size(771, 577);
            this.settingsPanel.TabIndex = 1;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.resetTemplatesButton);
            this.flowLayoutPanel1.Controls.Add(this.saveTemplatesButton);
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(248, 345);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(443, 37);
            this.flowLayoutPanel1.TabIndex = 30;
            // 
            // resetTemplatesButton
            // 
            this.resetTemplatesButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.resetTemplatesButton.AutoSize = true;
            this.resetTemplatesButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(13)))), ((int)(((byte)(13)))), ((int)(((byte)(13)))));
            this.resetTemplatesButton.FlatAppearance.BorderSize = 0;
            this.resetTemplatesButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
            this.resetTemplatesButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(18)))), ((int)(((byte)(18)))));
            this.resetTemplatesButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.resetTemplatesButton.Font = new System.Drawing.Font("Nirmala UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.resetTemplatesButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.resetTemplatesButton.Location = new System.Drawing.Point(356, 3);
            this.resetTemplatesButton.Name = "resetTemplatesButton";
            this.resetTemplatesButton.Size = new System.Drawing.Size(84, 31);
            this.resetTemplatesButton.TabIndex = 1;
            this.resetTemplatesButton.Text = "Reset";
            this.resetTemplatesButton.UseVisualStyleBackColor = false;
            this.resetTemplatesButton.Click += new System.EventHandler(this.resetTemplatesButton_Click);
            // 
            // saveTemplatesButton
            // 
            this.saveTemplatesButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.saveTemplatesButton.AutoSize = true;
            this.saveTemplatesButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(13)))), ((int)(((byte)(13)))), ((int)(((byte)(13)))));
            this.saveTemplatesButton.FlatAppearance.BorderSize = 0;
            this.saveTemplatesButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
            this.saveTemplatesButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(18)))), ((int)(((byte)(18)))));
            this.saveTemplatesButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.saveTemplatesButton.Font = new System.Drawing.Font("Nirmala UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.saveTemplatesButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.saveTemplatesButton.Location = new System.Drawing.Point(266, 3);
            this.saveTemplatesButton.Name = "saveTemplatesButton";
            this.saveTemplatesButton.Size = new System.Drawing.Size(84, 31);
            this.saveTemplatesButton.TabIndex = 2;
            this.saveTemplatesButton.Text = "Save";
            this.saveTemplatesButton.UseVisualStyleBackColor = false;
            this.saveTemplatesButton.Click += new System.EventHandler(this.saveTemplatesButton_Click);
            // 
            // folderButtonsPanel
            // 
            this.folderButtonsPanel.Controls.Add(this.selectFolderButton);
            this.folderButtonsPanel.Controls.Add(this.openFolderButton);
            this.folderButtonsPanel.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.folderButtonsPanel.Location = new System.Drawing.Point(248, 101);
            this.folderButtonsPanel.Name = "folderButtonsPanel";
            this.folderButtonsPanel.Size = new System.Drawing.Size(443, 37);
            this.folderButtonsPanel.TabIndex = 29;
            // 
            // selectFolderButton
            // 
            this.selectFolderButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.selectFolderButton.AutoSize = true;
            this.selectFolderButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(13)))), ((int)(((byte)(13)))), ((int)(((byte)(13)))));
            this.selectFolderButton.FlatAppearance.BorderSize = 0;
            this.selectFolderButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
            this.selectFolderButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(18)))), ((int)(((byte)(18)))));
            this.selectFolderButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.selectFolderButton.Font = new System.Drawing.Font("Nirmala UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.selectFolderButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.selectFolderButton.Location = new System.Drawing.Point(356, 3);
            this.selectFolderButton.Name = "selectFolderButton";
            this.selectFolderButton.Size = new System.Drawing.Size(84, 31);
            this.selectFolderButton.TabIndex = 1;
            this.selectFolderButton.Text = "Select Folder";
            this.selectFolderButton.UseVisualStyleBackColor = false;
            this.selectFolderButton.Click += new System.EventHandler(this.selectFolderButton_Click);
            // 
            // openFolderButton
            // 
            this.openFolderButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.openFolderButton.AutoSize = true;
            this.openFolderButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(13)))), ((int)(((byte)(13)))), ((int)(((byte)(13)))));
            this.openFolderButton.FlatAppearance.BorderSize = 0;
            this.openFolderButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
            this.openFolderButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(18)))), ((int)(((byte)(18)))));
            this.openFolderButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.openFolderButton.Font = new System.Drawing.Font("Nirmala UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.openFolderButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.openFolderButton.Location = new System.Drawing.Point(268, 3);
            this.openFolderButton.Name = "openFolderButton";
            this.openFolderButton.Size = new System.Drawing.Size(82, 31);
            this.openFolderButton.TabIndex = 1;
            this.openFolderButton.Text = "Open Folder";
            this.openFolderButton.UseVisualStyleBackColor = false;
            this.openFolderButton.Click += new System.EventHandler(this.openFolderButton_Click);
            this.openFolderButton.KeyDown += new System.Windows.Forms.KeyEventHandler(this.downloadFolderTextbox_KeyDown);
            this.openFolderButton.KeyUp += new System.Windows.Forms.KeyEventHandler(this.downloadFolderTextbox_KeyUp);
            // 
            // templatesListLabel
            // 
            this.templatesListLabel.Font = new System.Drawing.Font("Nirmala UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.templatesListLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.templatesListLabel.Location = new System.Drawing.Point(0, 384);
            this.templatesListLabel.Name = "templatesListLabel";
            this.templatesListLabel.Size = new System.Drawing.Size(771, 25);
            this.templatesListLabel.TabIndex = 28;
            this.templatesListLabel.Text = "TEMPLATES LIST";
            this.templatesListLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // templatesListTextbox
            // 
            this.templatesListTextbox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.templatesListTextbox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.templatesListTextbox.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.templatesListTextbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.templatesListTextbox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.templatesListTextbox.Location = new System.Drawing.Point(96, 416);
            this.templatesListTextbox.Multiline = true;
            this.templatesListTextbox.Name = "templatesListTextbox";
            this.templatesListTextbox.ReadOnly = true;
            this.templatesListTextbox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.templatesListTextbox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.templatesListTextbox.Size = new System.Drawing.Size(595, 101);
            this.templatesListTextbox.TabIndex = 27;
            this.templatesListTextbox.Text = resources.GetString("templatesListTextbox.Text");
            this.templatesListTextbox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // additionalSettingsButton
            // 
            this.additionalSettingsButton.AutoSize = true;
            this.additionalSettingsButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(13)))), ((int)(((byte)(13)))), ((int)(((byte)(13)))));
            this.additionalSettingsButton.FlatAppearance.BorderSize = 0;
            this.additionalSettingsButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
            this.additionalSettingsButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(18)))), ((int)(((byte)(18)))));
            this.additionalSettingsButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.additionalSettingsButton.Font = new System.Drawing.Font("Nirmala UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.additionalSettingsButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.additionalSettingsButton.Location = new System.Drawing.Point(327, 523);
            this.additionalSettingsButton.Name = "additionalSettingsButton";
            this.additionalSettingsButton.Size = new System.Drawing.Size(117, 31);
            this.additionalSettingsButton.TabIndex = 26;
            this.additionalSettingsButton.Text = "Additional Settings";
            this.additionalSettingsButton.UseVisualStyleBackColor = false;
            this.additionalSettingsButton.Click += new System.EventHandler(this.additionalSettingsButton_Click);
            // 
            // trackTemplateTextbox
            // 
            this.trackTemplateTextbox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.trackTemplateTextbox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.trackTemplateTextbox.Font = new System.Drawing.Font("Nirmala UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.trackTemplateTextbox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.trackTemplateTextbox.Location = new System.Drawing.Point(248, 249);
            this.trackTemplateTextbox.Multiline = true;
            this.trackTemplateTextbox.Name = "trackTemplateTextbox";
            this.trackTemplateTextbox.Size = new System.Drawing.Size(443, 21);
            this.trackTemplateTextbox.TabIndex = 2;
            this.trackTemplateTextbox.Text = "%TrackNumber%. %ArtistName% - %TrackTitle%";
            this.trackTemplateTextbox.WordWrap = false;
            // 
            // trackTemplateLabel
            // 
            this.trackTemplateLabel.Font = new System.Drawing.Font("Nirmala UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.trackTemplateLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.trackTemplateLabel.Location = new System.Drawing.Point(0, 246);
            this.trackTemplateLabel.Name = "trackTemplateLabel";
            this.trackTemplateLabel.Size = new System.Drawing.Size(242, 25);
            this.trackTemplateLabel.TabIndex = 1;
            this.trackTemplateLabel.Text = "TRACK TEMPLATE";
            this.trackTemplateLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // playlistTemplateTextbox
            // 
            this.playlistTemplateTextbox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.playlistTemplateTextbox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.playlistTemplateTextbox.Font = new System.Drawing.Font("Nirmala UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.playlistTemplateTextbox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.playlistTemplateTextbox.Location = new System.Drawing.Point(248, 283);
            this.playlistTemplateTextbox.Multiline = true;
            this.playlistTemplateTextbox.Name = "playlistTemplateTextbox";
            this.playlistTemplateTextbox.Size = new System.Drawing.Size(443, 21);
            this.playlistTemplateTextbox.TabIndex = 2;
            this.playlistTemplateTextbox.Text = "%PlaylistTitle% [ID%PlaylistID%]\\%ArtistName%";
            this.playlistTemplateTextbox.WordWrap = false;
            // 
            // playlistTemplateLabel
            // 
            this.playlistTemplateLabel.Font = new System.Drawing.Font("Nirmala UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.playlistTemplateLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.playlistTemplateLabel.Location = new System.Drawing.Point(0, 280);
            this.playlistTemplateLabel.Name = "playlistTemplateLabel";
            this.playlistTemplateLabel.Size = new System.Drawing.Size(242, 25);
            this.playlistTemplateLabel.TabIndex = 1;
            this.playlistTemplateLabel.Text = "PLAYLIST TEMPLATE";
            this.playlistTemplateLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // artistTemplateTextbox
            // 
            this.artistTemplateTextbox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.artistTemplateTextbox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.artistTemplateTextbox.Font = new System.Drawing.Font("Nirmala UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.artistTemplateTextbox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.artistTemplateTextbox.Location = new System.Drawing.Point(248, 179);
            this.artistTemplateTextbox.Multiline = true;
            this.artistTemplateTextbox.Name = "artistTemplateTextbox";
            this.artistTemplateTextbox.Size = new System.Drawing.Size(443, 21);
            this.artistTemplateTextbox.TabIndex = 2;
            this.artistTemplateTextbox.Text = "%ArtistName%";
            this.artistTemplateTextbox.WordWrap = false;
            // 
            // favoritesTemplateTextbox
            // 
            this.favoritesTemplateTextbox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.favoritesTemplateTextbox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.favoritesTemplateTextbox.Font = new System.Drawing.Font("Nirmala UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.favoritesTemplateTextbox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.favoritesTemplateTextbox.Location = new System.Drawing.Point(248, 318);
            this.favoritesTemplateTextbox.Multiline = true;
            this.favoritesTemplateTextbox.Name = "favoritesTemplateTextbox";
            this.favoritesTemplateTextbox.Size = new System.Drawing.Size(443, 21);
            this.favoritesTemplateTextbox.TabIndex = 2;
            this.favoritesTemplateTextbox.Text = "- Favorites";
            this.favoritesTemplateTextbox.WordWrap = false;
            // 
            // albumTemplateTextbox
            // 
            this.albumTemplateTextbox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.albumTemplateTextbox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.albumTemplateTextbox.Font = new System.Drawing.Font("Nirmala UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.albumTemplateTextbox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.albumTemplateTextbox.Location = new System.Drawing.Point(248, 214);
            this.albumTemplateTextbox.Multiline = true;
            this.albumTemplateTextbox.Name = "albumTemplateTextbox";
            this.albumTemplateTextbox.Size = new System.Drawing.Size(443, 21);
            this.albumTemplateTextbox.TabIndex = 2;
            this.albumTemplateTextbox.Text = "%AlbumTitle% (%Year%) (%AlbumPA%) [UPC%UPC%]";
            this.albumTemplateTextbox.WordWrap = false;
            // 
            // downloadFolderTextbox
            // 
            this.downloadFolderTextbox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.downloadFolderTextbox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.downloadFolderTextbox.Font = new System.Drawing.Font("Nirmala UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.downloadFolderTextbox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.downloadFolderTextbox.Location = new System.Drawing.Point(248, 74);
            this.downloadFolderTextbox.Multiline = true;
            this.downloadFolderTextbox.Name = "downloadFolderTextbox";
            this.downloadFolderTextbox.Size = new System.Drawing.Size(443, 21);
            this.downloadFolderTextbox.TabIndex = 2;
            this.downloadFolderTextbox.Text = "no folder selected";
            this.downloadFolderTextbox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.downloadFolderTextbox_KeyDown);
            this.downloadFolderTextbox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.downloadFolderTextbox_KeyUp);
            // 
            // artistTemplateLabel
            // 
            this.artistTemplateLabel.Font = new System.Drawing.Font("Nirmala UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.artistTemplateLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.artistTemplateLabel.Location = new System.Drawing.Point(0, 176);
            this.artistTemplateLabel.Name = "artistTemplateLabel";
            this.artistTemplateLabel.Size = new System.Drawing.Size(242, 25);
            this.artistTemplateLabel.TabIndex = 1;
            this.artistTemplateLabel.Text = "ARTIST TEMPLATE";
            this.artistTemplateLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // favoritesTemplateLabel
            // 
            this.favoritesTemplateLabel.Font = new System.Drawing.Font("Nirmala UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.favoritesTemplateLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.favoritesTemplateLabel.Location = new System.Drawing.Point(0, 315);
            this.favoritesTemplateLabel.Name = "favoritesTemplateLabel";
            this.favoritesTemplateLabel.Size = new System.Drawing.Size(242, 25);
            this.favoritesTemplateLabel.TabIndex = 1;
            this.favoritesTemplateLabel.Text = "FAVORITES TEMPLATE";
            this.favoritesTemplateLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // albumTemplateLabel
            // 
            this.albumTemplateLabel.Font = new System.Drawing.Font("Nirmala UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.albumTemplateLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.albumTemplateLabel.Location = new System.Drawing.Point(0, 211);
            this.albumTemplateLabel.Name = "albumTemplateLabel";
            this.albumTemplateLabel.Size = new System.Drawing.Size(242, 25);
            this.albumTemplateLabel.TabIndex = 1;
            this.albumTemplateLabel.Text = "ALBUM TEMPLATE";
            this.albumTemplateLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // downloadFolderLabel
            // 
            this.downloadFolderLabel.Font = new System.Drawing.Font("Nirmala UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.downloadFolderLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.downloadFolderLabel.Location = new System.Drawing.Point(0, 71);
            this.downloadFolderLabel.Name = "downloadFolderLabel";
            this.downloadFolderLabel.Size = new System.Drawing.Size(242, 25);
            this.downloadFolderLabel.TabIndex = 1;
            this.downloadFolderLabel.Text = "DOWNLOAD FOLDER";
            this.downloadFolderLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // downloadOptionsLabel
            // 
            this.downloadOptionsLabel.Font = new System.Drawing.Font("Nirmala UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.downloadOptionsLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.downloadOptionsLabel.Location = new System.Drawing.Point(0, 35);
            this.downloadOptionsLabel.Name = "downloadOptionsLabel";
            this.downloadOptionsLabel.Size = new System.Drawing.Size(771, 25);
            this.downloadOptionsLabel.TabIndex = 1;
            this.downloadOptionsLabel.Text = "DOWNLOAD OPTIONS";
            this.downloadOptionsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // templatesLabel
            // 
            this.templatesLabel.Font = new System.Drawing.Font("Nirmala UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.templatesLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.templatesLabel.Location = new System.Drawing.Point(0, 141);
            this.templatesLabel.Name = "templatesLabel";
            this.templatesLabel.Size = new System.Drawing.Size(771, 25);
            this.templatesLabel.TabIndex = 1;
            this.templatesLabel.Text = "TEMPLATES";
            this.templatesLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // settingsLabel
            // 
            this.settingsLabel.AutoSize = true;
            this.settingsLabel.Font = new System.Drawing.Font("Nirmala UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.settingsLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.settingsLabel.Location = new System.Drawing.Point(13, 10);
            this.settingsLabel.Name = "settingsLabel";
            this.settingsLabel.Size = new System.Drawing.Size(554, 25);
            this.settingsLabel.TabIndex = 1;
            this.settingsLabel.Text = "SETTINGS                                                                         " +
    "                   ";
            this.settingsLabel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.settingsLabel_MouseMove);
            // 
            // userInfoTextbox
            // 
            this.userInfoTextbox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(18)))), ((int)(((byte)(18)))));
            this.userInfoTextbox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.userInfoTextbox.ContextMenuStrip = this.mainContextMenuStrip;
            this.userInfoTextbox.Cursor = System.Windows.Forms.Cursors.Help;
            this.userInfoTextbox.Font = new System.Drawing.Font("Nirmala UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.userInfoTextbox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.userInfoTextbox.Location = new System.Drawing.Point(285, 63);
            this.userInfoTextbox.Multiline = true;
            this.userInfoTextbox.Name = "userInfoTextbox";
            this.userInfoTextbox.ReadOnly = true;
            this.userInfoTextbox.Size = new System.Drawing.Size(400, 97);
            this.userInfoTextbox.TabIndex = 2;
            this.userInfoTextbox.Text = "User ID = {user_id}\r\nE-mail = {user_email}\r\nCountry = {user_country}\r\nSubscriptio" +
    "n = {user_subscription}\r\nExpires = {user_subscription_expiration}\r\n";
            this.userInfoTextbox.GotFocus += new System.EventHandler(this.userInfoTextbox_GotFocus);
            this.userInfoTextbox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.userInfoTextbox_MouseDown);
            this.userInfoTextbox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.userInfoTextbox_MouseUp);
            // 
            // mainContextMenuStrip
            // 
            this.mainContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyToClipboardToolStripMenuItem,
            this.copyThisRowToClipboardToolStripMenuItem,
            this.copySelectedRowsToClipboardToolStripMenuItem,
            this.copyAllRowsToClipboardToolStripMenuItem});
            this.mainContextMenuStrip.Name = "mainContextMenuStrip";
            this.mainContextMenuStrip.Size = new System.Drawing.Size(244, 92);
            this.mainContextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.mainContextMenuStrip_Opening);
            // 
            // copyToClipboardToolStripMenuItem
            // 
            this.copyToClipboardToolStripMenuItem.Image = global::QobuzDownloaderX.Properties.Resources.clipboard;
            this.copyToClipboardToolStripMenuItem.Name = "copyToClipboardToolStripMenuItem";
            this.copyToClipboardToolStripMenuItem.Size = new System.Drawing.Size(243, 22);
            this.copyToClipboardToolStripMenuItem.Text = "Copy to clipboard";
            this.copyToClipboardToolStripMenuItem.Click += new System.EventHandler(this.copyToclipboardToolStripMenuItem_Click);
            // 
            // copyThisRowToClipboardToolStripMenuItem
            // 
            this.copyThisRowToClipboardToolStripMenuItem.Image = global::QobuzDownloaderX.Properties.Resources.clipboard;
            this.copyThisRowToClipboardToolStripMenuItem.Name = "copyThisRowToClipboardToolStripMenuItem";
            this.copyThisRowToClipboardToolStripMenuItem.Size = new System.Drawing.Size(243, 22);
            this.copyThisRowToClipboardToolStripMenuItem.Text = "Copy this row to clipboard";
            this.copyThisRowToClipboardToolStripMenuItem.Click += new System.EventHandler(this.copyThisRowToClipboardToolStripMenuItem_Click);
            // 
            // copySelectedRowsToClipboardToolStripMenuItem
            // 
            this.copySelectedRowsToClipboardToolStripMenuItem.Image = global::QobuzDownloaderX.Properties.Resources.clipboard;
            this.copySelectedRowsToClipboardToolStripMenuItem.Name = "copySelectedRowsToClipboardToolStripMenuItem";
            this.copySelectedRowsToClipboardToolStripMenuItem.Size = new System.Drawing.Size(243, 22);
            this.copySelectedRowsToClipboardToolStripMenuItem.Text = "Copy selected rows to clipboard";
            this.copySelectedRowsToClipboardToolStripMenuItem.Click += new System.EventHandler(this.copySelectedRowsToClipboardToolStripMenuItem_Click);
            // 
            // copyAllRowsToClipboardToolStripMenuItem
            // 
            this.copyAllRowsToClipboardToolStripMenuItem.Image = global::QobuzDownloaderX.Properties.Resources.clipboard;
            this.copyAllRowsToClipboardToolStripMenuItem.Name = "copyAllRowsToClipboardToolStripMenuItem";
            this.copyAllRowsToClipboardToolStripMenuItem.Size = new System.Drawing.Size(243, 22);
            this.copyAllRowsToClipboardToolStripMenuItem.Text = "Copy all rows to clipboard";
            this.copyAllRowsToClipboardToolStripMenuItem.Click += new System.EventHandler(this.copyAllRowsToClipboardToolStripMenuItem_Click);
            // 
            // userInfoLabel
            // 
            this.userInfoLabel.Font = new System.Drawing.Font("Nirmala UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.userInfoLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.userInfoLabel.Location = new System.Drawing.Point(0, 35);
            this.userInfoLabel.Name = "userInfoLabel";
            this.userInfoLabel.Size = new System.Drawing.Size(771, 25);
            this.userInfoLabel.TabIndex = 1;
            this.userInfoLabel.Text = "USER INFO";
            this.userInfoLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // disclaimerLabel
            // 
            this.disclaimerLabel.Font = new System.Drawing.Font("Nirmala UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.disclaimerLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.disclaimerLabel.Location = new System.Drawing.Point(18, 150);
            this.disclaimerLabel.Name = "disclaimerLabel";
            this.disclaimerLabel.Size = new System.Drawing.Size(733, 450);
            this.disclaimerLabel.TabIndex = 2;
            this.disclaimerLabel.Text = "DISCLAIMER / LEGAL ADVICE";
            this.disclaimerLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // exitButton
            // 
            this.exitButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(13)))), ((int)(((byte)(13)))), ((int)(((byte)(13)))));
            this.exitButton.FlatAppearance.BorderSize = 0;
            this.exitButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
            this.exitButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(18)))), ((int)(((byte)(18)))));
            this.exitButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.exitButton.Font = new System.Drawing.Font("Nirmala UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.exitButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.exitButton.Location = new System.Drawing.Point(914, 0);
            this.exitButton.Name = "exitButton";
            this.exitButton.Size = new System.Drawing.Size(37, 30);
            this.exitButton.TabIndex = 3;
            this.exitButton.TabStop = false;
            this.exitButton.Text = "X";
            this.exitButton.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.exitButton.UseVisualStyleBackColor = false;
            this.exitButton.Click += new System.EventHandler(this.exitButton_Click);
            // 
            // minimizeButton
            // 
            this.minimizeButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(13)))), ((int)(((byte)(13)))), ((int)(((byte)(13)))));
            this.minimizeButton.FlatAppearance.BorderSize = 0;
            this.minimizeButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
            this.minimizeButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(18)))), ((int)(((byte)(18)))));
            this.minimizeButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.minimizeButton.Font = new System.Drawing.Font("Nirmala UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.minimizeButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.minimizeButton.Location = new System.Drawing.Point(880, 0);
            this.minimizeButton.Name = "minimizeButton";
            this.minimizeButton.Size = new System.Drawing.Size(37, 30);
            this.minimizeButton.TabIndex = 2;
            this.minimizeButton.TabStop = false;
            this.minimizeButton.Text = "_";
            this.minimizeButton.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.minimizeButton.UseVisualStyleBackColor = false;
            this.minimizeButton.Click += new System.EventHandler(this.minimizeButton_Click);
            // 
            // aboutPanel
            // 
            this.aboutPanel.Controls.Add(this.userInfoTextbox);
            this.aboutPanel.Controls.Add(this.aboutLabel);
            this.aboutPanel.Controls.Add(this.userInfoLabel);
            this.aboutPanel.Controls.Add(this.disclaimerLabel);
            this.aboutPanel.Location = new System.Drawing.Point(787, 352);
            this.aboutPanel.Name = "aboutPanel";
            this.aboutPanel.Size = new System.Drawing.Size(771, 577);
            this.aboutPanel.TabIndex = 1;
            // 
            // aboutLabel
            // 
            this.aboutLabel.AutoSize = true;
            this.aboutLabel.Font = new System.Drawing.Font("Nirmala UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.aboutLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.aboutLabel.Location = new System.Drawing.Point(13, 10);
            this.aboutLabel.Name = "aboutLabel";
            this.aboutLabel.Size = new System.Drawing.Size(552, 25);
            this.aboutLabel.TabIndex = 1;
            this.aboutLabel.Text = "ABOUT                                                                            " +
    "                    ";
            this.aboutLabel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.aboutLabel_MouseMove);
            // 
            // extraSettingsPanel
            // 
            this.extraSettingsPanel.Controls.Add(this.advancedOptionsPanelRight);
            this.extraSettingsPanel.Controls.Add(this.advancedOptionsPanelLeft);
            this.extraSettingsPanel.Controls.Add(this.commentLabel);
            this.extraSettingsPanel.Controls.Add(this.dontSaveArtworkToDiskCheckBox);
            this.extraSettingsPanel.Controls.Add(this.taggingOptionsPanel);
            this.extraSettingsPanel.Controls.Add(this.languageLabel);
            this.extraSettingsPanel.Controls.Add(this.languageComboBox);
            this.extraSettingsPanel.Controls.Add(this.themeLabel);
            this.extraSettingsPanel.Controls.Add(this.themeComboBox);
            this.extraSettingsPanel.Controls.Add(this.themeSectionLabel);
            this.extraSettingsPanel.Controls.Add(this.commentCheckbox);
            this.extraSettingsPanel.Controls.Add(this.commentTextbox);
            this.extraSettingsPanel.Controls.Add(this.advancedOptionsLabel);
            this.extraSettingsPanel.Controls.Add(this.closeAdditionalButton);
            this.extraSettingsPanel.Controls.Add(this.savedArtLabel);
            this.extraSettingsPanel.Controls.Add(this.savedArtSizeSelect);
            this.extraSettingsPanel.Controls.Add(this.taggingOptionsLabel);
            this.extraSettingsPanel.Controls.Add(this.embeddedArtLabel);
            this.extraSettingsPanel.Controls.Add(this.embeddedArtSizeSelect);
            this.extraSettingsPanel.Controls.Add(this.extraSettingsLabel);
            this.extraSettingsPanel.Font = new System.Drawing.Font("Nirmala UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.extraSettingsPanel.Location = new System.Drawing.Point(267, 3);
            this.extraSettingsPanel.Name = "extraSettingsPanel";
            this.extraSettingsPanel.Size = new System.Drawing.Size(771, 577);
            this.extraSettingsPanel.TabIndex = 3;
            this.extraSettingsPanel.Visible = false;
            // 
            // advancedOptionsPanelRight
            // 
            this.advancedOptionsPanelRight.Controls.Add(this.downloadGoodiesCheckbox);
            this.advancedOptionsPanelRight.Controls.Add(this.downloadArtistOtherCheckbox);
            this.advancedOptionsPanelRight.Controls.Add(this.fixMD5sCheckbox);
            this.advancedOptionsPanelRight.Controls.Add(this.clearOldLogsCheckBox);
            this.advancedOptionsPanelRight.Location = new System.Drawing.Point(386, 289);
            this.advancedOptionsPanelRight.Name = "advancedOptionsPanelRight";
            this.advancedOptionsPanelRight.Size = new System.Drawing.Size(365, 110);
            this.advancedOptionsPanelRight.TabIndex = 41;
            // 
            // downloadGoodiesCheckbox
            // 
            this.downloadGoodiesCheckbox.AutoSize = true;
            this.downloadGoodiesCheckbox.Checked = true;
            this.downloadGoodiesCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.downloadGoodiesCheckbox.Font = new System.Drawing.Font("Nirmala UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.downloadGoodiesCheckbox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.downloadGoodiesCheckbox.Location = new System.Drawing.Point(3, 3);
            this.downloadGoodiesCheckbox.Name = "downloadGoodiesCheckbox";
            this.downloadGoodiesCheckbox.Size = new System.Drawing.Size(126, 17);
            this.downloadGoodiesCheckbox.TabIndex = 28;
            this.downloadGoodiesCheckbox.Text = "Download Goodies";
            this.downloadGoodiesCheckbox.UseVisualStyleBackColor = true;
            this.downloadGoodiesCheckbox.CheckedChanged += new System.EventHandler(this.downloadGoodiesCheckbox_CheckedChanged);
            // 
            // downloadArtistOtherCheckbox
            // 
            this.downloadArtistOtherCheckbox.AutoSize = true;
            this.downloadArtistOtherCheckbox.Checked = true;
            this.downloadArtistOtherCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.downloadArtistOtherCheckbox.Font = new System.Drawing.Font("Nirmala UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.downloadArtistOtherCheckbox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.downloadArtistOtherCheckbox.Location = new System.Drawing.Point(135, 3);
            this.downloadArtistOtherCheckbox.Name = "downloadArtistOtherCheckbox";
            this.downloadArtistOtherCheckbox.Size = new System.Drawing.Size(191, 17);
            this.downloadArtistOtherCheckbox.TabIndex = 29;
            this.downloadArtistOtherCheckbox.Text = "Download artist - Other / covers";
            this.downloadArtistOtherCheckbox.UseVisualStyleBackColor = true;
            this.downloadArtistOtherCheckbox.CheckedChanged += new System.EventHandler(this.downloadArtistOtherCheckbox_CheckedChanged);
            // 
            // fixMD5sCheckbox
            // 
            this.fixMD5sCheckbox.AutoSize = true;
            this.fixMD5sCheckbox.Checked = true;
            this.fixMD5sCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.fixMD5sCheckbox.Font = new System.Drawing.Font("Nirmala UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fixMD5sCheckbox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.fixMD5sCheckbox.Location = new System.Drawing.Point(3, 26);
            this.fixMD5sCheckbox.Name = "fixMD5sCheckbox";
            this.fixMD5sCheckbox.Size = new System.Drawing.Size(311, 17);
            this.fixMD5sCheckbox.TabIndex = 28;
            this.fixMD5sCheckbox.Text = "Auto-Fix Unset MD5s (must have FLAC in PATH variables)";
            this.fixMD5sCheckbox.UseVisualStyleBackColor = true;
            this.fixMD5sCheckbox.CheckedChanged += new System.EventHandler(this.fixMD5sCheckbox_CheckedChanged);
            // 
            // clearOldLogsCheckBox
            // 
            this.clearOldLogsCheckBox.AutoSize = true;
            this.clearOldLogsCheckBox.Font = new System.Drawing.Font("Nirmala UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.clearOldLogsCheckBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.clearOldLogsCheckBox.Location = new System.Drawing.Point(3, 49);
            this.clearOldLogsCheckBox.Name = "clearOldLogsCheckBox";
            this.clearOldLogsCheckBox.Size = new System.Drawing.Size(154, 17);
            this.clearOldLogsCheckBox.TabIndex = 30;
            this.clearOldLogsCheckBox.Text = "Clear old logs on startup";
            this.clearOldLogsCheckBox.UseVisualStyleBackColor = true;
            this.clearOldLogsCheckBox.CheckedChanged += new System.EventHandler(this.clearOldLogsCheckBox_CheckedChanged);
            // 
            // advancedOptionsPanelLeft
            // 
            this.advancedOptionsPanelLeft.Controls.Add(this.useTLS13Checkbox);
            this.advancedOptionsPanelLeft.Controls.Add(this.streamableCheckbox);
            this.advancedOptionsPanelLeft.Controls.Add(this.downloadSpeedCheckbox);
            this.advancedOptionsPanelLeft.Controls.Add(this.mergeArtistNamesCheckbox);
            this.advancedOptionsPanelLeft.Location = new System.Drawing.Point(18, 289);
            this.advancedOptionsPanelLeft.Name = "advancedOptionsPanelLeft";
            this.advancedOptionsPanelLeft.Size = new System.Drawing.Size(365, 110);
            this.advancedOptionsPanelLeft.TabIndex = 40;
            // 
            // useTLS13Checkbox
            // 
            this.useTLS13Checkbox.AutoSize = true;
            this.useTLS13Checkbox.Checked = true;
            this.useTLS13Checkbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.useTLS13Checkbox.Font = new System.Drawing.Font("Nirmala UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.useTLS13Checkbox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.useTLS13Checkbox.Location = new System.Drawing.Point(3, 3);
            this.useTLS13Checkbox.Name = "useTLS13Checkbox";
            this.useTLS13Checkbox.Size = new System.Drawing.Size(98, 17);
            this.useTLS13Checkbox.TabIndex = 26;
            this.useTLS13Checkbox.Text = "Enable TLS 1.3";
            this.useTLS13Checkbox.UseVisualStyleBackColor = true;
            this.useTLS13Checkbox.CheckedChanged += new System.EventHandler(this.useTLS13Checkbox_CheckedChanged);
            // 
            // streamableCheckbox
            // 
            this.streamableCheckbox.AutoSize = true;
            this.streamableCheckbox.Checked = true;
            this.streamableCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.streamableCheckbox.Font = new System.Drawing.Font("Nirmala UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.streamableCheckbox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.streamableCheckbox.Location = new System.Drawing.Point(107, 3);
            this.streamableCheckbox.Name = "streamableCheckbox";
            this.streamableCheckbox.Size = new System.Drawing.Size(117, 17);
            this.streamableCheckbox.TabIndex = 27;
            this.streamableCheckbox.Text = "Streamable Check";
            this.streamableCheckbox.UseVisualStyleBackColor = true;
            this.streamableCheckbox.CheckedChanged += new System.EventHandler(this.streamableCheckbox_CheckedChanged);
            // 
            // downloadSpeedCheckbox
            // 
            this.downloadSpeedCheckbox.AutoSize = true;
            this.downloadSpeedCheckbox.Font = new System.Drawing.Font("Nirmala UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.downloadSpeedCheckbox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.downloadSpeedCheckbox.Location = new System.Drawing.Point(3, 26);
            this.downloadSpeedCheckbox.Name = "downloadSpeedCheckbox";
            this.downloadSpeedCheckbox.Size = new System.Drawing.Size(142, 17);
            this.downloadSpeedCheckbox.TabIndex = 29;
            this.downloadSpeedCheckbox.Text = "Print Download Speed";
            this.downloadSpeedCheckbox.UseVisualStyleBackColor = true;
            this.downloadSpeedCheckbox.CheckedChanged += new System.EventHandler(this.downloadSpeedCheckbox_CheckedChanged);
            // 
            // mergeArtistNamesCheckbox
            // 
            this.mergeArtistNamesCheckbox.AutoSize = true;
            this.mergeArtistNamesCheckbox.Checked = true;
            this.mergeArtistNamesCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.mergeArtistNamesCheckbox.Font = new System.Drawing.Font("Nirmala UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mergeArtistNamesCheckbox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.mergeArtistNamesCheckbox.Location = new System.Drawing.Point(151, 26);
            this.mergeArtistNamesCheckbox.Name = "mergeArtistNamesCheckbox";
            this.mergeArtistNamesCheckbox.Size = new System.Drawing.Size(131, 17);
            this.mergeArtistNamesCheckbox.TabIndex = 15;
            this.mergeArtistNamesCheckbox.Text = "Merge Artists Names";
            this.mergeArtistNamesCheckbox.UseVisualStyleBackColor = true;
            this.mergeArtistNamesCheckbox.CheckedChanged += new System.EventHandler(this.mergeArtistNamesCheckbox_CheckedChanged);
            // 
            // commentLabel
            // 
            this.commentLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.commentLabel.Location = new System.Drawing.Point(41, 155);
            this.commentLabel.Name = "commentLabel";
            this.commentLabel.Size = new System.Drawing.Size(201, 16);
            this.commentLabel.TabIndex = 39;
            this.commentLabel.Text = "Custom Comment";
            this.commentLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dontSaveArtworkToDiskCheckBox
            // 
            this.dontSaveArtworkToDiskCheckBox.AutoSize = true;
            this.dontSaveArtworkToDiskCheckBox.Font = new System.Drawing.Font("Nirmala UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dontSaveArtworkToDiskCheckBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.dontSaveArtworkToDiskCheckBox.Location = new System.Drawing.Point(389, 219);
            this.dontSaveArtworkToDiskCheckBox.Name = "dontSaveArtworkToDiskCheckBox";
            this.dontSaveArtworkToDiskCheckBox.Size = new System.Drawing.Size(160, 17);
            this.dontSaveArtworkToDiskCheckBox.TabIndex = 31;
            this.dontSaveArtworkToDiskCheckBox.Text = "Don\'t save artwork to disk";
            this.dontSaveArtworkToDiskCheckBox.UseVisualStyleBackColor = true;
            this.dontSaveArtworkToDiskCheckBox.CheckedChanged += new System.EventHandler(this.dontSaveArtworkToDiskCheckBox_CheckedChanged);
            // 
            // taggingOptionsPanel
            // 
            this.taggingOptionsPanel.Controls.Add(this.albumArtistCheckbox);
            this.taggingOptionsPanel.Controls.Add(this.albumTitleCheckbox);
            this.taggingOptionsPanel.Controls.Add(this.trackArtistCheckbox);
            this.taggingOptionsPanel.Controls.Add(this.trackTitleCheckbox);
            this.taggingOptionsPanel.Controls.Add(this.releaseDateCheckbox);
            this.taggingOptionsPanel.Controls.Add(this.releaseTypeCheckbox);
            this.taggingOptionsPanel.Controls.Add(this.genreCheckbox);
            this.taggingOptionsPanel.Controls.Add(this.trackNumberCheckbox);
            this.taggingOptionsPanel.Controls.Add(this.trackTotalCheckbox);
            this.taggingOptionsPanel.Controls.Add(this.discNumberCheckbox);
            this.taggingOptionsPanel.Controls.Add(this.discTotalCheckbox);
            this.taggingOptionsPanel.Controls.Add(this.composerCheckbox);
            this.taggingOptionsPanel.Controls.Add(this.explicitCheckbox);
            this.taggingOptionsPanel.Controls.Add(this.coverArtCheckbox);
            this.taggingOptionsPanel.Controls.Add(this.copyrightCheckbox);
            this.taggingOptionsPanel.Controls.Add(this.labelCheckbox);
            this.taggingOptionsPanel.Controls.Add(this.upcCheckbox);
            this.taggingOptionsPanel.Controls.Add(this.isrcCheckbox);
            this.taggingOptionsPanel.Controls.Add(this.urlCheckbox);
            this.taggingOptionsPanel.Location = new System.Drawing.Point(18, 63);
            this.taggingOptionsPanel.Name = "taggingOptionsPanel";
            this.taggingOptionsPanel.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.taggingOptionsPanel.Size = new System.Drawing.Size(733, 91);
            this.taggingOptionsPanel.TabIndex = 38;
            // 
            // albumArtistCheckbox
            // 
            this.albumArtistCheckbox.AutoSize = true;
            this.albumArtistCheckbox.Checked = true;
            this.albumArtistCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.albumArtistCheckbox.Font = new System.Drawing.Font("Nirmala UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.albumArtistCheckbox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.albumArtistCheckbox.Location = new System.Drawing.Point(3, 3);
            this.albumArtistCheckbox.Name = "albumArtistCheckbox";
            this.albumArtistCheckbox.Size = new System.Drawing.Size(89, 17);
            this.albumArtistCheckbox.TabIndex = 2;
            this.albumArtistCheckbox.Text = "Album Artist";
            this.albumArtistCheckbox.UseVisualStyleBackColor = true;
            this.albumArtistCheckbox.CheckedChanged += new System.EventHandler(this.albumArtistCheckbox_CheckedChanged);
            // 
            // albumTitleCheckbox
            // 
            this.albumTitleCheckbox.AutoSize = true;
            this.albumTitleCheckbox.Checked = true;
            this.albumTitleCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.albumTitleCheckbox.Font = new System.Drawing.Font("Nirmala UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.albumTitleCheckbox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.albumTitleCheckbox.Location = new System.Drawing.Point(98, 3);
            this.albumTitleCheckbox.Name = "albumTitleCheckbox";
            this.albumTitleCheckbox.Size = new System.Drawing.Size(83, 17);
            this.albumTitleCheckbox.TabIndex = 3;
            this.albumTitleCheckbox.Text = "Album Title";
            this.albumTitleCheckbox.UseVisualStyleBackColor = true;
            this.albumTitleCheckbox.CheckedChanged += new System.EventHandler(this.albumTitleCheckbox_CheckedChanged);
            // 
            // trackArtistCheckbox
            // 
            this.trackArtistCheckbox.AutoSize = true;
            this.trackArtistCheckbox.Checked = true;
            this.trackArtistCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.trackArtistCheckbox.Font = new System.Drawing.Font("Nirmala UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.trackArtistCheckbox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.trackArtistCheckbox.Location = new System.Drawing.Point(187, 3);
            this.trackArtistCheckbox.Name = "trackArtistCheckbox";
            this.trackArtistCheckbox.Size = new System.Drawing.Size(81, 17);
            this.trackArtistCheckbox.TabIndex = 5;
            this.trackArtistCheckbox.Text = "Track Artist";
            this.trackArtistCheckbox.UseVisualStyleBackColor = true;
            this.trackArtistCheckbox.CheckedChanged += new System.EventHandler(this.trackArtistCheckbox_CheckedChanged);
            // 
            // trackTitleCheckbox
            // 
            this.trackTitleCheckbox.AutoSize = true;
            this.trackTitleCheckbox.Checked = true;
            this.trackTitleCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.trackTitleCheckbox.Font = new System.Drawing.Font("Nirmala UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.trackTitleCheckbox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.trackTitleCheckbox.Location = new System.Drawing.Point(274, 3);
            this.trackTitleCheckbox.Name = "trackTitleCheckbox";
            this.trackTitleCheckbox.Size = new System.Drawing.Size(75, 17);
            this.trackTitleCheckbox.TabIndex = 4;
            this.trackTitleCheckbox.Text = "Track Title";
            this.trackTitleCheckbox.UseVisualStyleBackColor = true;
            this.trackTitleCheckbox.CheckedChanged += new System.EventHandler(this.trackTitleCheckbox_CheckedChanged);
            // 
            // releaseDateCheckbox
            // 
            this.releaseDateCheckbox.AutoSize = true;
            this.releaseDateCheckbox.Checked = true;
            this.releaseDateCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.releaseDateCheckbox.Font = new System.Drawing.Font("Nirmala UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.releaseDateCheckbox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.releaseDateCheckbox.Location = new System.Drawing.Point(355, 3);
            this.releaseDateCheckbox.Name = "releaseDateCheckbox";
            this.releaseDateCheckbox.Size = new System.Drawing.Size(92, 17);
            this.releaseDateCheckbox.TabIndex = 7;
            this.releaseDateCheckbox.Text = "Release Date";
            this.releaseDateCheckbox.UseVisualStyleBackColor = true;
            this.releaseDateCheckbox.CheckedChanged += new System.EventHandler(this.releaseDateCheckbox_CheckedChanged);
            // 
            // releaseTypeCheckbox
            // 
            this.releaseTypeCheckbox.AutoSize = true;
            this.releaseTypeCheckbox.Checked = true;
            this.releaseTypeCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.releaseTypeCheckbox.Font = new System.Drawing.Font("Nirmala UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.releaseTypeCheckbox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.releaseTypeCheckbox.Location = new System.Drawing.Point(453, 3);
            this.releaseTypeCheckbox.Name = "releaseTypeCheckbox";
            this.releaseTypeCheckbox.Size = new System.Drawing.Size(90, 17);
            this.releaseTypeCheckbox.TabIndex = 6;
            this.releaseTypeCheckbox.Text = "Release Type";
            this.releaseTypeCheckbox.UseVisualStyleBackColor = true;
            this.releaseTypeCheckbox.CheckedChanged += new System.EventHandler(this.releaseTypeCheckbox_CheckedChanged);
            // 
            // genreCheckbox
            // 
            this.genreCheckbox.AutoSize = true;
            this.genreCheckbox.Checked = true;
            this.genreCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.genreCheckbox.Font = new System.Drawing.Font("Nirmala UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.genreCheckbox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.genreCheckbox.Location = new System.Drawing.Point(549, 3);
            this.genreCheckbox.Name = "genreCheckbox";
            this.genreCheckbox.Size = new System.Drawing.Size(57, 17);
            this.genreCheckbox.TabIndex = 8;
            this.genreCheckbox.Text = "Genre";
            this.genreCheckbox.UseVisualStyleBackColor = true;
            this.genreCheckbox.CheckedChanged += new System.EventHandler(this.genreCheckbox_CheckedChanged);
            // 
            // trackNumberCheckbox
            // 
            this.trackNumberCheckbox.AutoSize = true;
            this.trackNumberCheckbox.Checked = true;
            this.trackNumberCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.trackNumberCheckbox.Font = new System.Drawing.Font("Nirmala UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.trackNumberCheckbox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.trackNumberCheckbox.Location = new System.Drawing.Point(612, 3);
            this.trackNumberCheckbox.Name = "trackNumberCheckbox";
            this.trackNumberCheckbox.Size = new System.Drawing.Size(95, 17);
            this.trackNumberCheckbox.TabIndex = 15;
            this.trackNumberCheckbox.Text = "Track Number";
            this.trackNumberCheckbox.UseVisualStyleBackColor = true;
            this.trackNumberCheckbox.CheckedChanged += new System.EventHandler(this.trackNumberCheckbox_CheckedChanged);
            // 
            // trackTotalCheckbox
            // 
            this.trackTotalCheckbox.AutoSize = true;
            this.trackTotalCheckbox.Checked = true;
            this.trackTotalCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.trackTotalCheckbox.Font = new System.Drawing.Font("Nirmala UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.trackTotalCheckbox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.trackTotalCheckbox.Location = new System.Drawing.Point(3, 26);
            this.trackTotalCheckbox.Name = "trackTotalCheckbox";
            this.trackTotalCheckbox.Size = new System.Drawing.Size(83, 17);
            this.trackTotalCheckbox.TabIndex = 14;
            this.trackTotalCheckbox.Text = "Total Tracks";
            this.trackTotalCheckbox.UseVisualStyleBackColor = true;
            this.trackTotalCheckbox.CheckedChanged += new System.EventHandler(this.trackTotalCheckbox_CheckedChanged);
            // 
            // discNumberCheckbox
            // 
            this.discNumberCheckbox.AutoSize = true;
            this.discNumberCheckbox.Checked = true;
            this.discNumberCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.discNumberCheckbox.Font = new System.Drawing.Font("Nirmala UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.discNumberCheckbox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.discNumberCheckbox.Location = new System.Drawing.Point(92, 26);
            this.discNumberCheckbox.Name = "discNumberCheckbox";
            this.discNumberCheckbox.Size = new System.Drawing.Size(91, 17);
            this.discNumberCheckbox.TabIndex = 16;
            this.discNumberCheckbox.Text = "Disc Number";
            this.discNumberCheckbox.UseVisualStyleBackColor = true;
            this.discNumberCheckbox.CheckedChanged += new System.EventHandler(this.discNumberCheckbox_CheckedChanged);
            // 
            // discTotalCheckbox
            // 
            this.discTotalCheckbox.AutoSize = true;
            this.discTotalCheckbox.Checked = true;
            this.discTotalCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.discTotalCheckbox.Font = new System.Drawing.Font("Nirmala UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.discTotalCheckbox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.discTotalCheckbox.Location = new System.Drawing.Point(189, 26);
            this.discTotalCheckbox.Name = "discTotalCheckbox";
            this.discTotalCheckbox.Size = new System.Drawing.Size(79, 17);
            this.discTotalCheckbox.TabIndex = 17;
            this.discTotalCheckbox.Text = "Total Discs";
            this.discTotalCheckbox.UseVisualStyleBackColor = true;
            this.discTotalCheckbox.CheckedChanged += new System.EventHandler(this.discTotalCheckbox_CheckedChanged);
            // 
            // composerCheckbox
            // 
            this.composerCheckbox.AutoSize = true;
            this.composerCheckbox.Checked = true;
            this.composerCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.composerCheckbox.Font = new System.Drawing.Font("Nirmala UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.composerCheckbox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.composerCheckbox.Location = new System.Drawing.Point(274, 26);
            this.composerCheckbox.Name = "composerCheckbox";
            this.composerCheckbox.Size = new System.Drawing.Size(78, 17);
            this.composerCheckbox.TabIndex = 9;
            this.composerCheckbox.Text = "Composer";
            this.composerCheckbox.UseVisualStyleBackColor = true;
            this.composerCheckbox.CheckedChanged += new System.EventHandler(this.composerCheckbox_CheckedChanged);
            // 
            // explicitCheckbox
            // 
            this.explicitCheckbox.AutoSize = true;
            this.explicitCheckbox.Checked = true;
            this.explicitCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.explicitCheckbox.Font = new System.Drawing.Font("Nirmala UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.explicitCheckbox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.explicitCheckbox.Location = new System.Drawing.Point(358, 26);
            this.explicitCheckbox.Name = "explicitCheckbox";
            this.explicitCheckbox.Size = new System.Drawing.Size(108, 17);
            this.explicitCheckbox.TabIndex = 19;
            this.explicitCheckbox.Text = "Explicit Advisory";
            this.explicitCheckbox.UseVisualStyleBackColor = true;
            this.explicitCheckbox.CheckedChanged += new System.EventHandler(this.explicitCheckbox_CheckedChanged);
            // 
            // coverArtCheckbox
            // 
            this.coverArtCheckbox.AutoSize = true;
            this.coverArtCheckbox.Checked = true;
            this.coverArtCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.coverArtCheckbox.Font = new System.Drawing.Font("Nirmala UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.coverArtCheckbox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.coverArtCheckbox.Location = new System.Drawing.Point(472, 26);
            this.coverArtCheckbox.Name = "coverArtCheckbox";
            this.coverArtCheckbox.Size = new System.Drawing.Size(73, 17);
            this.coverArtCheckbox.TabIndex = 18;
            this.coverArtCheckbox.Text = "Cover Art";
            this.coverArtCheckbox.UseVisualStyleBackColor = true;
            this.coverArtCheckbox.CheckedChanged += new System.EventHandler(this.coverArtCheckbox_CheckedChanged);
            // 
            // copyrightCheckbox
            // 
            this.copyrightCheckbox.AutoSize = true;
            this.copyrightCheckbox.Checked = true;
            this.copyrightCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.copyrightCheckbox.Font = new System.Drawing.Font("Nirmala UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.copyrightCheckbox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.copyrightCheckbox.Location = new System.Drawing.Point(551, 26);
            this.copyrightCheckbox.Name = "copyrightCheckbox";
            this.copyrightCheckbox.Size = new System.Drawing.Size(77, 17);
            this.copyrightCheckbox.TabIndex = 10;
            this.copyrightCheckbox.Text = "Copyright";
            this.copyrightCheckbox.UseVisualStyleBackColor = true;
            this.copyrightCheckbox.CheckedChanged += new System.EventHandler(this.copyrightCheckbox_CheckedChanged);
            // 
            // labelCheckbox
            // 
            this.labelCheckbox.AutoSize = true;
            this.labelCheckbox.Checked = true;
            this.labelCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.labelCheckbox.Font = new System.Drawing.Font("Nirmala UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCheckbox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.labelCheckbox.Location = new System.Drawing.Point(634, 26);
            this.labelCheckbox.Name = "labelCheckbox";
            this.labelCheckbox.Size = new System.Drawing.Size(53, 17);
            this.labelCheckbox.TabIndex = 11;
            this.labelCheckbox.Text = "Label";
            this.labelCheckbox.UseVisualStyleBackColor = true;
            this.labelCheckbox.CheckedChanged += new System.EventHandler(this.labelCheckbox_CheckedChanged);
            // 
            // upcCheckbox
            // 
            this.upcCheckbox.AutoSize = true;
            this.upcCheckbox.Checked = true;
            this.upcCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.upcCheckbox.Font = new System.Drawing.Font("Nirmala UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.upcCheckbox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.upcCheckbox.Location = new System.Drawing.Point(3, 49);
            this.upcCheckbox.Name = "upcCheckbox";
            this.upcCheckbox.Size = new System.Drawing.Size(99, 17);
            this.upcCheckbox.TabIndex = 12;
            this.upcCheckbox.Text = "UPC / Barcode";
            this.upcCheckbox.UseVisualStyleBackColor = true;
            this.upcCheckbox.CheckedChanged += new System.EventHandler(this.upcCheckbox_CheckedChanged);
            // 
            // isrcCheckbox
            // 
            this.isrcCheckbox.AutoSize = true;
            this.isrcCheckbox.Checked = true;
            this.isrcCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.isrcCheckbox.Font = new System.Drawing.Font("Nirmala UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.isrcCheckbox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.isrcCheckbox.Location = new System.Drawing.Point(108, 49);
            this.isrcCheckbox.Name = "isrcCheckbox";
            this.isrcCheckbox.Size = new System.Drawing.Size(49, 17);
            this.isrcCheckbox.TabIndex = 13;
            this.isrcCheckbox.Text = "ISRC";
            this.isrcCheckbox.UseVisualStyleBackColor = true;
            this.isrcCheckbox.CheckedChanged += new System.EventHandler(this.isrcCheckbox_CheckedChanged);
            // 
            // urlCheckbox
            // 
            this.urlCheckbox.AutoSize = true;
            this.urlCheckbox.Checked = true;
            this.urlCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.urlCheckbox.Font = new System.Drawing.Font("Nirmala UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.urlCheckbox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.urlCheckbox.Location = new System.Drawing.Point(163, 49);
            this.urlCheckbox.Name = "urlCheckbox";
            this.urlCheckbox.Size = new System.Drawing.Size(46, 17);
            this.urlCheckbox.TabIndex = 14;
            this.urlCheckbox.Text = "URL";
            this.urlCheckbox.UseVisualStyleBackColor = true;
            this.urlCheckbox.CheckedChanged += new System.EventHandler(this.urlCheckbox_CheckedChanged);
            // 
            // languageLabel
            // 
            this.languageLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.languageLabel.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.languageLabel.Location = new System.Drawing.Point(22, 487);
            this.languageLabel.Name = "languageLabel";
            this.languageLabel.Size = new System.Drawing.Size(220, 13);
            this.languageLabel.TabIndex = 37;
            this.languageLabel.Text = "Current Language";
            this.languageLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // languageComboBox
            // 
            this.languageComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.languageComboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.languageComboBox.FormattingEnabled = true;
            this.languageComboBox.Location = new System.Drawing.Point(248, 484);
            this.languageComboBox.Name = "languageComboBox";
            this.languageComboBox.Size = new System.Drawing.Size(121, 21);
            this.languageComboBox.TabIndex = 36;
            this.languageComboBox.SelectedIndexChanged += new System.EventHandler(this.languageComboBox_SelectedIndexChanged);
            // 
            // themeLabel
            // 
            this.themeLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.themeLabel.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.themeLabel.Location = new System.Drawing.Point(22, 455);
            this.themeLabel.Name = "themeLabel";
            this.themeLabel.Size = new System.Drawing.Size(220, 13);
            this.themeLabel.TabIndex = 35;
            this.themeLabel.Text = "Current Theme";
            this.themeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // themeComboBox
            // 
            this.themeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.themeComboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.themeComboBox.FormattingEnabled = true;
            this.themeComboBox.Location = new System.Drawing.Point(248, 452);
            this.themeComboBox.Name = "themeComboBox";
            this.themeComboBox.Size = new System.Drawing.Size(121, 21);
            this.themeComboBox.TabIndex = 34;
            this.themeComboBox.SelectedIndexChanged += new System.EventHandler(this.themeComboBox_SelectedIndexChanged);
            // 
            // themeSectionLabel
            // 
            this.themeSectionLabel.Font = new System.Drawing.Font("Nirmala UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.themeSectionLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.themeSectionLabel.Location = new System.Drawing.Point(0, 412);
            this.themeSectionLabel.Name = "themeSectionLabel";
            this.themeSectionLabel.Size = new System.Drawing.Size(771, 25);
            this.themeSectionLabel.TabIndex = 33;
            this.themeSectionLabel.Text = "THEMING OPTIONS";
            this.themeSectionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // commentCheckbox
            // 
            this.commentCheckbox.AutoSize = true;
            this.commentCheckbox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.commentCheckbox.Font = new System.Drawing.Font("Nirmala UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.commentCheckbox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.commentCheckbox.Location = new System.Drawing.Point(20, 155);
            this.commentCheckbox.Name = "commentCheckbox";
            this.commentCheckbox.Size = new System.Drawing.Size(15, 14);
            this.commentCheckbox.TabIndex = 32;
            this.commentCheckbox.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.commentCheckbox.UseVisualStyleBackColor = true;
            this.commentCheckbox.CheckedChanged += new System.EventHandler(this.commentCheckbox_CheckedChanged);
            // 
            // commentTextbox
            // 
            this.commentTextbox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.commentTextbox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.commentTextbox.Font = new System.Drawing.Font("Nirmala UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.commentTextbox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.commentTextbox.Location = new System.Drawing.Point(248, 153);
            this.commentTextbox.Multiline = true;
            this.commentTextbox.Name = "commentTextbox";
            this.commentTextbox.Size = new System.Drawing.Size(503, 21);
            this.commentTextbox.TabIndex = 31;
            this.commentTextbox.WordWrap = false;
            this.commentTextbox.TextChanged += new System.EventHandler(this.commentTextbox_TextChanged);
            // 
            // advancedOptionsLabel
            // 
            this.advancedOptionsLabel.Font = new System.Drawing.Font("Nirmala UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.advancedOptionsLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.advancedOptionsLabel.Location = new System.Drawing.Point(0, 256);
            this.advancedOptionsLabel.Name = "advancedOptionsLabel";
            this.advancedOptionsLabel.Size = new System.Drawing.Size(771, 25);
            this.advancedOptionsLabel.TabIndex = 26;
            this.advancedOptionsLabel.Text = "ADVANCED OPTIONS";
            this.advancedOptionsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // closeAdditionalButton
            // 
            this.closeAdditionalButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(13)))), ((int)(((byte)(13)))), ((int)(((byte)(13)))));
            this.closeAdditionalButton.FlatAppearance.BorderSize = 0;
            this.closeAdditionalButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
            this.closeAdditionalButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(18)))), ((int)(((byte)(18)))));
            this.closeAdditionalButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.closeAdditionalButton.Font = new System.Drawing.Font("Nirmala UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.closeAdditionalButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.closeAdditionalButton.Location = new System.Drawing.Point(327, 523);
            this.closeAdditionalButton.Name = "closeAdditionalButton";
            this.closeAdditionalButton.Size = new System.Drawing.Size(117, 31);
            this.closeAdditionalButton.TabIndex = 25;
            this.closeAdditionalButton.Text = "Back to Settings";
            this.closeAdditionalButton.UseVisualStyleBackColor = false;
            this.closeAdditionalButton.Click += new System.EventHandler(this.closeAdditionalButton_Click);
            // 
            // savedArtLabel
            // 
            this.savedArtLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.savedArtLabel.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.savedArtLabel.Location = new System.Drawing.Point(18, 218);
            this.savedArtLabel.Name = "savedArtLabel";
            this.savedArtLabel.Size = new System.Drawing.Size(224, 13);
            this.savedArtLabel.TabIndex = 24;
            this.savedArtLabel.Text = "Saved Artwork Size On Disk";
            this.savedArtLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // savedArtSizeSelect
            // 
            this.savedArtSizeSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.savedArtSizeSelect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.savedArtSizeSelect.FormattingEnabled = true;
            this.savedArtSizeSelect.Items.AddRange(new object[] {
            "org",
            "max",
            "600",
            "300",
            "150",
            "100",
            "50"});
            this.savedArtSizeSelect.Location = new System.Drawing.Point(248, 215);
            this.savedArtSizeSelect.Name = "savedArtSizeSelect";
            this.savedArtSizeSelect.Size = new System.Drawing.Size(121, 21);
            this.savedArtSizeSelect.TabIndex = 23;
            this.savedArtSizeSelect.SelectedIndexChanged += new System.EventHandler(this.savedArtSizeSelect_SelectedIndexChanged);
            // 
            // taggingOptionsLabel
            // 
            this.taggingOptionsLabel.Font = new System.Drawing.Font("Nirmala UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.taggingOptionsLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.taggingOptionsLabel.Location = new System.Drawing.Point(0, 35);
            this.taggingOptionsLabel.Name = "taggingOptionsLabel";
            this.taggingOptionsLabel.Size = new System.Drawing.Size(771, 25);
            this.taggingOptionsLabel.TabIndex = 22;
            this.taggingOptionsLabel.Text = "TAGGING OPTIONS";
            this.taggingOptionsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // embeddedArtLabel
            // 
            this.embeddedArtLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.embeddedArtLabel.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.embeddedArtLabel.Location = new System.Drawing.Point(18, 186);
            this.embeddedArtLabel.Name = "embeddedArtLabel";
            this.embeddedArtLabel.Size = new System.Drawing.Size(224, 13);
            this.embeddedArtLabel.TabIndex = 21;
            this.embeddedArtLabel.Text = "Embedded Artwork Size";
            this.embeddedArtLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // embeddedArtSizeSelect
            // 
            this.embeddedArtSizeSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.embeddedArtSizeSelect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.embeddedArtSizeSelect.FormattingEnabled = true;
            this.embeddedArtSizeSelect.Items.AddRange(new object[] {
            "org",
            "max",
            "600",
            "300",
            "150",
            "100",
            "50"});
            this.embeddedArtSizeSelect.Location = new System.Drawing.Point(248, 183);
            this.embeddedArtSizeSelect.Name = "embeddedArtSizeSelect";
            this.embeddedArtSizeSelect.Size = new System.Drawing.Size(121, 21);
            this.embeddedArtSizeSelect.TabIndex = 20;
            this.embeddedArtSizeSelect.SelectedIndexChanged += new System.EventHandler(this.embeddedArtSizeSelect_SelectedIndexChanged);
            // 
            // extraSettingsLabel
            // 
            this.extraSettingsLabel.Font = new System.Drawing.Font("Nirmala UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.extraSettingsLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.extraSettingsLabel.Location = new System.Drawing.Point(13, 10);
            this.extraSettingsLabel.Name = "extraSettingsLabel";
            this.extraSettingsLabel.Size = new System.Drawing.Size(752, 25);
            this.extraSettingsLabel.TabIndex = 1;
            this.extraSettingsLabel.Text = "ADDITIONAL SETTINGS                                                              " +
    "       ";
            this.extraSettingsLabel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.extraSettingsLabel_MouseMove);
            // 
            // qualitySelectButton
            // 
            this.qualitySelectButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.qualitySelectButton.AutoSize = true;
            this.qualitySelectButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.qualitySelectButton.FlatAppearance.BorderSize = 0;
            this.qualitySelectButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
            this.qualitySelectButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(26)))), ((int)(((byte)(26)))));
            this.qualitySelectButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.qualitySelectButton.Font = new System.Drawing.Font("Nirmala UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.qualitySelectButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.qualitySelectButton.Location = new System.Drawing.Point(749, -1);
            this.qualitySelectButton.Name = "qualitySelectButton";
            this.qualitySelectButton.Size = new System.Drawing.Size(132, 31);
            this.qualitySelectButton.TabIndex = 1;
            this.qualitySelectButton.Text = "Quality Selector";
            this.qualitySelectButton.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.qualitySelectButton.UseVisualStyleBackColor = false;
            this.qualitySelectButton.Click += new System.EventHandler(this.qualitySelectButton_Click);
            // 
            // qualitySelectPanel
            // 
            this.qualitySelectPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.qualitySelectPanel.Controls.Add(this.mp3Button2);
            this.qualitySelectPanel.Controls.Add(this.flacLowButton2);
            this.qualitySelectPanel.Controls.Add(this.flacMidButton2);
            this.qualitySelectPanel.Controls.Add(this.flacHighButton2);
            this.qualitySelectPanel.Controls.Add(this.mp3Label2);
            this.qualitySelectPanel.Controls.Add(this.flacLowLabel2);
            this.qualitySelectPanel.Controls.Add(this.flacMidLabel2);
            this.qualitySelectPanel.Controls.Add(this.flacHighLabel2);
            this.qualitySelectPanel.Location = new System.Drawing.Point(730, 30);
            this.qualitySelectPanel.Name = "qualitySelectPanel";
            this.qualitySelectPanel.Size = new System.Drawing.Size(168, 87);
            this.qualitySelectPanel.TabIndex = 4;
            this.qualitySelectPanel.Visible = false;
            this.qualitySelectPanel.VisibleChanged += new System.EventHandler(this.qualitySelectPanel_VisibleChanged);
            // 
            // mp3Button2
            // 
            this.mp3Button2.AutoSize = true;
            this.mp3Button2.FlatAppearance.BorderSize = 0;
            this.mp3Button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.mp3Button2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.mp3Button2.Location = new System.Drawing.Point(15, 63);
            this.mp3Button2.Name = "mp3Button2";
            this.mp3Button2.Size = new System.Drawing.Size(13, 12);
            this.mp3Button2.TabIndex = 7;
            this.mp3Button2.UseVisualStyleBackColor = true;
            this.mp3Button2.CheckedChanged += new System.EventHandler(this.mp3Button2_CheckedChanged);
            // 
            // flacLowButton2
            // 
            this.flacLowButton2.AutoSize = true;
            this.flacLowButton2.FlatAppearance.BorderSize = 0;
            this.flacLowButton2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.flacLowButton2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.flacLowButton2.Location = new System.Drawing.Point(15, 45);
            this.flacLowButton2.Name = "flacLowButton2";
            this.flacLowButton2.Size = new System.Drawing.Size(13, 12);
            this.flacLowButton2.TabIndex = 5;
            this.flacLowButton2.UseVisualStyleBackColor = true;
            this.flacLowButton2.CheckedChanged += new System.EventHandler(this.flacLowButton2_CheckedChanged);
            // 
            // flacMidButton2
            // 
            this.flacMidButton2.AutoSize = true;
            this.flacMidButton2.FlatAppearance.BorderSize = 0;
            this.flacMidButton2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.flacMidButton2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.flacMidButton2.Location = new System.Drawing.Point(15, 27);
            this.flacMidButton2.Name = "flacMidButton2";
            this.flacMidButton2.Size = new System.Drawing.Size(13, 12);
            this.flacMidButton2.TabIndex = 3;
            this.flacMidButton2.UseVisualStyleBackColor = true;
            this.flacMidButton2.CheckedChanged += new System.EventHandler(this.flacMidButton2_CheckedChanged);
            // 
            // flacHighButton2
            // 
            this.flacHighButton2.AutoSize = true;
            this.flacHighButton2.FlatAppearance.BorderSize = 0;
            this.flacHighButton2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.flacHighButton2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.flacHighButton2.Location = new System.Drawing.Point(15, 9);
            this.flacHighButton2.Name = "flacHighButton2";
            this.flacHighButton2.Size = new System.Drawing.Size(13, 12);
            this.flacHighButton2.TabIndex = 1;
            this.flacHighButton2.UseVisualStyleBackColor = true;
            this.flacHighButton2.CheckedChanged += new System.EventHandler(this.flacHighButton2_CheckedChanged);
            // 
            // mp3Label2
            // 
            this.mp3Label2.AutoSize = true;
            this.mp3Label2.Font = new System.Drawing.Font("Nirmala UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mp3Label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.mp3Label2.Location = new System.Drawing.Point(31, 64);
            this.mp3Label2.Name = "mp3Label2";
            this.mp3Label2.Size = new System.Drawing.Size(84, 13);
            this.mp3Label2.TabIndex = 0;
            this.mp3Label2.Text = "MP3 (320 kbps)";
            this.mp3Label2.Click += new System.EventHandler(this.mp3Label2_Click);
            // 
            // flacLowLabel2
            // 
            this.flacLowLabel2.AutoSize = true;
            this.flacLowLabel2.Font = new System.Drawing.Font("Nirmala UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.flacLowLabel2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.flacLowLabel2.Location = new System.Drawing.Point(31, 46);
            this.flacLowLabel2.Name = "flacLowLabel2";
            this.flacLowLabel2.Size = new System.Drawing.Size(123, 13);
            this.flacLowLabel2.TabIndex = 6;
            this.flacLowLabel2.Text = "FLAC (16 bit / 44.1 kHz)";
            this.flacLowLabel2.Click += new System.EventHandler(this.flacLowLabel2_Click);
            // 
            // flacMidLabel2
            // 
            this.flacMidLabel2.AutoSize = true;
            this.flacMidLabel2.Font = new System.Drawing.Font("Nirmala UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.flacMidLabel2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.flacMidLabel2.Location = new System.Drawing.Point(31, 28);
            this.flacMidLabel2.Name = "flacMidLabel2";
            this.flacMidLabel2.Size = new System.Drawing.Size(114, 13);
            this.flacMidLabel2.TabIndex = 4;
            this.flacMidLabel2.Text = "FLAC (24 bit / 96 kHz)";
            this.flacMidLabel2.Click += new System.EventHandler(this.flacMidLabel2_Click);
            // 
            // flacHighLabel2
            // 
            this.flacHighLabel2.AutoSize = true;
            this.flacHighLabel2.Font = new System.Drawing.Font("Nirmala UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.flacHighLabel2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.flacHighLabel2.Location = new System.Drawing.Point(31, 10);
            this.flacHighLabel2.Name = "flacHighLabel2";
            this.flacHighLabel2.Size = new System.Drawing.Size(120, 13);
            this.flacHighLabel2.TabIndex = 2;
            this.flacHighLabel2.Text = "FLAC (24 bit / 192 kHz)";
            this.flacHighLabel2.Click += new System.EventHandler(this.flacHighLabel2_Click);
            // 
            // movingLabel
            // 
            this.movingLabel.AutoSize = true;
            this.movingLabel.BackColor = System.Drawing.Color.Transparent;
            this.movingLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(18)))), ((int)(((byte)(18)))));
            this.movingLabel.Location = new System.Drawing.Point(181, -5);
            this.movingLabel.Name = "movingLabel";
            this.movingLabel.Size = new System.Drawing.Size(565, 13);
            this.movingLabel.TabIndex = 0;
            this.movingLabel.Text = "=================================================================================" +
    "============";
            this.movingLabel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.movingLabel_MouseMove);
            // 
            // searchPanel
            // 
            this.searchPanel.Controls.Add(this.deselectAllRowsButton);
            this.searchPanel.Controls.Add(this.selectAllRowsButton);
            this.searchPanel.Controls.Add(this.batchDownloadSelectedRowsButton);
            this.searchPanel.Controls.Add(this.selectedRowsCountLabel);
            this.searchPanel.Controls.Add(this.limitSearchResultsLabel);
            this.searchPanel.Controls.Add(this.searchResultsCountLabel);
            this.searchPanel.Controls.Add(this.searchSortingLabel);
            this.searchPanel.Controls.Add(this.searchSortingPanel);
            this.searchPanel.Controls.Add(this.limitSearchResultsNumericUpDown);
            this.searchPanel.Controls.Add(this.searchResultsPanel);
            this.searchPanel.Controls.Add(this.searchAlbumsButton);
            this.searchPanel.Controls.Add(this.searchTracksButton);
            this.searchPanel.Controls.Add(this.searchTextbox);
            this.searchPanel.Controls.Add(this.searchLabel);
            this.searchPanel.Controls.Add(this.searchingLabel);
            this.searchPanel.Controls.Add(this.sortingSearchResultsLabel);
            this.searchPanel.Controls.Add(this.sortAscendantCheckBox);
            this.searchPanel.Font = new System.Drawing.Font("Nirmala UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.searchPanel.Location = new System.Drawing.Point(245, 36);
            this.searchPanel.Name = "searchPanel";
            this.searchPanel.Size = new System.Drawing.Size(771, 577);
            this.searchPanel.TabIndex = 29;
            // 
            // deselectAllRowsButton
            // 
            this.deselectAllRowsButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(13)))), ((int)(((byte)(13)))), ((int)(((byte)(13)))));
            this.deselectAllRowsButton.Enabled = false;
            this.deselectAllRowsButton.FlatAppearance.BorderSize = 0;
            this.deselectAllRowsButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
            this.deselectAllRowsButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(18)))), ((int)(((byte)(18)))));
            this.deselectAllRowsButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.deselectAllRowsButton.Font = new System.Drawing.Font("Nirmala UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.deselectAllRowsButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.deselectAllRowsButton.Location = new System.Drawing.Point(287, 550);
            this.deselectAllRowsButton.Name = "deselectAllRowsButton";
            this.deselectAllRowsButton.Size = new System.Drawing.Size(120, 25);
            this.deselectAllRowsButton.TabIndex = 21;
            this.deselectAllRowsButton.Text = "Deselect all";
            this.deselectAllRowsButton.UseVisualStyleBackColor = false;
            this.deselectAllRowsButton.Click += new System.EventHandler(this.deselectAllRowsButton_Click);
            // 
            // selectAllRowsButton
            // 
            this.selectAllRowsButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(13)))), ((int)(((byte)(13)))), ((int)(((byte)(13)))));
            this.selectAllRowsButton.Enabled = false;
            this.selectAllRowsButton.FlatAppearance.BorderSize = 0;
            this.selectAllRowsButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
            this.selectAllRowsButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(18)))), ((int)(((byte)(18)))));
            this.selectAllRowsButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.selectAllRowsButton.Font = new System.Drawing.Font("Nirmala UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.selectAllRowsButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.selectAllRowsButton.Location = new System.Drawing.Point(161, 550);
            this.selectAllRowsButton.Name = "selectAllRowsButton";
            this.selectAllRowsButton.Size = new System.Drawing.Size(120, 25);
            this.selectAllRowsButton.TabIndex = 20;
            this.selectAllRowsButton.Text = "Select all";
            this.selectAllRowsButton.UseVisualStyleBackColor = false;
            this.selectAllRowsButton.Click += new System.EventHandler(this.selectAllRowsButton_Click);
            // 
            // batchDownloadSelectedRowsButton
            // 
            this.batchDownloadSelectedRowsButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(13)))), ((int)(((byte)(13)))), ((int)(((byte)(13)))));
            this.batchDownloadSelectedRowsButton.Enabled = false;
            this.batchDownloadSelectedRowsButton.FlatAppearance.BorderSize = 0;
            this.batchDownloadSelectedRowsButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
            this.batchDownloadSelectedRowsButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(18)))), ((int)(((byte)(18)))));
            this.batchDownloadSelectedRowsButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.batchDownloadSelectedRowsButton.Font = new System.Drawing.Font("Nirmala UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.batchDownloadSelectedRowsButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.batchDownloadSelectedRowsButton.Location = new System.Drawing.Point(413, 549);
            this.batchDownloadSelectedRowsButton.Name = "batchDownloadSelectedRowsButton";
            this.batchDownloadSelectedRowsButton.Size = new System.Drawing.Size(340, 25);
            this.batchDownloadSelectedRowsButton.TabIndex = 19;
            this.batchDownloadSelectedRowsButton.Text = "BATCH DOWNLOAD SELECTED ROWS";
            this.batchDownloadSelectedRowsButton.UseVisualStyleBackColor = false;
            this.batchDownloadSelectedRowsButton.Click += new System.EventHandler(this.batchDownloadSelectedRowsButton_Click);
            // 
            // selectedRowsCountLabel
            // 
            this.selectedRowsCountLabel.Font = new System.Drawing.Font("Nirmala UI", 9F);
            this.selectedRowsCountLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.selectedRowsCountLabel.Location = new System.Drawing.Point(20, 550);
            this.selectedRowsCountLabel.Name = "selectedRowsCountLabel";
            this.selectedRowsCountLabel.Size = new System.Drawing.Size(135, 24);
            this.selectedRowsCountLabel.TabIndex = 18;
            this.selectedRowsCountLabel.Text = "0 selected rows";
            this.selectedRowsCountLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // limitSearchResultsLabel
            // 
            this.limitSearchResultsLabel.Font = new System.Drawing.Font("Nirmala UI", 10F);
            this.limitSearchResultsLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.limitSearchResultsLabel.Location = new System.Drawing.Point(408, 84);
            this.limitSearchResultsLabel.Name = "limitSearchResultsLabel";
            this.limitSearchResultsLabel.Size = new System.Drawing.Size(116, 27);
            this.limitSearchResultsLabel.TabIndex = 12;
            this.limitSearchResultsLabel.Text = "Results Limit:";
            this.limitSearchResultsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // searchResultsCountLabel
            // 
            this.searchResultsCountLabel.Font = new System.Drawing.Font("Nirmala UI", 12F);
            this.searchResultsCountLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.searchResultsCountLabel.Location = new System.Drawing.Point(585, 87);
            this.searchResultsCountLabel.Name = "searchResultsCountLabel";
            this.searchResultsCountLabel.Size = new System.Drawing.Size(168, 28);
            this.searchResultsCountLabel.TabIndex = 16;
            this.searchResultsCountLabel.Text = "…";
            this.searchResultsCountLabel.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // searchSortingLabel
            // 
            this.searchSortingLabel.AutoSize = true;
            this.searchSortingLabel.Font = new System.Drawing.Font("Nirmala UI", 10F);
            this.searchSortingLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.searchSortingLabel.Location = new System.Drawing.Point(18, 77);
            this.searchSortingLabel.Name = "searchSortingLabel";
            this.searchSortingLabel.Size = new System.Drawing.Size(56, 19);
            this.searchSortingLabel.TabIndex = 15;
            this.searchSortingLabel.Text = "Sorting:";
            this.searchSortingLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // searchSortingPanel
            // 
            this.searchSortingPanel.Controls.Add(this.sortGenreLabel);
            this.searchSortingPanel.Controls.Add(this.sortGenreButton);
            this.searchSortingPanel.Controls.Add(this.sortAlbumTrackNameLabel);
            this.searchSortingPanel.Controls.Add(this.sortArtistNameLabel);
            this.searchSortingPanel.Controls.Add(this.sortAlbumTrackNameButton);
            this.searchSortingPanel.Controls.Add(this.sortArtistNameButton);
            this.searchSortingPanel.Controls.Add(this.sortReleaseDateButton);
            this.searchSortingPanel.Controls.Add(this.sortReleaseDateLabel);
            this.searchSortingPanel.Location = new System.Drawing.Point(120, 78);
            this.searchSortingPanel.Name = "searchSortingPanel";
            this.searchSortingPanel.Size = new System.Drawing.Size(290, 38);
            this.searchSortingPanel.TabIndex = 14;
            // 
            // sortGenreLabel
            // 
            this.sortGenreLabel.AutoSize = true;
            this.sortGenreLabel.Cursor = System.Windows.Forms.Cursors.Default;
            this.sortGenreLabel.Font = new System.Drawing.Font("Nirmala UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sortGenreLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.sortGenreLabel.Location = new System.Drawing.Point(18, 21);
            this.sortGenreLabel.Name = "sortGenreLabel";
            this.sortGenreLabel.Size = new System.Drawing.Size(38, 13);
            this.sortGenreLabel.TabIndex = 10;
            this.sortGenreLabel.Text = "Genre";
            this.sortGenreLabel.Click += new System.EventHandler(this.sortGenreLabel_Click);
            // 
            // sortGenreButton
            // 
            this.sortGenreButton.AutoSize = true;
            this.sortGenreButton.Cursor = System.Windows.Forms.Cursors.Default;
            this.sortGenreButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.sortGenreButton.Location = new System.Drawing.Point(4, 21);
            this.sortGenreButton.Name = "sortGenreButton";
            this.sortGenreButton.Size = new System.Drawing.Size(14, 13);
            this.sortGenreButton.TabIndex = 9;
            this.sortGenreButton.UseVisualStyleBackColor = true;
            this.sortGenreButton.CheckedChanged += new System.EventHandler(this.sortGenreButton_CheckedChanged);
            // 
            // sortAlbumTrackNameLabel
            // 
            this.sortAlbumTrackNameLabel.AutoSize = true;
            this.sortAlbumTrackNameLabel.Cursor = System.Windows.Forms.Cursors.Default;
            this.sortAlbumTrackNameLabel.Font = new System.Drawing.Font("Nirmala UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sortAlbumTrackNameLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.sortAlbumTrackNameLabel.Location = new System.Drawing.Point(150, 21);
            this.sortAlbumTrackNameLabel.Name = "sortAlbumTrackNameLabel";
            this.sortAlbumTrackNameLabel.Size = new System.Drawing.Size(99, 13);
            this.sortAlbumTrackNameLabel.TabIndex = 8;
            this.sortAlbumTrackNameLabel.Text = "Album / Track Title";
            this.sortAlbumTrackNameLabel.Click += new System.EventHandler(this.sortAlbumTrackNameLabel_Click);
            // 
            // sortArtistNameLabel
            // 
            this.sortArtistNameLabel.AutoSize = true;
            this.sortArtistNameLabel.Cursor = System.Windows.Forms.Cursors.Default;
            this.sortArtistNameLabel.Font = new System.Drawing.Font("Nirmala UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sortArtistNameLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.sortArtistNameLabel.Location = new System.Drawing.Point(150, 2);
            this.sortArtistNameLabel.Name = "sortArtistNameLabel";
            this.sortArtistNameLabel.Size = new System.Drawing.Size(66, 13);
            this.sortArtistNameLabel.TabIndex = 7;
            this.sortArtistNameLabel.Text = "Artist Name";
            this.sortArtistNameLabel.Click += new System.EventHandler(this.sortArtistNameLabel_Click);
            // 
            // sortAlbumTrackNameButton
            // 
            this.sortAlbumTrackNameButton.AutoSize = true;
            this.sortAlbumTrackNameButton.Cursor = System.Windows.Forms.Cursors.Default;
            this.sortAlbumTrackNameButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.sortAlbumTrackNameButton.Location = new System.Drawing.Point(135, 21);
            this.sortAlbumTrackNameButton.Name = "sortAlbumTrackNameButton";
            this.sortAlbumTrackNameButton.Size = new System.Drawing.Size(14, 13);
            this.sortAlbumTrackNameButton.TabIndex = 5;
            this.sortAlbumTrackNameButton.UseVisualStyleBackColor = true;
            this.sortAlbumTrackNameButton.CheckedChanged += new System.EventHandler(this.sortAlbumTrackNameButton_CheckedChanged);
            // 
            // sortArtistNameButton
            // 
            this.sortArtistNameButton.AutoSize = true;
            this.sortArtistNameButton.Cursor = System.Windows.Forms.Cursors.Default;
            this.sortArtistNameButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.sortArtistNameButton.Location = new System.Drawing.Point(135, 3);
            this.sortArtistNameButton.Name = "sortArtistNameButton";
            this.sortArtistNameButton.Size = new System.Drawing.Size(14, 13);
            this.sortArtistNameButton.TabIndex = 4;
            this.sortArtistNameButton.UseVisualStyleBackColor = true;
            this.sortArtistNameButton.CheckedChanged += new System.EventHandler(this.sortArtistNameButton_CheckedChanged);
            // 
            // sortReleaseDateButton
            // 
            this.sortReleaseDateButton.AutoSize = true;
            this.sortReleaseDateButton.Checked = true;
            this.sortReleaseDateButton.Cursor = System.Windows.Forms.Cursors.Default;
            this.sortReleaseDateButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.sortReleaseDateButton.Location = new System.Drawing.Point(4, 3);
            this.sortReleaseDateButton.Name = "sortReleaseDateButton";
            this.sortReleaseDateButton.Size = new System.Drawing.Size(14, 13);
            this.sortReleaseDateButton.TabIndex = 3;
            this.sortReleaseDateButton.TabStop = true;
            this.sortReleaseDateButton.UseVisualStyleBackColor = true;
            this.sortReleaseDateButton.CheckedChanged += new System.EventHandler(this.sortReleaseDateButton_CheckedChanged);
            // 
            // sortReleaseDateLabel
            // 
            this.sortReleaseDateLabel.AutoSize = true;
            this.sortReleaseDateLabel.Cursor = System.Windows.Forms.Cursors.Default;
            this.sortReleaseDateLabel.Font = new System.Drawing.Font("Nirmala UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sortReleaseDateLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.sortReleaseDateLabel.Location = new System.Drawing.Point(18, 2);
            this.sortReleaseDateLabel.Name = "sortReleaseDateLabel";
            this.sortReleaseDateLabel.Size = new System.Drawing.Size(73, 13);
            this.sortReleaseDateLabel.TabIndex = 6;
            this.sortReleaseDateLabel.Text = "Release Date";
            this.sortReleaseDateLabel.Click += new System.EventHandler(this.sortReleaseDateLabel_Click);
            // 
            // limitSearchResultsNumericUpDown
            // 
            this.limitSearchResultsNumericUpDown.BackColor = System.Drawing.SystemColors.Window;
            this.limitSearchResultsNumericUpDown.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.limitSearchResultsNumericUpDown.Font = new System.Drawing.Font("Nirmala UI", 12F);
            this.limitSearchResultsNumericUpDown.Location = new System.Drawing.Point(525, 86);
            this.limitSearchResultsNumericUpDown.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.limitSearchResultsNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.limitSearchResultsNumericUpDown.Name = "limitSearchResultsNumericUpDown";
            this.limitSearchResultsNumericUpDown.Size = new System.Drawing.Size(50, 25);
            this.limitSearchResultsNumericUpDown.TabIndex = 13;
            this.limitSearchResultsNumericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.limitSearchResultsNumericUpDown.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // searchResultsPanel
            // 
            this.searchResultsPanel.AutoScroll = true;
            this.searchResultsPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.searchResultsPanel.Controls.Add(this.searchResultsTablePanel);
            this.searchResultsPanel.Location = new System.Drawing.Point(18, 116);
            this.searchResultsPanel.Name = "searchResultsPanel";
            this.searchResultsPanel.Size = new System.Drawing.Size(733, 431);
            this.searchResultsPanel.TabIndex = 10;
            // 
            // searchResultsTablePanel
            // 
            this.searchResultsTablePanel.ColumnCount = 4;
            this.searchResultsTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.searchResultsTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.searchResultsTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.searchResultsTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 687F));
            this.searchResultsTablePanel.Location = new System.Drawing.Point(3, 3);
            this.searchResultsTablePanel.Name = "searchResultsTablePanel";
            this.searchResultsTablePanel.RowCount = 1;
            this.searchResultsTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.searchResultsTablePanel.Size = new System.Drawing.Size(710, 476);
            this.searchResultsTablePanel.TabIndex = 0;
            // 
            // searchAlbumsButton
            // 
            this.searchAlbumsButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(13)))), ((int)(((byte)(13)))), ((int)(((byte)(13)))));
            this.searchAlbumsButton.FlatAppearance.BorderSize = 0;
            this.searchAlbumsButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
            this.searchAlbumsButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(18)))), ((int)(((byte)(18)))));
            this.searchAlbumsButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.searchAlbumsButton.Font = new System.Drawing.Font("Nirmala UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.searchAlbumsButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.searchAlbumsButton.Location = new System.Drawing.Point(581, 46);
            this.searchAlbumsButton.Name = "searchAlbumsButton";
            this.searchAlbumsButton.Size = new System.Drawing.Size(82, 31);
            this.searchAlbumsButton.TabIndex = 9;
            this.searchAlbumsButton.Text = "RELEASES";
            this.searchAlbumsButton.UseVisualStyleBackColor = false;
            this.searchAlbumsButton.Click += new System.EventHandler(this.searchAlbumsButton_Click);
            // 
            // searchTracksButton
            // 
            this.searchTracksButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(13)))), ((int)(((byte)(13)))), ((int)(((byte)(13)))));
            this.searchTracksButton.FlatAppearance.BorderSize = 0;
            this.searchTracksButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
            this.searchTracksButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(18)))), ((int)(((byte)(18)))));
            this.searchTracksButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.searchTracksButton.Font = new System.Drawing.Font("Nirmala UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.searchTracksButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.searchTracksButton.Location = new System.Drawing.Point(669, 46);
            this.searchTracksButton.Name = "searchTracksButton";
            this.searchTracksButton.Size = new System.Drawing.Size(82, 31);
            this.searchTracksButton.TabIndex = 8;
            this.searchTracksButton.Text = "TRACKS";
            this.searchTracksButton.UseVisualStyleBackColor = false;
            this.searchTracksButton.Click += new System.EventHandler(this.searchTracksButton_Click);
            // 
            // searchTextbox
            // 
            this.searchTextbox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.searchTextbox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.searchTextbox.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.searchTextbox.Font = new System.Drawing.Font("Nirmala UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.searchTextbox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.searchTextbox.Location = new System.Drawing.Point(18, 48);
            this.searchTextbox.MaxLength = 1000;
            this.searchTextbox.Multiline = true;
            this.searchTextbox.Name = "searchTextbox";
            this.searchTextbox.Size = new System.Drawing.Size(557, 27);
            this.searchTextbox.TabIndex = 7;
            this.searchTextbox.Text = "Input your search…";
            this.searchTextbox.WordWrap = false;
            this.searchTextbox.Click += new System.EventHandler(this.searchTextbox_Click);
            this.searchTextbox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.searchTextbox_KeyDown);
            this.searchTextbox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.searchTextbox_KeyUp);
            this.searchTextbox.Leave += new System.EventHandler(this.searchTextbox_Leave);
            this.searchTextbox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.searchTextbox_MouseDown);
            // 
            // searchLabel
            // 
            this.searchLabel.Font = new System.Drawing.Font("Nirmala UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.searchLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.searchLabel.Location = new System.Drawing.Point(13, 10);
            this.searchLabel.Name = "searchLabel";
            this.searchLabel.Size = new System.Drawing.Size(562, 30);
            this.searchLabel.TabIndex = 1;
            this.searchLabel.Text = "SEARCH";
            this.searchLabel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.searchLabel_MouseMove);
            // 
            // searchingLabel
            // 
            this.searchingLabel.Font = new System.Drawing.Font("Nirmala UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.searchingLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.searchingLabel.Location = new System.Drawing.Point(581, 49);
            this.searchingLabel.Name = "searchingLabel";
            this.searchingLabel.Size = new System.Drawing.Size(170, 25);
            this.searchingLabel.TabIndex = 11;
            this.searchingLabel.Text = "Searching…";
            this.searchingLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.searchingLabel.Visible = false;
            // 
            // sortingSearchResultsLabel
            // 
            this.sortingSearchResultsLabel.Font = new System.Drawing.Font("Nirmala UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sortingSearchResultsLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.sortingSearchResultsLabel.Location = new System.Drawing.Point(581, 49);
            this.sortingSearchResultsLabel.Name = "sortingSearchResultsLabel";
            this.sortingSearchResultsLabel.Size = new System.Drawing.Size(170, 25);
            this.sortingSearchResultsLabel.TabIndex = 12;
            this.sortingSearchResultsLabel.Text = "Sorting…";
            this.sortingSearchResultsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.sortingSearchResultsLabel.Visible = false;
            // 
            // sortAscendantCheckBox
            // 
            this.sortAscendantCheckBox.AutoSize = true;
            this.sortAscendantCheckBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.sortAscendantCheckBox.Location = new System.Drawing.Point(21, 99);
            this.sortAscendantCheckBox.Name = "sortAscendantCheckBox";
            this.sortAscendantCheckBox.Size = new System.Drawing.Size(80, 17);
            this.sortAscendantCheckBox.TabIndex = 9;
            this.sortAscendantCheckBox.Text = "Ascendant";
            this.sortAscendantCheckBox.UseVisualStyleBackColor = true;
            this.sortAscendantCheckBox.CheckedChanged += new System.EventHandler(this.sortAscendantCheckBox_CheckedChanged);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.notifyIcon1.ContextMenuStrip = this.sysTrayContextMenuStrip;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "QobuzDLX";
            this.notifyIcon1.DoubleClick += new System.EventHandler(this.notifyIcon1_DoubleClick);
            // 
            // sysTrayContextMenuStrip
            // 
            this.sysTrayContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showWindowToolStripMenuItem,
            this.hideWindowToolStripMenuItem,
            this.closeProgramToolStripMenuItem});
            this.sysTrayContextMenuStrip.Name = "sysTrayContextMenuStrip";
            this.sysTrayContextMenuStrip.Size = new System.Drawing.Size(153, 70);
            this.sysTrayContextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.sysTrayContextMenuStrip_Opening);
            // 
            // showWindowToolStripMenuItem
            // 
            this.showWindowToolStripMenuItem.Image = global::QobuzDownloaderX.Properties.Resources.ShowWindow;
            this.showWindowToolStripMenuItem.Name = "showWindowToolStripMenuItem";
            this.showWindowToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.showWindowToolStripMenuItem.Text = "Show window";
            this.showWindowToolStripMenuItem.Click += new System.EventHandler(this.showWindowToolStripMenuItem_Click);
            // 
            // hideWindowToolStripMenuItem
            // 
            this.hideWindowToolStripMenuItem.Image = global::QobuzDownloaderX.Properties.Resources.HideWindow;
            this.hideWindowToolStripMenuItem.Name = "hideWindowToolStripMenuItem";
            this.hideWindowToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.hideWindowToolStripMenuItem.Text = "Hide window";
            this.hideWindowToolStripMenuItem.Click += new System.EventHandler(this.hideWindowToolStripMenuItem_Click);
            // 
            // closeProgramToolStripMenuItem
            // 
            this.closeProgramToolStripMenuItem.Image = global::QobuzDownloaderX.Properties.Resources.Exit;
            this.closeProgramToolStripMenuItem.Name = "closeProgramToolStripMenuItem";
            this.closeProgramToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.closeProgramToolStripMenuItem.Text = "Close program";
            this.closeProgramToolStripMenuItem.Click += new System.EventHandler(this.closeProgramToolStripMenuItem_Click);
            // 
            // progressBarDownload
            // 
            this.progressBarDownload.BackgroundColor = System.Drawing.SystemColors.Window;
            this.progressBarDownload.BorderColor = System.Drawing.Color.Black;
            this.progressBarDownload.FillColor = System.Drawing.Color.RoyalBlue;
            this.progressBarDownload.Location = new System.Drawing.Point(184, 89);
            this.progressBarDownload.Name = "progressBarDownload";
            this.progressBarDownload.Size = new System.Drawing.Size(332, 23);
            this.progressBarDownload.Step = 1;
            this.progressBarDownload.TabIndex = 4;
            // 
            // qbdlxForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(18)))), ((int)(((byte)(18)))));
            this.ClientSize = new System.Drawing.Size(951, 615);
            this.Controls.Add(this.exitButton);
            this.Controls.Add(this.minimizeButton);
            this.Controls.Add(this.qualitySelectButton);
            this.Controls.Add(this.qualitySelectPanel);
            this.Controls.Add(this.downloaderPanel);
            this.Controls.Add(this.aboutPanel);
            this.Controls.Add(this.movingLabel);
            this.Controls.Add(this.navigationPanel);
            this.Controls.Add(this.settingsPanel);
            this.Controls.Add(this.searchPanel);
            this.Controls.Add(this.extraSettingsPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "qbdlxForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "QobuzDLX";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.qbdlxForm_FormClosing);
            this.Load += new System.EventHandler(this.qbdlxForm_Load);
            this.Shown += new System.EventHandler(this.qbdlxForm_Shown);
            this.navigationPanel.ResumeLayout(false);
            this.logoPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.logoPictureBox)).EndInit();
            this.downloaderPanel.ResumeLayout(false);
            this.downloaderPanel.PerformLayout();
            this.batchDownloadPanel.ResumeLayout(false);
            this.batchDownloadPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.albumPictureBox)).EndInit();
            this.settingsPanel.ResumeLayout(false);
            this.settingsPanel.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.folderButtonsPanel.ResumeLayout(false);
            this.folderButtonsPanel.PerformLayout();
            this.mainContextMenuStrip.ResumeLayout(false);
            this.aboutPanel.ResumeLayout(false);
            this.aboutPanel.PerformLayout();
            this.extraSettingsPanel.ResumeLayout(false);
            this.extraSettingsPanel.PerformLayout();
            this.advancedOptionsPanelRight.ResumeLayout(false);
            this.advancedOptionsPanelRight.PerformLayout();
            this.advancedOptionsPanelLeft.ResumeLayout(false);
            this.advancedOptionsPanelLeft.PerformLayout();
            this.taggingOptionsPanel.ResumeLayout(false);
            this.taggingOptionsPanel.PerformLayout();
            this.qualitySelectPanel.ResumeLayout(false);
            this.qualitySelectPanel.PerformLayout();
            this.searchPanel.ResumeLayout(false);
            this.searchPanel.PerformLayout();
            this.searchSortingPanel.ResumeLayout(false);
            this.searchSortingPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.limitSearchResultsNumericUpDown)).EndInit();
            this.searchResultsPanel.ResumeLayout(false);
            this.sysTrayContextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.Panel navigationPanel;
        internal System.Windows.Forms.Panel logoPanel;
        internal System.Windows.Forms.PictureBox logoPictureBox;
        internal System.Windows.Forms.Button downloaderButton;
        internal System.Windows.Forms.Button settingsButton;
        internal System.Windows.Forms.Button aboutButton;
        internal System.Windows.Forms.Panel downloaderPanel;
        internal System.Windows.Forms.Panel settingsPanel;
        internal System.Windows.Forms.Label downloadLabel;
        internal System.Windows.Forms.Button exitButton;
        internal System.Windows.Forms.Button minimizeButton;
        internal System.Windows.Forms.Label settingsLabel;
        internal System.Windows.Forms.Label versionNumber;
        internal System.Windows.Forms.Panel aboutPanel;
        internal System.Windows.Forms.Label aboutLabel;
        internal System.Windows.Forms.Button logoutButton;
        internal System.Windows.Forms.Label welcomeLabel;
        internal System.Windows.Forms.PictureBox albumPictureBox;
        internal System.Windows.Forms.Label infoLabel;
        internal System.Windows.Forms.Label albumLabel;
        internal System.Windows.Forms.Label artistLabel;
        internal System.Windows.Forms.TextBox userInfoTextbox;
        internal System.Windows.Forms.Label userInfoLabel;
        internal System.Windows.Forms.Label disclaimerLabel;
        internal Ookii.Dialogs.WinForms.VistaFolderBrowserDialog folderBrowser;
        internal System.Windows.Forms.Label downloadOptionsLabel;
        internal System.Windows.Forms.TextBox downloadFolderTextbox;
        internal System.Windows.Forms.Label downloadFolderLabel;
        internal System.Windows.Forms.Button openFolderButton;
        internal System.Windows.Forms.Button selectFolderButton;
        internal System.Windows.Forms.TextBox downloadOutput;
        internal System.Windows.Forms.TextBox trackTemplateTextbox;
        internal System.Windows.Forms.Label trackTemplateLabel;
        internal System.Windows.Forms.Label templatesLabel;
        internal System.Windows.Forms.TextBox playlistTemplateTextbox;
        internal System.Windows.Forms.TextBox albumTemplateTextbox;
        internal System.Windows.Forms.Label playlistTemplateLabel;
        internal System.Windows.Forms.Label albumTemplateLabel;
        internal System.Windows.Forms.Button resetTemplatesButton;
        internal System.Windows.Forms.Button saveTemplatesButton;
        internal System.Windows.Forms.TextBox favoritesTemplateTextbox;
        internal System.Windows.Forms.Label favoritesTemplateLabel;
        internal System.Windows.Forms.TextBox artistTemplateTextbox;
        internal System.Windows.Forms.Label artistTemplateLabel;
        internal System.Windows.Forms.Panel extraSettingsPanel;
        internal System.Windows.Forms.Label extraSettingsLabel;
        internal System.Windows.Forms.CheckBox albumArtistCheckbox;
        internal System.Windows.Forms.CheckBox explicitCheckbox;
        internal System.Windows.Forms.CheckBox coverArtCheckbox;
        internal System.Windows.Forms.CheckBox discTotalCheckbox;
        internal System.Windows.Forms.CheckBox discNumberCheckbox;
        internal System.Windows.Forms.CheckBox trackNumberCheckbox;
        internal System.Windows.Forms.CheckBox trackTotalCheckbox;
        internal System.Windows.Forms.CheckBox isrcCheckbox;
        internal System.Windows.Forms.CheckBox urlCheckbox;
        internal System.Windows.Forms.CheckBox mergeArtistNamesCheckbox;
        internal System.Windows.Forms.CheckBox upcCheckbox;
        internal System.Windows.Forms.CheckBox labelCheckbox;
        internal System.Windows.Forms.CheckBox copyrightCheckbox;
        internal System.Windows.Forms.CheckBox composerCheckbox;
        internal System.Windows.Forms.CheckBox genreCheckbox;
        internal System.Windows.Forms.CheckBox releaseDateCheckbox;
        internal System.Windows.Forms.CheckBox releaseTypeCheckbox;
        internal System.Windows.Forms.CheckBox trackArtistCheckbox;
        internal System.Windows.Forms.CheckBox trackTitleCheckbox;
        internal System.Windows.Forms.CheckBox albumTitleCheckbox;
        internal System.Windows.Forms.ComboBox embeddedArtSizeSelect;
        internal System.Windows.Forms.Label taggingOptionsLabel;
        internal System.Windows.Forms.Label embeddedArtLabel;
        internal System.Windows.Forms.Label savedArtLabel;
        internal System.Windows.Forms.ComboBox savedArtSizeSelect;
        internal System.Windows.Forms.Button closeAdditionalButton;
        internal System.Windows.Forms.Button additionalSettingsButton;
        internal System.Windows.Forms.Button qualitySelectButton;
        internal System.Windows.Forms.Panel qualitySelectPanel;
        internal System.Windows.Forms.RadioButton mp3Button2;
        internal System.Windows.Forms.RadioButton flacLowButton2;
        internal System.Windows.Forms.RadioButton flacMidButton2;
        internal System.Windows.Forms.RadioButton flacHighButton2;
        internal System.Windows.Forms.Label mp3Label2;
        internal System.Windows.Forms.Label flacLowLabel2;
        internal System.Windows.Forms.Label flacMidLabel2;
        internal System.Windows.Forms.Label flacHighLabel2;
        internal System.Windows.Forms.Label advancedOptionsLabel;
        internal System.Windows.Forms.CheckBox streamableCheckbox;
        internal System.Windows.Forms.CheckBox downloadGoodiesCheckbox;
        internal System.Windows.Forms.CheckBox downloadArtistOtherCheckbox;
        internal System.Windows.Forms.CheckBox useTLS13Checkbox;
        internal System.Windows.Forms.TextBox templatesListTextbox;
        internal System.Windows.Forms.Label templatesListLabel;
        internal System.Windows.Forms.CheckBox fixMD5sCheckbox;
        internal System.Windows.Forms.Label movingLabel;
        internal System.Windows.Forms.Button searchButton;
        internal System.Windows.Forms.Panel searchPanel;
        internal System.Windows.Forms.Label searchLabel;
        internal System.Windows.Forms.Button searchTracksButton;
        internal System.Windows.Forms.TextBox searchTextbox;
        internal System.Windows.Forms.Button searchAlbumsButton;
        internal System.Windows.Forms.Panel searchResultsPanel;
        internal System.Windows.Forms.TableLayoutPanel searchResultsTablePanel;
        internal System.Windows.Forms.Label searchingLabel;
        internal System.Windows.Forms.TextBox inputTextbox;
        internal System.Windows.Forms.Button downloadButton;
        internal System.Windows.Forms.CheckBox downloadSpeedCheckbox;
        internal System.Windows.Forms.Label progressLabel;
        internal System.Windows.Forms.CheckBox commentCheckbox;
        internal System.Windows.Forms.TextBox commentTextbox;
        internal System.Windows.Forms.Label themeSectionLabel;
        internal System.Windows.Forms.Label themeLabel;
        internal System.Windows.Forms.ComboBox themeComboBox;
        internal System.Windows.Forms.Label languageLabel;
        internal System.Windows.Forms.ComboBox languageComboBox;
        internal System.Windows.Forms.FlowLayoutPanel taggingOptionsPanel;
        internal System.Windows.Forms.Label commentLabel;
        internal System.Windows.Forms.FlowLayoutPanel folderButtonsPanel;
        internal System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        internal CustomProgressBar progressBarDownload;
        internal System.Windows.Forms.Button abortButton;
        internal System.Windows.Forms.Button skipButton;
        internal System.Windows.Forms.Button batchDownloadButton;
        internal System.Windows.Forms.Panel batchDownloadPanel;
        internal System.Windows.Forms.Button getAllBatchDownloadButton;
        internal System.Windows.Forms.Button closeBatchDownloadbutton;
        internal System.Windows.Forms.TextBox batchDownloadTextBox;
        internal System.Windows.Forms.Label batchDownloadLabel;
        internal System.Windows.Forms.Label progressItemsCountLabel;
        internal System.Windows.Forms.NumericUpDown limitSearchResultsNumericUpDown;
        internal System.Windows.Forms.Label limitSearchResultsLabel;
        internal System.Windows.Forms.Panel searchSortingPanel;
        internal System.Windows.Forms.Label searchSortingLabel;
        internal System.Windows.Forms.RadioButton sortAlbumTrackNameButton;
        internal System.Windows.Forms.RadioButton sortArtistNameButton;
        internal System.Windows.Forms.RadioButton sortReleaseDateButton;
        internal System.Windows.Forms.Label sortAlbumTrackNameLabel;
        internal System.Windows.Forms.Label sortArtistNameLabel;
        internal System.Windows.Forms.Label sortReleaseDateLabel;
        internal System.Windows.Forms.CheckBox sortAscendantCheckBox;
        internal System.Windows.Forms.Label sortingSearchResultsLabel;
        internal System.Windows.Forms.Label searchResultsCountLabel;
        internal System.Windows.Forms.Label batchDownloadProgressCountLabel;
        internal System.Windows.Forms.NotifyIcon notifyIcon1;
        internal System.Windows.Forms.ContextMenuStrip sysTrayContextMenuStrip;
        internal System.Windows.Forms.ToolStripMenuItem showWindowToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem hideWindowToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem closeProgramToolStripMenuItem;
        internal System.Windows.Forms.Label sortGenreLabel;
        internal System.Windows.Forms.RadioButton sortGenreButton;
        internal System.Windows.Forms.Label selectedRowsCountLabel;
        internal System.Windows.Forms.Button batchDownloadSelectedRowsButton;
        internal System.Windows.Forms.Button deselectAllRowsButton;
        internal System.Windows.Forms.Button selectAllRowsButton;
        internal System.Windows.Forms.ContextMenuStrip mainContextMenuStrip;
        internal System.Windows.Forms.ToolStripMenuItem copyToClipboardToolStripMenuItem;
        internal ToolStripMenuItem copyThisRowToClipboardToolStripMenuItem;
        internal ToolStripMenuItem copySelectedRowsToClipboardToolStripMenuItem;
        internal ToolStripMenuItem copyAllRowsToClipboardToolStripMenuItem;
        internal FlowLayoutPanel advancedOptionsPanelRight;
        internal FlowLayoutPanel advancedOptionsPanelLeft;
        internal CheckBox clearOldLogsCheckBox;
        internal CheckBox dontSaveArtworkToDiskCheckBox;
    }
}