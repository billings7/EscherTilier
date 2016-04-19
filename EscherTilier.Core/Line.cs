using System.Numerics;
using EscherTilier.Graphics;
using EscherTilier.Numerics;
using System;

namespace EscherTilier
{
    /// <summary>
    /// Defines a straight line between two points.
    /// </summary>
    public class Line : ILine
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Line"/> class.
        /// </summary>
        /// <param name="start">The start point.</param>
        /// <param name="end">The end point.</param>
        public Line(Vector2 start, Vector2 end)
        {
            Start = start;
            End = end;
        }

        /// <summary>
        /// Gets the start point of the line.
        /// </summary>
        /// <value>
        /// The start point.
        /// </value>
        public Vector2 Start { get; }

        /// <summary>
        /// Gets the ens point of the line.
        /// </summary>
        /// <value>
        /// The ens point.
        /// </value>
        public Vector2 End { get; }

        /// <summary>
        /// Gets the approximate bounds for this line after it has been transformed by the given <paramref name="transform" />.
        /// The rectangle returned should equal or contain the actual bounds.
        /// </summary>
        /// <param name="transform">The transform to apply to the line.</param>
        /// <returns></returns>
        public Rectangle GetApproximateBounds(Matrix3x2 transform)
        {
            return Rectangle.ContainingPoints(Vector2.Transform(Start, transform), Vector2.Transform(End, transform));
        }

        /// <summary>
        /// Adds the line to the given <paramref name="path" /> after transforming it by the given <paramref name="transform" />.
        /// </summary>
        /// <param name="path">The path to add the line to.</param>
        /// <param name="transform">The transform.</param>
        public void AddToPath(IGraphicsPath path, Matrix3x2 transform)
        {
            path.AddLine(Vector2.Transform(End, transform));
        }

        /// <summary>
        /// Draws the line to the given <paramref name="graphics" /> after transforming it by the given <paramref name="transform" />.
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
        /// Tests whether the given point is within the given tolerance on this line after it has been transformed by the given <paramref name="transform"/>,
        /// returning the exact point on the line if hit.
        /// </summary>
        /// <param name="point">The point to test.</param>
        /// <param name="tolerance">The tolerance. Must be greater than or equal 0.1.</param>
        /// <param name="transform">The transform.</param>
        /// <returns>The exact point on the line if hit; otherwise <see langword="null"/>.</returns>
        public LinePoint HitTest(Vector2 point, float tolerance, Matrix3x2 transform)
        {
            if (tolerance < 0.1f)
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

            //lbl.Text = $"{tmp} \t {linePt} \t {Math.Sqrt(distSq)}";

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
    }
}