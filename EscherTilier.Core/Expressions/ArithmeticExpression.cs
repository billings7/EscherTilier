using System;
using System.Linq;
using System.Collections.Generic;
using System;

namespace EscherTilier.Expressions
{
    public class ArithmeticExpression : AggregateExpression<float, float>
    {
        public ArithmeticOperation Operation { get; }

        public override float Evaluate(Shape shape)
        {
            throw new NotImplementedException();
        }
    }
}