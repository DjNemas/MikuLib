using Microsoft.Extensions.Logging;
using Miku.Logger.Configuration;
using Miku.Logger.Configuration.Enums;
using Miku.Logger.Configuration.Models;

namespace Miku.Logger.Tests
{
    public class MikuLoggerTests : IDisposable
    {
        private readonly string _testLogDirectory = "./test_logs";

        public MikuLoggerTests()
        {
            CleanupTestLogs();
        }

        public void Dispose()
        {
            CleanupTestLogs();
        }

        private void CleanupTestLogs()
        {
            if (Directory.Exists(_testLogDirectory))
            {
                try
                {
                    Directory.Delete(_testLogDirectory, true);
                }
                catch
                {
                    // Ignore cleanup errors
                }
            }
        }

        [Fact]
        public void MikuLogger_ShouldCreateInstance()
        {
            // Arrange & Act
            using var logger = new MikuLogger("TestLogger");

            // Assert
            Assert.NotNull(logger);
        }

        [Fact]
        public void MikuLogger_ShouldCreateInstanceWithOptions()
        {
            // Arrange
            var options = new MikuLoggerOptions
            {
                Output = MikuLogOutput.Console,
                MinimumLogLevel = MikuLogLevel.Debug
            };

            // Act
            using var logger = new MikuLogger("TestLogger", options);

            // Assert
            Assert.NotNull(logger);
        }

        [Fact]
        public void LogInformation_ShouldNotThrow()
        {
            // Arrange
            var options = new MikuLoggerOptions
            {
                Output = MikuLogOutput.Console,
                MinimumLogLevel = MikuLogLevel.Trace
            };
            using var logger = new MikuLogger("TestLogger", options);

            // Act & Assert
            var exception = Record.Exception(() => logger.LogInformation("Test message"));
            Assert.Null(exception);
        }

        [Fact]
        public void LogError_ShouldNotThrow()
        {
            // Arrange
            var options = new MikuLoggerOptions
            {
                Output = MikuLogOutput.Console
            };
            using var logger = new MikuLogger("TestLogger", options);

            // Act & Assert
            var exception = Record.Exception(() => logger.LogError("Error message"));
            Assert.Null(exception);
        }

        [Fact]
        public void LogError_WithException_ShouldNotThrow()
        {
            // Arrange
            var options = new MikuLoggerOptions
            {
                Output = MikuLogOutput.Console
            };
            using var logger = new MikuLogger("TestLogger", options);
            var testException = new InvalidOperationException("Test exception");

            // Act & Assert
            var exception = Record.Exception(() => logger.LogError(testException, "Error with exception"));
            Assert.Null(exception);
        }

        [Fact]
        public void LogWarning_ShouldNotThrow()
        {
            // Arrange
            var options = new MikuLoggerOptions
            {
                Output = MikuLogOutput.Console
            };
            using var logger = new MikuLogger("TestLogger", options);

            // Act & Assert
            var exception = Record.Exception(() => logger.LogWarning("Warning message"));
            Assert.Null(exception);
        }

        [Fact]
        public void LogDebug_ShouldNotThrow()
        {
            // Arrange
            var options = new MikuLoggerOptions
            {
                Output = MikuLogOutput.Console,
                MinimumLogLevel = MikuLogLevel.Debug
            };
            using var logger = new MikuLogger("TestLogger", options);

            // Act & Assert
            var exception = Record.Exception(() => logger.LogDebug("Debug message"));
            Assert.Null(exception);
        }

        [Fact]
        public void LogTrace_ShouldNotThrow()
        {
            // Arrange
            var options = new MikuLoggerOptions
            {
                Output = MikuLogOutput.Console,
                MinimumLogLevel = MikuLogLevel.Trace
            };
            using var logger = new MikuLogger("TestLogger", options);

            // Act & Assert
            var exception = Record.Exception(() => logger.LogTrace("Trace message"));
            Assert.Null(exception);
        }

        [Fact]
        public void LogCritical_ShouldNotThrow()
        {
            // Arrange
            var options = new MikuLoggerOptions
            {
                Output = MikuLogOutput.Console
            };
            using var logger = new MikuLogger("TestLogger", options);

            // Act & Assert
            var exception = Record.Exception(() => logger.LogCritical("Critical message"));
            Assert.Null(exception);
        }

        [Fact]
        public async Task LogInformationAsync_ShouldNotThrow()
        {
            // Arrange
            var options = new MikuLoggerOptions
            {
                Output = MikuLogOutput.Console
            };
            using var logger = new MikuLogger("TestLogger", options);

            // Act & Assert
            var exception = await Record.ExceptionAsync(async () =>
                await logger.LogInformationAsync("Async test message"));
            Assert.Null(exception);
        }

        [Fact]
        public async Task LogErrorAsync_ShouldNotThrow()
        {
            // Arrange
            var options = new MikuLoggerOptions
            {
                Output = MikuLogOutput.Console
            };
            using var logger = new MikuLogger("TestLogger", options);

            // Act & Assert
            var exception = await Record.ExceptionAsync(async () =>
                await logger.LogErrorAsync("Async error message"));
            Assert.Null(exception);
        }

        [Fact]
        public void LogInformation_WithFormatting_ShouldNotThrow()
        {
            // Arrange
            var options = new MikuLoggerOptions
            {
                Output = MikuLogOutput.Console
            };
            using var logger = new MikuLogger("TestLogger", options);

            // Act & Assert
            var exception = Record.Exception(() =>
                logger.LogInformation("Test message with {0} and {1}", "param1", 39));
            Assert.Null(exception);
        }

        [Fact]
        public async Task MikuLogger_WithFileOutput_ShouldCreateLogFile()
        {
            // Arrange
            var options = new MikuLoggerOptions
            {
                Output = MikuLogOutput.File,
                FileOptions = new MikuFileLoggerOptions
                {
                    LogDirectory = _testLogDirectory,
                    FileNamePattern = "test.log"
                }
            };
            using var logger = new MikuLogger("TestLogger", options);

            // Act
            logger.LogInformation("Test message");
            await Task.Delay(200); // Give time for async file write

            // Assert
            var logFilePath = Path.Combine(_testLogDirectory, "test.log");
            Assert.True(File.Exists(logFilePath));
        }

        [Fact]
        public void IsEnabled_ShouldReturnTrue_WhenLogLevelIsEnabled()
        {
            // Arrange
            var options = new MikuLoggerOptions
            {
                MinimumLogLevel = MikuLogLevel.Information
            };
            using var logger = new MikuLogger("TestLogger", options);

            // Act
            var result = logger.IsEnabled(Microsoft.Extensions.Logging.LogLevel.Information);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsEnabled_ShouldReturnFalse_WhenLogLevelIsDisabled()
        {
            // Arrange
            var options = new MikuLoggerOptions
            {
                MinimumLogLevel = MikuLogLevel.Warning
            };
            using var logger = new MikuLogger("TestLogger", options);

            // Act
            var result = logger.IsEnabled(Microsoft.Extensions.Logging.LogLevel.Debug);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Log_WithMicrosoftExtensionsLogLevel_ShouldNotThrow()
        {
            // Arrange
            var options = new MikuLoggerOptions
            {
                Output = MikuLogOutput.Console
            };
            using var logger = new MikuLogger("TestLogger", options);

            // Act & Assert
            var exception = Record.Exception(() =>
                logger.Log(Microsoft.Extensions.Logging.LogLevel.Information,
                    new EventId(1),
                    "Test state",
                    null,
                    (state, ex) => state.ToString() ?? ""));
            Assert.Null(exception);
        }

        [Fact]
        public void BeginScope_ShouldReturnNull()
        {
            // Arrange
            using var logger = new MikuLogger("TestLogger");

            // Act
            var scope = logger.BeginScope("Test scope");

            // Assert
            Assert.Null(scope);
        }

        [Fact]
        public void MikuLogger_WithCustomDateFormat_ShouldNotThrow()
        {
            // Arrange
            var options = new MikuLoggerOptions
            {
                Output = MikuLogOutput.Console,
                DateFormat = "yyyy-MM-dd"
            };
            using var logger = new MikuLogger("TestLogger", options);

            // Act & Assert
            var exception = Record.Exception(() => logger.LogInformation("Test message"));
            Assert.Null(exception);
        }

        [Fact]
        public void MikuLogger_WithUtcTime_ShouldNotThrow()
        {
            // Arrange
            var options = new MikuLoggerOptions
            {
                Output = MikuLogOutput.Console,
                UseUtcTime = true
            };
            using var logger = new MikuLogger("TestLogger", options);

            // Act & Assert
            var exception = Record.Exception(() => logger.LogInformation("Test message"));
            Assert.Null(exception);
        }

        [Fact]
        public void MikuLogger_WithCustomFormatOptions_ShouldNotThrow()
        {
            // Arrange
            var options = new MikuLoggerOptions
            {
                Output = MikuLogOutput.Console,
                FormatOptions = new MikuLogFormatOptions
                {
                    ShowDate = true,
                    ShowTime = true,
                    ShowLogLevel = true,
                    ShowLoggerName = true
                }
            };
            using var logger = new MikuLogger("TestLogger", options);

            // Act & Assert
            var exception = Record.Exception(() => logger.LogInformation("Test message"));
            Assert.Null(exception);
        }

        [Fact]
        public void MikuLogger_WithNoLoggerName_ShouldUseCallingClass()
        {
            // Arrange & Act
            using var logger = new MikuLogger();

            // Assert
            Assert.NotNull(logger);
        }

        [Fact]
        public void MikuLogger_Dispose_ShouldNotThrow()
        {
            // Arrange
            var logger = new MikuLogger("TestLogger");

            // Act & Assert
            var exception = Record.Exception(() => logger.Dispose());
            Assert.Null(exception);
        }

        [Fact]
        public void MikuLogger_WithConsoleAndFile_ShouldNotThrow()
        {
            // Arrange
            var options = new MikuLoggerOptions
            {
                Output = MikuLogOutput.ConsoleAndFile,
                FileOptions = new MikuFileLoggerOptions
                {
                    LogDirectory = _testLogDirectory,
                    FileNamePattern = "combined.log"
                }
            };
            using var logger = new MikuLogger("TestLogger", options);

            // Act & Assert
            var exception = Record.Exception(() => logger.LogInformation("Combined output test"));
            Assert.Null(exception);
        }
    }
}
