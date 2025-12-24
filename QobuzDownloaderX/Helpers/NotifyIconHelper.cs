using QobuzDownloaderX.UI;
using QobuzDownloaderX.Win32;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace QobuzDownloaderX.Helpers
{
    internal sealed class NotifyIconHelper
    {
        /// <summary>
        /// Renders a progress bar overlay on a <see cref="NotifyIcon"/> and optionally draws text on it.
        /// </summary>
        /// <param name="ntfy">The <see cref="NotifyIcon"/> whose icon will be updated with the rendered progress bar.</param>
        /// <param name="baseIcon">The base <see cref="Icon"/> used as a template for the progress bar.</param>
        /// <param name="progressBar">A <see cref="NotifyIconProgressBar"/> structure containing the bar's height, colors and border width.</param>
        /// <param name="value">The current position of the progress bar.</param>
        /// <param name="maximumValue">The maximum <paramref name="value"/> range of the progress bar.</param>
        /// <param name="text">Optional text to display centered above the progress bar. Must be 3 characters or fewer if provided.</param>
        [DebuggerStepThrough]
        public static void RenderNotifyIconProgressBar(
            NotifyIcon ntfy,
            Icon baseIcon,
            NotifyIconProgressBar progressBar,
            int value,
            int maximumValue,
            string text = null)
        {
            if (ntfy is null) throw new ArgumentNullException(nameof(ntfy));
            if (baseIcon is null) throw new ArgumentNullException(nameof(baseIcon));
            if (maximumValue <= 0) throw new ArgumentOutOfRangeException(nameof(maximumValue), $"{nameof(maximumValue)} must be greater than zero.");
            if (value < 0 || value > maximumValue) throw new ArgumentOutOfRangeException(nameof(value), $"{nameof(value)} must be between zero and {nameof(maximumValue)}.");

            Icon currentIcon = ntfy.Icon;

            using (Bitmap bmp = baseIcon.ToBitmap())
            {
                int width = bmp.Width;
                int height = bmp.Height;

                if (progressBar.Height <= 0)
                    throw new ArgumentOutOfRangeException(nameof(progressBar), $"{nameof(progressBar.Height)} must be greater than zero.");

                if (progressBar.Height > height)
                    throw new ArgumentOutOfRangeException(nameof(progressBar), $"{nameof(progressBar.Height)} ({progressBar.Height}) exceeds the icon height ({height}).");

                if (progressBar.BorderWidth > height)
                    throw new ArgumentOutOfRangeException(nameof(progressBar), $"{nameof(progressBar.BorderWidth)} ({progressBar.BorderWidth}) exceeds the icon height ({height}).");

                using (Graphics g = Graphics.FromImage(bmp))
                {
                    g.DrawImage(bmp, Point.Empty);

                    g.CompositingMode = CompositingMode.SourceOver;
                    g.CompositingQuality = CompositingQuality.HighQuality;
                    g.InterpolationMode = InterpolationMode.High;
                    g.PixelOffsetMode = PixelOffsetMode.Half;
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

                    int barY = height - progressBar.Height;

                    using (var backgroundBrush = new SolidBrush(progressBar.BackColor))
                    {
                        g.FillRectangle(backgroundBrush, 0, barY, width, progressBar.Height);
                    }

                    using (var fillBrush = new SolidBrush(progressBar.FillColor))
                    {
                        float percent = (float)value / maximumValue;
                        int filledWidth = (int)(width * percent);
                        g.FillRectangle(fillBrush, 0, barY, filledWidth, progressBar.Height);
                    }

                    if (progressBar.BorderWidth > 0)
                    {
                        using (var borderPen = new Pen(progressBar.BorderColor, progressBar.BorderWidth))
                        {
                            g.DrawRectangle(borderPen, 0, barY, width - 1, progressBar.Height);
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(text))
                    {
                        using (var fontFamily = new FontFamily("Segoe UI"))
                        {
                            var fontStyle = FontStyle.Bold;
                            var layoutRect = new RectangleF(0, 0, width, height);

                            float fontSizePx = ComputeMaxFontSizeForRectangle(g, text, fontFamily, fontStyle, layoutRect);

                            using (var font = new Font(fontFamily, fontSizePx, fontStyle, GraphicsUnit.Pixel))
                            using (var gp = new GraphicsPath())
                            {
                                var sf = new StringFormat
                                {
                                    Alignment = StringAlignment.Center,
                                    LineAlignment = StringAlignment.Center
                                };

                                gp.AddString(text, font.FontFamily, (int)font.Style, font.Size, layoutRect, sf);

                                using (var outlinePen = new Pen(Color.FromArgb(220, Color.Black), Math.Max(1.0f, fontSizePx * 0.18f)))
                                {
                                    outlinePen.LineJoin = LineJoin.Round;
                                    g.DrawPath(outlinePen, gp);
                                }

                                using (var foregroundBrush = new SolidBrush(progressBar.ForeColor))
                                {
                                    g.FillPath(foregroundBrush, gp);
                                }
                            }
                        }
                    }
                }

                IntPtr hIcon = bmp.GetHicon();
                using (Icon tempIcon = Icon.FromHandle(hIcon))
                {
                    Icon finalIcon = (Icon)tempIcon.Clone();
                    NativeMethods.DestroyIcon(hIcon);
                    ntfy.Icon = finalIcon;
                }

                currentIcon.Dispose();
            }
        }

        /// <summary>
        /// Determines the largest font size that allows the specified text to fit entirely
        /// within the given rectangle when drawn using the provided <see cref="Graphics"/> object.
        /// </summary>
        /// 
        /// <param name="g">
        /// The source <see cref="Graphics"/> object used to measure the text.
        /// </param>
        /// 
        /// <param name="text">
        /// The text to measure.
        /// </param>
        /// 
        /// <param name="fontFamily">
        /// The font family to use (e.g., "Segoe UI").
        /// </param>
        /// 
        /// <param name="fontStyle">
        /// The font style (e.g., <see cref="FontStyle.Regular"/>).
        /// </param>
        /// 
        /// <param name="layoutRectangle">
        /// The rectangle within which the text must fit.
        /// </param>
        /// 
        /// <param name="minimumSize">
        /// The minimum allowed font size (in <see cref="GraphicsUnit.Pixel"/>).
        /// <para></para>
        /// If the text does not fit even at this size, the function returns this value.
        /// <para></para>
        /// Default value is <c>1.0</c>.
        /// </param>
        /// 
        /// <param name="tolerance">
        /// The precision threshold for how closely the function tries to fit the text in the rectangle,
        /// in <see cref="GraphicsUnit.Pixel"/>.
        /// <para></para>
        /// Smaller values give more exact results but require more computation time.
        /// <para></para>
        /// Default value is <c>0.5</c>.
        /// </param>
        /// 
        /// <returns>
        /// The largest font size in <see cref="GraphicsUnit.Pixel"/> that fits the text inside the rectangle.
        /// <para></para>
        /// If the text cannot fit even at <paramref name="minimumSize"/>, that minimum value is returned.
        /// </returns>
        private static float ComputeMaxFontSizeForRectangle(
            Graphics g,
            string text,
            FontFamily fontFamily,
            FontStyle fontStyle,
            RectangleF layoutRectangle,
            float minimumSize = 1.0f,
            float tolerance = 0.5f)
        {
            float minSize = minimumSize;
            float maxSize = layoutRectangle.Height;
            float bestFit = minSize;

            while ((maxSize - minSize) > tolerance)
            {
                float midSize = (minSize + maxSize) / 2f;

                using (Font testFont = new Font(fontFamily, midSize, fontStyle, GraphicsUnit.Pixel))
                {
                    SizeF textSize = g.MeasureString(text, testFont);

                    if (textSize.Width <= layoutRectangle.Width &&
                        textSize.Height <= layoutRectangle.Height)
                    {
                        bestFit = midSize;
                        minSize = midSize;
                    }
                    else
                    {
                        maxSize = midSize;
                    }
                }
            }

            return Math.Max(minimumSize, bestFit);
        }
    }
}
