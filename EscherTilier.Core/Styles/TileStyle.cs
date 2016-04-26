using System.Collections.Generic;

namespace EscherTilier.Styles
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
        public TileStyle(IStyle style, IReadOnlyList<Shape> shapes)
        {
            Style = style;
            Shapes = shapes;
        }

        /// <summary>
        ///     Gets the shapes that this style can be used with.
        /// </summary>
        /// <value>
        ///     The shapes.
        /// </value>
        public IReadOnlyList<Shape> Shapes { get; }

        /// <summary>
        ///     Gets the style.
        /// </summary>
        /// <value>
        ///     The style.
        /// </value>
        public IStyle Style { get; }
    }
}