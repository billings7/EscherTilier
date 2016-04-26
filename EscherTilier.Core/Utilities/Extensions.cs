using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Threading;
using EscherTilier.Expressions;
using EscherTilier.Graphics;
using EscherTilier.Graphics.Resources;
using EscherTilier.Styles;
using JetBrains.Annotations;

namespace EscherTilier.Utilities
{
    /// <summary>
    ///     Extension methods.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        ///     Returns a labeled version of the object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <param name="label">The label.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
        ///     Compiles the specified expression.
        /// </summary>
        /// <typeparam name="T">The type of the value of the expression.</typeparam>
        /// <param name="expression">The expression to compile.</param>
        /// <returns>
        ///     The compiled expression.
        /// </returns>
        [NotNull]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static CompiledExpression<T> Compile<T>([NotNull] this IExpression<T> expression)
            => CompiledExpression<T>.Compile(expression);

        /// <summary>
        ///     Gets the value for the key given, adding it if it doesnt exist.
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

        /// <summary>
        ///     Adds or updates the value associated with the given key.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="key">The key.</param>
        /// <param name="factory">The value factory.</param>
        /// <param name="update">The value update function.</param>
        /// <returns></returns>
        public static TValue AddOrUpdate<TKey, TValue>(
            [NotNull] this Dictionary<TKey, TValue> dictionary,
            [NotNull] TKey key,
            [NotNull] Func<TKey, TValue> factory,
            [NotNull] Func<TValue, TKey, TValue> update)
        {
            if (dictionary == null) throw new ArgumentNullException(nameof(dictionary));
            if (factory == null) throw new ArgumentNullException(nameof(factory));
            if (update == null) throw new ArgumentNullException(nameof(update));

            TValue value;
            value = dictionary.TryGetValue(key, out value) ? update(value, key) : factory(key);
            dictionary[key] = value;
            return value;
        }

        /// <summary>
        ///     Attempts to remove the object at the top of the <see cref="Stack{T}" />, returning it if successful.
        /// </summary>
        /// <typeparam name="T">The type of the items in the stack.</typeparam>
        /// <param name="stack">The stack to pop.</param>
        /// <param name="value">The value at the top of the stack, or <see langword="default{T}" /> if the stack is empty.</param>
        /// <returns><see langword="true" /> if the stack was not empty; otherwise <see langword="false" />.</returns>
        public static bool TryPop<T>([NotNull] this Stack<T> stack, out T value)
        {
            if (stack == null) throw new ArgumentNullException(nameof(stack));
            if (stack.Count < 1)
            {
                value = default(T);
                return false;
            }

            value = stack.Pop();
            return true;
        }

        /// <summary>
        ///     Attempts to return the object at the top of the <see cref="Stack{T}" />.
        /// </summary>
        /// <typeparam name="T">The type of the items in the stack.</typeparam>
        /// <param name="stack">The stack to peek.</param>
        /// <param name="value">The value at the top of the stack, or <see langword="default{T}" /> if the stack is empty.</param>
        /// <returns><see langword="true" /> if the stack was not empty; otherwise <see langword="false" />.</returns>
        public static bool TryPeek<T>([NotNull] this Stack<T> stack, out T value)
        {
            if (stack == null) throw new ArgumentNullException(nameof(stack));
            if (stack.Count < 1)
            {
                value = default(T);
                return false;
            }

            value = stack.Peek();
            return true;
        }

        /// <summary>
        ///     Rounds each component of the vector.
        /// </summary>
        /// <param name="vector">The vector to round.</param>
        /// <param name="digits">The number of fractional digits in the return value.</param>
        /// <param name="mode">Specification for how to round a value if it is midway between two other numbers.</param>
        /// <returns>A vector where each component has a number of fractional digits equal to <paramref name="digits" />.</returns>
        public static Vector2 Round(
            this Vector2 vector,
            int digits = 0,
            MidpointRounding mode = MidpointRounding.ToEven)
        {
            return new Vector2(
                (float) Math.Round(vector.X, digits, mode),
                (float) Math.Round(vector.Y, digits, mode));
        }

        /// <summary>
        ///     Saves the current state of the graphics, sets the state using the given action,
        ///     and reutrns an <see cref="IDisposable" /> that can can be disposed to restore the saved state.
        /// </summary>
        /// <param name="graphics">The graphics.</param>
        /// <param name="fillStyle">The fill style, or <see langword="null" /> to leave unchanged.</param>
        /// <param name="lineStyle">The line style, or <see langword="null" /> to leave unchanged.</param>
        /// <param name="lineWidth">The line width, or <see langword="null" /> to leave unchanged.</param>
        /// <param name="transform">The transform, or <see langword="null" /> to leave unchanged.</param>
        /// <param name="resourceManager">The resource manager, or <see langword="null" /> to leave unchanged.</param>
        /// <param name="setState">An action which can be used to change any values that are needed.</param>
        /// <returns>
        ///     An <see cref="IDisposable" /> that can can be disposed to restore the saved state.
        /// </returns>
        public static IDisposable TempState(
            [NotNull] this IGraphics graphics,
            [CanBeNull] IStyle fillStyle = null,
            [CanBeNull] IStyle lineStyle = null,
            [CanBeNull] float? lineWidth = null,
            [CanBeNull] Matrix3x2? transform = null,
            [CanBeNull] IResourceManager resourceManager = null,
            [CanBeNull] Action<IGraphics> setState = null)
        {
            if (graphics == null) throw new ArgumentNullException(nameof(graphics));
            graphics.SaveState();
            try
            {
                if (fillStyle != null) graphics.FillStyle = fillStyle;
                if (lineStyle != null) graphics.LineStyle = lineStyle;
                if (lineWidth != null) graphics.LineWidth = lineWidth.Value;
                if (transform != null) graphics.Transform = transform.Value;
                if (resourceManager != null) graphics.ResourceManager = resourceManager;
                setState?.Invoke(graphics);
                return new TempStateRestorer(graphics);
            }
            catch
            {
                graphics.RestoreState();
                throw;
            }
        }

        /// <summary>
        ///     Object for restoring a state saved in an <see cref="IGraphics" /> instance.
        /// </summary>
        private struct TempStateRestorer : IDisposable
        {
            private IGraphics _graphics;

            /// <summary>
            ///     Initializes a new instance of the <see cref="TempStateRestorer" /> struct.
            /// </summary>
            /// <param name="graphics">The graphics.</param>
            public TempStateRestorer(IGraphics graphics)
            {
                _graphics = graphics;
            }

            /// <summary>
            ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
            /// </summary>
            public void Dispose() => Interlocked.Exchange(ref _graphics, null)?.RestoreState();
        }
    }
}