using JetBrains.Annotations;

namespace EscherTilier
{
    public class EdgePartShape
    {
        public EdgePartShape(EdgePart part, Edge edge, ShapeLines lines)
        {
            Part = part;
            Edge = edge;
            Lines = lines;
        }

        [NotNull]
        public Edge Edge { get; }

        [NotNull]
        public EdgePart Part { get; }

        [NotNull]
        [ItemNotNull]
        public ShapeLines Lines { get; }
    }
}