using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Miku.Logger.Configuration;
using Miku.Logger.Configuration.Enums;
using Miku.Logger.Extensions;

using MikuLogLevel = Miku.Logger.Configuration.Enums.LogLevel;

namespace Miku.Logger.Tests
{
    public class MikuLoggerExtensionsTests
    {
        [Fact]
        public void AddMikuLogger_WithoutOptions_ShouldAddToServiceCollection()
        {
            // Arrange
            var services = new ServiceCollection();
            var builder = services.AddLogging(loggingBuilder =>
            {
                // Act
                loggingBuilder.AddMikuLogger();
            });

            // Assert
            var serviceProvider = services.BuildServiceProvider();
            var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
            Assert.NotNull(loggerFactory);
        }

        [Fact]
        public void AddMikuLogger_WithOptions_ShouldAddToServiceCollection()
        {
            // Arrange
            var services = new ServiceCollection();
            var options = new MikuLoggerOptions
            {
                Output = LogOutput.Console,
                MinimumLogLevel = MikuLogLevel.Debug
            };

            var builder = services.AddLogging(loggingBuilder =>
            {
                // Act
                loggingBuilder.AddMikuLogger(options);
            });

            // Assert
            var serviceProvider = services.BuildServiceProvider();
            var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
            Assert.NotNull(loggerFactory);
        }

        [Fact]
        public void AddMikuLogger_WithConfigureAction_ShouldAddToServiceCollection()
        {
            // Arrange
            var services = new ServiceCollection();
            var builder = services.AddLogging(loggingBuilder =>
            {
                // Act
                loggingBuilder.AddMikuLogger(options =>
                {
                    options.Output = LogOutput.Console;
                    options.MinimumLogLevel = MikuLogLevel.Trace;
                });
            });

            // Assert
            var serviceProvider = services.BuildServiceProvider();
            var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
            Assert.NotNull(loggerFactory);
        }

        [Fact]
        public void AddMikuLogger_ShouldCreateWorkingLogger()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddMikuLogger(options =>
                {
                    options.Output = LogOutput.Console;
                });
            });

            var serviceProvider = services.BuildServiceProvider();
            var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();

            // Act
            var logger = loggerFactory.CreateLogger("TestCategory");
            var exception = Record.Exception(() => logger.LogInformation("Test message"));

            // Assert
            Assert.NotNull(logger);
            Assert.Null(exception);
        }

        [Fact]
        public void AddMikuLogger_ShouldReturnLoggingBuilder()
        {
            // Arrange
            var services = new ServiceCollection();
            ILoggingBuilder? result = null;

            services.AddLogging(loggingBuilder =>
            {
                // Act
                result = loggingBuilder.AddMikuLogger();
            });

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void AddMikuLogger_WithOptions_ShouldReturnLoggingBuilder()
        {
            // Arrange
            var services = new ServiceCollection();
            var options = new MikuLoggerOptions { Output = LogOutput.Console };
            ILoggingBuilder? result = null;

            services.AddLogging(loggingBuilder =>
            {
                // Act
                result = loggingBuilder.AddMikuLogger(options);
            });

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void AddMikuLogger_WithConfigureAction_ShouldReturnLoggingBuilder()
        {
            // Arrange
            var services = new ServiceCollection();
            ILoggingBuilder? result = null;

            services.AddLogging(loggingBuilder =>
            {
                // Act
                result = loggingBuilder.AddMikuLogger(options =>
                {
                    options.Output = LogOutput.Console;
                });
            });

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void AddMikuLogger_MultipleLoggers_ShouldWork()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddMikuLogger(options =>
                {
                    options.Output = LogOutput.Console;
                });
            });

            var serviceProvider = services.BuildServiceProvider();
            var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();

            // Act
            var logger1 = loggerFactory.CreateLogger("Category1");
            var logger2 = loggerFactory.CreateLogger("Category2");
            var exception = Record.Exception(() =>
            {
                logger1.LogInformation("Logger 1 message");
                logger2.LogInformation("Logger 2 message");
            });

            // Assert
            Assert.NotNull(logger1);
            Assert.NotNull(logger2);
            Assert.Null(exception);
        }
    }
}
