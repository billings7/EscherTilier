using System;
using System.Linq.Expressions;
using JetBrains.Annotations;

namespace EscherTiler.Expressions
{
    /// <summary>
    ///     An expression that represents the angle of a vertex.
    /// </summary>
    public class VertexExpression : IExpression<float>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="VertexExpression" /> class.
        /// </summary>
        /// <param name="vertexName">Name of the vertex.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public VertexExpression([NotNull] string vertexName)
        {
            if (string.IsNullOrWhiteSpace(vertexName))
            {
                throw new ArgumentNullException(
                    nameof(vertexName),
                    Strings.VertexExpression_VertexExpression_NameNullOrWhitespace);
            }
            VertexName = vertexName;
        }

        /// <summary>
        ///     Gets the type of the expression.
        /// </summary>
        /// <value>
        ///     The type of the expression.
        /// </value>
        public ExpressionType ExpressionType => ExpressionType.Vertex;

        /// <summary>
        ///     Gets the name of the vertex.
        /// </summary>
        /// <value>
        ///     The name of the vertex.
        /// </value>
        [NotNull]
        public string VertexName { get; }

        /// <summary>
        ///     Evaluates the expression for the given set of shapes.
        /// </summary>
        /// <param name="shapes">The shapes to evaluate the expression for.</param>
        /// <returns>The result of evaluating the expression.</returns>
        public float Evaluate(ShapeSet shapes) => shapes.GetVertex(VertexName).Angle;

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
                Expression.Call(shapes, nameof(ShapeSet.GetVertex), null, Expression.Constant(VertexName)),
                nameof(Vertex.Angle));
        }
    }
}