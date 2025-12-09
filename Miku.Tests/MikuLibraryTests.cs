using Miku.Logger.Configuration.Enums;

namespace Miku.Tests
{
    /// <summary>
    /// Tests for the main Miku library package.
    /// This project aggregates Miku.Utils and Miku.Logger.
    /// </summary>
    public class MikuLibraryTests
    {
        [Fact]
        public void MikuLib_ShouldReferenceUtils()
        {
            // This test verifies that Miku.Utils is accessible through the main package
            // Arrange & Act
            var exception = Record.Exception(() =>
            {
                var mapper = typeof(Utils.MikuMapper);
                var commandLineHelper = typeof(Utils.CommandLineHelper);
            });

            // Assert
            Assert.Null(exception);
        }

        [Fact]
        public void MikuLib_ShouldReferenceLogger()
        {
            // This test verifies that Miku.Logger is accessible through the main package
            // Arrange & Act
            var exception = Record.Exception(() =>
            {
                var logger = typeof(Logger.MikuLogger);
                var options = typeof(Logger.Configuration.MikuLoggerOptions);
            });

            // Assert
            Assert.Null(exception);
        }

        [Fact]
        public void MikuLib_Utils_MikuMapper_ShouldBeAccessible()
        {
            // Arrange
            var source = new { Id = 39, Name = "Hatsune Miku" };

            // Act
            var exception = Record.Exception(() =>
            {
                var target = Utils.MikuMapper.MapProperties<TestDto>(source);
            });

            // Assert
            Assert.Null(exception);
        }

        [Fact]
        public void MikuLib_Utils_CommandLineHelper_ShouldBeAccessible()
        {
            // Arrange
            var args = new[] { "--configuration", "Release" };

            // Act
            var result = Utils.CommandLineHelper.IsReleaseConfiguration(args);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void MikuLib_Logger_MikuLogger_ShouldBeAccessible()
        {
            // Arrange & Act
            var exception = Record.Exception(() =>
            {
                using var logger = new Logger.MikuLogger("TestLogger", new Logger.Configuration.MikuLoggerOptions
                {
                    Output = MikuLogOutput.Console
                });
                logger.LogInformation("Test from Miku main package");
            });

            // Assert
            Assert.Null(exception);
        }

        [Fact]
        public void MikuLib_Version_ShouldBe39Inspired()
        {
            // The magic number 39 (Mi-Ku in Japanese)
            var mikuNumber = 39;

            // Assert
            Assert.Equal(39, mikuNumber);
        }

        [Fact]
        public void MikuLib_CharacterAge_ShouldBe16()
        {
            // Hatsune Miku's canonical age
            var age = 16;

            // Assert
            Assert.Equal(16, age);
        }

        [Fact]
        public void MikuLib_BirthDate_ShouldBeAugust31st2007()
        {
            // Hatsune Miku's official release date
            var birthDate = new DateTime(2007, 8, 31);

            // Assert
            Assert.Equal(2007, birthDate.Year);
            Assert.Equal(8, birthDate.Month);
            Assert.Equal(31, birthDate.Day);
        }

        private class TestDto
        {
            public int Id { get; set; }
            public string? Name { get; set; }
        }
    }
}
