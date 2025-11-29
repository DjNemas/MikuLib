# Miku

The main Miku library - Your one-stop package for all Miku sub-libraries.

> "The world is mine... and so are these utilities!" - Hatsune Nemas

*Aggregating the power of CV01 since 2007*

## What is Miku?

Miku is a comprehensive .NET 10 library collection that brings together powerful utilities for modern application development. Named after and inspired by Hatsune Miku, the virtual singer who revolutionized music and technology.

## Included Libraries

### MikuUtils
- **MikuMapper**: Object-to-object mapping with nullable primitive support
- **CommandLineHelper**: Modern command line argument parsing
- Full documentation: [Miku.Utils README](../Miku.Utils/README.md)

### Miku.Logger
- Thread-safe logging with console and file output
- ILogger compatibility for ASP.NET Core
- Colored console output and log rotation
- Full documentation: [Miku.Logger README](../Miku.Logger/README.md)

## Installation

Install the main package to get all sub-libraries:

```bash
dotnet add package Miku
```

Or add to your `.csproj`:
```xml
<PackageReference Include="Miku" Version="10.0.39" />
```

## Quick Start

```csharp
using Miku.Utils;
using Miku.Logger;
using Miku.Logger.Configuration;

// Object Mapping
var userDto = MikuMapper.MapPropertys<UserDto>(user);

// Command Line Parsing
if (CommandLineHelper.IsReleaseConfiguration())
{
    // Use release settings
}

// Logging
var logger = new MikuLogger("MyApp");
logger.LogInformation("Application started");
```

## Features at a Glance

### Core Utilities
- Property mapping with type conversion
- Nullable primitive support (int ? int?, etc.)
- Command line argument parsing
- Environment detection

### Logging
- Multiple output targets (Console, File, Both)
- Thread-safe async operations
- Colored console output (Cyan by default)
- Automatic log rotation
- ILogger compatibility

### Performance
- High-performance reflection
- LINQ-optimized parsing
- Async queue-based file I/O
- Thread-safe operations

## Why "Miku"?

This library is named after and inspired by **Hatsune Miku** (????), the virtual singer:

- **Born**: August 31st, 2007
- **Created by**: Crypton Future Media
- **Series**: Character Vocal Series 01 (CV01)
- **Number**: 39 (Mi-Ku in Japanese)
- **Age**: 16 years old
- **Signature Color**: Cyan (#00CED1)

Just as Miku revolutionized music through technology, these libraries aim to make development more efficient and harmonious.

## Easter Eggs

This library contains subtle Hatsune Miku references throughout:

- Version 10.0.39 (10 for .NET 10, 39 for Mi-Ku)
- Default colors inspired by Miku's aesthetic
- CV01 references in code and documentation
- Comments with song references
- Constants using the number 39

For those who know, you know.

## Philosophy

> "The future of voice, the future of code!"

These libraries aim to:
- Be accessible to developers worldwide
- Work in harmony with existing .NET ecosystems
- Stay modern and future-proof
- Bring efficiency to coding

## Documentation

- [Miku.Utils Documentation](../Miku.Utils/README.md)
- [Miku.Utils Examples](../Miku.Utils/EXAMPLES.md)
- [Miku.Logger Documentation](../Miku.Logger/README.md)
- [Miku.Logger Examples](../Miku.Logger/EXAMPLES.md)

## Requirements

- .NET 10.0 or higher
- C# 14.0

## Contributing

Contributions are welcome! Please open an issue or pull request on GitHub.

## Credits

**Created by**: Hatsune Nemas

**Inspired by**:
- Hatsune Miku (????) - The virtual diva
- Crypton Future Media
- The global Vocaloid community

**Songs that inspired development**:
- "World is Mine"
- "Tell Your World"
- "Senbonzakura"

## License

MIT License - See LICENSE file for details.

## Repository

https://github.com/DjNemas/MikuLib

---

*"Singing the code since 2007!"*

**Version**: 10.0.39 (The Mi-Ku Edition)  
**Series**: CV01 Developer Tools  
**Default Color**: Cyan (#00CED1)
