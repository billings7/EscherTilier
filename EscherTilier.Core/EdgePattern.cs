using System.Collections.Generic;

namespace EscherTilier
{
    public class EdgePattern
    {
        public string EdgeName { get; }

        public IReadOnlyList<EdgePart> Parts { get; }
    }
}