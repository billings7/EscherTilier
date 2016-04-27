using System;
using JetBrains.Annotations;

namespace EscherTiler.Styles
{
    /// <summary>
    ///     Defines the style of a line.
    /// </summary>
    public class LineStyle
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="LineStyle" /> class.
        /// </summary>
        /// <param name="width">The line width.</param>
        /// <param name="style">The style.</param>
        public LineStyle(float width, [NotNull] SolidColourStyle style)
        {
            if (style == null) throw new ArgumentNullException(nameof(style));
            if (width <= 0)
                throw new ArgumentOutOfRangeException(nameof(width), Strings.LineStyle_LineStyle_LineWidthNegative);
            Width = width;
            Style = style;
        }

        /// <summary>
        ///     Gets the width of the line.
        /// </summary>
        /// <value>
        ///     The width.
        /// </value>
        public float Width { get; }

        /// <summary>
        ///     Gets the style used to draw the line.
        /// </summary>
        /// <value>
        ///     The style.
        /// </value>
        [NotNull]
        public SolidColourStyle Style { get; }
    }
}