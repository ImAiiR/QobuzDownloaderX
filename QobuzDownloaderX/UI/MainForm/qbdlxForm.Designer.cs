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
            this.gitHubLinkLabel = new System.Windows.Forms.LinkLabel();
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
            this.inputTextBox = new System.Windows.Forms.TextBox();
            this.downloadLabel = new System.Windows.Forms.Label();
            this.settingsPanel = new System.Windows.Forms.Panel();
            this.vaTrackTemplateTextBox = new System.Windows.Forms.TextBox();
            this.vaTrackTemplateLabel = new System.Windows.Forms.Label();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.resetTemplatesButton = new System.Windows.Forms.Button();
            this.saveTemplatesButton = new System.Windows.Forms.Button();
            this.folderButtonsPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.selectFolderButton = new System.Windows.Forms.Button();
            this.openFolderButton = new System.Windows.Forms.Button();
            this.templatesListLabel = new System.Windows.Forms.Label();
            this.templatesListTextBox = new System.Windows.Forms.TextBox();
            this.additionalSettingsButton = new System.Windows.Forms.Button();
            this.trackTemplateTextBox = new System.Windows.Forms.TextBox();
            this.trackTemplateLabel = new System.Windows.Forms.Label();
            this.playlistTemplateTextBox = new System.Windows.Forms.TextBox();
            this.playlistTemplateLabel = new System.Windows.Forms.Label();
            this.artistTemplateTextBox = new System.Windows.Forms.TextBox();
            this.favoritesTemplateTextBox = new System.Windows.Forms.TextBox();
            this.albumTemplateTextBox = new System.Windows.Forms.TextBox();
            this.downloadFolderTextBox = new System.Windows.Forms.TextBox();
            this.artistTemplateLabel = new System.Windows.Forms.Label();
            this.favoritesTemplateLabel = new System.Windows.Forms.Label();
            this.albumTemplateLabel = new System.Windows.Forms.Label();
            this.downloadFolderLabel = new System.Windows.Forms.Label();
            this.downloadOptionsLabel = new System.Windows.Forms.Label();
            this.templatesLabel = new System.Windows.Forms.Label();
            this.settingsLabel = new System.Windows.Forms.Label();
            this.userInfoTextBox = new System.Windows.Forms.RichTextBox();
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
            this.downloadFromArtistListBox = new System.Windows.Forms.CheckedListBox();
            this.playlistSectionLabel = new System.Windows.Forms.Label();
            this.advancedOptionsPanelLeft = new System.Windows.Forms.FlowLayoutPanel();
            this.useTLS13CheckBox = new System.Windows.Forms.CheckBox();
            this.streamableCheckBox = new System.Windows.Forms.CheckBox();
            this.downloadGoodiesCheckBox = new System.Windows.Forms.CheckBox();
            this.downloadSpeedCheckBox = new System.Windows.Forms.CheckBox();
            this.clearOldLogsCheckBox = new System.Windows.Forms.CheckBox();
            this.logFailedDownloadsCheckBox = new System.Windows.Forms.CheckBox();
            this.fixMD5sCheckBox = new System.Windows.Forms.CheckBox();
            this.advancedOptionsPanelRight = new System.Windows.Forms.FlowLayoutPanel();
            this.mergeArtistNamesCheckBox = new System.Windows.Forms.CheckBox();
            this.artistNamesSeparatorsPanel = new System.Windows.Forms.Panel();
            this.primaryListSeparatorLabel = new System.Windows.Forms.Label();
            this.primaryListSeparatorTextBox = new System.Windows.Forms.TextBox();
            this.listEndSeparatorLabel = new System.Windows.Forms.Label();
            this.listEndSeparatorTextBox = new System.Windows.Forms.TextBox();
            this.duplicateFilesFlowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.duplicateFilesLabel = new System.Windows.Forms.Label();
            this.skipDuplicatesRadioButton = new System.Windows.Forms.RadioButton();
            this.autoRenameDuplicatesRadioButton = new System.Windows.Forms.RadioButton();
            this.overwriteDuplicatesRadioButton = new System.Windows.Forms.RadioButton();
            this.showTipsCheckBox = new System.Windows.Forms.CheckBox();
            this.dontSaveArtworkToDiskCheckBox = new System.Windows.Forms.CheckBox();
            this.useItemPosInPlaylistCheckBox = new System.Windows.Forms.CheckBox();
            this.taggingOptionsPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.albumArtistCheckBox = new System.Windows.Forms.CheckBox();
            this.albumTitleCheckBox = new System.Windows.Forms.CheckBox();
            this.trackArtistCheckBox = new System.Windows.Forms.CheckBox();
            this.trackTitleCheckBox = new System.Windows.Forms.CheckBox();
            this.releaseDateCheckBox = new System.Windows.Forms.CheckBox();
            this.releaseTypeCheckBox = new System.Windows.Forms.CheckBox();
            this.genreCheckBox = new System.Windows.Forms.CheckBox();
            this.trackNumberCheckBox = new System.Windows.Forms.CheckBox();
            this.trackTotalCheckBox = new System.Windows.Forms.CheckBox();
            this.discNumberCheckBox = new System.Windows.Forms.CheckBox();
            this.discTotalCheckBox = new System.Windows.Forms.CheckBox();
            this.composerCheckBox = new System.Windows.Forms.CheckBox();
            this.explicitCheckBox = new System.Windows.Forms.CheckBox();
            this.coverArtCheckBox = new System.Windows.Forms.CheckBox();
            this.copyrightCheckBox = new System.Windows.Forms.CheckBox();
            this.labelCheckBox = new System.Windows.Forms.CheckBox();
            this.upcCheckBox = new System.Windows.Forms.CheckBox();
            this.isrcCheckBox = new System.Windows.Forms.CheckBox();
            this.urlCheckBox = new System.Windows.Forms.CheckBox();
            this.languageLabel = new System.Windows.Forms.Label();
            this.languageComboBox = new System.Windows.Forms.ComboBox();
            this.downloadFromArtistLabel = new System.Windows.Forms.Label();
            this.downloadAllFromArtistCheckBox = new System.Windows.Forms.CheckBox();
            this.themeLabel = new System.Windows.Forms.Label();
            this.themeComboBox = new System.Windows.Forms.ComboBox();
            this.themeSectionLabel = new System.Windows.Forms.Label();
            this.commentCheckBox = new System.Windows.Forms.CheckBox();
            this.commentTextBox = new System.Windows.Forms.TextBox();
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
            this.limitSearchResultsNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.searchSortingPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.searchSortingLabel = new System.Windows.Forms.Label();
            this.sortReleaseDateButton = new System.Windows.Forms.RadioButton();
            this.sortArtistNameButton = new System.Windows.Forms.RadioButton();
            this.sortAlbumTrackNameButton = new System.Windows.Forms.RadioButton();
            this.sortGenreButton = new System.Windows.Forms.RadioButton();
            this.sortAscendantCheckBox = new System.Windows.Forms.CheckBox();
            this.deselectAllRowsButton = new System.Windows.Forms.Button();
            this.selectAllRowsButton = new System.Windows.Forms.Button();
            this.batchDownloadSelectedRowsButton = new System.Windows.Forms.Button();
            this.selectedRowsCountLabel = new System.Windows.Forms.Label();
            this.searchResultsCountLabel = new System.Windows.Forms.Label();
            this.searchResultsPanel = new System.Windows.Forms.Panel();
            this.searchResultsTablePanel = new System.Windows.Forms.TableLayoutPanel();
            this.searchAlbumsButton = new System.Windows.Forms.Button();
            this.searchTracksButton = new System.Windows.Forms.Button();
            this.searchTextBox = new System.Windows.Forms.TextBox();
            this.searchLabel = new System.Windows.Forms.Label();
            this.searchingLabel = new System.Windows.Forms.Label();
            this.sortingSearchResultsLabel = new System.Windows.Forms.Label();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.sysTrayContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.showWindowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hideWindowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeProgramToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.prevTipButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.nextTipButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.tipEmojiLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.tipLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.timerTip = new System.Windows.Forms.Timer(this.components);
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
            this.advancedOptionsPanelLeft.SuspendLayout();
            this.advancedOptionsPanelRight.SuspendLayout();
            this.artistNamesSeparatorsPanel.SuspendLayout();
            this.duplicateFilesFlowLayoutPanel.SuspendLayout();
            this.taggingOptionsPanel.SuspendLayout();
            this.qualitySelectPanel.SuspendLayout();
            this.searchPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.limitSearchResultsNumericUpDown)).BeginInit();
            this.searchSortingPanel.SuspendLayout();
            this.searchResultsPanel.SuspendLayout();
            this.sysTrayContextMenuStrip.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // navigationPanel
            // 
            this.navigationPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(40)))), ((int)(((byte)(45)))));
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
            this.navigationPanel.Size = new System.Drawing.Size(180, 580);
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
            this.searchButton.TabIndex = 1;
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
            this.welcomeLabel.TabIndex = 3;
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
            this.settingsButton.TabIndex = 4;
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
            this.logoutButton.TabIndex = 5;
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
            this.aboutButton.TabIndex = 2;
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
            this.downloaderButton.TabIndex = 0;
            this.downloaderButton.Text = "DOWNLOADER";
            this.downloaderButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.downloaderButton.UseVisualStyleBackColor = true;
            this.downloaderButton.Click += new System.EventHandler(this.downloaderButton_Click);
            // 
            // logoPanel
            // 
            this.logoPanel.Controls.Add(this.gitHubLinkLabel);
            this.logoPanel.Controls.Add(this.versionNumber);
            this.logoPanel.Controls.Add(this.logoPictureBox);
            this.logoPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.logoPanel.Location = new System.Drawing.Point(0, 0);
            this.logoPanel.Name = "logoPanel";
            this.logoPanel.Size = new System.Drawing.Size(180, 100);
            this.logoPanel.TabIndex = 6;
            this.logoPanel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.logoPanel_MouseMove);
            // 
            // gitHubLinkLabel
            // 
            this.gitHubLinkLabel.AutoSize = true;
            this.gitHubLinkLabel.Font = new System.Drawing.Font("Nirmala UI", 9.25F);
            this.gitHubLinkLabel.Location = new System.Drawing.Point(5, 79);
            this.gitHubLinkLabel.Name = "gitHubLinkLabel";
            this.gitHubLinkLabel.Size = new System.Drawing.Size(48, 17);
            this.gitHubLinkLabel.TabIndex = 0;
            this.gitHubLinkLabel.TabStop = true;
            this.gitHubLinkLabel.Text = "GitHub";
            this.gitHubLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.gitHubLinkLabel_LinkClicked);
            // 
            // versionNumber
            // 
            this.versionNumber.BackColor = System.Drawing.Color.Transparent;
            this.versionNumber.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.versionNumber.Location = new System.Drawing.Point(119, 79);
            this.versionNumber.Name = "versionNumber";
            this.versionNumber.Size = new System.Drawing.Size(58, 18);
            this.versionNumber.TabIndex = 1;
            this.versionNumber.Text = "#.#.#.#";
            this.versionNumber.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // logoPictureBox
            // 
            this.logoPictureBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(52)))), ((int)(((byte)(62)))));
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
            this.downloaderPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(47)))), ((int)(((byte)(52)))));
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
            this.downloaderPanel.Controls.Add(this.inputTextBox);
            this.downloaderPanel.Controls.Add(this.downloadLabel);
            this.downloaderPanel.Location = new System.Drawing.Point(184, 274);
            this.downloaderPanel.Name = "downloaderPanel";
            this.downloaderPanel.Size = new System.Drawing.Size(771, 577);
            this.downloaderPanel.TabIndex = 0;
            // 
            // batchDownloadProgressCountLabel
            // 
            this.batchDownloadProgressCountLabel.AutoSize = true;
            this.batchDownloadProgressCountLabel.Font = new System.Drawing.Font("Nirmala UI Semilight", 12F);
            this.batchDownloadProgressCountLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.batchDownloadProgressCountLabel.Location = new System.Drawing.Point(185, 143);
            this.batchDownloadProgressCountLabel.Name = "batchDownloadProgressCountLabel";
            this.batchDownloadProgressCountLabel.Size = new System.Drawing.Size(21, 21);
            this.batchDownloadProgressCountLabel.TabIndex = 8;
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
            this.progressItemsCountLabel.TabIndex = 7;
            this.progressItemsCountLabel.Text = "…";
            this.progressItemsCountLabel.Visible = false;
            // 
            // batchDownloadPanel
            // 
            this.batchDownloadPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(40)))), ((int)(((byte)(45)))));
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
            this.batchDownloadButton.Location = new System.Drawing.Point(638, 46);
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
            this.skipButton.Location = new System.Drawing.Point(638, 79);
            this.skipButton.Name = "skipButton";
            this.skipButton.Size = new System.Drawing.Size(110, 31);
            this.skipButton.TabIndex = 5;
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
            this.abortButton.Location = new System.Drawing.Point(522, 79);
            this.abortButton.Name = "abortButton";
            this.abortButton.Size = new System.Drawing.Size(110, 31);
            this.abortButton.TabIndex = 4;
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
            this.progressLabel.TabIndex = 13;
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
            this.downloadButton.Location = new System.Drawing.Point(522, 46);
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
            this.infoLabel.TabIndex = 11;
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
            this.albumLabel.TabIndex = 9;
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
            this.artistLabel.TabIndex = 10;
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
            this.downloadOutput.Size = new System.Drawing.Size(733, 295);
            this.downloadOutput.TabIndex = 12;
            this.downloadOutput.Text = "Test String";
            this.downloadOutput.TextChanged += new System.EventHandler(this.downloadOutput_TextChanged);
            // 
            // albumPictureBox
            // 
            this.albumPictureBox.Cursor = System.Windows.Forms.Cursors.Default;
            this.albumPictureBox.Image = global::QobuzDownloaderX.Properties.Resources.QBDLX_PictureBox;
            this.albumPictureBox.Location = new System.Drawing.Point(18, 83);
            this.albumPictureBox.Name = "albumPictureBox";
            this.albumPictureBox.Size = new System.Drawing.Size(160, 160);
            this.albumPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.albumPictureBox.TabIndex = 2;
            this.albumPictureBox.TabStop = false;
            this.albumPictureBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.albumPictureBox_MouseClick);
            // 
            // inputTextBox
            // 
            this.inputTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.inputTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.inputTextBox.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.inputTextBox.Font = new System.Drawing.Font("Nirmala UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.inputTextBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.inputTextBox.Location = new System.Drawing.Point(18, 46);
            this.inputTextBox.MaxLength = 1000;
            this.inputTextBox.Multiline = true;
            this.inputTextBox.Name = "inputTextBox";
            this.inputTextBox.Size = new System.Drawing.Size(498, 31);
            this.inputTextBox.TabIndex = 1;
            this.inputTextBox.Text = "Paste a Qobuz URL…";
            this.inputTextBox.WordWrap = false;
            this.inputTextBox.Click += new System.EventHandler(this.inputTextBox_Click);
            this.inputTextBox.TextChanged += new System.EventHandler(this.inputTextBox_TextChanged);
            this.inputTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.inputTextBox_KeyDown);
            this.inputTextBox.Leave += new System.EventHandler(this.inputTextBox_Leave);
            this.inputTextBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.inputTextBox_MouseDown);
            // 
            // downloadLabel
            // 
            this.downloadLabel.Font = new System.Drawing.Font("Nirmala UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.downloadLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.downloadLabel.Location = new System.Drawing.Point(13, 10);
            this.downloadLabel.Name = "downloadLabel";
            this.downloadLabel.Size = new System.Drawing.Size(503, 25);
            this.downloadLabel.TabIndex = 0;
            this.downloadLabel.Text = "DOWNLOADER";
            this.downloadLabel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.downloadLabel_MouseMove);
            // 
            // settingsPanel
            // 
            this.settingsPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(47)))), ((int)(((byte)(52)))));
            this.settingsPanel.Controls.Add(this.vaTrackTemplateTextBox);
            this.settingsPanel.Controls.Add(this.vaTrackTemplateLabel);
            this.settingsPanel.Controls.Add(this.flowLayoutPanel1);
            this.settingsPanel.Controls.Add(this.folderButtonsPanel);
            this.settingsPanel.Controls.Add(this.templatesListLabel);
            this.settingsPanel.Controls.Add(this.templatesListTextBox);
            this.settingsPanel.Controls.Add(this.additionalSettingsButton);
            this.settingsPanel.Controls.Add(this.trackTemplateTextBox);
            this.settingsPanel.Controls.Add(this.trackTemplateLabel);
            this.settingsPanel.Controls.Add(this.playlistTemplateTextBox);
            this.settingsPanel.Controls.Add(this.playlistTemplateLabel);
            this.settingsPanel.Controls.Add(this.artistTemplateTextBox);
            this.settingsPanel.Controls.Add(this.favoritesTemplateTextBox);
            this.settingsPanel.Controls.Add(this.albumTemplateTextBox);
            this.settingsPanel.Controls.Add(this.downloadFolderTextBox);
            this.settingsPanel.Controls.Add(this.artistTemplateLabel);
            this.settingsPanel.Controls.Add(this.favoritesTemplateLabel);
            this.settingsPanel.Controls.Add(this.albumTemplateLabel);
            this.settingsPanel.Controls.Add(this.downloadFolderLabel);
            this.settingsPanel.Controls.Add(this.downloadOptionsLabel);
            this.settingsPanel.Controls.Add(this.templatesLabel);
            this.settingsPanel.Controls.Add(this.settingsLabel);
            this.settingsPanel.Location = new System.Drawing.Point(459, 193);
            this.settingsPanel.Name = "settingsPanel";
            this.settingsPanel.Size = new System.Drawing.Size(771, 577);
            this.settingsPanel.TabIndex = 0;
            // 
            // vaTrackTemplateTextBox
            // 
            this.vaTrackTemplateTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.vaTrackTemplateTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.vaTrackTemplateTextBox.Font = new System.Drawing.Font("Nirmala UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.vaTrackTemplateTextBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.vaTrackTemplateTextBox.Location = new System.Drawing.Point(261, 282);
            this.vaTrackTemplateTextBox.Multiline = true;
            this.vaTrackTemplateTextBox.Name = "vaTrackTemplateTextBox";
            this.vaTrackTemplateTextBox.Size = new System.Drawing.Size(430, 21);
            this.vaTrackTemplateTextBox.TabIndex = 13;
            this.vaTrackTemplateTextBox.Text = "%TrackNumber%. %ArtistName% - %TrackTitle%";
            this.vaTrackTemplateTextBox.WordWrap = false;
            // 
            // vaTrackTemplateLabel
            // 
            this.vaTrackTemplateLabel.Font = new System.Drawing.Font("Nirmala UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.vaTrackTemplateLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.vaTrackTemplateLabel.Location = new System.Drawing.Point(4, 279);
            this.vaTrackTemplateLabel.Name = "vaTrackTemplateLabel";
            this.vaTrackTemplateLabel.Size = new System.Drawing.Size(251, 25);
            this.vaTrackTemplateLabel.TabIndex = 12;
            this.vaTrackTemplateLabel.Text = "(V/A) TRACK TEMPLATE";
            this.vaTrackTemplateLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.resetTemplatesButton);
            this.flowLayoutPanel1.Controls.Add(this.saveTemplatesButton);
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(261, 374);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(430, 37);
            this.flowLayoutPanel1.TabIndex = 18;
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
            this.resetTemplatesButton.Location = new System.Drawing.Point(343, 3);
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
            this.saveTemplatesButton.Location = new System.Drawing.Point(253, 3);
            this.saveTemplatesButton.Name = "saveTemplatesButton";
            this.saveTemplatesButton.Size = new System.Drawing.Size(84, 31);
            this.saveTemplatesButton.TabIndex = 0;
            this.saveTemplatesButton.Text = "Save";
            this.saveTemplatesButton.UseVisualStyleBackColor = false;
            this.saveTemplatesButton.Click += new System.EventHandler(this.saveTemplatesButton_Click);
            // 
            // folderButtonsPanel
            // 
            this.folderButtonsPanel.Controls.Add(this.selectFolderButton);
            this.folderButtonsPanel.Controls.Add(this.openFolderButton);
            this.folderButtonsPanel.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.folderButtonsPanel.Location = new System.Drawing.Point(261, 101);
            this.folderButtonsPanel.Name = "folderButtonsPanel";
            this.folderButtonsPanel.Size = new System.Drawing.Size(430, 37);
            this.folderButtonsPanel.TabIndex = 4;
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
            this.selectFolderButton.Location = new System.Drawing.Point(343, 3);
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
            this.openFolderButton.Location = new System.Drawing.Point(255, 3);
            this.openFolderButton.Name = "openFolderButton";
            this.openFolderButton.Size = new System.Drawing.Size(82, 31);
            this.openFolderButton.TabIndex = 0;
            this.openFolderButton.Text = "Open Folder";
            this.openFolderButton.UseVisualStyleBackColor = false;
            this.openFolderButton.Click += new System.EventHandler(this.openFolderButton_Click);
            this.openFolderButton.KeyDown += new System.Windows.Forms.KeyEventHandler(this.downloadFolderTextBox_KeyDown);
            this.openFolderButton.KeyUp += new System.Windows.Forms.KeyEventHandler(this.downloadFolderTextBox_KeyUp);
            // 
            // templatesListLabel
            // 
            this.templatesListLabel.Font = new System.Drawing.Font("Nirmala UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.templatesListLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.templatesListLabel.Location = new System.Drawing.Point(0, 413);
            this.templatesListLabel.Name = "templatesListLabel";
            this.templatesListLabel.Size = new System.Drawing.Size(771, 25);
            this.templatesListLabel.TabIndex = 19;
            this.templatesListLabel.Text = "TEMPLATES LIST";
            this.templatesListLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // templatesListTextBox
            // 
            this.templatesListTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.templatesListTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.templatesListTextBox.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.templatesListTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.templatesListTextBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.templatesListTextBox.Location = new System.Drawing.Point(96, 440);
            this.templatesListTextBox.Multiline = true;
            this.templatesListTextBox.Name = "templatesListTextBox";
            this.templatesListTextBox.ReadOnly = true;
            this.templatesListTextBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.templatesListTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.templatesListTextBox.Size = new System.Drawing.Size(595, 77);
            this.templatesListTextBox.TabIndex = 20;
            this.templatesListTextBox.Text = resources.GetString("templatesListTextBox.Text");
            this.templatesListTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // additionalSettingsButton
            // 
            this.additionalSettingsButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(13)))), ((int)(((byte)(13)))), ((int)(((byte)(13)))));
            this.additionalSettingsButton.FlatAppearance.BorderSize = 0;
            this.additionalSettingsButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
            this.additionalSettingsButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(18)))), ((int)(((byte)(18)))));
            this.additionalSettingsButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.additionalSettingsButton.Font = new System.Drawing.Font("Nirmala UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.additionalSettingsButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.additionalSettingsButton.Location = new System.Drawing.Point(541, 533);
            this.additionalSettingsButton.Name = "additionalSettingsButton";
            this.additionalSettingsButton.Size = new System.Drawing.Size(150, 31);
            this.additionalSettingsButton.TabIndex = 21;
            this.additionalSettingsButton.Text = "Additional Settings";
            this.additionalSettingsButton.UseVisualStyleBackColor = false;
            this.additionalSettingsButton.Click += new System.EventHandler(this.additionalSettingsButton_Click);
            // 
            // trackTemplateTextBox
            // 
            this.trackTemplateTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.trackTemplateTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.trackTemplateTextBox.Font = new System.Drawing.Font("Nirmala UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.trackTemplateTextBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.trackTemplateTextBox.Location = new System.Drawing.Point(261, 249);
            this.trackTemplateTextBox.Multiline = true;
            this.trackTemplateTextBox.Name = "trackTemplateTextBox";
            this.trackTemplateTextBox.Size = new System.Drawing.Size(430, 21);
            this.trackTemplateTextBox.TabIndex = 11;
            this.trackTemplateTextBox.Text = "%TrackNumber%. %ArtistName% - %TrackTitle%";
            this.trackTemplateTextBox.WordWrap = false;
            // 
            // trackTemplateLabel
            // 
            this.trackTemplateLabel.Font = new System.Drawing.Font("Nirmala UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.trackTemplateLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.trackTemplateLabel.Location = new System.Drawing.Point(4, 246);
            this.trackTemplateLabel.Name = "trackTemplateLabel";
            this.trackTemplateLabel.Size = new System.Drawing.Size(251, 25);
            this.trackTemplateLabel.TabIndex = 10;
            this.trackTemplateLabel.Text = "(DEFAULT) TRACK TEMPLATE";
            this.trackTemplateLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // playlistTemplateTextBox
            // 
            this.playlistTemplateTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.playlistTemplateTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.playlistTemplateTextBox.Font = new System.Drawing.Font("Nirmala UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.playlistTemplateTextBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.playlistTemplateTextBox.Location = new System.Drawing.Point(261, 312);
            this.playlistTemplateTextBox.Multiline = true;
            this.playlistTemplateTextBox.Name = "playlistTemplateTextBox";
            this.playlistTemplateTextBox.Size = new System.Drawing.Size(430, 21);
            this.playlistTemplateTextBox.TabIndex = 15;
            this.playlistTemplateTextBox.Text = "%PlaylistTitle% [ID%PlaylistID%]\\%ArtistName%";
            this.playlistTemplateTextBox.WordWrap = false;
            // 
            // playlistTemplateLabel
            // 
            this.playlistTemplateLabel.Font = new System.Drawing.Font("Nirmala UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.playlistTemplateLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.playlistTemplateLabel.Location = new System.Drawing.Point(4, 309);
            this.playlistTemplateLabel.Name = "playlistTemplateLabel";
            this.playlistTemplateLabel.Size = new System.Drawing.Size(251, 25);
            this.playlistTemplateLabel.TabIndex = 14;
            this.playlistTemplateLabel.Text = "PLAYLIST TEMPLATE";
            this.playlistTemplateLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // artistTemplateTextBox
            // 
            this.artistTemplateTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.artistTemplateTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.artistTemplateTextBox.Font = new System.Drawing.Font("Nirmala UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.artistTemplateTextBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.artistTemplateTextBox.Location = new System.Drawing.Point(261, 180);
            this.artistTemplateTextBox.Multiline = true;
            this.artistTemplateTextBox.Name = "artistTemplateTextBox";
            this.artistTemplateTextBox.Size = new System.Drawing.Size(430, 21);
            this.artistTemplateTextBox.TabIndex = 7;
            this.artistTemplateTextBox.Text = "%ArtistName%";
            this.artistTemplateTextBox.WordWrap = false;
            // 
            // favoritesTemplateTextBox
            // 
            this.favoritesTemplateTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.favoritesTemplateTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.favoritesTemplateTextBox.Font = new System.Drawing.Font("Nirmala UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.favoritesTemplateTextBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.favoritesTemplateTextBox.Location = new System.Drawing.Point(261, 347);
            this.favoritesTemplateTextBox.Multiline = true;
            this.favoritesTemplateTextBox.Name = "favoritesTemplateTextBox";
            this.favoritesTemplateTextBox.Size = new System.Drawing.Size(430, 21);
            this.favoritesTemplateTextBox.TabIndex = 17;
            this.favoritesTemplateTextBox.Text = "- Favorites";
            this.favoritesTemplateTextBox.WordWrap = false;
            // 
            // albumTemplateTextBox
            // 
            this.albumTemplateTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.albumTemplateTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.albumTemplateTextBox.Font = new System.Drawing.Font("Nirmala UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.albumTemplateTextBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.albumTemplateTextBox.Location = new System.Drawing.Point(261, 214);
            this.albumTemplateTextBox.Multiline = true;
            this.albumTemplateTextBox.Name = "albumTemplateTextBox";
            this.albumTemplateTextBox.Size = new System.Drawing.Size(430, 21);
            this.albumTemplateTextBox.TabIndex = 9;
            this.albumTemplateTextBox.Text = "%AlbumTitle% (%Year%) (%AlbumPA%) [UPC%UPC%]";
            this.albumTemplateTextBox.WordWrap = false;
            // 
            // downloadFolderTextBox
            // 
            this.downloadFolderTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.downloadFolderTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.downloadFolderTextBox.Font = new System.Drawing.Font("Nirmala UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.downloadFolderTextBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.downloadFolderTextBox.Location = new System.Drawing.Point(261, 74);
            this.downloadFolderTextBox.Multiline = true;
            this.downloadFolderTextBox.Name = "downloadFolderTextBox";
            this.downloadFolderTextBox.Size = new System.Drawing.Size(430, 21);
            this.downloadFolderTextBox.TabIndex = 3;
            this.downloadFolderTextBox.Text = "no folder selected";
            this.downloadFolderTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.downloadFolderTextBox_KeyDown);
            this.downloadFolderTextBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.downloadFolderTextBox_KeyUp);
            // 
            // artistTemplateLabel
            // 
            this.artistTemplateLabel.Font = new System.Drawing.Font("Nirmala UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.artistTemplateLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.artistTemplateLabel.Location = new System.Drawing.Point(4, 176);
            this.artistTemplateLabel.Name = "artistTemplateLabel";
            this.artistTemplateLabel.Size = new System.Drawing.Size(251, 25);
            this.artistTemplateLabel.TabIndex = 6;
            this.artistTemplateLabel.Text = "ARTIST TEMPLATE";
            this.artistTemplateLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // favoritesTemplateLabel
            // 
            this.favoritesTemplateLabel.Font = new System.Drawing.Font("Nirmala UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.favoritesTemplateLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.favoritesTemplateLabel.Location = new System.Drawing.Point(4, 344);
            this.favoritesTemplateLabel.Name = "favoritesTemplateLabel";
            this.favoritesTemplateLabel.Size = new System.Drawing.Size(251, 25);
            this.favoritesTemplateLabel.TabIndex = 16;
            this.favoritesTemplateLabel.Text = "FAVORITES TEMPLATE";
            this.favoritesTemplateLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // albumTemplateLabel
            // 
            this.albumTemplateLabel.Font = new System.Drawing.Font("Nirmala UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.albumTemplateLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.albumTemplateLabel.Location = new System.Drawing.Point(4, 211);
            this.albumTemplateLabel.Name = "albumTemplateLabel";
            this.albumTemplateLabel.Size = new System.Drawing.Size(251, 25);
            this.albumTemplateLabel.TabIndex = 8;
            this.albumTemplateLabel.Text = "ALBUM TEMPLATE";
            this.albumTemplateLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // downloadFolderLabel
            // 
            this.downloadFolderLabel.Font = new System.Drawing.Font("Nirmala UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.downloadFolderLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.downloadFolderLabel.Location = new System.Drawing.Point(4, 71);
            this.downloadFolderLabel.Name = "downloadFolderLabel";
            this.downloadFolderLabel.Size = new System.Drawing.Size(251, 25);
            this.downloadFolderLabel.TabIndex = 2;
            this.downloadFolderLabel.Text = "DOWNLOAD FOLDER";
            this.downloadFolderLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // downloadOptionsLabel
            // 
            this.downloadOptionsLabel.Font = new System.Drawing.Font("Nirmala UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.downloadOptionsLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.downloadOptionsLabel.Location = new System.Drawing.Point(3, 35);
            this.downloadOptionsLabel.Name = "downloadOptionsLabel";
            this.downloadOptionsLabel.Size = new System.Drawing.Size(768, 25);
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
            this.templatesLabel.TabIndex = 5;
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
            this.settingsLabel.TabIndex = 0;
            this.settingsLabel.Text = "SETTINGS                                                                         " +
    "                   ";
            this.settingsLabel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.settingsLabel_MouseMove);
            // 
            // userInfoTextBox
            // 
            this.userInfoTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(18)))), ((int)(((byte)(18)))));
            this.userInfoTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.userInfoTextBox.ContextMenuStrip = this.mainContextMenuStrip;
            this.userInfoTextBox.Cursor = System.Windows.Forms.Cursors.Help;
            this.userInfoTextBox.Font = new System.Drawing.Font("Nirmala UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.userInfoTextBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.userInfoTextBox.Location = new System.Drawing.Point(18, 78);
            this.userInfoTextBox.Name = "userInfoTextBox";
            this.userInfoTextBox.ReadOnly = true;
            this.userInfoTextBox.Size = new System.Drawing.Size(734, 97);
            this.userInfoTextBox.TabIndex = 2;
            this.userInfoTextBox.Text = "User ID = {user_id}\nE-mail = {user_email}\nCountry = {user_country}\nSubscription =" +
    " {user_subscription}\nExpires = {user_subscription_expiration}\n";
            this.userInfoTextBox.WordWrap = false;
            this.userInfoTextBox.GotFocus += new System.EventHandler(this.userInfoTextBox_GotFocus);
            this.userInfoTextBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.userInfoTextBox_MouseDown);
            this.userInfoTextBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.userInfoTextBox_MouseUp);
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
            this.userInfoLabel.Location = new System.Drawing.Point(3, 35);
            this.userInfoLabel.Name = "userInfoLabel";
            this.userInfoLabel.Size = new System.Drawing.Size(768, 25);
            this.userInfoLabel.TabIndex = 1;
            this.userInfoLabel.Text = "USER INFO";
            this.userInfoLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // disclaimerLabel
            // 
            this.disclaimerLabel.Font = new System.Drawing.Font("Nirmala UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.disclaimerLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.disclaimerLabel.Location = new System.Drawing.Point(18, 196);
            this.disclaimerLabel.Name = "disclaimerLabel";
            this.disclaimerLabel.Size = new System.Drawing.Size(733, 411);
            this.disclaimerLabel.TabIndex = 3;
            this.disclaimerLabel.Text = "DISCLAIMER / LEGAL ADVICE";
            this.disclaimerLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
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
            this.aboutPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(47)))), ((int)(((byte)(52)))));
            this.aboutPanel.Controls.Add(this.userInfoTextBox);
            this.aboutPanel.Controls.Add(this.aboutLabel);
            this.aboutPanel.Controls.Add(this.userInfoLabel);
            this.aboutPanel.Controls.Add(this.disclaimerLabel);
            this.aboutPanel.Location = new System.Drawing.Point(791, 117);
            this.aboutPanel.Name = "aboutPanel";
            this.aboutPanel.Size = new System.Drawing.Size(771, 577);
            this.aboutPanel.TabIndex = 0;
            // 
            // aboutLabel
            // 
            this.aboutLabel.AutoSize = true;
            this.aboutLabel.Font = new System.Drawing.Font("Nirmala UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.aboutLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.aboutLabel.Location = new System.Drawing.Point(13, 10);
            this.aboutLabel.Name = "aboutLabel";
            this.aboutLabel.Size = new System.Drawing.Size(552, 25);
            this.aboutLabel.TabIndex = 0;
            this.aboutLabel.Text = "ABOUT                                                                            " +
    "                    ";
            this.aboutLabel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.aboutLabel_MouseMove);
            // 
            // extraSettingsPanel
            // 
            this.extraSettingsPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(47)))), ((int)(((byte)(52)))));
            this.extraSettingsPanel.Controls.Add(this.downloadFromArtistListBox);
            this.extraSettingsPanel.Controls.Add(this.playlistSectionLabel);
            this.extraSettingsPanel.Controls.Add(this.advancedOptionsPanelLeft);
            this.extraSettingsPanel.Controls.Add(this.showTipsCheckBox);
            this.extraSettingsPanel.Controls.Add(this.dontSaveArtworkToDiskCheckBox);
            this.extraSettingsPanel.Controls.Add(this.useItemPosInPlaylistCheckBox);
            this.extraSettingsPanel.Controls.Add(this.taggingOptionsPanel);
            this.extraSettingsPanel.Controls.Add(this.languageLabel);
            this.extraSettingsPanel.Controls.Add(this.languageComboBox);
            this.extraSettingsPanel.Controls.Add(this.downloadFromArtistLabel);
            this.extraSettingsPanel.Controls.Add(this.downloadAllFromArtistCheckBox);
            this.extraSettingsPanel.Controls.Add(this.themeLabel);
            this.extraSettingsPanel.Controls.Add(this.themeComboBox);
            this.extraSettingsPanel.Controls.Add(this.themeSectionLabel);
            this.extraSettingsPanel.Controls.Add(this.commentCheckBox);
            this.extraSettingsPanel.Controls.Add(this.commentTextBox);
            this.extraSettingsPanel.Controls.Add(this.advancedOptionsLabel);
            this.extraSettingsPanel.Controls.Add(this.closeAdditionalButton);
            this.extraSettingsPanel.Controls.Add(this.savedArtLabel);
            this.extraSettingsPanel.Controls.Add(this.savedArtSizeSelect);
            this.extraSettingsPanel.Controls.Add(this.taggingOptionsLabel);
            this.extraSettingsPanel.Controls.Add(this.embeddedArtLabel);
            this.extraSettingsPanel.Controls.Add(this.embeddedArtSizeSelect);
            this.extraSettingsPanel.Controls.Add(this.extraSettingsLabel);
            this.extraSettingsPanel.Font = new System.Drawing.Font("Nirmala UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.extraSettingsPanel.Location = new System.Drawing.Point(571, 155);
            this.extraSettingsPanel.Name = "extraSettingsPanel";
            this.extraSettingsPanel.Size = new System.Drawing.Size(771, 577);
            this.extraSettingsPanel.TabIndex = 0;
            this.extraSettingsPanel.Visible = false;
            // 
            // downloadFromArtistListBox
            // 
            this.downloadFromArtistListBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(18)))), ((int)(((byte)(18)))));
            this.downloadFromArtistListBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.downloadFromArtistListBox.CheckOnClick = true;
            this.downloadFromArtistListBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.downloadFromArtistListBox.Items.AddRange(new object[] {
            "Album",
            "EP/Single",
            "Live",
            "Compilation",
            "Download",
            "Other"});
            this.downloadFromArtistListBox.Location = new System.Drawing.Point(20, 321);
            this.downloadFromArtistListBox.MultiColumn = true;
            this.downloadFromArtistListBox.Name = "downloadFromArtistListBox";
            this.downloadFromArtistListBox.Size = new System.Drawing.Size(361, 34);
            this.downloadFromArtistListBox.TabIndex = 11;
            this.downloadFromArtistListBox.SelectedIndexChanged += new System.EventHandler(this.downloadFromArtistListBox_SelectedIndexChanged);
            // 
            // playlistSectionLabel
            // 
            this.playlistSectionLabel.Font = new System.Drawing.Font("Nirmala UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.playlistSectionLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.playlistSectionLabel.Location = new System.Drawing.Point(0, 392);
            this.playlistSectionLabel.Name = "playlistSectionLabel";
            this.playlistSectionLabel.Size = new System.Drawing.Size(377, 25);
            this.playlistSectionLabel.TabIndex = 13;
            this.playlistSectionLabel.Text = "PLAYLIST OPTIONS";
            this.playlistSectionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // advancedOptionsPanelLeft
            // 
            this.advancedOptionsPanelLeft.Controls.Add(this.useTLS13CheckBox);
            this.advancedOptionsPanelLeft.Controls.Add(this.streamableCheckBox);
            this.advancedOptionsPanelLeft.Controls.Add(this.downloadGoodiesCheckBox);
            this.advancedOptionsPanelLeft.Controls.Add(this.downloadSpeedCheckBox);
            this.advancedOptionsPanelLeft.Controls.Add(this.clearOldLogsCheckBox);
            this.advancedOptionsPanelLeft.Controls.Add(this.logFailedDownloadsCheckBox);
            this.advancedOptionsPanelLeft.Controls.Add(this.fixMD5sCheckBox);
            this.advancedOptionsPanelLeft.Controls.Add(this.advancedOptionsPanelRight);
            this.advancedOptionsPanelLeft.Controls.Add(this.duplicateFilesFlowLayoutPanel);
            this.advancedOptionsPanelLeft.Location = new System.Drawing.Point(390, 172);
            this.advancedOptionsPanelLeft.Name = "advancedOptionsPanelLeft";
            this.advancedOptionsPanelLeft.Size = new System.Drawing.Size(363, 355);
            this.advancedOptionsPanelLeft.TabIndex = 22;
            // 
            // useTLS13CheckBox
            // 
            this.useTLS13CheckBox.AutoSize = true;
            this.useTLS13CheckBox.Checked = true;
            this.useTLS13CheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.useTLS13CheckBox.Font = new System.Drawing.Font("Nirmala UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.useTLS13CheckBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.useTLS13CheckBox.Location = new System.Drawing.Point(3, 3);
            this.useTLS13CheckBox.Name = "useTLS13CheckBox";
            this.useTLS13CheckBox.Size = new System.Drawing.Size(98, 17);
            this.useTLS13CheckBox.TabIndex = 0;
            this.useTLS13CheckBox.Text = "Enable TLS 1.3";
            this.useTLS13CheckBox.UseVisualStyleBackColor = true;
            this.useTLS13CheckBox.CheckedChanged += new System.EventHandler(this.useTLS13CheckBox_CheckedChanged);
            // 
            // streamableCheckBox
            // 
            this.streamableCheckBox.AutoSize = true;
            this.streamableCheckBox.Checked = true;
            this.streamableCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.streamableCheckBox.Font = new System.Drawing.Font("Nirmala UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.streamableCheckBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.streamableCheckBox.Location = new System.Drawing.Point(107, 3);
            this.streamableCheckBox.Name = "streamableCheckBox";
            this.streamableCheckBox.Size = new System.Drawing.Size(117, 17);
            this.streamableCheckBox.TabIndex = 1;
            this.streamableCheckBox.Text = "Streamable Check";
            this.streamableCheckBox.UseVisualStyleBackColor = true;
            this.streamableCheckBox.CheckedChanged += new System.EventHandler(this.streamableCheckBox_CheckedChanged);
            // 
            // downloadGoodiesCheckBox
            // 
            this.downloadGoodiesCheckBox.AutoSize = true;
            this.downloadGoodiesCheckBox.Checked = true;
            this.downloadGoodiesCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.downloadGoodiesCheckBox.Font = new System.Drawing.Font("Nirmala UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.downloadGoodiesCheckBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.downloadGoodiesCheckBox.Location = new System.Drawing.Point(230, 3);
            this.downloadGoodiesCheckBox.Name = "downloadGoodiesCheckBox";
            this.downloadGoodiesCheckBox.Size = new System.Drawing.Size(126, 17);
            this.downloadGoodiesCheckBox.TabIndex = 2;
            this.downloadGoodiesCheckBox.Text = "Download Goodies";
            this.downloadGoodiesCheckBox.UseVisualStyleBackColor = true;
            this.downloadGoodiesCheckBox.CheckedChanged += new System.EventHandler(this.downloadGoodiesCheckBox_CheckedChanged);
            // 
            // downloadSpeedCheckBox
            // 
            this.downloadSpeedCheckBox.AutoSize = true;
            this.downloadSpeedCheckBox.Font = new System.Drawing.Font("Nirmala UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.downloadSpeedCheckBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.downloadSpeedCheckBox.Location = new System.Drawing.Point(3, 26);
            this.downloadSpeedCheckBox.Name = "downloadSpeedCheckBox";
            this.downloadSpeedCheckBox.Size = new System.Drawing.Size(142, 17);
            this.downloadSpeedCheckBox.TabIndex = 3;
            this.downloadSpeedCheckBox.Text = "Print Download Speed";
            this.downloadSpeedCheckBox.UseVisualStyleBackColor = true;
            this.downloadSpeedCheckBox.CheckedChanged += new System.EventHandler(this.downloadSpeedCheckBox_CheckedChanged);
            // 
            // clearOldLogsCheckBox
            // 
            this.clearOldLogsCheckBox.AutoSize = true;
            this.clearOldLogsCheckBox.Font = new System.Drawing.Font("Nirmala UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.clearOldLogsCheckBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.clearOldLogsCheckBox.Location = new System.Drawing.Point(151, 26);
            this.clearOldLogsCheckBox.Name = "clearOldLogsCheckBox";
            this.clearOldLogsCheckBox.Size = new System.Drawing.Size(154, 17);
            this.clearOldLogsCheckBox.TabIndex = 4;
            this.clearOldLogsCheckBox.Text = "Clear old logs on startup";
            this.clearOldLogsCheckBox.UseVisualStyleBackColor = true;
            this.clearOldLogsCheckBox.CheckedChanged += new System.EventHandler(this.clearOldLogsCheckBox_CheckedChanged);
            // 
            // logFailedDownloadsCheckBox
            // 
            this.logFailedDownloadsCheckBox.Font = new System.Drawing.Font("Nirmala UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.logFailedDownloadsCheckBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.logFailedDownloadsCheckBox.Location = new System.Drawing.Point(3, 49);
            this.logFailedDownloadsCheckBox.Name = "logFailedDownloadsCheckBox";
            this.logFailedDownloadsCheckBox.Size = new System.Drawing.Size(351, 30);
            this.logFailedDownloadsCheckBox.TabIndex = 5;
            this.logFailedDownloadsCheckBox.Text = "Log failed downloads to \'error.txt\' in the download folder";
            this.logFailedDownloadsCheckBox.UseVisualStyleBackColor = true;
            this.logFailedDownloadsCheckBox.CheckedChanged += new System.EventHandler(this.logFailedDownloadsCheckBox_CheckedChanged);
            // 
            // fixMD5sCheckBox
            // 
            this.fixMD5sCheckBox.Checked = true;
            this.fixMD5sCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.fixMD5sCheckBox.Font = new System.Drawing.Font("Nirmala UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fixMD5sCheckBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.fixMD5sCheckBox.Location = new System.Drawing.Point(3, 85);
            this.fixMD5sCheckBox.Name = "fixMD5sCheckBox";
            this.fixMD5sCheckBox.Size = new System.Drawing.Size(353, 30);
            this.fixMD5sCheckBox.TabIndex = 6;
            this.fixMD5sCheckBox.Text = "Auto-Fix Unset MD5s (must have FLAC in PATH variables)";
            this.fixMD5sCheckBox.UseVisualStyleBackColor = true;
            this.fixMD5sCheckBox.CheckedChanged += new System.EventHandler(this.fixMD5sCheckBox_CheckedChanged);
            // 
            // advancedOptionsPanelRight
            // 
            this.advancedOptionsPanelRight.AutoScroll = true;
            this.advancedOptionsPanelRight.Controls.Add(this.mergeArtistNamesCheckBox);
            this.advancedOptionsPanelRight.Controls.Add(this.artistNamesSeparatorsPanel);
            this.advancedOptionsPanelRight.Location = new System.Drawing.Point(0, 118);
            this.advancedOptionsPanelRight.Margin = new System.Windows.Forms.Padding(0);
            this.advancedOptionsPanelRight.Name = "advancedOptionsPanelRight";
            this.advancedOptionsPanelRight.Size = new System.Drawing.Size(355, 96);
            this.advancedOptionsPanelRight.TabIndex = 7;
            // 
            // mergeArtistNamesCheckBox
            // 
            this.mergeArtistNamesCheckBox.AutoSize = true;
            this.mergeArtistNamesCheckBox.Checked = true;
            this.mergeArtistNamesCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.mergeArtistNamesCheckBox.Font = new System.Drawing.Font("Nirmala UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mergeArtistNamesCheckBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.mergeArtistNamesCheckBox.Location = new System.Drawing.Point(3, 3);
            this.mergeArtistNamesCheckBox.Name = "mergeArtistNamesCheckBox";
            this.mergeArtistNamesCheckBox.Size = new System.Drawing.Size(131, 17);
            this.mergeArtistNamesCheckBox.TabIndex = 0;
            this.mergeArtistNamesCheckBox.Text = "Merge Artists Names";
            this.mergeArtistNamesCheckBox.UseVisualStyleBackColor = true;
            this.mergeArtistNamesCheckBox.CheckedChanged += new System.EventHandler(this.mergeArtistNamesCheckBox_CheckedChanged);
            // 
            // artistNamesSeparatorsPanel
            // 
            this.artistNamesSeparatorsPanel.AutoScroll = true;
            this.artistNamesSeparatorsPanel.Controls.Add(this.primaryListSeparatorLabel);
            this.artistNamesSeparatorsPanel.Controls.Add(this.primaryListSeparatorTextBox);
            this.artistNamesSeparatorsPanel.Controls.Add(this.listEndSeparatorLabel);
            this.artistNamesSeparatorsPanel.Controls.Add(this.listEndSeparatorTextBox);
            this.artistNamesSeparatorsPanel.Enabled = false;
            this.artistNamesSeparatorsPanel.Location = new System.Drawing.Point(3, 26);
            this.artistNamesSeparatorsPanel.Name = "artistNamesSeparatorsPanel";
            this.artistNamesSeparatorsPanel.Size = new System.Drawing.Size(218, 54);
            this.artistNamesSeparatorsPanel.TabIndex = 1;
            // 
            // primaryListSeparatorLabel
            // 
            this.primaryListSeparatorLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.primaryListSeparatorLabel.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.primaryListSeparatorLabel.Location = new System.Drawing.Point(3, 9);
            this.primaryListSeparatorLabel.Name = "primaryListSeparatorLabel";
            this.primaryListSeparatorLabel.Size = new System.Drawing.Size(166, 13);
            this.primaryListSeparatorLabel.TabIndex = 0;
            this.primaryListSeparatorLabel.Text = "Primary list separator";
            this.primaryListSeparatorLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // primaryListSeparatorTextBox
            // 
            this.primaryListSeparatorTextBox.Location = new System.Drawing.Point(169, 3);
            this.primaryListSeparatorTextBox.Name = "primaryListSeparatorTextBox";
            this.primaryListSeparatorTextBox.Size = new System.Drawing.Size(45, 22);
            this.primaryListSeparatorTextBox.TabIndex = 1;
            this.primaryListSeparatorTextBox.Text = " & ";
            this.primaryListSeparatorTextBox.Leave += new System.EventHandler(this.primaryListSeparatorTextBox_Leave);
            // 
            // listEndSeparatorLabel
            // 
            this.listEndSeparatorLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.listEndSeparatorLabel.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.listEndSeparatorLabel.Location = new System.Drawing.Point(3, 36);
            this.listEndSeparatorLabel.Name = "listEndSeparatorLabel";
            this.listEndSeparatorLabel.Size = new System.Drawing.Size(166, 13);
            this.listEndSeparatorLabel.TabIndex = 2;
            this.listEndSeparatorLabel.Text = "List end separator";
            this.listEndSeparatorLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // listEndSeparatorTextBox
            // 
            this.listEndSeparatorTextBox.Location = new System.Drawing.Point(169, 29);
            this.listEndSeparatorTextBox.Name = "listEndSeparatorTextBox";
            this.listEndSeparatorTextBox.Size = new System.Drawing.Size(45, 22);
            this.listEndSeparatorTextBox.TabIndex = 3;
            this.listEndSeparatorTextBox.Text = ", ";
            this.listEndSeparatorTextBox.Leave += new System.EventHandler(this.listEndSeparatorTextBox_Leave);
            // 
            // duplicateFilesFlowLayoutPanel
            // 
            this.duplicateFilesFlowLayoutPanel.Controls.Add(this.duplicateFilesLabel);
            this.duplicateFilesFlowLayoutPanel.Controls.Add(this.skipDuplicatesRadioButton);
            this.duplicateFilesFlowLayoutPanel.Controls.Add(this.autoRenameDuplicatesRadioButton);
            this.duplicateFilesFlowLayoutPanel.Controls.Add(this.overwriteDuplicatesRadioButton);
            this.duplicateFilesFlowLayoutPanel.Location = new System.Drawing.Point(0, 216);
            this.duplicateFilesFlowLayoutPanel.Margin = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.duplicateFilesFlowLayoutPanel.Name = "duplicateFilesFlowLayoutPanel";
            this.duplicateFilesFlowLayoutPanel.Size = new System.Drawing.Size(356, 90);
            this.duplicateFilesFlowLayoutPanel.TabIndex = 9;
            // 
            // duplicateFilesLabel
            // 
            this.duplicateFilesLabel.Font = new System.Drawing.Font("Nirmala UI", 12F);
            this.duplicateFilesLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.duplicateFilesLabel.Location = new System.Drawing.Point(3, 0);
            this.duplicateFilesLabel.Name = "duplicateFilesLabel";
            this.duplicateFilesLabel.Size = new System.Drawing.Size(214, 21);
            this.duplicateFilesLabel.TabIndex = 4;
            this.duplicateFilesLabel.Text = "Duplicate files";
            this.duplicateFilesLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // skipDuplicatesRadioButton
            // 
            this.skipDuplicatesRadioButton.Checked = true;
            this.skipDuplicatesRadioButton.Cursor = System.Windows.Forms.Cursors.Default;
            this.skipDuplicatesRadioButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.skipDuplicatesRadioButton.Location = new System.Drawing.Point(3, 24);
            this.skipDuplicatesRadioButton.Name = "skipDuplicatesRadioButton";
            this.skipDuplicatesRadioButton.Size = new System.Drawing.Size(343, 17);
            this.skipDuplicatesRadioButton.TabIndex = 0;
            this.skipDuplicatesRadioButton.TabStop = true;
            this.skipDuplicatesRadioButton.Text = "Ignore / skip download";
            this.skipDuplicatesRadioButton.UseVisualStyleBackColor = true;
            this.skipDuplicatesRadioButton.CheckedChanged += new System.EventHandler(this.skipDuplicatesRadioButton_CheckedChanged);
            // 
            // autoRenameDuplicatesRadioButton
            // 
            this.autoRenameDuplicatesRadioButton.Cursor = System.Windows.Forms.Cursors.Default;
            this.autoRenameDuplicatesRadioButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.autoRenameDuplicatesRadioButton.Location = new System.Drawing.Point(3, 47);
            this.autoRenameDuplicatesRadioButton.Name = "autoRenameDuplicatesRadioButton";
            this.autoRenameDuplicatesRadioButton.Size = new System.Drawing.Size(343, 17);
            this.autoRenameDuplicatesRadioButton.TabIndex = 4;
            this.autoRenameDuplicatesRadioButton.Text = "Automatically rename incoming file";
            this.autoRenameDuplicatesRadioButton.UseVisualStyleBackColor = true;
            this.autoRenameDuplicatesRadioButton.CheckedChanged += new System.EventHandler(this.autoRenameDuplicatesRadioButton_CheckedChanged);
            // 
            // overwriteDuplicatesRadioButton
            // 
            this.overwriteDuplicatesRadioButton.Cursor = System.Windows.Forms.Cursors.Default;
            this.overwriteDuplicatesRadioButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.overwriteDuplicatesRadioButton.Location = new System.Drawing.Point(3, 70);
            this.overwriteDuplicatesRadioButton.Name = "overwriteDuplicatesRadioButton";
            this.overwriteDuplicatesRadioButton.Size = new System.Drawing.Size(343, 17);
            this.overwriteDuplicatesRadioButton.TabIndex = 6;
            this.overwriteDuplicatesRadioButton.Text = "Overwrite existing file";
            this.overwriteDuplicatesRadioButton.UseVisualStyleBackColor = true;
            this.overwriteDuplicatesRadioButton.CheckedChanged += new System.EventHandler(this.overwriteDuplicatesRadioButton_CheckedChanged);
            // 
            // showTipsCheckBox
            // 
            this.showTipsCheckBox.AutoSize = true;
            this.showTipsCheckBox.Checked = true;
            this.showTipsCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showTipsCheckBox.Font = new System.Drawing.Font("Nirmala UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.showTipsCheckBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.showTipsCheckBox.Location = new System.Drawing.Point(23, 547);
            this.showTipsCheckBox.Name = "showTipsCheckBox";
            this.showTipsCheckBox.Size = new System.Drawing.Size(78, 17);
            this.showTipsCheckBox.TabIndex = 20;
            this.showTipsCheckBox.Text = "Show Tips";
            this.showTipsCheckBox.UseVisualStyleBackColor = true;
            this.showTipsCheckBox.CheckedChanged += new System.EventHandler(this.showTipsCheckBox_CheckedChanged);
            // 
            // dontSaveArtworkToDiskCheckBox
            // 
            this.dontSaveArtworkToDiskCheckBox.AutoSize = true;
            this.dontSaveArtworkToDiskCheckBox.Font = new System.Drawing.Font("Nirmala UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dontSaveArtworkToDiskCheckBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.dontSaveArtworkToDiskCheckBox.Location = new System.Drawing.Point(21, 256);
            this.dontSaveArtworkToDiskCheckBox.Name = "dontSaveArtworkToDiskCheckBox";
            this.dontSaveArtworkToDiskCheckBox.Size = new System.Drawing.Size(160, 17);
            this.dontSaveArtworkToDiskCheckBox.TabIndex = 9;
            this.dontSaveArtworkToDiskCheckBox.Text = "Don\'t save artwork to disk";
            this.dontSaveArtworkToDiskCheckBox.UseVisualStyleBackColor = true;
            this.dontSaveArtworkToDiskCheckBox.CheckedChanged += new System.EventHandler(this.dontSaveArtworkToDiskCheckBox_CheckedChanged);
            // 
            // useItemPosInPlaylistCheckBox
            // 
            this.useItemPosInPlaylistCheckBox.Font = new System.Drawing.Font("Nirmala UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.useItemPosInPlaylistCheckBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.useItemPosInPlaylistCheckBox.Location = new System.Drawing.Point(21, 420);
            this.useItemPosInPlaylistCheckBox.Name = "useItemPosInPlaylistCheckBox";
            this.useItemPosInPlaylistCheckBox.Size = new System.Drawing.Size(361, 31);
            this.useItemPosInPlaylistCheckBox.TabIndex = 14;
            this.useItemPosInPlaylistCheckBox.Text = "Use item positions instead of album track numbers in file names";
            this.useItemPosInPlaylistCheckBox.UseVisualStyleBackColor = true;
            this.useItemPosInPlaylistCheckBox.CheckedChanged += new System.EventHandler(this.useItemPosInPlaylistCheckBox_CheckedChanged);
            // 
            // taggingOptionsPanel
            // 
            this.taggingOptionsPanel.AutoScroll = true;
            this.taggingOptionsPanel.Controls.Add(this.albumArtistCheckBox);
            this.taggingOptionsPanel.Controls.Add(this.albumTitleCheckBox);
            this.taggingOptionsPanel.Controls.Add(this.trackArtistCheckBox);
            this.taggingOptionsPanel.Controls.Add(this.trackTitleCheckBox);
            this.taggingOptionsPanel.Controls.Add(this.releaseDateCheckBox);
            this.taggingOptionsPanel.Controls.Add(this.releaseTypeCheckBox);
            this.taggingOptionsPanel.Controls.Add(this.genreCheckBox);
            this.taggingOptionsPanel.Controls.Add(this.trackNumberCheckBox);
            this.taggingOptionsPanel.Controls.Add(this.trackTotalCheckBox);
            this.taggingOptionsPanel.Controls.Add(this.discNumberCheckBox);
            this.taggingOptionsPanel.Controls.Add(this.discTotalCheckBox);
            this.taggingOptionsPanel.Controls.Add(this.composerCheckBox);
            this.taggingOptionsPanel.Controls.Add(this.explicitCheckBox);
            this.taggingOptionsPanel.Controls.Add(this.coverArtCheckBox);
            this.taggingOptionsPanel.Controls.Add(this.copyrightCheckBox);
            this.taggingOptionsPanel.Controls.Add(this.labelCheckBox);
            this.taggingOptionsPanel.Controls.Add(this.upcCheckBox);
            this.taggingOptionsPanel.Controls.Add(this.isrcCheckBox);
            this.taggingOptionsPanel.Controls.Add(this.urlCheckBox);
            this.taggingOptionsPanel.Location = new System.Drawing.Point(18, 63);
            this.taggingOptionsPanel.Name = "taggingOptionsPanel";
            this.taggingOptionsPanel.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.taggingOptionsPanel.Size = new System.Drawing.Size(733, 72);
            this.taggingOptionsPanel.TabIndex = 2;
            // 
            // albumArtistCheckBox
            // 
            this.albumArtistCheckBox.AutoSize = true;
            this.albumArtistCheckBox.Checked = true;
            this.albumArtistCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.albumArtistCheckBox.Font = new System.Drawing.Font("Nirmala UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.albumArtistCheckBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.albumArtistCheckBox.Location = new System.Drawing.Point(3, 3);
            this.albumArtistCheckBox.Name = "albumArtistCheckBox";
            this.albumArtistCheckBox.Size = new System.Drawing.Size(89, 17);
            this.albumArtistCheckBox.TabIndex = 0;
            this.albumArtistCheckBox.Text = "Album Artist";
            this.albumArtistCheckBox.UseVisualStyleBackColor = true;
            this.albumArtistCheckBox.CheckedChanged += new System.EventHandler(this.albumArtistCheckBox_CheckedChanged);
            // 
            // albumTitleCheckBox
            // 
            this.albumTitleCheckBox.AutoSize = true;
            this.albumTitleCheckBox.Checked = true;
            this.albumTitleCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.albumTitleCheckBox.Font = new System.Drawing.Font("Nirmala UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.albumTitleCheckBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.albumTitleCheckBox.Location = new System.Drawing.Point(98, 3);
            this.albumTitleCheckBox.Name = "albumTitleCheckBox";
            this.albumTitleCheckBox.Size = new System.Drawing.Size(83, 17);
            this.albumTitleCheckBox.TabIndex = 1;
            this.albumTitleCheckBox.Text = "Album Title";
            this.albumTitleCheckBox.UseVisualStyleBackColor = true;
            this.albumTitleCheckBox.CheckedChanged += new System.EventHandler(this.albumTitleCheckBox_CheckedChanged);
            // 
            // trackArtistCheckBox
            // 
            this.trackArtistCheckBox.AutoSize = true;
            this.trackArtistCheckBox.Checked = true;
            this.trackArtistCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.trackArtistCheckBox.Font = new System.Drawing.Font("Nirmala UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.trackArtistCheckBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.trackArtistCheckBox.Location = new System.Drawing.Point(187, 3);
            this.trackArtistCheckBox.Name = "trackArtistCheckBox";
            this.trackArtistCheckBox.Size = new System.Drawing.Size(81, 17);
            this.trackArtistCheckBox.TabIndex = 2;
            this.trackArtistCheckBox.Text = "Track Artist";
            this.trackArtistCheckBox.UseVisualStyleBackColor = true;
            this.trackArtistCheckBox.CheckedChanged += new System.EventHandler(this.trackArtistCheckBox_CheckedChanged);
            // 
            // trackTitleCheckBox
            // 
            this.trackTitleCheckBox.AutoSize = true;
            this.trackTitleCheckBox.Checked = true;
            this.trackTitleCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.trackTitleCheckBox.Font = new System.Drawing.Font("Nirmala UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.trackTitleCheckBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.trackTitleCheckBox.Location = new System.Drawing.Point(274, 3);
            this.trackTitleCheckBox.Name = "trackTitleCheckBox";
            this.trackTitleCheckBox.Size = new System.Drawing.Size(75, 17);
            this.trackTitleCheckBox.TabIndex = 3;
            this.trackTitleCheckBox.Text = "Track Title";
            this.trackTitleCheckBox.UseVisualStyleBackColor = true;
            this.trackTitleCheckBox.CheckedChanged += new System.EventHandler(this.trackTitleCheckBox_CheckedChanged);
            // 
            // releaseDateCheckBox
            // 
            this.releaseDateCheckBox.AutoSize = true;
            this.releaseDateCheckBox.Checked = true;
            this.releaseDateCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.releaseDateCheckBox.Font = new System.Drawing.Font("Nirmala UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.releaseDateCheckBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.releaseDateCheckBox.Location = new System.Drawing.Point(355, 3);
            this.releaseDateCheckBox.Name = "releaseDateCheckBox";
            this.releaseDateCheckBox.Size = new System.Drawing.Size(92, 17);
            this.releaseDateCheckBox.TabIndex = 4;
            this.releaseDateCheckBox.Text = "Release Date";
            this.releaseDateCheckBox.UseVisualStyleBackColor = true;
            this.releaseDateCheckBox.CheckedChanged += new System.EventHandler(this.releaseDateCheckBox_CheckedChanged);
            // 
            // releaseTypeCheckBox
            // 
            this.releaseTypeCheckBox.AutoSize = true;
            this.releaseTypeCheckBox.Checked = true;
            this.releaseTypeCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.releaseTypeCheckBox.Font = new System.Drawing.Font("Nirmala UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.releaseTypeCheckBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.releaseTypeCheckBox.Location = new System.Drawing.Point(453, 3);
            this.releaseTypeCheckBox.Name = "releaseTypeCheckBox";
            this.releaseTypeCheckBox.Size = new System.Drawing.Size(90, 17);
            this.releaseTypeCheckBox.TabIndex = 5;
            this.releaseTypeCheckBox.Text = "Release Type";
            this.releaseTypeCheckBox.UseVisualStyleBackColor = true;
            this.releaseTypeCheckBox.CheckedChanged += new System.EventHandler(this.releaseTypeCheckBox_CheckedChanged);
            // 
            // genreCheckBox
            // 
            this.genreCheckBox.AutoSize = true;
            this.genreCheckBox.Checked = true;
            this.genreCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.genreCheckBox.Font = new System.Drawing.Font("Nirmala UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.genreCheckBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.genreCheckBox.Location = new System.Drawing.Point(549, 3);
            this.genreCheckBox.Name = "genreCheckBox";
            this.genreCheckBox.Size = new System.Drawing.Size(57, 17);
            this.genreCheckBox.TabIndex = 6;
            this.genreCheckBox.Text = "Genre";
            this.genreCheckBox.UseVisualStyleBackColor = true;
            this.genreCheckBox.CheckedChanged += new System.EventHandler(this.genreCheckBox_CheckedChanged);
            // 
            // trackNumberCheckBox
            // 
            this.trackNumberCheckBox.AutoSize = true;
            this.trackNumberCheckBox.Checked = true;
            this.trackNumberCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.trackNumberCheckBox.Font = new System.Drawing.Font("Nirmala UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.trackNumberCheckBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.trackNumberCheckBox.Location = new System.Drawing.Point(612, 3);
            this.trackNumberCheckBox.Name = "trackNumberCheckBox";
            this.trackNumberCheckBox.Size = new System.Drawing.Size(95, 17);
            this.trackNumberCheckBox.TabIndex = 7;
            this.trackNumberCheckBox.Text = "Track Number";
            this.trackNumberCheckBox.UseVisualStyleBackColor = true;
            this.trackNumberCheckBox.CheckedChanged += new System.EventHandler(this.trackNumberCheckBox_CheckedChanged);
            // 
            // trackTotalCheckBox
            // 
            this.trackTotalCheckBox.AutoSize = true;
            this.trackTotalCheckBox.Checked = true;
            this.trackTotalCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.trackTotalCheckBox.Font = new System.Drawing.Font("Nirmala UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.trackTotalCheckBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.trackTotalCheckBox.Location = new System.Drawing.Point(3, 26);
            this.trackTotalCheckBox.Name = "trackTotalCheckBox";
            this.trackTotalCheckBox.Size = new System.Drawing.Size(83, 17);
            this.trackTotalCheckBox.TabIndex = 8;
            this.trackTotalCheckBox.Text = "Total Tracks";
            this.trackTotalCheckBox.UseVisualStyleBackColor = true;
            this.trackTotalCheckBox.CheckedChanged += new System.EventHandler(this.trackTotalCheckBox_CheckedChanged);
            // 
            // discNumberCheckBox
            // 
            this.discNumberCheckBox.AutoSize = true;
            this.discNumberCheckBox.Checked = true;
            this.discNumberCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.discNumberCheckBox.Font = new System.Drawing.Font("Nirmala UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.discNumberCheckBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.discNumberCheckBox.Location = new System.Drawing.Point(92, 26);
            this.discNumberCheckBox.Name = "discNumberCheckBox";
            this.discNumberCheckBox.Size = new System.Drawing.Size(91, 17);
            this.discNumberCheckBox.TabIndex = 9;
            this.discNumberCheckBox.Text = "Disc Number";
            this.discNumberCheckBox.UseVisualStyleBackColor = true;
            this.discNumberCheckBox.CheckedChanged += new System.EventHandler(this.discNumberCheckBox_CheckedChanged);
            // 
            // discTotalCheckBox
            // 
            this.discTotalCheckBox.AutoSize = true;
            this.discTotalCheckBox.Checked = true;
            this.discTotalCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.discTotalCheckBox.Font = new System.Drawing.Font("Nirmala UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.discTotalCheckBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.discTotalCheckBox.Location = new System.Drawing.Point(189, 26);
            this.discTotalCheckBox.Name = "discTotalCheckBox";
            this.discTotalCheckBox.Size = new System.Drawing.Size(79, 17);
            this.discTotalCheckBox.TabIndex = 10;
            this.discTotalCheckBox.Text = "Total Discs";
            this.discTotalCheckBox.UseVisualStyleBackColor = true;
            this.discTotalCheckBox.CheckedChanged += new System.EventHandler(this.discTotalCheckBox_CheckedChanged);
            // 
            // composerCheckBox
            // 
            this.composerCheckBox.AutoSize = true;
            this.composerCheckBox.Checked = true;
            this.composerCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.composerCheckBox.Font = new System.Drawing.Font("Nirmala UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.composerCheckBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.composerCheckBox.Location = new System.Drawing.Point(274, 26);
            this.composerCheckBox.Name = "composerCheckBox";
            this.composerCheckBox.Size = new System.Drawing.Size(78, 17);
            this.composerCheckBox.TabIndex = 11;
            this.composerCheckBox.Text = "Composer";
            this.composerCheckBox.UseVisualStyleBackColor = true;
            this.composerCheckBox.CheckedChanged += new System.EventHandler(this.composerCheckBox_CheckedChanged);
            // 
            // explicitCheckBox
            // 
            this.explicitCheckBox.AutoSize = true;
            this.explicitCheckBox.Checked = true;
            this.explicitCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.explicitCheckBox.Font = new System.Drawing.Font("Nirmala UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.explicitCheckBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.explicitCheckBox.Location = new System.Drawing.Point(358, 26);
            this.explicitCheckBox.Name = "explicitCheckBox";
            this.explicitCheckBox.Size = new System.Drawing.Size(108, 17);
            this.explicitCheckBox.TabIndex = 12;
            this.explicitCheckBox.Text = "Explicit Advisory";
            this.explicitCheckBox.UseVisualStyleBackColor = true;
            this.explicitCheckBox.CheckedChanged += new System.EventHandler(this.explicitCheckBox_CheckedChanged);
            // 
            // coverArtCheckBox
            // 
            this.coverArtCheckBox.AutoSize = true;
            this.coverArtCheckBox.Checked = true;
            this.coverArtCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.coverArtCheckBox.Font = new System.Drawing.Font("Nirmala UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.coverArtCheckBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.coverArtCheckBox.Location = new System.Drawing.Point(472, 26);
            this.coverArtCheckBox.Name = "coverArtCheckBox";
            this.coverArtCheckBox.Size = new System.Drawing.Size(73, 17);
            this.coverArtCheckBox.TabIndex = 13;
            this.coverArtCheckBox.Text = "Cover Art";
            this.coverArtCheckBox.UseVisualStyleBackColor = true;
            this.coverArtCheckBox.CheckedChanged += new System.EventHandler(this.coverArtCheckBox_CheckedChanged);
            // 
            // copyrightCheckBox
            // 
            this.copyrightCheckBox.AutoSize = true;
            this.copyrightCheckBox.Checked = true;
            this.copyrightCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.copyrightCheckBox.Font = new System.Drawing.Font("Nirmala UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.copyrightCheckBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.copyrightCheckBox.Location = new System.Drawing.Point(551, 26);
            this.copyrightCheckBox.Name = "copyrightCheckBox";
            this.copyrightCheckBox.Size = new System.Drawing.Size(77, 17);
            this.copyrightCheckBox.TabIndex = 14;
            this.copyrightCheckBox.Text = "Copyright";
            this.copyrightCheckBox.UseVisualStyleBackColor = true;
            this.copyrightCheckBox.CheckedChanged += new System.EventHandler(this.copyrightCheckBox_CheckedChanged);
            // 
            // labelCheckBox
            // 
            this.labelCheckBox.AutoSize = true;
            this.labelCheckBox.Checked = true;
            this.labelCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.labelCheckBox.Font = new System.Drawing.Font("Nirmala UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCheckBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.labelCheckBox.Location = new System.Drawing.Point(634, 26);
            this.labelCheckBox.Name = "labelCheckBox";
            this.labelCheckBox.Size = new System.Drawing.Size(53, 17);
            this.labelCheckBox.TabIndex = 15;
            this.labelCheckBox.Text = "Label";
            this.labelCheckBox.UseVisualStyleBackColor = true;
            this.labelCheckBox.CheckedChanged += new System.EventHandler(this.labelCheckBox_CheckedChanged);
            // 
            // upcCheckBox
            // 
            this.upcCheckBox.AutoSize = true;
            this.upcCheckBox.Checked = true;
            this.upcCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.upcCheckBox.Font = new System.Drawing.Font("Nirmala UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.upcCheckBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.upcCheckBox.Location = new System.Drawing.Point(3, 49);
            this.upcCheckBox.Name = "upcCheckBox";
            this.upcCheckBox.Size = new System.Drawing.Size(99, 17);
            this.upcCheckBox.TabIndex = 16;
            this.upcCheckBox.Text = "UPC / Barcode";
            this.upcCheckBox.UseVisualStyleBackColor = true;
            this.upcCheckBox.CheckedChanged += new System.EventHandler(this.upcCheckBox_CheckedChanged);
            // 
            // isrcCheckBox
            // 
            this.isrcCheckBox.AutoSize = true;
            this.isrcCheckBox.Checked = true;
            this.isrcCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.isrcCheckBox.Font = new System.Drawing.Font("Nirmala UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.isrcCheckBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.isrcCheckBox.Location = new System.Drawing.Point(108, 49);
            this.isrcCheckBox.Name = "isrcCheckBox";
            this.isrcCheckBox.Size = new System.Drawing.Size(49, 17);
            this.isrcCheckBox.TabIndex = 17;
            this.isrcCheckBox.Text = "ISRC";
            this.isrcCheckBox.UseVisualStyleBackColor = true;
            this.isrcCheckBox.CheckedChanged += new System.EventHandler(this.isrcCheckBox_CheckedChanged);
            // 
            // urlCheckBox
            // 
            this.urlCheckBox.AutoSize = true;
            this.urlCheckBox.Checked = true;
            this.urlCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.urlCheckBox.Font = new System.Drawing.Font("Nirmala UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.urlCheckBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.urlCheckBox.Location = new System.Drawing.Point(163, 49);
            this.urlCheckBox.Name = "urlCheckBox";
            this.urlCheckBox.Size = new System.Drawing.Size(46, 17);
            this.urlCheckBox.TabIndex = 18;
            this.urlCheckBox.Text = "URL";
            this.urlCheckBox.UseVisualStyleBackColor = true;
            this.urlCheckBox.CheckedChanged += new System.EventHandler(this.urlCheckBox_CheckedChanged);
            // 
            // languageLabel
            // 
            this.languageLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.languageLabel.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.languageLabel.Location = new System.Drawing.Point(25, 492);
            this.languageLabel.Name = "languageLabel";
            this.languageLabel.Size = new System.Drawing.Size(286, 19);
            this.languageLabel.TabIndex = 16;
            this.languageLabel.Text = "Current Language";
            this.languageLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // languageComboBox
            // 
            this.languageComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.languageComboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.languageComboBox.FormattingEnabled = true;
            this.languageComboBox.Location = new System.Drawing.Point(317, 490);
            this.languageComboBox.Name = "languageComboBox";
            this.languageComboBox.Size = new System.Drawing.Size(60, 21);
            this.languageComboBox.TabIndex = 17;
            this.languageComboBox.SelectedIndexChanged += new System.EventHandler(this.languageComboBox_SelectedIndexChanged);
            // 
            // downloadFromArtistLabel
            // 
            this.downloadFromArtistLabel.Font = new System.Drawing.Font("Nirmala UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.downloadFromArtistLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.downloadFromArtistLabel.Location = new System.Drawing.Point(4, 284);
            this.downloadFromArtistLabel.Name = "downloadFromArtistLabel";
            this.downloadFromArtistLabel.Size = new System.Drawing.Size(373, 25);
            this.downloadFromArtistLabel.TabIndex = 10;
            this.downloadFromArtistLabel.Text = "DOWNLOAD FROM ARTIST";
            this.downloadFromArtistLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // downloadAllFromArtistCheckBox
            // 
            this.downloadAllFromArtistCheckBox.AutoSize = true;
            this.downloadAllFromArtistCheckBox.Font = new System.Drawing.Font("Nirmala UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.downloadAllFromArtistCheckBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.downloadAllFromArtistCheckBox.Location = new System.Drawing.Point(21, 363);
            this.downloadAllFromArtistCheckBox.Name = "downloadAllFromArtistCheckBox";
            this.downloadAllFromArtistCheckBox.Size = new System.Drawing.Size(260, 17);
            this.downloadAllFromArtistCheckBox.TabIndex = 12;
            this.downloadAllFromArtistCheckBox.Text = "Download all from artist (it may include more)";
            this.downloadAllFromArtistCheckBox.UseVisualStyleBackColor = true;
            this.downloadAllFromArtistCheckBox.CheckedChanged += new System.EventHandler(this.downloadAllFromArtistCheckBox_CheckedChanged);
            // 
            // themeLabel
            // 
            this.themeLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.themeLabel.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.themeLabel.Location = new System.Drawing.Point(25, 521);
            this.themeLabel.Name = "themeLabel";
            this.themeLabel.Size = new System.Drawing.Size(219, 19);
            this.themeLabel.TabIndex = 18;
            this.themeLabel.Text = "Current Theme";
            this.themeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // themeComboBox
            // 
            this.themeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.themeComboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.themeComboBox.FormattingEnabled = true;
            this.themeComboBox.Location = new System.Drawing.Point(250, 519);
            this.themeComboBox.Name = "themeComboBox";
            this.themeComboBox.Size = new System.Drawing.Size(127, 21);
            this.themeComboBox.TabIndex = 19;
            this.themeComboBox.SelectedIndexChanged += new System.EventHandler(this.themeComboBox_SelectedIndexChanged);
            // 
            // themeSectionLabel
            // 
            this.themeSectionLabel.Font = new System.Drawing.Font("Nirmala UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.themeSectionLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.themeSectionLabel.Location = new System.Drawing.Point(3, 459);
            this.themeSectionLabel.Name = "themeSectionLabel";
            this.themeSectionLabel.Size = new System.Drawing.Size(374, 25);
            this.themeSectionLabel.TabIndex = 15;
            this.themeSectionLabel.Text = "VISUAL OPTIONS";
            this.themeSectionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // commentCheckBox
            // 
            this.commentCheckBox.AutoSize = true;
            this.commentCheckBox.Font = new System.Drawing.Font("Nirmala UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.commentCheckBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.commentCheckBox.Location = new System.Drawing.Point(21, 148);
            this.commentCheckBox.Name = "commentCheckBox";
            this.commentCheckBox.Size = new System.Drawing.Size(115, 17);
            this.commentCheckBox.TabIndex = 3;
            this.commentCheckBox.Text = "Custom comment";
            this.commentCheckBox.UseVisualStyleBackColor = true;
            this.commentCheckBox.CheckedChanged += new System.EventHandler(this.commentCheckBox_CheckedChanged);
            // 
            // commentTextBox
            // 
            this.commentTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.commentTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.commentTextBox.Enabled = false;
            this.commentTextBox.Font = new System.Drawing.Font("Nirmala UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.commentTextBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.commentTextBox.Location = new System.Drawing.Point(21, 168);
            this.commentTextBox.Multiline = true;
            this.commentTextBox.Name = "commentTextBox";
            this.commentTextBox.Size = new System.Drawing.Size(356, 26);
            this.commentTextBox.TabIndex = 4;
            this.commentTextBox.WordWrap = false;
            this.commentTextBox.TextChanged += new System.EventHandler(this.commentTextBox_TextChanged);
            // 
            // advancedOptionsLabel
            // 
            this.advancedOptionsLabel.Font = new System.Drawing.Font("Nirmala UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.advancedOptionsLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.advancedOptionsLabel.Location = new System.Drawing.Point(386, 143);
            this.advancedOptionsLabel.Name = "advancedOptionsLabel";
            this.advancedOptionsLabel.Size = new System.Drawing.Size(384, 25);
            this.advancedOptionsLabel.TabIndex = 21;
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
            this.closeAdditionalButton.Location = new System.Drawing.Point(541, 533);
            this.closeAdditionalButton.Name = "closeAdditionalButton";
            this.closeAdditionalButton.Size = new System.Drawing.Size(150, 31);
            this.closeAdditionalButton.TabIndex = 23;
            this.closeAdditionalButton.Text = "Back to Settings";
            this.closeAdditionalButton.UseVisualStyleBackColor = false;
            this.closeAdditionalButton.Click += new System.EventHandler(this.closeAdditionalButton_Click);
            // 
            // savedArtLabel
            // 
            this.savedArtLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.savedArtLabel.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.savedArtLabel.Location = new System.Drawing.Point(22, 230);
            this.savedArtLabel.Name = "savedArtLabel";
            this.savedArtLabel.Size = new System.Drawing.Size(289, 13);
            this.savedArtLabel.TabIndex = 7;
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
            this.savedArtSizeSelect.Location = new System.Drawing.Point(317, 230);
            this.savedArtSizeSelect.Name = "savedArtSizeSelect";
            this.savedArtSizeSelect.Size = new System.Drawing.Size(60, 21);
            this.savedArtSizeSelect.TabIndex = 8;
            this.savedArtSizeSelect.SelectedIndexChanged += new System.EventHandler(this.savedArtSizeSelect_SelectedIndexChanged);
            // 
            // taggingOptionsLabel
            // 
            this.taggingOptionsLabel.Font = new System.Drawing.Font("Nirmala UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.taggingOptionsLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.taggingOptionsLabel.Location = new System.Drawing.Point(3, 35);
            this.taggingOptionsLabel.Name = "taggingOptionsLabel";
            this.taggingOptionsLabel.Size = new System.Drawing.Size(768, 25);
            this.taggingOptionsLabel.TabIndex = 1;
            this.taggingOptionsLabel.Text = "TAGGING OPTIONS";
            this.taggingOptionsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // embeddedArtLabel
            // 
            this.embeddedArtLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.embeddedArtLabel.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.embeddedArtLabel.Location = new System.Drawing.Point(23, 204);
            this.embeddedArtLabel.Name = "embeddedArtLabel";
            this.embeddedArtLabel.Size = new System.Drawing.Size(288, 13);
            this.embeddedArtLabel.TabIndex = 5;
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
            this.embeddedArtSizeSelect.Location = new System.Drawing.Point(317, 204);
            this.embeddedArtSizeSelect.Name = "embeddedArtSizeSelect";
            this.embeddedArtSizeSelect.Size = new System.Drawing.Size(60, 21);
            this.embeddedArtSizeSelect.TabIndex = 6;
            this.embeddedArtSizeSelect.SelectedIndexChanged += new System.EventHandler(this.embeddedArtSizeSelect_SelectedIndexChanged);
            // 
            // extraSettingsLabel
            // 
            this.extraSettingsLabel.Font = new System.Drawing.Font("Nirmala UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.extraSettingsLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.extraSettingsLabel.Location = new System.Drawing.Point(13, 10);
            this.extraSettingsLabel.Name = "extraSettingsLabel";
            this.extraSettingsLabel.Size = new System.Drawing.Size(752, 25);
            this.extraSettingsLabel.TabIndex = 0;
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
            this.qualitySelectButton.Location = new System.Drawing.Point(709, -1);
            this.qualitySelectButton.Name = "qualitySelectButton";
            this.qualitySelectButton.Size = new System.Drawing.Size(172, 31);
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
            this.searchPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(47)))), ((int)(((byte)(52)))));
            this.searchPanel.Controls.Add(this.limitSearchResultsNumericUpDown);
            this.searchPanel.Controls.Add(this.searchSortingPanel);
            this.searchPanel.Controls.Add(this.deselectAllRowsButton);
            this.searchPanel.Controls.Add(this.selectAllRowsButton);
            this.searchPanel.Controls.Add(this.batchDownloadSelectedRowsButton);
            this.searchPanel.Controls.Add(this.selectedRowsCountLabel);
            this.searchPanel.Controls.Add(this.searchResultsCountLabel);
            this.searchPanel.Controls.Add(this.searchResultsPanel);
            this.searchPanel.Controls.Add(this.searchAlbumsButton);
            this.searchPanel.Controls.Add(this.searchTracksButton);
            this.searchPanel.Controls.Add(this.searchTextBox);
            this.searchPanel.Controls.Add(this.searchLabel);
            this.searchPanel.Controls.Add(this.searchingLabel);
            this.searchPanel.Controls.Add(this.sortingSearchResultsLabel);
            this.searchPanel.Font = new System.Drawing.Font("Nirmala UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.searchPanel.Location = new System.Drawing.Point(335, 235);
            this.searchPanel.Name = "searchPanel";
            this.searchPanel.Size = new System.Drawing.Size(771, 577);
            this.searchPanel.TabIndex = 0;
            // 
            // limitSearchResultsNumericUpDown
            // 
            this.limitSearchResultsNumericUpDown.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.limitSearchResultsNumericUpDown.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.limitSearchResultsNumericUpDown.Font = new System.Drawing.Font("Nirmala UI", 15.6F);
            this.limitSearchResultsNumericUpDown.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.limitSearchResultsNumericUpDown.Location = new System.Drawing.Point(523, 46);
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
            this.limitSearchResultsNumericUpDown.Size = new System.Drawing.Size(50, 31);
            this.limitSearchResultsNumericUpDown.TabIndex = 8;
            this.limitSearchResultsNumericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.limitSearchResultsNumericUpDown.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // searchSortingPanel
            // 
            this.searchSortingPanel.Controls.Add(this.searchSortingLabel);
            this.searchSortingPanel.Controls.Add(this.sortReleaseDateButton);
            this.searchSortingPanel.Controls.Add(this.sortArtistNameButton);
            this.searchSortingPanel.Controls.Add(this.sortAlbumTrackNameButton);
            this.searchSortingPanel.Controls.Add(this.sortGenreButton);
            this.searchSortingPanel.Controls.Add(this.sortAscendantCheckBox);
            this.searchSortingPanel.Location = new System.Drawing.Point(18, 83);
            this.searchSortingPanel.Margin = new System.Windows.Forms.Padding(0);
            this.searchSortingPanel.Name = "searchSortingPanel";
            this.searchSortingPanel.Size = new System.Drawing.Size(734, 28);
            this.searchSortingPanel.TabIndex = 7;
            // 
            // searchSortingLabel
            // 
            this.searchSortingLabel.AutoSize = true;
            this.searchSortingLabel.Font = new System.Drawing.Font("Nirmala UI", 12F);
            this.searchSortingLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.searchSortingLabel.Location = new System.Drawing.Point(0, 0);
            this.searchSortingLabel.Margin = new System.Windows.Forms.Padding(0);
            this.searchSortingLabel.Name = "searchSortingLabel";
            this.searchSortingLabel.Size = new System.Drawing.Size(64, 21);
            this.searchSortingLabel.TabIndex = 4;
            this.searchSortingLabel.Text = "Sorting:";
            this.searchSortingLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // sortReleaseDateButton
            // 
            this.sortReleaseDateButton.AutoSize = true;
            this.sortReleaseDateButton.Checked = true;
            this.sortReleaseDateButton.Cursor = System.Windows.Forms.Cursors.Default;
            this.sortReleaseDateButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.sortReleaseDateButton.Location = new System.Drawing.Point(67, 5);
            this.sortReleaseDateButton.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.sortReleaseDateButton.Name = "sortReleaseDateButton";
            this.sortReleaseDateButton.Size = new System.Drawing.Size(91, 17);
            this.sortReleaseDateButton.TabIndex = 0;
            this.sortReleaseDateButton.TabStop = true;
            this.sortReleaseDateButton.Text = "Release Date";
            this.sortReleaseDateButton.UseVisualStyleBackColor = true;
            this.sortReleaseDateButton.CheckedChanged += new System.EventHandler(this.sortReleaseDateButton_CheckedChanged);
            // 
            // sortArtistNameButton
            // 
            this.sortArtistNameButton.AutoSize = true;
            this.sortArtistNameButton.Cursor = System.Windows.Forms.Cursors.Default;
            this.sortArtistNameButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.sortArtistNameButton.Location = new System.Drawing.Point(164, 5);
            this.sortArtistNameButton.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.sortArtistNameButton.Name = "sortArtistNameButton";
            this.sortArtistNameButton.Size = new System.Drawing.Size(84, 17);
            this.sortArtistNameButton.TabIndex = 4;
            this.sortArtistNameButton.Text = "Artist Name";
            this.sortArtistNameButton.UseVisualStyleBackColor = true;
            this.sortArtistNameButton.CheckedChanged += new System.EventHandler(this.sortArtistNameButton_CheckedChanged);
            // 
            // sortAlbumTrackNameButton
            // 
            this.sortAlbumTrackNameButton.AutoSize = true;
            this.sortAlbumTrackNameButton.Cursor = System.Windows.Forms.Cursors.Default;
            this.sortAlbumTrackNameButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.sortAlbumTrackNameButton.Location = new System.Drawing.Point(254, 5);
            this.sortAlbumTrackNameButton.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.sortAlbumTrackNameButton.Name = "sortAlbumTrackNameButton";
            this.sortAlbumTrackNameButton.Size = new System.Drawing.Size(117, 17);
            this.sortAlbumTrackNameButton.TabIndex = 6;
            this.sortAlbumTrackNameButton.Text = "Album / Track Title";
            this.sortAlbumTrackNameButton.UseVisualStyleBackColor = true;
            this.sortAlbumTrackNameButton.CheckedChanged += new System.EventHandler(this.sortAlbumTrackNameButton_CheckedChanged);
            // 
            // sortGenreButton
            // 
            this.sortGenreButton.AutoSize = true;
            this.sortGenreButton.Cursor = System.Windows.Forms.Cursors.Default;
            this.sortGenreButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.sortGenreButton.Location = new System.Drawing.Point(377, 5);
            this.sortGenreButton.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.sortGenreButton.Name = "sortGenreButton";
            this.sortGenreButton.Size = new System.Drawing.Size(56, 17);
            this.sortGenreButton.TabIndex = 2;
            this.sortGenreButton.Text = "Genre";
            this.sortGenreButton.UseVisualStyleBackColor = true;
            this.sortGenreButton.CheckedChanged += new System.EventHandler(this.sortGenreButton_CheckedChanged);
            // 
            // sortAscendantCheckBox
            // 
            this.sortAscendantCheckBox.AutoSize = true;
            this.sortAscendantCheckBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.sortAscendantCheckBox.Location = new System.Drawing.Point(439, 5);
            this.sortAscendantCheckBox.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.sortAscendantCheckBox.Name = "sortAscendantCheckBox";
            this.sortAscendantCheckBox.Size = new System.Drawing.Size(80, 17);
            this.sortAscendantCheckBox.TabIndex = 5;
            this.sortAscendantCheckBox.Text = "Ascendant";
            this.sortAscendantCheckBox.UseVisualStyleBackColor = true;
            this.sortAscendantCheckBox.CheckedChanged += new System.EventHandler(this.sortAscendantCheckBox_CheckedChanged);
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
            this.deselectAllRowsButton.Location = new System.Drawing.Point(285, 541);
            this.deselectAllRowsButton.Name = "deselectAllRowsButton";
            this.deselectAllRowsButton.Size = new System.Drawing.Size(120, 31);
            this.deselectAllRowsButton.TabIndex = 14;
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
            this.selectAllRowsButton.Location = new System.Drawing.Point(159, 541);
            this.selectAllRowsButton.Name = "selectAllRowsButton";
            this.selectAllRowsButton.Size = new System.Drawing.Size(120, 31);
            this.selectAllRowsButton.TabIndex = 12;
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
            this.batchDownloadSelectedRowsButton.Location = new System.Drawing.Point(411, 541);
            this.batchDownloadSelectedRowsButton.Name = "batchDownloadSelectedRowsButton";
            this.batchDownloadSelectedRowsButton.Size = new System.Drawing.Size(340, 31);
            this.batchDownloadSelectedRowsButton.TabIndex = 15;
            this.batchDownloadSelectedRowsButton.Text = "BATCH DOWNLOAD SELECTED ROWS";
            this.batchDownloadSelectedRowsButton.UseVisualStyleBackColor = false;
            this.batchDownloadSelectedRowsButton.Click += new System.EventHandler(this.batchDownloadSelectedRowsButton_Click);
            // 
            // selectedRowsCountLabel
            // 
            this.selectedRowsCountLabel.Font = new System.Drawing.Font("Nirmala UI", 10F);
            this.selectedRowsCountLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.selectedRowsCountLabel.Location = new System.Drawing.Point(16, 554);
            this.selectedRowsCountLabel.Name = "selectedRowsCountLabel";
            this.selectedRowsCountLabel.Size = new System.Drawing.Size(140, 16);
            this.selectedRowsCountLabel.TabIndex = 11;
            this.selectedRowsCountLabel.Text = "0 selected rows";
            this.selectedRowsCountLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // searchResultsCountLabel
            // 
            this.searchResultsCountLabel.Font = new System.Drawing.Font("Nirmala UI", 10F);
            this.searchResultsCountLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.searchResultsCountLabel.Location = new System.Drawing.Point(16, 532);
            this.searchResultsCountLabel.Name = "searchResultsCountLabel";
            this.searchResultsCountLabel.Size = new System.Drawing.Size(140, 16);
            this.searchResultsCountLabel.TabIndex = 10;
            this.searchResultsCountLabel.Text = "…";
            this.searchResultsCountLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // searchResultsPanel
            // 
            this.searchResultsPanel.AutoScroll = true;
            this.searchResultsPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.searchResultsPanel.Controls.Add(this.searchResultsTablePanel);
            this.searchResultsPanel.Location = new System.Drawing.Point(18, 117);
            this.searchResultsPanel.Name = "searchResultsPanel";
            this.searchResultsPanel.Size = new System.Drawing.Size(733, 408);
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
            this.searchAlbumsButton.TabIndex = 2;
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
            this.searchTracksButton.TabIndex = 3;
            this.searchTracksButton.Text = "TRACKS";
            this.searchTracksButton.UseVisualStyleBackColor = false;
            this.searchTracksButton.Click += new System.EventHandler(this.searchTracksButton_Click);
            // 
            // searchTextBox
            // 
            this.searchTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.searchTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.searchTextBox.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.searchTextBox.Font = new System.Drawing.Font("Nirmala UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.searchTextBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.searchTextBox.Location = new System.Drawing.Point(18, 46);
            this.searchTextBox.MaxLength = 1000;
            this.searchTextBox.Multiline = true;
            this.searchTextBox.Name = "searchTextBox";
            this.searchTextBox.Size = new System.Drawing.Size(498, 31);
            this.searchTextBox.TabIndex = 1;
            this.searchTextBox.Text = "Input your search…";
            this.searchTextBox.WordWrap = false;
            this.searchTextBox.Click += new System.EventHandler(this.searchTextBox_Click);
            this.searchTextBox.TextChanged += new System.EventHandler(this.searchTextBox_TextChanged);
            this.searchTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.searchTextBox_KeyDown);
            this.searchTextBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.searchTextBox_KeyUp);
            this.searchTextBox.Leave += new System.EventHandler(this.searchTextBox_Leave);
            this.searchTextBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.searchTextBox_MouseDown);
            // 
            // searchLabel
            // 
            this.searchLabel.Font = new System.Drawing.Font("Nirmala UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.searchLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.searchLabel.Location = new System.Drawing.Point(13, 10);
            this.searchLabel.Name = "searchLabel";
            this.searchLabel.Size = new System.Drawing.Size(503, 30);
            this.searchLabel.TabIndex = 0;
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
            // statusStrip1
            // 
            this.statusStrip1.AllowMerge = false;
            this.statusStrip1.AutoSize = false;
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.prevTipButton,
            this.nextTipButton,
            this.tipEmojiLabel,
            this.tipLabel});
            this.statusStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.statusStrip1.Location = new System.Drawing.Point(1, 564);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(951, 22);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 7;
            this.statusStrip1.Text = "statusStrip1";
            this.statusStrip1.Visible = false;
            // 
            // prevTipButton
            // 
            this.prevTipButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.prevTipButton.Image = ((System.Drawing.Image)(resources.GetObject("prevTipButton.Image")));
            this.prevTipButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.prevTipButton.Name = "prevTipButton";
            this.prevTipButton.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.prevTipButton.ShowDropDownArrow = false;
            this.prevTipButton.Size = new System.Drawing.Size(73, 19);
            this.prevTipButton.Text = "Previous tip";
            this.prevTipButton.Click += new System.EventHandler(this.prevTipButton_Click);
            // 
            // nextTipButton
            // 
            this.nextTipButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.nextTipButton.Image = ((System.Drawing.Image)(resources.GetObject("nextTipButton.Image")));
            this.nextTipButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.nextTipButton.Name = "nextTipButton";
            this.nextTipButton.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.nextTipButton.ShowDropDownArrow = false;
            this.nextTipButton.Size = new System.Drawing.Size(53, 19);
            this.nextTipButton.Text = "Next tip";
            this.nextTipButton.Click += new System.EventHandler(this.nextTipButton_Click);
            // 
            // tipEmojiLabel
            // 
            this.tipEmojiLabel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tipEmojiLabel.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.tipEmojiLabel.Name = "tipEmojiLabel";
            this.tipEmojiLabel.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.tipEmojiLabel.Size = new System.Drawing.Size(16, 15);
            this.tipEmojiLabel.Text = "💡";
            // 
            // tipLabel
            // 
            this.tipLabel.AutoSize = false;
            this.tipLabel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tipLabel.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.tipLabel.Name = "tipLabel";
            this.tipLabel.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.tipLabel.Size = new System.Drawing.Size(47, 15);
            this.tipLabel.Spring = true;
            this.tipLabel.Text = "Tip Text";
            // 
            // timerTip
            // 
            this.timerTip.Interval = 200;
            this.timerTip.Tick += new System.EventHandler(this.timerTip_Tick);
            // 
            // progressBarDownload
            // 
            this.progressBarDownload.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.progressBarDownload.BorderColor = System.Drawing.Color.Black;
            this.progressBarDownload.FillColor = System.Drawing.Color.RoyalBlue;
            this.progressBarDownload.Location = new System.Drawing.Point(184, 79);
            this.progressBarDownload.Name = "progressBarDownload";
            this.progressBarDownload.Size = new System.Drawing.Size(332, 31);
            this.progressBarDownload.Step = 1;
            this.progressBarDownload.TabIndex = 6;
            // 
            // qbdlxForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(31)))), ((int)(((byte)(33)))));
            this.ClientSize = new System.Drawing.Size(951, 580);
            this.Controls.Add(this.exitButton);
            this.Controls.Add(this.minimizeButton);
            this.Controls.Add(this.qualitySelectButton);
            this.Controls.Add(this.qualitySelectPanel);
            this.Controls.Add(this.movingLabel);
            this.Controls.Add(this.navigationPanel);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.downloaderPanel);
            this.Controls.Add(this.searchPanel);
            this.Controls.Add(this.settingsPanel);
            this.Controls.Add(this.extraSettingsPanel);
            this.Controls.Add(this.aboutPanel);
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
            this.logoPanel.PerformLayout();
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
            this.advancedOptionsPanelLeft.ResumeLayout(false);
            this.advancedOptionsPanelLeft.PerformLayout();
            this.advancedOptionsPanelRight.ResumeLayout(false);
            this.advancedOptionsPanelRight.PerformLayout();
            this.artistNamesSeparatorsPanel.ResumeLayout(false);
            this.artistNamesSeparatorsPanel.PerformLayout();
            this.duplicateFilesFlowLayoutPanel.ResumeLayout(false);
            this.taggingOptionsPanel.ResumeLayout(false);
            this.taggingOptionsPanel.PerformLayout();
            this.qualitySelectPanel.ResumeLayout(false);
            this.qualitySelectPanel.PerformLayout();
            this.searchPanel.ResumeLayout(false);
            this.searchPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.limitSearchResultsNumericUpDown)).EndInit();
            this.searchSortingPanel.ResumeLayout(false);
            this.searchSortingPanel.PerformLayout();
            this.searchResultsPanel.ResumeLayout(false);
            this.sysTrayContextMenuStrip.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
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
        internal System.Windows.Forms.RichTextBox userInfoTextBox;
        internal System.Windows.Forms.Label userInfoLabel;
        internal System.Windows.Forms.Label disclaimerLabel;
        internal Ookii.Dialogs.WinForms.VistaFolderBrowserDialog folderBrowser;
        internal System.Windows.Forms.Label downloadOptionsLabel;
        internal System.Windows.Forms.TextBox downloadFolderTextBox;
        internal System.Windows.Forms.Label downloadFolderLabel;
        internal System.Windows.Forms.Button openFolderButton;
        internal System.Windows.Forms.Button selectFolderButton;
        internal System.Windows.Forms.TextBox downloadOutput;
        internal System.Windows.Forms.TextBox trackTemplateTextBox;
        internal System.Windows.Forms.Label trackTemplateLabel;
        internal System.Windows.Forms.Label templatesLabel;
        internal System.Windows.Forms.TextBox playlistTemplateTextBox;
        internal System.Windows.Forms.TextBox albumTemplateTextBox;
        internal System.Windows.Forms.Label playlistTemplateLabel;
        internal System.Windows.Forms.Label albumTemplateLabel;
        internal System.Windows.Forms.Button resetTemplatesButton;
        internal System.Windows.Forms.Button saveTemplatesButton;
        internal System.Windows.Forms.TextBox favoritesTemplateTextBox;
        internal System.Windows.Forms.Label favoritesTemplateLabel;
        internal System.Windows.Forms.TextBox artistTemplateTextBox;
        internal System.Windows.Forms.Label artistTemplateLabel;
        internal System.Windows.Forms.Panel extraSettingsPanel;
        internal System.Windows.Forms.Label extraSettingsLabel;
        internal System.Windows.Forms.CheckBox albumArtistCheckBox;
        internal System.Windows.Forms.CheckBox explicitCheckBox;
        internal System.Windows.Forms.CheckBox coverArtCheckBox;
        internal System.Windows.Forms.CheckBox discTotalCheckBox;
        internal System.Windows.Forms.CheckBox discNumberCheckBox;
        internal System.Windows.Forms.CheckBox trackNumberCheckBox;
        internal System.Windows.Forms.CheckBox trackTotalCheckBox;
        internal System.Windows.Forms.CheckBox isrcCheckBox;
        internal System.Windows.Forms.CheckBox urlCheckBox;
        internal System.Windows.Forms.CheckBox mergeArtistNamesCheckBox;
        internal System.Windows.Forms.CheckBox upcCheckBox;
        internal System.Windows.Forms.CheckBox labelCheckBox;
        internal System.Windows.Forms.CheckBox copyrightCheckBox;
        internal System.Windows.Forms.CheckBox composerCheckBox;
        internal System.Windows.Forms.CheckBox genreCheckBox;
        internal System.Windows.Forms.CheckBox releaseDateCheckBox;
        internal System.Windows.Forms.CheckBox releaseTypeCheckBox;
        internal System.Windows.Forms.CheckBox trackArtistCheckBox;
        internal System.Windows.Forms.CheckBox trackTitleCheckBox;
        internal System.Windows.Forms.CheckBox albumTitleCheckBox;
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
        internal System.Windows.Forms.CheckBox streamableCheckBox;
        internal System.Windows.Forms.CheckBox downloadGoodiesCheckBox;
        internal System.Windows.Forms.CheckBox useTLS13CheckBox;
        internal System.Windows.Forms.TextBox templatesListTextBox;
        internal System.Windows.Forms.Label templatesListLabel;
        internal System.Windows.Forms.CheckBox fixMD5sCheckBox;
        internal System.Windows.Forms.Label movingLabel;
        internal System.Windows.Forms.Button searchButton;
        internal System.Windows.Forms.Panel searchPanel;
        internal System.Windows.Forms.Label searchLabel;
        internal System.Windows.Forms.Button searchTracksButton;
        internal System.Windows.Forms.TextBox searchTextBox;
        internal System.Windows.Forms.Button searchAlbumsButton;
        internal System.Windows.Forms.Panel searchResultsPanel;
        internal System.Windows.Forms.TableLayoutPanel searchResultsTablePanel;
        internal System.Windows.Forms.Label searchingLabel;
        internal System.Windows.Forms.TextBox inputTextBox;
        internal System.Windows.Forms.Button downloadButton;
        internal System.Windows.Forms.CheckBox downloadSpeedCheckBox;
        internal System.Windows.Forms.Label progressLabel;
        internal System.Windows.Forms.CheckBox commentCheckBox;
        internal System.Windows.Forms.TextBox commentTextBox;
        internal System.Windows.Forms.Label themeSectionLabel;
        internal System.Windows.Forms.Label themeLabel;
        internal System.Windows.Forms.ComboBox themeComboBox;
        internal System.Windows.Forms.Label languageLabel;
        internal System.Windows.Forms.ComboBox languageComboBox;
        internal System.Windows.Forms.FlowLayoutPanel taggingOptionsPanel;
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
        internal System.Windows.Forms.Label searchSortingLabel;
        internal System.Windows.Forms.RadioButton sortAlbumTrackNameButton;
        internal System.Windows.Forms.RadioButton sortArtistNameButton;
        internal System.Windows.Forms.RadioButton sortReleaseDateButton;
        internal System.Windows.Forms.CheckBox sortAscendantCheckBox;
        internal System.Windows.Forms.Label sortingSearchResultsLabel;
        internal System.Windows.Forms.Label searchResultsCountLabel;
        internal System.Windows.Forms.Label batchDownloadProgressCountLabel;
        internal System.Windows.Forms.NotifyIcon notifyIcon1;
        internal System.Windows.Forms.ContextMenuStrip sysTrayContextMenuStrip;
        internal System.Windows.Forms.ToolStripMenuItem showWindowToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem hideWindowToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem closeProgramToolStripMenuItem;
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
        internal Label downloadFromArtistLabel;
        internal CheckedListBox downloadFromArtistListBox;
        internal CheckBox downloadAllFromArtistCheckBox;
        internal Label primaryListSeparatorLabel;
        internal TextBox listEndSeparatorTextBox;
        internal Label listEndSeparatorLabel;
        internal TextBox primaryListSeparatorTextBox;
        internal Panel artistNamesSeparatorsPanel;
        internal CheckBox useItemPosInPlaylistCheckBox;
        internal Label playlistSectionLabel;
        internal StatusStrip statusStrip1;
        internal Timer timerTip;
        internal ToolStripStatusLabel tipLabel;
        internal ToolStripStatusLabel tipEmojiLabel;
        internal ToolStripDropDownButton nextTipButton;
        internal ToolStripDropDownButton prevTipButton;
        internal CheckBox showTipsCheckBox;
        internal TextBox vaTrackTemplateTextBox;
        internal Label vaTrackTemplateLabel;
        internal LinkLabel gitHubLinkLabel;
        internal FlowLayoutPanel searchSortingPanel;
        internal CheckBox logFailedDownloadsCheckBox;
        internal FlowLayoutPanel duplicateFilesFlowLayoutPanel;
        internal Label duplicateFilesLabel;
        internal RadioButton skipDuplicatesRadioButton;
        internal RadioButton autoRenameDuplicatesRadioButton;
        internal RadioButton overwriteDuplicatesRadioButton;
    }
}