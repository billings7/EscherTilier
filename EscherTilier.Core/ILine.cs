using System.Numerics;
using EscherTilier.Graphics;
using EscherTilier.Numerics;
using JetBrains.Annotations;

namespace EscherTilier
{
    /// <summary>
    ///     Interface to a line segment.
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
        ///     Gets the approximate bounds for this line after it has been transformed by the given <paramref name="transform" />.
        ///     The rectangle returned should equal or contain the actual bounds.
        /// </summary>
        /// <param name="transform">The transform to apply to the line.</param>
        /// <returns></returns>
        Rectangle GetApproximateBounds(Matrix3x2 transform);

        /// <summary>
        ///     Adds the line to the given <paramref name="path" /> after transforming it by the given
        ///     <paramref name="transform" />.
        /// </summary>
        /// <param name="path">The path to add the line to.</param>
        /// <param name="transform">The transform.</param>
        void AddToPath([NotNull] IGraphicsPath path, Matrix3x2 transform);

        /// <summary>
        ///     Draws the line to the given <paramref name="graphics" /> after transforming it by the given
        ///     <paramref name="transform" />.
        /// </summary>
        /// <param name="graphics">The graphics to draw to.</param>
        /// <param name="transform">The transform.</param>
        void Draw([NotNull] IGraphics graphics, Matrix3x2 transform);

        /// <summary>
        ///     Tests whether the given point is within the given tolerance on this line after it has been transformed by the given
        ///     <paramref name="transform" />,
        ///     returning the exact point on the line if hit.
        /// </summary>
        /// <param name="point">The point to test.</param>
        /// <param name="tolerance">The tolerance. Must be greater than or equal 0.1.</param>
        /// <param name="transform">The transform.</param>
        /// <returns>The exact point on the line if hit; otherwise <see langword="null" />.</returns>
        [CanBeNull]
        LinePoint HitTest(Vector2 point, float tolerance, Matrix3x2 transform);
    }
}