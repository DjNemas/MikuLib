# MikuLib - Meta Package

A collection of powerful .NET 10 libraries inspired by Hatsune Miku.

> "The world is mine... to code!" - Hatsune Nemas

*Powered by CV01 energy since 2007*

![MikuLib Console & Logger Demo](https://djnemas.de/SX/WindowsTerminal_Utmoe5RPDL.gif)

## What is MikuLib?

MikuLib is a comprehensive .NET 10 library collection that brings together powerful utilities for modern application development. Named after and inspired by Hatsune Miku, the virtual singer who revolutionized music and technology.

## Included Libraries

This meta package includes all MikuLib sub-libraries:

### MikuLib.Core
**Core types and utilities** shared across all MikuLib packages:
- **RgbColor**: 24-bit RGB color structure with hex parsing and interpolation
- **AnsiCodes**: ANSI escape code constants for console styling
- **ColorHelper**: Color manipulation utilities (gradients, rainbow, darken/lighten)

### MikuLib.Console
**Beautiful colored console output** with TrueColor and 256-color support:
- **MikuConsole**: Colored text output with gradients and rainbow effects
- **ConsoleAnimation**: Typewriter, fade, pulse, wave, and loading animations

### MikuLib.Utils
**Comprehensive utility library** featuring:
- **MikuMapper**: Object-to-object mapping with nullable primitive support
- **CommandLineHelper**: Modern command line argument parsing with auto-detection

### MikuLib.Logger
**Thread-safe logging library** featuring:
- Console and file output with customizable colors
- **Server-Sent Events (SSE)** for real-time log streaming
- **TrueColor (24-bit RGB)** and **256-color** support
- ILogger compatibility for ASP.NET Core
- Async operations and automatic log rotation
- Zero data loss guarantee

## Installation

Install the meta package to get all sub-libraries:

```bash
dotnet add package MikuLib
```

Or add to your `.csproj`:
```xml
<PackageReference Include="MikuLib" Version="10.2.39" />
```

You can also install individual packages:

```bash
dotnet add package MikuLib.Core
dotnet add package MikuLib.Console
dotnet add package MikuLib.Utils
dotnet add package MikuLib.Logger
```

## Quick Start

### Core Color Types
```csharp
using Miku.Core;

// Create colors
var cyan = RgbColor.MikuCyan;           // Predefined Miku cyan
var pink = RgbColor.FromHex("#E12885"); // From hex string

// Interpolate colors
var blend = RgbColor.Lerp(cyan, pink, 0.5);

// Rainbow effects
var rainbow = ColorHelper.GetRainbow(phase);
var mikuRainbow = ColorHelper.GetMikuRainbow(phase);
```

### Console Output with Colors
```csharp
using Miku.Core;
using MikuLib.Console;

// Colored output
MikuConsole.WriteLine("Hello Miku!", RgbColor.MikuCyan);
MikuConsole.WriteGradientLine("Gradient text", RgbColor.MikuCyan, RgbColor.MikuPink);

// Animations
await ConsoleAnimation.TypewriterAsync("Typing...", RgbColor.MikuCyan, 30);
await ConsoleAnimation.PulseAsync("Pulsing!", RgbColor.MikuCyan, RgbColor.MikuPink, 0, 0);
```

### Logging with Colors
```csharp
using Miku.Core;
using Miku.Logger;
using Miku.Logger.Configuration;
using Miku.Logger.Configuration.Enums;
using Miku.Logger.Configuration.Models;

var options = new MikuLoggerOptions
{
    Output = LogOutput.Console,
    ConsoleColors = new ConsoleColorOptions
    {
        ColorSpace = ColorSpace.TrueColor,
        TrueColors = new TrueColorOptions
        {
            InformationColor = RgbColor.MikuCyan,
            ErrorColor = RgbColor.MikuPink
        }
    }
};

using var logger = new MikuLogger("MyApp", options);
logger.LogInformation("Hello with Miku Cyan!");
```

### Object Mapping
```csharp
using Miku.Utils;

var userDto = MikuMapper.MapProperties<UserDto>(user);
```

## Features at a Glance

### Core Types (MikuLib.Core)
- 24-bit RGB color structure with hex parsing
- Color interpolation and manipulation
- ANSI escape codes for console styling
- Rainbow and gradient generators
- Predefined Miku colors (Cyan, Pink, Teal, DarkCyan)

### Console Utilities (MikuLib.Console)
- TrueColor (16 million colors) console output
- 256-color palette support
- Gradient text and progress bars
- Typewriter, fade, pulse, and wave animations
- Centered and positioned text output
- Block drawing characters and boxes

### Utility Functions (MikuLib.Utils)
- Object-to-object property mapping
- Nullable primitive support
- Command line argument parsing
- Environment detection

### Logging (MikuLib.Logger)
- Three color modes: Console (16), Extended256 (256), TrueColor (16M)
- Server-Sent Events for real-time streaming
- Multiple output targets (Console, File, SSE)
- Thread-safe async operations
- Automatic log rotation
- ILogger compatibility
- High-performance logging (5000+ msg/s)
- Zero data loss guarantee

## Predefined Miku Colors

| Color | Hex | Description |
|-------|-----|-------------|
| MikuCyan | #00CED1 | Signature cyan |
| MikuPink | #E12885 | Secondary pink |
| MikuTeal | #39C5BB | Hair highlight |
| MikuDarkCyan | #008B8B | Dark accent |

## Version Numbering

MikuLib follows this versioning pattern:

```
MAJOR.MINOR.39
  |     |    └─── Always 39 (Mi-Ku Easter Egg)
  |     └──────── Increments for new features/updates
  └────────────── Matches .NET version (10, 11, 12, etc.)
```

**Current Versions:**
- MikuLib.Core: 10.0.39
- MikuLib.Console: 10.0.39
- MikuLib.Utils: 10.1.39
- MikuLib.Logger: 10.2.39
- **MikuLib (Meta): 10.2.39**

## Requirements

- .NET 10.0 or higher
- C# 14.0

## Why "Miku"?

This library is named after and inspired by **Hatsune Miku** (初音ミク), the virtual singer:

- **Born**: August 31st, 2007
- **Created by**: Crypton Future Media
- **Series**: Character Vocal Series 01 (CV01)
- **Number**: 39 (Mi-Ku in Japanese goroawase)
- **Age**: 16 years old
- **Signature Color**: Cyan (#00CED1)

Just as Miku revolutionized music through technology, these libraries aim to make development more efficient and harmonious.

## Easter Eggs

This library contains subtle Hatsune Miku references throughout:

- Version numbers ending in 39 (Mi-Ku)
- Default colors inspired by Miku's aesthetic
- CV01 references in code and documentation
- Constants using the number 39
- Predefined Miku color palette

For those who know, you know.

## Philosophy

> "The future of voice, the future of code!"

MikuLib aims to:
- Be accessible to developers worldwide
- Work in harmony with existing .NET ecosystems
- Stay modern and future-proof
- Bring efficiency and joy to coding

## Documentation

Full documentation is available in the GitHub repository:
- [MikuLib GitHub](https://github.com/DjNemas/MikuLib)
- [Issue Tracker](https://github.com/DjNemas/MikuLib/issues)

## Contributing

Contributions are welcome! Please visit the [GitHub repository](https://github.com/DjNemas/MikuLib).

## Credits

**Created by**: Hatsune Nemas

**Inspired by**:
- Hatsune Miku (初音ミク) - The virtual diva who started it all
- Crypton Future Media - For creating VOCALOID technology
- The global Vocaloid community - For endless creativity and passion

## License

MIT License - See LICENSE file for details.

---

*"Singing the code since 2007!"*

**Version**: 10.2.39 (The Mi-Ku Edition)  
**Series**: CV01 Developer Tools  
**Default Color**: Cyan (#00CED1)
