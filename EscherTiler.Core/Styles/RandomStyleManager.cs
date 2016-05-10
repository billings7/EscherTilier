using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using EscherTiler.Utilities;
using JetBrains.Annotations;

namespace EscherTiler.Styles
{
    /// <summary>
    ///     Style manager that picks a random style for each tile.
    /// </summary>
    /// <seealso cref="EscherTiler.Styles.StyleManager" />
    public class RandomStyleManager : StyleManager
    {
        private int _seed;

        /// <summary>
        ///     Initializes a new instance of the <see cref="RandomStyleManager" /> class.
        /// </summary>
        /// <param name="seed">The seed.</param>
        /// <param name="lineStyle">The line style.</param>
        /// <param name="styles">The styles.</param>
        public RandomStyleManager(
            int seed,
            [NotNull] LineStyle lineStyle,
            [CanBeNull] IReadOnlyCollection<TileStyle> styles)
            : base(lineStyle, styles)
        {
            Seed = seed;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="RandomStyleManager" /> class.
        /// </summary>
        /// <param name="seed">The seed.</param>
        /// <param name="lineStyle">The line style.</param>
        /// <param name="styles">The styles.</param>
        public RandomStyleManager(
            int seed,
            [NotNull] LineStyle lineStyle,
            [CanBeNull] params TileStyle[] styles)
            : base(lineStyle, styles)
        {
            Seed = seed;
        }

        /// <summary>
        ///     Gets the seed of the random number generator.
        /// </summary>
        /// <value>
        ///     The seed.
        /// </value>
        public int Seed
        {
            get { return _seed; }
            set
            {
                if (_seed == value) return;
                _seed = value;

                OnChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        ///     Gets the style for the given tile.
        /// </summary>
        /// <param name="tile">The tile.</param>
        /// <param name="styles">The styles to choose from.</param>
        /// <param name="state">The style state associated with the tile.</param>
        /// <returns></returns>
        protected override IStyle GetStyle(TileBase tile, IStyle[] styles, ref object state)
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