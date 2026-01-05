namespace QobuzDownloaderX.UI
{
    // ***********************************************************************
    // Author   : ElektroStudios
    // Modified : 20-January-2024
    // ***********************************************************************

    // ChatGPT code conversion from VB.NET to C# 7.3

    #region Public Members Summary

    #region Properties
    // BackgroundColor As Color
    // BackgroundOpacity As Double
    // CloseOnEscapeKey As Boolean
    // CloseOnLeftMouseClick As Boolean
    // ContextMenuStrip As ContextMenuStrip
    // FitBoundsToWorkingArea
    // Image As Image
    // ImageBorder As Boolean
    // ImageBorderColor As Color
    // ImageBorderSize As Integer
    // ImageLayout As ImageLayout
    // TitleBar As Boolean
    // TopMost As Boolean
    // TransparencyKey As Color
    #endregion

    #endregion

    #region Usings

    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    #endregion

    #region DevFloatingPicture

    namespace DevCase.UI.Components
    {
        /// <summary>
        /// A control that displays a picture in a stylish way that is similar to Telegram client.
        /// </summary>
        /// 
        /// <example>
        /// This is a code example.
        /// <code language="C#">
        /// string imagePath = @"C:\Image.jpg";
        /// Image img = Image.FromFile(imagePath);
        ///
        /// using (var fpic = new DevFloatingPicture(img)
        /// {
        ///     ImageLayout = ImageLayout.Zoom,
        ///     BackgroundColor = Color.Black,
        ///     BackgroundOpacity = 0.75,
        ///     FitBoundsToWorkingArea = true,
        ///     TopMost = true,
        ///     TitleBar = false,
        ///     ImageBorder = true,
        ///     ImageBorderColor = Color.FromArgb(255, 30, 30, 30),
        ///     ImageBorderSize = 5,
        ///     CloseOnEscapeKey = true,
        ///     CloseOnLeftMouseClick = false
        /// })
        /// {
        ///     fpic.ShowDialog();
        /// }
        /// </code>
        /// </example>
        [ToolboxItem(true)]
        [DesignerCategory(nameof(DesignerCategoryAttribute.Component))]
        [DisplayName(nameof(DevFloatingPicture))]
        [Description("A component to display a picture on the screen in a Telegram-like style.")]
        [DesignTimeVisible(true)]
        [ToolboxBitmap(typeof(PictureBox), "PictureBox.bmp")]
        [ToolboxItemFilter("System.Windows.Forms", ToolboxItemFilterType.Allow)]
        [ComVisible(true)]
        [DefaultBindingProperty(nameof(DevFloatingPicture.Image))]
        [DefaultProperty(nameof(DevFloatingPicture.Image))]
        public class DevFloatingPicture : Component
        {
            #region Private Fields

            /// <summary>
            /// The background Form where FrontForm is hosted.
            /// </summary>
            protected Form BackForm;

            /// <summary>
            /// The Form where the picture is shown.
            /// </summary>
            protected Form FrontForm;

            #endregion

            #region Properties

            /// <summary>
            /// Gets or sets the image to show in this control.
            /// </summary>
            public Image Image
            {
                get => image_;
                set
                {
                    image_ = value;
                    FrontForm.BackgroundImage = value;
                }
            }

            /// <summary>
            /// Backing field
            /// </summary>
            private Image image_;

            /// <summary>
            /// Gets or sets the image layout.
            /// Default value is Zoom
            /// </summary>
            public ImageLayout ImageLayout
            {
                get => FrontForm.BackgroundImageLayout;
                set => FrontForm.BackgroundImageLayout = value;
            }

            /// <summary>
            /// Gets or sets a value indicating whether to fit the control bounds to the desktop working area.
            /// Default = False
            /// </summary>
            public bool FitBoundsToWorkingArea
            {
                get => fitBoundsToWorkingArea_;
                set
                {
                    if (fitBoundsToWorkingArea_ == value)
                        return;

                    fitBoundsToWorkingArea_ = value;
                    AdjustFormSizes();
                }
            }

            private bool fitBoundsToWorkingArea_;

            /// <summary>
            /// Gets or sets the context menu for this control.
            /// </summary>
            public ContextMenuStrip ContextMenuStrip
            {
                get => contextMenuStrip_;
                set
                {
                    if (!Equals(contextMenuStrip_, value))
                    {
                        contextMenuStrip_?.Dispose();
                        contextMenuStrip_ = value;

                        BackForm.ContextMenuStrip = value;
                        FrontForm.ContextMenuStrip = value;
                    }
                }
            }

            private ContextMenuStrip contextMenuStrip_;

            /// <summary>
            /// Gets or sets whether the control has a title bar.
            /// Default false
            /// </summary>
            public bool TitleBar
            {
                get => FrontForm.FormBorderStyle == FormBorderStyle.FixedSingle;
                set => FrontForm.FormBorderStyle = value ? FormBorderStyle.FixedSingle : FormBorderStyle.None;
            }

            /// <summary>
            /// Gets or sets topmost.
            /// Default false
            /// </summary>
            public bool TopMost
            {
                get => FrontForm.TopMost;
                set
                {
                    BackForm.TopMost = value;
                    FrontForm.TopMost = value;

                    if (value)
                    {
                        FrontForm.BringToFront();
                        FrontForm.Activate();
                    }
                }
            }

            /// <summary>
            /// Gets or sets background color.
            /// Default Black
            /// </summary>
            public Color BackgroundColor
            {
                get => BackForm.BackColor;
                set => BackForm.BackColor = value;
            }

            /// <summary>
            /// Gets or sets opacity (not image)
            /// Default 0.75
            /// </summary>
            public double BackgroundOpacity
            {
                get => BackForm.Opacity;
                set => BackForm.Opacity = value;
            }

            /// <summary>
            /// Transparency key
            /// Default Fuchsia
            /// </summary>
            public Color TransparencyKey
            {
                get => FrontForm.TransparencyKey;
                set
                {
                    FrontForm.TransparencyKey = value;
                    FrontForm.BackColor = value;
                }
            }

            public bool CloseOnEscapeKey { get; set; } = true;
            public bool CloseOnLeftMouseClick { get; set; } = true;

            public bool ImageBorder { get; set; }
            public int ImageBorderSize { get; set; } = 2;
            public Color ImageBorderColor { get; set; } = Color.Black;

            #endregion

            #region Constructors

            [DebuggerStepThrough]
            public DevFloatingPicture() : this(null) { }

            /// <summary>
            /// Initializes new instance.
            /// </summary>
            public DevFloatingPicture(Image img)
            {
                image_ = img;

                BackForm = new Form
                {
                    Visible = false,
                    IsMdiContainer = false,
                    FormBorderStyle = FormBorderStyle.None,
                    MaximizeBox = false,
                    MinimizeBox = false,
                    BackColor = Color.Black,
                    Opacity = 0.75,
                    WindowState = FormWindowState.Normal,
                    ShowInTaskbar = false,
                    TabStop = false,
                    TopMost = false,
                    CausesValidation = false,
                    ControlBox = false,
                    Margin = Padding.Empty,
                    Padding = Padding.Empty,
                    SizeGripStyle = SizeGripStyle.Hide,
                    StartPosition = FormStartPosition.CenterScreen,
                    AutoScaleMode = AutoScaleMode.None,
                    BackgroundImageLayout = ImageLayout.None,
                    ShowIcon = false,
                    ClientSize = new Size(0, 0),
                    Size = new Size(0, 0)
                };

                FrontForm = new Form
                {
                    Visible = false,
                    Owner = BackForm,
                    Dock = DockStyle.Fill,
                    FormBorderStyle = FormBorderStyle.None,
                    MaximizeBox = false,
                    MinimizeBox = false,
                    Opacity = 1,
                    ShowIcon = false,
                    WindowState = FormWindowState.Normal,
                    BackgroundImage = image_,
                    BackgroundImageLayout = ImageLayout.Zoom,
                    BackColor = Color.Fuchsia,
                    TransparencyKey = Color.Fuchsia,
                    ShowInTaskbar = false,
                    TabStop = false,
                    TopMost = false,
                    CausesValidation = false,
                    ControlBox = true,
                    Margin = Padding.Empty,
                    Padding = Padding.Empty,
                    SizeGripStyle = SizeGripStyle.Hide,
                    StartPosition = FormStartPosition.CenterScreen,
                    ClientSize = new Size(0, 0),
                    Size = new Size(0, 0)
                };

                contextMenuStrip_ = new ContextMenuStrip();
                contextMenuStrip_.Items.Add("&Close Picture", SystemIcons.Error.ToBitmap(), (s, e) => FrontForm?.Close());

                BackForm.ContextMenuStrip = contextMenuStrip_;
                FrontForm.ContextMenuStrip = contextMenuStrip_;

                GC.KeepAlive(FrontForm.Handle);
                GC.KeepAlive(BackForm.Handle);

                PropertyInfo doubleBufferProperty =
                    typeof(Form).GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);

                doubleBufferProperty?.SetValue(FrontForm, true);

                // Event Subscriptions (VB Handles Equivalent)
                BackForm.FormClosed += Forms_FormClosed;
                FrontForm.FormClosed += Forms_FormClosed;

                BackForm.KeyPress += Forms_KeyPress;
                FrontForm.KeyPress += Forms_KeyPress;

                BackForm.MouseClick += Forms_MouseClick;
                FrontForm.MouseClick += Forms_MouseClick;

                BackForm.GotFocus += BackForm_GotFocus;
                BackForm.MouseDown += BackForm_MouseDown;
                BackForm.MouseHover += BackForm_MouseHover;

                FrontForm.Paint += FrontForm_Paint;
            }

            #endregion

            #region Public Methods

            public void Show(IWin32Window owner = null)
            {
                ShowWithoutFlickering(false, owner);
            }

            public DialogResult ShowDialog(IWin32Window owner = null)
            {
                return ShowWithoutFlickering(true, owner);
            }

            public void Close()
            {
                FrontForm.Close();
            }

            #endregion

            #region Private Methods

            protected virtual void AdjustFormSizes()
            {
                Screen screen = Screen.FromControl(FrontForm);
                Rectangle workingArea = fitBoundsToWorkingArea_ ? screen.WorkingArea : screen.Bounds;

                BackForm.Location = workingArea.Location;
                BackForm.Size = workingArea.Size;

                FrontForm.Location = workingArea.Location;
                FrontForm.Size = workingArea.Size;

                Application.DoEvents();
            }

            private DialogResult ShowWithoutFlickering(bool dialogModal, IWin32Window owner = null)
            {
                AdjustFormSizes();

                double previousBackFormOpacity = BackForm.Opacity;
                BackForm.Opacity = 0;
                FrontForm.Opacity = 0;

                Point previousBackFormLocation = BackForm.Location;
                Point previousFrontFormLocation = FrontForm.Location;

                BackForm.Location = new Point(-10000, -10000);
                FrontForm.Location = new Point(-10000, -10000);

                BackForm.Show();

                void antiFlickerHandler(object s, EventArgs e)
                {
                    FrontForm.Shown -= antiFlickerHandler;

                    Task.Factory.StartNew(() =>
                    {
                        Thread.Sleep(20);

                        BackForm.Invoke((MethodInvoker)(() => BackForm.Opacity = previousBackFormOpacity));

                        FrontForm.Invoke((MethodInvoker)(() =>
                        {
                            FrontForm.Opacity = 1;
                            FrontForm.BringToFront();
                            FrontForm.Focus();
                        }));

                        BackForm.Invoke((MethodInvoker)(() => BackForm.Location = previousBackFormLocation));
                        FrontForm.Invoke((MethodInvoker)(() => FrontForm.Location = previousFrontFormLocation));
                    });
                }

                FrontForm.Shown += antiFlickerHandler;

                if (dialogModal)
                    return FrontForm.ShowDialog(owner);

                FrontForm.Show(owner);
                return DialogResult.None;
            }

            protected override void Dispose(bool disposing)
            {
                if (disposing)
                {
                    FrontForm?.Dispose();
                    BackForm?.Dispose();
                }

                base.Dispose(disposing);
            }

            #endregion

            #region Event Handlers

            protected virtual void Forms_FormClosed(object sender, FormClosedEventArgs e)
            {
                BackForm?.Dispose();
                FrontForm?.Dispose();
            }

            protected virtual void Forms_KeyPress(object sender, KeyPressEventArgs e)
            {
                if (e.KeyChar == Convert.ToChar(Keys.Escape) && CloseOnEscapeKey)
                    ((Form)sender).Close();
            }

            protected virtual void Forms_MouseClick(object sender, MouseEventArgs e)
            {
                if (e.Button == MouseButtons.Left && CloseOnLeftMouseClick)
                    ((Form)sender).Close();
            }

            protected virtual void BackForm_GotFocus(object sender, EventArgs e)
            {
                if (!CloseOnLeftMouseClick)
                    FrontForm?.Focus();
            }

            protected virtual void BackForm_MouseDown(object sender, MouseEventArgs e)
            {
                if (!CloseOnLeftMouseClick)
                    FrontForm?.Focus();
            }

            protected virtual void BackForm_MouseHover(object sender, EventArgs e)
            {
                if (!CloseOnLeftMouseClick)
                    FrontForm?.Focus();
            }

            protected virtual void FrontForm_Paint(object sender, PaintEventArgs e)
            {
                if (!ImageBorder || ImageBorderSize <= 0)
                    return;

                if (DesignMode)
                    return;

                Rectangle imageRect;

                switch (FrontForm.BackgroundImageLayout)
                {
                    case ImageLayout.Stretch:
                    case ImageLayout.Tile:
                        imageRect = FrontForm.ClientRectangle;
                        break;

                    case ImageLayout.Center:
                        imageRect = new Rectangle(
                            (FrontForm.ClientSize.Width - Image.Width) / 2,
                            (FrontForm.ClientSize.Height - Image.Height) / 2,
                            Image.Width, Image.Height);
                        break;

                    case ImageLayout.Zoom:
                        Size imageSize = Image.Size;
                        Size clientSize = FrontForm.ClientSize;
                        double aspectRatio = (double)imageSize.Width / imageSize.Height;

                        imageRect = new Rectangle();

                        if (clientSize.Width / aspectRatio <= clientSize.Height)
                        {
                            imageRect.Width = clientSize.Width;
                            imageRect.Height = (int)(clientSize.Width / aspectRatio);
                        }
                        else
                        {
                            imageRect.Width = (int)(clientSize.Height * aspectRatio);
                            imageRect.Height = clientSize.Height;
                        }

                        imageRect.X = (clientSize.Width - imageRect.Width) / 2;
                        imageRect.Y = (clientSize.Height - imageRect.Height) / 2;
                        break;

                    default:
                        imageRect = new Rectangle(0, 0, Image.Width, Image.Height);
                        break;
                }

                using (Pen pen = new Pen(ImageBorderColor, ImageBorderSize))
                {
                    e.Graphics.DrawRectangle(pen, imageRect);
                }
            }

            #endregion
        }
    }

    #endregion

}
