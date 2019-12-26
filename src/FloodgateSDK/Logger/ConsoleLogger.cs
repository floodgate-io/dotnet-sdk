using System;

namespace FloodGate.SDK
{
    public class ConsoleLogger : LoggerBase, ILogger
    {
        public void Debug(string message)
        {
            WriteLog(message, ConsoleColor.Green, LogLevel.Debug);
        }

        public void Error(string message)
        {
            WriteLog(message, ConsoleColor.Red, LogLevel.Error);
        }

        public void Error(Exception exception)
        {
            WriteLog(exception.Message, ConsoleColor.Red, LogLevel.Error);
        }

        public void Info(string message)
        {
            WriteLog(message);
        }

        public void Warning(string message)
        {
            WriteLog(message, ConsoleColor.Yellow, LogLevel.Warning);
        }

        private void WriteLog(string message, ConsoleColor colour = ConsoleColor.White, LogLevel logLevel = LogLevel.Info)
        {
            Console.ForegroundColor = colour;
            Console.WriteLine(FormatMessage(message, logLevel));
            Console.ResetColor();
        }
    }
}
