using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using JetBrains.Annotations;

namespace EscherTiler.Utilities
{
    /// <summary>
    ///     Random number helper for tilings.
    /// </summary>
    public static class TileRandom
    {
        [NotNull]
        private static readonly MD5 _md5 = MD5.Create();

        /// <summary>
        ///     Gets a pseudo random value pased on the seed and X and Y coordinates given.
        /// </summary>
        /// <param name="seed">The seed.</param>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <returns></returns>
        public static float Random(int seed, float x, float y)
        {
            U u = new U(seed, x, y);

            byte[] bytes = _md5.ComputeHash(u.Get12Bytes());

            u = new U(bytes);
            return ((long)u.UInt1 + u.UInt2 + u.UInt3 + u.UInt4) / (float) (1L << 34);
        }

        [StructLayout(LayoutKind.Explicit)]
        private struct U
        {
            private static readonly U _default = default(U);

            [UsedImplicitly]
            [FieldOffset(0)]
            public int Seed;

            [UsedImplicitly]
            [FieldOffset(sizeof(int))]
            public float X;

            [UsedImplicitly]
            [FieldOffset(sizeof(int) + sizeof(float))]
            public float Y;

            [UsedImplicitly]
            [FieldOffset(0)]
            public byte B0;

            [UsedImplicitly]
            [FieldOffset(1)]
            public byte B1;

            [UsedImplicitly]
            [FieldOffset(2)]
            public byte B2;

            [UsedImplicitly]
            [FieldOffset(3)]
            public byte B3;

            [UsedImplicitly]
            [FieldOffset(4)]
            public byte B4;

            [UsedImplicitly]
            [FieldOffset(5)]
            public byte B5;

            [UsedImplicitly]
            [FieldOffset(6)]
            public byte B6;

            [UsedImplicitly]
            [FieldOffset(7)]
            public byte B7;

            [UsedImplicitly]
            [FieldOffset(8)]
            public byte B8;

            [UsedImplicitly]
            [FieldOffset(9)]
            public byte B9;

            [UsedImplicitly]
            [FieldOffset(10)]
            public byte B10;

            [UsedImplicitly]
            [FieldOffset(11)]
            public byte B11;

            [UsedImplicitly]
            [FieldOffset(12)]
            public byte B12;

            [UsedImplicitly]
            [FieldOffset(13)]
            public byte B13;

            [UsedImplicitly]
            [FieldOffset(14)]
            public byte B14;

            [UsedImplicitly]
            [FieldOffset(15)]
            public byte B15;

            [FieldOffset(0)]
            public uint UInt1;

            [FieldOffset(1 * sizeof(uint))]
            public uint UInt2;

            [FieldOffset(2 * sizeof(uint))]
            public uint UInt3;

            [FieldOffset(3 * sizeof(uint))]
            public uint UInt4;

            public U(int seed, float x, float y)
            {
                this = _default;
                Seed = seed;
                X = x;
                Y = y;
            }

            public U([NotNull] byte[] bytes)
            {
                Debug.Assert(bytes != null, "bytes != null");
                this = _default;
                B0 = bytes[0];
                B1 = bytes[1];
                B2 = bytes[2];
                B3 = bytes[3];
                B4 = bytes[4];
                B5 = bytes[5];
                B6 = bytes[6];
                B7 = bytes[7];
                B8 = bytes[8];
                B9 = bytes[9];
                B10 = bytes[10];
                B11 = bytes[11];
                B12 = bytes[12];
                B13 = bytes[13];
                B14 = bytes[14];
                B15 = bytes[15];
            }

            [NotNull]
            public byte[] Get12Bytes()
            {
                return new[]
                {
                    B0,
                    B1,
                    B2,
                    B3,
                    B4,
                    B5,
                    B6,
                    B7,
                    B8,
                    B9,
                    B10,
                    B11,
                    B12
                };
            }
        }
    }
}