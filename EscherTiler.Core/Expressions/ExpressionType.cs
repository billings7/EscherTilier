namespace EscherTiler.Expressions
{
    /// <summary>
    ///     Defines the possible <see cref="IExpression" /> types.
    /// </summary>
    public enum ExpressionType
    {
        Compiled,
        Number,
        Edge,
        Vertex,
        Add,
        Subtract,
        Multiply,
        Divide,
        Equal,
        NotEqual,
        GreaterThan,
        LessThan,
        GreaterThanOrEqual,
        LessThanOrEqual,
        And,
        Or,
        BoolEqual,
        Xor,
        Not
    }
}