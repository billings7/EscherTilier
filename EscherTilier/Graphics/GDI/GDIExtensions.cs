﻿using System.Drawing;
using System.Drawing.Drawing2D;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using EscherTilier.Styles;
using JetBrains.Annotations;

namespace EscherTilier.Graphics.GDI
{
    public static class GDIExtensions
    {
        private static readonly Union _union = new Union();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PointF ToPointF(this Vector2 vector)
        {
            Union u = _union;
            u.Vector2 = vector;
            return u.PointF;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 ToVector2(this PointF point)
        {
            Union u = _union;
            u.PointF = point;
            return u.Vector2;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rectangle ToGDIRectangle(this Numerics.Rectangle rectangle)
        {
            Union u = _union;
            u.Rectangle = rectangle;
            return u.GdiRectangle;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix3x2 ToMatrix3x2(this Matrix matrix)
        {
            float[] elements = matrix.Elements;
            return new Matrix3x2(
                elements[0],
                elements[1],
                elements[2],
                elements[3],
                elements[4],
                elements[5]);
        }

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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color ToColor(this Colour colour)
        {
            return Color.FromArgb(colour.ToArgb());
        }

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
            public Rectangle GdiRectangle;

            [FieldOffset(0)]
            public Numerics.Rectangle Rectangle;
        }
    }
}