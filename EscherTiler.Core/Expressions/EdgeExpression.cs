using System;
using System.Linq.Expressions;
using JetBrains.Annotations;

namespace EscherTiler.Expressions
{
    /// <summary>
    ///     An expression that represents the length of the edge of a shape.
    /// </summary>
    public class EdgeExpression : IExpression<float>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="EdgeExpression" /> class.
        /// </summary>
        /// <param name="edgeName">Name of the edge.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public EdgeExpression([NotNull] string edgeName)
        {
            if (string.IsNullOrWhiteSpace(edgeName))
            {
                throw new ArgumentNullException(
                    nameof(edgeName),
                    Strings.EdgeExpression_EdgeExpression_NameNullOrWhitespace);
            }
            EdgeName = edgeName;
        }

        /// <summary>
        ///     Gets the type of the expression.
        /// </summary>
        /// <value>
        ///     The type of the expression.
        /// </value>
        public ExpressionType ExpressionType => ExpressionType.Edge;

        /// <summary>
        ///     Gets the name of the edge.
        /// </summary>
        /// <value>
        ///     The name of the edge.
        /// </value>
        [NotNull]
        public string EdgeName { get; }

        /// <summary>
        ///     Evaluates the expression for the given set of shapes.
        /// </summary>
        /// <param name="shapes">The shapes to evaluate the expression for.</param>
        /// <returns>The result of evaluating the expression.</returns>
        public float Evaluate(ShapeSet shapes) => shapes.GetEdge(EdgeName).Length;

        /// <summary>
        ///     Gets the <see cref="Expression" /> version of this expression.
        /// </summary>
        /// <param name="shapes">
        ///     The <see cref="ParameterExpression" /> for the <c>shapes</c> parameter to
        ///     <see cref="IExpression{T}.Evaluate" />.
        /// </param>
        public Expression GetLinqExpression(ParameterExpression shapes)
        {
            if (shapes == null) throw new ArgumentNullException(nameof(shapes));

            return Expression.Property(
                Expression.Call(shapes, nameof(ShapeSet.GetEdge), null, Expression.Constant(EdgeName)),
                nameof(Edge.Length));
        }
    }
}