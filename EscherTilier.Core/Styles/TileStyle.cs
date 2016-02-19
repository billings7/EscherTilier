using System.Collections.Generic;

namespace EscherTilier.Styles
{
    public class TileStyle
    {
        public TileStyle(IStyle style, IReadOnlyList<Shape> shapes)
        {
            Style = style;
            Shapes = shapes;
        }

        public IReadOnlyList<Shape> Shapes { get; }

        public IStyle Style { get; }
    }
}