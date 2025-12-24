using QobuzDownloaderX.Win32;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace QobuzDownloaderX.Helpers
{
    internal sealed class TaskbarHelper
    {
        private static readonly bool isRunningOnWin8OrLater =
            Environment.OSVersion.Platform == PlatformID.Win32NT &&
            Environment.OSVersion.Version.CompareTo(new Version(6, 2)) >= 0;

        [DebuggerStepThrough]
        public static void SetProgressValue(int currentValue, int maximumValue)
        {
            if (isRunningOnWin8OrLater) TaskbarList.Instance.SetProgressValue(Process.GetCurrentProcess().MainWindowHandle, Convert.ToUInt32(currentValue), Convert.ToUInt32(maximumValue));
            qbdlxForm.lastTaskBarProgressCurrentValue = currentValue;
            qbdlxForm.lastTaskBarProgressMaxValue = maximumValue;

            if (!qbdlxForm._qbdlxForm.Visible)
            {
                using (Icon baseIcon = (Icon)Properties.Resources.QBDLX_Icon1.Clone())
                {
                    NotifyIconHelper.RenderNotifyIconProgressBar(
                        qbdlxForm._qbdlxForm.notifyIcon1,
                        baseIcon,
                        qbdlxForm.ntfyProgressBar,
                        currentValue,
                        maximumValue);
                }
            }
        }

        [DebuggerStepThrough]
        public static void SetProgressState(TaskbarProgressState state)
        {
            if (isRunningOnWin8OrLater) TaskbarList.Instance.SetProgressState(Process.GetCurrentProcess().MainWindowHandle, state);
            qbdlxForm.lastTaskBarProgressState = state;
            qbdlxForm._qbdlxForm.notifyIcon1.Icon = Properties.Resources.QBDLX_Icon1;

            switch (state)
            {
                case TaskbarProgressState.Paused:
                    qbdlxForm.ntfyProgressBar.FillColor = Color.Yellow;
                    break;

                case TaskbarProgressState.Error:
                    qbdlxForm.ntfyProgressBar.FillColor = Color.Red;
                    break;

                default:
                    qbdlxForm.ntfyProgressBar.FillColor = Color.LimeGreen;
                    break;
            }
        }

        // UNUSED
        // ======
        //
        //[DebuggerStepThrough]
        //public static void SetProgressValue(int currentValue, int maximumValue, IntPtr windowHandle)
        //{
        //    if (isRunningOnWin8OrLater) TaskbarList.Instance.SetProgressValue(windowHandle, Convert.ToUInt32(currentValue), Convert.ToUInt32(maximumValue));
        //    qbdlxForm.lastTaskBarProgressCurrentValue = currentValue;
        //    qbdlxForm.lastTaskBarProgressMaxValue = maximumValue;
        //
        //}
        
        //[DebuggerStepThrough]
        //public static void SetProgressState(TaskbarProgressState state, IntPtr windowHandle)
        //{
        //    if (isRunningOnWin8OrLater) TaskbarList.Instance.SetProgressState(windowHandle, state);
        //    qbdlxForm.lastTaskBarProgressState = state;
        //}
    }
}
