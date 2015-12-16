using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EscherTilier.Expressions
{

    public class NumberExpression : IExpression<float>
    {
        public float Value { get; }

        public float Evaluate(Shape shape) => Value;
    }
}
