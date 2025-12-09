using Miku.Core;

namespace MikuLib.Console
{
    /// <summary>
    /// Provides console animation utilities for MikuConsole.
    /// <para>
    /// This static class offers a variety of animation effects including:
    /// <list type="bullet">
    ///   <item><description>Typewriter effects - Character-by-character text reveal</description></item>
    ///   <item><description>Fade effects - Smooth transitions from/to black</description></item>
    ///   <item><description>Pulse effects - Color oscillation between two colors</description></item>
    ///   <item><description>Wave effects - Animated color waves across text</description></item>
    ///   <item><description>Loading indicators - Spinners and progress bars</description></item>
    ///   <item><description>Screen effects - Line-by-line reveals</description></item>
    /// </list>
    /// </para>
    /// </summary>
    /// <remarks>
    /// Like the choreography at Miku's concerts,
    /// bring your console to life with animations!
    /// <para>
    /// All animation methods support <see cref="CancellationToken"/> for graceful cancellation.
    /// </para>
    /// </remarks>
    /// <example>
    /// <code>
    /// // Typewriter effect with gradient
    /// await MikuConsoleAnimation.TypewriterGradientAsync(
    ///     "Hello Miku!",
    ///     RgbColor.MikuCyan,
    ///     RgbColor.MikuPink,
    ///     delayMs: 50
    /// );
    /// 
    /// // Pulsing text
    /// await MikuConsoleAnimation.PulseAsync(
    ///     "Pulsing!",
    ///     RgbColor.MikuCyan,
    ///     RgbColor.MikuPink,
    ///     x: 10, y: 5,
    ///     pulseCount: 3
    /// );
    /// </code>
    /// </example>
    public static class MikuConsoleAnimation
    {
        #region Typewriter Effect

        /// <summary>
        /// Writes text with a typewriter effect, displaying one character at a time.
        /// </summary>
        /// <param name="text">The text to write character by character.</param>
        /// <param name="color">The color to use for all characters.</param>
        /// <param name="delayMs">
        /// The delay in milliseconds between each character.
        /// Lower values create faster typing. Default is 30ms (~33 characters per second).
        /// </param>
        /// <param name="cancellationToken">
        /// A token to cancel the animation. When cancelled, the animation stops
        /// but already written characters remain visible.
        /// </param>
        /// <returns>A task that completes when all characters have been written or the operation is cancelled.</returns>
        /// <example>
        /// <code>
        /// await MikuConsoleAnimation.TypewriterAsync("Hello World!", RgbColor.MikuCyan, delayMs: 50);
        /// </code>
        /// </example>
        public static async Task TypewriterAsync(
            string text,
            RgbColor color,
            int delayMs = 30,
            CancellationToken cancellationToken = default)
        {
            foreach (char c in text)
            {
                if (cancellationToken.IsCancellationRequested) break;
                MikuConsole.Write(c.ToString(), color);
                await Task.Delay(delayMs, cancellationToken);
            }
        }

        /// <summary>
        /// Writes text with a typewriter effect followed by a newline.
        /// </summary>
        /// <param name="text">The text to write character by character.</param>
        /// <param name="color">The color to use for all characters.</param>
        /// <param name="delayMs">
        /// The delay in milliseconds between each character. Default is 30ms.
        /// </param>
        /// <param name="cancellationToken">A token to cancel the animation.</param>
        /// <returns>A task that completes when the text and newline have been written.</returns>
        /// <example>
        /// <code>
        /// await MikuConsoleAnimation.TypewriterLineAsync("Line 1", RgbColor.MikuCyan);
        /// await MikuConsoleAnimation.TypewriterLineAsync("Line 2", RgbColor.MikuPink);
        /// </code>
        /// </example>
        public static async Task TypewriterLineAsync(
            string text,
            RgbColor color,
            int delayMs = 30,
            CancellationToken cancellationToken = default)
        {
            await TypewriterAsync(text, color, delayMs, cancellationToken);
            MikuConsole.WriteLine();
        }

        /// <summary>
        /// Writes text with a typewriter effect using a color gradient from start to end.
        /// <para>
        /// Each character is displayed in a color that smoothly transitions from
        /// <paramref name="from"/> to <paramref name="to"/> based on its position in the text.
        /// </para>
        /// </summary>
        /// <param name="text">The text to write with gradient coloring.</param>
        /// <param name="from">The starting color (first character).</param>
        /// <param name="to">The ending color (last character).</param>
        /// <param name="delayMs">
        /// The delay in milliseconds between each character. Default is 30ms.
        /// </param>
        /// <param name="cancellationToken">A token to cancel the animation.</param>
        /// <returns>A task that completes when all characters have been written.</returns>
        /// <example>
        /// <code>
        /// // Gradient from cyan to pink
        /// await MikuConsoleAnimation.TypewriterGradientAsync(
        ///     "Beautiful gradient text!",
        ///     RgbColor.MikuCyan,
        ///     RgbColor.MikuPink,
        ///     delayMs: 40
        /// );
        /// </code>
        /// </example>
        public static async Task TypewriterGradientAsync(
            string text,
            RgbColor from,
            RgbColor to,
            int delayMs = 30,
            CancellationToken cancellationToken = default)
        {
            for (int i = 0; i < text.Length; i++)
            {
                if (cancellationToken.IsCancellationRequested) break;
                // Calculate the interpolation factor (0.0 to 1.0) based on character position
                var color = RgbColor.Lerp(from, to, i / (double)(text.Length - 1));
                MikuConsole.Write(text[i].ToString(), color);
                await Task.Delay(delayMs, cancellationToken);
            }
        }

        #endregion

        #region Fade Effects

        /// <summary>
        /// Fades in text from black to the target color at a specific position.
        /// <para>
        /// The text is redrawn multiple times with increasing brightness
        /// to create a smooth fade-in effect.
        /// </para>
        /// </summary>
        /// <param name="text">The text to fade in.</param>
        /// <param name="targetColor">The final color when fully faded in.</param>
        /// <param name="x">The horizontal cursor position (0-based, left edge).</param>
        /// <param name="y">The vertical cursor position (0-based, top edge).</param>
        /// <param name="durationMs">
        /// Total duration of the fade effect in milliseconds. Default is 500ms.
        /// </param>
        /// <param name="steps">
        /// Number of intermediate steps in the fade. More steps = smoother fade.
        /// Default is 20 steps.
        /// </param>
        /// <param name="cancellationToken">A token to cancel the animation.</param>
        /// <returns>A task that completes when the fade-in is complete.</returns>
        /// <example>
        /// <code>
        /// await MikuConsoleAnimation.FadeInAsync(
        ///     "Appearing text",
        ///     RgbColor.MikuCyan,
        ///     x: 10, y: 5,
        ///     durationMs: 1000,
        ///     steps: 30
        /// );
        /// </code>
        /// </example>
        public static async Task FadeInAsync(
            string text,
            RgbColor targetColor,
            int x,
            int y,
            int durationMs = 500,
            int steps = 20,
            CancellationToken cancellationToken = default)
        {
            int delayPerStep = durationMs / steps;

            for (int i = 0; i <= steps; i++)
            {
                if (cancellationToken.IsCancellationRequested) break;

                // Interpolate from black (0%) to target color (100%)
                var color = RgbColor.Lerp(RgbColor.Black, targetColor, i / (double)steps);
                MikuConsole.WriteAt(x, y, text, color);
                await Task.Delay(delayPerStep, cancellationToken);
            }
        }

        /// <summary>
        /// Fades out text from the start color to black at a specific position.
        /// <para>
        /// The text is redrawn with decreasing brightness and then cleared
        /// with spaces when the fade completes.
        /// </para>
        /// </summary>
        /// <param name="text">The text to fade out.</param>
        /// <param name="startColor">The initial color before fading.</param>
        /// <param name="x">The horizontal cursor position (0-based, left edge).</param>
        /// <param name="y">The vertical cursor position (0-based, top edge).</param>
        /// <param name="durationMs">
        /// Total duration of the fade effect in milliseconds. Default is 500ms.
        /// </param>
        /// <param name="steps">
        /// Number of intermediate steps in the fade. Default is 20 steps.
        /// </param>
        /// <param name="cancellationToken">A token to cancel the animation.</param>
        /// <returns>A task that completes when the fade-out is complete and text is cleared.</returns>
        /// <example>
        /// <code>
        /// await MikuConsoleAnimation.FadeOutAsync(
        ///     "Disappearing text",
        ///     RgbColor.MikuCyan,
        ///     x: 10, y: 5,
        ///     durationMs: 800
        /// );
        /// </code>
        /// </example>
        public static async Task FadeOutAsync(
            string text,
            RgbColor startColor,
            int x,
            int y,
            int durationMs = 500,
            int steps = 20,
            CancellationToken cancellationToken = default)
        {
            int delayPerStep = durationMs / steps;

            for (int i = 0; i <= steps; i++)
            {
                if (cancellationToken.IsCancellationRequested) break;

                // Interpolate from start color (0%) to black (100%)
                var color = RgbColor.Lerp(startColor, RgbColor.Black, i / (double)steps);
                MikuConsole.WriteAt(x, y, text, color);
                await Task.Delay(delayPerStep, cancellationToken);
            }

            // Clear the text by overwriting with spaces
            MikuConsole.WriteAt(x, y, new string(' ', text.Length), RgbColor.Black);
        }

        #endregion

        #region Pulse Effects

        /// <summary>
        /// Pulses text between two colors at a specific position.
        /// <para>
        /// The text smoothly transitions from <paramref name="color1"/> to <paramref name="color2"/>
        /// and back, creating a pulsing or throbbing visual effect.
        /// </para>
        /// </summary>
        /// <param name="text">The text to pulse. Can contain newlines for multi-line text.</param>
        /// <param name="color1">The first color (start and end of each pulse cycle).</param>
        /// <param name="color2">The second color (peak of each pulse cycle).</param>
        /// <param name="x">The horizontal cursor position (0-based, left edge).</param>
        /// <param name="y">The vertical cursor position (0-based, top edge).</param>
        /// <param name="pulseCount">Number of complete pulse cycles. Default is 3.</param>
        /// <param name="pulseDurationMs">
        /// Duration of each pulse cycle in milliseconds. Default is 500ms.
        /// </param>
        /// <param name="cancellationToken">A token to cancel the animation.</param>
        /// <returns>A task that completes when all pulse cycles are finished.</returns>
        /// <example>
        /// <code>
        /// await MikuConsoleAnimation.PulseAsync(
        ///     "ALERT!",
        ///     RgbColor.MikuCyan,
        ///     RgbColor.MikuPink,
        ///     x: 10, y: 5,
        ///     pulseCount: 5,
        ///     pulseDurationMs: 300
        /// );
        /// </code>
        /// </example>
        public static async Task PulseAsync(
            string text,
            RgbColor color1,
            RgbColor color2,
            int x,
            int y,
            int pulseCount = 3,
            int pulseDurationMs = 500,
            CancellationToken cancellationToken = default)
        {
            int steps = 20;
            int delayPerStep = pulseDurationMs / steps / 2; // Divide by 2 for up and down

            for (int p = 0; p < pulseCount; p++)
            {
                // Pulse up: color1 -> color2
                for (int i = 0; i <= steps; i++)
                {
                    if (cancellationToken.IsCancellationRequested) return;
                    var color = RgbColor.Lerp(color1, color2, i / (double)steps);
                    MikuConsole.WriteAt(x, y, text, color);
                    await Task.Delay(delayPerStep, cancellationToken);
                }

                // Pulse down: color2 -> color1
                for (int i = steps; i >= 0; i--)
                {
                    if (cancellationToken.IsCancellationRequested) return;
                    var color = RgbColor.Lerp(color1, color2, i / (double)steps);
                    MikuConsole.WriteAt(x, y, text, color);
                    await Task.Delay(delayPerStep, cancellationToken);
                }
            }
        }

        /// <summary>
        /// Creates a smooth breathing effect that oscillates between dark and bright colors.
        /// <para>
        /// Unlike <see cref="PulseAsync"/>, this method uses a sine wave for smoother,
        /// more organic color transitions that resemble breathing.
        /// </para>
        /// </summary>
        /// <param name="text">The text to animate with breathing effect.</param>
        /// <param name="darkColor">The color at the "exhale" phase (dimmest point).</param>
        /// <param name="brightColor">The color at the "inhale" phase (brightest point).</param>
        /// <param name="x">The horizontal cursor position (0-based, left edge).</param>
        /// <param name="y">The vertical cursor position (0-based, top edge).</param>
        /// <param name="breathCount">Number of complete breath cycles. Default is 3.</param>
        /// <param name="breathDurationMs">
        /// Duration of each breath cycle in milliseconds. Default is 2000ms (2 seconds).
        /// </param>
        /// <param name="cancellationToken">A token to cancel the animation.</param>
        /// <returns>A task that completes when all breath cycles are finished.</returns>
        /// <example>
        /// <code>
        /// await MikuConsoleAnimation.BreathingAsync(
        ///     "Calm breathing...",
        ///     RgbColor.MikuDarkCyan,  // Exhale (dark)
        ///     RgbColor.MikuCyan,      // Inhale (bright)
        ///     x: 5, y: 10,
        ///     breathCount: 5,
        ///     breathDurationMs: 3000
        /// );
        /// </code>
        /// </example>
        public static async Task BreathingAsync(
            string text,
            RgbColor darkColor,
            RgbColor brightColor,
            int x,
            int y,
            int breathCount = 3,
            int breathDurationMs = 2000,
            CancellationToken cancellationToken = default)
        {
            int steps = 60; // Higher step count for smoother sine wave
            int delayPerStep = breathDurationMs / steps;

            for (int b = 0; b < breathCount; b++)
            {
                for (int i = 0; i < steps; i++)
                {
                    if (cancellationToken.IsCancellationRequested) return;

                    // Use sine wave for smooth oscillation
                    // Sin returns -1 to 1, we normalize to 0 to 1
                    double t = (Math.Sin(i * Math.PI * 2 / steps) + 1) / 2;
                    var color = RgbColor.Lerp(darkColor, brightColor, t);
                    MikuConsole.WriteAt(x, y, text, color);
                    await Task.Delay(delayPerStep, cancellationToken);
                }
            }
        }

        #endregion

        #region Wave Effects

        /// <summary>
        /// Creates a color wave effect that travels across the text.
        /// <para>
        /// Each character in the text oscillates between two colors with a phase offset,
        /// creating the appearance of a wave moving across the text.
        /// </para>
        /// </summary>
        /// <param name="text">The text to animate with wave effect.</param>
        /// <param name="color1">The first wave color.</param>
        /// <param name="color2">The second wave color.</param>
        /// <param name="x">The horizontal cursor position (0-based, left edge).</param>
        /// <param name="y">The vertical cursor position (0-based, top edge).</param>
        /// <param name="waveDurationMs">
        /// Duration of each wave cycle in milliseconds. Default is 2000ms.
        /// </param>
        /// <param name="waveCount">Number of wave cycles to display. Default is 3.</param>
        /// <param name="cancellationToken">A token to cancel the animation.</param>
        /// <returns>A task that completes when all wave cycles are finished.</returns>
        /// <example>
        /// <code>
        /// await MikuConsoleAnimation.ColorWaveAsync(
        ///     "Wave effect!",
        ///     RgbColor.MikuCyan,
        ///     RgbColor.MikuPink,
        ///     x: 10, y: 5,
        ///     waveDurationMs: 1500,
        ///     waveCount: 4
        /// );
        /// </code>
        /// </example>
        public static async Task ColorWaveAsync(
            string text,
            RgbColor color1,
            RgbColor color2,
            int x,
            int y,
            int waveDurationMs = 2000,
            int waveCount = 3,
            CancellationToken cancellationToken = default)
        {
            int frames = 60;
            int delayPerFrame = waveDurationMs / frames;

            for (int w = 0; w < waveCount; w++)
            {
                for (int frame = 0; frame < frames; frame++)
                {
                    if (cancellationToken.IsCancellationRequested) return;

                    MikuConsole.SetCursorPosition(x, y);

                    for (int i = 0; i < text.Length; i++)
                    {
                        // Calculate phase based on frame and character position
                        // i * 3 creates spacing between wave peaks
                        double phase = (frame + i * 3) * 0.2;
                        // Normalize sine wave from -1..1 to 0..1
                        double t = (Math.Sin(phase) + 1) / 2;
                        var color = RgbColor.Lerp(color1, color2, t);
                        MikuConsole.Write(text[i].ToString(), color);
                    }

                    await Task.Delay(delayPerFrame, cancellationToken);
                }
            }
        }

        /// <summary>
        /// Creates a rainbow wave effect that travels across the text.
        /// <para>
        /// Uses <see cref="ColorHelper.GetRainbow(double)"/> to cycle through
        /// the full spectrum of rainbow colors.
        /// </para>
        /// </summary>
        /// <param name="text">The text to animate with rainbow wave.</param>
        /// <param name="x">The horizontal cursor position (0-based, left edge).</param>
        /// <param name="y">The vertical cursor position (0-based, top edge).</param>
        /// <param name="durationMs">
        /// Total duration of the animation in milliseconds. Default is 3000ms.
        /// </param>
        /// <param name="cancellationToken">A token to cancel the animation.</param>
        /// <returns>A task that completes when the animation duration has elapsed.</returns>
        /// <example>
        /// <code>
        /// await MikuConsoleAnimation.RainbowWaveAsync(
        ///     "Rainbow magic!",
        ///     x: 10, y: 5,
        ///     durationMs: 5000
        /// );
        /// </code>
        /// </example>
        public static async Task RainbowWaveAsync(
            string text,
            int x,
            int y,
            int durationMs = 3000,
            CancellationToken cancellationToken = default)
        {
            int frames = durationMs / 30; // Target ~33 FPS

            for (int frame = 0; frame < frames; frame++)
            {
                if (cancellationToken.IsCancellationRequested) return;

                MikuConsole.SetCursorPosition(x, y);

                for (int i = 0; i < text.Length; i++)
                {
                    // Phase based on frame and character position creates wave motion
                    var color = ColorHelper.GetRainbow((frame + i) * 0.2);
                    MikuConsole.Write(text[i].ToString(), color);
                }

                await Task.Delay(30, cancellationToken); // ~33 FPS
            }
        }

        #endregion

        #region Loading Indicators

        /// <summary>
        /// Braille spinner characters for the <see cref="SpinnerAsync"/> animation.
        /// <para>
        /// These characters create a smooth spinning dot pattern:
        /// ? ? ? ? ? ? ? ? ? ?
        /// </para>
        /// </summary>
        private static readonly char[] SpinnerChars =
        {
            '\u280B', '\u2819', '\u2839', '\u2838', '\u283C', '\u2834', '\u2826', '\u2827', '\u2807', '\u280F'
        };

        /// <summary>
        /// Shows an animated spinner with a message.
        /// <para>
        /// Displays a rotating Braille dot pattern followed by a message,
        /// useful for indicating ongoing operations.
        /// </para>
        /// </summary>
        /// <param name="message">The message to display next to the spinner.</param>
        /// <param name="color">The color for both spinner and message.</param>
        /// <param name="x">The horizontal cursor position (0-based, left edge).</param>
        /// <param name="y">The vertical cursor position (0-based, top edge).</param>
        /// <param name="durationMs">
        /// Duration to show the spinner in milliseconds. Default is 2000ms.
        /// </param>
        /// <param name="cancellationToken">A token to cancel the animation.</param>
        /// <returns>A task that completes when the duration has elapsed.</returns>
        /// <example>
        /// <code>
        /// await MikuConsoleAnimation.SpinnerAsync(
        ///     "Loading data...",
        ///     RgbColor.MikuCyan,
        ///     x: 5, y: 10,
        ///     durationMs: 3000
        /// );
        /// </code>
        /// </example>
        public static async Task SpinnerAsync(
            string message,
            RgbColor color,
            int x,
            int y,
            int durationMs = 2000,
            CancellationToken cancellationToken = default)
        {
            int frame = 0;
            int elapsed = 0;

            while (elapsed < durationMs && !cancellationToken.IsCancellationRequested)
            {
                // Display spinner character followed by message
                MikuConsole.WriteAt(x, y, $"{SpinnerChars[frame % SpinnerChars.Length]} {message}", color);
                frame++;
                await Task.Delay(80, cancellationToken); // ~12.5 FPS for spinner
                elapsed += 80;
            }
        }

        /// <summary>
        /// Shows an animated progress bar that fills over time.
        /// <para>
        /// Displays a bracketed progress bar [????????] with percentage,
        /// useful for showing determinate progress.
        /// </para>
        /// </summary>
        /// <param name="x">The horizontal cursor position (0-based, left edge).</param>
        /// <param name="y">The vertical cursor position (0-based, top edge).</param>
        /// <param name="width">The width of the progress bar in characters (excluding brackets and percentage).</param>
        /// <param name="fillColor">The color for filled portion (? characters).</param>
        /// <param name="emptyColor">The color for empty portion (? characters).</param>
        /// <param name="durationMs">
        /// Duration to fill the progress bar in milliseconds. Default is 2000ms.
        /// </param>
        /// <param name="cancellationToken">A token to cancel the animation.</param>
        /// <returns>A task that completes when the progress bar reaches 100%.</returns>
        /// <example>
        /// <code>
        /// await MikuConsoleAnimation.ProgressBarAsync(
        ///     x: 5, y: 10,
        ///     width: 40,
        ///     fillColor: RgbColor.MikuCyan,
        ///     emptyColor: RgbColor.Gray,
        ///     durationMs: 3000
        /// );
        /// </code>
        /// </example>
        public static async Task ProgressBarAsync(
            int x,
            int y,
            int width,
            RgbColor fillColor,
            RgbColor emptyColor,
            int durationMs = 2000,
            CancellationToken cancellationToken = default)
        {
            const char fullBlock = '\u2588';  // ? - Filled portion
            const char lightShade = '\u2591'; // ? - Empty portion

            int steps = 50;
            int delayPerStep = durationMs / steps;

            for (int i = 0; i <= steps; i++)
            {
                if (cancellationToken.IsCancellationRequested) return;

                // Calculate filled and empty character counts
                int filled = (int)((i / (double)steps) * width);
                int empty = width - filled;

                // Draw progress bar: [????????????????] XX%
                MikuConsole.SetCursorPosition(x, y);
                MikuConsole.Write("[", RgbColor.White);
                MikuConsole.Write(new string(fullBlock, filled), fillColor);
                MikuConsole.Write(new string(lightShade, empty), emptyColor);
                MikuConsole.Write($"] {i * 100 / steps}%", RgbColor.White);

                await Task.Delay(delayPerStep, cancellationToken);
            }
        }

        #endregion

        #region Screen Effects

        /// <summary>
        /// Creates a line-by-line reveal effect for multiple lines of text.
        /// <para>
        /// Each line appears sequentially with a delay, creating a
        /// top-to-bottom reveal animation.
        /// </para>
        /// </summary>
        /// <param name="lines">An array of text lines to reveal.</param>
        /// <param name="color">The color to use for all lines.</param>
        /// <param name="startX">The horizontal starting position for all lines.</param>
        /// <param name="startY">The vertical starting position (first line).</param>
        /// <param name="delayPerLineMs">
        /// Delay between revealing each line in milliseconds. Default is 100ms.
        /// </param>
        /// <param name="cancellationToken">A token to cancel the animation.</param>
        /// <returns>A task that completes when all lines have been revealed.</returns>
        /// <example>
        /// <code>
        /// var credits = new[] {
        ///     "Created by Hatsune Nemas",
        ///     "Version 10.0.39",
        ///     "Thank you for using MikuLib!"
        /// };
        /// 
        /// await MikuConsoleAnimation.RevealLinesAsync(
        ///     credits,
        ///     RgbColor.MikuCyan,
        ///     startX: 10,
        ///     startY: 5,
        ///     delayPerLineMs: 200
        /// );
        /// </code>
        /// </example>
        public static async Task RevealLinesAsync(
            string[] lines,
            RgbColor color,
            int startX,
            int startY,
            int delayPerLineMs = 100,
            CancellationToken cancellationToken = default)
        {
            for (int i = 0; i < lines.Length; i++)
            {
                if (cancellationToken.IsCancellationRequested) return;
                MikuConsole.WriteAt(startX, startY + i, lines[i], color);
                await Task.Delay(delayPerLineMs, cancellationToken);
            }
        }

        /// <summary>
        /// Creates a line-by-line reveal with alternating colors for visual variety.
        /// <para>
        /// Odd-numbered lines use <paramref name="color1"/>, even-numbered lines
        /// use <paramref name="color2"/>, creating a striped pattern.
        /// </para>
        /// </summary>
        /// <param name="lines">An array of text lines to reveal.</param>
        /// <param name="color1">The color for lines at indices 0, 2, 4, etc.</param>
        /// <param name="color2">The color for lines at indices 1, 3, 5, etc.</param>
        /// <param name="startX">The horizontal starting position for all lines.</param>
        /// <param name="startY">The vertical starting position (first line).</param>
        /// <param name="delayPerLineMs">
        /// Delay between revealing each line in milliseconds. Default is 100ms.
        /// </param>
        /// <param name="cancellationToken">A token to cancel the animation.</param>
        /// <returns>A task that completes when all lines have been revealed.</returns>
        /// <example>
        /// <code>
        /// var menu = new[] {
        ///     "??????????????????????",
        ///     "?  Option 1          ?",
        ///     "?  Option 2          ?",
        ///     "?  Option 3          ?",
        ///     "??????????????????????"
        /// };
        /// 
        /// await MikuConsoleAnimation.RevealLinesAlternatingAsync(
        ///     menu,
        ///     RgbColor.MikuCyan,
        ///     RgbColor.MikuPink,
        ///     startX: 10,
        ///     startY: 5,
        ///     delayPerLineMs: 150
        /// );
        /// </code>
        /// </example>
        public static async Task RevealLinesAlternatingAsync(
            string[] lines,
            RgbColor color1,
            RgbColor color2,
            int startX,
            int startY,
            int delayPerLineMs = 100,
            CancellationToken cancellationToken = default)
        {
            for (int i = 0; i < lines.Length; i++)
            {
                if (cancellationToken.IsCancellationRequested) return;
                // Alternate colors based on line index (even/odd)
                var color = i % 2 == 0 ? color1 : color2;
                MikuConsole.WriteAt(startX, startY + i, lines[i], color);
                await Task.Delay(delayPerLineMs, cancellationToken);
            }
        }

        #endregion
    }
}
