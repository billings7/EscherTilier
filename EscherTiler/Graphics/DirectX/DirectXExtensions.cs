using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using EscherTiler.Styles;
using SharpDX;
using SharpDX.Mathematics.Interop;
using Vector2 = System.Numerics.Vector2;

namespace EscherTiler.Graphics.DirectX
{
    public static class DirectXExtensions
    {
        private static readonly Union _union = new Union();

        [SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
        static DirectXExtensions()
        {
            Union u = _union;
            u.Vector2 = new Vector2(1f, 2f);
            if (u.RawVector2.X != 1f || u.RawVector2.Y != 2f) throw new ApplicationException();
            if (u.Size2F.Width != 1f || u.Size2F.Height != 2f) throw new ApplicationException();

            u.Colour = new Colour(0.1f, 0.2f, 0.3f, 0.4f);
            if (u.RawColor4.R != 0.1f || u.RawColor4.G != 0.2f || u.RawColor4.B != 0.3f || u.RawColor4.A != 0.4f)
                throw new ApplicationException();

            u.Matrix3x2 = new System.Numerics.Matrix3x2(1f, 2f, 3f, 4f, 5f, 6f);
            if (u.RawMatrix3x2.M11 != 1f || u.RawMatrix3x2.M12 != 2f ||
                u.RawMatrix3x2.M21 != 3f || u.RawMatrix3x2.M22 != 4f ||
                u.RawMatrix3x2.M31 != 5f || u.RawMatrix3x2.M32 != 6f)
                throw new ApplicationException();
        }

        /// <summary>
        ///     Converts a <see cref="System.Numerics.Vector2" /> to a DirectX <see cref="RawVector2" />.
        /// </summary>
        /// <param name="vector">The vector.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RawVector2 ToRawVector2(this Vector2 vector)
        {
            Union u = _union;
            u.Vector2 = vector;
            return u.RawVector2;
        }

        /// <summary>
        ///     Converts a <see cref="Vector2" /> to a DirectX <see cref="Size2" />.
        /// </summary>
        /// <param name="vector">The vector.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Size2F ToSize2F(this Vector2 vector)
        {
            Union u = _union;
            u.Vector2 = vector;
            return u.Size2F;
        }

        /// <summary>
        ///     Converts a <see cref="Colour" /> to a DirectX <see cref="RawColor4" />.
        /// </summary>
        /// <param name="colour">The colour.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RawColor4 ToRawColor4(this Colour colour)
        {
            Union u = _union;
            u.Colour = colour;
            return u.RawColor4;
        }

        /// <summary>
        ///     Converts a <see cref="System.Numerics.Matrix3x2" /> to a DirectX <see cref="RawMatrix3x2" />.
        /// </summary>
        /// <param name="matrix">The matrix.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RawMatrix3x2 ToRawMatrix3x2(this System.Numerics.Matrix3x2 matrix)
        {
            Union u = _union;
            u.Matrix3x2 = matrix;
            return u.RawMatrix3x2;
        }

        /// <summary>
        ///     Converts a DirectX <see cref="RawMatrix3x2" /> to a <see cref="System.Numerics.Matrix3x2" />.
        /// </summary>
        /// <param name="matrix">The matrix.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static System.Numerics.Matrix3x2 ToMatrix3x2(this RawMatrix3x2 matrix)
        {
            Union u = _union;
            u.RawMatrix3x2 = matrix;
            return u.Matrix3x2;
        }

        /// <summary>
        ///     Converts a <see cref="Numerics.Rectangle" /> to a DirectX <see cref="RawRectangleF" />.
        /// </summary>
        /// <param name="rect">The rectangle.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RawRectangleF ToRawRectangleF(this Numerics.Rectangle rect)
        {
            return new RawRectangleF(rect.Left, rect.Top, rect.Right, rect.Bottom);
        }

        /// <summary>
        ///     Converts a <see cref="GradientStop" /> to a DirectX <see cref="SharpDX.Direct2D1.GradientStop" />.
        /// </summary>
        /// <param name="gradientStop">The gradient stop.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SharpDX.Direct2D1.GradientStop ToGradientStop(this GradientStop gradientStop)
        {
            return new SharpDX.Direct2D1.GradientStop
            {
                Color = gradientStop.Colour.ToRawColor4(),
                Position = gradientStop.Position
            };
        }

        [StructLayout(LayoutKind.Explicit)]
        private struct Union
        {
            [FieldOffset(0)]
            public Vector2 Vector2;

            [FieldOffset(0)]
            public RawVector2 RawVector2;

            [FieldOffset(0)]
            public Size2F Size2F;

            [FieldOffset(0)]
            public RawColor4 RawColor4;

            [FieldOffset(0)]
            public Colour Colour;

            [FieldOffset(0)]
            public System.Numerics.Matrix3x2 Matrix3x2;

            [FieldOffset(0)]
            public RawMatrix3x2 RawMatrix3x2;
        }
    }
}