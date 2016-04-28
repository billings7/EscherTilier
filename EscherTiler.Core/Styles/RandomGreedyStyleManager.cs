using System;
using System.Collections.Generic;
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

        protected override IStyle GetStyle(TileBase tile, IStyle[] styles)
        {
            throw new NotImplementedException();
        }
    }
}