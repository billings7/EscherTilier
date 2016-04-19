using System.Numerics;
using EscherTilier.Graphics;
using EscherTilier.Numerics;
using System;
using System.Runtime.CompilerServices;

namespace EscherTilier
{
    /// <summary>
    /// Defines a quadratic bezier curve.
    /// </summary>
    public class QuadraticBezierCurve : ILine
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QuadraticBezierCurve"/> class.
        /// </summary>
        /// <param name="start">The start point.</param>
        /// <param name="controlPoint">The control point.</param>
        /// <param name="end">The end point.</param>
        public QuadraticBezierCurve(Vector2 start, Vector2 controlPoint, Vector2 end)
        {
            Start = start;
            ControlPoint = controlPoint;
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
        /// Gets the control point.
        /// </summary>
        /// <value>
        /// The control point.
        /// </value>
        public Vector2 ControlPoint { get; }

        /// <summary>
        /// Gets the approximate bounds for this line after it has been transformed by the given <paramref name="transform" />.
        /// The rectangle returned should equal or contain the actual bounds.
        /// </summary>
        /// <param name="transform">The transform to apply to the line.</param>
        /// <returns></returns>
        public Rectangle GetApproximateBounds(Matrix3x2 transform)
        {
            return new Rectangle(Vector2.Transform(Start, transform), Vector2.Zero)
                .Expand(Vector2.Transform(End, transform))
                .Expand(Vector2.Transform(ControlPoint, transform));
        }

        /// <summary>
        /// Adds the line to the given <paramref name="path" /> after transforming it by the given <paramref name="transform" />.
        /// </summary>
        /// <param name="path">The path to add the line to.</param>
        /// <param name="transform">The transform.</param>
        public void AddToPath(IGraphicsPath path, Matrix3x2 transform)
        {
            path.AddQuadraticBezier(
                Vector2.Transform(ControlPoint, transform),
                Vector2.Transform(End, transform));
        }

        /// <summary>
        /// Draws the line to the given <paramref name="graphics" /> after transforming it by the given <paramref name="transform" />.
        /// </summary>
        /// <param name="graphics">The graphics to draw to.</param>
        /// <param name="transform">The transform.</param>
        public void Draw(IGraphics graphics, Matrix3x2 transform)
        {
            graphics.DrawQuadraticBezier(
                Vector2.Transform(Start, transform),
                Vector2.Transform(ControlPoint, transform),
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
            Vector2 b = Vector2.Transform(ControlPoint, transform);
            Vector2 c = Vector2.Transform(End, transform);

            Rectangle bounds = new Rectangle(a, Vector2.Zero).Expand(b).Expand(c);
            bounds = new Rectangle(bounds.X - tolerance, bounds.Y - tolerance,
                bounds.Width + tolerance * 2, bounds.Height + tolerance * 2);

            if (!bounds.Contains(point))
                return null;

            float approxLen =
                Vector2.Distance(a, b) +
                Vector2.Distance(b, c);

            Vector2 closest = Vector2.Zero;
            float closestDistSq = float.PositiveInfinity;
            float cloestT = 0;

            float step = tolerance / approxLen;

            for (float t = 0; ; t += step)
            {
                if (t > 1) t = 1;

                Vector2 p = GetPointOnCurve(t, a, b, c);
                float distSq = Vector2.DistanceSquared(p, point);

                if (distSq < closestDistSq)
                {
                    closest = p;
                    cloestT = t;
                    closestDistSq = distSq;
                }

                if (t == 1) break;
            }

            float tolSq = tolerance * tolerance;

            if (closestDistSq > tolSq)
                return null;

            return new LinePoint(closest, cloestT);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Vector2 GetPointOnCurve(float t, Vector2 a, Vector2 b, Vector2 c)
        {
            float it = 1 - t;
            return (it * it * a) + (2 * it * t * b) + (t * t * c);
        }
    }
}