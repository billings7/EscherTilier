using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using EscherTilier.Utilities;
using JetBrains.Annotations;
using NodeType = System.Linq.Expressions.ExpressionType;

namespace EscherTilier.Expressions
{
    /// <summary>
    ///     An expression that represents a logical operation.
    /// </summary>
    public class LogicalExpression : AggregateExpression<bool, bool>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="LogicalExpression" /> class.
        /// </summary>
        /// <param name="expressionType">Type of the expression.</param>
        /// <param name="expressions">The expressions.</param>
        public LogicalExpression(ExpressionType expressionType, [NotNull] IReadOnlyList<IExpression<bool>> expressions)
            : base(expressionType, expressions)
        {
            switch (expressionType)
            {
                case ExpressionType.And:
                case ExpressionType.Or:
                case ExpressionType.BoolEqual:
                    break;
                case ExpressionType.Xor:
                    if (expressions.Count != 2)
                        throw new ArgumentException(Strings.LogicalExpression_LogicalExpression_XorChildCount);
                    break;
                case ExpressionType.Not:
                    if (expressions.Count != 1)
                        throw new ArgumentException(Strings.LogicalExpression_LogicalExpression_NotChildCount);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(
                        nameof(expressionType),
                        expressionType,
                        Strings.LogicalExpression_LogicalExpression_InvalidType);
            }
        }

        /// <summary>
        ///     Evaluates the expression for the given shape.
        /// </summary>
        /// <param name="shapes"></param>
        /// <returns>The result of evaluating the expression.</returns>
        public override bool Evaluate(ShapeSet shapes)
        {
            if (shapes == null) throw new ArgumentNullException(nameof(shapes));
            switch (ExpressionType)
            {
                case ExpressionType.And:
                    return Expressions.All(e => e.Evaluate(shapes));
                case ExpressionType.Or:
                    return Expressions.Any(e => e.Evaluate(shapes));
                case ExpressionType.BoolEqual:
                    return Expressions.AreEqual();
                case ExpressionType.Xor:
                    Debug.Assert(Expressions.Count == 2);
                    return Expressions[0].Evaluate(shapes) ^ Expressions[1].Evaluate(shapes);
                case ExpressionType.Not:
                    Debug.Assert(Expressions.Count == 1);
                    return !Expressions[0].Evaluate(shapes);
                default:
                    Debug.Assert(false);
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        ///     Gets the <see cref="Expression" /> version of this expression.
        /// </summary>
        /// <param name="shapes">
        ///     The <see cref="ParameterExpression" /> for the <c>shapes</c> parameter to
        ///     <see cref="IExpression{T}.Evaluate" />.
        /// </param>
        public override Expression GetLinqExpression(ParameterExpression shapes)
        {
            if (shapes == null) throw new ArgumentNullException(nameof(shapes));
            switch (ExpressionType)
            {
                case ExpressionType.And:
                    return GetAndOrLinqExpression(shapes, true);
                case ExpressionType.Or:
                    return GetAndOrLinqExpression(shapes, false);
                case ExpressionType.BoolEqual:
                    return GetEqualLinqExpression(shapes);
                case ExpressionType.Xor:
                    Debug.Assert(Expressions.Count == 2);

                    Expression a = Expressions[0].GetLinqExpression(shapes);
                    Expression b = Expressions[1].GetLinqExpression(shapes);

                    ConstantExpression constA, constB;
                    if ((constA = a as ConstantExpression) != null &&
                        (constB = b as ConstantExpression) != null)
                    {
                        Debug.Assert(constA.Value is bool);
                        Debug.Assert(constB.Value is bool);

                        return Expression.Constant((bool) constA.Value ^ (bool) constB.Value);
                    }

                    return Expression.ExclusiveOr(a, b);
                case ExpressionType.Not:
                    Debug.Assert(Expressions.Count == 1);

                    Expression expression = Expressions[0].GetLinqExpression(shapes);

                    ConstantExpression constExp = expression as ConstantExpression;
                    if (constExp != null)
                    {
                        Debug.Assert(constExp.Value is bool);

                        return Expression.Constant(!(bool) constExp.Value);
                    }

                    return Expression.Not(expression);
                default:
                    Debug.Assert(false);
                    throw new ArgumentOutOfRangeException();
            }
        }

        [NotNull]
        private Expression GetAndOrLinqExpression([NotNull] ParameterExpression shapes, bool isAnd)
        {
            Expression result = null;

            foreach (Expression expression in Expressions.Select(e => e.GetLinqExpression(shapes)))
            {
                Debug.Assert(expression != null, "expression != null");

                ConstantExpression constExp = expression as ConstantExpression;
                if (constExp != null)
                {
                    Debug.Assert(constExp.Value is bool);

                    if (isAnd)
                    {
                        if (!(bool) constExp.Value) return Expression.Constant(false);
                    }
                    else if ((bool) constExp.Value) return Expression.Constant(true);
                    continue;
                }

                result = result == null
                    ? expression
                    : (isAnd
                        ? Expression.AndAlso(result, expression)
                        : Expression.OrElse(result, expression));
            }

            return result ?? Expression.Constant(isAnd);
        }

        [NotNull]
        private Expression GetEqualLinqExpression([NotNull] ParameterExpression shapes)
        {
            List<BinaryExpression> comparisons = new List<BinaryExpression>();
            List<Expression> body = new List<Expression>();
            List<ParameterExpression> vars = new List<ParameterExpression>();

            Expression last = null;
            bool lastConst = false;
            foreach (Expression exp in Expressions.Select(e => e.GetLinqExpression(shapes)))
            {
                Expression expression = exp;
                Debug.Assert(expression != null, "expression != null");

                ConstantExpression constExp = expression as ConstantExpression;
                if (constExp != null)
                {
                    Debug.Assert(constExp.Value is bool);

                    bool curr = (bool) constExp.Value;
                    if (last != null && lastConst != curr) return Expression.Constant(false);

                    lastConst = curr;
                }
                else
                {
                    ParameterExpression varExp = Expression.Variable(typeof(float));
                    vars.Add(varExp);
                    body.Add(Expression.Assign(varExp, expression));
                    expression = varExp;
                }

                if (last != null)
                    comparisons.Add(Expression.Equal(last, expression));

                last = expression;
            }

            Expression result = null;

            foreach (BinaryExpression comparison in comparisons)
            {
                Debug.Assert(comparison != null, "comparison != null");

                ConstantExpression leftConst, rightConst;

                if ((leftConst = comparison.Left as ConstantExpression) != null &&
                    (rightConst = comparison.Right as ConstantExpression) != null)
                {
#if DEBUG
                    Debug.Assert(leftConst.Value is bool);
                    Debug.Assert(rightConst.Value is bool);

                    bool leftVal = (bool) leftConst.Value;
                    bool rightVal = (bool) rightConst.Value;

                    Debug.Assert(leftVal == rightVal);
#endif

                    continue;
                }

                result = result == null
                    ? comparison
                    : Expression.AndAlso(result, comparison);
            }

            return result == null
                ? (Expression)Expression.Constant(true)
                : Expression.Block(typeof(bool), vars, body);
        }
    }
}