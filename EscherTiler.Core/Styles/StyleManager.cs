using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using EscherTiler.Utilities;
using JetBrains.Annotations;

namespace EscherTiler.Styles
{
    public abstract class StyleManager : IEnumerable<TileStyle>
    {
        /// <summary>
        ///     Gets the styles.
        /// </summary>
        /// <value>
        ///     The styles.
        /// </value>
        [NotNull]
        [ItemNotNull]
        public IReadOnlyCollection<TileStyle> Styles => _styles;

        /// <summary>
        ///     Gets or sets the line style.
        /// </summary>
        /// <value>
        ///     The line style.
        /// </value>
        [NotNull]
        public LineStyle LineStyle { get; set; }

        [NotNull]
        private readonly HashSet<TileStyle> _styles = new HashSet<TileStyle>();

        [NotNull]
        protected readonly Dictionary<Shape, HashSet<TileStyle>> StylesByShape =
            new Dictionary<Shape, HashSet<TileStyle>>();

        /// <summary>
        /// Occurs when the any of the fill styles have changed.
        /// </summary>
        public event EventHandler StylesChanged;

        /// <summary>
        ///     Initializes a new instance of the <see cref="StyleManager" /> class.
        /// </summary>
        /// <param name="lineStyle">The line style.</param>
        /// <param name="styles">The styles.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        protected StyleManager([NotNull] LineStyle lineStyle, [CanBeNull] [ItemNotNull] IReadOnlyCollection<TileStyle> styles)
        {
            if (lineStyle == null) throw new ArgumentNullException(nameof(lineStyle));

            LineStyle = lineStyle;
            if (styles != null)
            {
                foreach (TileStyle style in styles)
                    // ReSharper disable once VirtualMemberCallInContructor
                    Add(style);
            }
        }

        /// <summary>
        ///     Adds the specified style.
        /// </summary>
        /// <param name="style">The style.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public virtual void Add([NotNull] TileStyle style)
        {
            if (style == null) throw new ArgumentNullException(nameof(style));
            _styles.Add(style);

            foreach (Shape shape in style.Shapes)
            {
                Debug.Assert(shape != null, "shape != null");

                HashSet<TileStyle> styles = StylesByShape.GetOrAdd(shape, _ => new HashSet<TileStyle>());
                Debug.Assert(styles != null, "styles != null");
                styles.Add(style);
            }

            OnChanged(EventArgs.Empty);
        }

        /// <summary>
        ///     Raises the <see cref="E:Changed" /> event.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected virtual void OnChanged(EventArgs e)
        {
            StylesChanged?.Invoke(this, e);
        }

        /// <summary>
        ///     Gets the styles for the shape given.
        /// </summary>
        /// <param name="shape">The shape.</param>
        /// <returns>The styles that apply to the specified shape.</returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        public IReadOnlyCollection<IStyle> GetStyles([NotNull] Shape shape)
        {
            if (shape == null) throw new ArgumentNullException(nameof(shape));

            HashSet<TileStyle> styles;
            return StylesByShape.TryGetValue(shape, out styles) && styles.Count > 0
                ? styles.Select(s => s.Style).ToArray()
                : Array.Empty<IStyle>();
        }

        /// <summary>
        ///     Gets the style for the given tile.
        /// </summary>
        /// <param name="tile">The tile.</param>
        /// <returns>The style for the tile.</returns>
        [CanBeNull]
        public IStyle GetStyle([NotNull] TileBase tile)
        {
            if (tile == null) throw new ArgumentNullException(nameof(tile));

            HashSet<TileStyle> styles;
            if (!StylesByShape.TryGetValue(tile.Shape, out styles))
                return null;
            Debug.Assert(styles != null, "styles != null");

            IStyle style = GetStyle(tile, styles.Select(s => s.Style).ToArray(), ref tile.StyleState);
            return style?.Transform(tile.Transform);
        }

        /// <summary>
        ///     Gets the style for the given tile.
        /// </summary>
        /// <param name="tile">The tile.</param>
        /// <param name="styles">The styles to choose from.</param>
        /// <param name="state">The style state associated with the tile.</param>
        /// <returns></returns>
        [CanBeNull]
        protected abstract IStyle GetStyle([NotNull] TileBase tile, [NotNull] IStyle[] styles, ref object state);

        /// <summary>
        ///     Gets the style state for the given tile.
        /// </summary>
        /// <param name="tile">The tile.</param>
        /// <returns></returns>
        protected object GetState([NotNull] TileBase tile) => tile.StyleState;

        /// <summary>
        ///     Sets the style state for the given tile.
        /// </summary>
        /// <param name="tile">The tile.</param>
        /// <param name="state">The state.</param>
        protected void SetState([NotNull] TileBase tile, object state) => tile.StyleState = state;

        /// <summary>
        ///     Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        ///     An enumerator that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<TileStyle> GetEnumerator() => _styles.GetEnumerator();

        /// <summary>
        ///     Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        ///     An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}