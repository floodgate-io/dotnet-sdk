using System;

namespace FloodGate.SDK
{
    public interface ILogger
    {
        /// <summary>
        /// Write debug logs
        /// </summary>
        void Debug(string message);

        /// <summary>
        /// Write info logs
        /// </summary>
        void Info(string message);

        /// <summary>
        /// Write warning logs
        /// </summary>
        void Warning(string message);

        /// <summary>
        /// Write error logs
        /// </summary>
        void Error(string message);

        /// <summary>
        /// Write error logs from Exception
        /// </summary>
        /// <param name="exception"></param>
        void Error(Exception exception);
    }
}
