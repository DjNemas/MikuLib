# MikuLib.Core

Core types and utilities shared across all MikuLib packages.

> "The heart of the voice!" - Hatsune Nemas

*The foundation of CV01 since August 31st, 2007*

## Features

- **RgbColor** - 24-bit RGB color structure with hex parsing and interpolation
- **AnsiCodes** - ANSI escape code constants for console styling
- **ColorHelper** - Color manipulation utilities (gradients, rainbow, darken/lighten)

## Installation

```bash
dotnet add package MikuLib.Core
```

Or add to your `.csproj`:
```xml
<PackageReference Include="MikuLib.Core" Version="10.0.39" />
```

## Quick Start

### RgbColor

```csharp
using Miku.Core;

// Create colors
var cyan = RgbColor.MikuCyan;           // Predefined Miku cyan
var pink = RgbColor.FromHex("#E12885"); // From hex string
var custom = new RgbColor(255, 128, 0); // From RGB values

// Convert to hex
string hex = cyan.ToHex(); // "#00CED1"

// Interpolate between colors
var blend = RgbColor.Lerp(cyan, pink, 0.5); // 50% blend
```

### AnsiCodes

```csharp
using Miku.Core;

// Text styles
Console.Write(AnsiCodes.Bold + "Bold text" + AnsiCodes.Reset);
Console.Write(AnsiCodes.Underline + "Underlined" + AnsiCodes.Reset);
Console.Write(AnsiCodes.Italic + "Italic" + AnsiCodes.Reset);

// TrueColor (24-bit)
var color = RgbColor.MikuCyan;
Console.Write(AnsiCodes.ForegroundRgb(color) + "Miku Cyan!" + AnsiCodes.Reset);

// 256-color palette
Console.Write(AnsiCodes.Foreground256(44) + "256-color cyan" + AnsiCodes.Reset);

// Cursor control
Console.Write(AnsiCodes.MoveTo(5, 10)); // Move to row 5, column 10
Console.Write(AnsiCodes.ClearLine);     // Clear current line
```

### ColorHelper

```csharp
using Miku.Core;

// Create gradients
var gradient = ColorHelper.CreateGradient(RgbColor.MikuCyan, RgbColor.MikuPink, 10);

// Rainbow colors
var rainbow = ColorHelper.GetRainbow(phase); // phase in radians
var mikuRainbow = ColorHelper.GetMikuRainbow(phase); // Miku-themed rainbow

// Modify colors
var darker = ColorHelper.Darken(RgbColor.MikuCyan, 0.5);  // 50% darker
var lighter = ColorHelper.Lighten(RgbColor.MikuCyan, 0.3); // 30% lighter
var complement = ColorHelper.GetComplementary(RgbColor.MikuCyan);

// Check brightness
bool isDark = ColorHelper.IsDark(color);
int brightness = ColorHelper.GetBrightness(color); // 0-255
```

## Predefined Miku Colors

Like the colors from Miku's iconic design:

| Color | Hex | Property | Description |
|-------|-----|----------|-------------|
| Miku Cyan | #00CED1 | `RgbColor.MikuCyan` | Signature hair/outfit color |
| Miku Pink | #E12885 | `RgbColor.MikuPink` | Secondary accent color |
| Miku Teal | #39C5BB | `RgbColor.MikuTeal` | Hair highlight |
| Miku Dark Cyan | #008B8B | `RgbColor.MikuDarkCyan` | Dark accent |

## Standard Colors

| Color | Property |
|-------|----------|
| Black | `RgbColor.Black` |
| White | `RgbColor.White` |
| Red | `RgbColor.Red` |
| Green | `RgbColor.Green` |
| Blue | `RgbColor.Blue` |
| Yellow | `RgbColor.Yellow` |
| Magenta | `RgbColor.Magenta` |
| Cyan | `RgbColor.Cyan` |
| Gray | `RgbColor.Gray` |
| Dark Red | `RgbColor.DarkRed` |
| Orange | `RgbColor.Orange` |

## ANSI Code Categories

### Text Styles
- `Bold`, `Dim`, `Italic`, `Underline`
- `Blink`, `RapidBlink`, `Reverse`, `Hidden`, `Strikethrough`
- Reset codes for each style

### Cursor Control
- `HideCursor`, `ShowCursor`
- `SaveCursor`, `RestoreCursor`
- `MoveTo(row, col)`, `MoveUp(n)`, `MoveDown(n)`, `MoveLeft(n)`, `MoveRight(n)`

### Screen Control
- `ClearScreen`, `ClearToEnd`, `ClearToBeginning`
- `ClearLine`, `ClearLineToEnd`, `ClearLineToBeginning`

### Colors
- `ForegroundRgb(color)`, `BackgroundRgb(color)` - TrueColor
- `Foreground256(code)`, `Background256(code)` - 256-color

## ColorHelper Methods

| Method | Description |
|--------|-------------|
| `Lerp(from, to, t)` | Interpolate between colors |
| `GetRainbow(phase)` | Rainbow color from phase |
| `GetMikuRainbow(phase, blend)` | Miku-themed rainbow |
| `CreateGradient(from, to, steps)` | Create gradient array |
| `CreateMultiGradient(colors, steps)` | Multi-stop gradient |
| `Darken(color, factor)` | Darken by factor |
| `Lighten(color, factor)` | Lighten by factor |
| `GetComplementary(color)` | Complementary color |
| `GetBrightness(color)` | Perceived brightness (0-255) |
| `IsDark(color)` | Check if dark (brightness < 128) |

## Requirements

- .NET 10.0 or higher

## Used By

- **MikuLib.Logger** - For TrueColor log output
- **MikuLib.Console** - For colored console output
- **MikuLib.Utils** - For console utilities

## License

MIT License - See LICENSE file for details.

## Credits

Created by **Hatsune Nemas** with inspiration from:
- Hatsune Miku - CV01, born August 31st, 2007
- Crypton Future Media
- The global Vocaloid community

## Repository

https://github.com/DjNemas/MikuLib

---

*"The core of the voice, the core of the code!"*

**Version**: 10.0.39 (CV01 Foundation)  
**Signature Color**: Cyan (#00CED1)
