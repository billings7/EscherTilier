using System;
using System.Linq;
using System.Collections.Generic;
using System;

namespace EscherTilier.Expressions
{
    public class EdgeExpression : IExpression<float>
    {
        public string VertexName { get; }

        public float Evaluate(Shape shape)
        {
            throw new NotImplementedException();
        }
    }
}