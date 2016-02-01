using System.Collections.Generic;

namespace EscherTilier.Styles
{
    public class TileStyle
    {
        public IReadOnlyList<Shape> Shapes { get; }

        public IStyle Style { get; }
    }
}