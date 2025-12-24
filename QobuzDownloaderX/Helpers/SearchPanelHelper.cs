using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using QopenAPI;

using QobuzDownloaderX.Properties;

namespace QobuzDownloaderX.Helpers
{
    class SearchPanelHelper
    {
        public Service QoService = new Service();
        public User QoUser = new User();
        public Item QoItem = new Item();
        public SearchAlbumResult QoAlbumSearch = new SearchAlbumResult();
        public SearchTrackResult QoTrackSearch = new SearchTrackResult();

        public int limitResults;
        public string lastSearchType = "";

        public void SearchInitiate(string searchType, string app_id, string searchQuery, string user_auth_token)
        {
            limitResults = (int)qbdlxForm._qbdlxForm.limitSearchResultsNumericUpDown.Value;
            qbdlxForm._qbdlxForm.Invoke(new Action(() => qbdlxForm._qbdlxForm.searchResultsCountLabel.Text = "…"));

            if (searchType == "releases")
            {
                QoAlbumSearch = null;
                QoAlbumSearch = QoService.SearchAlbumsWithAuth(app_id, searchQuery, limitResults, 0, user_auth_token);
                QoAlbumSearch.Albums = SortAlbums(QoAlbumSearch.Albums);
                PopulateTableAlbums(qbdlxForm._qbdlxForm, QoAlbumSearch);
                qbdlxForm._qbdlxForm.Invoke(new Action(() => qbdlxForm._qbdlxForm.searchResultsCountLabel.Text = $"{QoAlbumSearch.Albums.Items.Count:N0} {qbdlxForm._qbdlxForm.languageManager.GetTranslation("searchResultsCountLabel")}"));
            }
            else if (searchType == "tracks")
            {
                QoTrackSearch = null;
                QoTrackSearch = QoService.SearchTracksWithAuth(app_id, searchQuery, limitResults, 0, user_auth_token);
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

            mainForm.Invoke((MethodInvoker)delegate ()
            {
                qbdlxForm._qbdlxForm.searchSortingPanel.Enabled = false;

                TableLayoutPanel searchResultsTablePanel = mainForm.searchResultsTablePanel;
                searchResultsTablePanel.SuspendLayout();
                foreach (Control ctrl in searchResultsTablePanel.Controls)
                {
                    if (ctrl is PictureBox pb)
                    {
                        pb.Image?.Dispose();
                    }
                    ctrl?.Dispose();
                }
                searchResultsTablePanel.Controls.Clear();

                searchResultsTablePanel.ColumnCount = 5; // Artwork, Artist, Title, Quality, Button
                searchResultsTablePanel.RowCount = limitResults; // Set row count based on the number of albums
                searchResultsTablePanel.AutoSize = true;

                // Set ColumnStyles to define the size of each column
                searchResultsTablePanel.ColumnStyles.Clear();
                searchResultsTablePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 70F));  // Artwork column
                searchResultsTablePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 200F)); // Artist name
                searchResultsTablePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 200F)); // Album title
                searchResultsTablePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 140F)); // Quality
                searchResultsTablePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 70F));  // Button column

                int rowIndex = 0;

                foreach (var album in albums)
                {
                    // Add PictureBox for artwork
                    PictureBox artwork = new PictureBox
                    {
                        SizeMode = PictureBoxSizeMode.StretchImage
                    };
                    try { artwork.Load(album.Image.Large.ToString()); /* Using the thumbnail URL */ } catch { artwork.Image = Resources.qbdlx_new; /* Use QBDLX Icon as fallback */ }
                    artwork.Width = 65;
                    artwork.Height = 65;
                    artwork.Anchor = AnchorStyles.None; // Center both horizontally and vertically
                    searchResultsTablePanel.Controls.Add(artwork, 0, rowIndex);

                    // Add Label for artist name
                    System.Windows.Forms.Label artistName = new System.Windows.Forms.Label
                    {
                        Text = album.Artist.Name.ToString(),
                        AutoSize = true, // Disable auto-sizing to allow wrapping
                        /*artistName.MaximumSize = new Size(0, 0);*/ // Word-wrap if needed
                        TextAlign = ContentAlignment.MiddleCenter, // Center text horizontally and vertically
                        Anchor = AnchorStyles.None, // Center within the cell
                        ForeColor = ColorTranslator.FromHtml(qbdlxForm._qbdlxForm._themeManager._currentTheme.LabelText), // Set text color
                        Font = new Font(fontName, 10F, FontStyle.Regular) // Set font size and style
                    };
                    searchResultsTablePanel.Controls.Add(artistName, 1, rowIndex);

                    // Add Label for album title
                    System.Windows.Forms.Label albumTitle = new System.Windows.Forms.Label
                    {
                        Text = album.Title.ToString().TrimEnd()
                    };
                    if (album.Version != null) { albumTitle.Text = albumTitle.Text + " (" + album.Version + ")"; }
                    if (album.ParentalWarning == true) { albumTitle.Text = "[E] " + albumTitle.Text; } // Add "[E]" if Qobuz lists the release with a parental warning
                    albumTitle.AutoSize = true;
                    /*albumTitle.MaximumSize = new Size(0, 0);*/ // Allow word-wrap
                    albumTitle.TextAlign = ContentAlignment.MiddleCenter; // Center text horizontally and vertically
                    albumTitle.Anchor = AnchorStyles.None; // Center within the cell
                    albumTitle.ForeColor = ColorTranslator.FromHtml(qbdlxForm._qbdlxForm._themeManager._currentTheme.LabelText); // Set text color
                    albumTitle.Font = new Font(fontName, 10F, FontStyle.Regular); // Set font size and style
                    searchResultsTablePanel.Controls.Add(albumTitle, 2, rowIndex);

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
                    searchResultsTablePanel.Controls.Add(qualityLabel, 3, rowIndex);

                    // Add Button for selecting album ID
                    Button selectButton = new Button
                    {
                        Text = qbdlxForm._qbdlxForm.languageManager.GetTranslation("downloadButton"),
                        ForeColor = ColorTranslator.FromHtml(qbdlxForm._qbdlxForm._themeManager._currentTheme.ButtonText), // Set button text color
                        BackColor = ColorTranslator.FromHtml(qbdlxForm._qbdlxForm._themeManager._currentTheme.ButtonBackground), // Set button background color
                        Font = new Font(fontName, 8F, FontStyle.Regular), // Set font size and style
                        FlatStyle = FlatStyle.Flat // Set FlatStyle to Flat
                    };
                    selectButton.FlatAppearance.BorderSize = 0;  // Set border size
                    selectButton.FlatAppearance.MouseOverBackColor = ColorTranslator.FromHtml(qbdlxForm._qbdlxForm._themeManager._currentTheme.HighlightedButtonBackground); // Set background color when hovering
                    selectButton.FlatAppearance.MouseDownBackColor = ColorTranslator.FromHtml(qbdlxForm._qbdlxForm._themeManager._currentTheme.ClickedButtonBackground); // Set background color when clicked
                    string albumLink = "https://play.qobuz.com/album/" + album.Id.ToString(); // Store the album link
                    selectButton.Click += (sender, e) => SendURL(mainForm, albumLink);
                    selectButton.Anchor = AnchorStyles.None; // Center the button
                    selectButton.Enabled = !qbdlxForm._qbdlxForm.getLinkTypeIsBusy;
                    mainForm.downloadButton.EnabledChanged += (sender, e) =>
                    {
                        selectButton.Enabled =
                            mainForm.downloadButton.Enabled == true ||
                            !qbdlxForm._qbdlxForm.getLinkTypeIsBusy ||
                            string.IsNullOrWhiteSpace(mainForm.inputTextbox.Text);

                    };
                    searchResultsTablePanel.Controls.Add(selectButton, 4, rowIndex);

                    rowIndex++;
                }
                searchResultsTablePanel.ResumeLayout();
                qbdlxForm._qbdlxForm.searchSortingPanel.Enabled = true;
            });

            lastSearchType = "releases";
        }

        public void PopulateTableTracks(qbdlxForm mainForm, SearchTrackResult QoTrackSearch)
        {

            // Access the "items" array from the response
            var tracks = QoTrackSearch.Tracks.Items;

            // Load the font name from the translation file
            string fontName = qbdlxForm._qbdlxForm.languageManager.GetTranslation("TranslationFont");

            mainForm.Invoke((MethodInvoker)delegate ()
            {
                qbdlxForm._qbdlxForm.searchSortingPanel.Enabled = false;
                TableLayoutPanel searchResultsTablePanel = mainForm.searchResultsTablePanel;
                searchResultsTablePanel.SuspendLayout();
                foreach (Control ctrl in searchResultsTablePanel.Controls)
                {
                    if (ctrl is PictureBox pb)
                    {
                        pb.Image?.Dispose();
                    }
                    ctrl?.Dispose();
                }
                searchResultsTablePanel.Controls.Clear();

                searchResultsTablePanel.ColumnCount = 5; // Artwork, Artist, Title, Quality, Button
                searchResultsTablePanel.RowCount = limitResults; // Set row count based on the number of albums
                searchResultsTablePanel.AutoSize = true;

                // Set ColumnStyles to define the size of each column
                searchResultsTablePanel.ColumnStyles.Clear();
                searchResultsTablePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 70F));  // Artwork column
                searchResultsTablePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 200F)); // Artist name 
                searchResultsTablePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 200F)); // Track title
                searchResultsTablePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 140F)); // Quality
                searchResultsTablePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 70F));  // Button column

                int rowIndex = 0;

                foreach (var track in tracks)
                {
                    // Add PictureBox for artwork
                    PictureBox artwork = new PictureBox
                    {
                        SizeMode = PictureBoxSizeMode.StretchImage
                    };
                    try { artwork.Load(track.Album.Image.Large.ToString()); /* Using the thumbnail URL */ } catch { artwork.Image = Resources.qbdlx_new; /* Use QBDLX Icon as fallback */ }
                    artwork.Width = 65;
                    artwork.Height = 65;
                    artwork.Anchor = AnchorStyles.None; // Center both horizontally and vertically
                    searchResultsTablePanel.Controls.Add(artwork, 0, rowIndex);

                    // Add Label for artist name
                    System.Windows.Forms.Label artistName = new System.Windows.Forms.Label
                    {
                        Text = track.Performer.Name.ToString(),
                        AutoSize = true, // Disable auto-sizing to allow wrapping
                        /*artistName.MaximumSize = new Size(0, 0);*/ // Word-wrap if needed
                        TextAlign = ContentAlignment.MiddleCenter, // Center text horizontally and vertically
                        Anchor = AnchorStyles.None, // Center within the cell
                        ForeColor = ColorTranslator.FromHtml(qbdlxForm._qbdlxForm._themeManager._currentTheme.LabelText), // Set text color
                        Font = new Font(fontName, 10F, FontStyle.Regular) // Set font size and style
                    };
                    searchResultsTablePanel.Controls.Add(artistName, 1, rowIndex);

                    // Add Label for track title
                    System.Windows.Forms.Label trackTitle = new System.Windows.Forms.Label
                    {
                        Text = track.Title.ToString().TrimEnd()
                    };
                    if (track.Version != null) { trackTitle.Text = trackTitle.Text + " (" + track.Version + ")"; }
                    if (track.ParentalWarning == true) { trackTitle.Text = "[E] " + trackTitle.Text; } // Add "[E]" if Qobuz lists the track with a parental warning
                    trackTitle.AutoSize = true;
                    /*trackTitle.MaximumSize = new Size(0, 0);*/ // Allow word-wrap
                    trackTitle.TextAlign = ContentAlignment.MiddleCenter; // Center text horizontally and vertically
                    trackTitle.Anchor = AnchorStyles.None; // Center within the cell
                    trackTitle.ForeColor = ColorTranslator.FromHtml(qbdlxForm._qbdlxForm._themeManager._currentTheme.LabelText); // Set text color
                    trackTitle.Font = new Font(fontName, 10F, FontStyle.Regular); // Set font size and style
                    searchResultsTablePanel.Controls.Add(trackTitle, 2, rowIndex);

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
                    searchResultsTablePanel.Controls.Add(qualityLabel, 3, rowIndex);

                    // Add Button for selecting album ID
                    Button selectButton = new Button
                    {
                        Text = qbdlxForm._qbdlxForm.languageManager.GetTranslation("downloadButton"),
                        ForeColor = ColorTranslator.FromHtml(qbdlxForm._qbdlxForm._themeManager._currentTheme.ButtonText), // Set button text color
                        BackColor = ColorTranslator.FromHtml(qbdlxForm._qbdlxForm._themeManager._currentTheme.ButtonBackground), // Set button background color
                        Font = new Font(fontName, 8F, FontStyle.Regular), // Set font size and style
                        FlatStyle = FlatStyle.Flat // Set FlatStyle to Flat
                    };
                    selectButton.FlatAppearance.BorderSize = 0;  // Set border size
                    selectButton.FlatAppearance.MouseOverBackColor = ColorTranslator.FromHtml(qbdlxForm._qbdlxForm._themeManager._currentTheme.HighlightedButtonBackground); // Set background color when hovering
                    selectButton.FlatAppearance.MouseDownBackColor = ColorTranslator.FromHtml(qbdlxForm._qbdlxForm._themeManager._currentTheme.ClickedButtonBackground); // Set background color when clicked
                    string trackLink = "https://open.qobuz.com/track/" + track.Id.ToString(); // Store the track link
                    selectButton.Click += (sender, e) => SendURL(mainForm, trackLink);
                    selectButton.Anchor = AnchorStyles.None; // Center the button
                    selectButton.Enabled = !qbdlxForm._qbdlxForm.getLinkTypeIsBusy;
                    mainForm.downloadButton.EnabledChanged += (sender, e) =>
                    {
                        selectButton.Enabled =
                            mainForm.downloadButton.Enabled == true ||
                            !qbdlxForm._qbdlxForm.getLinkTypeIsBusy ||
                            string.IsNullOrWhiteSpace(mainForm.inputTextbox.Text);
                    };
                    searchResultsTablePanel.Controls.Add(selectButton, 4, rowIndex);

                    rowIndex++;
                }
                searchResultsTablePanel.ResumeLayout();
                qbdlxForm._qbdlxForm.searchSortingPanel.Enabled = true;
            });

            lastSearchType = "tracks";
        }

        public void SendURL(qbdlxForm mainForm, string url)
        {
            try
            {
                // Send URL to the input textbox, and start download
                mainForm.logger.Debug("Sending URL to download panel, and starting download");
                TextBox inputTextbox = mainForm.inputTextbox;
                inputTextbox.Text = url;
                inputTextbox.ForeColor = Color.FromArgb(200, 200, 200);
                mainForm.downloaderButton_Click(this, EventArgs.Empty);
                mainForm.downloadButton.PerformClick();
                mainForm.searchButton.PerformClick(); // Return to search panel.
            }
            catch (Exception ex)
            {
                mainForm.logger.Error("Error on SendURL (SearchPanelHelper). Error below:\r\n" + ex);
                return;
            }
        }

        public string GetQualityInfoAlbumLabelText(Item album)
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
            string genreName = GetShortenedGenreName(album.Genre.Name);

            string labelText = $"{album.ReleaseDateOriginal?.Substring(0, 4)}, {tracksText}\r\n{qualityText}\r\n{genreName}";
            return labelText;
        }

        public string GetQualityInfoTrackLabelText(Item track)
        {
            if (track == null) return string.Empty;

            // Text Format:
            // ------------
            // RELEASE DATE
            // QUALITY INFO
            // GENRE
            string qualityText = $"{track.MaximumBitDepth}bit / {track.MaximumSamplingRate}kHz";
            string genreName = GetShortenedGenreName(track.Album.Genre.Name);

            string labelText = $"{track.ReleaseDateOriginal?.Substring(0, 4)}\r\n{qualityText}\r\n{genreName}";
            return labelText;
        }

        public string GetShortenedGenreName(string genre)
        {
            if (string.IsNullOrEmpty(genre))
                return string.Empty;

            // Shorten English genre names
            string genreName = genre
                .Replace("Alternative", "Alt.")
                .Replace("Brazilian", "Brazil.")
                .Replace("Film Soundtracks", "OST")
                .Replace("Latin America", "Latin Am.")
                .Replace("Miscellaneous", "Misc.")
                .Replace(" Music", "")
                .Replace(" music", "")
                .Replace("North America", "North Am.");

            // Shorten Spanish genre names
            genreName = genreName
                .Replace("Alternativa", "Alt.")
                .Replace("América latina", "Latina")
                .Replace("Bandas sonoras de", "BSO")
                .Replace("brasileño", "Brasil.")
                .Replace("Chanson ", "")
                .Replace("Música ", "")
                .Replace("música ", "")
                .Replace("Músicas ", "")
                .Replace("músicas ", "")
                .Replace("Música de ", "")
                .Replace("música de ", "")
                .Replace("Músicas de ", "")
                .Replace("músicas de ", "")
                .Replace("Norteamérica", "Norteam.")
                .Replace("World music", "Mundial");

            // Format before truncating
            genreName = genreName.TrimStart(' ', '&', '-', '.', ',').Replace("&", "&&");
            if (!string.IsNullOrEmpty(genreName))
                genreName = char.ToUpper(genreName[0]) + genreName.Substring(1).TrimEnd();

            // Truncate if too long
            if (genreName.Length > 18)
                genreName = genreName.Substring(0, 18) + "…";

            return genreName;
        }

        public Albums SortAlbums(Albums albums)
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

        public Tracks SortTracks(Tracks tracks)
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
    }
}
