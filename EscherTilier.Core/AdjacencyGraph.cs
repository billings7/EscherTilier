using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using JetBrains.Annotations;

namespace EscherTilier
{
    /// <summary>
    ///     Represents a graph of adjacent values.
    /// </summary>
    /// <typeparam name="T">The type of the nodes of the graph.</typeparam>
    public class AdjacencyGraph<T> : IAdjacencyGraph<T>
    {
        [NotNull]
        private readonly IEqualityComparer<T> _comparer;

        [NotNull]
        private readonly HashSet<T> _all;

        [NotNull]
        private readonly Dictionary<T, Dictionary<T, bool>> _adjacencies;
        
        /// <summary>
        ///     Initializes a new instance of the <see cref="AdjacencyGraph{T}" /> class.
        /// </summary>
        public AdjacencyGraph()
            : this(null) { }

        /// <summary>
        ///     Initializes a new instance of the <see cref="AdjacencyGraph{T}" /> class.
        /// </summary>
        /// <param name="comparer">The comparer used to compared values in the list.</param>
        public AdjacencyGraph([CanBeNull] IEqualityComparer<T> comparer)
        {
            _comparer = comparer ?? EqualityComparer<T>.Default;
            _adjacencies = new Dictionary<T, Dictionary<T, bool>>(_comparer);
            _all = new HashSet<T>(_comparer);
        }

        /// <summary>
        ///     Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        ///     An enumerator that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<T> GetEnumerator() => _all.GetEnumerator();

        /// <summary>
        ///     Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        ///     An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        ///     Gets the number of elements in the collection.
        /// </summary>
        /// <returns>
        ///     The number of elements in the collection.
        /// </returns>
        public int Count => _all.Count;

        /// <summary>
        ///     Gets all adjacencies in the list.
        /// </summary>
        /// <value>
        ///     All adjacencies.
        /// </value>
        public IEnumerable<Tuple<T, T>> AllAdjacencies
        {
            get
            {
                foreach (KeyValuePair<T, Dictionary<T, bool>> kvp in _adjacencies)
                    foreach (T val in kvp.Value.Where(e => e.Value).Select(e => e.Key))
                        yield return Tuple.Create(kvp.Key, val);
            }
        }

        /// <summary>
        ///     Adds an adjacency between two values.
        /// </summary>
        /// <param name="a">The first adjacent value.</param>
        /// <param name="b">The second adjacent value.</param>
        /// <returns><see langword="true" /> if the adjacency was added, <see langword="false" /> if it already existed.</returns>
        public bool Add([NotNull] T a, [NotNull] T b)
        {
            if (a == null) throw new ArgumentNullException(nameof(a));
            if (b == null) throw new ArgumentNullException(nameof(b));
            if (_comparer.Equals(a, b)) throw new ArgumentException(Strings.AdjacencyList_Add_SameValues);

            _all.Add(a);
            _all.Add(b);
            Dictionary<T, bool> set;
            if (!_adjacencies.TryGetValue(a, out set))
                _adjacencies[a] = set = new Dictionary<T, bool>(_comparer);
            Debug.Assert(set != null, "set != null");

            if (set.ContainsKey(b)) return false;
            set.Add(b, true);

            if (!_adjacencies.TryGetValue(b, out set))
                _adjacencies[b] = set = new Dictionary<T, bool>(_comparer);
            Debug.Assert(set != null, "set != null");

            Debug.Assert(!set.ContainsKey(a));
            set.Add(a, false);

            return true;
        }

        /// <summary>
        ///     Removes the adjacency between two values.
        /// </summary>
        /// <param name="a">The first adjacent value.</param>
        /// <param name="b">The second adjacent value.</param>
        /// <returns><see langword="true" /> if the adjacency was remove, <see langword="false" /> if it did not exist.</returns>
        public bool Remove([NotNull] T a, [NotNull] T b)
        {
            if (a == null) throw new ArgumentNullException(nameof(a));
            if (b == null) throw new ArgumentNullException(nameof(b));

            Dictionary<T, bool> set;
            if (!_adjacencies.TryGetValue(a, out set))
                return false;

            Debug.Assert(set != null, "set != null");
            if (!set.Remove(b))
                return false;

            if (set.Count < 1)
            {
                _adjacencies.Remove(a);
                _all.Remove(a);
            }

            if (!_adjacencies.TryGetValue(b, out set))
            {
                Debug.Assert(false);
                return true;
            }
            Debug.Assert(set != null, "set != null");

            if (!set.Remove(a))
                Debug.Assert(false);

            if (set.Count < 1)
            {
                _adjacencies.Remove(b);
                _all.Remove(b);
            }

            return true;
        }

        /// <summary>
        ///     Gets the values adjacent to the value given.
        /// </summary>
        /// <param name="value">The value to get the adjacent values for.</param>
        /// <returns>The adjacent values.</returns>
        [NotNull]
        [ItemNotNull]
        public IEnumerable<T> GetAdjacent(T value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            Dictionary<T, bool> set;
            if (!_adjacencies.TryGetValue(value, out set))
                return Enumerable.Empty<T>();

            Debug.Assert(set != null, "set != null");
            return set.Keys;
        }

        /// <summary>
        ///     Attempts to get the values adjacent to the value given.
        /// </summary>
        /// <param name="value">The value to get the adjacent values for.</param>
        /// <param name="adjacent">The adjacent values.</param>
        /// <returns><see langword="true" /> if there are adjacencies; otherwise <see langword="false" />.</returns>
        public bool TryGetAdjacent(T value, [ItemNotNull] out IEnumerable<T> adjacent)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            Dictionary<T, bool> set;
            if (_adjacencies.TryGetValue(value, out set))
            {
                Debug.Assert(set != null, "set != null");
                adjacent = set.Keys;
                return true;
            }

            adjacent = Enumerable.Empty<T>();
            return false;
        }

        /// <summary>
        ///     Determines whether the graph contains the value given.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public bool Contains(T value) => _all.Contains(value);

        /// <summary>
        ///     Determines whether the graph contains an adjacency between the values given.
        /// </summary>
        /// <param name="a">The first adjacent value.</param>
        /// <param name="b">The second adjacent value.</param>
        /// <returns></returns>
        public bool Contains(T a, T b)
        {
            Dictionary<T, bool> set;
            // ReSharper disable once PossibleNullReferenceException
            return _adjacencies.TryGetValue(a, out set) && set.ContainsKey(b);
        }
    }

    /// <summary>
    /// An interface to an graph that represents adjacent values.
    /// </summary>
    /// <typeparam name="T">The type of the nodes of the graph.</typeparam>
    public interface IAdjacencyGraph<T> : IReadOnlyCollection<T>
    {
        /// <summary>
        ///     Gets all adjacencies in the list.
        /// </summary>
        /// <value>
        ///     All adjacencies.
        /// </value>
        [NotNull]
        [ItemNotNull]
        IEnumerable<Tuple<T, T>> AllAdjacencies { get; }

        /// <summary>
        ///     Gets the values adjacent to the value given.
        /// </summary>
        /// <param name="value">The value to get the adjacent values for.</param>
        /// <returns>The adjacent values.</returns>
        [NotNull]
        [ItemNotNull]
        IEnumerable<T> GetAdjacent([NotNull] T value);

        /// <summary>
        ///     Attempts to get the values adjacent to the value given.
        /// </summary>
        /// <param name="value">The value to get the adjacent values for.</param>
        /// <param name="adjacent">The adjacent values.</param>
        /// <returns><see langword="true" /> if there are adjacencies; otherwise <see langword="false" />.</returns>
        bool TryGetAdjacent([NotNull] T value, [NotNull] [ItemNotNull] out IEnumerable<T> adjacent);

        /// <summary>
        ///     Determines whether the graph contains the value given.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        bool Contains([NotNull] T value);

        /// <summary>
        ///     Determines whether the graph contains an adjacency between the values given.
        /// </summary>
        /// <param name="a">The first adjacent value.</param>
        /// <param name="b">The second adjacent value.</param>
        /// <returns></returns>
        bool Contains([NotNull] T a, [NotNull] T b);
    }
}