using System.Numerics;

namespace EscherTiler.Styles
{
    /// <summary>
    ///     A style that draws a solid colour.
    /// </summary>
    public partial class SolidColourStyle : IStyle
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="SolidColourStyle" /> class.
        /// </summary>
        /// <param name="colour">The colour.</param>
        public SolidColourStyle(Colour colour)
        {
            Colour = colour;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="SolidColourStyle" /> class.
        /// </summary>
        /// <param name="colour">The colour.</param>
        /// <param name="alpha">The alpha.</param>
        public SolidColourStyle(Colour colour, float alpha)
        {
            Colour = new Colour(colour, alpha);
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="SolidColourStyle" /> class.
        /// </summary>
        /// <param name="red">The red component.</param>
        /// <param name="green">The green component.</param>
        /// <param name="blue">The blue component.</param>
        /// <param name="alpha">The alpha component.</param>
        public SolidColourStyle(byte red, byte green, byte blue, byte alpha = 255)
        {
            Colour = new Colour(red, green, blue, alpha);
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="SolidColourStyle" /> class.
        /// </summary>
        /// <param name="red">The red component.</param>
        /// <param name="green">The green component.</param>
        /// <param name="blue">The blue component.</param>
        /// <param name="alpha">The alpha component.</param>
        public SolidColourStyle(float red, float green, float blue, float alpha = 1f)
        {
            Colour = new Colour(red, green, blue, alpha);
        }

        /// <summary>
        ///     Gets the type of the style.
        /// </summary>
        /// <value>
        ///     The type.
        /// </value>
        public StyleType Type => StyleType.SolidColour;

        /// <summary>
        ///     Gets the colour.
        /// </summary>
        /// <value>
        ///     The colour.
        /// </value>
        public Colour Colour { get; }

        /// <summary>
        ///     Returns a copy of this style with the given transform applied.
        /// </summary>
        /// <param name="matrix">The transform matrix.</param>
        /// <returns>
        ///     The transformed style.
        /// </returns>
        public IStyle Transform(Matrix3x2 matrix) => this;
    }
}