using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using EscherTilier.Graphics;
using EscherTilier.Numerics;
using JetBrains.Annotations;

namespace EscherTilier
{
    /// <summary>
    ///     Defines a straight line between two points.
    /// </summary>
    public class Line : ILine
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Line" /> class.
        /// </summary>
        /// <param name="start">The start point.</param>
        /// <param name="end">The end point.</param>
        public Line([NotNull] LineVector start, [NotNull] LineVector end)
        {
            if (start == null) throw new ArgumentNullException(nameof(start));
            if (end == null) throw new ArgumentNullException(nameof(end));

            Start = start;
            End = end;
        }

        /// <summary>
        ///     Gets the start point of the line.
        /// </summary>
        /// <value>
        ///     The start point.
        /// </value>
        public LineVector Start { get; }

        /// <summary>
        ///     Gets the end point of the line.
        /// </summary>
        /// <value>
        ///     The end point.
        /// </value>
        public LineVector End { get; }

        /// <summary>
        ///     Gets the points that are used to define this line.
        /// </summary>
        /// <value>
        ///     The points.
        /// </value>
        /// <remarks>At minimum, this should include the <see cref="Start" /> and <see cref="End" /> points.</remarks>
        public IEnumerable<LineVector> Points
        {
            get
            {
                yield return Start;
                yield return End;
            }
        }

        /// <summary>
        ///     Gets the approximate bounds for this line after it has been transformed by the given <paramref name="transform" />.
        ///     The rectangle returned should equal or contain the actual bounds.
        /// </summary>
        /// <param name="transform">The transform to apply to the line.</param>
        /// <returns></returns>
        public Rectangle GetApproximateBounds(Matrix3x2 transform)
        {
            return Rectangle.ContainingPoints(Vector2.Transform(Start, transform), Vector2.Transform(End, transform));
        }

        /// <summary>
        ///     Adds the line to the given <paramref name="path" /> after transforming it by the given
        ///     <paramref name="transform" />.
        /// </summary>
        /// <param name="path">The path to add the line to.</param>
        /// <param name="transform">The transform.</param>
        public void AddToPath(IGraphicsPath path, Matrix3x2 transform)
        {
            path.AddLine(Vector2.Transform(End, transform));
        }

        /// <summary>
        ///     Draws the line to the given <paramref name="graphics" /> after transforming it by the given
        ///     <paramref name="transform" />.
        /// </summary>
        /// <param name="graphics">The graphics to draw to.</param>
        /// <param name="transform">The transform.</param>
        public void Draw(IGraphics graphics, Matrix3x2 transform)
        {
            graphics.DrawLine(
                Vector2.Transform(Start, transform),
                Vector2.Transform(End, transform));
        }

        /// <summary>
        ///     Tests whether the given point is within the given tolerance on this line after it has been transformed by the given
        ///     <paramref name="transform" />,
        ///     returning the exact point on the line if hit.
        /// </summary>
        /// <param name="point">The point to test.</param>
        /// <param name="tolerance">The tolerance. Must be greater than 0.</param>
        /// <param name="transform">The transform.</param>
        /// <returns>The exact point on the line if hit; otherwise <see langword="null" />.</returns>
        public LinePoint HitTest(Vector2 point, float tolerance, Matrix3x2 transform)
        {
            if (tolerance <= 0)
                throw new ArgumentOutOfRangeException(nameof(tolerance));

            Vector2 a = Vector2.Transform(Start, transform);
            Vector2 b = Vector2.Transform(End, transform);

            Rectangle bounds = Rectangle.ContainingPoints(a, b);
            bounds = new Rectangle(
                bounds.X - tolerance,
                bounds.Y - tolerance,
                bounds.Width + tolerance * 2,
                bounds.Height + tolerance * 2);

            if (!bounds.Contains(point))
                return null;

            Vector2 n = Vector2.Normalize(b - a);

            float tmp = Vector2.Dot(a - point, n);
            Vector2 linePt = tmp * n;

            float distSq = ((a - point) - linePt).LengthSquared();

            float tolSq = tolerance * tolerance;

            if (distSq > tolSq)
                return null;

            // tmp is negative at this point
            if (tmp > tolerance) return null;
            if (tmp >= 0) return new LinePoint(a, 0);
            tmp = -tmp;

            float lineLen = Vector2.Distance(a, b);

            if (tmp > lineLen + tolerance) return null;
            if (tmp >= lineLen) return new LinePoint(b, 0);

            return new LinePoint(a - linePt, tmp / lineLen);
        }

        /// <summary>
        ///     Splits the line into two lines at the distance along the line given.
        /// </summary>
        /// <param name="distance">The distance along the line to split the line, in the range 0-1 (exclusive).</param>
        /// <param name="line1">The first line.</param>
        /// <param name="line2">The second line.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">The distance must be in the range 0-1.</exception>
        public void SplitLine(float distance, out ILine line1, out ILine line2)
        {
            if (distance <= 0 || distance >= 1)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(distance),
                    distance,
                    "The distance must be in the range 0-1.");
            }

            Vector2 mid = Start + ((End.Vector - Start.Vector) * distance);

            LineVector midVector = new LineVector(mid);

            line1 = new Line(Start, midVector);
            line2 = new Line(midVector, End);
        }

        /// <summary>
        ///     Gets the tangent vector at the distance along the line given.
        /// </summary>
        /// <param name="distance">The distance along the line to get the tangent of, in the range 0-1 (inclusive).</param>
        /// <param name="transform">The transform to apply to the line.</param>
        /// <returns>
        ///     The tagent vector at the distance along the line.
        /// </returns>
        /// <exception cref="System.ArgumentOutOfRangeException">The distance must be in the range 0-1.</exception>
        public Vector2 GetTangent(float distance, Matrix3x2 transform)
        {
            if (distance < 0 || distance > 1)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(distance),
                    distance,
                    "The distance must be in the range 0-1.");
            }

            Vector2 a = Vector2.Transform(Start, transform);
            Vector2 b = Vector2.Transform(End, transform);

            return b - a;
        }
    }
}