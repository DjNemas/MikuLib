# MikuLib.Console

Beautiful colored console output with TrueColor (24-bit RGB) and 256-color support.

> "Paint your terminal with Miku's colors!" - Hatsune Nemas

*Like the light shows at Miku's concerts since August 31st, 2007*

![MikuLib Console & Logger Demo](https://djnemas.de/SX/WindowsTerminal_4XPonwjrkx.gif)

## Features

- **MikuConsole** - Colored console output with multiple color modes
- **MikuConsoleAnimation** - Typewriter, fade, pulse, wave, and more effects
- **TrueColor Support** - Full 24-bit RGB (16 million colors)
- **256-Color Support** - Extended terminal color palette
- **Thread-Safe** - All operations are synchronized

## Installation

```bash
dotnet add package MikuLib.Console
```

Or add to your `.csproj`:

```xml
<PackageReference Include="MikuLib.Console" Version="10.0.39" />
```

## Quick Start

```csharp
using Miku.Core;
using Miku.Console;

// Initialize console
MikuConsole.Initialize();

// Write colored text
MikuConsole.WriteLine("Hello Miku!", MikuRgbColor.MikuCyan);
MikuConsole.WriteLine("Error!", MikuRgbColor.MikuPink);

// Gradient text
MikuConsole.WriteGradientLine("Cyan to Pink", MikuRgbColor.MikuCyan, MikuRgbColor.MikuPink);

// Rainbow text
MikuConsole.WriteRainbowLine("Rainbow colors!");

// Miku-themed rainbow
MikuConsole.WriteMikuRainbow("Miku Rainbow!");
```

## MikuConsole Methods

### Basic Output

```csharp
// TrueColor (24-bit RGB)
MikuConsole.Write("text", MikuRgbColor.MikuCyan);
MikuConsole.WriteLine("text", MikuRgbColor.MikuCyan);
MikuConsole.WriteLine("text", foreground, background);

// 256-Color palette
MikuConsole.Write256("text", 44);  // Color code 0-255
MikuConsole.WriteLine256("text", 44);
```

### Styled Text

```csharp
MikuConsole.WriteBold("Bold text", MikuRgbColor.MikuCyan);
MikuConsole.WriteUnderline("Underlined", MikuRgbColor.MikuPink);
MikuConsole.WriteItalic("Italic text", MikuRgbColor.MikuTeal);
MikuConsole.WriteStyled("Custom", color, MikuAnsiCodes.Bold, MikuAnsiCodes.Underline);
```

### Gradients and Rainbow

```csharp
MikuConsole.WriteGradient("Gradient text", MikuRgbColor.MikuCyan, MikuRgbColor.MikuPink);
MikuConsole.WriteGradientLine("With newline", from, to);
MikuConsole.WriteRainbow("Rainbow!");
MikuConsole.WriteRainbowLine("Rainbow line!");
MikuConsole.WriteMikuRainbow("Miku colors!");
```

### Positioning

```csharp
MikuConsole.WriteAt(10, 5, "At position", MikuRgbColor.MikuCyan);
MikuConsole.WriteCentered("Centered text", MikuRgbColor.MikuPink);
MikuConsole.WriteCenteredLine("Centered with newline", color);

// Get current cursor position
var (left, top) = MikuConsole.GetCursorPosition();
MikuConsole.SetCursorPosition(x, y);
```

### Drawing

```csharp
MikuConsole.DrawBar(20, MikuRgbColor.MikuCyan);           // Solid bar at cursor
MikuConsole.DrawBar(10, 5, 20, MikuRgbColor.MikuCyan);    // Bar at position (x=10, y=5)

MikuConsole.DrawGradientBar(20, from, to);            // Gradient bar at cursor
MikuConsole.DrawGradientBar(10, 5, 20, from, to);     // Gradient bar at position

MikuConsole.DrawBox(0, 0, 40, 10, MikuRgbColor.MikuCyan); // Box with border
```

### Console Control

```csharp
MikuConsole.Initialize();  // Setup UTF-8 encoding
MikuConsole.Clear();       // Clear screen
MikuConsole.HideCursor();  // Hide cursor
MikuConsole.ShowCursor();  // Show cursor
```

## MikuConsoleAnimation Methods

### Typewriter Effects

```csharp
await MikuConsoleAnimation.TypewriterAsync("Typing...", MikuRgbColor.MikuCyan, delayMs: 30);
await MikuConsoleAnimation.TypewriterLineAsync("With newline", color, delayMs: 30);
await MikuConsoleAnimation.TypewriterGradientAsync("Gradient typing", from, to, delayMs: 30);
```

### Fade Effects

```csharp
await MikuConsoleAnimation.FadeInAsync("Fade in", MikuRgbColor.MikuCyan, x: 0, y: 0);
await MikuConsoleAnimation.FadeOutAsync("Fade out", MikuRgbColor.MikuCyan, x: 0, y: 0);
```

### Pulse Effects

```csharp
await MikuConsoleAnimation.PulseAsync("Pulsing!", color1, color2, x: 0, y: 0, pulseCount: 3);
await MikuConsoleAnimation.BreathingAsync("Breathing", dark, bright, x: 0, y: 0, breathCount: 3);
```

### Wave Effects

```csharp
await MikuConsoleAnimation.ColorWaveAsync("Wave text", color1, color2, x: 0, y: 0);
await MikuConsoleAnimation.RainbowWaveAsync("Rainbow wave", x: 0, y: 0);
```

### Loading Indicators

```csharp
await MikuConsoleAnimation.SpinnerAsync("Loading...", MikuRgbColor.MikuCyan, x: 0, y: 0);
await MikuConsoleAnimation.ProgressBarAsync(x: 0, y: 0, width: 40, fillColor, emptyColor);
```

### Screen Effects

```csharp
await MikuConsoleAnimation.RevealLinesAsync(lines, MikuRgbColor.MikuCyan, startX: 0, startY: 0);
await MikuConsoleAnimation.RevealLinesAlternatingAsync(lines, color1, color2, startX: 0, startY: 0);
```

## Predefined Miku Colors

Use colors from `Miku.Core.MikuRgbColor`:

| Color | Hex | Description |
|-------|-----|-------------|
| `MikuRgbColor.MikuCyan` | #00CED1 | Miku's signature cyan |
| `MikuRgbColor.MikuPink` | #E12885 | Secondary pink |
| `MikuRgbColor.MikuTeal` | #39C5BB | Hair highlight |
| `MikuRgbColor.MikuDarkCyan` | #008B8B | Dark accent |

Plus standard colors: Black, White, Red, Green, Blue, Yellow, Magenta, Cyan, Gray, DarkRed, Orange

## Complete Example

```csharp
using Miku.Core;
using Miku.Console;

MikuConsole.Initialize();
MikuConsole.HideCursor();

try
{
    MikuConsole.Clear();
    
    // Animated title
    await MikuConsoleAnimation.TypewriterGradientAsync(
        "Welcome to MikuLib.Console!",
        MikuRgbColor.MikuCyan,
        MikuRgbColor.MikuPink,
        delayMs: 50
    );
    
    MikuConsole.WriteLine();
    MikuConsole.WriteLine();
    
    // Draw a box
    MikuConsole.DrawBox(2, 3, 50, 10, MikuRgbColor.MikuCyan);
    
    // Text inside box
    MikuConsole.WriteAt(4, 5, "Like Miku's concerts,", MikuRgbColor.MikuPink);
    MikuConsole.WriteAt(4, 6, "your console shines bright!", MikuRgbColor.MikuCyan);
    
    // Progress bar
    MikuConsole.WriteAt(4, 8, "Loading: ", MikuRgbColor.White);
    await MikuConsoleAnimation.ProgressBarAsync(
        13, 8, 30,
        MikuRgbColor.MikuCyan,
        MikuRgbColor.Gray,
        durationMs: 2000
    );
    
    // Rainbow finale
    MikuConsole.WriteRainbowLine("    Complete!");
}
finally
{
    MikuConsole.ShowCursor();
}
```

## Requirements

- .NET 10.0 or higher
- Terminal with ANSI escape sequence support (most modern terminals)

## Dependencies

- `MikuLib.Core` - For MikuRgbColor, MikuAnsiCodes, and MikuColorHelper

## License

MIT License - See LICENSE file for details.

## Credits

Created by **Hatsune Nemas** with inspiration from:

- Hatsune Miku - CV01, born August 31st, 2007
- The colorful light shows at Miku's concerts
- Crypton Future Media

## Repository

https://github.com/DjNemas/MikuLib

---

*"Paint your terminal with the colors of the future!"*

**Version**: 10.0.39 (CV01 Console Edition)  
**Primary Color**: Cyan (#00CED1)
