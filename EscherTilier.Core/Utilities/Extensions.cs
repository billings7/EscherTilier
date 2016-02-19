using System;
using System.Collections.Generic;
using System.Linq;
using EscherTilier.Expressions;
using JetBrains.Annotations;

namespace EscherTilier.Utilities
{
    public static class Extensions
    {
        /// <summary>
        ///     Returns a labeled version of the object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <param name="label">The label.</param>
        public static Labeled<T> WithLabel<T>(this T value, [NotNull] string label)
        {
            if (label == null) throw new ArgumentNullException(nameof(label));

            return new Labeled<T>(label, value);
        }

        /// <summary>
        ///     Gets the index of the first occurance of a value in an enumerable.
        /// </summary>
        /// <typeparam name="T">The type of the elements of the enumerable.</typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <param name="value">The value to find the index of.</param>
        /// <param name="comparer">
        ///     The comparer used to check for equality, or <see langword="null" /> to use the default comparer
        ///     for the type.
        /// </param>
        /// <returns>The index of the value if found, otherwise <c>-1</c>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumerable" /> was <see langword="null" />.</exception>
        /// <exception cref="ArgumentException">
        ///     The the value was not in the first <see cref="int.MaxValue" /> elements of the
        ///     enumerable.
        /// </exception>
        public static int IndexOf<T>(
            [NotNull] this IEnumerable<T> enumerable,
            [CanBeNull] T value,
            [CanBeNull] IEqualityComparer<T> comparer = null)
        {
            if (enumerable == null) throw new ArgumentNullException(nameof(enumerable));
            comparer = comparer ?? EqualityComparer<T>.Default;
            int i = 0;
            foreach (T val in enumerable)
            {
                if (comparer.Equals(val, value)) return i;
                if (i == int.MaxValue)
                    throw new ArgumentException(Strings.Extensions_IndexOf_TooLarge, nameof(enumerable));
                i++;
            }
            return -1;
        }

        /// <summary>
        ///     Determines whether all the elements in an enumerable are distinct.
        /// </summary>
        /// <typeparam name="T">The type of the elements of the enumerable.</typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <param name="comparer">
        ///     The comparer used to check for equality, or <see langword="null" /> to use the default comparer
        ///     for the type.
        /// </param>
        /// <returns><see langword="true" /> if all the elements are distinct; otherwise <see langword="false" />.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumerable" /> was <see langword="null" />.</exception>
        public static bool AreDistinct<T>(
            [NotNull] this IEnumerable<T> enumerable,
            [CanBeNull] IEqualityComparer<T> comparer = null)
        {
            if (enumerable == null) throw new ArgumentNullException(nameof(enumerable));
            comparer = comparer ?? EqualityComparer<T>.Default;
            HashSet<T> set = new HashSet<T>(comparer);
            foreach (T val in enumerable)
                if (!set.Add(val)) return false;
            return true;
        }

        /// <summary>
        ///     Determines whether the value returned by the specified selector function for all the elements in an enumerable are
        ///     distinct.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of the enumerable.</typeparam>
        /// <typeparam name="TResult">The type of the values that must be distinct.</typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <param name="selector">The selector.</param>
        /// <param name="comparer">
        ///     The comparer used to check for equality, or <see langword="null" /> to use the default comparer
        ///     for the type.
        /// </param>
        /// <returns>
        ///     <see langword="true" /> if all the elements are distinct; otherwise <see langword="false" />.
        /// </returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="enumerable" /> was <see langword="null" />.</exception>
        public static bool AreDistinct<TSource, TResult>(
            [NotNull] this IEnumerable<TSource> enumerable,
            [NotNull] Func<TSource, TResult> selector,
            [CanBeNull] IEqualityComparer<TResult> comparer = null)
        {
            if (enumerable == null) throw new ArgumentNullException(nameof(enumerable));
            comparer = comparer ?? EqualityComparer<TResult>.Default;
            HashSet<TResult> set = new HashSet<TResult>(comparer);
            foreach (TSource val in enumerable)
                if (!set.Add(selector(val))) return false;
            return true;
        }

        /// <summary>
        ///     Determines if all the elements in an enumerable are equal to each other.
        /// </summary>
        /// <typeparam name="T">The type of the elements of the enumerable.</typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <param name="comparer">
        ///     The comparer used to check for equality, or <see langword="null" /> to use the default comparer
        ///     for the type.
        /// </param>
        /// <returns>
        ///     <see langword="true" /> if all the elements are equal; otherwise <see langword="false" />.
        /// </returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="enumerable" /> was <see langword="null" />.</exception>
        public static bool AreEqual<T>([NotNull] this IEnumerable<T> enumerable, IEqualityComparer<T> comparer = null)
        {
            if (enumerable == null) throw new ArgumentNullException(nameof(enumerable));
            comparer = comparer ?? EqualityComparer<T>.Default;

            using (IEnumerator<T> enumerator = enumerable.GetEnumerator())
            {
                if (!enumerator.MoveNext()) return true;

                T first = enumerator.Current;

                while (enumerator.MoveNext())
                {
                    if (!comparer.Equals(first, enumerator.Current))
                        return false;
                }

                return true;
            }
        }

        /// <summary>
        /// Compiles the specified expression.
        /// </summary>
        /// <typeparam name="T">The type of the value of the expression.</typeparam>
        /// <param name="expression">The expression to compile.</param>
        /// <returns>
        /// The compiled expression.
        /// </returns>
        [NotNull]
        public static CompiledExpression<T> Compile<T>([NotNull] this IExpression<T> expression)
                    => CompiledExpression<T>.Compile(expression);

        /// <summary>
        /// Gets the value for the key given, adding it if it doesnt exist.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="key">The key.</param>
        /// <param name="factory">The factory.</param>
        /// <returns></returns>
        public static TValue GetOrAdd<TKey, TValue>(
                    [NotNull] this Dictionary<TKey, TValue> dictionary,
                    [NotNull] TKey key,
                    [NotNull] Func<TKey, TValue> factory)
        {
            if (dictionary == null) throw new ArgumentNullException(nameof(dictionary));
            if (factory == null) throw new ArgumentNullException(nameof(factory));

            TValue value;
            if (dictionary.TryGetValue(key, out value))
                return value;

            value = factory(key);
            dictionary.Add(key, value);
            return value;
        }
    }

    /// <summary>
    ///     Helpter methods for arrays.
    /// </summary>
    /// <typeparam name="T">The type of the element of the array.</typeparam>
    public static class Array<T>
    {
        /// <summary>
        ///     An instance of an empty array of type <typeparamref name="T" />.
        /// </summary>
        [NotNull]
        public static readonly T[] Empty = Enumerable.Empty<T>() as T[] ?? new T[0];
    }
}