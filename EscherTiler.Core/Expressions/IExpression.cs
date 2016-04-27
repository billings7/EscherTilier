using System.Linq.Expressions;
using JetBrains.Annotations;

namespace EscherTiler.Expressions
{
    /// <summary>
    ///     Interface to an expression.
    /// </summary>
    public interface IExpression
    {
        /// <summary>
        ///     Gets the type of the expression.
        /// </summary>
        /// <value>
        ///     The type of the expression.
        /// </value>
        ExpressionType ExpressionType { get; }

        /// <summary>
        ///     Gets the <see cref="Expression" /> version of this expression.
        /// </summary>
        /// <param name="shapes">
        ///     The <see cref="ParameterExpression" /> for the <c>shapes</c> parameter to
        ///     <see cref="IExpression{T}.Evaluate" />.
        /// </param>
        [NotNull]
        Expression GetLinqExpression([NotNull] ParameterExpression shapes);
    }

    /// <summary>
    ///     Interface to an expression that can be evaluated to a value of a specific type.
    /// </summary>
    /// <typeparam name="T">The type that the expression evaluates to.</typeparam>
    public interface IExpression<out T> : IExpression
    {
        /// <summary>
        ///     Evaluates the expression for the given set of shapes.
        /// </summary>
        /// <param name="shapes">The shapes to evaluate the expression for.</param>
        /// <returns>The result of evaluating the expression.</returns>
        T Evaluate([NotNull] ShapeSet shapes);
    }
}