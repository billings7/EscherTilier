using System.Numerics;

namespace EscherTilier.Styles
{
    /// <summary>
    /// Interface to a style for drawing a tile.
    /// </summary>
    public interface IStyle
    {
        /// <summary>
        /// Returns a copy of this style with the given transform applied.
        /// </summary>
        /// <param name="matrix">The transform matrix.</param>
        /// <returns>The transformed style.</returns>
        IStyle Transform(Matrix3x2 matrix);
    }
}