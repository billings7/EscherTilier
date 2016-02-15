using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace EscherTilier
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
        /// <param name="comparer">The comparer to used to check for equality.</param>
        /// <returns>The index of the value if found, otherwise <c>-1</c>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumerable"/> was <see langword="null"/>.</exception>
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
        /// <param name="comparer">The comparer to used to check for equality.</param>
        /// <returns><see langword="true" /> if all the elements are distinct; otherwise <see langword="false" />.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumerable"/> was <see langword="null"/>.</exception>
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
        /// Determines whether the value returned by the specified selector function for all the elements in an enumerable are distinct.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of the enumerable.</typeparam>
        /// <typeparam name="TResult">The type of the values that must be distinct.</typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <param name="selector">The selector.</param>
        /// <param name="comparer">The comparer to used to check for equality.</param>
        /// <returns>
        ///   <see langword="true" /> if all the elements are distinct; otherwise <see langword="false" />.
        /// </returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="ArgumentNullException"><paramref name="enumerable" /> was <see langword="null" />.</exception>
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