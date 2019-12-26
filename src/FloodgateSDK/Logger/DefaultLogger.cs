using System;

namespace FloodGate.SDK
{
    public class DefaultLogger : LoggerBase, ILogger
    {
        public void Debug(string message) { }

        public void Error(string message) { }

        public void Error(Exception exception) { }

        public void Info(string message) { }

        public void Warning(string message) { }
    }
}
