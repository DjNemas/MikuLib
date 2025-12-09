using Miku.Core;
using Miku.Logger.Configuration.Enums;
using Miku.Logger.Configuration.Models;

namespace Miku.Logger.Writers
{
    /// <summary>
    /// Thread-safe console writer with color support.
    /// Supports standard 16 colors, 256-color palette, and TrueColor (24-bit RGB).
    /// </summary>
    internal class ConsoleLogWriter
    {
        // ANSI escape sequence constants
        private const string AnsiReset = "\x1b[0m";
        private const string AnsiExtended256Prefix = "\x1b[38;5;";
        private const string AnsiTrueColorPrefix = "\x1b[38;2;";

        private readonly ConsoleColorOptions _colorOptions;
        private readonly SemaphoreSlim _writeSemaphore = new(1, 1);
        private readonly bool _supportsAnsi;

        public ConsoleLogWriter(ConsoleColorOptions colorOptions)
        {
            _colorOptions = colorOptions ?? throw new ArgumentNullException(nameof(colorOptions));
            _supportsAnsi = CheckAnsiSupport();
        }

        /// <summary>
        /// Checks if the current console supports ANSI escape sequences.
        /// </summary>
        private static bool CheckAnsiSupport()
        {
            // Windows 10+ and most Unix terminals support ANSI
            // Check for common environment variables that indicate ANSI support
            var term = Environment.GetEnvironmentVariable("TERM");
            var colorTerm = Environment.GetEnvironmentVariable("COLORTERM");

            // TrueColor support indicators
            if (colorTerm == "truecolor" || colorTerm == "24bit")
                return true;

            // Common terminal types that support ANSI
            if (!string.IsNullOrEmpty(term) &&
                (term.Contains("xterm") || term.Contains("256color") || term.Contains("color")))
                return true;

            // Windows Terminal and modern Windows consoles support ANSI
            if (Environment.OSVersion.Platform == PlatformID.Win32NT &&
                Environment.OSVersion.Version.Major >= 10)
            {
                // Try to enable virtual terminal processing on Windows
                try
                {
                    // Writing ANSI codes will work if VT is enabled
                    return true;
                }
                catch
                {
                    return false;
                }
            }

            // Unix-like systems generally support ANSI
            if (Environment.OSVersion.Platform == PlatformID.Unix ||
                Environment.OSVersion.Platform == PlatformID.MacOSX)
                return true;

            return false;
        }

        /// <summary>
        /// Writes a log message to console asynchronously.
        /// </summary>
        public async Task WriteAsync(string message, LogLevel logLevel, CancellationToken cancellationToken = default)
        {
            await _writeSemaphore.WaitAsync(cancellationToken);
            try
            {
                WriteToConsole(message, logLevel);
            }
            finally
            {
                _writeSemaphore.Release();
            }
        }

        /// <summary>
        /// Writes a log message to console synchronously.
        /// </summary>
        public void Write(string message, LogLevel logLevel)
        {
            _writeSemaphore.Wait();
            try
            {
                WriteToConsole(message, logLevel);
            }
            finally
            {
                _writeSemaphore.Release();
            }
        }

        private void WriteToConsole(string message, LogLevel logLevel)
        {
            if (!_colorOptions.Enabled)
            {
                Console.WriteLine(message);
                return;
            }

            switch (_colorOptions.ColorSpace)
            {
                case ColorSpace.Console:
                    WriteWithConsoleColor(message, logLevel);
                    break;

                case ColorSpace.Extended256:
                    if (_supportsAnsi)
                        WriteWithExtended256Color(message, logLevel);
                    else
                        WriteWithConsoleColor(message, logLevel); // Fallback
                    break;

                case ColorSpace.TrueColor:
                    if (_supportsAnsi)
                        WriteWithTrueColor(message, logLevel);
                    else
                        WriteWithConsoleColor(message, logLevel); // Fallback
                    break;

                default:
                    Console.WriteLine(message);
                    break;
            }
        }

        /// <summary>
        /// Writes using standard 16 console colors.
        /// </summary>
        private void WriteWithConsoleColor(string message, LogLevel logLevel)
        {
            var color = GetConsoleColorForLogLevel(logLevel);
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        /// <summary>
        /// Writes using 256-color palette with ANSI escape sequences.
        /// </summary>
        private void WriteWithExtended256Color(string message, LogLevel logLevel)
        {
            var colorCode = GetExtended256ColorForLogLevel(logLevel);
            Console.Write($"{AnsiExtended256Prefix}{colorCode}m{message}{AnsiReset}");
            Console.WriteLine();
        }

        /// <summary>
        /// Writes using TrueColor (24-bit RGB) with ANSI escape sequences.
        /// </summary>
        private void WriteWithTrueColor(string message, LogLevel logLevel)
        {
            var rgb = GetTrueColorForLogLevel(logLevel);
            Console.Write($"{AnsiTrueColorPrefix}{rgb.R};{rgb.G};{rgb.B}m{message}{AnsiReset}");
            Console.WriteLine();
        }

        private ConsoleColor GetConsoleColorForLogLevel(LogLevel logLevel)
        {
            return logLevel switch
            {
                LogLevel.Trace => _colorOptions.TraceColor,
                LogLevel.Debug => _colorOptions.DebugColor,
                LogLevel.Information => _colorOptions.InformationColor,
                LogLevel.Warning => _colorOptions.WarningColor,
                LogLevel.Error => _colorOptions.ErrorColor,
                LogLevel.Critical => _colorOptions.CriticalColor,
                _ => Console.ForegroundColor
            };
        }

        private byte GetExtended256ColorForLogLevel(LogLevel logLevel)
        {
            var colors = _colorOptions.Extended256Colors;
            return logLevel switch
            {
                LogLevel.Trace => colors.TraceColor,
                LogLevel.Debug => colors.DebugColor,
                LogLevel.Information => colors.InformationColor,
                LogLevel.Warning => colors.WarningColor,
                LogLevel.Error => colors.ErrorColor,
                LogLevel.Critical => colors.CriticalColor,
                _ => 7 // Default white
            };
        }

        private RgbColor GetTrueColorForLogLevel(LogLevel logLevel)
        {
            var colors = _colorOptions.TrueColors;
            return logLevel switch
            {
                LogLevel.Trace => colors.TraceColor,
                LogLevel.Debug => colors.DebugColor,
                LogLevel.Information => colors.InformationColor,
                LogLevel.Warning => colors.WarningColor,
                LogLevel.Error => colors.ErrorColor,
                LogLevel.Critical => colors.CriticalColor,
                _ => RgbColor.White
            };
        }
    }
}
