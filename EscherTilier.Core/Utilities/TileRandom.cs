using System.Security.Cryptography;
using JetBrains.Annotations;

namespace EscherTilier.Utilities
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
        public static unsafe float Random(int seed, float x, float y)
        {
            byte[] bytes = new byte[12];
            fixed (byte* ptr = bytes)
            {
                U* uptr = (U*) ptr;
                *uptr = new U(seed, x, y);
            }

            bytes = _md5.ComputeHash(bytes);

            fixed (byte* ptr = bytes)
            {
                uint* intPtr = (uint*) ptr;
                return (intPtr[0] + intPtr[1] + intPtr[2] + intPtr[3]) / (float) (1L << 32);
            }
        }

        private struct U
        {
            [UsedImplicitly]
            public int Seed;

            [UsedImplicitly]
            public float X;

            [UsedImplicitly]
            public float Y;

            public U(int seed, float x, float y)
            {
                Seed = seed;
                X = x;
                Y = y;
            }
        }
    }
}