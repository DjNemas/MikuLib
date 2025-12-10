namespace Miku.Logger.Configuration.Enums
{
    /// <summary>
    /// Color space options for console output.
    /// </summary>
    /// <remarks>
    /// Like the different visual styles in Miku's concerts,
    /// different color spaces offer varying levels of visual richness.
    /// </remarks>
    public enum MikuColorSpace
    {
        /// <summary>Standard 16 console colors</summary>
        Console,

        /// <summary>256 color palette</summary>
        Extended256,

        /// <summary>24-bit RGB colors (TrueColor)</summary>
        TrueColor
    }
}
