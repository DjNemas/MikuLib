# MikuLib.Core

Core types and utilities shared across all MikuLib packages.

> "The heart of the voice!" - Hatsune Nemas

*The foundation of CV01 since August 31st, 2007*

## Features

- **MikuRgbColor** - 24-bit RGB color structure with hex parsing and interpolation
- **MikuAnsiCodes** - ANSI escape code constants for console styling
- **MikuColorHelper** - Color manipulation utilities (gradients, rainbow, darken/lighten)

## Installation

```bash
dotnet add package MikuLib.Core
```

Or add to your `.csproj`:
```xml
<PackageReference Include="MikuLib.Core" Version="10.0.39" />
```

## Quick Start

### MikuRgbColor

```csharp
using Miku.Core;

// Create colors
var cyan = MikuRgbColor.MikuCyan;           // Predefined Miku cyan
var pink = MikuRgbColor.FromHex("#E12885"); // From hex string
var custom = new MikuRgbColor(255, 128, 0); // From RGB values

// Convert to hex
string hex = cyan.ToHex(); // "#00CED1"

// Interpolate between colors
var blend = MikuRgbColor.Lerp(cyan, pink, 0.5); // 50% blend
```

### MikuAnsiCodes

```csharp
using Miku.Core;

// Text styles
Console.Write(MikuAnsiCodes.Bold + "Bold text" + MikuAnsiCodes.Reset);
Console.Write(MikuAnsiCodes.Underline + "Underlined" + MikuAnsiCodes.Reset);
Console.Write(MikuAnsiCodes.Italic + "Italic" + MikuAnsiCodes.Reset);

// TrueColor (24-bit)
var color = MikuRgbColor.MikuCyan;
Console.Write(MikuAnsiCodes.ForegroundRgb(color) + "Miku Cyan!" + MikuAnsiCodes.Reset);

// 256-color palette
Console.Write(MikuAnsiCodes.Foreground256(44) + "256-color cyan" + MikuAnsiCodes.Reset);

// Cursor control
Console.Write(MikuAnsiCodes.MoveTo(5, 10)); // Move to row 5, column 10
Console.Write(MikuAnsiCodes.ClearLine);     // Clear current line
```

### MikuColorHelper

```csharp
using Miku.Core;

// Create gradients
var gradient = MikuColorHelper.CreateGradient(MikuRgbColor.MikuCyan, MikuRgbColor.MikuPink, 10);

// Rainbow colors
var rainbow = MikuColorHelper.GetRainbow(phase); // phase in radians
var mikuRainbow = MikuColorHelper.GetMikuRainbow(phase); // Miku-themed rainbow

// Modify colors
var darker = MikuColorHelper.Darken(MikuRgbColor.MikuCyan, 0.5);  // 50% darker
var lighter = MikuColorHelper.Lighten(MikuRgbColor.MikuCyan, 0.3); // 30% lighter
var complement = MikuColorHelper.GetComplementary(MikuRgbColor.MikuCyan);

// Check brightness
bool isDark = MikuColorHelper.IsDark(color);
int brightness = MikuColorHelper.GetBrightness(color); // 0-255
```

## Predefined Miku Colors

Like the colors from Miku's iconic design:

| Color | Hex | Property | Description |
|-------|-----|----------|-------------|
| Miku Cyan | #00CED1 | `MikuRgbColor.MikuCyan` | Signature hair/outfit color |
| Miku Pink | #E12885 | `MikuRgbColor.MikuPink` | Secondary accent color |
| Miku Teal | #39C5BB | `MikuRgbColor.MikuTeal` | Hair highlight |
| Miku Dark Cyan | #008B8B | `MikuRgbColor.MikuDarkCyan` | Dark accent |

## Standard Colors

| Color | Property |
|-------|----------|
| Black | `MikuRgbColor.Black` |
| White | `MikuRgbColor.White` |
| Red | `MikuRgbColor.Red` |
| Green | `MikuRgbColor.Green` |
| Blue | `MikuRgbColor.Blue` |
| Yellow | `MikuRgbColor.Yellow` |
| Magenta | `MikuRgbColor.Magenta` |
| Cyan | `MikuRgbColor.Cyan` |
| Gray | `MikuRgbColor.Gray` |
| Dark Red | `MikuRgbColor.DarkRed` |
| Orange | `MikuRgbColor.Orange` |

## MikuAnsiCodes Categories

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

## MikuColorHelper Methods

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
