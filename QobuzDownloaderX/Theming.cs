using System.Collections.Generic;
using Newtonsoft.Json;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using QobuzDownloaderX.Properties;
using System.Drawing.Imaging;

namespace QobuzDownloaderX
{
    public class ThemeSettings
    {
        public Dictionary<string, Theme> Themes { get; set; }
        private Dictionary<string, Image> originalImages = new Dictionary<string, Image>();
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
            string themesFilePath = Path.GetDirectoryName(Application.ExecutablePath) + "\\themes.json";
            if (File.Exists(themesFilePath))
            {
                string json = File.ReadAllText(themesFilePath);
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
            string themesFilePath = Path.GetDirectoryName(Application.ExecutablePath) + "\\themes.json";
            if (File.Exists(themesFilePath))
            {
                string json = File.ReadAllText(themesFilePath);
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
                    qbdlxForm._qbdlxForm.logger.Info("Setting button theme for button: " + button.Name);
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
                    qbdlxForm._qbdlxForm.logger.Info("Setting label theme for label: " + label.Name);
                    if (label.Name == "versionNumber") { if (parent.Name == "LoginForm") { label.BackColor = ColorTranslator.FromHtml(_currentTheme.MainPanelBackground); } else { label.BackColor = ColorTranslator.FromHtml(_currentTheme.SidePanelBackground); } }

                    // Apply specific colors for specific panels if needed
                    label.ForeColor = label.Name == "movingLabel"
                        ? ColorTranslator.FromHtml(_currentTheme.MainPanelBackground)
                        : ColorTranslator.FromHtml(_currentTheme.LabelText);
                }
                else if (control is TextBox textBox)
                {
                    qbdlxForm._qbdlxForm.logger.Info("Setting textBox theme for textBox: " + textBox.Name);
                    textBox.ForeColor = ColorTranslator.FromHtml(_currentTheme.TextBoxText);

                    // Apply specific colors for specific panels if needed
                    textBox.BackColor = (textBox.Name == "userInfoTextbox" || textBox.Name == "emailTextbox" || textBox.Name == "passwordTextbox" || textBox.Name == "appidTextbox" || textBox.Name == "appSecretTextbox")
                        ? ColorTranslator.FromHtml(_currentTheme.MainPanelBackground)
                        : ColorTranslator.FromHtml(_currentTheme.TextBoxBackground);

                    textBox.ForeColor = (textBox.Text == "Paste a Qobuz URL..." || textBox.Text == "Input your search..." || textBox.Text == "e-mail" || textBox.Text == "password" || textBox.Text == "token")
                        ? ColorTranslator.FromHtml(_currentTheme.PlaceholderTextBoxText)
                        : ColorTranslator.FromHtml(_currentTheme.TextBoxText);
                }
                else if (control is PictureBox pictureBox)
                {
                    qbdlxForm._qbdlxForm.logger.Info("Setting textBox theme for textBox: " + pictureBox.Name);

                    // Apply specific colors for specific panels if needed
                    pictureBox.BackColor = pictureBox.Name == "logoPictureBox"
                        ? ColorTranslator.FromHtml(_currentTheme.SidePanelBackground)
                        : ColorTranslator.FromHtml(_currentTheme.MainPanelBackground);

                    if (_currentTheme.InvertLogo)
                    {
                        if (pictureBox.Name == "logoPictureBox" || pictureBox.Name == "qbdlxPictureBox" || pictureBox.Name == "albumPictureBox") { pictureBox.Image = InvertImage(pictureBox.Image); }
                    }
                    else
                    {
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
                    qbdlxForm._qbdlxForm.logger.Info("Setting panel theme for panel: " + panel.Name);

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
}
