using Miku.Core;
using MikuLib.Console;

namespace MikuLib.Console.Tests;

/// <summary>
/// Tests for MikuConsole static class.
/// Note: Many methods write to Console.Out, so we capture output using StringWriter.
/// </summary>
public class MikuConsoleTests
{
    /// <summary>
    /// Helper to capture console output for a single test action.
    /// </summary>
    private static string CaptureOutput(Action action)
    {
        var originalOut = System.Console.Out;
        using var stringWriter = new StringWriter();
        System.Console.SetOut(stringWriter);
        try
        {
            action();
            stringWriter.Flush();
            return stringWriter.ToString();
        }
        finally
        {
            System.Console.SetOut(originalOut);
        }
    }

    #region Constants Tests

    [Fact]
    public void FullBlock_ShouldBeCorrectUnicodeCharacter()
    {
        Assert.Equal('\u2588', MikuConsole.FullBlock);
    }

    [Fact]
    public void MediumShade_ShouldBeCorrectUnicodeCharacter()
    {
        Assert.Equal('\u2592', MikuConsole.MediumShade);
    }

    [Fact]
    public void LightShade_ShouldBeCorrectUnicodeCharacter()
    {
        Assert.Equal('\u2591', MikuConsole.LightShade);
    }

    [Fact]
    public void DarkShade_ShouldBeCorrectUnicodeCharacter()
    {
        Assert.Equal('\u2593', MikuConsole.DarkShade);
    }

    #endregion

    #region Write Methods - Plain Tests

    [Fact]
    public void Write_Plain_ShouldWriteText()
    {
        var output = CaptureOutput(() => MikuConsole.Write("Hello"));
        Assert.Contains("Hello", output);
    }

    [Fact]
    public void WriteLine_Plain_ShouldWriteTextWithNewline()
    {
        var output = CaptureOutput(() => MikuConsole.WriteLine("Hello"));
        Assert.Contains("Hello", output);
        Assert.Contains(Environment.NewLine, output);
    }

    [Fact]
    public void WriteLine_Empty_ShouldWriteNewline()
    {
        var output = CaptureOutput(() => MikuConsole.WriteLine());
        Assert.Equal(Environment.NewLine, output);
    }

    #endregion

    #region Write Methods - TrueColor Tests

    [Fact]
    public void Write_WithColor_ShouldContainAnsiCodes()
    {
        var output = CaptureOutput(() => MikuConsole.Write("Test", RgbColor.MikuCyan));
        
        Assert.Contains("\x1b[", output);
        Assert.Contains("Test", output);
        Assert.Contains("\x1b[0m", output);
    }

    [Fact]
    public void Write_WithForegroundAndBackground_ShouldContainBothCodes()
    {
        var output = CaptureOutput(() => MikuConsole.Write("Test", RgbColor.White, RgbColor.Black));
        
        Assert.Contains("38;2;", output);
        Assert.Contains("48;2;", output);
    }

    [Fact]
    public void WriteLine_WithColor_ShouldContainNewline()
    {
        var output = CaptureOutput(() => MikuConsole.WriteLine("Test", RgbColor.MikuCyan));
        
        Assert.Contains("Test", output);
        Assert.Contains(Environment.NewLine, output);
    }

    #endregion

    #region Write Methods - 256 Color Tests

    [Fact]
    public void Write256_ShouldContain256ColorCode()
    {
        var output = CaptureOutput(() => MikuConsole.Write256("Test", 44));
        
        Assert.Contains("38;5;44", output);
        Assert.Contains("Test", output);
    }

    [Fact]
    public void Write256_WithBackground_ShouldContainBothCodes()
    {
        var output = CaptureOutput(() => MikuConsole.Write256("Test", 44, 160));
        
        Assert.Contains("38;5;44", output);
        Assert.Contains("48;5;160", output);
    }

    [Fact]
    public void WriteLine256_ShouldContainNewline()
    {
        var output = CaptureOutput(() => MikuConsole.WriteLine256("Test", 44));
        
        Assert.Contains("Test", output);
        Assert.Contains(Environment.NewLine, output);
    }

    #endregion

    #region Styled Write Methods Tests

    [Fact]
    public void WriteBold_ShouldContainBoldCode()
    {
        var output = CaptureOutput(() => MikuConsole.WriteBold("Bold", RgbColor.White));
        
        Assert.Contains("\x1b[1m", output);
        Assert.Contains("Bold", output);
    }

    [Fact]
    public void WriteUnderline_ShouldContainUnderlineCode()
    {
        var output = CaptureOutput(() => MikuConsole.WriteUnderline("Underline", RgbColor.White));
        
        Assert.Contains("\x1b[4m", output);
        Assert.Contains("Underline", output);
    }

    [Fact]
    public void WriteItalic_ShouldContainItalicCode()
    {
        var output = CaptureOutput(() => MikuConsole.WriteItalic("Italic", RgbColor.White));
        
        Assert.Contains("\x1b[3m", output);
        Assert.Contains("Italic", output);
    }

    [Fact]
    public void WriteStyled_WithMultipleStyles_ShouldContainAllCodes()
    {
        var output = CaptureOutput(() => 
            MikuConsole.WriteStyled("Styled", RgbColor.White, AnsiCodes.Bold, AnsiCodes.Underline));
        
        Assert.Contains("\x1b[1m", output);
        Assert.Contains("\x1b[4m", output);
        Assert.Contains("Styled", output);
    }

    #endregion

    #region Gradient Methods Tests

    [Fact]
    public void WriteGradient_ShouldWriteEachCharacterWithDifferentColor()
    {
        var output = CaptureOutput(() => MikuConsole.WriteGradient("ABC", RgbColor.Red, RgbColor.Blue));
        
        Assert.Contains("A", output);
        Assert.Contains("B", output);
        Assert.Contains("C", output);
        Assert.Contains("38;2;", output);
    }

    [Fact]
    public void WriteGradientLine_ShouldContainNewline()
    {
        var output = CaptureOutput(() => MikuConsole.WriteGradientLine("Test", RgbColor.Red, RgbColor.Blue));
        
        Assert.Contains(Environment.NewLine, output);
    }

    [Fact]
    public void WriteRainbow_ShouldWriteText()
    {
        var output = CaptureOutput(() => MikuConsole.WriteRainbow("Rainbow"));
        
        Assert.Contains("R", output);
        Assert.Contains("a", output);
        Assert.Contains("i", output);
        Assert.Contains("n", output);
        Assert.Contains("b", output);
        Assert.Contains("o", output);
        Assert.Contains("w", output);
    }

    [Fact]
    public void WriteRainbowLine_ShouldContainNewline()
    {
        var output = CaptureOutput(() => MikuConsole.WriteRainbowLine("Test"));
        
        Assert.Contains(Environment.NewLine, output);
    }

    [Fact]
    public void WriteMikuRainbow_ShouldWriteText()
    {
        var output = CaptureOutput(() => MikuConsole.WriteMikuRainbow("Miku"));
        
        Assert.Contains("M", output);
        Assert.Contains("i", output);
        Assert.Contains("k", output);
        Assert.Contains("u", output);
    }

    [Fact]
    public void WriteMikuRainbowLine_ShouldContainNewline()
    {
        var output = CaptureOutput(() => MikuConsole.WriteMikuRainbowLine("Test"));
        
        Assert.Contains(Environment.NewLine, output);
    }

    #endregion

    #region Block Drawing Tests

    [Fact]
    public void DrawBar_ShouldWriteFullBlockCharacters()
    {
        var output = CaptureOutput(() => MikuConsole.DrawBar(5, RgbColor.MikuCyan));
        
        int blockCount = output.Count(c => c == '\u2588');
        Assert.Equal(5, blockCount);
    }

    [Fact]
    public void DrawBar_WithCustomCharacter_ShouldUseCustomCharacter()
    {
        var output = CaptureOutput(() => MikuConsole.DrawBar(3, RgbColor.MikuCyan, 'X'));
        
        Assert.Contains("XXX", output);
    }

    [Fact]
    public void DrawGradientBar_ShouldWriteCorrectWidth()
    {
        var output = CaptureOutput(() => MikuConsole.DrawGradientBar(10, RgbColor.Red, RgbColor.Blue));
        
        int blockCount = output.Count(c => c == '\u2588');
        Assert.Equal(10, blockCount);
    }

    [Fact]
    public void DrawGradientBar_WithCustomCharacter_ShouldUseCustomCharacter()
    {
        var output = CaptureOutput(() => MikuConsole.DrawGradientBar(5, RgbColor.Red, RgbColor.Blue, '*'));
        
        int starCount = output.Count(c => c == '*');
        Assert.Equal(5, starCount);
    }

    #endregion

    #region ANSI Support Tests

    [Fact]
    public void SupportsAnsi_ShouldReturnBoolean()
    {
        var result = MikuConsole.SupportsAnsi;
        Assert.IsType<bool>(result);
    }

    #endregion

    #region Reset Methods Tests

    [Fact]
    public void ResetColors_ShouldWriteResetCode()
    {
        var output = CaptureOutput(() => MikuConsole.ResetColors());
        
        Assert.Contains("\x1b[0m", output);
    }

    #endregion

    #region Thread Safety Tests

    [Fact]
    public async Task Write_ShouldBeThreadSafe()
    {
        var originalOut = System.Console.Out;
        using var stringWriter = new StringWriter();
        System.Console.SetOut(stringWriter);
        
        try
        {
            var tasks = new List<Task>();
            
            for (int i = 0; i < 10; i++)
            {
                int index = i;
                tasks.Add(Task.Run(() => MikuConsole.Write($"Task{index}", RgbColor.MikuCyan)));
            }
            
            await Task.WhenAll(tasks);
            
            stringWriter.Flush();
            var output = stringWriter.ToString();
            
            for (int i = 0; i < 10; i++)
            {
                Assert.Contains($"Task{i}", output);
            }
        }
        finally
        {
            System.Console.SetOut(originalOut);
        }
    }

    #endregion
}
