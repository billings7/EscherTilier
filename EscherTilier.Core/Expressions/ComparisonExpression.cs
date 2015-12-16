using System;
using System.Linq;
using System.Collections.Generic;
using System;

namespace EscherTilier.Expressions
{
    public class ComparisonExpression : AggregateExpression<float, bool>
    {
        public ComparisonOperation Operation { get; }

        public override bool Evaluate(Shape shape)
        {
            throw new NotImplementedException();
        }
    }
}