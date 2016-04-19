using System;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace EscherTilier.Numerics
{
    /// <summary>
    ///     Helper methods for matricies.
    /// </summary>
    public static class Matrix
    {
        /// <summary>
        ///     Creates a reflection matrix that reflects along a line defined by two vectors.
        /// </summary>
        /// <param name="to">The first point defining the line to reflect along.</param>
        /// <param name="from">The second point defining the line to reflect along.</param>
        /// <returns>The reflection matrix.</returns>
        public static Matrix3x2 CreateReflection(Vector2 to, Vector2 from)
        {
            Vector2 v2 = to * to;

            float xy2 = 2 * to.X * to.Y;
            if (@from == default(Vector2))
                return new Matrix3x2(v2.X - v2.Y, xy2, xy2, v2.Y - v2.X, 0, 0) * (1 / to.LengthSquared());

            return Matrix3x2.CreateTranslation(-@from)
                   * (new Matrix3x2(v2.X - v2.Y, xy2, xy2, v2.Y - v2.X, 0, 0) * (1 / to.LengthSquared()))
                   * Matrix3x2.CreateTranslation(@from);
        }

        /// <summary>
        ///     Creates a reflection matrix that reflects along a line from the origin to a point given.
        /// </summary>
        /// <param name="to">The point defining the line to reflect along.</param>
        /// <returns>The reflection matrix.</returns>
        public static Matrix3x2 CreateReflection(Vector2 to)
        {
            Vector2 v2 = to * to;
            float xy2 = 2 * to.X * to.Y;
            return new Matrix3x2(v2.X - v2.Y, xy2, xy2, v2.Y - v2.X, 0, 0) * (1 / to.LengthSquared());
        }

        public static Matrix3x2 GetTransform(Vector2 fromStart, Vector2 fromEnd, Vector2 toStart, Vector2 toEnd)
        {
            Vector2 fromVector = fromEnd - fromStart;
            Vector2 toVector = toEnd - toStart;

            float fromLengthSq = Vector2.Dot(fromVector, fromVector);
            float toLengthSq = Vector2.Dot(toVector, toVector);

            float scale = (float) Math.Sqrt(toLengthSq / fromLengthSq);

            float angle = AngleBetween(fromVector, toVector);

            Matrix3x2 transform =
                Matrix3x2.CreateTranslation(-fromStart) *
                Matrix3x2.CreateScale(scale) *
                Matrix3x2.CreateRotation(angle) *
                Matrix3x2.CreateTranslation(toStart);
            return transform;
        }

        /// <summary>
        ///     Gets the angle between two vectors.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float AngleBetween(Vector2 vector1, Vector2 vector2)
        {
            float sin = vector1.X * vector2.Y - vector2.X * vector1.Y;
            float cos = vector1.X * vector2.X + vector2.Y * vector1.Y;

            return (float)Math.Atan2(sin, cos);
        }
    }
}