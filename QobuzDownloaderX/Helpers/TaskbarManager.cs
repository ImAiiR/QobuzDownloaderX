using System;
using System.Diagnostics;

using QobuzDownloaderX.Win32;

namespace QobuzDownloaderX
{
    internal sealed class TaskbarManager
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
        }

        [DebuggerStepThrough]
        public static void SetProgressValue(int currentValue, int maximumValue, IntPtr windowHandle)
        {
            if (isRunningOnWin8OrLater) TaskbarList.Instance.SetProgressValue(windowHandle, Convert.ToUInt32(currentValue), Convert.ToUInt32(maximumValue));
            qbdlxForm.lastTaskBarProgressCurrentValue = currentValue;
            qbdlxForm.lastTaskBarProgressMaxValue = maximumValue;
        }

        [DebuggerStepThrough]
        public static void SetProgressState(TaskbarProgressBarState state)
        {
            if (isRunningOnWin8OrLater) TaskbarList.Instance.SetProgressState(Process.GetCurrentProcess().MainWindowHandle, state);
            qbdlxForm.lastTaskBarProgressState = state;
        }

        [DebuggerStepThrough]
        public static void SetProgressState(TaskbarProgressBarState state, IntPtr windowHandle)
        {
            if (isRunningOnWin8OrLater) TaskbarList.Instance.SetProgressState(windowHandle, state);
            qbdlxForm.lastTaskBarProgressState = state;
        }
    }
}
