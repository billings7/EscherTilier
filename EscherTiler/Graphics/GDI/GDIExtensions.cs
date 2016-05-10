using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using EscherTiler.Styles;
using JetBrains.Annotations;

namespace EscherTiler.Graphics.GDI
{
    /// <summary>
    ///     Extension methods for interracting with GDI+ related objects and methods.
    /// </summary>
    public static class GDIExtensions
    {
        private static readonly Union _union = new Union();

        /// <summary>
        ///     Converts a <see cref="Vector2" /> to a GDI+ <see cref="PointF" />.
        /// </summary>
        /// <param name="vector">The vector.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PointF ToPointF(this Vector2 vector)
        {
            Union u = _union;
            u.Vector2 = vector;
            return u.PointF;
        }

        /// <summary>
        ///     Converts a GDI+ <see cref="PointF" /> to a <see cref="Vector2" />.
        /// </summary>
        /// <param name="point">The point.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 ToVector2(this PointF point)
        {
            Union u = _union;
            u.PointF = point;
            return u.Vector2;
        }

        /// <summary>
        ///     Converts a GDI+ <see cref="Size" /> to a <see cref="Vector2" />.
        /// </summary>
        /// <param name="size">The size.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 ToVector2(this Size size) => new Vector2(size.Width, size.Height);

        /// <summary>
        ///     Converts a <see cref="Numerics.Rectangle" /> to a GDI+ <see cref="RectangleF" />.
        /// </summary>
        /// <param name="rectangle">The rectangle.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RectangleF ToGDIRectangleF(this Numerics.Rectangle rectangle)
        {
            Union u = _union;
            u.Rectangle = rectangle;
            return u.GdiRectangleF;
        }

        /// <summary>
        ///     Converts a GDI+ <see cref="Rectangle" /> to a <see cref="Numerics.Rectangle" />.
        /// </summary>
        /// <param name="rectangle">The rectangle.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Numerics.Rectangle ToRectangle(this Rectangle rectangle)
        {
            return new Numerics.Rectangle(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
        }

        /// <summary>
        ///     Converts a GDI+ <see cref="RectangleF" /> to a <see cref="Numerics.Rectangle" />.
        /// </summary>
        /// <param name="rectangle">The rectangle.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Numerics.Rectangle ToRectangle(this RectangleF rectangle)
        {
            Union u = _union;
            u.GdiRectangleF = rectangle;
            return u.Rectangle;
        }

        /// <summary>
        ///     Converts a GDI+ <see cref="Matrix" /> to a <see cref="Matrix3x2" />.
        /// </summary>
        /// <param name="matrix">The matrix.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix3x2 ToMatrix3x2([NotNull] this Matrix matrix)
        {
            if (matrix == null) throw new ArgumentNullException(nameof(matrix));
            float[] elements = matrix.Elements;
            return new Matrix3x2(
                elements[0],
                elements[1],
                elements[2],
                elements[3],
                elements[4],
                elements[5]);
        }

        /// <summary>
        ///     Converts a <see cref="Matrix3x2" /> to a GDI+ <see cref="Matrix" />.
        /// </summary>
        /// <param name="matrix">The matrix.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [NotNull]
        public static Matrix ToMatrix(this Matrix3x2 matrix)
        {
            return new Matrix(
                matrix.M11,
                matrix.M12,
                matrix.M21,
                matrix.M22,
                matrix.M31,
                matrix.M32);
        }

        /// <summary>
        ///     Converts a <see cref="Colour" /> to a GDI+ <see cref="Color" />.
        /// </summary>
        /// <param name="colour">The colour.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color ToColor(this Colour colour)
        {
            return Color.FromArgb(colour.ToArgb());
        }

        /// <summary>
        ///     Struct with a number of fields of different types that occupy the same location in memory, to allow efficient
        ///     conversion between types.
        /// </summary>
        [StructLayout(LayoutKind.Explicit)]
        private struct Union
        {
            [FieldOffset(0)]
            public Vector2 Vector2;

            [FieldOffset(0)]
            public PointF PointF;

            [FieldOffset(0)]
            public SizeF SizeF;

            [FieldOffset(0)]
            public RectangleF GdiRectangleF;

            [FieldOffset(0)]
            public Numerics.Rectangle Rectangle;
        }
    }
}