using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using JetBrains.Annotations;

namespace EscherTilier.Expressions
{
    /// <summary>
    ///     Abstract base class for an expression that aggregates the value of multiple sub expressions.
    /// </summary>
    /// <typeparam name="TChild">The type of the child expression.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    public abstract class AggregateExpression<TChild, TResult> : IExpression<TResult>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="AggregateExpression{TChild, TResult}" /> class.
        /// </summary>
        /// <param name="expressionType">Type of the expression.</param>
        /// <param name="expressions">The expressions.</param>
        protected AggregateExpression(
            ExpressionType expressionType,
            [NotNull] IReadOnlyList<IExpression<TChild>> expressions)
        {
            if (expressions == null) throw new ArgumentNullException(nameof(expressions));
            if (expressions.Count < 1)
            {
                throw new ArgumentException(
                    Strings.AggregateExpression_AggregateExpression_NoExpressions,
                    nameof(expressions));
            }
            if (expressions.Any(e => e == null))
            {
                throw new ArgumentNullException(
                    nameof(expressions),
                    Strings.AggregateExpression_AggregateExpression_ExpressionsNull);
            }
            ExpressionType = expressionType;
            Expressions = expressions;
        }

        /// <summary>
        ///     Gets the type of the expression.
        /// </summary>
        /// <value>
        ///     The type of the expression.
        /// </value>
        public ExpressionType ExpressionType { get; }

        /// <summary>
        ///     Gets the expressions that this expression applies to.
        /// </summary>
        /// <value>
        ///     The expressions.
        /// </value>
        [NotNull]
        [ItemNotNull]
        public IReadOnlyList<IExpression<TChild>> Expressions { get; }

        /// <summary>
        ///     Evaluates the expression for the given set of shapes.
        /// </summary>
        /// <param name="shapes">The shapes to evaluate the expression for.</param>
        /// <returns>The result of evaluating the expression.</returns>
        public abstract TResult Evaluate(ShapeSet shapes);

        /// <summary>
        ///     Gets the <see cref="Expression" /> version of this expression.
        /// </summary>
        /// <param name="shapes">
        ///     The <see cref="ParameterExpression" /> for the <c>shapes</c> parameter to
        ///     <see cref="IExpression{T}.Evaluate" />.
        /// </param>
        public abstract Expression GetLinqExpression(ParameterExpression shapes);
    }
}