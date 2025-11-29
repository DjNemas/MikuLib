using Microsoft.Extensions.Logging;
using Miku.Logger.Configuration;
using Miku.Logger.Writers;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Miku.Logger
{
    /// <summary>
    /// Advanced logging implementation with console and file output support.
    /// Implements Microsoft.Extensions.Logging.ILogger for compatibility.
    /// </summary>
    /// <remarks>
    /// Like a voice echoing through concerts since August 31st, 2007,
    /// this logger carries your messages with CV01 precision and clarity.
    /// </remarks>
    /// <example>
    /// <code>
    /// // Basic usage with automatic class name detection
    /// var logger = new MikuLogger();
    /// logger.LogInformation("Application started");
    /// 
    /// // Explicit category name
    /// var logger = new MikuLogger("MyApp");
    /// 
    /// // With options
    /// var options = new MikuLoggerOptions
    /// {
    ///     Output = LogOutput.ConsoleAndFile,
    ///     MinimumLogLevel = LogLevel.Debug
    /// };
    /// var logger = new MikuLogger("MyApp", options);
    /// 
    /// // Logging
    /// logger.LogInformation("Application started");
    /// logger.LogError("Error occurred: {Error}", exception.Message);
    /// await logger.LogInformationAsync("Async log message");
    /// </code>
    /// </example>
    public class MikuLogger : ILogger, IDisposable
    {
        // Mi-Ku in Japanese (3=Mi, 9=Ku)
        private const int MikuMagicNumber = 39;
        
        // Character Vocal Series 01
        private const string VocaloidId = "CV01";
        
        // Signature color in hex
        private const string MikuCyanColor = "#00CED1";

        private readonly string _categoryName;
        private readonly MikuLoggerOptions _options;
        private readonly ConsoleLogWriter? _consoleWriter;
        private readonly FileLogWriter? _fileWriter;
        private bool _disposed;

        /// <summary>
        /// Initializes a new instance of the MikuLogger class.
        /// If no category name is provided, the calling class name will be used automatically.
        /// </summary>
        /// <param name="categoryName">The category name for the logger. If null, uses the calling class name.</param>
        /// <param name="options">Configuration options for the logger.</param>
        public MikuLogger(string? categoryName = null, MikuLoggerOptions? options = null)
        {
            _categoryName = categoryName ?? GetCallingClassName();
            _options = options ?? new MikuLoggerOptions();

            if (_options.Output.HasFlag(LogOutput.Console))
            {
                _consoleWriter = new ConsoleLogWriter(_options.ConsoleColors);
            }

            if (_options.Output.HasFlag(LogOutput.File))
            {
                _fileWriter = new FileLogWriter(_options.FileOptions);
            }
        }

        /// <summary>
        /// Gets the name of the calling class using stack trace analysis.
        /// </summary>
        private static string GetCallingClassName()
        {
            var stackTrace = new StackTrace();
            
            // Skip first frames (GetCallingClassName, constructor, etc.)
            for (int i = 2; i < stackTrace.FrameCount; i++)
            {
                var frame = stackTrace.GetFrame(i);
                var method = frame?.GetMethod();
                
                if (method?.DeclaringType != null)
                {
                    var declaringType = method.DeclaringType;
                    
                    // Skip compiler-generated and system types
                    if (!declaringType.FullName?.StartsWith("System.") == true &&
                        !declaringType.FullName?.StartsWith("Microsoft.") == true &&
                        !declaringType.FullName?.Contains("<>") == true)
                    {
                        return declaringType.Name;
                    }
                }
            }
            
            return "Unknown";
        }

        #region ILogger Implementation

        /// <inheritdoc/>
        public IDisposable? BeginScope<TState>(TState state) where TState : notnull
        {
            return null;
        }

        /// <inheritdoc/>
        public bool IsEnabled(Microsoft.Extensions.Logging.LogLevel logLevel)
        {
            return ConvertLogLevel(logLevel) >= _options.MinimumLogLevel;
        }

        /// <inheritdoc/>
        public void Log<TState>(
            Microsoft.Extensions.Logging.LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception? exception,
            Func<TState, Exception?, string> formatter)
        {
            if (!IsEnabled(logLevel))
                return;

            var message = formatter(state, exception);
            var mikuLogLevel = ConvertLogLevel(logLevel);
            
            LogInternal(mikuLogLevel, message, exception);
        }

        #endregion

        #region Synchronous Logging Methods

        /// <summary>
        /// Logs a trace message.
        /// </summary>
        public void LogTrace(string message, params object[] args)
            => Log(Configuration.LogLevel.Trace, message, args);

        /// <summary>
        /// Logs a debug message.
        /// </summary>
        public void LogDebug(string message, params object[] args)
            => Log(Configuration.LogLevel.Debug, message, args);

        /// <summary>
        /// Logs an informational message.
        /// </summary>
        public void LogInformation(string message, params object[] args)
            => Log(Configuration.LogLevel.Information, message, args);

        /// <summary>
        /// Logs a warning message.
        /// </summary>
        public void LogWarning(string message, params object[] args)
            => Log(Configuration.LogLevel.Warning, message, args);

        /// <summary>
        /// Logs an error message.
        /// </summary>
        public void LogError(string message, params object[] args)
            => Log(Configuration.LogLevel.Error, message, args);

        /// <summary>
        /// Logs an error message with exception.
        /// </summary>
        public void LogError(Exception exception, string message, params object[] args)
            => Log(Configuration.LogLevel.Error, message, exception, args);

        /// <summary>
        /// Logs a critical message.
        /// </summary>
        public void LogCritical(string message, params object[] args)
            => Log(Configuration.LogLevel.Critical, message, args);

        /// <summary>
        /// Logs a critical message with exception.
        /// </summary>
        public void LogCritical(Exception exception, string message, params object[] args)
            => Log(Configuration.LogLevel.Critical, message, exception, args);

        private void Log(Configuration.LogLevel logLevel, string message, params object[] args)
            => Log(logLevel, message, null, args);

        private void Log(Configuration.LogLevel logLevel, string message, Exception? exception, params object[] args)
        {
            if (logLevel < _options.MinimumLogLevel)
                return;

            var formattedMessage = args.Length > 0 ? string.Format(message, args) : message;
            LogInternal(logLevel, formattedMessage, exception);
        }

        #endregion

        #region Asynchronous Logging Methods

        /// <summary>
        /// Logs a trace message asynchronously.
        /// </summary>
        public Task LogTraceAsync(string message, params object[] args)
            => LogAsync(Configuration.LogLevel.Trace, message, args);

        /// <summary>
        /// Logs a debug message asynchronously.
        /// </summary>
        public Task LogDebugAsync(string message, params object[] args)
            => LogAsync(Configuration.LogLevel.Debug, message, args);

        /// <summary>
        /// Logs an informational message asynchronously.
        /// </summary>
        public Task LogInformationAsync(string message, params object[] args)
            => LogAsync(Configuration.LogLevel.Information, message, args);

        /// <summary>
        /// Logs a warning message asynchronously.
        /// </summary>
        public Task LogWarningAsync(string message, params object[] args)
            => LogAsync(Configuration.LogLevel.Warning, message, args);

        /// <summary>
        /// Logs an error message asynchronously.
        /// </summary>
        public Task LogErrorAsync(string message, params object[] args)
            => LogAsync(Configuration.LogLevel.Error, message, args);

        /// <summary>
        /// Logs an error message with exception asynchronously.
        /// </summary>
        public Task LogErrorAsync(Exception exception, string message, params object[] args)
            => LogAsync(Configuration.LogLevel.Error, message, exception, args);

        /// <summary>
        /// Logs a critical message asynchronously.
        /// </summary>
        public Task LogCriticalAsync(string message, params object[] args)
            => LogAsync(Configuration.LogLevel.Critical, message, args);

        /// <summary>
        /// Logs a critical message with exception asynchronously.
        /// </summary>
        public Task LogCriticalAsync(Exception exception, string message, params object[] args)
            => LogAsync(Configuration.LogLevel.Critical, message, exception, args);

        private Task LogAsync(Configuration.LogLevel logLevel, string message, params object[] args)
            => LogAsync(logLevel, message, null, args);

        private async Task LogAsync(Configuration.LogLevel logLevel, string message, Exception? exception, params object[] args)
        {
            if (logLevel < _options.MinimumLogLevel)
                return;

            var formattedMessage = args.Length > 0 ? string.Format(message, args) : message;
            await LogInternalAsync(logLevel, formattedMessage, exception);
        }

        #endregion

        #region Internal Logging Logic

        private void LogInternal(Configuration.LogLevel logLevel, string message, Exception? exception)
        {
            var logMessage = FormatLogMessage(logLevel, message, exception);

            _consoleWriter?.Write(logMessage, logLevel);
            _fileWriter?.Write(logMessage);
        }

        private async Task LogInternalAsync(Configuration.LogLevel logLevel, string message, Exception? exception)
        {
            var logMessage = FormatLogMessage(logLevel, message, exception);

            var tasks = new List<Task>();

            if (_consoleWriter != null)
                tasks.Add(_consoleWriter.WriteAsync(logMessage, logLevel));

            if (_fileWriter != null)
                tasks.Add(_fileWriter.WriteAsync(logMessage));

            await Task.WhenAll(tasks);
        }

        private string FormatLogMessage(Configuration.LogLevel logLevel, string message, Exception? exception)
        {
            var parts = new List<string>();

            // Add timestamp (date and/or time)
            if (_options.FormatOptions.ShowDate || _options.FormatOptions.ShowTime)
            {
                var timestamp = _options.UseUtcTime ? DateTime.UtcNow : DateTime.Now;
                
                if (_options.FormatOptions.ShowDate && _options.FormatOptions.ShowTime)
                {
                    parts.Add(timestamp.ToString(_options.DateFormat));
                }
                else if (_options.FormatOptions.ShowDate)
                {
                    parts.Add(timestamp.ToString("yyyy-MM-dd"));
                }
                else if (_options.FormatOptions.ShowTime)
                {
                    parts.Add(timestamp.ToString("HH:mm:ss.fff"));
                }
            }

            // Add log level
            if (_options.FormatOptions.ShowLogLevel)
            {
                var logLevelStr = GetLogLevelString(logLevel);
                parts.Add($"[{logLevelStr}]");
            }

            // Add logger/category name
            if (_options.FormatOptions.ShowLoggerName)
            {
                parts.Add($"[{_categoryName}]");
            }

            // Add message
            parts.Add(message);

            var formatted = string.Join(" ", parts);

            if (exception != null)
            {
                formatted += $"{Environment.NewLine}Exception: {exception}";
            }

            return formatted;
        }

        private static string GetLogLevelString(Configuration.LogLevel logLevel)
        {
            return logLevel switch
            {
                Configuration.LogLevel.Trace => "TRACE",
                Configuration.LogLevel.Debug => "DEBUG",
                Configuration.LogLevel.Information => "INFO",
                Configuration.LogLevel.Warning => "WARN",
                Configuration.LogLevel.Error => "ERROR",
                Configuration.LogLevel.Critical => "CRITICAL",
                Configuration.LogLevel.None => "NONE",
                _ => "UNKNOWN"
            };
        }

        private static Configuration.LogLevel ConvertLogLevel(Microsoft.Extensions.Logging.LogLevel logLevel)
        {
            return logLevel switch
            {
                Microsoft.Extensions.Logging.LogLevel.Trace => Configuration.LogLevel.Trace,
                Microsoft.Extensions.Logging.LogLevel.Debug => Configuration.LogLevel.Debug,
                Microsoft.Extensions.Logging.LogLevel.Information => Configuration.LogLevel.Information,
                Microsoft.Extensions.Logging.LogLevel.Warning => Configuration.LogLevel.Warning,
                Microsoft.Extensions.Logging.LogLevel.Error => Configuration.LogLevel.Error,
                Microsoft.Extensions.Logging.LogLevel.Critical => Configuration.LogLevel.Critical,
                Microsoft.Extensions.Logging.LogLevel.None => Configuration.LogLevel.None,
                _ => Configuration.LogLevel.Information
            };
        }

        #endregion

        #region Dispose Pattern

        /// <summary>
        /// Disposes the logger and flushes any remaining log messages.
        /// </summary>
        public void Dispose()
        {
            if (_disposed) return;

            _disposed = true;
            _fileWriter?.Dispose();
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
