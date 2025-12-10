namespace Miku.Core
{
    /// <summary>
    /// Represents a 24-bit RGB color.
    /// </summary>
    /// <remarks>
    /// Like Miku's signature cyan (#00CED1), express yourself in any color imaginable!
    /// </remarks>
    public readonly struct MikuRgbColor : IEquatable<MikuRgbColor>
    {
        /// <summary>Gets the red component (0-255).</summary>
        public byte R { get; }

        /// <summary>Gets the green component (0-255).</summary>
        public byte G { get; }

        /// <summary>Gets the blue component (0-255).</summary>
        public byte B { get; }

        /// <summary>
        /// Initializes a new MikuRgbColor with the specified RGB values.
        /// </summary>
        public MikuRgbColor(byte r, byte g, byte b)
        {
            R = r;
            G = g;
            B = b;
        }

        /// <summary>
        /// Creates an MikuRgbColor from a hex string (e.g., "#00CED1" or "00CED1").
        /// </summary>
        public static MikuRgbColor FromHex(string hex)
        {
            ArgumentNullException.ThrowIfNull(hex);

            hex = hex.TrimStart('#');

            if (hex.Length != 6)
                throw new ArgumentException("Hex color must be 6 characters (e.g., '00CED1').", nameof(hex));

            return new MikuRgbColor(
                Convert.ToByte(hex[..2], 16),
                Convert.ToByte(hex[2..4], 16),
                Convert.ToByte(hex[4..6], 16)
            );
        }

        /// <summary>
        /// Converts the color to a hex string.
        /// </summary>
        public string ToHex() => $"#{R:X2}{G:X2}{B:X2}";

        /// <summary>
        /// Interpolates between two colors.
        /// </summary>
        /// <param name="from">Start color.</param>
        /// <param name="to">End color.</param>
        /// <param name="t">Interpolation factor (0.0 - 1.0).</param>
        public static MikuRgbColor Lerp(MikuRgbColor from, MikuRgbColor to, double t)
        {
            t = Math.Clamp(t, 0, 1);
            return new MikuRgbColor(
                (byte)(from.R + (to.R - from.R) * t),
                (byte)(from.G + (to.G - from.G) * t),
                (byte)(from.B + (to.B - from.B) * t)
            );
        }

        /// <inheritdoc/>
        public override string ToString() => ToHex();

        /// <inheritdoc/>
        public override bool Equals(object? obj) => obj is MikuRgbColor other && Equals(other);

        /// <inheritdoc/>
        public bool Equals(MikuRgbColor other) => R == other.R && G == other.G && B == other.B;

        /// <inheritdoc/>
        public override int GetHashCode() => HashCode.Combine(R, G, B);

        /// <summary>Equality operator.</summary>
        public static bool operator ==(MikuRgbColor left, MikuRgbColor right) => left.Equals(right);

        /// <summary>Inequality operator.</summary>
        public static bool operator !=(MikuRgbColor left, MikuRgbColor right) => !left.Equals(right);

        #region Predefined Colors - Miku Theme

        /// <summary>Miku's signature cyan color (#00CED1).</summary>
        public static MikuRgbColor MikuCyan => new(0x00, 0xCE, 0xD1);

        /// <summary>Miku's secondary pink color (#E12885).</summary>
        public static MikuRgbColor MikuPink => new(0xE1, 0x28, 0x85);

        /// <summary>Miku's hair highlight (#39C5BB).</summary>
        public static MikuRgbColor MikuTeal => new(0x39, 0xC5, 0xBB);

        /// <summary>Miku's dark accent (#008B8B).</summary>
        public static MikuRgbColor MikuDarkCyan => new(0x00, 0x8B, 0x8B);

        #endregion

        #region Predefined Colors - Standard

        /// <summary>Black (#000000).</summary>
        public static MikuRgbColor Black => new(0, 0, 0);

        /// <summary>White (#FFFFFF).</summary>
        public static MikuRgbColor White => new(255, 255, 255);

        /// <summary>Red (#FF0000).</summary>
        public static MikuRgbColor Red => new(255, 0, 0);

        /// <summary>Green (#00FF00).</summary>
        public static MikuRgbColor Green => new(0, 255, 0);

        /// <summary>Blue (#0000FF).</summary>
        public static MikuRgbColor Blue => new(0, 0, 255);

        /// <summary>Yellow (#FFFF00).</summary>
        public static MikuRgbColor Yellow => new(255, 255, 0);

        /// <summary>Magenta (#FF00FF).</summary>
        public static MikuRgbColor Magenta => new(255, 0, 255);

        /// <summary>Cyan (#00FFFF).</summary>
        public static MikuRgbColor Cyan => new(0, 255, 255);

        /// <summary>Gray (#808080).</summary>
        public static MikuRgbColor Gray => new(128, 128, 128);

        /// <summary>Dark Red (#8B0000).</summary>
        public static MikuRgbColor DarkRed => new(139, 0, 0);

        /// <summary>Orange (#FFA500).</summary>
        public static MikuRgbColor Orange => new(255, 165, 0);

        #endregion
    }
}
