using System.Linq.Expressions;

namespace EscherTiler.Expressions
{
    /// <summary>
    ///     An expression that represents a constant number.
    /// </summary>
    public class NumberExpression : IExpression<float>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="NumberExpression" /> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public NumberExpression(float value)
        {
            Value = value;
        }

        /// <summary>
        ///     Gets or sets the type of the expression.
        /// </summary>
        /// <value>
        ///     The type of the expression.
        /// </value>
        public ExpressionType ExpressionType => ExpressionType.Number;

        /// <summary>
        ///     Gets the numeric value of the expression.
        /// </summary>
        /// <value>
        ///     The value.
        /// </value>
        public float Value { get; }

        /// <summary>
        ///     Evaluates the expression for the given shape.
        /// </summary>
        /// <param name="shapes"></param>
        /// <returns>The result of evaluating the expression.</returns>
        public float Evaluate(ShapeSet shapes) => Value;

        /// <summary>
        ///     Gets the <see cref="Expression" /> version of this expression.
        /// </summary>
        /// <param name="shapes"></param>
        public Expression GetLinqExpression(ParameterExpression shapes) => Expression.Constant(Value);
    }
}