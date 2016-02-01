using System.Collections.Generic;

namespace EscherTilier
{
    public class EdgePart
    {
        public int ID { get; }

        public PartDirection Direction { get; }

        public float Amount { get; }

        public IReadOnlyList<EdgePartAdjacency> AdjacentParts { get; }
    }
}