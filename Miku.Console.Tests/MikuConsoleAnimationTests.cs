// Type alias for backward compatibility
using Miku.Console;
using Miku.Core;

namespace MikuLib.Console.Tests;

/// <summary>
/// Tests for MikuConsoleAnimation static class.
/// </summary>
public class MikuConsoleAnimationTests
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

    #region Typewriter Tests

    [Fact]
    public async Task TypewriterAsync_ShouldWriteAllCharacters()
    {
        var output = await CaptureOutputAsync(async () =>
        {
            await MikuConsoleAnimation.TypewriterAsync("Hello", MikuRgbColor.MikuCyan, delayMs: 1);
        });

        Assert.Contains("H", output);
        Assert.Contains("e", output);
        Assert.Contains("l", output);
        Assert.Contains("o", output);
    }

    [Fact]
    public async Task TypewriterAsync_WithCancellation_ShouldStopEarly()
    {
        using var cts = new CancellationTokenSource();
        cts.CancelAfter(50);

        var output = await CaptureOutputAsync(async () =>
        {
            try
            {
                await MikuConsoleAnimation.TypewriterAsync(
                    "This is a very long text that should be cancelled",
                    MikuRgbColor.MikuCyan,
                    delayMs: 100,
                    cts.Token
                );
            }
            catch (TaskCanceledException)
            {
                // Expected behavior
            }
        });

        Assert.Contains("T", output);
    }

    [Fact]
    public async Task TypewriterLineAsync_ShouldWriteTextAndEndWithNewline()
    {
        var output = await CaptureOutputAsync(async () =>
        {
            await MikuConsoleAnimation.TypewriterLineAsync("Test", MikuRgbColor.MikuCyan, delayMs: 1);
        });

        // Check that the text was written
        Assert.Contains("T", output);
        Assert.Contains("e", output);
        Assert.Contains("s", output);
        Assert.Contains("t", output);
        // Check that output ends with newline
        Assert.True(output.TrimEnd().Length < output.Length || output.Contains(Environment.NewLine));
    }

    [Fact]
    public async Task TypewriterGradientAsync_ShouldWriteWithGradientColors()
    {
        var output = await CaptureOutputAsync(async () =>
        {
            await MikuConsoleAnimation.TypewriterGradientAsync(
                "ABC",
                MikuRgbColor.Red,
                MikuRgbColor.Blue,
                delayMs: 1
            );
        });

        Assert.Contains("A", output);
        Assert.Contains("B", output);
        Assert.Contains("C", output);
        Assert.Contains("38;2;", output);
    }

    [Fact]
    public async Task TypewriterAsync_WithEmptyString_ShouldNotThrow()
    {
        await CaptureOutputAsync(async () =>
        {
            await MikuConsoleAnimation.TypewriterAsync("", MikuRgbColor.MikuCyan, delayMs: 1);
        });
        Assert.True(true);
    }

    #endregion

    #region Empty Input Tests

    [Fact]
    public async Task RevealLinesAsync_WithEmptyArray_ShouldNotThrow()
    {
        await CaptureOutputAsync(async () =>
        {
            await MikuConsoleAnimation.RevealLinesAsync(
                Array.Empty<string>(),
                MikuRgbColor.MikuCyan,
                startX: 0,
                startY: 0,
                delayPerLineMs: 1
            );
        });
        Assert.True(true);
    }

    #endregion
}
