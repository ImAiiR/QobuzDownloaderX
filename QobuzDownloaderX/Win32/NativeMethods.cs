using System;
using System.Runtime.InteropServices;
using System.Text;

namespace QobuzDownloaderX.Win32
{
    internal class NativeMethods
    {
        [DllImport("gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        public static extern IntPtr CreateRoundRectRgn
        (
            int nLeftRect,     // x-coordinate of upper-left corner
            int nTopRect,      // y-coordinate of upper-left corner
            int nRightRect,    // x-coordinate of lower-right corner
            int nBottomRect,   // y-coordinate of lower-right corner
            int nWidthEllipse, // width of ellipse
            int nHeightEllipse // height of ellipse
        );

        [DllImport("shell32.dll", CharSet = CharSet.Unicode, SetLastError = false)]
        public static extern bool PathYetAnotherMakeUniqueName(
            StringBuilder pszUniqueName,   // Output buffer
            string pszPath,                // Folder path
            string pszShort,               // Optional short name (can be null)
            string pszFile                 // Original file name
        );

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool DestroyIcon(IntPtr hIcon);

        [DllImport("user32.dll")]
        public static extern bool IsIconic(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
    }
}
