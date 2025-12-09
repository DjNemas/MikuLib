# MikuLib.Console Examples

Practical examples for using MikuLib.Console.

> "Like performing at Magical Mirai!" - Hatsune Nemas

## Table of Contents

1. [Basic Usage](#basic-usage)
2. [Colored Output](#colored-output)
3. [Styled Text](#styled-text)
4. [Gradients and Rainbow](#gradients-and-rainbow)
5. [Positioning and Layout](#positioning-and-layout)
6. [Drawing Elements](#drawing-elements)
7. [Animations](#animations)
8. [Loading Indicators](#loading-indicators)
9. [Complete Application](#complete-application)

---

## Basic Usage

### Initialize and Setup

```csharp
using Miku.Core;
using MikuLib.Console;

// Always initialize first for UTF-8 support
MikuConsole.Initialize();

// Hide cursor for cleaner animations
MikuConsole.HideCursor();

try
{
    // Your code here
    MikuConsole.WriteLine("Hello Miku!", RgbColor.MikuCyan);
}
finally
{
    // Always restore cursor and reset colors
    MikuConsole.ShowCursor();
    MikuConsole.ResetColors();
}
```

### Basic Write Methods

```csharp
// Write without newline
MikuConsole.Write("Hello ");
MikuConsole.Write("World!");
MikuConsole.WriteLine();  // Empty line

// Write with newline
MikuConsole.WriteLine("This is a new line");

// Plain text (no color)
MikuConsole.Write("Plain text");
MikuConsole.WriteLine("Plain line");
```

---

## Colored Output

### TrueColor (24-bit RGB)

```csharp
// Using predefined Miku colors
MikuConsole.WriteLine("Miku Cyan!", RgbColor.MikuCyan);
MikuConsole.WriteLine("Miku Pink!", RgbColor.MikuPink);
MikuConsole.WriteLine("Miku Teal!", RgbColor.MikuTeal);
MikuConsole.WriteLine("Miku Dark Cyan!", RgbColor.MikuDarkCyan);

// Using standard colors
MikuConsole.WriteLine("Red text", RgbColor.Red);
MikuConsole.WriteLine("Green text", RgbColor.Green);
MikuConsole.WriteLine("Blue text", RgbColor.Blue);

// Custom RGB colors
var customColor = new RgbColor(255, 128, 64);
MikuConsole.WriteLine("Custom orange", customColor);

// From hex string
var hexColor = RgbColor.FromHex("#FF6B9D");
MikuConsole.WriteLine("From hex", hexColor);

// With background color
MikuConsole.WriteLine("Foreground and Background", RgbColor.White, RgbColor.MikuCyan);
```

### 256-Color Palette

```csharp
// Standard colors (0-15)
MikuConsole.WriteLine256("Color 0 (Black)", 0);
MikuConsole.WriteLine256("Color 1 (Red)", 1);
MikuConsole.WriteLine256("Color 44 (Cyan-ish)", 44);

// With background
MikuConsole.Write256("With background", 15, 4);  // White on red

// Show all 256 colors
for (int i = 0; i < 256; i++)
{
    MikuConsole.Write256("?", (byte)i);
    if (i % 16 == 15) MikuConsole.WriteLine();
}
```

---

## Styled Text

### Bold, Italic, Underline

```csharp
MikuConsole.WriteBold("Bold text", RgbColor.MikuCyan);
MikuConsole.WriteLine();

MikuConsole.WriteItalic("Italic text", RgbColor.MikuPink);
MikuConsole.WriteLine();

MikuConsole.WriteUnderline("Underlined text", RgbColor.MikuTeal);
MikuConsole.WriteLine();
```

### Combined Styles

```csharp
using Miku.Core;

// Combine multiple ANSI styles
MikuConsole.WriteStyled(
    "Bold and Underlined", 
    RgbColor.MikuCyan, 
    AnsiCodes.Bold, 
    AnsiCodes.Underline
);
```

---

## Gradients and Rainbow

### Horizontal Gradient

```csharp
// Text with gradient
MikuConsole.WriteGradient("Gradient from cyan to pink", RgbColor.MikuCyan, RgbColor.MikuPink);
MikuConsole.WriteLine();

// With newline
MikuConsole.WriteGradientLine("Gradient line", RgbColor.Blue, RgbColor.Red);
```

### Rainbow Text

```csharp
// Standard rainbow
MikuConsole.WriteRainbow("Rainbow colored text!");
MikuConsole.WriteLine();

// With phase offset (shifts colors)
MikuConsole.WriteRainbowLine("Shifted rainbow", phaseOffset: 2.0);

// Miku-themed rainbow (cyan/pink biased)
MikuConsole.WriteMikuRainbow("Miku rainbow colors!");
MikuConsole.WriteLine();

MikuConsole.WriteMikuRainbowLine("Miku rainbow with newline");
```

### Animated Rainbow

```csharp
for (int frame = 0; frame < 100; frame++)
{
    MikuConsole.SetCursorPosition(0, 0);
    MikuConsole.WriteRainbow("Animated Rainbow!", phaseOffset: frame * 0.2);
    await Task.Delay(50);
}
```

---

## Positioning and Layout

### Cursor Positioning

```csharp
// Set cursor position
MikuConsole.SetCursorPosition(10, 5);
MikuConsole.Write("At position (10, 5)", RgbColor.MikuCyan);

// Get current position
int x = MikuConsole.CursorLeft;
int y = MikuConsole.CursorTop;

// Write at specific position
MikuConsole.WriteAt(0, 0, "Top-left corner", RgbColor.MikuCyan);
MikuConsole.WriteAt(20, 10, "Middle area", RgbColor.MikuPink);
```

### Centered Text

```csharp
// Center on current line
MikuConsole.WriteCentered("Centered Text", RgbColor.MikuCyan);
MikuConsole.WriteLine();

// Center with newline
MikuConsole.WriteCenteredLine("Centered Line", RgbColor.MikuPink);
```

### Window Dimensions

```csharp
int width = MikuConsole.WindowWidth;
int height = MikuConsole.WindowHeight;

MikuConsole.WriteLine($"Console size: {width}x{height}", RgbColor.White);

// Responsive width
int barWidth = Math.Min(width - 4, 80);
MikuConsole.DrawBar(barWidth, RgbColor.MikuCyan);
```

---

## Drawing Elements

### Solid Bars

```csharp
// Simple bar
MikuConsole.DrawBar(20, RgbColor.MikuCyan);
MikuConsole.WriteLine();

// Custom character
MikuConsole.DrawBar(20, RgbColor.MikuPink, '?');
MikuConsole.WriteLine();

// Different lengths
MikuConsole.DrawBar(10, RgbColor.Red);
MikuConsole.DrawBar(20, RgbColor.Yellow);
MikuConsole.DrawBar(10, RgbColor.Green);
```

### Gradient Bars

```csharp
// Cyan to pink gradient
MikuConsole.DrawGradientBar(50, RgbColor.MikuCyan, RgbColor.MikuPink);
MikuConsole.WriteLine();

// Custom character
MikuConsole.DrawGradientBar(50, RgbColor.Blue, RgbColor.Red, '?');
MikuConsole.WriteLine();

// Progress bar style
MikuConsole.Write("[");
MikuConsole.DrawGradientBar(40, RgbColor.MikuDarkCyan, RgbColor.MikuCyan);
MikuConsole.Write("] 100%", RgbColor.White);
```

### Boxes

```csharp
// Simple box
MikuConsole.DrawBox(5, 2, 40, 10, RgbColor.MikuCyan);

// Box with content
MikuConsole.DrawBox(0, 0, 50, 8, RgbColor.MikuCyan);
MikuConsole.WriteAt(2, 2, "Title", RgbColor.MikuPink);
MikuConsole.WriteAt(2, 4, "Content goes here", RgbColor.White);
MikuConsole.WriteAt(2, 5, "More content", RgbColor.Gray);
```

---

## Animations

### Typewriter Effect

```csharp
// Simple typewriter
await MikuConsoleAnimation.TypewriterAsync(
    "Typing this text character by character...",
    RgbColor.MikuCyan,
    delayMs: 30
);

// With newline
await MikuConsoleAnimation.TypewriterLineAsync(
    "This ends with a newline",
    RgbColor.MikuPink,
    delayMs: 50
);

// Gradient typewriter
await MikuConsoleAnimation.TypewriterGradientAsync(
    "Gradient while typing!",
    RgbColor.MikuCyan,
    RgbColor.MikuPink,
    delayMs: 40
);
```

### Fade Effects

```csharp
// Fade in from black
await MikuConsoleAnimation.FadeInAsync(
    "Fading in...",
    RgbColor.MikuCyan,
    x: 10, y: 5,
    durationMs: 1000
);

// Fade out to black
await MikuConsoleAnimation.FadeOutAsync(
    "Fading out...",
    RgbColor.MikuCyan,
    x: 10, y: 7,
    durationMs: 1000
);
```

### Pulse and Breathing

```csharp
// Pulse between two colors
await MikuConsoleAnimation.PulseAsync(
    "PULSING!",
    RgbColor.MikuCyan,
    RgbColor.MikuPink,
    x: 10, y: 5,
    pulseCount: 5,
    pulseDurationMs: 500
);

// Smooth breathing effect
await MikuConsoleAnimation.BreathingAsync(
    "Breathing...",
    RgbColor.MikuDarkCyan,
    RgbColor.MikuCyan,
    x: 10, y: 7,
    breathCount: 3,
    breathDurationMs: 2000
);
```

### Wave Effects

```csharp
// Color wave
await MikuConsoleAnimation.ColorWaveAsync(
    "Wave effect across text!",
    RgbColor.MikuCyan,
    RgbColor.MikuPink,
    x: 5, y: 5,
    waveDurationMs: 3000,
    waveCount: 2
);

// Rainbow wave
await MikuConsoleAnimation.RainbowWaveAsync(
    "Rainbow wave!",
    x: 5, y: 7,
    durationMs: 5000
);
```

### Screen Effects

```csharp
var lines = new[]
{
    "Line 1: Introduction",
    "Line 2: Main content",
    "Line 3: More details",
    "Line 4: Conclusion"
};

// Reveal lines one by one
await MikuConsoleAnimation.RevealLinesAsync(
    lines,
    RgbColor.MikuCyan,
    startX: 5, startY: 3,
    delayPerLineMs: 200
);

// Alternating colors
await MikuConsoleAnimation.RevealLinesAlternatingAsync(
    lines,
    RgbColor.MikuCyan,
    RgbColor.MikuPink,
    startX: 5, startY: 10,
    delayPerLineMs: 150
);
```

---

## Loading Indicators

### Spinner

```csharp
// Simple spinner
await MikuConsoleAnimation.SpinnerAsync(
    "Loading...",
    RgbColor.MikuCyan,
    x: 5, y: 5,
    durationMs: 3000
);
```

### Progress Bar

```csharp
// Animated progress bar
await MikuConsoleAnimation.ProgressBarAsync(
    x: 5, y: 5,
    width: 40,
    fillColor: RgbColor.MikuCyan,
    emptyColor: RgbColor.Gray,
    durationMs: 3000
);
```

### Custom Progress Bar

```csharp
// Manual progress update
for (int i = 0; i <= 100; i++)
{
    MikuConsole.SetCursorPosition(0, 5);
    
    int filled = i * 40 / 100;
    int empty = 40 - filled;
    
    MikuConsole.Write("[", RgbColor.White);
    MikuConsole.DrawBar(filled, RgbColor.MikuCyan);
    MikuConsole.DrawBar(empty, RgbColor.Gray, '?');
    MikuConsole.Write($"] {i}%", RgbColor.White);
    
    await Task.Delay(50);
}
```

---

## Complete Application

### Interactive Menu

```csharp
using Miku.Core;
using MikuLib.Console;

class Program
{
    static async Task Main()
    {
        MikuConsole.Initialize();
        MikuConsole.HideCursor();
        
        try
        {
            await ShowMenu();
        }
        finally
        {
            MikuConsole.ShowCursor();
            MikuConsole.ResetColors();
        }
    }
    
    static async Task ShowMenu()
    {
        MikuConsole.Clear();
        
        // Header
        MikuConsole.DrawBox(5, 2, 50, 3, RgbColor.MikuCyan);
        MikuConsole.WriteAt(7, 3, "MIKU APPLICATION", RgbColor.MikuPink);
        
        // Menu items
        var menuItems = new[] { "Option 1", "Option 2", "Option 3", "Exit" };
        int selected = 0;
        
        while (true)
        {
            // Draw menu
            for (int i = 0; i < menuItems.Length; i++)
            {
                MikuConsole.SetCursorPosition(10, 7 + i);
                
                if (i == selected)
                {
                    MikuConsole.Write("? ", RgbColor.MikuPink);
                    MikuConsole.WriteLine(menuItems[i], RgbColor.MikuCyan);
                }
                else
                {
                    MikuConsole.Write("  ", RgbColor.White);
                    MikuConsole.WriteLine(menuItems[i], RgbColor.Gray);
                }
            }
            
            // Handle input
            var key = System.Console.ReadKey(true);
            
            switch (key.Key)
            {
                case ConsoleKey.UpArrow:
                    selected = (selected - 1 + menuItems.Length) % menuItems.Length;
                    break;
                case ConsoleKey.DownArrow:
                    selected = (selected + 1) % menuItems.Length;
                    break;
                case ConsoleKey.Enter:
                    if (selected == menuItems.Length - 1) return;
                    await ShowAction(menuItems[selected]);
                    break;
            }
        }
    }
    
    static async Task ShowAction(string item)
    {
        MikuConsole.SetCursorPosition(10, 15);
        await MikuConsoleAnimation.TypewriterAsync(
            $"Selected: {item}",
            RgbColor.MikuCyan,
            30
        );
        await Task.Delay(1000);
        
        MikuConsole.SetCursorPosition(10, 15);
        MikuConsole.Write(new string(' ', 40));
    }
}
```

### Splash Screen

```csharp
static async Task ShowSplash()
{
    MikuConsole.Clear();
    
    var logo = new[]
    {
        @"  ????   ??????????  ??????   ???",
        @"  ????? ??????????? ???????   ???",
        @"  ????????????????????? ???   ???",
        @"  ????????????????????? ???   ???",
        @"  ??? ??? ?????????  ????????????",
        @"  ???     ?????????  ??? ??????? "
    };
    
    // Fade in logo
    for (int brightness = 0; brightness <= 255; brightness += 15)
    {
        MikuConsole.SetCursorPosition(0, 3);
        var color = RgbColor.Lerp(RgbColor.Black, RgbColor.MikuCyan, brightness / 255.0);
        
        foreach (var line in logo)
        {
            MikuConsole.WriteLine(line, color);
        }
        
        await Task.Delay(30);
    }
    
    // Subtitle with typewriter
    MikuConsole.SetCursorPosition(0, 11);
    await MikuConsoleAnimation.TypewriterGradientAsync(
        "        Version 10.0.39 - CV01 Edition",
        RgbColor.MikuCyan,
        RgbColor.MikuPink,
        40
    );
    
    // Loading bar
    MikuConsole.SetCursorPosition(10, 14);
    MikuConsole.Write("Loading: ", RgbColor.White);
    await MikuConsoleAnimation.ProgressBarAsync(
        19, 14, 30,
        RgbColor.MikuCyan,
        RgbColor.Gray,
        2000
    );
    
    await Task.Delay(500);
}
```

---

## Tips and Best Practices

### Always Use Try-Finally

```csharp
MikuConsole.HideCursor();
try
{
    // Your code
}
finally
{
    MikuConsole.ShowCursor();
    MikuConsole.ResetColors();
}
```

### Check Console Size

```csharp
int width = Math.Min(MikuConsole.WindowWidth - 2, 80);
// Use width for responsive layouts
```

### Use CancellationToken for Long Animations

```csharp
var cts = new CancellationTokenSource();

// Cancel after 5 seconds
cts.CancelAfter(5000);

try
{
    await MikuConsoleAnimation.RainbowWaveAsync(
        "Long animation...",
        0, 0,
        durationMs: 60000,
        cancellationToken: cts.Token
    );
}
catch (OperationCanceledException)
{
    // Animation was cancelled
}
```

---

*"Paint your terminal with the colors of the future!"*

**Version**: 10.0.39
