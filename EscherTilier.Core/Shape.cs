using System.Collections.Generic;

namespace EscherTilier
{
    public class Shape
    {
        public ShapeTemplate Template { get; }

        public IReadOnlyList<Vertex> Vertices { get; }

        public IReadOnlyList<Edge> Edges { get; }
    }
}