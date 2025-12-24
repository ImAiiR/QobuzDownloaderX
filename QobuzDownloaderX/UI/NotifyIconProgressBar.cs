using System.Drawing;

namespace QobuzDownloaderX.UI
{
    /// <summary>
    /// Represents a customizable progress bar to be displayed as the icon of a <see cref="System.Windows.Forms.NotifyIcon"/>.
    /// </summary>
    public struct NotifyIconProgressBar
    {
        /// <summary>
        /// Gets or sets the height of the progress bar, in pixels.
        /// </summary>
        public int Height;

        /// <summary>
        /// Gets or sets the background color of the progress bar.
        /// </summary>
        public Color BackColor;

        /// <summary>
        /// Gets or sets the foreground color of the progress bar.
        /// </summary>
        public Color ForeColor;

        /// <summary>
        /// Gets or sets the fill color of the progress portion of the bar.
        /// </summary>
        public Color FillColor;

        /// <summary>
        /// Gets or sets the color of the border around the progress bar.
        /// </summary>
        public Color BorderColor;

        /// <summary>
        /// Gets or sets the width of the border, in pixels.
        /// </summary>
        public int BorderWidth;

        /// <summary>
        /// Returns an empty instance of <see cref="NotifyIconProgressBar"/> with all properties set to default values.
        /// </summary>
        public static NotifyIconProgressBar Empty
        {
            get
            {
                return new NotifyIconProgressBar
                {
                    Height = 0,
                    BackColor = Color.Empty,
                    ForeColor = Color.Empty,
                    FillColor = Color.Empty,
                    BorderColor = Color.Empty,
                    BorderWidth = 0
                };
            }
        }
    }
}
