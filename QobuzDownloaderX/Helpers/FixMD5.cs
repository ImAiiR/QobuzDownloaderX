using System;
using System.Diagnostics;

namespace QobuzDownloaderX
{
    class FixMD5
    {
        readonly GetInfo getInfo = new GetInfo();

        public string outputResult { get; set; }

        public void fixMD5(string filePath, string flacEXEPath)
        {
            qbdlxForm._qbdlxForm.logger.Debug("Attempting to fix unset MD5...");
            // string driveLetter = filePath.Substring(0, 2); // UNUSED
            string cmdText = "/C echo Fixing unset MD5s... & \"" + flacEXEPath + "\" -f8 \"" + filePath + "\"";
            qbdlxForm._qbdlxForm.logger.Debug("Commands - " + cmdText);

            try
            {
                qbdlxForm._qbdlxForm.logger.Debug("Running cmd to run ffmpeg command to fix MD5");
                Process cmd = new Process();
                cmd.StartInfo.FileName = "cmd.exe";
                cmd.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                cmd.StartInfo.Arguments = cmdText;
                cmd.Start();
                cmd.WaitForExit();
                outputResult = "COMPLETE";
                qbdlxForm._qbdlxForm.logger.Debug("MD5 has been fixed for file!");
            }
            catch (Exception fixMD5ex)
            {
                outputResult = "FAILED";
                qbdlxForm._qbdlxForm.logger.Error("Failed to fix MD5s, error below:\r\n" + fixMD5ex);
            }
        }
    }
}
