using Miku.Logger.Configuration;
using Miku.Logger.Configuration.Enums;
using Miku.Logger.Configuration.Models;

namespace Miku.Logger.Tests
{
    public class MikuLoggerOptionsTests
    {
        [Fact]
        public void MikuLoggerOptions_DefaultValues_ShouldBeSet()
        {
            // Arrange & Act
            var options = new MikuLoggerOptions();

            // Assert
            Assert.Equal(MikuLogOutput.ConsoleAndFile, options.Output);
            Assert.Equal(MikuLogLevel.Information, options.MinimumLogLevel);
            Assert.NotNull(options.ConsoleColors);
            Assert.NotNull(options.FileOptions);
            Assert.NotNull(options.FormatOptions);
            Assert.Equal("yyyy-MM-dd HH:mm:ss.fff", options.DateFormat);
            Assert.False(options.UseUtcTime);
        }

        [Fact]
        public void LogFormatOptions_DefaultValues_ShouldBeSet()
        {
            // Arrange & Act
            var options = new MikuLogFormatOptions();

            // Assert
            Assert.True(options.ShowDate);
            Assert.True(options.ShowTime);
            Assert.True(options.ShowLogLevel);
            Assert.True(options.ShowLoggerName);
        }

        [Fact]
        public void ConsoleColorOptions_DefaultValues_ShouldBeSet()
        {
            // Arrange & Act
            var options = new MikuConsoleColorOptions();

            // Assert
            Assert.True(options.Enabled);
            Assert.Equal(MikuColorSpace.Console, options.ColorSpace);
            Assert.Equal(ConsoleColor.Gray, options.TraceColor);
            Assert.Equal(ConsoleColor.Yellow, options.DebugColor);
            Assert.Equal(ConsoleColor.Cyan, options.InformationColor);
            Assert.Equal(ConsoleColor.Magenta, options.WarningColor);
            Assert.Equal(ConsoleColor.Red, options.ErrorColor);
            Assert.Equal(ConsoleColor.DarkRed, options.CriticalColor);
        }

        [Fact]
        public void FileLoggerOptions_DefaultValues_ShouldBeSet()
        {
            // Arrange & Act
            var options = new MikuFileLoggerOptions();

            // Assert
            Assert.Equal("./logs", options.LogDirectory);
            Assert.Equal("log.txt", options.FileNamePattern);
            Assert.False(options.UseDateFolders);
            Assert.Equal("yyyy-MM-dd", options.DateFolderFormat);
            Assert.Equal(10 * 1024 * 1024, options.MaxFileSizeBytes);
            Assert.Equal(10, options.MaxFileCount);
            Assert.True(options.AppendToExisting);
        }

        [Fact]
        public void LogOutput_Flags_ShouldCombineProperly()
        {
            // Arrange & Act
            var consoleOnly = MikuLogOutput.Console;
            var fileOnly = MikuLogOutput.File;
            var both = MikuLogOutput.ConsoleAndFile;

            // Assert
            Assert.True(both.HasFlag(MikuLogOutput.Console));
            Assert.True(both.HasFlag(MikuLogOutput.File));
            Assert.False(consoleOnly.HasFlag(MikuLogOutput.File));
            Assert.False(fileOnly.HasFlag(MikuLogOutput.Console));
        }

        [Fact]
        public void LogLevel_Values_ShouldBeOrdered()
        {
            // Assert
            Assert.True(MikuLogLevel.Trace < MikuLogLevel.Debug);
            Assert.True(MikuLogLevel.Debug < MikuLogLevel.Information);
            Assert.True(MikuLogLevel.Information < MikuLogLevel.Warning);
            Assert.True(MikuLogLevel.Warning < MikuLogLevel.Error);
            Assert.True(MikuLogLevel.Error < MikuLogLevel.Critical);
            Assert.True(MikuLogLevel.Critical < MikuLogLevel.None);
        }

        [Fact]
        public void MikuLoggerOptions_CustomValues_ShouldBeSettable()
        {
            // Arrange & Act
            var options = new MikuLoggerOptions
            {
                Output = MikuLogOutput.Console,
                MinimumLogLevel = MikuLogLevel.Debug,
                DateFormat = "yyyy-MM-dd",
                UseUtcTime = true
            };

            // Assert
            Assert.Equal(MikuLogOutput.Console, options.Output);
            Assert.Equal(MikuLogLevel.Debug, options.MinimumLogLevel);
            Assert.Equal("yyyy-MM-dd", options.DateFormat);
            Assert.True(options.UseUtcTime);
        }

        [Fact]
        public void LogFormatOptions_CustomValues_ShouldBeSettable()
        {
            // Arrange & Act
            var options = new MikuLogFormatOptions
            {
                ShowDate = false,
                ShowTime = true,
                ShowLogLevel = false,
                ShowLoggerName = true
            };

            // Assert
            Assert.False(options.ShowDate);
            Assert.True(options.ShowTime);
            Assert.False(options.ShowLogLevel);
            Assert.True(options.ShowLoggerName);
        }

        [Fact]
        public void ConsoleColorOptions_CustomValues_ShouldBeSettable()
        {
            // Arrange & Act
            var options = new MikuConsoleColorOptions
            {
                Enabled = false,
                ColorSpace = MikuColorSpace.Extended256,
                InformationColor = ConsoleColor.Green
            };

            // Assert
            Assert.False(options.Enabled);
            Assert.Equal(MikuColorSpace.Extended256, options.ColorSpace);
            Assert.Equal(ConsoleColor.Green, options.InformationColor);
        }

        [Fact]
        public void FileLoggerOptions_CustomValues_ShouldBeSettable()
        {
            // Arrange & Act
            var options = new MikuFileLoggerOptions
            {
                LogDirectory = "./custom_logs",
                FileNamePattern = "app_{0:yyyy-MM-dd}.log",
                UseDateFolders = true,
                MaxFileSizeBytes = 5 * 1024 * 1024,
                MaxFileCount = 5,
                AppendToExisting = false
            };

            // Assert
            Assert.Equal("./custom_logs", options.LogDirectory);
            Assert.Equal("app_{0:yyyy-MM-dd}.log", options.FileNamePattern);
            Assert.True(options.UseDateFolders);
            Assert.Equal(5 * 1024 * 1024, options.MaxFileSizeBytes);
            Assert.Equal(5, options.MaxFileCount);
            Assert.False(options.AppendToExisting);
        }

        [Fact]
        public void ColorSpace_Values_ShouldExist()
        {
            // Arrange & Act
            var console = MikuColorSpace.Console;
            var extended = MikuColorSpace.Extended256;
            var trueColor = MikuColorSpace.TrueColor;

            // Assert
            Assert.NotEqual(console, extended);
            Assert.NotEqual(extended, trueColor);
            Assert.NotEqual(console, trueColor);
        }
    }
}
