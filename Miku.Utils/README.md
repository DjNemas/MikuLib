# MikuLib.Utils

A comprehensive utility library for .NET 10 containing essential tools for object mapping, command line argument parsing, and more.

> "The world is mine... to code!" - Hatsune Nemas

*Powered by CV01 energy since 2007*

## Features

### MikuMapper - Object Mapping
- Property mapping between objects with automatic type conversion
- Support for nullable primitives (e.g., `int ↔ int?`, `bool ↔ bool?`)
- Property exclusion during mapping
- Collection mapping support
- High-performance reflection-based mapping

### CommandLineHelper - Argument Parsing
- Modern, LINQ-based command line argument parsing
- Automatic detection via `Environment.GetCommandLineArgs()`
- Support for multiple parameter formats: `--param value`, `--param:value`, `--param=value`
- Multi-value parameter support
- Environment variable integration
- Case-insensitive by default

## Installation

```bash
dotnet add package MikuLib.Utils
```

Or add to your `.csproj`:
```xml
<PackageReference Include="MikuLib.Utils" Version="10.1.39" />
```

## Quick Start

### MikuMapper

```csharp
using Miku.Utils;

// Map object properties
public class User 
{ 
    public int Id { get; set; }
    public string Name { get; set; }
    public bool? IsActive { get; set; }
}

public class UserDto 
{ 
    public int Id { get; set; }
    public string Name { get; set; }
    public bool IsActive { get; set; }
}

var user = new User { Id = 1, Name = "Miku", IsActive = true };

// Simple mapping
var userDto = MikuMapper.MapProperties<UserDto>(user);

// Exclude properties
var userDto = MikuMapper.MapProperties<UserDto>(user, true, "Password");

// Map collections
var users = new List<User> { user1, user2 };
var userDtos = MikuMapper.MapProperties<UserDto>(users);
```

### CommandLineHelper

```csharp
using Miku.Utils;

// Automatic detection (uses Environment.GetCommandLineArgs())
if (CommandLineHelper.IsReleaseConfiguration())
{
    // Use release settings
}

var outputPath = CommandLineHelper.GetParameterValue("--output") ?? "./default";

// Modern parsing with ParsedArguments
var parsed = CommandLineHelper.Parse();
var includes = parsed.GetValues("--include"); // Multi-value support
var verbose = parsed.HasParameter("--verbose");
```

## MikuMapper Examples

### Basic Mapping
```csharp
var source = new SourceClass { Id = 1, Name = "Test" };
var target = MikuMapper.MapProperties<TargetClass>(source);
```

### Exclude Properties
```csharp
var dto = MikuMapper.MapProperties<UserDto>(user, true, "Password", "Salt");
```

### Nullable Primitives
```csharp
// Automatically handles nullable conversions
// int? -> int, bool -> bool?, etc.
var mapped = MikuMapper.MapProperties<TargetType>(source);
```

### Update Existing Object
```csharp
var existingUser = dbContext.Users.Find(1);
MikuMapper.MapProperties(updateDto, existingUser, true, "Id");
await dbContext.SaveChangesAsync();
```

## CommandLineHelper Examples

### Configuration Detection
```csharp
var connectionString = CommandLineHelper.IsReleaseConfiguration()
    ? releaseConnectionString
    : debugConnectionString;
```

### Parameter Values
```csharp
// Single value
var output = CommandLineHelper.GetParameterValue("--output");

// Multiple values
var includes = CommandLineHelper.GetParameterValues("--include");

// With default
var port = CommandLineHelper.GetParameterValue("--port") ?? "8080";
```

### Advanced Parsing
```csharp
// For: --include *.cs --include *.txt --output ./build --verbose
var parsed = CommandLineHelper.Parse();

// Get all includes
var includes = parsed.GetValues("--include"); // ["*.cs", "*.txt"]

// Get single value
var output = parsed.GetValue("--output"); // "./build"

// Check flag
if (parsed.HasParameter("--verbose"))
{
    // Enable verbose logging
}
```

### Environment Detection
```csharp
// Checks command line args AND environment variables
if (CommandLineHelper.IsEnvironment("Production"))
{
    // Production configuration
}

var env = CommandLineHelper.GetEnvironment(); // Returns: Development, Production, etc.
```

## API Reference

### MikuMapper

**Map to new object:**
```csharp
T MapProperties<T>(object source, bool ignoreNull = true, params string[] excludeProperties)
```

**Map collections:**
```csharp
IEnumerable<T> MapProperties<T>(IEnumerable<object> source, bool ignoreNull = true, params string[] excludeProperties)
```

**Map to existing object:**
```csharp
void MapProperties<T>(object source, in T target, bool ignoreNull = true, params string[] excludeProperties)
```

### CommandLineHelper

**Configuration checks:**
- `IsReleaseConfiguration(args?)` - Check for release build
- `IsDebugConfiguration(args?)` - Check for debug build

**Parameter access:**
- `HasParameter(parameter, args?)` - Check if parameter exists
- `GetParameterValue(parameter, args?)` - Get single value
- `GetParameterValues(parameter, args?)` - Get multiple values

**Parsing:**
- `Parse(args?)` - Returns `ParsedArguments` object
- `ParseArguments(args?)` - Returns `Dictionary<string, string>`
- `ParseArgumentsWithMultipleValues(args?)` - Returns `Dictionary<string, List<string>>`

**Environment:**
- `IsEnvironment(environment, args?)` - Check environment
- `GetEnvironment(args?, defaultEnvironment)` - Get environment name

## Migration from 10.0.39

> ♪ Even Miku makes typos sometimes! ♪

The `MapPropertys` methods have been renamed to `MapProperties` (correct spelling). The old methods are still available but marked as `[Obsolete]` and will be removed in version 10.2.39.

**Before:**
```csharp
var dto = MikuMapper.MapPropertys<UserDto>(user);  // Typo in name
```

**After:**
```csharp
var dto = MikuMapper.MapProperties<UserDto>(user);  // Correct spelling
```

## Performance

- **MikuMapper**: Reflection-based with cached property info for optimal performance
- **CommandLineHelper**: LINQ-optimized for efficient parsing
- **Thread-Safe**: All operations are thread-safe

## Requirements

- .NET 10.0 or higher
- C# 14.0

## Documentation

Full XML documentation is included for IntelliSense support. See [EXAMPLES.md](EXAMPLES.md) for detailed usage scenarios.

## Contributing

Contributions are welcome! Please open an issue or pull request on GitHub.

## License

MIT License - See LICENSE file for details.

## Credits

Created by **Hatsune Nemas** with inspiration from:
- Hatsune Miku (初音ミク) - CV01, born August 31st, 2007
- Crypton Future Media
- The global Vocaloid community

## Repository

https://github.com/DjNemas/MikuLib

---

*"The future of voice, the future of code!"*

**Version**: 10.1.39 (Mi-Ku Edition)  
**Series**: CV01 Developer Tools
