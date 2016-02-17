using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using JetBrains.Annotations;

namespace EscherTilier.Expressions
{
    /// <summary>
    ///     An expression that represents an arithmetic operation.
    /// </summary>
    public class ArithmeticExpression : AggregateExpression<float, float>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ArithmeticExpression" /> class.
        /// </summary>
        /// <param name="expressionType">Type of the expression.</param>
        /// <param name="expressions">The expressions.</param>
        private ArithmeticExpression(
            ExpressionType expressionType,
            [NotNull] IReadOnlyList<IExpression<float>> expressions)
            : base(expressionType, expressions) { }

        /// <summary>
        ///     Creates an <see cref="ArithmeticExpression" /> that represents an addition operation.
        /// </summary>
        /// <param name="expressions">The expressions to add.</param>
        /// <returns>The created <see cref="ArithmeticExpression" />.</returns>
        [NotNull]
        public static ArithmeticExpression Add([NotNull] IReadOnlyList<IExpression<float>> expressions)
            => new ArithmeticExpression(ExpressionType.Add, expressions);

        /// <summary>
        ///     Creates an <see cref="ArithmeticExpression" /> that represents a subtraction operation.
        /// </summary>
        /// <param name="expressions">The expressions to subtract.</param>
        /// <returns>The created <see cref="ArithmeticExpression" />.</returns>
        [NotNull]
        public static ArithmeticExpression Subtract([NotNull] IReadOnlyList<IExpression<float>> expressions)
            => new ArithmeticExpression(ExpressionType.Subtract, expressions);

        /// <summary>
        ///     Creates an <see cref="ArithmeticExpression" /> that represents a multiplication operation.
        /// </summary>
        /// <param name="expressions">The expressions to multiply.</param>
        /// <returns>The created <see cref="ArithmeticExpression" />.</returns>
        [NotNull]
        public static ArithmeticExpression Multiply([NotNull] IReadOnlyList<IExpression<float>> expressions)
        {
            if (expressions.Count < 2)
            {
                throw new ArgumentException(
                    Strings.ArithmeticExpression_ArithmeticExpression_NotEnoughExpressions,
                    nameof(expressions));
            }
            return new ArithmeticExpression(ExpressionType.Multiply, expressions);
        }

        /// <summary>
        ///     Creates an <see cref="ArithmeticExpression" /> that represents a division operation.
        /// </summary>
        /// <param name="expressions">The expressions to divide.</param>
        /// <returns>The created <see cref="ArithmeticExpression" />.</returns>
        [NotNull]
        public static ArithmeticExpression Divide([NotNull] IReadOnlyList<IExpression<float>> expressions)
        {
            if (expressions.Count < 2)
            {
                throw new ArgumentException(
                    Strings.ArithmeticExpression_ArithmeticExpression_NotEnoughExpressions,
                    nameof(expressions));
            }
            if (expressions.Count > 2)
            {
                ArithmeticExpression denom = new ArithmeticExpression(
                    ExpressionType.Multiply,
                    expressions.Skip(1).ToArray());
                expressions = new[] { expressions[0], denom };
            }
            return new ArithmeticExpression(ExpressionType.Divide, expressions);
        }

        /// <summary>
        ///     Evaluates the expression for the given set of shapes.
        /// </summary>
        /// <param name="shapes">The shapes to evaluate the expression for.</param>
        /// <returns>The result of evaluating the expression.</returns>
        public override float Evaluate(ShapeSet shapes)
        {
            if (shapes == null) throw new ArgumentNullException(nameof(shapes));
            bool first = true;
            switch (ExpressionType)
            {
                case ExpressionType.Add:
                    float sum = 0;
                    foreach (float val in Expressions.Select(e => e.Evaluate(shapes)))
                        sum += val;
                    return sum;
                case ExpressionType.Subtract:
                    if (Expressions.Count == 1) return -Expressions[0].Evaluate(shapes);

                    float sub = 0;
                    foreach (float val in Expressions.Select(e => e.Evaluate(shapes)))
                    {
                        if (first) sub = val;
                        else sub -= val;
                        first = false;
                    }
                    return sub;
                case ExpressionType.Multiply:
                    float mul = 1f;
                    foreach (float val in Expressions.Select(e => e.Evaluate(shapes)))
                    {
                        if (first) mul = val;
                        else mul *= val;
                        first = false;
                    }
                    return mul;
                case ExpressionType.Divide:
                    Debug.Assert(Expressions.Count == 2);
                    return Expressions[0].Evaluate(shapes) / Expressions[1].Evaluate(shapes);
                default:
                    Debug.Assert(false);
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        ///     Gets the <see cref="Expression" /> version of this expression.
        /// </summary>
        /// <param name="shapes"></param>
        public override Expression GetLinqExpression(ParameterExpression shapes)
        {
            Expression result = null;

            float constPart = 0;

            switch (ExpressionType)
            {
                case ExpressionType.Add:
                    foreach (Expression expression in Expressions.Select(e => e.GetLinqExpression(shapes)))
                    {
                        Debug.Assert(expression != null, "expression != null");

                        ConstantExpression constExp = expression as ConstantExpression;
                        if (constExp != null)
                        {
                            Debug.Assert(constExp.Value is float);
                            constPart += (float) constExp.Value;
                            continue;
                        }

                        result = result == null ? expression : Expression.Add(result, expression);
                    }

                    // ReSharper disable once CompareOfFloatsByEqualityOperator
                    if (constPart != 0)
                    {
                        result = result == null
                            ? (Expression) Expression.Constant(constPart)
                            : Expression.Add(result, Expression.Constant(constPart));
                    }

                    Debug.Assert(result != null, "result != null");
                    return result;
                case ExpressionType.Subtract:
                    if (Expressions.Count == 1)
                    {
                        result = Expressions[0].GetLinqExpression(shapes);

                        ConstantExpression constExp = result as ConstantExpression;
                        if (constExp == null) return Expression.Negate(result);

                        Debug.Assert(constExp.Value is float);
                        return Expression.Constant(-(float) constExp.Value);
                    }

                    bool first = true;
                    foreach (Expression expression in Expressions.Select(e => e.GetLinqExpression(shapes)))
                    {
                        Debug.Assert(expression != null, "expression != null");

                        ConstantExpression constExp = expression as ConstantExpression;
                        if (!first && constExp != null)
                        {
                            Debug.Assert(constExp.Value is float);
                            constPart += (float) constExp.Value;
                            continue;
                        }
                        first = false;

                        result = result == null ? expression : Expression.Subtract(result, expression);
                    }

                    // ReSharper disable once CompareOfFloatsByEqualityOperator
                    if (constPart != 0)
                    {
                        result = result == null
                            ? (Expression) Expression.Constant(constPart)
                            : Expression.Subtract(result, Expression.Constant(constPart));
                    }

                    Debug.Assert(result != null, "result != null");
                    return result;
                case ExpressionType.Multiply:
                    foreach (Expression expression in Expressions.Select(e => e.GetLinqExpression(shapes)))
                    {
                        Debug.Assert(expression != null, "expression != null");

                        ConstantExpression constExp = expression as ConstantExpression;
                        if (constExp != null)
                        {
                            Debug.Assert(constExp.Value is float);
                            constPart *= (float) constExp.Value;
                            continue;
                        }

                        result = result == null ? expression : Expression.Multiply(result, expression);
                    }

                    // ReSharper disable once CompareOfFloatsByEqualityOperator
                    if (constPart != 0)
                    {
                        result = result == null
                            ? (Expression) Expression.Constant(constPart)
                            : Expression.Multiply(result, Expression.Constant(constPart));
                    }

                    Debug.Assert(result != null, "result != null");
                    return result;
                case ExpressionType.Divide:
                    Debug.Assert(Expressions.Count == 2);

                    Expression n = Expressions[0].GetLinqExpression(shapes);
                    Expression d = Expressions[1].GetLinqExpression(shapes);

                    ConstantExpression constN, constD;
                    if ((constN = n as ConstantExpression) != null &&
                        (constD = d as ConstantExpression) != null)
                    {
                        Debug.Assert(constN.Value is float);
                        Debug.Assert(constD.Value is float);

                        return Expression.Constant((float) constN.Value / (float) constD.Value);
                    }

                    return Expression.Divide(n, d);
                default:
                    Debug.Assert(false);
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}