using Microsoft.Extensions.Options;
using Miku.Logger.Configuration;
using Miku.Logger.Configuration.Enums;
using Miku.Logger.Providers;

namespace Miku.Logger.Tests
{
    public class MikuLoggerProviderTests
    {
        [Fact]
        public void MikuLoggerProvider_WithOptions_ShouldCreateInstance()
        {
            // Arrange
            var options = new MikuLoggerOptions
            {
                Output = MikuLogOutput.Console
            };

            // Act
            using var provider = new MikuLoggerProvider(options);

            // Assert
            Assert.NotNull(provider);
        }

        [Fact]
        public void MikuLoggerProvider_WithIOptions_ShouldCreateInstance()
        {
            // Arrange
            var options = Options.Create(new MikuLoggerOptions
            {
                Output = MikuLogOutput.Console
            });

            // Act
            using var provider = new MikuLoggerProvider(options);

            // Assert
            Assert.NotNull(provider);
        }

        [Fact]
        public void CreateLogger_ShouldReturnLogger()
        {
            // Arrange
            var options = new MikuLoggerOptions
            {
                Output = MikuLogOutput.Console
            };
            using var provider = new MikuLoggerProvider(options);

            // Act
            var logger = provider.CreateLogger("TestCategory");

            // Assert
            Assert.NotNull(logger);
        }

        [Fact]
        public void CreateLogger_WithSameCategoryName_ShouldReturnSameInstance()
        {
            // Arrange
            var options = new MikuLoggerOptions
            {
                Output = MikuLogOutput.Console
            };
            using var provider = new MikuLoggerProvider(options);

            // Act
            var logger1 = provider.CreateLogger("TestCategory");
            var logger2 = provider.CreateLogger("TestCategory");

            // Assert
            Assert.Same(logger1, logger2);
        }

        [Fact]
        public void CreateLogger_WithDifferentCategoryNames_ShouldReturnDifferentInstances()
        {
            // Arrange
            var options = new MikuLoggerOptions
            {
                Output = MikuLogOutput.Console
            };
            using var provider = new MikuLoggerProvider(options);

            // Act
            var logger1 = provider.CreateLogger("Category1");
            var logger2 = provider.CreateLogger("Category2");

            // Assert
            Assert.NotSame(logger1, logger2);
        }

        [Fact]
        public void Dispose_ShouldNotThrow()
        {
            // Arrange
            var options = new MikuLoggerOptions
            {
                Output = MikuLogOutput.Console
            };
            var provider = new MikuLoggerProvider(options);
            provider.CreateLogger("TestCategory");

            // Act & Assert
            var exception = Record.Exception(() => provider.Dispose());
            Assert.Null(exception);
        }

        [Fact]
        public void Constructor_WithNullOptions_ShouldThrowArgumentNullException()
        {
            // Arrange
            MikuLoggerOptions? options = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new MikuLoggerProvider(options!));
        }

        [Fact]
        public void Constructor_WithNullIOptions_ShouldThrowArgumentNullException()
        {
            // Arrange
            IOptions<MikuLoggerOptions>? options = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new MikuLoggerProvider(options!));
        }

        [Fact]
        public void CreateLogger_AfterDispose_ShouldStillWork()
        {
            // Arrange
            var options = new MikuLoggerOptions
            {
                Output = MikuLogOutput.Console
            };
            var provider = new MikuLoggerProvider(options);
            provider.Dispose();

            // Act
            var logger = provider.CreateLogger("TestCategory");

            // Assert
            Assert.NotNull(logger);
        }
    }
}
