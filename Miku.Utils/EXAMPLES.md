# Miku.Utils - Usage Examples

This document provides practical examples for using Miku.Utils in various scenarios.

## Table of Contents
1. [MikuMapper Examples](#mikumapper-examples)
2. [CommandLineHelper Examples](#commandlinehelper-examples)
3. [Combined Usage](#combined-usage)

---

## MikuMapper Examples

### Basic Object Mapping

```csharp
using Miku.Utils;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public bool? IsActive { get; set; }
}

public class UserDto
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public bool IsActive { get; set; }
}

// Simple mapping
var user = new User 
{ 
    Id = 1, 
    Username = "miku", 
    Email = "miku@example.com",
    Password = "secret",
    IsActive = true 
};

// Map without password
var userDto = MikuMapper.MapProperties<UserDto>(user, true, "Password");
// Result: userDto has all properties except Password
```

### Collection Mapping

```csharp
var users = new List<User>
{
    new User { Id = 1, Username = "miku", Email = "miku@example.com" },
    new User { Id = 2, Username = "rin", Email = "rin@example.com" }
};

// Map entire collection
var userDtos = MikuMapper.MapProperties<UserDto>(users, true, "Password");
// Returns IEnumerable<UserDto> with all users mapped
```

### Update Existing Object

```csharp
// Update existing entity from DTO
var updateDto = new UserUpdateDto 
{ 
    Username = "miku_updated", 
    Email = "newemail@example.com" 
};

var existingUser = dbContext.Users.Find(userId);

// Update properties, exclude Id to prevent overwriting
MikuMapper.MapProperties(updateDto, existingUser, true, "Id");

await dbContext.SaveChangesAsync();
```

### Nullable Primitive Handling

```csharp
// Automatic nullable conversions
public class Source
{
    public int? NullableInt { get; set; } = 42;
    public bool IsActive { get; set; } = true;
}

public class Target
{
    public int NullableInt { get; set; } // Non-nullable
    public bool? IsActive { get; set; }  // Nullable
}

var source = new Source();
var target = MikuMapper.MapProperties<Target>(source);
// Works seamlessly: int? ? int, bool ? bool?
```

---

## CommandLineHelper Examples

### Configuration Detection

```csharp
using Miku.Utils;

// In DbContext factory or startup
public MikuDbContext CreateDbContext(string[] args)
{
    var connectionString = CommandLineHelper.IsReleaseConfiguration(args)
        ? releaseConnectionString
        : debugConnectionString;

    // Or use automatic detection
    var connectionString = CommandLineHelper.IsReleaseConfiguration()
        ? releaseConnectionString
        : debugConnectionString;

    return new MikuDbContext(connectionString);
}
```

### Parameter Value Access

```csharp
// Simple parameter access
var outputPath = CommandLineHelper.GetParameterValue("--output") ?? "./default";
var port = CommandLineHelper.GetParameterValue("--port") ?? "8080";

// Check if parameter exists
if (CommandLineHelper.HasParameter("--verbose"))
{
    logger.MinimumLogLevel = LogLevel.Debug;
}
```

### Multi-Value Parameters

```csharp
// For: --include *.cs --include *.txt --include *.json

// Option 1: Get all values directly
var includes = CommandLineHelper.GetParameterValues("--include");
foreach (var pattern in includes)
{
    Console.WriteLine($"Including: {pattern}");
}

// Option 2: Use Parse() for modern API
var parsed = CommandLineHelper.Parse();
var includePatterns = parsed.GetValues("--include");
// Returns: ["*.cs", "*.txt", "*.json"]
```

### Advanced Parsing with ParsedArguments

```csharp
// Command line: dotnet run --source file1.cs --source file2.cs --output ./build --verbose --log-level Debug

var parsed = CommandLineHelper.Parse();

// Multi-value parameter
var sources = parsed.GetValues("--source");
// Returns: ["file1.cs", "file2.cs"]

// Single-value parameter
var output = parsed.GetValue("--output");
// Returns: "./build"

// Check flag existence
if (parsed.HasParameter("--verbose"))
{
    Console.WriteLine("Verbose mode enabled");
}

// Get value with default
var logLevel = parsed.GetValueOrDefault("--log-level", "Information");
// Returns: "Debug"

// Count occurrences
var sourceCount = parsed.GetValueCount("--source");
// Returns: 2
```

### Environment Detection

```csharp
// Check environment from args or environment variables
if (CommandLineHelper.IsEnvironment("Production"))
{
    // Use production settings
    app.UseHttpsRedirection();
}
else if (CommandLineHelper.IsEnvironment("Development"))
{
    // Development settings
    app.UseDeveloperExceptionPage();
}

// Get current environment
var environment = CommandLineHelper.GetEnvironment();
Console.WriteLine($"Running in: {environment}");
```

### Dictionary Parsing

```csharp
// Parse all arguments into dictionary
var args = CommandLineHelper.ParseArguments();

// Access like dictionary
if (args.TryGetValue("--output", out var output))
{
    Console.WriteLine($"Output directory: {output}");
}

// For multi-value support
var argsMulti = CommandLineHelper.ParseArgumentsWithMultipleValues();
if (argsMulti.TryGetValue("--define", out var defines))
{
    foreach (var define in defines)
    {
        Console.WriteLine($"Define: {define}");
    }
}
```

---

## Combined Usage

### ASP.NET Core Startup

```csharp
using Miku.Utils;

var builder = WebApplication.CreateBuilder(args);

// Use CommandLineHelper for configuration
var connectionString = CommandLineHelper.IsReleaseConfiguration()
    ? builder.Configuration.GetConnectionString("Release")
    : builder.Configuration.GetConnectionString("Debug");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

var app = builder.Build();

// Map configuration to settings DTO
var configSettings = builder.Configuration.GetSection("AppSettings").Get<AppSettings>();
var settingsDto = MikuMapper.MapProperties<AppSettingsDto>(configSettings);
```

### Entity Framework Migration Tool

```csharp
public class MyDbContextFactory : IDesignTimeDbContextFactory<MyDbContext>
{
    public MyDbContext CreateDbContext(string[] args)
    {
        // Automatically detect configuration
        var isRelease = CommandLineHelper.IsReleaseConfiguration(args);
        
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .AddJsonFile($"appsettings.{(isRelease ? "Release" : "Debug")}.json")
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<MyDbContext>();
        optionsBuilder.UseSqlServer(configuration.GetConnectionString("Default"));

        return new MyDbContext(optionsBuilder.Options);
    }
}
```

### API Controller with DTO Mapping

```csharp
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly AppDbContext _context;

    public UsersController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto>> GetUser(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null) return NotFound();

        // Map to DTO, exclude sensitive data
        var userDto = MikuMapper.MapProperties<UserDto>(user, true, "Password", "Salt");
        return Ok(userDto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(int id, UserUpdateDto updateDto)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null) return NotFound();

        // Update entity from DTO, preserve Id
        MikuMapper.MapProperties(updateDto, user, true, "Id");
        
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
```

### Build Script

```csharp
// Build script with command line parsing
var parsed = CommandLineHelper.Parse();

var configuration = parsed.GetValueOrDefault("--configuration", "Debug");
var outputPath = parsed.GetValueOrDefault("--output", "./bin");
var sourceFiles = parsed.GetValues("--source");
var defines = parsed.GetValues("--define");

Console.WriteLine($"Building with configuration: {configuration}");
Console.WriteLine($"Output: {outputPath}");

foreach (var source in sourceFiles)
{
    Console.WriteLine($"Source: {source}");
}

foreach (var define in defines)
{
    Console.WriteLine($"Define: {define}");
}
```

---

## Best Practices

### MikuMapper
1. **Exclude Sensitive Data**: Always exclude passwords, tokens, etc. when mapping to DTOs
2. **Use Collection Mapping**: More efficient than manual loops
3. **Preserve IDs**: Exclude primary keys when updating entities
4. **Null Handling**: Use `ignoreNull: true` (default) to avoid overwriting with nulls

### CommandLineHelper
1. **Use Parse()**: Modern `ParsedArguments` API for complex scenarios
2. **Provide Defaults**: Use `GetValueOrDefault()` for optional parameters
3. **Case-Insensitive**: Don't worry about `--Verbose` vs `--verbose`
4. **Auto-Detection**: Let `Environment.GetCommandLineArgs()` work for you
5. **Multi-Value**: Use `GetValues()` when parameters can repeat

---

For more information, see the [README.md](README.md) and inline XML documentation.
