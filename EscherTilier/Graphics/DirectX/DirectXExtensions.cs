using System.Runtime.InteropServices;
using EscherTilier.Styles;
using SharpDX;
using SharpDX.Mathematics.Interop;
using Vector2 = System.Numerics.Vector2;

namespace EscherTilier.Graphics.DirectX
{
    public static class DirectXExtensions
    {
        private static readonly Union _union = new Union();

        /// <summary>
        ///     Converts a <see cref="Vector2" /> to a DirectX <see cref="RawVector2" />.
        /// </summary>
        /// <param name="vector">The vector.</param>
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
        public static Size2F ToSize2F(this Vector2 vector)
        {
            Union u = _union;
            u.Vector2 = vector;
            return u.Size2F;
        }

        /// <summary>
        ///     Converts a <see cref="Colour" /> to a DirectX <see cref="RawColor4" />.
        /// </summary>
        /// <param name="vector">The vector.</param>
        public static RawColor4 ToRawColor4(this Colour colour)
        {
            Union u = _union;
            u.Colour = colour;
            return u.RawColor4;
        }

        /// <summary>
        ///     Converts a <see cref="System.Numerics.Matrix3x2" /> to a DirectX <see cref="Matrix3x2" />.
        /// </summary>
        /// <param name="vector">The vector.</param>
        public static Matrix3x2 ToMatrix3x2(this System.Numerics.Matrix3x2 matrix)
        {
            Union u = _union;
            u.SNMatrix3x2 = matrix;
            return u.Matrix3x2;
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
            public System.Numerics.Matrix3x2 SNMatrix3x2;

            [FieldOffset(0)]
            public Matrix3x2 Matrix3x2;
        }
    }
}