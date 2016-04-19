using System;
using System.Collections.Generic;
using System.Numerics;
using EscherTilier.Numerics;
using JetBrains.Annotations;

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

        [NotNull]
        public Edge Edge { get; }

        [NotNull]
        public EdgePart Part { get; }

        [NotNull]
        [ItemNotNull]
        public IList<ILine> Lines { get; }
    }
}