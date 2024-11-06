using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QopenAPI;
using QobuzDownloaderX.Properties;
using QobuzDownloaderX.Download;
using System.Reflection;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;

namespace QobuzDownloaderX.Download
{
    public class SearchResultRow
    {
        public string ThumbnailUrl { get; set; }
        public string Artist { get; set; }
        public string Title { get; set; }
        public bool Explicit { get; set; }
        public string FormattedDuration { get; set; }
        public string WebPlayerUrl { get; set; }
        public string StoreUrl { get; set; }
        public int TrackCount { get; set; }
        public string ReleaseDate { get; set; }
        public string FormattedQuality { get; set; }
    }

    class SearchPanelHelper
    {
        public Service QoService = new Service();
        public User QoUser = new User();
        public Item QoItem = new Item();
        public SearchAlbumResult QoAlbumSearch = new SearchAlbumResult();
        public SearchTrackResult QoTrackSearch = new SearchTrackResult();

        public void SearchInitiate(string searchType, string app_id, string searchQuery, string user_auth_token)
        {
            if (searchType == "releases")
            {
                QoAlbumSearch = QoService.SearchAlbumsWithAuth(app_id, searchQuery, 25, 0, user_auth_token);
                PopulateTableAlbums(qbdlxForm._qbdlxForm, QoAlbumSearch);
            }
            else if (searchType == "tracks")
            {
                QoTrackSearch = QoService.SearchTracksWithAuth(app_id, searchQuery, 25, 0, user_auth_token);
                PopulateTableTracks(qbdlxForm._qbdlxForm, QoTrackSearch);
            }
        }

        public void PopulateTableAlbums(qbdlxForm mainForm, SearchAlbumResult QoAlbumSearch)
        {
            // Access the "items" array from the response
            var albums = QoAlbumSearch.Albums.Items;

            mainForm.Invoke((MethodInvoker)delegate ()
            {
                TableLayoutPanel searchResultsTablePanel = mainForm.searchResultsTablePanel;
                searchResultsTablePanel.Controls.Clear();
                searchResultsTablePanel.ColumnCount = 5; // Artwork, Artist, Title, Quality, Button
                searchResultsTablePanel.RowCount = 25; // Set row count based on the number of albums
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
                    PictureBox artwork = new PictureBox();
                    artwork.SizeMode = PictureBoxSizeMode.StretchImage;
                    try { artwork.Load(album.Image.Large.ToString()); /* Using the thumbnail URL */ } catch { artwork.Image = Resources.qbdlx_new; /* Use QBDLX Icon as fallback */ }
                    artwork.Width = 65;
                    artwork.Height = 65;
                    artwork.Anchor = AnchorStyles.None; // Center both horizontally and vertically
                    searchResultsTablePanel.Controls.Add(artwork, 0, rowIndex);

                    // Add Label for artist name
                    System.Windows.Forms.Label artistName = new System.Windows.Forms.Label();
                    artistName.Text = album.Artist.Name.ToString();
                    artistName.AutoSize = true; // Disable auto-sizing to allow wrapping
                    /*artistName.MaximumSize = new Size(0, 0);*/ // Word-wrap if needed
                    artistName.TextAlign = ContentAlignment.MiddleCenter; // Center text horizontally and vertically
                    artistName.Anchor = AnchorStyles.None; // Center within the cell
                    artistName.ForeColor = ColorTranslator.FromHtml(qbdlxForm._qbdlxForm._themeManager._currentTheme.LabelText); // Set text color
                    artistName.Font = new Font("Nirmala UI", 10F, FontStyle.Regular); // Set font size and style
                    searchResultsTablePanel.Controls.Add(artistName, 1, rowIndex);

                    // Add Label for album title
                    System.Windows.Forms.Label albumTitle = new System.Windows.Forms.Label();
                    albumTitle.Text = album.Title.ToString().TrimEnd();
                    if (album.Version != null) { albumTitle.Text = albumTitle.Text + " (" + album.Version + ")"; }
                    if (album.ParentalWarning == true) { albumTitle.Text = "[E] " + albumTitle.Text; } // Add "[E]" if Qobuz lists the release with a parental warning
                    albumTitle.AutoSize = true;
                    /*albumTitle.MaximumSize = new Size(0, 0);*/ // Allow word-wrap
                    albumTitle.TextAlign = ContentAlignment.MiddleCenter; // Center text horizontally and vertically
                    albumTitle.Anchor = AnchorStyles.None; // Center within the cell
                    albumTitle.ForeColor = ColorTranslator.FromHtml(qbdlxForm._qbdlxForm._themeManager._currentTheme.LabelText); // Set text color
                    albumTitle.Font = new Font("Nirmala UI", 10F, FontStyle.Regular); // Set font size and style
                    searchResultsTablePanel.Controls.Add(albumTitle, 2, rowIndex);

                    // Add Label for quality
                    System.Windows.Forms.Label qualityLabel = new System.Windows.Forms.Label();
                    qualityLabel.Text = album.MaximumBitDepth.ToString() + "bit/" + album.MaximumSamplingRate + "kHz";
                    qualityLabel.AutoSize = true;
                    qualityLabel.MaximumSize = new Size(0, 0); // Allow word-wrap
                    qualityLabel.TextAlign = ContentAlignment.MiddleCenter; // Center text horizontally and vertically
                    qualityLabel.Anchor = AnchorStyles.None; // Center within the cell

                    // Set text color
                    if (qualityLabel.Text.Contains("24bit"))
                    {
                        qualityLabel.ForeColor = ColorTranslator.FromHtml(qbdlxForm._qbdlxForm._themeManager._currentTheme.HiResLabelText);
                    }
                    else
                    {
                        qualityLabel.ForeColor = ColorTranslator.FromHtml(qbdlxForm._qbdlxForm._themeManager._currentTheme.LabelText); ;
                    }

                    qualityLabel.Font = new Font("Nirmala UI", 10F, FontStyle.Regular); // Set font size and style
                    searchResultsTablePanel.Controls.Add(qualityLabel, 3, rowIndex);

                    // Add Button for selecting album ID
                    Button selectButton = new Button();
                    selectButton.Text = "GET";
                    selectButton.ForeColor = ColorTranslator.FromHtml(qbdlxForm._qbdlxForm._themeManager._currentTheme.ButtonText); // Set button text color
                    selectButton.BackColor = ColorTranslator.FromHtml(qbdlxForm._qbdlxForm._themeManager._currentTheme.ButtonBackground); // Set button background color
                    selectButton.Font = new Font("Nirmala UI", 8F, FontStyle.Regular); // Set font size and style
                    selectButton.FlatStyle = FlatStyle.Flat; // Set FlatStyle to Flat
                    selectButton.FlatAppearance.BorderSize = 0;  // Set border size
                    selectButton.FlatAppearance.MouseOverBackColor = ColorTranslator.FromHtml(qbdlxForm._qbdlxForm._themeManager._currentTheme.HighlightedButtonBackground); // Set background color when hovering
                    selectButton.FlatAppearance.MouseDownBackColor = ColorTranslator.FromHtml(qbdlxForm._qbdlxForm._themeManager._currentTheme.ClickedButtonBackground); // Set background color when clicked
                    string albumLink = "https://play.qobuz.com/album/" + album.Id.ToString(); // Store the album link
                    selectButton.Click += (sender, e) => SendURL(mainForm, albumLink);
                    selectButton.Anchor = AnchorStyles.None; // Center the button
                    searchResultsTablePanel.Controls.Add(selectButton, 4, rowIndex);

                    rowIndex++;
                }
            });
        }

        public void PopulateTableTracks(qbdlxForm mainForm, SearchTrackResult QoTrackSearch)
        {
            // Access the "items" array from the response
            var tracks = QoTrackSearch.Tracks.Items;

            mainForm.Invoke((MethodInvoker)delegate ()
            {
                TableLayoutPanel searchResultsTablePanel = mainForm.searchResultsTablePanel;
                searchResultsTablePanel.Controls.Clear();
                searchResultsTablePanel.ColumnCount = 5; // Artwork, Artist, Title, Quality, Button
                searchResultsTablePanel.RowCount = 25; // Set row count based on the number of albums
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
                    PictureBox artwork = new PictureBox();
                    artwork.SizeMode = PictureBoxSizeMode.StretchImage;
                    try { artwork.Load(track.Album.Image.Large.ToString()); /* Using the thumbnail URL */ } catch { artwork.Image = Resources.qbdlx_new; /* Use QBDLX Icon as fallback */ }
                    artwork.Width = 65;
                    artwork.Height = 65;
                    artwork.Anchor = AnchorStyles.None; // Center both horizontally and vertically
                    searchResultsTablePanel.Controls.Add(artwork, 0, rowIndex);

                    // Add Label for artist name
                    System.Windows.Forms.Label artistName = new System.Windows.Forms.Label();
                    artistName.Text = track.Performer.Name.ToString();
                    artistName.AutoSize = true; // Disable auto-sizing to allow wrapping
                    /*artistName.MaximumSize = new Size(0, 0);*/ // Word-wrap if needed
                    artistName.TextAlign = ContentAlignment.MiddleCenter; // Center text horizontally and vertically
                    artistName.Anchor = AnchorStyles.None; // Center within the cell
                    artistName.ForeColor = ColorTranslator.FromHtml(qbdlxForm._qbdlxForm._themeManager._currentTheme.LabelText); // Set text color
                    artistName.Font = new Font("Nirmala UI", 10F, FontStyle.Regular); // Set font size and style
                    searchResultsTablePanel.Controls.Add(artistName, 1, rowIndex);

                    // Add Label for track title
                    System.Windows.Forms.Label trackTitle = new System.Windows.Forms.Label();
                    trackTitle.Text = track.Title.ToString().TrimEnd();
                    if (track.Version != null) { trackTitle.Text = trackTitle.Text + " (" + track.Version + ")"; }
                    if (track.ParentalWarning == true) { trackTitle.Text = "[E] " + trackTitle.Text; } // Add "[E]" if Qobuz lists the track with a parental warning
                    trackTitle.AutoSize = true;
                    /*trackTitle.MaximumSize = new Size(0, 0);*/ // Allow word-wrap
                    trackTitle.TextAlign = ContentAlignment.MiddleCenter; // Center text horizontally and vertically
                    trackTitle.Anchor = AnchorStyles.None; // Center within the cell
                    trackTitle.ForeColor = ColorTranslator.FromHtml(qbdlxForm._qbdlxForm._themeManager._currentTheme.LabelText); // Set text color
                    trackTitle.Font = new Font("Nirmala UI", 10F, FontStyle.Regular); // Set font size and style
                    searchResultsTablePanel.Controls.Add(trackTitle, 2, rowIndex);

                    // Add Label for quality
                    System.Windows.Forms.Label qualityLabel = new System.Windows.Forms.Label();
                    qualityLabel.Text = track.MaximumBitDepth.ToString() + "bit/" + track.MaximumSamplingRate + "kHz";
                    qualityLabel.AutoSize = true;
                    qualityLabel.MaximumSize = new Size(0, 0); // Allow word-wrap
                    qualityLabel.TextAlign = ContentAlignment.MiddleCenter; // Center text horizontally and vertically
                    qualityLabel.Anchor = AnchorStyles.None; // Center within the cell

                    // Set text color
                    if (qualityLabel.Text.Contains("24bit"))
                    {
                        qualityLabel.ForeColor = ColorTranslator.FromHtml(qbdlxForm._qbdlxForm._themeManager._currentTheme.HiResLabelText);
                    }
                    else
                    {
                        qualityLabel.ForeColor = ColorTranslator.FromHtml(qbdlxForm._qbdlxForm._themeManager._currentTheme.LabelText);
                    }

                    qualityLabel.Font = new Font("Nirmala UI", 10F, FontStyle.Regular); // Set font size and style
                    searchResultsTablePanel.Controls.Add(qualityLabel, 3, rowIndex);

                    // Add Button for selecting album ID
                    Button selectButton = new Button();
                    selectButton.Text = "GET";
                    selectButton.ForeColor = ColorTranslator.FromHtml(qbdlxForm._qbdlxForm._themeManager._currentTheme.ButtonText); // Set button text color
                    selectButton.BackColor = ColorTranslator.FromHtml(qbdlxForm._qbdlxForm._themeManager._currentTheme.ButtonBackground); // Set button background color
                    selectButton.Font = new Font("Nirmala UI", 8F, FontStyle.Regular); // Set font size and style
                    selectButton.FlatStyle = FlatStyle.Flat; // Set FlatStyle to Flat
                    selectButton.FlatAppearance.BorderSize = 0;  // Set border size
                    selectButton.FlatAppearance.MouseOverBackColor = ColorTranslator.FromHtml(qbdlxForm._qbdlxForm._themeManager._currentTheme.HighlightedButtonBackground); // Set background color when hovering
                    selectButton.FlatAppearance.MouseDownBackColor = ColorTranslator.FromHtml(qbdlxForm._qbdlxForm._themeManager._currentTheme.ClickedButtonBackground); // Set background color when clicked
                    string trackLink = "https://open.qobuz.com/track/" + track.Id.ToString(); // Store the track link
                    selectButton.Click += (sender, e) => SendURL(mainForm, trackLink);
                    selectButton.Anchor = AnchorStyles.None; // Center the button
                    searchResultsTablePanel.Controls.Add(selectButton, 4, rowIndex);

                    rowIndex++;
                }
            });
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
                mainForm.getLinkType();
            }
            catch (Exception ex)
            {
                mainForm.logger.Error("Error on SendURL (SearchPanelHelper). Error below:\r\n" + ex);
                return;
            }
        }
    }
}
