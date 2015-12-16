using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Generic;

namespace EscherTilier.Styles
{
    public class TileStyle
    {
        public IReadOnlyList<Shape> Shapes { get; }

        public IStyle Style { get; }
    }
}