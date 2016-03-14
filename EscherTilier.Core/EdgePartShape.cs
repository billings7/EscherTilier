using System.Collections.Generic;
using System.Numerics;

namespace EscherTilier
{
    public class EdgePartShape
    {
        public EdgePartShape(EdgePart part, Edge edge)
        {
            Part = part;
            Edge = edge;
            Lines = new List<ILine>
            {
                new Line(Vector2.Zero, new Vector2(1, 0))
            };
        }

        public Edge Edge { get; }

        public EdgePart Part { get; }

        public IList<ILine> Lines { get; }
    }
}