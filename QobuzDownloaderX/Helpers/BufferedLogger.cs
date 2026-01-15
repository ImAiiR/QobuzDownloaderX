using QobuzDownloaderX.Win32;
using System;
using System.IO;
using System.Text;
using ZetaLongPaths;

namespace QobuzDownloaderX.Helpers
{
    // Upsides of this buffering approach:
    //
    //   - Thread-safe file writes.
    //
    //   - Flexibility in the write/flush calls, which:
    //       Minimizes application slowdown by performing less writes to large log files.
    //       Minimizes disk drive wear and tear by limiting unnecessary I/O (read/write) operations.
    //
    // Downsides of this buffering approach:
    //
    //   - For those who like to see log files updating in real time,
    //     log entries are buffered so will not appear in the file immediately.
    //
    //   - If the process is terminated abruptly (e.g., using Task Manager),
    //     any remaining log entries in '_buffer' cannot be written to log file. 
    internal sealed class BufferedLogger : IDisposable
    {
        private const int flushThreshold = 1024 * 1024; // 1 MB
        private const long maxLogFileSize = 50L * 1024L * 1024L; // 50 MB

        private string _filePath;
        private StreamWriter _writer;
        private readonly StringBuilder _buffer;
        private bool _disposed = false;
        private readonly object _lock = new object();

        public BufferedLogger(string filePath)
        {
            _filePath = filePath;
            _buffer = new StringBuilder();

            _writer = new StreamWriter(new FileStream(_filePath, FileMode.Append, FileAccess.Write, FileShare.Read), Encoding.UTF8)
            {
                AutoFlush = false
            };
        }

        private void WriteLog(string level, string message)
        {
            var logMessage = $"[{DateTime.Now}] [{level}] {message}";

            lock (_lock) // Thread-safety
            {
                if (_disposed)
                {
                    // Ignore writes after a 'Logger.Dispose()' call
                    return;
                }

                try
                {
                    this.RotateToNewLogFileIfNeeded();

                    // Write to console immediately
                    System.Diagnostics.Debug.WriteLine($"{level} | {message}");

                    // Add to buffer
                    _buffer.AppendLine(logMessage);

                    // If buffer exceeds threshold, flush to file
                    if (_buffer.Length >= BufferedLogger.flushThreshold)
                    {
                        _writer.Write(_buffer.ToString());
                        _buffer.Clear();
                        _writer.Flush();
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Somehow, the log failed to write, lol");
                    System.Diagnostics.Debug.WriteLine(logMessage);
                    System.Diagnostics.Debug.WriteLine(ex);
                }
            }
        }

        public void Debug(string message) => WriteLog("DEBUG", message);

        public void Info(string message) => WriteLog("INFO", message);

        public void Warning(string message) => WriteLog("WARNING", message);

        public void Error(string message) => WriteLog("ERROR", message);

        private void RotateToNewLogFileIfNeeded()
        {
            if (!ZlpIOHelper.FileExists(_filePath))
                return;

            long size = new FileInfo(_filePath).Length;

            if (size < BufferedLogger.maxLogFileSize)
                return;

            if (_buffer.Length > 0)
            {
                _writer.Write(_buffer.ToString());
                _buffer.Clear();
                _writer.Flush();
            }

            _writer.Dispose();

            string newName = Miscellaneous.GetDuplicateFileName(_filePath);
            System.Diagnostics.Debug.WriteLine($"Rotating to new log file: {newName}");

            _writer = new StreamWriter(new FileStream(newName, FileMode.Append, FileAccess.Write, FileShare.None), Encoding.UTF8)
            {
                AutoFlush = false
            };

            _filePath = newName;
        }

        public void Dispose()
        {
            lock (_lock)
            {
                if (!_disposed)
                {
                    try
                    {
                        // Flush any remaining buffered logs
                        if (_buffer.Length > 0)
                        {
                            _writer.Write(_buffer.ToString());
                            _buffer.Clear();
                            _writer.Flush();
                        }

                        _writer.Dispose();
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine("Failed to flush logs to log file on Logger.Dispose().");
                        System.Diagnostics.Debug.WriteLine(ex);
                    }

                    _disposed = true;
                }
            }
        }
    }
}
