using System;
using System.Linq.Expressions;
using JetBrains.Annotations;

namespace EscherTilier.Expressions
{
    public interface ICompiledExpression : IExpression
    {
        /// <summary>
        ///     Gets the raw non-compiled expression that this expression was compiled from.
        /// </summary>
        /// <value>
        ///     The raw expression.
        /// </value>
        IExpression RawExpression { get; }
    }

    public class CompiledExpression<T> : IExpression<T>, ICompiledExpression
    {
        [NotNull]
        private readonly Func<ShapeSet, T> _eval;

        /// <summary>
        ///     Initializes a new instance of the <see cref="CompiledExpression{T}" /> class.
        /// </summary>
        /// <param name="eval">The eval function.</param>
        /// <param name="rawExpression">The raw expression.</param>
        /// <exception cref="System.ArgumentNullException">
        /// </exception>
        private CompiledExpression([NotNull] Func<ShapeSet, T> eval, [NotNull] IExpression<T> rawExpression)
        {
            if (eval == null) throw new ArgumentNullException(nameof(eval));
            if (rawExpression == null) throw new ArgumentNullException(nameof(rawExpression));
            _eval = eval;
            RawExpression = rawExpression;
        }

        /// <summary>
        ///     Gets the raw non-compiled expression that this expression was compiled from.
        /// </summary>
        /// <value>
        ///     The raw expression.
        /// </value>
        [NotNull]
        public IExpression<T> RawExpression { get; }

        /// <summary>
        ///     Gets the raw non-compiled expression that this expression was compiled from.
        /// </summary>
        /// <value>
        ///     The raw expression.
        /// </value>
        IExpression ICompiledExpression.RawExpression => RawExpression;

        /// <summary>
        ///     Gets the type of the expression.
        /// </summary>
        /// <value>
        ///     The type of the expression.
        /// </value>
        public ExpressionType ExpressionType => ExpressionType.Compiled;

        /// <summary>
        ///     Gets the <see cref="Expression" /> version of this expression.
        /// </summary>
        /// <param name="shapes"></param>
        public Expression GetLinqExpression(ParameterExpression shapes) => RawExpression.GetLinqExpression(shapes);

        /// <summary>
        ///     Evaluates the expression for the given set of shapes.
        /// </summary>
        /// <param name="shapes">The shapes to evaluate the expression for.</param>
        /// <returns>The result of evaluating the expression.</returns>
        public T Evaluate(ShapeSet shapes) => _eval(shapes);

        /// <summary>
        ///     Compiles the specified expression.
        /// </summary>
        /// <param name="expression">The expression to compile.</param>
        /// <returns>The compiled expression.</returns>
        [NotNull]
        public static CompiledExpression<T> Compile([NotNull] IExpression<T> expression)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));

            CompiledExpression<T> compiled = expression as CompiledExpression<T>;
            if (compiled != null) return compiled;

            ParameterExpression shapes = Expression.Parameter(typeof(ShapeSet), "shapes");

            Expression body = expression.GetLinqExpression(shapes);

            Func<ShapeSet, T> func =
                Expression.Lambda<Func<ShapeSet, T>>(body, nameof(IExpression<T>.Evaluate), new[] { shapes })
                    .Compile();

            return new CompiledExpression<T>(func, expression);
        }
    }
}