using System.Drawing;
using JetBrains.Annotations;

namespace EscherTilier.Styles
{
    public class LineStyle
    {
        public LineStyle(float width, SolidColourStyle style)
        {
            Width = width;
            Style = style;
        }

        public float Width { get; }

        [NotNull]
        public SolidColourStyle Style { get; }
    }
}