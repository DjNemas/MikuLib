# MikuLib

> "The world is mine... and so are these utilities!" - Hatsune Nemas

A collection of powerful .NET 10 libraries inspired by Hatsune Miku.

## Libraries

### MikuLib.Logger (v10.1.39)
High-performance, thread-safe logging with zero data loss guarantee.
- Singleton File Stream Management
- 5000+ msg/s throughput
- ILogger compatible
- [Documentation](Miku.Logger/README.md)

### MikuLib.Utils (v10.0.39)
Essential utilities for .NET applications.
- MikuMapper for object mapping
- CommandLineHelper for argument parsing
- [Documentation](Miku.Utils/README.md)

## Installation

```bash
# Install all libraries
dotnet add package MikuLib --version 10.1.39

# Or individual packages
dotnet add package MikuLib.Logger --version 10.1.39
dotnet add package MikuLib.Utils --version 10.0.39
```

## Quick Start

```csharp
using Miku.Logger;
using Miku.Utils;

// Logging
var logger = new MikuLogger("MyApp");
logger.LogInformation("Application started");

// Object Mapping
var userDto = MikuMapper.MapPropertys<UserDto>(user);
```

## Version Pattern

```
MAJOR.MINOR.39
  |     |    ???? Always 39 (Mi-Ku)
  |     ????????? Features
  ??????????????? .NET version
```

Current: **10.1.39**

## Why "39"?

In Japanese, 3 = "mi", 9 = "ku" ? Miku (??)

## Links

- [GitHub](https://github.com/DjNemas/MikuLib)
- [NuGet](https://www.nuget.org/profiles/HatsuneNemas)
- [Performance Optimization](PERFORMANCE_OPTIMIZATION.md)

## License

MIT License

---

*Singing since 2007, Coding since 2025*

**Version**: 10.1.39 | **Color**: Cyan (#00CED1)
