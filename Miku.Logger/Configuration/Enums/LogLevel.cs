namespace Miku.Logger.Configuration.Enums
{
    /// <summary>
    /// Log severity levels.
    /// </summary>
    /// <remarks>
    /// From the softest whisper (Trace) to the most critical crescendo (Critical),
    /// each level represents the importance of the message.
    /// </remarks>
    public enum LogLevel
    {
        /// <summary>Logs that contain the most detailed messages.</summary>
        Trace = 0,

        /// <summary>Logs used for interactive investigation during development.</summary>
        Debug = 1,

        /// <summary>Logs that track the general flow of the application.</summary>
        Information = 2,

        /// <summary>Logs that highlight abnormal or unexpected events.</summary>
        Warning = 3,

        /// <summary>Logs that highlight when the current flow of execution is stopped due to a failure.</summary>
        Error = 4,

        /// <summary>Logs that describe an unrecoverable application or system crash.</summary>
        Critical = 5,

        /// <summary>Not used for writing log messages. Specifies that no messages should be written.</summary>
        None = 6
    }
}
