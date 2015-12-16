using System;
using System.Linq;
using System.Collections.Generic;
using System;

namespace EscherTilier.Expressions
{
    public class VertexExpression : IExpression<float>
    {
        public string VertexName { get; }

        public float Evaluate(Shape shape)
        {
            throw new NotImplementedException();
        }
    }
}