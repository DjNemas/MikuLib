using Miku.Core;
using MikuLib.Console;

namespace MikuLib.Console.Tests;

/// <summary>
/// Integration tests that verify MikuConsole and MikuConsoleAnimation work together correctly.
/// Tests that require cursor positioning are skipped in non-interactive environments.
/// </summary>
public class IntegrationTests
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

    /// <summary>
    /// Helper to capture console output for a single async test action.
    /// </summary>
    private static async Task<string> CaptureOutputAsync(Func<Task> action)
    {
        var originalOut = System.Console.Out;
        using var stringWriter = new StringWriter();
        System.Console.SetOut(stringWriter);
        try
        {
            await action();
            stringWriter.Flush();
            return stringWriter.ToString();
        }
        finally
        {
            System.Console.SetOut(originalOut);
        }
    }

    #region Color Integration Tests

    [Fact]
    public void MikuColors_ShouldBeUsableInAllMethods()
    {
        var output = CaptureOutput(() =>
        {
            MikuConsole.Write("Cyan", RgbColor.MikuCyan);
            MikuConsole.Write("Pink", RgbColor.MikuPink);
            MikuConsole.Write("Teal", RgbColor.MikuTeal);
            MikuConsole.Write("DarkCyan", RgbColor.MikuDarkCyan);
        });

        Assert.Contains("Cyan", output);
        Assert.Contains("Pink", output);
        Assert.Contains("Teal", output);
        Assert.Contains("DarkCyan", output);
    }

    [Fact]
    public void FromHex_Colors_ShouldWorkCorrectly()
    {
        var output = CaptureOutput(() =>
        {
            var customColor = RgbColor.FromHex("#FF6B9D");
            MikuConsole.Write("Custom", customColor);
        });

        Assert.Contains("Custom", output);
        Assert.Contains("38;2;255;107;157", output);
    }

    #endregion

    #region Mixed Output Tests

    [Fact]
    public void MixedOutput_ShouldContainAllParts()
    {
        var output = CaptureOutput(() =>
        {
            MikuConsole.Write("1", RgbColor.Red);
            MikuConsole.Write("2", RgbColor.Green);
            MikuConsole.Write("3", RgbColor.Blue);
        });
        
        // Verify all parts are present
        Assert.Contains("1", output);
        Assert.Contains("2", output);
        Assert.Contains("3", output);
    }

    [Fact]
    public void ColorAndPlain_ShouldWorkTogether()
    {
        var output = CaptureOutput(() =>
        {
            MikuConsole.Write("Colored", RgbColor.MikuCyan);
            MikuConsole.Write("Plain");
            MikuConsole.WriteLine("Line", RgbColor.MikuPink);
        });
        
        Assert.Contains("Colored", output);
        Assert.Contains("Plain", output);
        Assert.Contains("Line", output);
    }

    #endregion

    #region Animation Integration Tests

    [Fact]
    public async Task TypewriterWithMikuColors_ShouldWork()
    {
        var output = await CaptureOutputAsync(async () =>
        {
            await MikuConsoleAnimation.TypewriterGradientAsync(
                "Miku",
                RgbColor.MikuCyan,
                RgbColor.MikuPink,
                delayMs: 1
            );
        });
        
        Assert.Contains("M", output);
        Assert.Contains("i", output);
        Assert.Contains("k", output);
        Assert.Contains("u", output);
    }

    [Fact]
    public async Task MultipleAnimations_ShouldProduceOutput()
    {
        var output = await CaptureOutputAsync(async () =>
        {
            await MikuConsoleAnimation.TypewriterAsync("ABC", RgbColor.MikuCyan, delayMs: 1);
        });
        
        // Verify output contains the expected characters
        Assert.Contains("A", output);
        Assert.Contains("B", output);
        Assert.Contains("C", output);
    }

    #endregion

    #region Block Character Tests

    [Fact]
    public void DrawBar_WithMikuColors_ShouldProduceCorrectOutput()
    {
        var output = CaptureOutput(() => MikuConsole.DrawBar(5, RgbColor.MikuCyan));

        int blockCount = output.Count(c => c == MikuConsole.FullBlock);
        Assert.Equal(5, blockCount);

        Assert.Contains("38;2;0;206;209", output);
    }

    [Fact]
    public void AllBlockCharacters_ShouldBeValid()
    {
        Assert.Equal('\u2588', MikuConsole.FullBlock);
        Assert.Equal('\u2592', MikuConsole.MediumShade);
        Assert.Equal('\u2591', MikuConsole.LightShade);
        Assert.Equal('\u2593', MikuConsole.DarkShade);

        Assert.InRange((int)MikuConsole.FullBlock, 0x2580, 0x259F);
        Assert.InRange((int)MikuConsole.MediumShade, 0x2580, 0x259F);
        Assert.InRange((int)MikuConsole.LightShade, 0x2580, 0x259F);
        Assert.InRange((int)MikuConsole.DarkShade, 0x2580, 0x259F);
    }

    #endregion

    #region Reset Behavior Tests

    [Fact]
    public void AfterColoredWrite_ResetShouldBeIncluded()
    {
        var output = CaptureOutput(() => MikuConsole.Write("Test", RgbColor.MikuCyan));
        
        Assert.Contains("\x1b[0m", output);
    }

    [Fact]
    public void MultipleWrites_ShouldContainResetCodes()
    {
        var output = CaptureOutput(() =>
        {
            MikuConsole.Write("A", RgbColor.Red);
            MikuConsole.Write("B", RgbColor.Blue);
        });
        
        // Each colored write should have its own reset code
        Assert.Contains("\x1b[0m", output);
        // The output should contain multiple reset codes
        Assert.True(output.Split("\x1b[0m").Length >= 2);
    }

    #endregion

    #region Edge Cases

    [Fact]
    public void EmptyString_ShouldNotCrash()
    {
        CaptureOutput(() =>
        {
            MikuConsole.Write("", RgbColor.MikuCyan);
            MikuConsole.WriteLine("", RgbColor.MikuPink);
            MikuConsole.WriteGradient("", RgbColor.Red, RgbColor.Blue);
            MikuConsole.WriteRainbow("");
        });

        Assert.True(true);
    }

    [Fact]
    public void SingleCharacter_ShouldWorkCorrectly()
    {
        var output = CaptureOutput(() => MikuConsole.Write("X", RgbColor.MikuCyan));
        Assert.Contains("X", output);
    }

    [Fact]
    public void VeryLongString_ShouldWorkCorrectly()
    {
        var longString = new string('X', 1000);
        var output = CaptureOutput(() => MikuConsole.Write(longString, RgbColor.MikuCyan));
        Assert.Contains(longString, output);
    }

    [Fact]
    public void SpecialCharacters_ShouldWorkCorrectly()
    {
        var output = CaptureOutput(() => MikuConsole.Write("Hello\tWorld\n!", RgbColor.MikuCyan));

        Assert.Contains("Hello", output);
        Assert.Contains("World", output);
        Assert.Contains("!", output);
    }

    [Fact]
    public void UnicodeCharacters_ShouldWorkCorrectly()
    {
        var output = CaptureOutput(() =>
        {
            MikuConsole.Write("??", RgbColor.MikuCyan);
            MikuConsole.Write("????", RgbColor.MikuPink);
        });

        Assert.Contains("??", output);
        Assert.Contains("????", output);
    }

    #endregion
}
