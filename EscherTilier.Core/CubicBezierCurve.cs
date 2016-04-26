using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using EscherTilier.Graphics;
using EscherTilier.Numerics;
using JetBrains.Annotations;

namespace EscherTilier
{
    /// <summary>
    ///     Defines a cubic bezier curve.
    /// </summary>
    public class CubicBezierCurve : ILine
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="CubicBezierCurve" /> class.
        /// </summary>
        /// <param name="start">The start point.</param>
        /// <param name="controlPointA">The first control point.</param>
        /// <param name="controlPointB">The second control point.</param>
        /// <param name="end">The end point.</param>
        public CubicBezierCurve(
            [NotNull] LineVector start,
            [NotNull] LineVector controlPointA,
            [NotNull] LineVector controlPointB,
            [NotNull] LineVector end)
        {
            if (start == null) throw new ArgumentNullException(nameof(start));
            if (controlPointA == null) throw new ArgumentNullException(nameof(controlPointA));
            if (controlPointB == null) throw new ArgumentNullException(nameof(controlPointB));
            if (end == null) throw new ArgumentNullException(nameof(end));
            Start = start;
            ControlPointA = controlPointA;
            ControlPointB = controlPointB;
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
        ///     Gets the first control point.
        /// </summary>
        /// <value>
        ///     The first control point.
        /// </value>
        public LineVector ControlPointA { get; }

        /// <summary>
        ///     Gets the second control point.
        /// </summary>
        /// <value>
        ///     The second control point.
        /// </value>
        public LineVector ControlPointB { get; }

        /// <summary>
        ///     Gets the points that are used to define this line.
        /// </summary>
        /// <value>
        ///     The points.
        /// </value>
        public IEnumerable<LineVector> Points
        {
            get
            {
                yield return Start;
                yield return ControlPointA;
                yield return ControlPointB;
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
                Vector2.Transform(ControlPointA, transform),
                Vector2.Transform(ControlPointB, transform));
        }

        /// <summary>
        ///     Adds the line to the given <paramref name="path" /> after transforming it by the given
        ///     <paramref name="transform" />.
        /// </summary>
        /// <param name="path">The path to add the line to.</param>
        /// <param name="transform">The transform.</param>
        /// <param name="reverse">If set to <see langword="true"/>, add the line from <see cref="Start"/> to <see cref="End"/>.</param>
        public void AddToPath(IGraphicsPath path, Matrix3x2 transform, bool reverse)
        {
            if (path == null) throw new ArgumentNullException(nameof(path));

            path.AddCubicBezier(
                Vector2.Transform(reverse ? ControlPointB : ControlPointA, transform),
                Vector2.Transform(reverse ? ControlPointA : ControlPointB, transform),
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
            if (graphics == null) throw new ArgumentNullException(nameof(graphics));

            graphics.DrawCubicBezier(
                Vector2.Transform(Start, transform),
                Vector2.Transform(ControlPointA, transform),
                Vector2.Transform(ControlPointB, transform),
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
            Vector2 b = Vector2.Transform(ControlPointA, transform);
            Vector2 c = Vector2.Transform(ControlPointB, transform);
            Vector2 d = Vector2.Transform(End, transform);
            
            if (!float.IsPositiveInfinity(tolerance))
            {
                Rectangle bounds = Rectangle.ContainingPoints(a, b, c, d);
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
                Vector2.Distance(b, c) +
                Vector2.Distance(c, d);

            Vector2 closest = Vector2.Zero;
            float closestDistSq = float.PositiveInfinity;
            float cloestT = 0;

            float step = tolerance / approxLen;

            for (float t = 0;; t += step)
            {
                if (t > 1) t = 1;

                Vector2 p = GetPointOnCurve(t, a, b, c, d);
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
        private static Vector2 GetPointOnCurve(float t, Vector2 a, Vector2 b, Vector2 c, Vector2 d)
        {
            float it = 1 - t;
            return (it * it * it * a) + (3 * it * it * t * b) + (3 * it * t * t * c) + (t * t * t * d);
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
            Vector2 p2 = ControlPointA;
            Vector2 p3 = ControlPointB;
            Vector2 p4 = End;

            Vector2 p12 = ((p2 - p1) * distance) + p1;
            Vector2 p23 = ((p3 - p2) * distance) + p2;
            Vector2 p34 = ((p4 - p3) * distance) + p3;

            Vector2 p123 = ((p23 - p12) * distance) + p12;
            Vector2 p234 = ((p34 - p23) * distance) + p23;

            Vector2 p1234 = ((p234 - p123) * distance) + p123;

            LineVector midVector = new LineVector(p1234);

            line1 = new CubicBezierCurve(Start, new LineVector(p12), new LineVector(p123), midVector);
            line2 = new CubicBezierCurve(midVector, new LineVector(p234), new LineVector(p34), End);
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
            Vector2 b = Vector2.Transform(ControlPointA, transform);
            Vector2 c = Vector2.Transform(ControlPointB, transform);
            Vector2 d = Vector2.Transform(End, transform);

            return GetTangent(distance, a, b, c, d);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Vector2 GetTangent(float t, Vector2 a, Vector2 b, Vector2 c, Vector2 d)
        {
            float it = 1 - t;
            return (3 * it * it * (b - a)) + (6 * it * t * (c - b)) + (3 * t * t * (d - c));
        }
    }
}