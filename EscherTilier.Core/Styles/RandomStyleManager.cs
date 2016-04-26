using System.Diagnostics;
using System.Numerics;
using EscherTilier.Graphics.Resources;
using EscherTilier.Utilities;
using JetBrains.Annotations;

namespace EscherTilier.Styles
{
    public class RandomStyleManager : StyleManager
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="RandomStyleManager" /> class.
        /// </summary>
        /// <param name="resourceManager">The resource manager.</param>
        /// <param name="seed">The seed.</param>
        public RandomStyleManager(
            [CanBeNull] IResourceManager resourceManager,
            int seed)
            : base(resourceManager)
        {
            Seed = seed;
        }

        /// <summary>
        ///     Gets the seed of the random number generator.
        /// </summary>
        /// <value>
        ///     The seed.
        /// </value>
        public int Seed { get; }

        /// <summary>
        ///     Gets the style.
        /// </summary>
        /// <param name="tile">The tile.</param>
        /// <param name="styles">The styles.</param>
        /// <returns></returns>
        protected override IStyle GetStyle(TileBase tile, IStyle[] styles)
        {
            Debug.Assert(tile != null, "tile != null");
            Debug.Assert(styles != null, "styles != null");

            if (styles.Length < 1) return null;

            Vector2 coords = tile.Centroid.Round(3);

            float random = TileRandom.Random(Seed, coords.X, coords.Y);

            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (random == 1) return styles[styles.Length - 1];

            return styles[(int) (random * styles.Length)];
        }
    }
}