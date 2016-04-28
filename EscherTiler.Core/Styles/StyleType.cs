namespace EscherTiler.Styles
{
    /// <summary>
    ///     Defines the different types of <see cref="IStyle" />.
    /// </summary>
    public enum StyleType : byte
    {
        /// <summary>
        ///     The solid colour style.
        /// </summary>
        SolidColour,

        /// <summary>
        ///     The random colour style.
        /// </summary>
        RandomColour,

        /// <summary>
        ///     The linear gradient style.
        /// </summary>
        LinearGradient,

        /// <summary>
        ///     The radial gradient style.
        /// </summary>
        RadialGradient,

        /// <summary>
        ///     The image style.
        /// </summary>
        Image
    }
}