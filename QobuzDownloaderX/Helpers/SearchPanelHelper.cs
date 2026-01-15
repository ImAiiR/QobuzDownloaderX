using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using QopenAPI;

using QobuzDownloaderX.Properties;
using QobuzDownloaderX.Win32;

namespace QobuzDownloaderX.Helpers
{
    internal sealed class RowInfo
    {
        internal int RowIndex { get; set; }
        internal bool Selected { get; set; }
        internal string AlbumOrTrackUrl { get; set; }
    }

    internal sealed class SearchPanelHelper
    {
        private static int? lastAnchorRowIndex = null;
        private static Tuple<int, int> lastShiftRange = null;

        // Stores the first GET button created during table population
        private Button firstGetButton = null;

        public static readonly List<int> selectedRowindices = new List<int>();

        private readonly Service QoService = new Service();
        internal User QoUser = new User();
        internal Item QoItem = new Item();
        internal SearchAlbumResult QoAlbumSearch = new SearchAlbumResult();
        internal SearchTrackResult QoTrackSearch = new SearchTrackResult();
        internal string lastSearchType = "";
        private int limitResults;

        public void SearchInitiate(string searchType, string app_id, string searchQuery, string user_auth_token)
        {
            limitResults = (int)qbdlxForm._qbdlxForm.limitSearchResultsNumericUpDown.Value;
            qbdlxForm._qbdlxForm.Invoke(new Action(() => qbdlxForm._qbdlxForm.searchResultsCountLabel.Text = "…"));

            if (searchType == "releases")
            {
                QoAlbumSearch = null;
                QoAlbumSearch = QoService.SearchAlbumsWithAuth(app_id, user_auth_token, searchQuery, limitResults, 0);
                QoAlbumSearch.Albums = SortAlbums(QoAlbumSearch.Albums);
                PopulateTableAlbums(qbdlxForm._qbdlxForm, QoAlbumSearch);
                qbdlxForm._qbdlxForm.Invoke(new Action(() => qbdlxForm._qbdlxForm.searchResultsCountLabel.Text = $"{QoAlbumSearch.Albums.Items.Count:N0} {qbdlxForm._qbdlxForm.languageManager.GetTranslation("searchResultsCountLabel")}"));
            }
            else if (searchType == "tracks")
            {
                QoTrackSearch = null;
                QoTrackSearch = QoService.SearchTracksWithAuth(app_id, user_auth_token, searchQuery, limitResults, 0);
                QoTrackSearch.Tracks = SortTracks(QoTrackSearch.Tracks);
                PopulateTableTracks(qbdlxForm._qbdlxForm, QoTrackSearch);
                qbdlxForm._qbdlxForm.Invoke(new Action(() => qbdlxForm._qbdlxForm.searchResultsCountLabel.Text = $"{QoTrackSearch.Tracks.Items.Count:N0} {qbdlxForm._qbdlxForm.languageManager.GetTranslation("searchResultsCountLabel")}"));
            }
        }

        public void PopulateTableAlbums(qbdlxForm mainForm, SearchAlbumResult QoAlbumSearch)
        {
            // Access the "items" array from the response
            var albums = QoAlbumSearch.Albums.Items;

            // Load the font name from the translation file
            string fontName = qbdlxForm._qbdlxForm.languageManager.GetTranslation("TranslationFont");

            string downloadButtonText = qbdlxForm._qbdlxForm.languageManager.GetTranslation("downloadButton");

            mainForm.Invoke((MethodInvoker)delegate ()
            {
                qbdlxForm._qbdlxForm.searchSortingPanel.Enabled = false;

                TableLayoutPanel searchResultsTablePanel = mainForm.searchResultsTablePanel;
                searchResultsTablePanel.SuspendLayout();
                NativeMethods.SendMessage(searchResultsTablePanel.Handle, Constants.WM_SETREDRAW, IntPtr.Zero, IntPtr.Zero);

                // Restore related selection row controls
                lastAnchorRowIndex = 0;
                lastShiftRange = null;
                selectedRowindices.Clear();
                qbdlxForm._qbdlxForm.selectedRowsCountLabel.Text =
                    string.Format(qbdlxForm._qbdlxForm.languageManager.GetTranslation("selectedRowsCountLabel"),
                                  selectedRowindices.Count);
                qbdlxForm._qbdlxForm.selectAllRowsButton.Enabled = false;
                qbdlxForm._qbdlxForm.deselectAllRowsButton.Enabled = false;
                qbdlxForm._qbdlxForm.batchDownloadSelectedRowsButton.Enabled = false;

                // Clear previous controls
                DisposeAllControls(searchResultsTablePanel);

                searchResultsTablePanel.ColumnCount = 5; // Artwork, Artist name, Album title, Quality and Genre, "GET" Button
                searchResultsTablePanel.RowCount = limitResults;
                searchResultsTablePanel.AutoSize = true;

                // Set ColumnStyles to define the size of each column
                searchResultsTablePanel.ColumnStyles.Clear();
                searchResultsTablePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 70F));  // Artwork
                searchResultsTablePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 200F)); // Artist name
                searchResultsTablePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 200F)); // Album title
                searchResultsTablePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 140F)); // Quality and Genre
                searchResultsTablePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 70F));  // "GET" Button

                firstGetButton = null;
                int rowIndex = 0;

                foreach (var album in albums)
                {
                    int currentRowIndex = rowIndex; // capture the current row index

                    // Create the row panel
                    Panel rowPanel = new Panel
                    {
                        Dock = DockStyle.Fill,
                        BackColor = Color.Transparent,
                        Cursor = Cursors.Default,
                        Tag = new RowInfo { RowIndex = rowIndex, Selected = false },
                        Margin = new Padding(0, 1, 0, 0),
                        TabStop = false
                    };

                    // Create a inner table for proper column alignment
                    TableLayoutPanel innerRow = CreateSearchResultRow();

                    // Add PictureBox for artwork
                    PictureBox artwork = new PictureBox
                    {
                        SizeMode = PictureBoxSizeMode.StretchImage
                    };
                    try { artwork.Load(album.Image.Large.ToString()); /* Using the thumbnail URL */ } catch { artwork.Image = Resources.qbdlx_new; /* Use QBDLX Icon as fallback */ }
                    artwork.Width = 65;
                    artwork.Height = 65;
                    artwork.Anchor = AnchorStyles.None; // Center both horizontally and vertically
                    artwork.Cursor = Cursors.Hand;
                    innerRow.Controls.Add(artwork, 0, 0);

                    artwork.MouseClick += (s, e) =>
                    {
                        if (e.Button != MouseButtons.Left)
                            return;

                        artwork.Focus();
                        Miscellaneous.ShowFloatingImageFromUrl(album.Image?.Large);
                        qbdlxForm._qbdlxForm.BringToFront();
                    };

                    // Add Label for artist name
                    System.Windows.Forms.Label artistName = new System.Windows.Forms.Label
                    {
                        Text = album.Artist.Name.Replace(@"&", @"&&").Trim(),
                        AutoSize = true, // Disable auto-sizing to allow wrapping
                        /*artistName.MaximumSize = new Size(0, 0);*/ // Word-wrap if needed
                        TextAlign = ContentAlignment.MiddleCenter, // Center text horizontally and vertically
                        Anchor = AnchorStyles.None, // Center within the cell
                        ForeColor = ColorTranslator.FromHtml(qbdlxForm._qbdlxForm._themeManager._currentTheme.LabelText), // Set text color
                        Font = new Font(fontName, 10F, FontStyle.Regular), // Set font size and style
                    };
                    innerRow.Controls.Add(artistName, 1, 0);

                    // Add Label for album title
                    System.Windows.Forms.Label albumTitle = new System.Windows.Forms.Label
                    {
                        Text = album.Title.Replace(@"&", @"&&").Trim()
                    };
                    if (album.Version != null) { albumTitle.Text = albumTitle.Text + " (" + album.Version + ")"; }
                    if (album.ParentalWarning == true) { albumTitle.Text = "[E] " + albumTitle.Text; } // Add "[E]" if Qobuz lists the release with a parental warning
                    albumTitle.AutoSize = true;
                    /*albumTitle.MaximumSize = new Size(0, 0);*/ // Allow word-wrap
                    albumTitle.TextAlign = ContentAlignment.MiddleCenter; // Center text horizontally and vertically
                    albumTitle.Anchor = AnchorStyles.None; // Center within the cell
                    albumTitle.ForeColor = ColorTranslator.FromHtml(qbdlxForm._qbdlxForm._themeManager._currentTheme.LabelText); // Set text color
                    albumTitle.Font = new Font(fontName, 10F, FontStyle.Regular); // Set font size and style
                    innerRow.Controls.Add(albumTitle, 2, 0);

                    // Add Label for quality and other info
                    System.Windows.Forms.Label qualityLabel = new System.Windows.Forms.Label
                    {
                        Text = GetQualityInfoAlbumLabelText(album),
                        ForeColor = ColorTranslator.FromHtml(qbdlxForm._qbdlxForm._themeManager._currentTheme.LabelText),
                        AutoSize = true,
                        MaximumSize = new Size(0, 0), // Allow word-wrap
                        TextAlign = ContentAlignment.TopCenter, // Center text horizontally and vertically
                        Anchor = AnchorStyles.None // Center within the cell
                    };

                    // Set text color
                    if (qualityLabel.Text.Contains("24bit"))
                    {
                        qualityLabel.ForeColor = ColorTranslator.FromHtml(qbdlxForm._qbdlxForm._themeManager._currentTheme.HiResLabelText);
                    }
                    else
                    {
                        qualityLabel.ForeColor = ColorTranslator.FromHtml(qbdlxForm._qbdlxForm._themeManager._currentTheme.LabelText); ;
                    }

                    qualityLabel.Font = new Font(fontName, 10F, FontStyle.Regular); // Set font size and style
                    innerRow.Controls.Add(qualityLabel, 3, 0);

                    // Add Button for selecting album ID
                    Button selectButton = new Button
                    {
                        Text = downloadButtonText,
                        ForeColor = ColorTranslator.FromHtml(qbdlxForm._qbdlxForm._themeManager._currentTheme.ButtonText), // Set button text color
                        BackColor = ColorTranslator.FromHtml(qbdlxForm._qbdlxForm._themeManager._currentTheme.ButtonBackground), // Set button background color
                        Font = new Font(fontName, 8F, FontStyle.Regular), // Set font size and style
                        FlatStyle = FlatStyle.Flat // Set FlatStyle to Flat
                    };
                    selectButton.FlatAppearance.BorderSize = 0;  // Set border size
                    selectButton.FlatAppearance.MouseOverBackColor = ColorTranslator.FromHtml(qbdlxForm._qbdlxForm._themeManager._currentTheme.HighlightedButtonBackground); // Set background color when hovering
                    selectButton.FlatAppearance.MouseDownBackColor = ColorTranslator.FromHtml(qbdlxForm._qbdlxForm._themeManager._currentTheme.ClickedButtonBackground); // Set background color when clicked
                    string albumLink = "https://play.qobuz.com/album/" + album.Id.ToString(); // Store the album link
                    selectButton.Tag = albumLink;
                    selectButton.Anchor = AnchorStyles.None; // Center the button
                    selectButton.Enabled = !qbdlxForm.getLinkTypeIsBusy;
                    selectButton.Click += (sender, e) => SendURL(mainForm, albumLink);
                    innerRow.Controls.Add(selectButton, 4, 0);
                    if (firstGetButton == null)
                    {
                        firstGetButton = selectButton;
                    }

                    void downloadButtonHandler(object s, EventArgs e)
                    {
                        selectButton.Enabled =
                            mainForm.downloadButton.Enabled ||
                            !qbdlxForm.getLinkTypeIsBusy ||
                            string.IsNullOrWhiteSpace(mainForm.inputTextBox.Text);
                    }
                    mainForm.downloadButton.EnabledChanged += downloadButtonHandler;

                    // Add inner table to row panel
                    rowPanel.Controls.Add(innerRow);

                    AttachRowHighlightPaint(rowPanel);

                    AttachClickRecursive(rowPanel, (s, e) =>
                    {
                        RowClickHandler(
                            s as Control,
                            searchResultsTablePanel,
                            selectedRowindices,
                            qbdlxForm._qbdlxForm);
                    });

                    rowPanel.Disposed += (s, e) =>
                    {
                        mainForm.downloadButton.EnabledChanged -= downloadButtonHandler;
                    };

                    // Add rowPanel to main table
                    searchResultsTablePanel.Controls.Add(rowPanel, 0, rowIndex);
                    searchResultsTablePanel.SetColumnSpan(rowPanel, 5);

                    rowIndex++;
                }
                NativeMethods.SendMessage(searchResultsTablePanel.Handle, Constants.WM_SETREDRAW, (IntPtr)1, IntPtr.Zero);
                searchResultsTablePanel.ResumeLayout();
                qbdlxForm._qbdlxForm.searchResultsPanel.AutoScrollPosition = new Point(0, 0);
                qbdlxForm._qbdlxForm.searchSortingPanel.Enabled = rowIndex > 0;
                qbdlxForm._qbdlxForm.selectAllRowsButton.Enabled = rowIndex > 0;

                if (firstGetButton != null)
                {
                    mainForm.BeginInvoke((Action)(() =>
                    {
                        mainForm.ActiveControl = firstGetButton;
                        firstGetButton.Focus();
                    }));
                }
            });

            lastSearchType = "releases";
        }

        public void PopulateTableTracks(qbdlxForm mainForm, SearchTrackResult QoTrackSearch)
        {
            // Access the "items" array from the response
            var tracks = QoTrackSearch.Tracks.Items;

            // Load the font name from the translation file
            string fontName = qbdlxForm._qbdlxForm.languageManager.GetTranslation("TranslationFont");

            string downloadButtonText = qbdlxForm._qbdlxForm.languageManager.GetTranslation("downloadButton");

            mainForm.Invoke((MethodInvoker)delegate ()
            {
                qbdlxForm._qbdlxForm.searchSortingPanel.Enabled = false;

                TableLayoutPanel searchResultsTablePanel = mainForm.searchResultsTablePanel;
                searchResultsTablePanel.SuspendLayout();
                NativeMethods.SendMessage(searchResultsTablePanel.Handle, Constants.WM_SETREDRAW, IntPtr.Zero, IntPtr.Zero);

                // Restore related selection row controls
                lastAnchorRowIndex = 0;
                lastShiftRange = null;
                selectedRowindices.Clear();
                qbdlxForm._qbdlxForm.selectedRowsCountLabel.Text =
                    string.Format(qbdlxForm._qbdlxForm.languageManager.GetTranslation("selectedRowsCountLabel"),
                                  selectedRowindices.Count);
                qbdlxForm._qbdlxForm.selectAllRowsButton.Enabled = false;
                qbdlxForm._qbdlxForm.deselectAllRowsButton.Enabled = false;
                qbdlxForm._qbdlxForm.batchDownloadSelectedRowsButton.Enabled = false;

                // Clear previous controls
                DisposeAllControls(searchResultsTablePanel);

                searchResultsTablePanel.ColumnCount = 5; // Artwork, Artist, Track title, Quality, "GET" button
                searchResultsTablePanel.RowCount = limitResults;
                searchResultsTablePanel.AutoSize = true;

                // Set ColumnStyles to define the size of each column
                searchResultsTablePanel.ColumnStyles.Clear();
                searchResultsTablePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 70F));  // Artwork
                searchResultsTablePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 200F)); // Artist name
                searchResultsTablePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 200F)); // Track title
                searchResultsTablePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 140F)); // Quality
                searchResultsTablePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 70F));  // "GET" Button

                firstGetButton = null;
                int rowIndex = 0;

                foreach (var track in tracks)
                {
                    int currentRowIndex = rowIndex; // capture the current row index

                    // Create the row panel
                    Panel rowPanel = new Panel
                    {
                        Dock = DockStyle.Fill,
                        BackColor = Color.Transparent,
                        Cursor = Cursors.Default,
                        Tag = new RowInfo { RowIndex = rowIndex, Selected = false },
                        Margin = new Padding(0, 1, 0, 0),
                        TabStop = false
                    };

                    // Create a inner table for proper column alignment
                    TableLayoutPanel innerRow = CreateSearchResultRow();

                    // Add PictureBox for artwork
                    PictureBox artwork = new PictureBox
                    {
                        SizeMode = PictureBoxSizeMode.StretchImage
                    };
                    try { artwork.Load(track.Album.Image.Large.ToString()); /* Using the thumbnail URL */ }
                    catch { artwork.Image = Resources.qbdlx_new; /* Use QBDLX Icon as fallback */ }
                    artwork.Width = 65;
                    artwork.Height = 65;
                    artwork.Anchor = AnchorStyles.None; // Center both horizontally and vertically
                    artwork.Cursor = Cursors.Hand;
                    innerRow.Controls.Add(artwork, 0, 0);

                    artwork.MouseClick += (s, e) =>
                    {
                        if (e.Button != MouseButtons.Left)
                            return;

                        artwork.Focus();
                        Miscellaneous.ShowFloatingImageFromUrl(track.Album?.Image?.Large);
                        qbdlxForm._qbdlxForm.BringToFront();
                    };

                    // Add Label for artist name
                    System.Windows.Forms.Label artistName = new System.Windows.Forms.Label
                    {
                        Text = track.Performer.Name.Replace(@"&", @"&&").Trim(),
                        AutoSize = true, // Disable auto-sizing to allow wrapping
                        /*artistName.MaximumSize = new Size(0, 0);*/ // Word-wrap if needed
                        TextAlign = ContentAlignment.MiddleCenter, // Center text horizontally and vertically
                        Anchor = AnchorStyles.None, // Center within the cell
                        ForeColor = ColorTranslator.FromHtml(qbdlxForm._qbdlxForm._themeManager._currentTheme.LabelText), // Set text color
                        Font = new Font(fontName, 10F, FontStyle.Regular) // Set font size and style
                    };
                    innerRow.Controls.Add(artistName, 1, 0);

                    // Add Label for track title
                    System.Windows.Forms.Label trackTitle = new System.Windows.Forms.Label
                    {
                        Text = track.Title.Replace(@"&", @"&&").Trim()
                    };
                    if (track.Version != null) { trackTitle.Text = trackTitle.Text + " (" + track.Version + ")"; }
                    if (track.ParentalWarning == true) { trackTitle.Text = "[E] " + trackTitle.Text; } // Add "[E]" if Qobuz lists the track with a parental warning
                    trackTitle.AutoSize = true;
                    /*trackTitle.MaximumSize = new Size(0, 0);*/ // Allow word-wrap
                    trackTitle.TextAlign = ContentAlignment.MiddleCenter; // Center text horizontally and vertically
                    trackTitle.Anchor = AnchorStyles.None; // Center within the cell
                    trackTitle.ForeColor = ColorTranslator.FromHtml(qbdlxForm._qbdlxForm._themeManager._currentTheme.LabelText); // Set text color
                    trackTitle.Font = new Font(fontName, 10F, FontStyle.Regular); // Set font size and style
                    innerRow.Controls.Add(trackTitle, 2, 0);

                    // Add Label for quality
                    System.Windows.Forms.Label qualityLabel = new System.Windows.Forms.Label
                    {
                        Text = GetQualityInfoTrackLabelText(track),
                        AutoSize = true,
                        MaximumSize = new Size(0, 0), // Allow word-wrap
                        TextAlign = ContentAlignment.MiddleCenter, // Center text horizontally and vertically
                        Anchor = AnchorStyles.None // Center within the cell
                    };

                    // Set text color
                    if (qualityLabel.Text.Contains("24bit"))
                    {
                        qualityLabel.ForeColor = ColorTranslator.FromHtml(qbdlxForm._qbdlxForm._themeManager._currentTheme.HiResLabelText);
                    }
                    else
                    {
                        qualityLabel.ForeColor = ColorTranslator.FromHtml(qbdlxForm._qbdlxForm._themeManager._currentTheme.LabelText);
                    }

                    qualityLabel.Font = new Font(fontName, 10F, FontStyle.Regular); // Set font size and style
                    innerRow.Controls.Add(qualityLabel, 3, 0);

                    // Add Button for selecting track ID
                    Button selectButton = new Button
                    {
                        Text = downloadButtonText,
                        ForeColor = ColorTranslator.FromHtml(qbdlxForm._qbdlxForm._themeManager._currentTheme.ButtonText), // Set button text color
                        BackColor = ColorTranslator.FromHtml(qbdlxForm._qbdlxForm._themeManager._currentTheme.ButtonBackground), // Set button background color
                        Font = new Font(fontName, 8F, FontStyle.Regular), // Set font size and style
                        FlatStyle = FlatStyle.Flat // Set FlatStyle to Flat
                    };
                    selectButton.FlatAppearance.BorderSize = 0;  // Set border size
                    selectButton.FlatAppearance.MouseOverBackColor = ColorTranslator.FromHtml(qbdlxForm._qbdlxForm._themeManager._currentTheme.HighlightedButtonBackground); // Set background color when hovering
                    selectButton.FlatAppearance.MouseDownBackColor = ColorTranslator.FromHtml(qbdlxForm._qbdlxForm._themeManager._currentTheme.ClickedButtonBackground); // Set background color when clicked
                    string trackLink = "https://open.qobuz.com/track/" + track.Id.ToString(); // Store the track link
                    selectButton.Tag = trackLink;
                    selectButton.Anchor = AnchorStyles.None; // Center the button
                    selectButton.Enabled = !qbdlxForm.getLinkTypeIsBusy;
                    selectButton.Click += (sender, e) => SendURL(mainForm, trackLink);
                    innerRow.Controls.Add(selectButton, 4, 0);
                    if (firstGetButton == null)
                    {
                        firstGetButton = selectButton;
                    }

                    void downloadButtonHandler(object s, EventArgs e)
                    {
                        selectButton.Enabled =
                            mainForm.downloadButton.Enabled ||
                            !qbdlxForm.getLinkTypeIsBusy ||
                            string.IsNullOrWhiteSpace(mainForm.inputTextBox.Text);
                    }
                    mainForm.downloadButton.EnabledChanged += downloadButtonHandler;

                    // Add inner table to row panel
                    rowPanel.Controls.Add(innerRow);

                    AttachRowHighlightPaint(rowPanel);

                    AttachClickRecursive(rowPanel, (s, e) =>
                    {
                        RowClickHandler(
                            s as Control,
                            searchResultsTablePanel,
                            selectedRowindices,
                            qbdlxForm._qbdlxForm);
                    });

                    rowPanel.Disposed += (s, e) =>
                    {
                        mainForm.downloadButton.EnabledChanged -= downloadButtonHandler;
                    };

                    // Add rowPanel to main table
                    searchResultsTablePanel.Controls.Add(rowPanel, 0, rowIndex);
                    searchResultsTablePanel.SetColumnSpan(rowPanel, 5);

                    rowIndex++;
                }

                NativeMethods.SendMessage(searchResultsTablePanel.Handle, Constants.WM_SETREDRAW, (IntPtr)1, IntPtr.Zero);
                searchResultsTablePanel.ResumeLayout();
                qbdlxForm._qbdlxForm.searchResultsPanel.AutoScrollPosition = new Point(0, 0);
                qbdlxForm._qbdlxForm.searchSortingPanel.Enabled = rowIndex > 0;
                qbdlxForm._qbdlxForm.selectAllRowsButton.Enabled = rowIndex > 0; 
                
                if (firstGetButton != null)
                {
                    mainForm.BeginInvoke((Action)(() =>
                    {
                        mainForm.ActiveControl = firstGetButton;
                        firstGetButton.Focus();
                    }));
                }
            });

            lastSearchType = "tracks";
        }

        /// <summary>
        /// Safely disposes all controls in a parent panel, including nested panels, TableLayoutPanels, and PictureBoxes.
        /// </summary>
        /// <param name="parentPanel">The panel whose controls should be disposed.</param>
        private static void DisposeAllControls(Panel parentPanel)
        {
            if (parentPanel == null) return;

            foreach (Control control in parentPanel.Controls)
            {
                DisposeControlRecursively(control);
            }

            parentPanel.Controls.Clear();
        }

        /// <summary>
        /// Recursively disposes a control and its nested controls, disposing PictureBox images as well.
        /// </summary>
        /// <param name="control">The control to dispose.</param>
        private static void DisposeControlRecursively(Control control)
        {
            if (control == null) return;

            if (control is Panel panel)
            {
                foreach (Control child in panel.Controls)
                {
                    DisposeControlRecursively(child);
                }
            }
            else if (control is TableLayoutPanel tableLayout)
            {
                foreach (Control child in tableLayout.Controls)
                {
                    DisposeControlRecursively(child);
                }
            }
            else if (control is PictureBox pictureBox)
            {
                pictureBox.Image?.Dispose();
            }

            control.Dispose();
        }

        /// <summary>
        /// Creates a TableLayoutPanel with a predefined 5-column layout for search results.
        /// </summary>
        /// <returns>A configured TableLayoutPanel.</returns>
        private static TableLayoutPanel CreateSearchResultRow()
        {
            var innerRow = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 5,
                RowCount = 1,
                BackColor = Color.Transparent,
                TabStop = false
            };

            // Define column widths
            innerRow.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 70F));  // Artwork
            innerRow.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 200F)); // Artist name
            innerRow.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 200F)); // Album / Track title
            innerRow.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 140F)); // Quality and Genre
            innerRow.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 70F));  // "GET" Button

            innerRow.ContextMenuStrip = qbdlxForm._qbdlxForm.mainContextMenuStrip;

            return innerRow;
        }

        /// <summary>
        /// Attaches a click event recursively to a control and all its children except Buttons.
        /// </summary>
        private static void AttachClickRecursive(Control parent, EventHandler handler)
        {
            if (parent == null) return;

            if (!(parent is Button) && !(parent is PictureBox))
            {
                parent.Click += (s, e) =>
                {
                    // If click is over a button, ignore
                    Point mousePos = parent.PointToClient(Control.MousePosition);
                    Control hit = parent.GetChildAtPoint(mousePos);
                    if (hit is Button)
                        return;

                    handler?.Invoke(s, e);
                };
            }

            foreach (Control child in parent.Controls)
                AttachClickRecursive(child, handler);
        }

        /// <summary>
        /// Attaches a Paint event to a panel to highlight it when selected and draw a checkmark.
        /// </summary>
        /// <param name="rowPanel">The panel to attach the Paint event to.</param>
        private static void AttachRowHighlightPaint(Panel rowPanel)
        {
            if (rowPanel == null) return;

            rowPanel.Paint += (sender, e) =>
            {
                Color gridLineColor = ColorTranslator.FromHtml(qbdlxForm._qbdlxForm._themeManager._currentTheme.FormBackground);

                // Grid line
                using (Pen gridPen = new Pen(gridLineColor, 1))
                {
                    int y = rowPanel.Height - 1;
                    e.Graphics.DrawLine(gridPen, 0, y, rowPanel.Width, y);
                }

                if (rowPanel.Tag is RowInfo info && info.Selected)
                {
                    // Get the current theme color dynamically
                    Color highlightColor = ColorTranslator.FromHtml(qbdlxForm._qbdlxForm._themeManager._currentTheme.SelectedRowBackground);

                    // Draw semi-transparent highlight
                    using (Brush highlightBrush = new SolidBrush(highlightColor))
                    {
                        e.Graphics.FillRectangle(highlightBrush, rowPanel.ClientRectangle);
                    }

                    // Draw checkmark in the right corner
                    string checkmark = "✅";
                    using (Font font = new Font("Segoe UI Emoji", 36, FontStyle.Bold))
                    using (Brush textBrush = new SolidBrush(Color.Black))
                    {
                        SizeF textSize = e.Graphics.MeasureString(checkmark, font);
                        float x = rowPanel.Width - textSize.Width - 10; // right corner padding
                        float y = (rowPanel.Height - textSize.Height) / 2;
                        e.Graphics.DrawString(checkmark, font, textBrush, x, y);
                    }
                }
            };
        }

        /// <summary>
        /// Handles a row panel click.
        /// </summary>
        /// <param name="clickedControl">The control that was clicked (sender).</param>
        /// <param name="searchResultsPanel">The parent panel containing all row panels.</param>
        /// <param name="selectedRowIndices">The list tracking selected row indices.</param>
        /// <param name="parentForm">The form containing buttons and labels to update.</param>
        private static void RowClickHandler(Control clickedControl, Panel searchResultsPanel, List<int> selectedRowIndices, qbdlxForm parentForm)
        {
            if (clickedControl == null) return;

            Panel rowPanel = null;
            Control current = clickedControl;
            while (current != null)
            {
                if (current.Tag is RowInfo)
                {
                    rowPanel = current as Panel;
                    break;
                }
                current = current.Parent;
            }
            if (rowPanel == null) return;

            int clickedIndex = searchResultsPanel.Controls.IndexOf(rowPanel);
            if (clickedIndex < 0) return;

            bool ctrl = (Control.ModifierKeys & Keys.Control) == Keys.Control;
            bool shift = (Control.ModifierKeys & Keys.Shift) == Keys.Shift;

            // ===== SHIFT =====
            if (shift)
            {
                // If there is no anchor, set it to the current click (convenience)
                if (!lastAnchorRowIndex.HasValue)
                    lastAnchorRowIndex = clickedIndex;

                int start = Math.Min(lastAnchorRowIndex.Value, clickedIndex);
                int end = Math.Max(lastAnchorRowIndex.Value, clickedIndex);

                // Deselect rows from the previous range that are no longer in the new range
                if (lastShiftRange != null)
                {
                    int prevStart = lastShiftRange.Item1;
                    int prevEnd = lastShiftRange.Item2;

                    for (int i = prevStart; i <= prevEnd; i++)
                    {
                        if (i < start || i > end)
                        {
                            // Only deselect if it was selected
                            if (selectedRowIndices.Contains(i))
                                SelectRow(searchResultsPanel, selectedRowIndices, i, false);
                        }
                    }
                }

                // Select all rows in the new range
                for (int i = start; i <= end; i++)
                {
                    if (!selectedRowIndices.Contains(i))
                        SelectRow(searchResultsPanel, selectedRowIndices, i, true);
                }

                // Store the current Shift range for the next Shift click
                lastShiftRange = Tuple.Create(start, end);
            }
            // ===== CTRL =====
            else if (ctrl)
            {
                // Break the "Shift chain"
                lastShiftRange = null;

                ToggleRow(searchResultsPanel, selectedRowIndices, clickedIndex);
                lastAnchorRowIndex = clickedIndex;
            }
            // ===== NORMAL CLICK =====
            else
            {
                // Break the "Shift chain"
                lastShiftRange = null;

                ToggleRow(searchResultsPanel, selectedRowIndices, clickedIndex);
                lastAnchorRowIndex = clickedIndex;
            }

            UpdateSelectionUI(searchResultsPanel, selectedRowIndices, parentForm);
        }

        private static void ToggleRow(Panel panel, List<int> selected, int index)
        {
            if (index < 0 || index >= panel.Controls.Count) return;

            var row = panel.Controls[index] as Panel;
            if (row?.Tag is RowInfo info)
            {
                info.Selected = !info.Selected;

                if (info.Selected)
                    selected.Add(index);
                else
                    selected.Remove(index);

                ToggleGetButton(row, !info.Selected);
                row.Invalidate();
            }
        }

        private static void ToggleGetButton(Panel rowPanel, bool visible)
        {
            foreach (Control c in rowPanel.Controls)
            {
                if (c is TableLayoutPanel t)
                {
                    foreach (Control inner in t.Controls)
                    {
                        if (inner is Button btn)
                        {
                            btn.Visible = visible;
                            return;
                        }
                    }
                }
            }
        }

        private static void UpdateSelectionUI(Panel panel, List<int> selected, qbdlxForm form)
        {
            form.selectAllRowsButton.Enabled = selected.Count < panel.Controls.Count;
            form.deselectAllRowsButton.Enabled = selected.Any();
            form.batchDownloadSelectedRowsButton.Enabled =
                selected.Any() && !qbdlxForm.getLinkTypeIsBusy;

            form.selectedRowsCountLabel.Text = string.Format(
                form.languageManager.GetTranslation("selectedRowsCountLabel"),
                selected.Count);
        }

        private static void SelectRow(Panel panel, List<int> selected, int index, bool selectedState)
        {
            if (index < 0 || index >= panel.Controls.Count) return;

            var row = panel.Controls[index] as Panel;
            if (row?.Tag is RowInfo info)
            {
                info.Selected = selectedState;

                if (selectedState && !selected.Contains(index))
                    selected.Add(index);
                else if (!selectedState)
                    selected.Remove(index);

                ToggleGetButton(row, !selectedState);
                row.Invalidate();
            }
        }

        private void SendURL(qbdlxForm mainForm, string url)
        {
            try
            {
                mainForm.searchAlbumsButton.Focus();
                SetAllGetButtonsEnabledState(mainForm.searchResultsTablePanel, false);

                // Send URL to the input textbox, and start download
                mainForm.logger.Debug("Sending URL to downloader panel, and starting download");
                TextBox inputTextBox = mainForm.inputTextBox;
                inputTextBox.Text = url;
                inputTextBox.ForeColor = Color.FromArgb(200, 200, 200);

                NativeMethods.SendMessage(mainForm.Handle, Constants.WM_SETREDRAW, IntPtr.Zero, IntPtr.Zero);
                mainForm.downloaderButton_Click(this, EventArgs.Empty);
                mainForm.downloadButton.PerformClick();
                mainForm.searchButton.PerformClick(); // Return to search panel.
                NativeMethods.SendMessage(mainForm.Handle, Constants.WM_SETREDRAW, (IntPtr)1, IntPtr.Zero);
            }
            catch (Exception ex)
            {
                mainForm.logger.Error("Exception thrown on SendURL. Error message below:\r\n" + ex);
                return;
            }
        }

        private string GetQualityInfoAlbumLabelText(Item album)
        {
            if (album == null) return string.Empty;

            // Text Format:
            // ------------
            // RELEASE DATE, "SINGLE" OR "{n} tracks"
            // QUALITY INFO
            // GENRE
            string tracksText = album.TracksCount == 1
                ? "Single"
                : $"{album.TracksCount} {qbdlxForm._qbdlxForm.languageManager.GetTranslation("tracks")}";

            string qualityText = $"{album.MaximumBitDepth}bit / {album.MaximumSamplingRate}kHz";
            string genreName = Miscellaneous.GetShortenedGenreName(album.Genre.Name);

            string labelText = $"{album.ReleaseDateOriginal?.Substring(0, 4)}, {tracksText}\r\n{qualityText}\r\n{genreName}";
            return labelText;
        }

        private string GetQualityInfoTrackLabelText(Item track)
        {
            if (track == null) return string.Empty;

            // Text Format:
            // ------------
            // RELEASE DATE
            // QUALITY INFO
            // GENRE
            string qualityText = $"{track.MaximumBitDepth}bit / {track.MaximumSamplingRate}kHz";
            string genreName = Miscellaneous.GetShortenedGenreName(track.Album.Genre.Name);

            string labelText = $"{track.ReleaseDateOriginal?.Substring(0, 4)}\r\n{qualityText}\r\n{genreName}";
            return labelText;
        }

        internal Albums SortAlbums(Albums albums)
        {
            if (albums == null || albums.Items == null)
                return albums;

            DateTime? parseDate(Item i)
            {
                if (DateTime.TryParse(i.ReleaseDateOriginal, out DateTime dt))
                    return dt;
                return null;
            }

            IEnumerable<Item> query = albums.Items;

            bool descending = !qbdlxForm._qbdlxForm.sortAscendantCheckBox.Checked;

            if (qbdlxForm._qbdlxForm.sortArtistNameButton.Checked)
                query = descending ? query.OrderByDescending(i => i.Artist?.Name) : query.OrderBy(i => i.Artist?.Name);

            else if (qbdlxForm._qbdlxForm.sortAlbumTrackNameButton.Checked)
                query = descending ? query.OrderByDescending(i => i.Title) : query.OrderBy(i => i.Title);

            else if (qbdlxForm._qbdlxForm.sortGenreButton.Checked)
                query = descending ? query.OrderByDescending(i => Miscellaneous.GetShortenedGenreName(i.Genre.Name)) : query.OrderBy(i => Miscellaneous.GetShortenedGenreName(i.Genre.Name));

            else if (qbdlxForm._qbdlxForm.sortReleaseDateButton.Checked)
                query = descending
                    ? query.OrderByDescending(i => parseDate(i))
                    : query.OrderBy(i => parseDate(i));

            return new Albums
            {
                Items = query.ToList(),
                Total = albums.Total
            };
        }

        internal Tracks SortTracks(Tracks tracks)
        {
            if (tracks == null || tracks.Items == null)
                return tracks;

            DateTime? parseDate(Item i)
            {
                if (DateTime.TryParse(i.ReleaseDateOriginal, out DateTime dt))
                    return dt;
                return null;
            }

            IEnumerable<Item> query = tracks.Items;

            bool descending = !qbdlxForm._qbdlxForm.sortAscendantCheckBox.Checked;

            if (qbdlxForm._qbdlxForm.sortArtistNameButton.Checked)
                query = descending
                    ? query.OrderByDescending(i => i.Album?.Artist?.Name ?? i.Name)
                    : query.OrderBy(i => i.Album?.Artist?.Name ?? i.Name);

            else if (qbdlxForm._qbdlxForm.sortAlbumTrackNameButton.Checked)
                query = descending ? query.OrderByDescending(i => i.Title) : query.OrderBy(i => i.Title);

            else if (qbdlxForm._qbdlxForm.sortGenreButton.Checked)
                query = descending ? query.OrderByDescending(i => Miscellaneous.GetShortenedGenreName(i.Genre.Name)) : query.OrderBy(i => Miscellaneous.GetShortenedGenreName(i.Genre.Name));

            else if (qbdlxForm._qbdlxForm.sortReleaseDateButton.Checked)
                query = descending
                    ? query.OrderByDescending(i => parseDate(i))
                    : query.OrderBy(i => parseDate(i));

            return new Tracks
            {
                Items = query.ToList(),
                Total = tracks.Total
            };
        }

        /// <summary>
        /// Enables or disables all "GET" buttons in the search results panel.
        /// </summary>
        /// <param name="searchResultsPanel">The TableLayoutPanel containing all rows.</param>
        /// <param name="enabled">Whether the buttons should be enabled or disabled.</param>
        private static void SetAllGetButtonsEnabledState(Panel searchResultsPanel, bool enabled)
        {
            if (searchResultsPanel == null) return;

            string downloadButtonText = qbdlxForm._qbdlxForm.languageManager.GetTranslation("downloadButton");
            foreach (Control rowCtrl in searchResultsPanel.Controls)
            {
                if (rowCtrl is Panel rowPanel)
                {
                    foreach (Control innerCtrl in rowPanel.Controls)
                    {
                        if (innerCtrl is TableLayoutPanel innerTable)
                        {
                            foreach (Control c in innerTable.Controls)
                            {
                                if (c is Button btn && btn.Text == downloadButtonText)
                                {
                                    btn.Enabled = enabled;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
