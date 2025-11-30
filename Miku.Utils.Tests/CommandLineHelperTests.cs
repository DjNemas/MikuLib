namespace Miku.Utils.Tests
{
    public class CommandLineHelperTests
    {
        [Fact]
        public void HasParameterWithValue_ShouldReturnTrue_WhenParameterAndValueExist()
        {
            // Arrange
            var args = new[] { "--configuration", "Release", "--verbose" };

            // Act
            var result = CommandLineHelper.HasParameterWithValue("--configuration", "Release", args);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void HasParameterWithValue_ShouldReturnFalse_WhenParameterDoesNotExist()
        {
            // Arrange
            var args = new[] { "--configuration", "Debug" };

            // Act
            var result = CommandLineHelper.HasParameterWithValue("--output", "build", args);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void HasParameterWithValue_ShouldReturnFalse_WhenValueDoesNotMatch()
        {
            // Arrange
            var args = new[] { "--configuration", "Debug" };

            // Act
            var result = CommandLineHelper.HasParameterWithValue("--configuration", "Release", args);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void HasParameterWithValue_ShouldBeCaseInsensitive()
        {
            // Arrange
            var args = new[] { "--configuration", "release" };

            // Act
            var result = CommandLineHelper.HasParameterWithValue("--configuration", "Release", args);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsReleaseConfiguration_ShouldReturnTrue_WhenReleaseConfigExists()
        {
            // Arrange
            var args = new[] { "--configuration", "Release" };

            // Act
            var result = CommandLineHelper.IsReleaseConfiguration(args);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsReleaseConfiguration_ShouldReturnFalse_WhenDebugConfigExists()
        {
            // Arrange
            var args = new[] { "--configuration", "Debug" };

            // Act
            var result = CommandLineHelper.IsReleaseConfiguration(args);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsDebugConfiguration_ShouldReturnTrue_WhenDebugConfigExists()
        {
            // Arrange
            var args = new[] { "--configuration", "Debug" };

            // Act
            var result = CommandLineHelper.IsDebugConfiguration(args);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void HasParameter_ShouldReturnTrue_WhenParameterExists()
        {
            // Arrange
            var args = new[] { "--verbose", "--configuration", "Release" };

            // Act
            var result = CommandLineHelper.HasParameter("--verbose", args);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void HasParameter_ShouldReturnFalse_WhenParameterDoesNotExist()
        {
            // Arrange
            var args = new[] { "--configuration", "Release" };

            // Act
            var result = CommandLineHelper.HasParameter("--verbose", args);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void GetParameterValue_ShouldReturnValue_WhenParameterExists()
        {
            // Arrange
            var args = new[] { "--output", "./build", "--verbose" };

            // Act
            var result = CommandLineHelper.GetParameterValue("--output", args);

            // Assert
            Assert.Equal("./build", result);
        }

        [Fact]
        public void GetParameterValue_ShouldReturnNull_WhenParameterDoesNotExist()
        {
            // Arrange
            var args = new[] { "--configuration", "Release" };

            // Act
            var result = CommandLineHelper.GetParameterValue("--output", args);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void GetParameterValue_ShouldSupportColonSeparator()
        {
            // Arrange
            var args = new[] { "--output:./build", "--verbose" };

            // Act
            var result = CommandLineHelper.GetParameterValue("--output", args);

            // Assert
            Assert.Equal("./build", result);
        }

        [Fact]
        public void GetParameterValue_ShouldSupportEqualsSeparator()
        {
            // Arrange
            var args = new[] { "--output=./build", "--verbose" };

            // Act
            var result = CommandLineHelper.GetParameterValue("--output", args);

            // Assert
            Assert.Equal("./build", result);
        }

        [Fact]
        public void GetParameterValues_ShouldReturnMultipleValues()
        {
            // Arrange
            var args = new[] { "--include", "*.cs", "--include", "*.txt", "--output", "./build" };

            // Act
            var result = CommandLineHelper.GetParameterValues("--include", args);

            // Assert
            Assert.Equal(2, result.Length);
            Assert.Equal("*.cs", result[0]);
            Assert.Equal("*.txt", result[1]);
        }

        [Fact]
        public void GetParameterValues_ShouldReturnEmptyArray_WhenParameterDoesNotExist()
        {
            // Arrange
            var args = new[] { "--output", "./build" };

            // Act
            var result = CommandLineHelper.GetParameterValues("--include", args);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void ParseArguments_ShouldParseDictionary()
        {
            // Arrange
            var args = new[] { "--output", "./build", "--verbose", "--configuration", "Release" };

            // Act
            var result = CommandLineHelper.ParseArguments(args);

            // Assert
            Assert.Equal(3, result.Count);
            Assert.Equal("./build", result["--output"]);
            Assert.Equal(string.Empty, result["--verbose"]);
            Assert.Equal("Release", result["--configuration"]);
        }

        [Fact]
        public void ParseArguments_ShouldOverwriteDuplicateKeys()
        {
            // Arrange
            var args = new[] { "--output", "./build", "--output", "./dist" };

            // Act
            var result = CommandLineHelper.ParseArguments(args);

            // Assert
            Assert.Single(result);
            Assert.Equal("./dist", result["--output"]);
        }

        [Fact]
        public void ParseArgumentsWithMultipleValues_ShouldKeepAllValues()
        {
            // Arrange
            var args = new[] { "--include", "*.cs", "--include", "*.txt", "--output", "./build" };

            // Act
            var result = CommandLineHelper.ParseArgumentsWithMultipleValues(args);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal(2, result["--include"].Count);
            Assert.Equal("*.cs", result["--include"][0]);
            Assert.Equal("*.txt", result["--include"][1]);
            Assert.Single(result["--output"]);
            Assert.Equal("./build", result["--output"][0]);
        }

        [Fact]
        public void Parse_ShouldReturnParsedArguments()
        {
            // Arrange
            var args = new[] { "--output", "./build", "--include", "*.cs", "--include", "*.txt", "--verbose" };

            // Act
            var parsed = CommandLineHelper.Parse(args);

            // Assert
            Assert.Equal("./build", parsed.GetValue("--output"));
            Assert.Equal(2, parsed.GetValues("--include").Length);
            Assert.True(parsed.HasParameter("--verbose"));
        }

        [Fact]
        public void ParsedArguments_GetValue_ShouldReturnLastValue()
        {
            // Arrange
            var args = new[] { "--output", "./build", "--output", "./dist" };
            var parsed = CommandLineHelper.Parse(args);

            // Act
            var result = parsed.GetValue("--output");

            // Assert
            Assert.Equal("./dist", result);
        }

        [Fact]
        public void ParsedArguments_GetValue_ShouldReturnNull_WhenParameterDoesNotExist()
        {
            // Arrange
            var args = new[] { "--output", "./build" };
            var parsed = CommandLineHelper.Parse(args);

            // Act
            var result = parsed.GetValue("--verbose");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void ParsedArguments_GetValues_ShouldReturnAllValues()
        {
            // Arrange
            var args = new[] { "--include", "*.cs", "--include", "*.txt" };
            var parsed = CommandLineHelper.Parse(args);

            // Act
            var result = parsed.GetValues("--include");

            // Assert
            Assert.Equal(2, result.Length);
            Assert.Equal("*.cs", result[0]);
            Assert.Equal("*.txt", result[1]);
        }

        [Fact]
        public void ParsedArguments_GetValueOrDefault_ShouldReturnDefaultWhenNotFound()
        {
            // Arrange
            var args = new[] { "--output", "./build" };
            var parsed = CommandLineHelper.Parse(args);

            // Act
            var result = parsed.GetValueOrDefault("--timeout", "30");

            // Assert
            Assert.Equal("30", result);
        }

        [Fact]
        public void ParsedArguments_GetValueCount_ShouldReturnCorrectCount()
        {
            // Arrange
            var args = new[] { "--include", "*.cs", "--include", "*.txt" };
            var parsed = CommandLineHelper.Parse(args);

            // Act
            var result = parsed.GetValueCount("--include");

            // Assert
            Assert.Equal(2, result);
        }

        [Fact]
        public void ParsedArguments_GetParameterNames_ShouldReturnAllParameterNames()
        {
            // Arrange
            var args = new[] { "--output", "./build", "--verbose", "--include", "*.cs" };
            var parsed = CommandLineHelper.Parse(args);

            // Act
            var result = parsed.GetParameterNames();

            // Assert
            Assert.Equal(3, result.Length);
            Assert.Contains("--output", result);
            Assert.Contains("--verbose", result);
            Assert.Contains("--include", result);
        }

        [Fact]
        public void ParsedArguments_TryGetValue_ShouldReturnTrueWhenFound()
        {
            // Arrange
            var args = new[] { "--output", "./build" };
            var parsed = CommandLineHelper.Parse(args);

            // Act
            var result = parsed.TryGetValue("--output", out var value);

            // Assert
            Assert.True(result);
            Assert.Equal("./build", value);
        }

        [Fact]
        public void ParsedArguments_TryGetValue_ShouldReturnFalseWhenNotFound()
        {
            // Arrange
            var args = new[] { "--output", "./build" };
            var parsed = CommandLineHelper.Parse(args);

            // Act
            var result = parsed.TryGetValue("--verbose", out var value);

            // Assert
            Assert.False(result);
            Assert.Null(value);
        }

        [Fact]
        public void HasParameter_WithEmptyArgs_ShouldReturnFalse()
        {
            // Arrange
            var args = Array.Empty<string>();

            // Act
            var result = CommandLineHelper.HasParameter("--verbose", args);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void GetParameterValue_WithEmptyArgs_ShouldReturnNull()
        {
            // Arrange
            var args = Array.Empty<string>();

            // Act
            var result = CommandLineHelper.GetParameterValue("--output", args);

            // Assert
            Assert.Null(result);
        }
    }
}
