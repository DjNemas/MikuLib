using Miku.Core;
using Miku.Logger;
using Miku.Logger.Configuration;
using Miku.Logger.Configuration.Enums;
using Miku.Logger.Configuration.Models;
using MikuLib.Console;

namespace MikuConsoleAndLogger.Preview;

/// <summary>
/// MikuConsole &amp; MikuLogger Demo - A visual showcase of all color capabilities!
/// <para>
/// This demo application demonstrates the full range of features provided by:
/// <list type="bullet">
///   <item><description><b>MikuConsole</b> - Advanced console output with TrueColor (24-bit RGB) and 256-color support</description></item>
///   <item><description><b>MikuLogger</b> - Flexible logging with multiple color modes and output targets</description></item>
/// </list>
/// </para>
/// <para>Featuring Hatsune Miku's signature colors and more!</para>
/// </summary>
public class Program
{
    /// <summary>
    /// Application entry point.
    /// <para>
    /// Initializes the MikuConsole environment, hides the cursor for clean animations,
    /// and ensures proper cleanup on exit.
    /// </para>
    /// </summary>
    /// <param name="args">Command line arguments (not used in this demo).</param>
    public static async Task Main(string[] args)
    {
        // Initialize MikuConsole to enable ANSI escape sequence support on Windows
        // This is required for TrueColor and 256-color output to work correctly
        MikuConsole.Initialize();
        
        // Hide the cursor to prevent flickering during animations
        MikuConsole.HideCursor();

        try
        {
            await RunDemo();
        }
        finally
        {
            // Always restore cursor visibility and reset colors on exit
            // This ensures the terminal is left in a clean state
            MikuConsole.ShowCursor();
            MikuConsole.ResetColors();
        }
    }

    /// <summary>
    /// Orchestrates the demo by running all sections in sequence.
    /// <para>
    /// Each section demonstrates different capabilities of MikuConsole and MikuLogger,
    /// progressing from basic 16-color output to advanced TrueColor animations.
    /// </para>
    /// </summary>
    private static async Task RunDemo()
    {
        // Section 0: Animated intro with ASCII art logo
        await ShowIntroAnimation();

        // Section 1: Basic 16-color console output (System.ConsoleColor compatible)
        await ShowStandardColors();

        // Section 2: Extended 256-color palette demonstration
        await ShowExtended256Colors();

        // Section 3: Full TrueColor (24-bit RGB) with 16 million colors
        await ShowTrueColorDemo();

        // Section 4: Miku-themed color palette showcase
        await ShowMikuTheme();

        // Section 5: Animated color gradients and effects
        await ShowGradientAnimations();

        // Section 6: MikuLogger simulating real application logging
        await ShowLoggerDemo();

        // Section 7: Rainbow wave animation using sine functions
        await ShowRainbowWave();

        // Section 8: Matrix-style rain effect with Japanese characters
        await ShowMikuMatrixRain();

        // Section 9: Closing animation with credits
        await ShowOutro();
    }

    #region Intro Animation

    /// <summary>
    /// Displays an animated introduction with the MIKU CONSOLE &amp; LOGGER ASCII art logo.
    /// <para>
    /// The animation includes:
    /// <list type="number">
    ///   <item><description>Fade-in effect from black to Miku Cyan</description></item>
    ///   <item><description>Pulsing effect alternating between Cyan and Pink</description></item>
    ///   <item><description>Typewriter-style version text with gradient colors</description></item>
    /// </list>
    /// </para>
    /// </summary>
    private static async Task ShowIntroAnimation()
    {
        MikuConsole.Clear();

        // ASCII art representation of "MIKU" centered, then "CONSOLE", "AND", and "LOGGER"
        // Clean, balanced design with proper spacing
        var mikuArt = new[]
        {
            @"",
            @"            ███╗   ███╗██╗██╗  ██╗██╗   ██╗",
            @"            ████╗ ████║██║██║ ██╔╝██║   ██║",
            @"            ██╔████╔██║██║█████╔╝ ██║   ██║",
            @"            ██║╚██╔╝██║██║██╔═██╗ ██║   ██║",
            @"            ██║ ╚═╝ ██║██║██║  ██╗╚██████╔╝",
            @"            ╚═╝     ╚═╝╚═╝╚═╝  ╚═╝ ╚═════╝ ",
            @"",
            @"   ██████╗ ██████╗ ███╗   ██╗███████╗ ██████╗ ██╗     ███████╗",
            @"  ██╔════╝██╔═══██╗████╗  ██║██╔════╝██╔═══██╗██║     ██╔════╝",
            @"  ██║     ██║   ██║██╔██╗ ██║███████╗██║   ██║██║     █████╗  ",
            @"  ██║     ██║   ██║██║╚██╗██║╚════██║██║   ██║██║     ██╔══╝  ",
            @"  ╚██████╗╚██████╔╝██║ ╚████║███████║╚██████╔╝███████╗███████╗",
            @"   ╚═════╝ ╚═════╝ ╚═╝  ╚═══╝╚══════╝ ╚═════╝ ╚══════╝╚══════╝",
            @"",
            @"                 █████╗ ███╗   ██╗██████╗ ",
            @"                ██╔══██╗████╗  ██║██╔══██╗",
            @"                ███████║██╔██╗ ██║██║  ██║",
            @"                ██╔══██║██║╚██╗██║██║  ██║",
            @"                ██║  ██║██║ ╚████║██████╔╝",
            @"                ╚═╝  ╚═╝╚═╝  ╚═══╝╚═════╝ ",
            @"",
            @"  ██╗      ██████╗  ██████╗  ██████╗ ███████╗██████╗ ",
            @"  ██║     ██╔═══██╗██╔════╝ ██╔════╝ ██╔════╝██╔══██╗",
            @"  ██║     ██║   ██║██║  ███╗██║  ███╗█████╗  ██████╔╝",
            @"  ██║     ██║   ██║██║   ██║██║   ██║██╔══╝  ██╔══██╗",
            @"  ███████╗╚██████╔╝╚██████╔╝╚██████╔╝███████╗██║  ██║",
            @"  ╚══════╝ ╚═════╝  ╚═════╝  ╚═════╝ ╚══════╝╚═╝  ╚═╝"
        };

        // Fade-in animation: gradually increase brightness from 0 to 255
        // Using RgbColor.Lerp to interpolate between black and Miku Cyan
        for (int brightness = 0; brightness <= 255; brightness += 15)
        {
            MikuConsole.SetCursorPosition(0, 0);

            foreach (var line in mikuArt)
            {
                // Calculate interpolated color based on current brightness level
                // brightness / 255.0 gives a value between 0.0 and 1.0
                var color = RgbColor.Lerp(RgbColor.Black, RgbColor.MikuCyan, brightness / 255.0);
                MikuConsole.WriteLine(line, color);
            }

            await Task.Delay(30); // ~33 FPS animation
        }

        // Pulsing effect: smoothly transition between Cyan and Pink
        // Uses MikuConsoleAnimation helper for consistent animation timing
        await MikuConsoleAnimation.PulseAsync(
            string.Join("\n", mikuArt),
            RgbColor.MikuCyan,
            RgbColor.MikuPink,
            0, 0,                    // Start position (x=0, y=0)
            pulseCount: 2,           // Number of pulse cycles
            pulseDurationMs: 1000    // Duration of each pulse in milliseconds
        );

        // Redraw the logo in solid Miku Cyan after pulse animation completes
        MikuConsole.SetCursorPosition(0, 0);
        foreach (var line in mikuArt)
        {
            MikuConsole.WriteLine(line, RgbColor.MikuCyan);
        }

        // Display version information with typewriter effect and gradient coloring
        MikuConsole.WriteLine();
        await MikuConsoleAnimation.TypewriterGradientAsync(
            "      MikuConsole v10.0.39 + MikuLogger v10.2.39 - CV01 Edition",
            RgbColor.MikuCyan,
            RgbColor.MikuPink,
            30  // Delay between each character in milliseconds
        );
        MikuConsole.WriteLine();

        await Task.Delay(1500); // Pause to let user appreciate the intro
    }

    #endregion

    #region Standard Colors

    /// <summary>
    /// Demonstrates the standard 16-color console palette and MikuLogger's Console color mode.
    /// <para>
    /// This section shows:
    /// <list type="bullet">
    ///   <item><description>All 16 standard System.ConsoleColor values</description></item>
    ///   <item><description>MikuLogger output using ColorSpace.Console mode</description></item>
    /// </list>
    /// </para>
    /// <remarks>
    /// The 16-color mode is the most compatible option, working on virtually all terminals
    /// including older Windows Command Prompt versions.
    /// </remarks>
    /// </summary>
    private static async Task ShowStandardColors()
    {
        MikuConsole.Clear();
        await ShowSectionHeader("STANDARD CONSOLE COLORS (16 COLORS)");

        MikuConsole.WriteLine("\n  Standard 16 Console Colors:\n", RgbColor.White);

        // Unicode full block character used to display color swatches
        const char fullBlock = '\u2588'; // █
        
        // Get all values from the System.ConsoleColor enum (0-15)
        var consoleColors = Enum.GetValues<System.ConsoleColor>();

        // Display each color as a block with its name
        foreach (var color in consoleColors)
        {
            // Use System.Console for standard color output
            System.Console.ForegroundColor = color;
            MikuConsole.Write($"  {new string(fullBlock, 6)} {color,-15}");
            System.Console.ResetColor();

            // Create a 4-column layout (new line every 4 colors)
            if ((int)color % 4 == 3)
                MikuConsole.WriteLine();

            await Task.Delay(100); // Staggered animation effect
        }

        // Reset to default colors before continuing
        MikuConsole.ResetConsoleColors();
        MikuConsole.WriteLine();
        MikuConsole.WriteLine();

        // Demonstrate MikuLogger with Console color mode
        MikuConsole.WriteLine("  MikuLogger Standard Color Mode:", RgbColor.White);
        MikuConsole.WriteLine();

        // Configure logger to use standard 16-color output
        var options = new MikuLoggerOptions
        {
            Output = MikuLogOutput.Console,
            MinimumLogLevel = MikuLogLevel.Trace,
            ConsoleColors = new MikuConsoleColorOptions
            {
                ColorSpace = MikuColorSpace.Console,
                Enabled = true
            }
        };

        using var logger = new MikuLogger("ColorDemo", options);

        // Demonstrate each log level with appropriate colors
        logger.LogTrace("This is a TRACE message");
        await Task.Delay(200);
        logger.LogDebug("This is a DEBUG message");
        await Task.Delay(200);
        logger.LogInformation("This is an INFO message");
        await Task.Delay(200);
        logger.LogWarning("This is a WARNING message");
        await Task.Delay(200);
        logger.LogError("This is an ERROR message");
        await Task.Delay(200);
        logger.LogCritical("This is a CRITICAL message");

        await Task.Delay(2000); // Pause before next section
    }

    #endregion

    #region Extended 256 Colors

    /// <summary>
    /// Demonstrates the extended 256-color palette available in modern terminals.
    /// <para>
    /// The 256-color palette consists of:
    /// <list type="bullet">
    ///   <item><description>Colors 0-7: Standard colors (same as 16-color mode)</description></item>
    ///   <item><description>Colors 8-15: High-intensity (bright) colors</description></item>
    ///   <item><description>Colors 16-231: 6×6×6 RGB color cube (216 colors)</description></item>
    ///   <item><description>Colors 232-255: Grayscale ramp (24 shades)</description></item>
    /// </list>
    /// </para>
    /// </summary>
    private static async Task ShowExtended256Colors()
    {
        MikuConsole.Clear();
        await ShowSectionHeader("EXTENDED 256 COLOR PALETTE");

        MikuConsole.WriteLine("\n  256-Color Palette Overview:\n", RgbColor.White);

        const char fullBlock = '\u2588'; // █

        // Display standard colors (0-7)
        // These are the basic 8 colors available in all terminals
        MikuConsole.WriteLine("  Standard Colors (0-7):", RgbColor.White);
        MikuConsole.Write("  ");
        for (int i = 0; i < 8; i++)
        {
            // Write256 outputs text using ANSI 256-color escape sequences
            MikuConsole.Write256(new string(fullBlock, 2), (byte)i);
            await Task.Delay(20);
        }
        MikuConsole.WriteLine();

        // Display high-intensity colors (8-15)
        // These are brighter versions of the standard 8 colors
        MikuConsole.WriteLine("  High-Intensity Colors (8-15):", RgbColor.White);
        MikuConsole.Write("  ");
        for (int i = 8; i < 16; i++)
        {
            MikuConsole.Write256(new string(fullBlock, 2), (byte)i);
            await Task.Delay(20);
        }
        MikuConsole.WriteLine();

        MikuConsole.WriteLine("\n  216 Color Cube (6x6x6):\n", RgbColor.White);

        // Display the 6×6×6 RGB color cube (colors 16-231)
        // The formula for color index is: 16 + (36 × r) + (6 × g) + b
        // where r, g, b are values from 0-5
        for (int g = 0; g < 6; g++)  // Green varies by row
        {
            MikuConsole.Write("  ");
            for (int r = 0; r < 6; r++)  // Red varies by column group
            {
                for (int b = 0; b < 6; b++)  // Blue varies within each group
                {
                    int colorIndex = 16 + (r * 36) + (g * 6) + b;
                    MikuConsole.Write256(fullBlock.ToString(), (byte)colorIndex);
                }
                MikuConsole.Write(" ");  // Space between red groups
            }
            MikuConsole.WriteLine();
            await Task.Delay(50);
        }

        // Display grayscale ramp (colors 232-255)
        // 24 shades of gray from dark to light
        MikuConsole.WriteLine("\n  Grayscale (232-255):", RgbColor.White);
        MikuConsole.Write("  ");
        for (int i = 232; i < 256; i++)
        {
            MikuConsole.Write256(fullBlock.ToString(), (byte)i);
            await Task.Delay(20);
        }
        MikuConsole.WriteLine();

        // Demonstrate MikuLogger with Extended256 color mode
        MikuConsole.WriteLine("\n  MikuLogger Extended256 Mode:\n", RgbColor.White);

        var options = new MikuLoggerOptions
        {
            Output = MikuLogOutput.Console,
            MinimumLogLevel = MikuLogLevel.Trace,
            ConsoleColors = new MikuConsoleColorOptions
            {
                ColorSpace = MikuColorSpace.Extended256,
                Extended256Colors = new MikuExtended256ColorOptions
                {
                    TraceColor = 245,       // Light gray
                    DebugColor = 226,       // Yellow
                    InformationColor = 44,  // Cyan (close to Miku Cyan)
                    WarningColor = 208,     // Orange
                    ErrorColor = 196,       // Red
                    CriticalColor = 160     // Dark red
                }
            }
        };

        using var logger = new MikuLogger("Extended256", options);

        logger.LogTrace("Extended256 TRACE message");
        await Task.Delay(200);
        logger.LogDebug("Extended256 DEBUG message");
        await Task.Delay(200);
        logger.LogInformation("Extended256 INFO message - Miku Cyan!");
        await Task.Delay(200);
        logger.LogWarning("Extended256 WARNING message");
        await Task.Delay(200);
        logger.LogError("Extended256 ERROR message");
        await Task.Delay(200);
        logger.LogCritical("Extended256 CRITICAL message");

        await Task.Delay(2000);
    }

    #endregion

    #region TrueColor Demo

    /// <summary>
    /// Demonstrates TrueColor (24-bit RGB) output with 16 million possible colors.
    /// <para>
    /// This section showcases:
    /// <list type="bullet">
    ///   <item><description>Individual RGB channel gradients (red, green, blue)</description></item>
    ///   <item><description>Miku-themed color gradients using RgbColor.Lerp</description></item>
    ///   <item><description>MikuLogger with custom TrueColor configuration</description></item>
    /// </list>
    /// </para>
    /// <remarks>
    /// TrueColor requires a modern terminal emulator. Windows Terminal, VS Code terminal,
    /// and most Linux terminals support it. Legacy Windows Command Prompt does not.
    /// </remarks>
    /// </summary>
    private static async Task ShowTrueColorDemo()
    {
        MikuConsole.Clear();
        await ShowSectionHeader("TRUECOLOR (24-bit RGB) - 16 MILLION COLORS");

        MikuConsole.WriteLine("\n  RGB Color Space Demo:\n", RgbColor.White);

        const char fullBlock = '\u2588'; // █

        // Red channel gradient: R varies 0-255, G=0, B=0
        MikuConsole.Write("  R: ", RgbColor.White);
        for (int r = 0; r <= 255; r += 5)
        {
            MikuConsole.Write(fullBlock.ToString(), new RgbColor((byte)r, 0, 0));
        }
        MikuConsole.WriteLine();

        // Green channel gradient: R=0, G varies 0-255, B=0
        MikuConsole.Write("  G: ", RgbColor.White);
        for (int g = 0; g <= 255; g += 5)
        {
            MikuConsole.Write(fullBlock.ToString(), new RgbColor(0, (byte)g, 0));
        }
        MikuConsole.WriteLine();

        // Blue channel gradient: R=0, G=0, B varies 0-255
        MikuConsole.Write("  B: ", RgbColor.White);
        for (int b = 0; b <= 255; b += 5)
        {
            MikuConsole.Write(fullBlock.ToString(), new RgbColor(0, 0, (byte)b));
        }
        MikuConsole.WriteLine();

        await Task.Delay(500);

        // Demonstrate Miku-themed color gradients
        MikuConsole.WriteLine("\n  Miku Color Gradients:\n", RgbColor.White);

        // Gradient from Miku Cyan (#00CED1) to Miku Pink (#E12885)
        MikuConsole.Write("  Cyan -> Pink: ", RgbColor.White);
        MikuConsole.DrawGradientBar(50, RgbColor.MikuCyan, RgbColor.MikuPink);
        MikuConsole.WriteLine();

        // Gradient from Miku Teal (#39C5BB) to Miku Cyan
        MikuConsole.Write("  Teal -> Cyan: ", RgbColor.White);
        MikuConsole.DrawGradientBar(50, RgbColor.MikuTeal, RgbColor.MikuCyan);
        MikuConsole.WriteLine();

        // Gradient from Miku Pink to Miku Dark Cyan (#008B8B)
        MikuConsole.Write("  Pink -> Dark: ", RgbColor.White);
        MikuConsole.DrawGradientBar(50, RgbColor.MikuPink, RgbColor.MikuDarkCyan);
        MikuConsole.WriteLine();

        await Task.Delay(500);

        // Demonstrate MikuLogger with TrueColor mode
        MikuConsole.WriteLine("\n  MikuLogger TrueColor Mode:\n", RgbColor.White);

        var options = new MikuLoggerOptions
        {
            Output = MikuLogOutput.Console,
            MinimumLogLevel = MikuLogLevel.Trace,
            ConsoleColors = new MikuConsoleColorOptions
            {
                ColorSpace = MikuColorSpace.TrueColor,
                TrueColors = new MikuTrueColorOptions
                {
                    TraceColor = RgbColor.Gray,
                    DebugColor = RgbColor.Yellow,
                    InformationColor = RgbColor.MikuCyan,
                    WarningColor = RgbColor.Orange,
                    ErrorColor = RgbColor.FromHex("#E12885"),
                    CriticalColor = RgbColor.DarkRed
                }
            }
        };

        using var logger = new MikuLogger("TrueColor", options);

        logger.LogTrace("TrueColor TRACE - Gray");
        await Task.Delay(200);
        logger.LogDebug("TrueColor DEBUG - Yellow");
        await Task.Delay(200);
        logger.LogInformation("TrueColor INFO - Miku Cyan #00CED1");
        await Task.Delay(200);
        logger.LogWarning("TrueColor WARNING - Orange");
        await Task.Delay(200);
        logger.LogError("TrueColor ERROR - Miku Pink #E12885");
        await Task.Delay(200);
        logger.LogCritical("TrueColor CRITICAL - Dark Red");

        await Task.Delay(2000);
    }

    #endregion

    #region Miku Theme

    /// <summary>
    /// Showcases the Hatsune Miku color theme with animated color cycling.
    /// <para>
    /// Features:
    /// <list type="bullet">
    ///   <item><description>Animated ASCII face with rainbow color cycling</description></item>
    ///   <item><description>Complete Miku color palette with hex values</description></item>
    ///   <item><description>ColorHelper.GetMikuRainbow for smooth color transitions</description></item>
    /// </list>
    /// </para>
    /// </summary>
    private static async Task ShowMikuTheme()
    {
        MikuConsole.Clear();
        await ShowSectionHeader("HATSUNE MIKU COLOR THEME");

        // Simple ASCII art representation of a face
        var mikuFace = new[]
        {
            @"            ████████            ",
            @"         ███        ███         ",
            @"       ██    ██  ██    ██       ",
            @"      ██    ████████    ██      ",
            @"     ██                  ██     ",
            @"     ██   ██      ██   ██       ",
            @"     ██                  ██     ",
            @"      ██    ████████    ██      ",
            @"       ██            ██         ",
            @"         ███      ███           ",
            @"            ██████              "
        };

        MikuConsole.WriteLine();

        // Animate the face with rainbow colors cycling through Miku's palette
        for (int frame = 0; frame < 30; frame++)
        {
            MikuConsole.SetCursorPosition(0, 4);

            for (int i = 0; i < mikuFace.Length; i++)
            {
                // Calculate phase for color cycling
                // Each line has a slightly different phase for a wave effect
                double phase = (frame + i) * 0.3;
                
                // GetMikuRainbow returns colors that cycle through Miku's signature palette
                var color = ColorHelper.GetMikuRainbow(phase);
                MikuConsole.WriteLine("  " + mikuFace[i], color);
            }

            await Task.Delay(100); // ~10 FPS animation
        }

        MikuConsole.WriteLine();
        MikuConsole.WriteLine();

        // Display the complete Miku color palette
        MikuConsole.WriteLine("  ═══════════════════════════════════════════════", RgbColor.MikuCyan);
        MikuConsole.WriteLine("                 MIKU COLOR PALETTE", RgbColor.MikuPink);
        MikuConsole.WriteLine("  ═══════════════════════════════════════════════", RgbColor.MikuCyan);
        MikuConsole.WriteLine();

        // Define the official Miku color palette with hex codes
        var mikuColors = new (string name, RgbColor color)[]
        {
            ("Miku Cyan      #00CED1", RgbColor.MikuCyan),       // Primary hair/eyes color
            ("Miku Pink      #E12885", RgbColor.MikuPink),       // Accent color
            ("Miku Teal      #39C5BB", RgbColor.MikuTeal),       // Alternative cyan
            ("Miku Dark Cyan #008B8B", RgbColor.MikuDarkCyan),   // Shadow color
            ("Twintail Blue  #00BFFF", RgbColor.FromHex("#00BFFF")),  // Twintail highlights
            ("Headphone Gray #708090", RgbColor.FromHex("#708090")),  // Headphone color
            ("Tie Red        #DC143C", RgbColor.FromHex("#DC143C")),  // Tie accent
            ("Skirt Black    #1C1C1C", RgbColor.FromHex("#1C1C1C"))   // Skirt color
        };

        // Display each color with a color swatch and name
        foreach (var (name, color) in mikuColors)
        {
            MikuConsole.Write("  ");
            MikuConsole.DrawBar(8, MikuConsole.CursorTop, 8, color);  // Draw solid color bar
            MikuConsole.Write("  ");
            MikuConsole.WriteLine(name, color);  // Color name in matching color
            await Task.Delay(200);
        }

        await Task.Delay(2000);
    }

    #endregion

    #region Gradient Animations

    /// <summary>
    /// Demonstrates various animated color gradient effects.
    /// <para>
    /// Animations include:
    /// <list type="bullet">
    ///   <item><description>Horizontal rainbow wave using sine-based color cycling</description></item>
    ///   <item><description>Miku pulse effect radiating from center</description></item>
    ///   <item><description>Breathing effect with smooth color transitions</description></item>
    /// </list>
    /// </para>
    /// </summary>
    private static async Task ShowGradientAnimations()
    {
        MikuConsole.Clear();
        await ShowSectionHeader("COLOR GRADIENT ANIMATIONS");

        // Calculate width based on console window, max 80 characters
        int width = Math.Min(MikuConsole.WindowWidth - 4, 80);
        int animationFrames = 60;

        const char fullBlock = '\u2588'; // █

        MikuConsole.WriteLine("\n  Horizontal Rainbow Wave:\n", RgbColor.White);

        // Rainbow wave animation: colors cycle horizontally over time
        for (int frame = 0; frame < animationFrames; frame++)
        {
            MikuConsole.SetCursorPosition(2, 5);

            for (int x = 0; x < width; x++)
            {
                // Phase determines the color in the rainbow spectrum
                // Adding frame * 2 creates the wave motion
                double phase = (x + frame * 2) * 0.1;
                var color = ColorHelper.GetRainbow(phase);
                MikuConsole.Write(fullBlock.ToString(), color);
            }

            await Task.Delay(30); // ~33 FPS
        }

        MikuConsole.WriteLine("\n\n  Miku Pulse Effect:\n", RgbColor.White);

        // Pulse effect: colors pulse outward from the center
        for (int frame = 0; frame < animationFrames; frame++)
        {
            MikuConsole.SetCursorPosition(2, 9);

            for (int x = 0; x < width; x++)
            {
                // Calculate distance from center (normalized 0-1)
                double distance = Math.Abs(x - width / 2.0) / (width / 2.0);
                
                // Create pulsing wave that radiates from center
                // Subtracting distance * 5 makes the wave move outward
                double pulse = Math.Sin(frame * 0.2 - distance * 5);
                
                // Normalize pulse from -1..1 to 0..1 for color interpolation
                pulse = (pulse + 1) / 2;

                // Interpolate between Miku Cyan and Pink based on pulse value
                var color = RgbColor.Lerp(RgbColor.MikuCyan, RgbColor.MikuPink, pulse);
                MikuConsole.Write(fullBlock.ToString(), color);
            }

            await Task.Delay(30);
        }

        MikuConsole.WriteLine("\n\n  Breathing Effect:\n", RgbColor.White);

        // Breathing animation: smooth fade between dark and bright cyan
        // Uses MikuConsoleAnimation helper for consistent timing
        await MikuConsoleAnimation.BreathingAsync(
            new string(fullBlock, width),
            RgbColor.MikuDarkCyan,  // Exhale color (dark)
            RgbColor.MikuCyan,      // Inhale color (bright)
            2, 13,                   // Position (x=2, y=13)
            breathCount: 2,          // Number of breath cycles
            breathDurationMs: 3000   // Duration of each breath
        );

        await Task.Delay(1000);
    }

    #endregion

    #region Logger Demo

    /// <summary>
    /// Simulates a realistic application lifecycle with MikuLogger output.
    /// <para>
    /// Demonstrates logging patterns commonly seen in real applications:
    /// <list type="bullet">
    ///   <item><description>Application startup and configuration logging</description></item>
    ///   <item><description>Database and service connection events</description></item>
    ///   <item><description>Warning and error handling scenarios</description></item>
    ///   <item><description>Graceful shutdown sequence</description></item>
    /// </list>
    /// </para>
    /// </summary>
    private static async Task ShowLoggerDemo()
    {
        MikuConsole.Clear();
        await ShowSectionHeader("MIKULOGGER IN ACTION");

        MikuConsole.WriteLine("\n  Simulating Application Lifecycle:\n", RgbColor.White);

        // Configure logger with custom TrueColor theme
        var options = new MikuLoggerOptions
        {
            Output = MikuLogOutput.Console,
            MinimumLogLevel = MikuLogLevel.Trace,
            ConsoleColors = new MikuConsoleColorOptions
            {
                ColorSpace = MikuColorSpace.TrueColor,
                TrueColors = new MikuTrueColorOptions
                {
                    TraceColor = RgbColor.FromHex("#6B7280"),
                    DebugColor = RgbColor.FromHex("#FBBF24"),
                    InformationColor = RgbColor.MikuCyan,
                    WarningColor = RgbColor.FromHex("#F97316"),
                    ErrorColor = RgbColor.FromHex("#E12885"),
                    CriticalColor = RgbColor.FromHex("#7F1D1D")
                }
            }
        };

        using var logger = new MikuLogger("MikuApp", options);

        var logMessages = new (MikuLogLevel level, string message, int delay)[]
        {
            (MikuLogLevel.Information, "Application starting...", 300),
            (MikuLogLevel.Debug, "Loading configuration from appsettings.json", 200),
            (MikuLogLevel.Trace, "Configuration values: Environment=Production, Port=5000", 150),
            (MikuLogLevel.Information, "Database connection established", 400),
            (MikuLogLevel.Debug, "Initializing Miku Voice Engine v39.0", 200),
            (MikuLogLevel.Information, "CV01 Module loaded successfully", 300),
            
            (MikuLogLevel.Warning, "Memory usage approaching 80%", 500),
            (MikuLogLevel.Debug, "Running garbage collection...", 200),
            (MikuLogLevel.Information, "Memory optimized: 45% usage", 300),
            (MikuLogLevel.Information, "HTTP Server listening on port 5000", 300),
            (MikuLogLevel.Trace, "Request received: GET /api/songs", 100),
            (MikuLogLevel.Information, "Request processed in 23ms", 200),
            (MikuLogLevel.Warning, "Rate limit approaching for client 192.168.1.39", 400),
            
            (MikuLogLevel.Error, "Failed to connect to external API: Timeout after 30s", 600),
            (MikuLogLevel.Information, "Retrying connection (attempt 2/3)...", 300),
            (MikuLogLevel.Information, "Connection restored successfully", 300),
            (MikuLogLevel.Critical, "CRITICAL: Unhandled exception in voice synthesis!", 800),
            (MikuLogLevel.Information, "Exception handled, service recovered", 400),
            
            (MikuLogLevel.Information, "Graceful shutdown initiated...", 300),
            (MikuLogLevel.Information, "All 39 connections closed", 200),
            (MikuLogLevel.Information, "Application stopped. Goodbye!", 300)
        };

        foreach (var (level, message, delay) in logMessages)
        {
            switch (level)
            {
                case MikuLogLevel.Trace:
                    logger.LogTrace(message);
                    break;
                case MikuLogLevel.Debug:
                    logger.LogDebug(message);
                    break;
                case MikuLogLevel.Information:
                    logger.LogInformation(message);
                    break;
                case MikuLogLevel.Warning:
                    logger.LogWarning(message);
                    break;
                case MikuLogLevel.Error:
                    logger.LogError(message);
                    break;
                case MikuLogLevel.Critical:
                    logger.LogCritical(message);
                    break;
            }
            await Task.Delay(delay);
        }

        await Task.Delay(1500);
    }

    #endregion

    #region Rainbow Wave

    /// <summary>
    /// Displays an animated rainbow wave pattern using mathematical sine functions.
    /// <para>
    /// The animation creates a flowing wave effect by:
    /// <list type="bullet">
    ///   <item><description>Using sine waves to determine character intensity</description></item>
    ///   <item><description>Applying rainbow colors based on position and time</description></item>
    ///   <item><description>Using different block characters (█ and ▓) for depth</description></item>
    /// </list>
    /// </para>
    /// </summary>
    private static async Task ShowRainbowWave()
    {
        MikuConsole.Clear();
        await ShowSectionHeader("RAINBOW WAVE ANIMATION");

        int width = Math.Min(MikuConsole.WindowWidth - 4, 80);
        int height = 15;

        const char fullBlock = '\u2588';  // █ - Full intensity
        const char darkShade = '\u2593';  // ▓ - Medium intensity

        MikuConsole.WriteLine();

        // Animate for 120 frames (~4 seconds at 30 FPS)
        for (int frame = 0; frame < 120; frame++)
        {
            MikuConsole.SetCursorPosition(0, 4);

            for (int y = 0; y < height; y++)
            {
                MikuConsole.Write("  ");  // Left margin
                
                for (int x = 0; x < width; x++)
                {
                    // Calculate wave value using sine function
                    // x + frame creates horizontal movement
                    // y * 0.5 creates vertical phase offset for wave shape
                    double wave = Math.Sin((x + frame) * 0.2 + y * 0.5);
                    
                    // Calculate rainbow color phase
                    double phase = (x + y + frame) * 0.1;

                    // Use different characters based on wave intensity
                    if (wave > 0.3)
                    {
                        // Full intensity - wave peak
                        var color = ColorHelper.GetRainbow(phase);
                        MikuConsole.Write(fullBlock.ToString(), color);
                    }
                    else if (wave > -0.3)
                    {
                        // Medium intensity - wave slope
                        var color = ColorHelper.GetRainbow(phase);
                        color = ColorHelper.Darken(color, 0.5);  // Darken by 50%
                        MikuConsole.Write(darkShade.ToString(), color);
                    }
                    else
                    {
                        // Below threshold - wave trough (empty space)
                        MikuConsole.Write(" ");
                    }
                }
                MikuConsole.WriteLine();
            }

            await Task.Delay(30); // ~33 FPS
        }

        await Task.Delay(500);
    }

    #endregion

    #region Matrix Rain

    /// <summary>
    /// Creates a Matrix-style digital rain effect using Miku-themed colors.
    /// <para>
    /// Features:
    /// <list type="bullet">
    ///   <item><description>Japanese katakana and Miku-related characters</description></item>
    ///   <item><description>Variable drop speeds for depth illusion</description></item>
    ///   <item><description>Fading trail effect behind each drop</description></item>
    ///   <item><description>Random pink highlights for accent</description></item>
    /// </list>
    /// </para>
    /// <remarks>
    /// The seed value 39 is a reference to Miku's character number (CV01 = Character Voice 01,
    /// and 39 can be read as "mi-ku" in Japanese).
    /// </remarks>
    /// </summary>
    private static async Task ShowMikuMatrixRain()
    {
        MikuConsole.Clear();

        // Display header
        MikuConsole.WriteCenteredLine("═══════════════════════════════════════════════", RgbColor.MikuCyan);
        MikuConsole.WriteCenteredLine("          MIKU MATRIX RAIN EFFECT              ", RgbColor.MikuPink);
        MikuConsole.WriteCenteredLine("═══════════════════════════════════════════════", RgbColor.MikuCyan);

        int width = Math.Min(MikuConsole.WindowWidth, 100);
        int height = MikuConsole.WindowHeight - 6;
        int startY = 4;  // Start below the header

        // Arrays to track each column's drop position and speed
        var drops = new int[width];
        var speeds = new int[width];
        
        // Character set: Japanese katakana + "MIKU" + numbers (Miku theme!)
        var chars = "ミクMIKU01アイウエオカキクケコサシスセソタチツテト39".ToCharArray();
        
        // Use seed 39 (み=3, く=9 → Miku) for reproducible randomness
        var random = new Random(39);

        // Initialize drops at random positions above the visible area
        for (int i = 0; i < width; i++)
        {
            drops[i] = random.Next(-height, 0);  // Start above screen
            speeds[i] = random.Next(1, 3);       // Speed 1-2
        }

        // Run animation for 200 frames (~10 seconds)
        for (int frame = 0; frame < 200; frame++)
        {
            for (int x = 0; x < width; x++)
            {
                // Only process if drop is in visible range
                if (drops[x] >= 0 && drops[x] < height)
                {
                    int y = startY + drops[x];
                    
                    // Bounds checking
                    if (y >= startY && y < MikuConsole.WindowHeight - 1)
                    {
                        // Draw the leading character (brightest)
                        MikuConsole.WriteAt(x, y, chars[random.Next(chars.Length)].ToString(), RgbColor.MikuCyan);

                        // Draw the trailing characters with fading effect
                        for (int trail = 1; trail < 8 && drops[x] - trail >= 0; trail++)
                        {
                            int trailY = startY + drops[x] - trail;
                            
                            if (trailY >= startY && trailY < MikuConsole.WindowHeight - 1)
                            {
                                // Calculate fade: 1.0 at trail=1, 0.0 at trail=8
                                double fade = 1.0 - (trail / 8.0);
                                var trailColor = RgbColor.Lerp(RgbColor.MikuDarkCyan, RgbColor.MikuCyan, fade);

                                // Random pink highlights (30% chance when random > 7)
                                if (random.Next(10) > 7)
                                    trailColor = RgbColor.Lerp(trailColor, RgbColor.MikuPink, 0.3);

                                MikuConsole.WriteAt(x, trailY, chars[random.Next(chars.Length)].ToString(), trailColor);
                            }
                        }

                        // Clear the character at the end of the trail
                        int clearY = startY + drops[x] - 8;
                        if (clearY >= startY && clearY < MikuConsole.WindowHeight - 1)
                        {
                            MikuConsole.WriteAt(x, clearY, " ");
                        }
                    }
                }

                // Move drop down by its speed
                drops[x] += speeds[x];

                // Reset drop when it goes off screen
                if (drops[x] > height + 10)
                {
                    drops[x] = random.Next(-10, 0);  // Reset above screen
                    speeds[x] = random.Next(1, 3);   // New random speed
                }
            }

            await Task.Delay(50); // ~20 FPS for rain effect
        }

        await Task.Delay(500);
    }

    #endregion

    #region Outro

    /// <summary>
    /// Displays the closing credits with animated reveal effect.
    /// <para>
    /// Shows version information, credits, and waits for user input to exit.
    /// </para>
    /// </summary>
    private static async Task ShowOutro()
    {
        MikuConsole.Clear();

        // ASCII art credit box with version information
        var outroArt = new[]
        {
            @"",
            @"     ╔════════════════════════════════════════════════════════╗",
            @"     ║                                                        ║",
            @"     ║                Thank you for watching!                 ║",
            @"     ║                                                        ║",
            @"     ║       MikuConsole v10.0.39 + MikuLogger v10.2.39       ║",
            @"     ║             Character Vocal Series 01                  ║",
            @"     ║                                                        ║",
            @"     ║   'Paint your terminal & tell your logs to the world!' ║",
            @"     ║                                                        ║",
            @"     ║               github.com/DjNemas/MikuLib               ║",
            @"     ║                                                        ║",
            @"     ╚════════════════════════════════════════════════════════╝",
            @""
        };

        // Reveal lines with alternating colors for visual interest
        await MikuConsoleAnimation.RevealLinesAlternatingAsync(
            outroArt,
            RgbColor.MikuCyan,
            RgbColor.MikuPink,
            0, 0,    // Start position
            100      // Delay between lines in milliseconds
        );

        MikuConsole.WriteLine();

        // Draw decorative gradient bar
        MikuConsole.Write("     ");
        MikuConsole.DrawGradientBar(30, RgbColor.MikuCyan, RgbColor.MikuPink);
        MikuConsole.DrawGradientBar(30, RgbColor.MikuPink, RgbColor.MikuCyan);
        MikuConsole.WriteLine();

        MikuConsole.WriteLine();
        MikuConsole.WriteCenteredLine("Press any key to exit...", RgbColor.Gray);

        // Wait for user input before exiting
        System.Console.ReadKey(true);
    }

    #endregion

    #region Helper Methods

    /// <summary>
    /// Displays a styled section header with decorative box borders.
    /// </summary>
    /// <param name="title">The title text to display in the header.</param>
    /// <remarks>
    /// The header uses Unicode box-drawing characters (╔, ═, ╗, ║, ╚, ╝)
    /// to create a professional-looking section divider.
    /// </remarks>
    private static async Task ShowSectionHeader(string title)
    {
        MikuConsole.WriteLine();
        
        // Top border: ╔═══════╗
        MikuConsole.WriteLine($"  ╔{'═'.ToString().PadRight(title.Length + 4, '═')}╗", RgbColor.MikuCyan);
        
        // Title line: ║  TITLE  ║
        MikuConsole.WriteLine($"  ║  {title}  ║", RgbColor.MikuPink);
        
        // Bottom border: ╚═══════╝
        MikuConsole.WriteLine($"  ╚{'═'.ToString().PadRight(title.Length + 4, '═')}╝", RgbColor.MikuCyan);
        
        await Task.Delay(300);  // Brief pause after header
    }

    #endregion
}
