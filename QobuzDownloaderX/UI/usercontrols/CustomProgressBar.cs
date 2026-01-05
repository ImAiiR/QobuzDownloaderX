using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace QobuzDownloaderX.UserControls
{
    public class CustomProgressBar : ProgressBar
    {
        protected override CreateParams CreateParams // Avoids flickering.
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000; // WS_EX_COMPOSITED
                return cp;
            }
        }

        private Color _backgroundColor = SystemColors.Window;
        private Color _fillColor = Color.RoyalBlue;
        private Color _borderColor = Color.Black;

        [Category("Appearance")]
        public Color BackgroundColor
        {
            get => _backgroundColor;
            set { _backgroundColor = value; Invalidate(); }
        }

        [Category("Appearance")]
        public Color FillColor
        {
            get => _fillColor;
            set { _fillColor = value; Invalidate(); }
        }

        [Category("Appearance")]
        public Color BorderColor
        {
            get => _borderColor;
            set { _borderColor = value; Invalidate(); }
        }

        public CustomProgressBar()
        {
            // Enable custom painting
            this.SetStyle(ControlStyles.UserPaint, true);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            // Draw background
            using (SolidBrush bgBrush = new SolidBrush(_backgroundColor))
            {
                e.Graphics.FillRectangle(bgBrush, this.ClientRectangle);
            }

            // Calculate fill width
            float percentage = (float)(this.Value - this.Minimum) / (this.Maximum - this.Minimum);
            Rectangle fillRect = new Rectangle(0, 0, (int)(this.Width * percentage), this.Height);

            // Draw fill
            using (SolidBrush fillBrush = new SolidBrush(_fillColor))
            {
                e.Graphics.FillRectangle(fillBrush, fillRect);
            }

            // Draw border
            using (Pen pen = new Pen(_borderColor))
            {
                e.Graphics.DrawRectangle(pen, 0, 0, this.Width - 1, this.Height - 1);
            }

            // Draw text
            string text = $"{(int)(percentage * 100)}%";

            // Build vector path for the text
            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddString(
                    text,
                    this.DesignMode ? this.Font.FontFamily: Application.OpenForms.OfType<qbdlxForm>().Single().Font.FontFamily,
                    (int)this.Font.Style,
                    this.Font.SizeInPoints * e.Graphics.DpiY / 72, // Exact pixel size
                    this.ClientRectangle,
                    new StringFormat()
                    {
                        Alignment = StringAlignment.Center,
                        LineAlignment = StringAlignment.Center
                    }
                );

                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                // Fill inside text
                using (Brush fillBrush = new SolidBrush(this.ForeColor))
                {
                    e.Graphics.FillPath(fillBrush, path);
                }
            }

        }

    }
}
