namespace EscherTiler.Graphics.GDI
{
    /// <summary>
    ///     Defines the possible printing modes for a <see cref="TilerPrintDocument" />.
    /// </summary>
    public enum TilingPrintMode
    {
        /// <summary>
        ///     Prints the full tiling with full styles.
        /// </summary>
        TilingFull,

        /// <summary>
        ///     Prints the tiling with black lines and white filling.
        /// </summary>
        TilingLines,

        /// <summary>
        ///     Prints a single tile with full style.
        /// </summary>
        SingleTileFull,

        /// <summary>
        ///     Prints a single tile with black lines and white filling.
        /// </summary>
        SingleTileLines
    }
}