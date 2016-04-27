using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using EscherTiler.Graphics;
using EscherTiler.Numerics;

namespace EscherTiler
{
    // TODO Too complicated to work out bounding box for now
    /// <summary>
    /// Defines an arc of an ellipse.
    /// </summary>
    public class Arc : ILine
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Arc" /> class.
        /// </summary>
        /// <param name="start">The start point.</param>
        /// <param name="end">The end point.</param>
        /// <param name="radius">The radius of the arc.</param>
        /// <param name="angle">The angle of the arc, in radians.</param>
        /// <param name="clockwise">If set to <see langword="true" /> the arc will be drawn clockwise.</param>
        /// <param name="isLarge">Specifies whether the arc is larger than 180 degrees.</param>
        public Arc(Vector2 start, Vector2 end, Vector2 radius, float angle, bool clockwise, bool isLarge)
        {
            Start = start;
            End = end;
            Radius = radius;
            Angle = angle;
            Clockwise = clockwise;
            IsLarge = isLarge;
        }

        /// <summary>
        ///     Gets the start point of the arc.
        /// </summary>
        /// <value>
        ///     The start point.
        /// </value>
        public Vector2 Start { get; }

        /// <summary>
        ///     Gets the end point of the arc.
        /// </summary>
        /// <value>
        ///     The end point.
        /// </value>
        public Vector2 End { get; }

        /// <summary>
        ///     Gets the radius of the arc in each axis.
        /// </summary>
        /// <value>
        ///     The radius.
        /// </value>
        public Vector2 Radius { get; }

        /// <summary>
        ///     Gets the angle of the arc, in radians.
        /// </summary>
        /// <value>
        ///     The angle.
        /// </value>
        public float Angle { get; }

        /// <summary>
        ///     Gets a value indicating whether this <see cref="Arc" /> should be drawn clockwise.
        /// </summary>
        /// <value>
        ///     <see langword="true" /> if clockwise; otherwise, <see langword="false" />.
        /// </value>
        public bool Clockwise { get; }

        /// <summary>
        ///     Gets a value indicating whether this <see cref="Arc" /> is larger than 180 degrees.
        /// </summary>
        /// <value>
        ///     <see langword="true" /> if this <see cref="Arc" /> is larger than 180 degrees; otherwise, <see langword="false" />.
        /// </value>
        public bool IsLarge { get; }

        /// <summary>
        ///     Gets the approximate bounds for this line after it has been transformed by the given <paramref name="transform" />.
        ///     The rectangle returned should equal or contain the actual bounds.
        /// </summary>
        /// <param name="transform">The transform to apply to the line.</param>
        /// <returns></returns>
        public Rectangle GetApproximateBounds(Matrix3x2 transform)
        {






            Vector2 start = Vector2.Transform(Start, transform);
            Vector2 end = Vector2.Transform(End, transform);

            Vector2 zero = Vector2.Transform(Vector2.Zero, transform);
            Vector2 rad = Vector2.Transform(Radius, transform);
            rad = Radius * (Vector2.Distance(zero, rad) / Radius.Length());

            return new Rectangle(start, Vector2.Zero)
                .Expand(end)
                .Expand(start + rad)
                .Expand(end + rad)
                .Expand(start - rad)
                .Expand(end - rad);
        }

        /// <summary>
        ///     Adds the line to the given <paramref name="path" /> after transforming it by the given
        ///     <paramref name="transform" />.
        /// </summary>
        /// <param name="path">The path to add the line to.</param>
        /// <param name="transform">The transform.</param>
        public void AddToPath(IGraphicsPath path, Matrix3x2 transform)
        {
            Vector2 zero = Vector2.Transform(Vector2.Zero, transform);
            Vector2 rad = Vector2.Transform(Radius, transform);
            rad = Radius * (Vector2.Distance(zero, rad) / Radius.Length());

            Vector2 angVec = Vector2.Transform(new Vector2(0, 1), Matrix3x2.CreateRotation(Angle) * transform);
            float ang = AngleBetween(new Vector2(0, 1), angVec - zero);

            path.AddArc(
                Vector2.Transform(End, transform),
                rad,
                ang,
                Clockwise /* TODO probably need to work this out somehow */,
                IsLarge);
        }

        /// <summary>
        ///     Draws the line to the given <paramref name="graphics" /> after transforming it by the given
        ///     <paramref name="transform" />.
        /// </summary>
        /// <param name="graphics">The graphics to draw to.</param>
        /// <param name="transform">The transform.</param>
        public void Draw(IGraphics graphics, Matrix3x2 transform)
        {
            Vector2 zero = Vector2.Transform(Vector2.Zero, transform);
            Vector2 rad = Vector2.Transform(Radius, transform);
            rad = Radius * (Vector2.Distance(zero, rad) / Radius.Length());

            Vector2 angVec = Vector2.Transform(new Vector2(0, 1), Matrix3x2.CreateRotation(Angle) * transform);
            float ang = AngleBetween(new Vector2(0, 1), angVec - zero);

            graphics.DrawArc(
                Vector2.Transform(Start, transform),
                Vector2.Transform(End, transform),
                rad,
                ang,
                Clockwise /* TODO probably need to work this out somehow */,
                IsLarge);
        }

        /// <summary>
        ///     Gets the angle between two vectors.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float AngleBetween(Vector2 vector1, Vector2 vector2)
        {
            float sin = vector1.X * vector2.Y - vector2.X * vector1.Y;
            float cos = vector1.X * vector2.X + vector2.Y * vector1.Y;

            return (float) Math.Atan2(sin, cos);
        }
    }
}