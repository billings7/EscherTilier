using System.Collections.Generic;

namespace EscherTilier
{
    public class EdgePartShape
    {
        public EdgePart Part { get; }

        public IReadOnlyList<ILine> Lines { get; }
    }
}