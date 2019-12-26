namespace FloodGate.SDK
{
    public enum LogLevel
    {
        /// <summary>
        /// Don't perform any logging
        /// </summary>
        Disabled = 0,

        /// <summary>
        /// Log additional debug information
        /// </summary>
        Debug = 1,

        /// <summary>
        /// Log and throw error and terminate application execution
        /// </summary>
        Error = 2,

        /// <summary>
        /// Log warning but continue application execution
        /// </summary>
        Warning = 3,

        /// <summary>
        /// Log application execution information
        /// </summary>
        Info = 4
    }
}
