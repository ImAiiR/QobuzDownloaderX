using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QobuzDownloaderX.Download
{
    class FixMD5
    {
        GetInfo getInfo = new GetInfo();

        public string outputResult { get; set; }
        public string driveLetter { get; set; }
        public string cmdText { get; set; }

        public void fixMD5(string filePath, string flacEXEPath)
        {
            Console.WriteLine("Attempting to fix unset MD5...");
            driveLetter = filePath.Substring(0, 2);
            Console.WriteLine("Drive letter - " + driveLetter);
            Console.WriteLine("File Path - " + filePath);
            Console.WriteLine("FLAC exe Path - " + flacEXEPath);
            cmdText = "/C echo Fixing unset MD5s... & \"" + flacEXEPath + "\" -f8 \"" + filePath + "\"";
            Console.WriteLine("Commands - " + cmdText);

            try
            {
                Process cmd = new Process();
                cmd.StartInfo.FileName = "cmd.exe";
                cmd.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                cmd.StartInfo.Arguments = cmdText;
                cmd.Start();
                cmd.WaitForExit();
                outputResult = "COMPLETE";
            }
            catch (Exception fixMD5ex)
            {
                outputResult = "Failed";
                Console.WriteLine("Failed to fix MD5s");
                Console.WriteLine(fixMD5ex);
            }
        }
    }
}
