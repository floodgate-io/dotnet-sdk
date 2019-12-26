namespace FloodGate.SDK
{
    interface ILoggerBase
    {
        string FormatMessage(string message, LogLevel logLevel);
    }
}
