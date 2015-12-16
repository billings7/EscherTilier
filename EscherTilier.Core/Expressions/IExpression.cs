using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EscherTilier.Expressions
{
    public interface IExpression<T>
    {
        T Evaluate(Shape shape);
    }
}
