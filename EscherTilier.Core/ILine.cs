using System.Collections.Generic;
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
        [NotNull]
        LineVector Start { get; }

        /// <summary>
        ///     Gets the end point of the line.
        /// </summary>
        /// <value>
        ///     The end point.
        /// </value>
        [NotNull]
        LineVector End { get; }

        /// <summary>
        ///     Gets the points that are used to define this line.
        /// </summary>
        /// <value>
        ///     The points.
        /// </value>
        /// <remarks>At minimum, this should include the <see cref="Start" /> and <see cref="End" /> points.</remarks>
        [NotNull]
        [ItemNotNull]
        IEnumerable<LineVector> Points { get; }

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
        /// <param name="transform">The transform to apply to the line.</param>
        /// <param name="reverse">If set to <see langword="true"/>, add the line from <see cref="Start"/> to <see cref="End"/>.</param>
        void AddToPath([NotNull] IGraphicsPath path, Matrix3x2 transform, bool reverse);

        /// <summary>
        ///     Draws the line to the given <paramref name="graphics" /> after transforming it by the given
        ///     <paramref name="transform" />.
        /// </summary>
        /// <param name="graphics">The graphics to draw to.</param>
        /// <param name="transform">The transform to apply to the line.</param>
        void Draw([NotNull] IGraphics graphics, Matrix3x2 transform);

        /// <summary>
        ///     Tests whether the given point is within the given tolerance on this line after it has been transformed by the given
        ///     <paramref name="transform" />,
        ///     returning the exact point on the line if hit.
        /// </summary>
        /// <param name="point">The point to test.</param>
        /// <param name="tolerance">The tolerance. Must be greater than 0.</param>
        /// <param name="transform">The transform to apply to the line.</param>
        /// <returns>The exact point on the line if hit; otherwise <see langword="null" />.</returns>
        [CanBeNull]
        LinePoint HitTest(Vector2 point, float tolerance, Matrix3x2 transform);

        /// <summary>
        ///     Splits the line into two lines at the distance along the line given.
        /// </summary>
        /// <param name="distance">The distance along the line to split the line, in the range 0-1 (exclusive).</param>
        /// <param name="line1">The first line.</param>
        /// <param name="line2">The second line.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">The distance must be in the range 0-1.</exception>
        void SplitLine(float distance, [NotNull] out ILine line1, [NotNull] out ILine line2);

        /// <summary>
        ///     Gets the tangent vector at the distance along the line given.
        /// </summary>
        /// <param name="distance">The distance along the line to get the tangent of, in the range 0-1 (inclusive).</param>
        /// <param name="transform">The transform to apply to the line.</param>
        /// <returns>The tagent vector at the distance along the line.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">The distance must be in the range 0-1.</exception>
        Vector2 GetTangent(float distance, Matrix3x2 transform);
    }
}