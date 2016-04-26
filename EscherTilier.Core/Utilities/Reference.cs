using System;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;
using JetBrains.Annotations;

namespace EscherTilier.Utilities
{
    /// <summary>
    ///     Provides methods for creating instances of <see cref="Reference{T}" />.
    /// </summary>
    public static class Reference
    {
        /// <summary>
        ///     Gets a <see cref="Reference{T}" /> to the property or field in the given expression.
        /// </summary>
        /// <typeparam name="T">The type of the value being referenced.</typeparam>
        /// <param name="refExp">The expression specifying the property or field to reference.</param>
        /// <returns>A <see cref="Reference{T}" /> to the property or field.</returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="refExp" /> was <see langword="null" />.</exception>
        /// <exception cref="System.NotSupportedException">The specified expression is not supported by this method.</exception>
        public static Reference<T> To<T>([NotNull] Expression<Func<T>> refExp)
        {
            if (refExp == null) throw new ArgumentNullException(nameof(refExp));

            MemberExpression memberExp = refExp.Body as MemberExpression;
            if (memberExp != null)
            {
                FieldInfo field = memberExp.Member as FieldInfo;
                if (field != null)
                {
                    ParameterExpression pe = Expression.Parameter(typeof(T), "value");
                    return new Reference<T>(
                        Expression.Lambda<Func<T>>(memberExp).Compile(),
                        Expression.Lambda<Action<T>>(
                            Expression.Assign(Expression.Field(memberExp.Expression, field), pe),
                            pe).Compile());
                }

                PropertyInfo property = memberExp.Member as PropertyInfo;
                if (property != null)
                {
                    ParameterExpression pe = Expression.Parameter(typeof(T), "value");
                    return new Reference<T>(
                        Expression.Lambda<Func<T>>(memberExp).Compile(),
                        Expression.Lambda<Action<T>>(
                            Expression.Assign(Expression.Property(memberExp.Expression, property), pe),
                            pe).Compile());
                }

                Debug.Assert(false);
                throw new ArgumentException("Unexpected expression.", nameof(refExp));
            }

            throw new NotSupportedException();
        }

        /// <summary>
        ///     Creates a <see cref="Reference{T}" /> with the specified getter and setter..
        /// </summary>
        /// <typeparam name="T">The type of the value being referenced.</typeparam>
        /// <param name="getter">The getter function.</param>
        /// <param name="setter">The setter action.</param>
        /// <returns>A <see cref="Reference{T}" /> with the getter and setter given.</returns>
        public static Reference<T> Create<T>([NotNull] Func<T> getter, [NotNull] Action<T> setter)
            => new Reference<T>(getter, setter);
    }

    /// <summary>
    ///     Defines a reference to some value.
    /// </summary>
    public class Reference<T>
    {
        [NotNull]
        private readonly Func<T> _getter;

        [NotNull]
        private readonly Action<T> _setter;

        /// <summary>
        ///     Initializes a new instance of the <see cref="Reference{T}" /> class.
        /// </summary>
        /// <param name="getter">The getter function.</param>
        /// <param name="setter">The setter action.</param>
        public Reference([NotNull] Func<T> getter, [NotNull] Action<T> setter)
        {
            if (getter == null) throw new ArgumentNullException(nameof(getter));
            if (setter == null) throw new ArgumentNullException(nameof(setter));
            _getter = getter;
            _setter = setter;
        }

        /// <summary>
        ///     Gets or sets the value.
        /// </summary>
        /// <value>
        ///     The value.
        /// </value>
        public T Value
        {
            get { return _getter(); }
            set { _setter(value); }
        }
    }
}