using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using JetBrains.Annotations;
using NodeType = System.Linq.Expressions.ExpressionType;

namespace EscherTiler.Expressions
{
    /// <summary>
    ///     An expression that represents a comparison operation.
    /// </summary>
    public class ComparisonExpression : AggregateExpression<float, bool>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ComparisonExpression" /> class.
        /// </summary>
        /// <param name="expressionType">Type of the expression.</param>
        /// <param name="expressions">The expressions.</param>
        public ComparisonExpression(
            ExpressionType expressionType,
            [NotNull] IReadOnlyList<IExpression<float>> expressions)
            : base(expressionType, expressions)
        {
            switch (expressionType)
            {
                case ExpressionType.Equal:
                case ExpressionType.NotEqual:
                case ExpressionType.GreaterThan:
                case ExpressionType.LessThan:
                case ExpressionType.GreaterThanOrEqual:
                case ExpressionType.LessThanOrEqual:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(
                        nameof(expressionType),
                        expressionType,
                        Strings.ComparisonExpression_ComparisonExpression_InvalidType);
            }
        }

        /// <summary>
        ///     Evaluates the expression for the given set of shapes.
        /// </summary>
        /// <param name="shapes">The shapes to evaluate the expression for.</param>
        /// <returns>The result of evaluating the expression.</returns>
        public override bool Evaluate(ShapeSet shapes)
        {
            if (shapes == null) throw new ArgumentNullException(nameof(shapes));
            const float tolerance = 0.0001f;

            float last = 0;
            bool first = true;
            switch (ExpressionType)
            {
                case ExpressionType.Equal:
                    float min = 0, max = 0;
                    foreach (float val in Expressions.Select(e => e.Evaluate(shapes)))
                    {
                        if (first) min = max = val;
                        else
                        {
                            if (val < min) min = val;
                            else if (val > max) max = val;
                            if (max - min > tolerance) return false;
                        }
                        first = false;
                    }
                    return true;
                case ExpressionType.NotEqual:
                    HashSet<float> set = new HashSet<float>();
                    foreach (float val in Expressions.Select(e => e.Evaluate(shapes)))
                        if (!set.Add(val)) return false;
                    return true;
                case ExpressionType.GreaterThan:
                    foreach (float val in Expressions.Select(e => e.Evaluate(shapes)))
                    {
                        if (!first)
                            if (last <= val || Math.Abs(last - val) > tolerance) return false;
                        last = val;
                        first = false;
                    }
                    return true;
                case ExpressionType.LessThan:
                    foreach (float val in Expressions.Select(e => e.Evaluate(shapes)))
                    {
                        if (!first)
                            if (last >= val || Math.Abs(last - val) > tolerance) return false;
                        last = val;
                        first = false;
                    }
                    return true;
                case ExpressionType.GreaterThanOrEqual:
                    foreach (float val in Expressions.Select(e => e.Evaluate(shapes)))
                    {
                        if (!first && last < val) return false;
                        last = val;
                        first = false;
                    }
                    return true;
                case ExpressionType.LessThanOrEqual:
                    foreach (float val in Expressions.Select(e => e.Evaluate(shapes)))
                    {
                        if (!first && last > val) return false;
                        last = val;
                        first = false;
                    }
                    return true;
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
                case ExpressionType.Equal:
                    return GetEqualLinqExpression(shapes);
                case ExpressionType.NotEqual:
                    return GetNotEqualLinqExpression(shapes);
                case ExpressionType.GreaterThan:
                    return GetComparisonLinqExpression(shapes, NodeType.GreaterThan);
                case ExpressionType.LessThan:
                    return GetComparisonLinqExpression(shapes, NodeType.LessThan);
                case ExpressionType.GreaterThanOrEqual:
                    return GetComparisonLinqExpression(shapes, NodeType.GreaterThanOrEqual);
                case ExpressionType.LessThanOrEqual:
                    return GetComparisonLinqExpression(shapes, NodeType.LessThanOrEqual);
                default:
                    Debug.Assert(false);
                    throw new ArgumentOutOfRangeException();
            }
        }

        [NotNull]
        private Expression GetEqualLinqExpression([NotNull] ParameterExpression shapes)
        {
            const float tolerance = 0.0001f;

            List<Expression> body = new List<Expression>();

            float min = 0, max = 0;
            ParameterExpression
                currExp = Expression.Variable(typeof(float), "curr"),
                minExp = Expression.Variable(typeof(float), "min"),
                maxExp = Expression.Variable(typeof(float), "max");
            Expression firstExp = null;
            bool first = true, anyConst = false;
            foreach (Expression expression in Expressions.Select(e => e.GetLinqExpression(shapes)))
            {
                Debug.Assert(expression != null, "expression != null");

                ConstantExpression constExp = expression as ConstantExpression;
                if (constExp != null)
                {
                    Debug.Assert(constExp.Value is float);

                    if (first) min = max = (float) constExp.Value;
                    else if ((float) constExp.Value < min) min = (float) constExp.Value;
                    else if ((float) constExp.Value > max) max = (float) constExp.Value;

                    anyConst = true;
                    first = false;
                    continue;
                }

                firstExp = expression;

                body.Add(Expression.Assign(currExp, expression));

                body.Add(
                    Expression.IfThenElse(
                        Expression.LessThan(currExp, minExp),
                        Expression.Assign(minExp, currExp),
                        Expression.IfThen(
                            Expression.GreaterThan(currExp, maxExp),
                            Expression.Assign(maxExp, currExp))));

                first = false;
            }

            if (firstExp == null)
                return Expression.Constant(max - min < tolerance);

            if (anyConst)
            {
                body.Insert(0, Expression.Assign(minExp, Expression.Constant(min)));
                body.Insert(1, Expression.Assign(maxExp, Expression.Constant(max)));
            }
            else
            {
                body.Insert(0, Expression.Assign(minExp, firstExp));
                body.Insert(1, Expression.Assign(maxExp, firstExp));
            }

            body.Add(
                Expression.GreaterThan(
                    Expression.Subtract(maxExp, minExp),
                    Expression.Constant(tolerance)));

            return Expression.Block(
                typeof(bool),
                new[] { currExp, minExp, maxExp },
                body);
        }

        [NotNull]
        private Expression GetNotEqualLinqExpression([NotNull] ParameterExpression shapes)
        {
            if (Expressions.Count == 2)
            {
                Expression a = Expressions[0].GetLinqExpression(shapes);
                Expression b = Expressions[1].GetLinqExpression(shapes);

                ConstantExpression constA, constB;
                if ((constA = a as ConstantExpression) != null &&
                    (constB = b as ConstantExpression) != null)
                    return Expression.Constant(constA.Value != constB.Value);

                return Expression.NotEqual(a, b);
            }

            List<Expression> body = new List<Expression>();
            HashSet<float> set = new HashSet<float>();
            ParameterExpression setExp = Expression.Variable(typeof(HashSet<float>), "set");
            body.Add(Expression.Assign(setExp, Expression.New(typeof(HashSet<float>))));
            Expression result = null;

            foreach (Expression expression in Expressions.Select(e => e.GetLinqExpression(shapes)))
            {
                Debug.Assert(expression != null, "expression != null");

                ConstantExpression constExp = expression as ConstantExpression;
                if (constExp != null)
                {
                    Debug.Assert(constExp.Value is float);

                    if (!set.Add((float) constExp.Value))
                        return Expression.Constant(false);
                    continue;
                }

                result = result == null
                    ? (Expression) Expression.Call(setExp, nameof(HashSet<float>.Add), null, expression)
                    : Expression.AndAlso(
                        result,
                        Expression.Call(setExp, nameof(HashSet<float>.Add), null, expression));
            }

            if (result == null)
                return Expression.Constant(true);

            return Expression.Block(
                typeof(bool),
                new[] { setExp },
                Expression.Assign(
                    setExp,
                    Expression.New(
                        // ReSharper disable once AssignNullToNotNullAttribute
                        typeof(HashSet<float>).GetConstructor(new[] { typeof(IEnumerable<float>) }),
                        Expression.Constant(set.ToArray(), typeof(IEnumerable<float>)))),
                result);
        }

        [NotNull]
        private Expression GetComparisonLinqExpression([NotNull] ParameterExpression shapes, NodeType type)
        {
            List<BinaryExpression> comparisons = new List<BinaryExpression>();
            List<Expression> body = new List<Expression>();
            List<ParameterExpression> vars = new List<ParameterExpression>();

            Expression last = null;
            float lastConst = 0;
            foreach (Expression exp in Expressions.Select(e => e.GetLinqExpression(shapes)))
            {
                Expression expression = exp;
                Debug.Assert(expression != null, "expression != null");

                ConstantExpression constExp = expression as ConstantExpression;
                if (constExp != null)
                {
                    Debug.Assert(constExp.Value is float);

                    float curr = (float) constExp.Value;
                    if (last != null)
                    {
                        switch (type)
                        {
                            // TODO tolerance
                            case NodeType.GreaterThan:
                                if (lastConst <= curr) return Expression.Constant(false);
                                break;
                            case NodeType.LessThan:
                                if (lastConst >= curr) return Expression.Constant(false);
                                break;
                            case NodeType.GreaterThanOrEqual:
                                if (lastConst < curr) return Expression.Constant(false);
                                break;
                            case NodeType.LessThanOrEqual:
                                if (lastConst > curr) return Expression.Constant(false);
                                break;
                        }
                    }

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
                    comparisons.Add(Expression.MakeBinary(type, last, expression));

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
                    Debug.Assert(leftConst.Value is float);
                    Debug.Assert(rightConst.Value is float);

                    float leftVal = (float) leftConst.Value;
                    float rightVal = (float) rightConst.Value;

                    switch (type)
                    {
                        case NodeType.GreaterThan:
                            Debug.Assert(leftVal > rightVal);
                            break;
                        case NodeType.LessThan:
                            Debug.Assert(leftVal > rightVal);
                            break;
                        case NodeType.GreaterThanOrEqual:
                            Debug.Assert(leftVal > rightVal);
                            break;
                        case NodeType.LessThanOrEqual:
                            Debug.Assert(leftVal > rightVal);
                            break;
                        default:
                            Debug.Assert(false);
                            throw new ArgumentOutOfRangeException();
                    }
#endif

                    continue;
                }

                result = result == null
                    ? comparison
                    : Expression.AndAlso(result, comparison);
            }

            body.Add(result);

            return result == null
                ? (Expression) Expression.Constant(true)
                : Expression.Block(typeof(bool), vars, body);
        }
    }
}