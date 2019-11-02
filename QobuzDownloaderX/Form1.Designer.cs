namespace QobuzDownloaderX
{
    partial class QobuzDownloaderX
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(QobuzDownloaderX));
            this.testURLBox = new System.Windows.Forms.TextBox();
            this.selectFolder = new System.Windows.Forms.Button();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.output = new System.Windows.Forms.TextBox();
            this.downloadBG = new System.ComponentModel.BackgroundWorker();
            this.openFolderButton = new System.Windows.Forms.Button();
            this.downloadButton = new System.Windows.Forms.Button();
            this.albumUrl = new System.Windows.Forms.TextBox();
            this.imageURLTextbox = new System.Windows.Forms.TextBox();
            this.verNumLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.albumArtistTextBox = new System.Windows.Forms.TextBox();
            this.albumTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.releaseDateTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.upcTextBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.albumArtPicBox = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label6 = new System.Windows.Forms.Label();
            this.totalTracksTextbox = new System.Windows.Forms.TextBox();
            this.totalTracksLabel = new System.Windows.Forms.Label();
            this.getLinkTypeBG = new System.ComponentModel.BackgroundWorker();
            this.downloadAlbumBG = new System.ComponentModel.BackgroundWorker();
            this.downloadTrackBG = new System.ComponentModel.BackgroundWorker();
            this.downloadDiscogBG = new System.ComponentModel.BackgroundWorker();
            this.qualityTextbox = new System.Windows.Forms.TextBox();
            this.qualityLabel = new System.Windows.Forms.Label();
            this.openSearch = new System.Windows.Forms.Button();
            this.tagsLabel = new System.Windows.Forms.Label();
            this.albumArtistCheckbox = new System.Windows.Forms.CheckBox();
            this.artistCheckbox = new System.Windows.Forms.CheckBox();
            this.trackTitleCheckbox = new System.Windows.Forms.CheckBox();
            this.trackNumberCheckbox = new System.Windows.Forms.CheckBox();
            this.trackTotalCheckbox = new System.Windows.Forms.CheckBox();
            this.discNumberCheckbox = new System.Windows.Forms.CheckBox();
            this.discTotalCheckbox = new System.Windows.Forms.CheckBox();
            this.albumCheckbox = new System.Windows.Forms.CheckBox();
            this.explicitCheckbox = new System.Windows.Forms.CheckBox();
            this.upcCheckbox = new System.Windows.Forms.CheckBox();
            this.isrcCheckbox = new System.Windows.Forms.CheckBox();
            this.copyrightCheckbox = new System.Windows.Forms.CheckBox();
            this.composerCheckbox = new System.Windows.Forms.CheckBox();
            this.genreCheckbox = new System.Windows.Forms.CheckBox();
            this.releaseCheckbox = new System.Windows.Forms.CheckBox();
            this.commentCheckbox = new System.Windows.Forms.CheckBox();
            this.commentTextbox = new System.Windows.Forms.TextBox();
            this.imageCheckbox = new System.Windows.Forms.CheckBox();
            this.mp3Checkbox = new System.Windows.Forms.CheckBox();
            this.flacLowCheckbox = new System.Windows.Forms.CheckBox();
            this.flacMidCheckbox = new System.Windows.Forms.CheckBox();
            this.flacHighCheckbox = new System.Windows.Forms.CheckBox();
            this.mp3WarnLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.albumArtPicBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // testURLBox
            // 
            this.testURLBox.Location = new System.Drawing.Point(946, 394);
            this.testURLBox.Multiline = true;
            this.testURLBox.Name = "testURLBox";
            this.testURLBox.Size = new System.Drawing.Size(390, 20);
            this.testURLBox.TabIndex = 1;
            this.testURLBox.Text = "Steamed Qobuz File URL";
            this.testURLBox.Visible = false;
            this.testURLBox.WordWrap = false;
            // 
            // selectFolder
            // 
            this.selectFolder.Location = new System.Drawing.Point(13, 115);
            this.selectFolder.Name = "selectFolder";
            this.selectFolder.Size = new System.Drawing.Size(349, 23);
            this.selectFolder.TabIndex = 2;
            this.selectFolder.Text = "Choose Folder";
            this.selectFolder.UseVisualStyleBackColor = true;
            this.selectFolder.Click += new System.EventHandler(this.selectFolder_Click);
            // 
            // output
            // 
            this.output.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.output.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.output.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.output.Location = new System.Drawing.Point(12, 144);
            this.output.Multiline = true;
            this.output.Name = "output";
            this.output.ReadOnly = true;
            this.output.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.output.Size = new System.Drawing.Size(705, 339);
            this.output.TabIndex = 3;
            this.output.Text = "Test String";
            // 
            // openFolderButton
            // 
            this.openFolderButton.Location = new System.Drawing.Point(368, 115);
            this.openFolderButton.Name = "openFolderButton";
            this.openFolderButton.Size = new System.Drawing.Size(349, 23);
            this.openFolderButton.TabIndex = 13;
            this.openFolderButton.Text = "Open Folder";
            this.openFolderButton.UseVisualStyleBackColor = true;
            this.openFolderButton.Click += new System.EventHandler(this.openFolderButton_Click);
            // 
            // downloadButton
            // 
            this.downloadButton.Location = new System.Drawing.Point(597, 86);
            this.downloadButton.Name = "downloadButton";
            this.downloadButton.Size = new System.Drawing.Size(120, 23);
            this.downloadButton.TabIndex = 17;
            this.downloadButton.Text = "Download";
            this.downloadButton.UseVisualStyleBackColor = true;
            this.downloadButton.Click += new System.EventHandler(this.downloadButton_Click);
            // 
            // albumUrl
            // 
            this.albumUrl.Location = new System.Drawing.Point(15, 88);
            this.albumUrl.Name = "albumUrl";
            this.albumUrl.Size = new System.Drawing.Size(579, 20);
            this.albumUrl.TabIndex = 16;
            this.albumUrl.WordWrap = false;
            this.albumUrl.KeyDown += new System.Windows.Forms.KeyEventHandler(this.albumUrl_KeyDown);
            // 
            // imageURLTextbox
            // 
            this.imageURLTextbox.Location = new System.Drawing.Point(946, 420);
            this.imageURLTextbox.Multiline = true;
            this.imageURLTextbox.Name = "imageURLTextbox";
            this.imageURLTextbox.Size = new System.Drawing.Size(390, 20);
            this.imageURLTextbox.TabIndex = 31;
            this.imageURLTextbox.Text = "Release Cover Art URL";
            this.imageURLTextbox.Visible = false;
            this.imageURLTextbox.WordWrap = false;
            // 
            // verNumLabel
            // 
            this.verNumLabel.Location = new System.Drawing.Point(159, 55);
            this.verNumLabel.Name = "verNumLabel";
            this.verNumLabel.Size = new System.Drawing.Size(63, 13);
            this.verNumLabel.TabIndex = 38;
            this.verNumLabel.Text = "#.#.#.#";
            this.verNumLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(801, 213);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 13);
            this.label1.TabIndex = 40;
            this.label1.Text = "Cover Art";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(751, 236);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 13);
            this.label2.TabIndex = 41;
            this.label2.Text = "Album Artist";
            // 
            // albumArtistTextBox
            // 
            this.albumArtistTextBox.Location = new System.Drawing.Point(754, 252);
            this.albumArtistTextBox.Name = "albumArtistTextBox";
            this.albumArtistTextBox.ReadOnly = true;
            this.albumArtistTextBox.Size = new System.Drawing.Size(150, 20);
            this.albumArtistTextBox.TabIndex = 42;
            this.albumArtistTextBox.WordWrap = false;
            // 
            // albumTextBox
            // 
            this.albumTextBox.Location = new System.Drawing.Point(754, 294);
            this.albumTextBox.Name = "albumTextBox";
            this.albumTextBox.ReadOnly = true;
            this.albumTextBox.Size = new System.Drawing.Size(150, 20);
            this.albumTextBox.TabIndex = 44;
            this.albumTextBox.WordWrap = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(751, 278);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(36, 13);
            this.label3.TabIndex = 43;
            this.label3.Text = "Album";
            // 
            // releaseDateTextBox
            // 
            this.releaseDateTextBox.Location = new System.Drawing.Point(754, 463);
            this.releaseDateTextBox.Name = "releaseDateTextBox";
            this.releaseDateTextBox.ReadOnly = true;
            this.releaseDateTextBox.Size = new System.Drawing.Size(150, 20);
            this.releaseDateTextBox.TabIndex = 46;
            this.releaseDateTextBox.WordWrap = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(751, 447);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(72, 13);
            this.label4.TabIndex = 45;
            this.label4.Text = "Release Date";
            // 
            // upcTextBox
            // 
            this.upcTextBox.Location = new System.Drawing.Point(754, 420);
            this.upcTextBox.Name = "upcTextBox";
            this.upcTextBox.ReadOnly = true;
            this.upcTextBox.Size = new System.Drawing.Size(150, 20);
            this.upcTextBox.TabIndex = 48;
            this.upcTextBox.WordWrap = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(751, 404);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(29, 13);
            this.label5.TabIndex = 47;
            this.label5.Text = "UPC";
            // 
            // albumArtPicBox
            // 
            this.albumArtPicBox.Location = new System.Drawing.Point(754, 57);
            this.albumArtPicBox.MaximumSize = new System.Drawing.Size(150, 150);
            this.albumArtPicBox.MinimumSize = new System.Drawing.Size(150, 150);
            this.albumArtPicBox.Name = "albumArtPicBox";
            this.albumArtPicBox.Size = new System.Drawing.Size(150, 150);
            this.albumArtPicBox.TabIndex = 39;
            this.albumArtPicBox.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::QobuzDownloaderX.Properties.Resources.qbdlx;
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(207, 52);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 36;
            this.pictureBox1.TabStop = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 70);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(132, 13);
            this.label6.TabIndex = 49;
            this.label6.Text = "Qobuz Album / Track Link";
            // 
            // totalTracksTextbox
            // 
            this.totalTracksTextbox.Location = new System.Drawing.Point(754, 378);
            this.totalTracksTextbox.Name = "totalTracksTextbox";
            this.totalTracksTextbox.ReadOnly = true;
            this.totalTracksTextbox.Size = new System.Drawing.Size(150, 20);
            this.totalTracksTextbox.TabIndex = 56;
            this.totalTracksTextbox.WordWrap = false;
            // 
            // totalTracksLabel
            // 
            this.totalTracksLabel.AutoSize = true;
            this.totalTracksLabel.Location = new System.Drawing.Point(751, 362);
            this.totalTracksLabel.Name = "totalTracksLabel";
            this.totalTracksLabel.Size = new System.Drawing.Size(67, 13);
            this.totalTracksLabel.TabIndex = 55;
            this.totalTracksLabel.Text = "Total Tracks";
            // 
            // getLinkTypeBG
            // 
            this.getLinkTypeBG.DoWork += new System.ComponentModel.DoWorkEventHandler(this.getLinkTypeBG_DoWork);
            // 
            // downloadAlbumBG
            // 
            this.downloadAlbumBG.DoWork += new System.ComponentModel.DoWorkEventHandler(this.downloadAlbumBG_DoWork);
            // 
            // downloadTrackBG
            // 
            this.downloadTrackBG.DoWork += new System.ComponentModel.DoWorkEventHandler(this.downloadTrackBG_DoWork);
            // 
            // downloadDiscogBG
            // 
            this.downloadDiscogBG.DoWork += new System.ComponentModel.DoWorkEventHandler(this.downloadDiscogBG_DoWork);
            // 
            // qualityTextbox
            // 
            this.qualityTextbox.Location = new System.Drawing.Point(754, 336);
            this.qualityTextbox.Name = "qualityTextbox";
            this.qualityTextbox.ReadOnly = true;
            this.qualityTextbox.Size = new System.Drawing.Size(150, 20);
            this.qualityTextbox.TabIndex = 59;
            this.qualityTextbox.WordWrap = false;
            // 
            // qualityLabel
            // 
            this.qualityLabel.AutoSize = true;
            this.qualityLabel.Location = new System.Drawing.Point(751, 320);
            this.qualityLabel.Name = "qualityLabel";
            this.qualityLabel.Size = new System.Drawing.Size(71, 13);
            this.qualityLabel.TabIndex = 58;
            this.qualityLabel.Text = "Album Quality";
            // 
            // openSearch
            // 
            this.openSearch.Location = new System.Drawing.Point(597, 57);
            this.openSearch.Name = "openSearch";
            this.openSearch.Size = new System.Drawing.Size(120, 23);
            this.openSearch.TabIndex = 60;
            this.openSearch.Text = "Open Search";
            this.openSearch.UseVisualStyleBackColor = true;
            this.openSearch.Click += new System.EventHandler(this.openSearch_Click);
            // 
            // tagsLabel
            // 
            this.tagsLabel.Location = new System.Drawing.Point(12, 501);
            this.tagsLabel.Name = "tagsLabel";
            this.tagsLabel.Size = new System.Drawing.Size(914, 23);
            this.tagsLabel.TabIndex = 61;
            this.tagsLabel.Text = "🠋 Choose which tags to save 🠋";
            this.tagsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.tagsLabel.Click += new System.EventHandler(this.tagsLabel_Click);
            // 
            // albumArtistCheckbox
            // 
            this.albumArtistCheckbox.AutoSize = true;
            this.albumArtistCheckbox.Checked = true;
            this.albumArtistCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.albumArtistCheckbox.Location = new System.Drawing.Point(325, 563);
            this.albumArtistCheckbox.Name = "albumArtistCheckbox";
            this.albumArtistCheckbox.Size = new System.Drawing.Size(81, 17);
            this.albumArtistCheckbox.TabIndex = 62;
            this.albumArtistCheckbox.Text = "Album Artist";
            this.albumArtistCheckbox.UseVisualStyleBackColor = true;
            this.albumArtistCheckbox.CheckedChanged += new System.EventHandler(this.albumArtistCheckbox_CheckedChanged);
            // 
            // artistCheckbox
            // 
            this.artistCheckbox.AutoSize = true;
            this.artistCheckbox.Checked = true;
            this.artistCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.artistCheckbox.Location = new System.Drawing.Point(325, 609);
            this.artistCheckbox.Name = "artistCheckbox";
            this.artistCheckbox.Size = new System.Drawing.Size(80, 17);
            this.artistCheckbox.TabIndex = 63;
            this.artistCheckbox.Text = "Track Artist";
            this.artistCheckbox.UseVisualStyleBackColor = true;
            this.artistCheckbox.CheckedChanged += new System.EventHandler(this.artistCheckbox_CheckedChanged);
            // 
            // trackTitleCheckbox
            // 
            this.trackTitleCheckbox.AutoSize = true;
            this.trackTitleCheckbox.Checked = true;
            this.trackTitleCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.trackTitleCheckbox.Location = new System.Drawing.Point(325, 586);
            this.trackTitleCheckbox.Name = "trackTitleCheckbox";
            this.trackTitleCheckbox.Size = new System.Drawing.Size(77, 17);
            this.trackTitleCheckbox.TabIndex = 64;
            this.trackTitleCheckbox.Text = "Track Title";
            this.trackTitleCheckbox.UseVisualStyleBackColor = true;
            this.trackTitleCheckbox.CheckedChanged += new System.EventHandler(this.trackTitleCheckbox_CheckedChanged);
            // 
            // trackNumberCheckbox
            // 
            this.trackNumberCheckbox.AutoSize = true;
            this.trackNumberCheckbox.Checked = true;
            this.trackNumberCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.trackNumberCheckbox.Location = new System.Drawing.Point(325, 632);
            this.trackNumberCheckbox.Name = "trackNumberCheckbox";
            this.trackNumberCheckbox.Size = new System.Drawing.Size(94, 17);
            this.trackNumberCheckbox.TabIndex = 65;
            this.trackNumberCheckbox.Text = "Track Number";
            this.trackNumberCheckbox.UseVisualStyleBackColor = true;
            this.trackNumberCheckbox.CheckedChanged += new System.EventHandler(this.trackNumberCheckbox_CheckedChanged);
            // 
            // trackTotalCheckbox
            // 
            this.trackTotalCheckbox.AutoSize = true;
            this.trackTotalCheckbox.Checked = true;
            this.trackTotalCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.trackTotalCheckbox.Location = new System.Drawing.Point(325, 655);
            this.trackTotalCheckbox.Name = "trackTotalCheckbox";
            this.trackTotalCheckbox.Size = new System.Drawing.Size(86, 17);
            this.trackTotalCheckbox.TabIndex = 66;
            this.trackTotalCheckbox.Text = "Total Tracks";
            this.trackTotalCheckbox.UseVisualStyleBackColor = true;
            this.trackTotalCheckbox.CheckedChanged += new System.EventHandler(this.trackTotalCheckbox_CheckedChanged);
            // 
            // discNumberCheckbox
            // 
            this.discNumberCheckbox.AutoSize = true;
            this.discNumberCheckbox.Checked = true;
            this.discNumberCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.discNumberCheckbox.Location = new System.Drawing.Point(325, 676);
            this.discNumberCheckbox.Name = "discNumberCheckbox";
            this.discNumberCheckbox.Size = new System.Drawing.Size(87, 17);
            this.discNumberCheckbox.TabIndex = 67;
            this.discNumberCheckbox.Text = "Disc Number";
            this.discNumberCheckbox.UseVisualStyleBackColor = true;
            this.discNumberCheckbox.CheckedChanged += new System.EventHandler(this.discNumberCheckbox_CheckedChanged);
            // 
            // discTotalCheckbox
            // 
            this.discTotalCheckbox.AutoSize = true;
            this.discTotalCheckbox.Checked = true;
            this.discTotalCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.discTotalCheckbox.Location = new System.Drawing.Point(325, 699);
            this.discTotalCheckbox.Name = "discTotalCheckbox";
            this.discTotalCheckbox.Size = new System.Drawing.Size(79, 17);
            this.discTotalCheckbox.TabIndex = 68;
            this.discTotalCheckbox.Text = "Total Discs";
            this.discTotalCheckbox.UseVisualStyleBackColor = true;
            this.discTotalCheckbox.CheckedChanged += new System.EventHandler(this.discTotalCheckbox_CheckedChanged);
            // 
            // albumCheckbox
            // 
            this.albumCheckbox.AutoSize = true;
            this.albumCheckbox.Checked = true;
            this.albumCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.albumCheckbox.Location = new System.Drawing.Point(325, 540);
            this.albumCheckbox.Name = "albumCheckbox";
            this.albumCheckbox.Size = new System.Drawing.Size(78, 17);
            this.albumCheckbox.TabIndex = 69;
            this.albumCheckbox.Text = "Album Title";
            this.albumCheckbox.UseVisualStyleBackColor = true;
            this.albumCheckbox.CheckedChanged += new System.EventHandler(this.albumCheckbox_CheckedChanged);
            // 
            // explicitCheckbox
            // 
            this.explicitCheckbox.AutoSize = true;
            this.explicitCheckbox.Checked = true;
            this.explicitCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.explicitCheckbox.Location = new System.Drawing.Point(425, 678);
            this.explicitCheckbox.Name = "explicitCheckbox";
            this.explicitCheckbox.Size = new System.Drawing.Size(106, 17);
            this.explicitCheckbox.TabIndex = 76;
            this.explicitCheckbox.Text = "Explicit Advisory*";
            this.explicitCheckbox.UseVisualStyleBackColor = true;
            this.explicitCheckbox.CheckedChanged += new System.EventHandler(this.explicitCheckbox_CheckedChanged);
            // 
            // upcCheckbox
            // 
            this.upcCheckbox.AutoSize = true;
            this.upcCheckbox.Checked = true;
            this.upcCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.upcCheckbox.Location = new System.Drawing.Point(425, 655);
            this.upcCheckbox.Name = "upcCheckbox";
            this.upcCheckbox.Size = new System.Drawing.Size(52, 17);
            this.upcCheckbox.TabIndex = 75;
            this.upcCheckbox.Text = "UPC*";
            this.upcCheckbox.UseVisualStyleBackColor = true;
            this.upcCheckbox.CheckedChanged += new System.EventHandler(this.upcCheckbox_CheckedChanged);
            // 
            // isrcCheckbox
            // 
            this.isrcCheckbox.AutoSize = true;
            this.isrcCheckbox.Checked = true;
            this.isrcCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.isrcCheckbox.Location = new System.Drawing.Point(425, 632);
            this.isrcCheckbox.Name = "isrcCheckbox";
            this.isrcCheckbox.Size = new System.Drawing.Size(51, 17);
            this.isrcCheckbox.TabIndex = 74;
            this.isrcCheckbox.Text = "ISRC";
            this.isrcCheckbox.UseVisualStyleBackColor = true;
            this.isrcCheckbox.CheckedChanged += new System.EventHandler(this.isrcCheckbox_CheckedChanged);
            // 
            // copyrightCheckbox
            // 
            this.copyrightCheckbox.AutoSize = true;
            this.copyrightCheckbox.Checked = true;
            this.copyrightCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.copyrightCheckbox.Location = new System.Drawing.Point(425, 609);
            this.copyrightCheckbox.Name = "copyrightCheckbox";
            this.copyrightCheckbox.Size = new System.Drawing.Size(70, 17);
            this.copyrightCheckbox.TabIndex = 73;
            this.copyrightCheckbox.Text = "Copyright";
            this.copyrightCheckbox.UseVisualStyleBackColor = true;
            this.copyrightCheckbox.CheckedChanged += new System.EventHandler(this.copyrightCheckbox_CheckedChanged);
            // 
            // composerCheckbox
            // 
            this.composerCheckbox.AutoSize = true;
            this.composerCheckbox.Checked = true;
            this.composerCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.composerCheckbox.Location = new System.Drawing.Point(425, 586);
            this.composerCheckbox.Name = "composerCheckbox";
            this.composerCheckbox.Size = new System.Drawing.Size(73, 17);
            this.composerCheckbox.TabIndex = 72;
            this.composerCheckbox.Text = "Composer";
            this.composerCheckbox.UseVisualStyleBackColor = true;
            this.composerCheckbox.CheckedChanged += new System.EventHandler(this.composerCheckbox_CheckedChanged);
            // 
            // genreCheckbox
            // 
            this.genreCheckbox.AutoSize = true;
            this.genreCheckbox.Checked = true;
            this.genreCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.genreCheckbox.Location = new System.Drawing.Point(425, 563);
            this.genreCheckbox.Name = "genreCheckbox";
            this.genreCheckbox.Size = new System.Drawing.Size(55, 17);
            this.genreCheckbox.TabIndex = 71;
            this.genreCheckbox.Text = "Genre";
            this.genreCheckbox.UseVisualStyleBackColor = true;
            this.genreCheckbox.CheckedChanged += new System.EventHandler(this.genreCheckbox_CheckedChanged);
            // 
            // releaseCheckbox
            // 
            this.releaseCheckbox.AutoSize = true;
            this.releaseCheckbox.Checked = true;
            this.releaseCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.releaseCheckbox.Location = new System.Drawing.Point(425, 540);
            this.releaseCheckbox.Name = "releaseCheckbox";
            this.releaseCheckbox.Size = new System.Drawing.Size(91, 17);
            this.releaseCheckbox.TabIndex = 70;
            this.releaseCheckbox.Text = "Release Date";
            this.releaseCheckbox.UseVisualStyleBackColor = true;
            this.releaseCheckbox.CheckedChanged += new System.EventHandler(this.releaseCheckbox_CheckedChanged);
            // 
            // commentCheckbox
            // 
            this.commentCheckbox.AutoSize = true;
            this.commentCheckbox.Location = new System.Drawing.Point(533, 699);
            this.commentCheckbox.Name = "commentCheckbox";
            this.commentCheckbox.Size = new System.Drawing.Size(70, 17);
            this.commentCheckbox.TabIndex = 78;
            this.commentCheckbox.Text = "Comment";
            this.commentCheckbox.UseVisualStyleBackColor = true;
            this.commentCheckbox.CheckedChanged += new System.EventHandler(this.commentCheckbox_CheckedChanged);
            // 
            // commentTextbox
            // 
            this.commentTextbox.Location = new System.Drawing.Point(609, 697);
            this.commentTextbox.Name = "commentTextbox";
            this.commentTextbox.Size = new System.Drawing.Size(112, 20);
            this.commentTextbox.TabIndex = 79;
            this.commentTextbox.TextChanged += new System.EventHandler(this.commentTextbox_TextChanged);
            // 
            // imageCheckbox
            // 
            this.imageCheckbox.AutoSize = true;
            this.imageCheckbox.Checked = true;
            this.imageCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.imageCheckbox.Location = new System.Drawing.Point(425, 699);
            this.imageCheckbox.Name = "imageCheckbox";
            this.imageCheckbox.Size = new System.Drawing.Size(70, 17);
            this.imageCheckbox.TabIndex = 80;
            this.imageCheckbox.Text = "Cover Art";
            this.imageCheckbox.UseVisualStyleBackColor = true;
            this.imageCheckbox.CheckedChanged += new System.EventHandler(this.imageCheckbox_CheckedChanged);
            // 
            // mp3Checkbox
            // 
            this.mp3Checkbox.AutoSize = true;
            this.mp3Checkbox.Location = new System.Drawing.Point(243, 61);
            this.mp3Checkbox.Name = "mp3Checkbox";
            this.mp3Checkbox.Size = new System.Drawing.Size(69, 17);
            this.mp3Checkbox.TabIndex = 81;
            this.mp3Checkbox.Text = "MP3 320";
            this.mp3Checkbox.UseVisualStyleBackColor = true;
            this.mp3Checkbox.CheckedChanged += new System.EventHandler(this.mp3Checkbox_CheckedChanged);
            // 
            // flacLowCheckbox
            // 
            this.flacLowCheckbox.AutoSize = true;
            this.flacLowCheckbox.Location = new System.Drawing.Point(318, 61);
            this.flacLowCheckbox.Name = "flacLowCheckbox";
            this.flacLowCheckbox.Size = new System.Drawing.Size(93, 17);
            this.flacLowCheckbox.TabIndex = 82;
            this.flacLowCheckbox.Text = "FLAC 16/44.1";
            this.flacLowCheckbox.UseVisualStyleBackColor = true;
            this.flacLowCheckbox.CheckedChanged += new System.EventHandler(this.flacLowCheckbox_CheckedChanged);
            // 
            // flacMidCheckbox
            // 
            this.flacMidCheckbox.AutoSize = true;
            this.flacMidCheckbox.Location = new System.Drawing.Point(417, 61);
            this.flacMidCheckbox.Name = "flacMidCheckbox";
            this.flacMidCheckbox.Size = new System.Drawing.Size(84, 17);
            this.flacMidCheckbox.TabIndex = 83;
            this.flacMidCheckbox.Text = "FLAC 24/96";
            this.flacMidCheckbox.UseVisualStyleBackColor = true;
            this.flacMidCheckbox.CheckedChanged += new System.EventHandler(this.flacMidCheckbox_CheckedChanged);
            // 
            // flacHighCheckbox
            // 
            this.flacHighCheckbox.AutoSize = true;
            this.flacHighCheckbox.Checked = true;
            this.flacHighCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.flacHighCheckbox.Location = new System.Drawing.Point(507, 61);
            this.flacHighCheckbox.Name = "flacHighCheckbox";
            this.flacHighCheckbox.Size = new System.Drawing.Size(90, 17);
            this.flacHighCheckbox.TabIndex = 84;
            this.flacHighCheckbox.Text = "FLAC 24/192";
            this.flacHighCheckbox.UseVisualStyleBackColor = true;
            this.flacHighCheckbox.CheckedChanged += new System.EventHandler(this.flacHighCheckbox_CheckedChanged);
            // 
            // mp3WarnLabel
            // 
            this.mp3WarnLabel.AutoSize = true;
            this.mp3WarnLabel.Location = new System.Drawing.Point(744, 711);
            this.mp3WarnLabel.Name = "mp3WarnLabel";
            this.mp3WarnLabel.Size = new System.Drawing.Size(182, 13);
            this.mp3WarnLabel.TabIndex = 85;
            this.mp3WarnLabel.Text = "* = Not available on MP3 downloads.";
            // 
            // QobuzDownloaderX
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(938, 533);
            this.Controls.Add(this.mp3WarnLabel);
            this.Controls.Add(this.flacHighCheckbox);
            this.Controls.Add(this.flacMidCheckbox);
            this.Controls.Add(this.flacLowCheckbox);
            this.Controls.Add(this.mp3Checkbox);
            this.Controls.Add(this.imageCheckbox);
            this.Controls.Add(this.commentTextbox);
            this.Controls.Add(this.commentCheckbox);
            this.Controls.Add(this.explicitCheckbox);
            this.Controls.Add(this.upcCheckbox);
            this.Controls.Add(this.isrcCheckbox);
            this.Controls.Add(this.copyrightCheckbox);
            this.Controls.Add(this.composerCheckbox);
            this.Controls.Add(this.genreCheckbox);
            this.Controls.Add(this.releaseCheckbox);
            this.Controls.Add(this.albumCheckbox);
            this.Controls.Add(this.discTotalCheckbox);
            this.Controls.Add(this.discNumberCheckbox);
            this.Controls.Add(this.trackTotalCheckbox);
            this.Controls.Add(this.trackNumberCheckbox);
            this.Controls.Add(this.trackTitleCheckbox);
            this.Controls.Add(this.artistCheckbox);
            this.Controls.Add(this.albumArtistCheckbox);
            this.Controls.Add(this.tagsLabel);
            this.Controls.Add(this.openSearch);
            this.Controls.Add(this.qualityTextbox);
            this.Controls.Add(this.qualityLabel);
            this.Controls.Add(this.totalTracksTextbox);
            this.Controls.Add(this.totalTracksLabel);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.upcTextBox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.releaseDateTextBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.albumTextBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.albumArtistTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.albumArtPicBox);
            this.Controls.Add(this.verNumLabel);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.imageURLTextbox);
            this.Controls.Add(this.downloadButton);
            this.Controls.Add(this.albumUrl);
            this.Controls.Add(this.openFolderButton);
            this.Controls.Add(this.output);
            this.Controls.Add(this.selectFolder);
            this.Controls.Add(this.testURLBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "QobuzDownloaderX";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "QobuzDownloaderX";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.QobuzDownloaderX_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.albumArtPicBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox testURLBox;
        private System.Windows.Forms.Button selectFolder;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.TextBox output;
        private System.ComponentModel.BackgroundWorker downloadBG;
        private System.Windows.Forms.Button openFolderButton;
        private System.Windows.Forms.Button downloadButton;
        private System.Windows.Forms.TextBox albumUrl;
        private System.Windows.Forms.TextBox imageURLTextbox;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label verNumLabel;
        private System.Windows.Forms.PictureBox albumArtPicBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox albumArtistTextBox;
        private System.Windows.Forms.TextBox albumTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox releaseDateTextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox upcTextBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox totalTracksTextbox;
        private System.Windows.Forms.Label totalTracksLabel;
        private System.ComponentModel.BackgroundWorker getLinkTypeBG;
        private System.ComponentModel.BackgroundWorker downloadAlbumBG;
        private System.ComponentModel.BackgroundWorker downloadTrackBG;
        private System.ComponentModel.BackgroundWorker downloadDiscogBG;
        private System.Windows.Forms.TextBox qualityTextbox;
        private System.Windows.Forms.Label qualityLabel;
        private System.Windows.Forms.Button openSearch;
        private System.Windows.Forms.Label tagsLabel;
        private System.Windows.Forms.CheckBox albumArtistCheckbox;
        private System.Windows.Forms.CheckBox artistCheckbox;
        private System.Windows.Forms.CheckBox trackTitleCheckbox;
        private System.Windows.Forms.CheckBox trackNumberCheckbox;
        private System.Windows.Forms.CheckBox trackTotalCheckbox;
        private System.Windows.Forms.CheckBox discNumberCheckbox;
        private System.Windows.Forms.CheckBox discTotalCheckbox;
        private System.Windows.Forms.CheckBox albumCheckbox;
        private System.Windows.Forms.CheckBox explicitCheckbox;
        private System.Windows.Forms.CheckBox upcCheckbox;
        private System.Windows.Forms.CheckBox isrcCheckbox;
        private System.Windows.Forms.CheckBox copyrightCheckbox;
        private System.Windows.Forms.CheckBox composerCheckbox;
        private System.Windows.Forms.CheckBox genreCheckbox;
        private System.Windows.Forms.CheckBox releaseCheckbox;
        private System.Windows.Forms.CheckBox commentCheckbox;
        private System.Windows.Forms.TextBox commentTextbox;
        private System.Windows.Forms.CheckBox imageCheckbox;
        private System.Windows.Forms.CheckBox mp3Checkbox;
        private System.Windows.Forms.CheckBox flacLowCheckbox;
        private System.Windows.Forms.CheckBox flacMidCheckbox;
        private System.Windows.Forms.CheckBox flacHighCheckbox;
        private System.Windows.Forms.Label mp3WarnLabel;
    }
}

