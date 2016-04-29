using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using EscherTiler.Utilities;
using JetBrains.Annotations;

namespace EscherTiler.Styles
{
    public class RandomGreedyStyleManager : RandomStyleManager
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="RandomGreedyStyleManager" /> class.
        /// </summary>
        /// <param name="seed">The seed.</param>
        /// <param name="lineStyle">The line style.</param>
        /// <param name="styles">The styles.</param>
        public RandomGreedyStyleManager(
            int seed,
            [NotNull] LineStyle lineStyle,
            [CanBeNull] IReadOnlyList<TileStyle> styles)
            : base(seed, lineStyle, styles) { }

        /// <summary>
        ///     Initializes a new instance of the <see cref="RandomStyleManager" /> class.
        /// </summary>
        /// <param name="seed">The seed.</param>
        /// <param name="lineStyle">The line style.</param>
        /// <param name="styles">The styles.</param>
        public RandomGreedyStyleManager(int seed, [NotNull] LineStyle lineStyle, [CanBeNull] params TileStyle[] styles)
            : base(seed, lineStyle, styles) { }

        /// <summary>
        ///     Gets the style for the given tile.
        /// </summary>
        /// <param name="tile">The tile.</param>
        /// <param name="styles">The styles to choose from.</param>
        /// <param name="state">The style state associated with the tile.</param>
        /// <returns></returns>
        protected override IStyle GetStyle(TileBase tile, IStyle[] styles, ref object state)
        {
            IStyle[] unusedStyles = styles.Except(tile.AdjacentTiles.Values.Select(t => t.Style)).ToArray();

            if (unusedStyles.Length == 0)
                return base.GetStyle(tile, styles, ref state);

            if (unusedStyles.Length == 1)
            {
                Vector2 coords = tile.Centroid.Round(3);

                float random = TileRandom.Random(Seed, coords.X, coords.Y);

                return random > (1f / styles.Length)
                    ? unusedStyles[0]
                    : base.GetStyle(tile, styles, ref state);
            }

            return base.GetStyle(tile, unusedStyles, ref state);
        }
    }
}