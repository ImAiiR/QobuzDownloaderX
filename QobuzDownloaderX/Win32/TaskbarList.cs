
namespace QobuzDownloaderX.Win32
{
    public sealed class TaskbarList
    {
        private static readonly object lockObj = new object();
        private static ITaskbarList4 taskbarList;

        private TaskbarList() { }

        public static ITaskbarList4 Instance
        {
            get
            {
                if (taskbarList == null)
                {
                    lock (lockObj)
                    {
                        if (taskbarList == null)
                        {
                            taskbarList = (ITaskbarList4)new CTaskbarList();
                            taskbarList.HrInit();
                        }
                    }
                }
                return taskbarList;
            }
        }
    }
}
