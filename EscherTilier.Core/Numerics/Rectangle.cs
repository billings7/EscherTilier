using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using JetBrains.Annotations;

namespace EscherTilier.Numerics
{
    /// <summary>
    ///     Defines a rectangle.
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    public struct Rectangle : IEquatable<Rectangle>
    {
        /// <summary>
        ///     The location of the rectangle.
        /// </summary>
        [FieldOffset(0)]
        public readonly Vector2 Location;

        /// <summary>
        ///     The size of the rectangle.
        /// </summary>
        [FieldOffset(2 * sizeof(float))]
        public readonly Vector2 Size;

        /// <summary>
        ///     The X co-ordinate of the top left corner of the rectangle.
        /// </summary>
        [FieldOffset(0)]
        public readonly float X;

        /// <summary>
        ///     The Y co-ordinate of the top left corner of the rectangle.
        /// </summary>
        [FieldOffset(sizeof(float))]
        public readonly float Y;

        /// <summary>
        ///     The width of the rectangle.
        /// </summary>
        [FieldOffset(2 * sizeof(float))]
        public readonly float Width;

        /// <summary>
        ///     The height of the rectangle.
        /// </summary>
        [FieldOffset(3 * sizeof(float))]
        public readonly float Height;

        /// <summary>
        ///     Initializes a new instance of the <see cref="Rectangle" /> struct.
        /// </summary>
        /// <param name="location">The location.</param>
        /// <param name="size">The size.</param>
        public Rectangle(Vector2 location, Vector2 size)
        {
            this = default(Rectangle);
            Location = location;
            Size = size;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Rectangle" /> struct.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public Rectangle(float x, float y, float width, float height)
        {
            this = default(Rectangle);
            Location = new Vector2(x, y);
            Size = new Vector2(width, height);
        }

        /// <summary>
        /// Creates a rectangle that contains the points given.
        /// </summary>
        /// <param name="point1">The first point.</param>
        /// <param name="point2">The second point.</param>
        /// <returns></returns>
        public static Rectangle ContainingPoints(Vector2 point1, Vector2 point2)
        {
            float x1 = Math.Min(point1.X, point2.X);
            float x2 = Math.Max(point1.X, point2.X);
            float y1 = Math.Min(point1.Y, point2.Y);
            float y2 = Math.Max(point1.Y, point2.Y);

            return new Rectangle(x1, y1, x2 - x1, y2 - y1);
        }

        /// <summary>
        /// Creates a rectangle that contains the points given.
        /// </summary>
        /// <param name="point1">The first point.</param>
        /// <param name="point2">The second point.</param>
        /// <param name="point3">The third point.</param>
        /// <returns></returns>
        public static Rectangle ContainingPoints(Vector2 point1, Vector2 point2, Vector2 point3)
        {
            float x1 = Math.Min(point1.X, Math.Min(point2.X, point3.X));
            float x2 = Math.Max(point1.X, Math.Max(point2.X, point3.X));
            float y1 = Math.Min(point1.Y, Math.Min(point2.Y, point3.Y));
            float y2 = Math.Max(point1.Y, Math.Max(point2.Y, point3.Y));

            return new Rectangle(x1, y1, x2 - x1, y2 - y1);
        }

        /// <summary>
        /// Creates a rectangle that contains the points given.
        /// </summary>
        /// <param name="point1">The first point.</param>
        /// <param name="point2">The second point.</param>
        /// <param name="point3">The third point.</param>
        /// <param name="point4">The fourth point.</param>
        /// <returns></returns>
        public static Rectangle ContainingPoints(Vector2 point1, Vector2 point2, Vector2 point3, Vector2 point4)
        {
            float x1 = Math.Min(Math.Min(point1.X, point2.X), Math.Min(point3.X, point4.X));
            float x2 = Math.Max(Math.Max(point1.X, point2.X), Math.Max(point3.X, point4.X));
            float y1 = Math.Min(Math.Min(point1.Y, point2.Y), Math.Min(point3.Y, point4.Y));
            float y2 = Math.Max(Math.Max(point1.Y, point2.Y), Math.Max(point3.Y, point4.Y));

            return new Rectangle(x1, y1, x2 - x1, y2 - y1);
        }

        /// <summary>
        /// Creates a rectangle that contains all the points given.
        /// </summary>
        /// <param name="points">The points the rectangle should contain.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="System.ArgumentException">Need at least two points.</exception>
        public static Rectangle ContainingPoints([NotNull] params Vector2[] points)
        {
            if (points == null) throw new ArgumentNullException(nameof(points));
            if (points.Length < 2) throw new ArgumentException("Need at least two points.", nameof(points));

            float x1 = points[0].X;
            float x2 = points[0].X;
            float y1 = points[0].Y;
            float y2 = points[0].Y;

            for (int i = 1; i < points.Length; i++)
            {
                Vector2 p = points[i];

                if (p.X < x1) x1 = p.X;
                else if (p.X > x2) x2 = p.X;

                if (p.Y < y1) y1 = p.Y;
                else if (p.Y > y2) y2 = p.Y;
            }

            return new Rectangle(x1, y1, x2 - x1, y2 - y1);
        }

        /// <summary>
        ///     Gets the position of the left side of the rectangle.
        /// </summary>
        public float Left => X;

        /// <summary>
        ///     Gets the position of the top side of the rectangle.
        /// </summary>
        public float Top => Y;

        /// <summary>
        ///     Gets the position of the right side of the rectangle.
        /// </summary>
        public float Right => X + Width;

        /// <summary>
        ///     Gets the position of the bottom side of the rectangle.
        /// </summary>
        public float Bottom => Y + Height;

        /// <summary>
        ///     Determines whether this rectangle contains the given point.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.Pure]
        public bool Contains(Vector2 point)
            => Left >= point.X && point.X >= Right && Top >= point.Y && point.Y >= Bottom;

        /// <summary>
        ///     Determines if this rectangle intersects with <paramref name="rect" />.
        /// </summary>
        /// <param name="rect">The rect.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.Pure]
        public bool IntersectsWith(Rectangle rect)
        {
            return (rect.X < X + Width) &&
                   (X < (rect.X + rect.Width)) &&
                   (rect.Y < Y + Height) &&
                   (Y < rect.Y + rect.Height);
        }

        /// <summary>
        /// Gets a <see cref="Rectangle"/> structure that contains the union of two <see cref="Rectangle"/> structures.
        /// </summary>
        /// <param name="a">A rectangle to union.</param>
        /// <param name="b">A rectangle to union.</param>
        /// <returns>A <see cref="Rectangle"/> structure that bounds the union of the two <see cref="Rectangle"/> structures.</returns>
        [System.Diagnostics.Contracts.Pure]
        public static Rectangle Union(Rectangle a, Rectangle b)
        {
            float x1 = Math.Min(a.X, b.X);
            float x2 = Math.Max(a.X + a.Width, b.X + b.Width);
            float y1 = Math.Min(a.Y, b.Y);
            float y2 = Math.Max(a.Y + a.Height, b.Y + b.Height);

            return new Rectangle(x1, y1, x2 - x1, y2 - y1);
        }

        /// <summary>
        /// Gets a copy of this <see cref="Rectangle" /> structure that contains the point given.
        /// </summary>
        /// <param name="point">The point the rectangle should be expanded to include.</param>
        /// <returns>
        /// A <see cref="Rectangle" /> structure that contains both this rectangle and the point given.
        /// </returns>
        [System.Diagnostics.Contracts.Pure]
        public Rectangle Expand(Vector2 point)
        {
            float x1 = Math.Min(X, point.X);
            float x2 = Math.Max(X + Width, point.X);
            float y1 = Math.Min(Y, point.Y);
            float y2 = Math.Max(Y + Height, point.Y);

            return new Rectangle(x1, y1, x2 - x1, y2 - y1);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(Rectangle other)
        {
            return X.Equals(other.X) && Y.Equals(other.Y) && Width.Equals(other.Width) && Height.Equals(other.Height);
        }

        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <returns>
        /// true if <paramref name="obj"/> and this instance are the same type and represent the same value; otherwise, false.
        /// </returns>
        /// <param name="obj">The object to compare with the current instance. </param>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Rectangle && Equals((Rectangle) obj);
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer that is the hash code for this instance.
        /// </returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = X.GetHashCode();
                hashCode = (hashCode * 397) ^ Y.GetHashCode();
                hashCode = (hashCode * 397) ^ Width.GetHashCode();
                hashCode = (hashCode * 397) ^ Height.GetHashCode();
                return hashCode;
            }
        }

        public static bool operator ==(Rectangle left, Rectangle right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Rectangle left, Rectangle right)
        {
            return !left.Equals(right);
        }
    }
}