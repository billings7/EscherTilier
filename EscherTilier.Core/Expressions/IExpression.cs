namespace EscherTilier.Expressions
{
    public interface IExpression<T>
    {
        T Evaluate(Shape shape);
    }
}