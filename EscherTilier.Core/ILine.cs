using System.Numerics;
using EscherTilier.Graphics;
using EscherTilier.Numerics;

namespace EscherTilier
{
    /// <summary>
    /// Interface to a line segment.
    /// </summary>
    public interface ILine
    {
        /// <summary>
        ///     Gets the start point of the line.
        /// </summary>
        /// <value>
        ///     The start point.
        /// </value>
        Vector2 Start { get; }

        /// <summary>
        ///     Gets the ens point of the line.
        /// </summary>
        /// <value>
        ///     The ens point.
        /// </value>
        Vector2 End { get; }

        /// <summary>
        ///     Gets the approximate bounds for this line after it has been transformed by the given <paramref name="transform"/>.
        ///     The rectangle returned should equal or contain the actual bounds.
        /// </summary>
        /// <param name="transform">The transform to apply to the line.</param>
        /// <returns></returns>
        Rectangle GetApproximateBounds(Matrix3x2 transform);

        /// <summary>
        /// Adds the line to the given <paramref name="path"/> after transforming it by the given <paramref name="transform"/>.
        /// </summary>
        /// <param name="path">The path to add the line to.</param>
        /// <param name="transform">The transform.</param>
        void AddToPath(IGraphicsPath path, Matrix3x2 transform);

        /// <summary>
        /// Draws the line to the given <paramref name="graphics"/> after transforming it by the given <paramref name="transform"/>.
        /// </summary>
        /// <param name="graphics">The graphics to draw to.</param>
        /// <param name="transform">The transform.</param>
        void Draw(IGraphics graphics, Matrix3x2 transform);
    }
}