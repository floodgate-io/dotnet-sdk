using System;
using System.IO;
using System.Threading.Tasks;

namespace FloodGate.SDK
{
    public class FileLogger : LoggerBase, ILogger
    {
        private string logFilePath;

        public FileLogger(string logFilePath)
        {
            this.logFilePath = logFilePath;
        }

        public void Debug(string message)
        {
            WriteLogFileAsync(message, LogLevel.Debug).ConfigureAwait(false);
        }

        public void Error(string message)
        {
            WriteLogFileAsync(message, LogLevel.Error).ConfigureAwait(false);
        }

        public void Error(Exception exception)
        {
            WriteLogFileAsync(exception.Message, LogLevel.Error).ConfigureAwait(false);
        }

        public void Info(string message)
        {
            WriteLogFileAsync(message).ConfigureAwait(false);
        }

        public void Warning(string message)
        {
            WriteLogFileAsync(message, LogLevel.Warning).ConfigureAwait(false);
        }

        /// <summary>
        /// Write log file line one by one
        /// </summary>
        /// <param name="message"></param>
        private void WriteLogFile(string message, LogLevel logLevel = LogLevel.Info)
        {
            using (StreamWriter w = File.AppendText(logFilePath))
            {
                w.WriteLine(FormatMessage(message));
            }
        }

        private async Task WriteLogFileAsync(string message, LogLevel logLevel = LogLevel.Info)
        {
            using (StreamWriter w = File.AppendText(logFilePath))
            {
                await w.WriteLineAsync(FormatMessage(message)).ConfigureAwait(false);
            }
        }
    }
}
