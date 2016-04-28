using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using EscherTiler.Graphics;
using EscherTiler.Numerics;
using JetBrains.Annotations;

namespace EscherTiler
{
    /// <summary>
    ///     Defines a quadratic bezier curve.
    /// </summary>
    public class QuadraticBezierCurve : ILine
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="QuadraticBezierCurve" /> class.
        /// </summary>
        /// <param name="start">The start point.</param>
        /// <param name="controlPoint">The control point.</param>
        /// <param name="end">The end point.</param>
        public QuadraticBezierCurve(
            [NotNull] LineVector start,
            [NotNull] LineVector controlPoint,
            [NotNull] LineVector end)
        {
            if (start == null) throw new ArgumentNullException(nameof(start));
            if (controlPoint == null) throw new ArgumentNullException(nameof(controlPoint));
            if (end == null) throw new ArgumentNullException(nameof(end));
            Start = start;
            ControlPoint = controlPoint;
            End = end;
        }

        /// <summary>
        ///     Gets the type of the line.
        /// </summary>
        /// <value>
        ///     The type.
        /// </value>
        public LineType Type => LineType.QuadraticBezierCurve;

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
        ///     Gets the control point.
        /// </summary>
        /// <value>
        ///     The control point.
        /// </value>
        [NotNull]
        public LineVector ControlPoint { get; }

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
                yield return ControlPoint;
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
            return Rectangle.ContainingPoints(
                Vector2.Transform(Start, transform),
                Vector2.Transform(End, transform),
                Vector2.Transform(ControlPoint, transform));
        }

        /// <summary>
        ///     Adds the line to the given <paramref name="path" /> after transforming it by the given
        ///     <paramref name="transform" />.
        /// </summary>
        /// <param name="path">The path to add the line to.</param>
        /// <param name="transform">The transform.</param>
        /// <param name="reverse">If set to <see langword="true" />, add the line from <see cref="Start" /> to <see cref="End" />.</param>
        public void AddToPath(IGraphicsPath path, Matrix3x2 transform, bool reverse)
        {
            path.AddQuadraticBezier(
                Vector2.Transform(ControlPoint, transform),
                Vector2.Transform(reverse ? Start : End, transform));
        }

        /// <summary>
        ///     Draws the line to the given <paramref name="graphics" /> after transforming it by the given
        ///     <paramref name="transform" />.
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
            Vector2 b = Vector2.Transform(ControlPoint, transform);
            Vector2 c = Vector2.Transform(End, transform);

            if (!float.IsPositiveInfinity(tolerance))
            {
                Rectangle bounds = Rectangle.ContainingPoints(a, b, c);
                bounds = new Rectangle(
                    bounds.X - tolerance,
                    bounds.Y - tolerance,
                    bounds.Width + tolerance * 2,
                    bounds.Height + tolerance * 2);

                if (!bounds.Contains(point))
                    return null;
            }

            float approxLen =
                Vector2.Distance(a, b) +
                Vector2.Distance(b, c);

            Vector2 closest = Vector2.Zero;
            float closestDistSq = float.PositiveInfinity;
            float cloestT = 0;

            float step = tolerance / approxLen;

            for (float t = 0;; t += step)
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

                // ReSharper disable once CompareOfFloatsByEqualityOperator
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

            Vector2 p1 = Start;
            Vector2 p2 = ControlPoint;
            Vector2 p3 = End;

            Vector2 p12 = ((p2 - p1) * distance) + p1;
            Vector2 p23 = ((p3 - p2) * distance) + p2;

            Vector2 p123 = ((p23 - p12) * distance) + p12;

            LineVector midVector = new LineVector(p123);

            line1 = new QuadraticBezierCurve(Start, new LineVector(p12), midVector);
            line2 = new QuadraticBezierCurve(midVector, new LineVector(p23), End);
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
            Vector2 b = Vector2.Transform(ControlPoint, transform);
            Vector2 c = Vector2.Transform(End, transform);

            return GetTangent(distance, a, b, c);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Vector2 GetTangent(float t, Vector2 a, Vector2 b, Vector2 c)
        {
            float it = 1 - t;
            return (2 * it * (b - a)) + (2 * t * (c - b));
        }
    }
}