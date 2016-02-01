namespace EscherTilier.Expressions
{
    public class NumberExpression : IExpression<float>
    {
        public float Value { get; }

        public float Evaluate(Shape shape) => Value;
    }
}