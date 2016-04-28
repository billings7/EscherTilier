using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using EscherTiler.Graphics.Resources;
using JetBrains.Annotations;

namespace EscherTiler.Styles
{
    public abstract class StyleManager : IEnumerable<TileStyle>
    {
        [NotNull]
        [ItemNotNull]
        public IList<TileStyle> Styles => _styles;

        [NotNull]
        public LineStyle LineStyle { get; set; }
        
        [NotNull]
        private readonly List<TileStyle> _styles = new List<TileStyle>();

        public StyleManager()
        {
        }

        public StyleManager([CanBeNull] IEnumerable<TileStyle> styles)
        {
            if (styles != null)
                _styles.AddRange(styles);
        }

        public void Add(TileStyle style) => _styles.Add(style);

        public void Add(params TileStyle[] styles) => _styles.AddRange(styles);

        public void Add(IEnumerable<TileStyle> styles) => _styles.AddRange(styles);

        /// <summary>
        ///     Gets the style for the given tile.
        /// </summary>
        /// <param name="tile">The tile.</param>
        /// <returns>The style for the tile.</returns>
        [NotNull]
        public IStyle GetStyle([NotNull] TileBase tile)
        {
            if (tile == null) throw new ArgumentNullException(nameof(tile));

            IStyle style = GetStyle(tile, _styles.Where(s => s.Shapes.Contains(tile.Shape)).Select(s => s.Style).ToArray());
            if (style == null) throw new InvalidOperationException();

            style = style.Transform(tile.Transform);
            return style;
        }

        /// <summary>
        ///     Gets the style for the given tile.
        /// </summary>
        /// <param name="tile">The tile.</param>
        /// <param name="styles">The styles to choose from.</param>
        /// <returns></returns>
        [NotNull]
        protected abstract IStyle GetStyle([NotNull] TileBase tile, [NotNull] IStyle[] styles);

        public IEnumerator<TileStyle> GetEnumerator() => _styles.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}