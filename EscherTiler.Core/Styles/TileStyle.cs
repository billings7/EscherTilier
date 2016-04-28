using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace EscherTiler.Styles
{
    /// <summary>
    ///     Defines a style that can be applied to a tile that is based on one of a specific set of shapes.
    /// </summary>
    public class TileStyle
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="TileStyle" /> class.
        /// </summary>
        /// <param name="style">The style.</param>
        /// <param name="shapes">The shapes.</param>
        public TileStyle([NotNull] IStyle style, [NotNull] IReadOnlyCollection<Shape> shapes)
        {
            if (style == null) throw new ArgumentNullException(nameof(style));
            if (shapes == null) throw new ArgumentNullException(nameof(shapes));
            Style = style;
            Shapes = shapes;
        }

        /// <summary>
        ///     Gets the shapes that this style can be used with.
        /// </summary>
        /// <value>
        ///     The shapes.
        /// </value>
        [NotNull]
        public IReadOnlyCollection<Shape> Shapes { get; }

        /// <summary>
        ///     Gets the style.
        /// </summary>
        /// <value>
        ///     The style.
        /// </value>
        [NotNull]
        public IStyle Style { get; }
    }
}