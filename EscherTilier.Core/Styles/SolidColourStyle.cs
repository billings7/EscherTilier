using System.Numerics;

namespace EscherTilier.Styles
{
    /// <summary>
    ///     A style that draws a solid colour.
    /// </summary>
    public class SolidColourStyle : IStyle
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