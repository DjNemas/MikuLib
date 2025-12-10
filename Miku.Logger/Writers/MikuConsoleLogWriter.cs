using Miku.Core;
using Miku.Logger.Configuration.Enums;
using Miku.Logger.Configuration.Models;

namespace Miku.Logger.Writers
{
    /// <summary>
    /// Thread-safe console writer with color support.
    /// Supports standard 16 colors, 256-color palette, and TrueColor (24-bit RGB).
    /// </summary>
    internal class MikuConsoleLogWriter
    {
        // ANSI escape sequence constants
        private const string AnsiReset = "\x1b[0m";
        private const string AnsiExtended256Prefix = "\x1b[38;5;";
        private const string AnsiTrueColorPrefix = "\x1b[38;2;";

        private readonly MikuConsoleColorOptions _colorOptions;
        private readonly SemaphoreSlim _writeSemaphore = new(1, 1);
        private readonly bool _supportsAnsi;

        public MikuConsoleLogWriter(MikuConsoleColorOptions colorOptions)
        {
            _colorOptions = colorOptions ?? throw new ArgumentNullException(nameof(colorOptions));
            _supportsAnsi = CheckAnsiSupport();
        }

        /// <summary>
        /// Checks if the current console supports ANSI escape sequences.
        /// </summary>
        private static bool CheckAnsiSupport()
        {
            var term = Environment.GetEnvironmentVariable("TERM");
            var colorTerm = Environment.GetEnvironmentVariable("COLORTERM");

            if (colorTerm == "truecolor" || colorTerm == "24bit")
                return true;

            if (!string.IsNullOrEmpty(term) &&
                (term.Contains("xterm") || term.Contains("256color") || term.Contains("color")))
                return true;

            if (Environment.OSVersion.Platform == PlatformID.Win32NT &&
                Environment.OSVersion.Version.Major >= 10)
            {
                try
                {
                    return true;
                }
                catch
                {
                    return false;
                }
            }

            if (Environment.OSVersion.Platform == PlatformID.Unix ||
                Environment.OSVersion.Platform == PlatformID.MacOSX)
                return true;

            return false;
        }

        /// <summary>
        /// Writes a log message to console asynchronously.
        /// </summary>
        public async Task WriteAsync(string message, MikuLogLevel logLevel, CancellationToken cancellationToken = default)
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
        public void Write(string message, MikuLogLevel logLevel)
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

        private void WriteToConsole(string message, MikuLogLevel logLevel)
        {
            if (!_colorOptions.Enabled)
            {
                Console.WriteLine(message);
                return;
            }

            switch (_colorOptions.ColorSpace)
            {
                case MikuColorSpace.Console:
                    WriteWithConsoleColor(message, logLevel);
                    break;

                case MikuColorSpace.Extended256:
                    if (_supportsAnsi)
                        WriteWithExtended256Color(message, logLevel);
                    else
                        WriteWithConsoleColor(message, logLevel);
                    break;

                case MikuColorSpace.TrueColor:
                    if (_supportsAnsi)
                        WriteWithTrueColor(message, logLevel);
                    else
                        WriteWithConsoleColor(message, logLevel);
                    break;

                default:
                    Console.WriteLine(message);
                    break;
            }
        }

        private void WriteWithConsoleColor(string message, MikuLogLevel logLevel)
        {
            var color = GetConsoleColorForLogLevel(logLevel);
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        private void WriteWithExtended256Color(string message, MikuLogLevel logLevel)
        {
            var colorCode = GetExtended256ColorForLogLevel(logLevel);
            Console.Write($"{AnsiExtended256Prefix}{colorCode}m{message}{AnsiReset}");
            Console.WriteLine();
        }

        private void WriteWithTrueColor(string message, MikuLogLevel logLevel)
        {
            var rgb = GetTrueColorForLogLevel(logLevel);
            Console.Write($"{AnsiTrueColorPrefix}{rgb.R};{rgb.G};{rgb.B}m{message}{AnsiReset}");
            Console.WriteLine();
        }

        private ConsoleColor GetConsoleColorForLogLevel(MikuLogLevel logLevel)
        {
            return logLevel switch
            {
                MikuLogLevel.Trace => _colorOptions.TraceColor,
                MikuLogLevel.Debug => _colorOptions.DebugColor,
                MikuLogLevel.Information => _colorOptions.InformationColor,
                MikuLogLevel.Warning => _colorOptions.WarningColor,
                MikuLogLevel.Error => _colorOptions.ErrorColor,
                MikuLogLevel.Critical => _colorOptions.CriticalColor,
                _ => Console.ForegroundColor
            };
        }

        private byte GetExtended256ColorForLogLevel(MikuLogLevel logLevel)
        {
            var colors = _colorOptions.Extended256Colors;
            return logLevel switch
            {
                MikuLogLevel.Trace => colors.TraceColor,
                MikuLogLevel.Debug => colors.DebugColor,
                MikuLogLevel.Information => colors.InformationColor,
                MikuLogLevel.Warning => colors.WarningColor,
                MikuLogLevel.Error => colors.ErrorColor,
                MikuLogLevel.Critical => colors.CriticalColor,
                _ => 7
            };
        }

        private MikuRgbColor GetTrueColorForLogLevel(MikuLogLevel logLevel)
        {
            var colors = _colorOptions.TrueColors;
            return logLevel switch
            {
                MikuLogLevel.Trace => colors.TraceColor,
                MikuLogLevel.Debug => colors.DebugColor,
                MikuLogLevel.Information => colors.InformationColor,
                MikuLogLevel.Warning => colors.WarningColor,
                MikuLogLevel.Error => colors.ErrorColor,
                MikuLogLevel.Critical => colors.CriticalColor,
                _ => MikuRgbColor.White
            };
        }
    }
}
