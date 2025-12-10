using Miku.Core;
using Miku.Logger.Configuration;
using Miku.Logger.Configuration.Enums;
using Miku.Logger.Configuration.Models;

namespace Miku.Logger.Tests;

/// <summary>
/// Tests for Extended256 and TrueColor console output.
/// Like testing all the colors of Miku's concerts!
/// </summary>
public class ExtendedColorTests
{
    #region RgbColor Tests

    [Fact]
    public void RgbColor_Constructor_ShouldSetValues()
    {
        // Arrange & Act
        var color = new MikuRgbColor(0x00, 0xCE, 0xD1);

        // Assert
        Assert.Equal(0x00, color.R);
        Assert.Equal(0xCE, color.G);
        Assert.Equal(0xD1, color.B);
    }

    [Fact]
    public void RgbColor_FromHex_WithHash_ShouldParse()
    {
        // Arrange & Act
        var color = MikuRgbColor.FromHex("#00CED1");

        // Assert
        Assert.Equal(0x00, color.R);
        Assert.Equal(0xCE, color.G);
        Assert.Equal(0xD1, color.B);
    }

    [Fact]
    public void RgbColor_FromHex_WithoutHash_ShouldParse()
    {
        // Arrange & Act
        var color = MikuRgbColor.FromHex("FF5733");

        // Assert
        Assert.Equal(0xFF, color.R);
        Assert.Equal(0x57, color.G);
        Assert.Equal(0x33, color.B);
    }

    [Fact]
    public void RgbColor_ToHex_ShouldReturnCorrectFormat()
    {
        // Arrange
        var color = new MikuRgbColor(0, 206, 209);

        // Act
        var hex = color.ToHex();

        // Assert
        Assert.Equal("#00CED1", hex);
    }

    [Fact]
    public void RgbColor_MikuCyan_ShouldBeCorrect()
    {
        // Arrange & Act
        var mikuCyan = MikuRgbColor.MikuCyan;

        // Assert - Miku's signature color #00CED1
        Assert.Equal(0x00, mikuCyan.R);
        Assert.Equal(0xCE, mikuCyan.G);
        Assert.Equal(0xD1, mikuCyan.B);
        Assert.Equal("#00CED1", mikuCyan.ToHex());
    }

    [Fact]
    public void RgbColor_Equality_ShouldWork()
    {
        // Arrange
        var color1 = new MikuRgbColor(100, 150, 200);
        var color2 = new MikuRgbColor(100, 150, 200);
        var color3 = new MikuRgbColor(100, 150, 201);

        // Assert
        Assert.Equal(color1, color2);
        Assert.True(color1 == color2);
        Assert.NotEqual(color1, color3);
        Assert.True(color1 != color3);
    }

    [Fact]
    public void RgbColor_FromHex_InvalidLength_ShouldThrow()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => MikuRgbColor.FromHex("FFF"));
        Assert.Throws<ArgumentException>(() => MikuRgbColor.FromHex("FFFFFFF"));
    }

    [Fact]
    public void RgbColor_FromHex_Null_ShouldThrow()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => MikuRgbColor.FromHex(null!));
    }

    [Fact]
    public void RgbColor_PredefinedColors_ShouldExist()
    {
        // Act & Assert - Verify all predefined colors are accessible
        Assert.Equal(new MikuRgbColor(0, 0, 0), MikuRgbColor.Black);
        Assert.Equal(new MikuRgbColor(255, 255, 255), MikuRgbColor.White);
        Assert.Equal(new MikuRgbColor(255, 0, 0), MikuRgbColor.Red);
        Assert.Equal(new MikuRgbColor(0, 255, 0), MikuRgbColor.Green);
        Assert.Equal(new MikuRgbColor(0, 0, 255), MikuRgbColor.Blue);
        Assert.Equal(new MikuRgbColor(255, 255, 0), MikuRgbColor.Yellow);
        Assert.Equal(new MikuRgbColor(0, 255, 255), MikuRgbColor.Cyan);
        Assert.Equal(new MikuRgbColor(255, 0, 255), MikuRgbColor.Magenta);
    }

    #endregion

    #region Extended256ColorOptions Tests

    [Fact]
    public void Extended256ColorOptions_DefaultValues_ShouldBeSet()
    {
        // Arrange & Act
        var options = new MikuExtended256ColorOptions();

        // Assert
        Assert.Equal(245, options.TraceColor);
        Assert.Equal(226, options.DebugColor);
        Assert.Equal(44, options.InformationColor);
        Assert.Equal(208, options.WarningColor);
        Assert.Equal(196, options.ErrorColor);
        Assert.Equal(160, options.CriticalColor);
    }

    [Fact]
    public void Extended256ColorOptions_CustomValues_ShouldBeSettable()
    {
        // Arrange & Act
        var options = new MikuExtended256ColorOptions
        {
            TraceColor = 250,
            DebugColor = 220,
            InformationColor = 39,
            WarningColor = 214,
            ErrorColor = 9,
            CriticalColor = 52
        };

        // Assert
        Assert.Equal(250, options.TraceColor);
        Assert.Equal(220, options.DebugColor);
        Assert.Equal(39, options.InformationColor);
        Assert.Equal(214, options.WarningColor);
        Assert.Equal(9, options.ErrorColor);
        Assert.Equal(52, options.CriticalColor);
    }

    #endregion

    #region TrueColorOptions Tests

    [Fact]
    public void TrueColorOptions_DefaultValues_ShouldBeSet()
    {
        // Arrange & Act
        var options = new MikuTrueColorOptions();

        // Assert
        Assert.Equal(MikuRgbColor.Gray, options.TraceColor);
        Assert.Equal(MikuRgbColor.Yellow, options.DebugColor);
        Assert.Equal(MikuRgbColor.MikuCyan, options.InformationColor);
        Assert.Equal(MikuRgbColor.Orange, options.WarningColor);
        Assert.Equal(MikuRgbColor.Red, options.ErrorColor);
        Assert.Equal(MikuRgbColor.DarkRed, options.CriticalColor);
    }

    [Fact]
    public void TrueColorOptions_CustomRgbValues_ShouldBeSettable()
    {
        // Arrange & Act
        var customCyan = MikuRgbColor.FromHex("#39C5BB"); // Miku's hair highlight
        var options = new MikuTrueColorOptions
        {
            InformationColor = customCyan
        };

        // Assert
        Assert.Equal(customCyan, options.InformationColor);
        Assert.Equal("#39C5BB", options.InformationColor.ToHex());
    }

    #endregion

    #region ConsoleColorOptions Integration Tests

    [Fact]
    public void ConsoleColorOptions_Extended256Colors_ShouldBeAccessible()
    {
        // Arrange
        var options = new MikuConsoleColorOptions
        {
            ColorSpace = MikuColorSpace.Extended256
        };

        // Act
        options.Extended256Colors.InformationColor = 51; // Bright cyan

        // Assert
        Assert.Equal(MikuColorSpace.Extended256, options.ColorSpace);
        Assert.Equal(51, options.Extended256Colors.InformationColor);
    }

    [Fact]
    public void ConsoleColorOptions_TrueColors_ShouldBeAccessible()
    {
        // Arrange
        var options = new MikuConsoleColorOptions
        {
            ColorSpace = MikuColorSpace.TrueColor
        };

        // Act
        options.TrueColors.InformationColor = MikuRgbColor.MikuTeal;

        // Assert
        Assert.Equal(MikuColorSpace.TrueColor, options.ColorSpace);
        Assert.Equal(MikuRgbColor.MikuTeal, options.TrueColors.InformationColor);
    }

    [Fact]
    public void ConsoleColorOptions_AllColorSpaces_ShouldHaveDefaults()
    {
        // Arrange & Act
        var options = new MikuConsoleColorOptions();

        // Assert
        Assert.NotNull(options.Extended256Colors);
        Assert.NotNull(options.TrueColors);
        Assert.Equal(ConsoleColor.Cyan, options.InformationColor);
        Assert.Equal(44, options.Extended256Colors.InformationColor);
        Assert.Equal(MikuRgbColor.MikuCyan, options.TrueColors.InformationColor);
    }

    #endregion

    #region MikuLogger Integration Tests

    [Fact]
    public void MikuLogger_WithExtended256_ShouldNotThrow()
    {
        // Arrange
        var options = new MikuLoggerOptions
        {
            Output = MikuLogOutput.Console,
            ConsoleColors = new MikuConsoleColorOptions
            {
                Enabled = true,
                ColorSpace = MikuColorSpace.Extended256
            }
        };

        // Act & Assert
        using var logger = new MikuLogger("Extended256Test", options);
        var exception = Record.Exception(() => logger.LogInformation("Test with 256 colors"));
        Assert.Null(exception);
    }

    [Fact]
    public void MikuLogger_WithTrueColor_ShouldNotThrow()
    {
        // Arrange
        var options = new MikuLoggerOptions
        {
            Output = MikuLogOutput.Console,
            ConsoleColors = new MikuConsoleColorOptions
            {
                Enabled = true,
                ColorSpace = MikuColorSpace.TrueColor,
                TrueColors = new MikuTrueColorOptions
                {
                    InformationColor = MikuRgbColor.MikuCyan
                }
            }
        };

        // Act & Assert
        using var logger = new MikuLogger("TrueColorTest", options);
        var exception = Record.Exception(() => logger.LogInformation("Test with TrueColor - Miku Cyan!"));
        Assert.Null(exception);
    }

    [Fact]
    public void MikuLogger_WithCustomHexColor_ShouldNotThrow()
    {
        // Arrange
        var options = new MikuLoggerOptions
        {
            Output = MikuLogOutput.Console,
            ConsoleColors = new MikuConsoleColorOptions
            {
                Enabled = true,
                ColorSpace = MikuColorSpace.TrueColor,
                TrueColors = new MikuTrueColorOptions
                {
                    TraceColor = MikuRgbColor.FromHex("#808080"),
                    DebugColor = MikuRgbColor.FromHex("#FFD700"),
                    InformationColor = MikuRgbColor.FromHex("#00CED1"),
                    WarningColor = MikuRgbColor.FromHex("#FFA500"),
                    ErrorColor = MikuRgbColor.FromHex("#FF4444"),
                    CriticalColor = MikuRgbColor.FromHex("#8B0000")
                }
            }
        };

        // Act & Assert
        using var logger = new MikuLogger("CustomColorTest", options);

        var exceptions = new List<Exception?>
        {
            Record.Exception(() => logger.LogTrace("Trace")),
            Record.Exception(() => logger.LogDebug("Debug")),
            Record.Exception(() => logger.LogInformation("Info")),
            Record.Exception(() => logger.LogWarning("Warning")),
            Record.Exception(() => logger.LogError("Error")),
            Record.Exception(() => logger.LogCritical("Critical"))
        };

        Assert.All(exceptions, ex => Assert.Null(ex));
    }

    [Fact]
    public void MikuLogger_ColorSpaceFallback_WhenDisabled_ShouldNotThrow()
    {
        // Arrange
        var options = new MikuLoggerOptions
        {
            Output = MikuLogOutput.Console,
            ConsoleColors = new MikuConsoleColorOptions
            {
                Enabled = false, // Disabled - should not use any colors
                ColorSpace = MikuColorSpace.TrueColor
            }
        };

        // Act & Assert
        using var logger = new MikuLogger("DisabledColorTest", options);
        var exception = Record.Exception(() => logger.LogInformation("Test without colors"));
        Assert.Null(exception);
    }

    #endregion
}
