using System.Collections.Generic;

namespace EscherTilier
{
    public class ShapeTemplate
    {
        public Template Template { get; }

        public string Name { get; }

        public IReadOnlyList<string> EdgeNames { get; }

        public IReadOnlyList<string> VertexNames { get; }
    }
}