using System.Diagnostics;

namespace QobuzDownloaderX
{
    internal class DownloadStats
    {
        public Stopwatch SpeedWatch { get; set; }
        public long CumulativeBytesRead { get; set; } = 0;
        public long LastUiBytes { get; set; } = 0;
        public long LastUiTimeMs { get; set; } = 0;
        public string LastSpeedText { get; set; } = "";
    }
}