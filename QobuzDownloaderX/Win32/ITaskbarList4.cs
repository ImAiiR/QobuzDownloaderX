using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace QobuzDownloaderX.Win32
{

    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("C43DC798-95D1-4BEA-9030-BB99E2983A1A")]
    public interface ITaskbarList4
    {
        // ITaskbarList
        [PreserveSig]
        int HrInit();

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [PreserveSig]
        int NotImplemented_AddTab();

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [PreserveSig]
        int NotImplemented_DeleteTab();

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [PreserveSig]
        int NotImplemented_ActivateTab();

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [PreserveSig]
        int NotImplemented_SetActiveAlt();

        // ITaskbarList2
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [PreserveSig]
        int NotImplemented_MarkFullscreenWindow();

        // ITaskbarList3
        [PreserveSig]
        int SetProgressValue(IntPtr hWnd, ulong ullCompleted, ulong ullTotal);

        [PreserveSig]
        int SetProgressState(IntPtr hWnd, TaskbarProgressBarState tbpFlags);

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [PreserveSig]
        int NotImplemented_RegisterTab();

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [PreserveSig]
        int NotImplemented_UnregisterTab();

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [PreserveSig]
        int NotImplemented_SetTabOrder();

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [PreserveSig]
        int NotImplemented_SetTabActive();

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [PreserveSig]
        int NotImplemented_ThumbBarAddButtons();

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [PreserveSig]
        int NotImplemented_ThumbBarUpdateButtons();

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [PreserveSig]
        int NotImplemented_ThumbBarSetImageList();

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [PreserveSig]
        int NotImplemented_SetOverlayIcon();

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [PreserveSig]
        int NotImplemented_SetThumbnailTooltip();

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [PreserveSig]
        int NotImplemented_SetThumbnailClip();

        // ITaskbarList4
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [PreserveSig]
        int NotImplemented_SetTabProperties();
    }
}
