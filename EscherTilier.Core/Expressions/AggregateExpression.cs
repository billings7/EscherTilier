using System.Collections.Generic;

namespace EscherTilier.Expressions
{
    public abstract class AggregateExpression<TChild, TResult> : IExpression<TResult>
    {
        public IReadOnlyList<IExpression<TChild>> Expressions { get; }

        public abstract TResult Evaluate(Shape shape);
    }
}