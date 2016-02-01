using System.Collections.Generic;
using EscherTilier.Expressions;
using JetBrains.Annotations;

namespace EscherTilier
{
    public class TilingDefinition
    {
        public int ID { get; }

        [CanBeNull]
        public IExpression<bool> Condition { get; }

        public IReadOnlyList<EdgePattern> EdgePatterns { get; }
    }
}