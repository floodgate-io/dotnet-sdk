using System;

namespace FloodGate.SDK
{
    public class LoggerBase : ILoggerBase
    {
        public string FormatMessage(string message, LogLevel logLevel = LogLevel.Info)
        {
            return $"{DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss")} - {logLevel} - {message}";
        }
    }
}
