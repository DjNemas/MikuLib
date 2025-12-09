# Changelog

All notable changes to Miku.Core will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/).

## [10.0.39] - 2025-12-10

### Initial Release

Core types and utilities shared across MikuLib packages.

#### RgbColor Structure
- 24-bit RGB color representation
- Hex string parsing with `FromHex()` (supports "#RRGGBB" and "RRGGBB")
- Hex string output with `ToHex()`
- Color interpolation with `Lerp()`
- Predefined Miku colors: MikuCyan, MikuPink, MikuTeal, MikuDarkCyan
- Standard colors: Black, White, Red, Green, Blue, Yellow, Magenta, Cyan, Gray, DarkRed, Orange
- Equality operators and IEquatable implementation

#### AnsiCodes Constants
- Text styles: Bold, Dim, Italic, Underline, Blink, Reverse, Hidden, Strikethrough
- Style resets for individual styles
- Cursor control: Hide, Show, Save, Restore, MoveTo, MoveUp/Down/Left/Right
- Screen control: ClearScreen, ClearLine, ClearToEnd, ClearToBeginning
- Color helpers: ForegroundRgb, BackgroundRgb, Foreground256, Background256

#### ColorHelper Utilities
- `Lerp()` - Color interpolation
- `GetRainbow()` - Rainbow color from phase
- `GetMikuRainbow()` - Miku-themed rainbow (cyan/pink biased)
- `CreateGradient()` - Create gradient array between two colors
- `CreateMultiGradient()` - Multi-stop gradient
- `Darken()` - Darken color by factor
- `Lighten()` - Lighten color by factor
- `GetComplementary()` - Get complementary color
- `GetBrightness()` - Calculate perceived brightness
- `IsDark()` - Check if color is dark

---

## Version Numbering

Miku.Core follows this versioning pattern:

```
MAJOR.MINOR.39
```

- **MAJOR**: Matches .NET version (10, 11, 12, etc.)
- **MINOR**: Increments for new features and updates
- **39**: Always 39 in honor of Hatsune Miku (Mi-Ku)

**Examples**: 10.0.39 -> 10.1.39 -> 11.0.39
