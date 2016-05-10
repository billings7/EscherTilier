using System;
using System.Collections.Generic;
using System.Diagnostics;
using JetBrains.Annotations;

namespace EscherTiler.Styles
{
    /// <summary>
    ///     Style manager that just uses a single style.
    /// </summary>
    /// <seealso cref="EscherTiler.Styles.StyleManager" />
    public class SimpleStyleManager : StyleManager
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="StyleManager" /> class.
        /// </summary>
        /// <param name="lineStyle">The line style.</param>
        /// <param name="styles">The styles.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public SimpleStyleManager([NotNull] LineStyle lineStyle, [CanBeNull] IReadOnlyCollection<TileStyle> styles)
            : base(lineStyle, styles) { }

        /// <summary>
        ///     Initializes a new instance of the <see cref="StyleManager" /> class.
        /// </summary>
        /// <param name="lineStyle">The line style.</param>
        /// <param name="styles">The styles.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public SimpleStyleManager([NotNull] LineStyle lineStyle, [CanBeNull] params TileStyle[] styles)
            : base(lineStyle, styles) { }

        /// <summary>
        ///     Adds the specified style.
        /// </summary>
        /// <param name="style">The style.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public override void Add(TileStyle style)
        {
            foreach (Shape shape in style.Shapes)
            {
                HashSet<TileStyle> styles;
                if (StylesByShape.TryGetValue(shape, out styles) && styles.Count > 0)
                    throw new ArgumentException("A SimpleStyleManager can only contain a single style for each shape.");
            }

            base.Add(style);
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
            Debug.Assert(styles.Length == 1);
            return styles[0];
        }
    }
}