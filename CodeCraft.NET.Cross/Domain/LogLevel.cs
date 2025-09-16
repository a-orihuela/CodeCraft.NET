namespace CodeCraft.NET.Cross.Domain
{
    /// <summary>
    /// Log level enumeration for application logging
    /// Maps to standard logging levels used throughout the application
    /// </summary>
    public enum LogLevel
    {
        /// <summary>
        /// Debug level for detailed diagnostic information
        /// </summary>
        Debug = 0,

        /// <summary>
        /// Information level for general information messages
        /// </summary>
        Info = 1,

        /// <summary>
        /// Warning level for potentially harmful situations
        /// </summary>
        Warning = 2,

        /// <summary>
        /// Error level for error events that might still allow the application to continue running
        /// </summary>
        Error = 3,

        /// <summary>
        /// Critical level for very severe error events that will presumably lead the application to abort
        /// </summary>
        Critical = 4
    }
}