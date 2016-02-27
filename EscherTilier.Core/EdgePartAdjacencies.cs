using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using JetBrains.Annotations;

namespace EscherTilier
{
    /// <summary>
    ///     Defines adjacent <see cref="EdgePart">edge parts</see>.
    /// </summary>
    public class EdgePartAdjacencies : IEnumerable<Labeled<EdgePart>>
    {
        [NotNull]
        private readonly HashSet<Adjacency> _adjacencies;

        [NotNull]
        private readonly Dictionary<Labeled<EdgePart>, Labeled<EdgePart>> _all;

        /// <summary>
        ///     Defines an adjacency between two edge parts.
        /// </summary>
        private struct Adjacency : IEquatable<Adjacency>
        {
            public readonly Labeled<EdgePart> PartA;
            public readonly Labeled<EdgePart> PartB;

            public Adjacency(Labeled<EdgePart> partA, Labeled<EdgePart> partB)
            {
                int comp = string.Compare(partA.Label, partB.Label, StringComparison.InvariantCulture);
                if (comp == 0)
                    comp = partA.Value.ID.CompareTo(partB.Value.ID);

                if (comp > 0)
                {
                    PartA = partB;
                    PartB = partA;
                }
                else
                {
                    PartA = partA;
                    PartB = partB;
                }
            }

            /// <summary>
            ///     Indicates whether the current object is equal to another object of the same type.
            /// </summary>
            /// <returns>
            ///     true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.
            /// </returns>
            /// <param name="other">An object to compare with this object.</param>
            public bool Equals(Adjacency other) => PartA.Equals(other.PartA) && PartB.Equals(other.PartB);

            /// <summary>
            ///     Indicates whether this instance and a specified object are equal.
            /// </summary>
            /// <returns>
            ///     true if <paramref name="obj" /> and this instance are the same type and represent the same value; otherwise, false.
            /// </returns>
            /// <param name="obj">The object to compare with the current instance. </param>
            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                return obj is Adjacency && Equals((Adjacency) obj);
            }

            /// <summary>
            ///     Returns the hash code for this instance.
            /// </summary>
            /// <returns>
            ///     A 32-bit signed integer that is the hash code for this instance.
            /// </returns>
            public override int GetHashCode() => unchecked((PartA.GetHashCode() * 397) ^ PartB.GetHashCode());

            /// <summary>
            ///     Implements the operator ==.
            /// </summary>
            /// <param name="left">The left.</param>
            /// <param name="right">The right.</param>
            /// <returns>
            ///     The result of the operator.
            /// </returns>
            public static bool operator ==(Adjacency left, Adjacency right) => left.Equals(right);

            /// <summary>
            ///     Implements the operator !=.
            /// </summary>
            /// <param name="left">The left.</param>
            /// <param name="right">The right.</param>
            /// <returns>
            ///     The result of the operator.
            /// </returns>
            public static bool operator !=(Adjacency left, Adjacency right) => !left.Equals(right);
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="EdgePartAdjacencies" /> class.
        /// </summary>
        public EdgePartAdjacencies()
        {
            _adjacencies = new HashSet<Adjacency>();
            _all = new Dictionary<Labeled<EdgePart>, Labeled<EdgePart>>();
        }

        /// <summary>
        ///     Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        ///     An enumerator that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<Labeled<EdgePart>> GetEnumerator() => _all.Keys.GetEnumerator();

        /// <summary>
        ///     Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        ///     An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        ///     Gets the number of elements in the collection.
        /// </summary>
        /// <returns>
        ///     The number of elements in the collection.
        /// </returns>
        public int Count => _adjacencies.Count;

        /// <summary>
        ///     Gets all adjacencies in the collection.
        /// </summary>
        /// <value>
        ///     All adjacencies.
        /// </value>
        public IEnumerable<Tuple<Labeled<EdgePart>, Labeled<EdgePart>>> AllAdjacencies
            => _adjacencies.Select(a => Tuple.Create(a.PartA, a.PartB));

        /// <summary>
        ///     Adds an adjacency between two edge parts.
        /// </summary>
        /// <param name="a">The first adjacent edge part.</param>
        /// <param name="b">The second adjacent edge part.</param>
        /// <returns>
        ///     <see langword="true" /> if the adjacency was added, <see langword="false" /> if it already existed.
        /// </returns>
        /// <exception cref="System.ArgumentException">
        ///     <paramref name="a" /> or <paramref name="b" /> already existed in this collection.
        /// </exception>
        public bool Add(Labeled<EdgePart> a, Labeled<EdgePart> b)
        {
            if (a == b) throw new ArgumentException(Strings.AdjacencyList_Add_SameValues);

            if (_all.ContainsKey(a))
                throw new ArgumentException(Strings.EdgePartAdjacencies_Add_Duplicate, nameof(a));
            if (_all.ContainsKey(b) && a != b)
                throw new ArgumentException(Strings.EdgePartAdjacencies_Add_Duplicate, nameof(b));

            _all.Add(a, b);
            if (a != b) _all.Add(b, a);

            return _adjacencies.Add(new Adjacency(a, b));
        }

        /// <summary>
        ///     Removes the adjacency between two edge parts.
        /// </summary>
        /// <param name="a">The first adjacent edge part.</param>
        /// <param name="b">The second adjacent edge part.</param>
        /// <returns><see langword="true" /> if the adjacency was removed, <see langword="false" /> if it did not exist.</returns>
        public bool Remove(Labeled<EdgePart> a, Labeled<EdgePart> b)
        {
            if (!_adjacencies.Remove(new Adjacency(a, b)))
                return false;

            bool rema = _all.Remove(a);
            bool remb = _all.Remove(b);
            Debug.Assert(rema && remb, "rema && remb");
            return true;
        }

        /// <summary>
        ///     Gets the edge part adjacent to the value given.
        /// </summary>
        /// <param name="value">The edge part to get the adjacent values for.</param>
        /// <returns>The adjacent values.</returns>
        public Labeled<EdgePart>? GetAdjacent(Labeled<EdgePart> value)
        {
            return _all.TryGetValue(value, out value) ? (Labeled<EdgePart>?) value : null;
        }

        /// <summary>
        ///     Attempts to get the edge part adjacent to the edge part given.
        /// </summary>
        /// <param name="value">The edge part to get the adjacent values for.</param>
        /// <param name="adjacent">The adjacent edge part.</param>
        /// <returns><see langword="true" /> if there is an adjacency; otherwise <see langword="false" />.</returns>
        public bool TryGetAdjacent(Labeled<EdgePart> value, [ItemNotNull] out Labeled<EdgePart> adjacent)
        {
            return _all.TryGetValue(value, out adjacent);
        }

        /// <summary>
        ///     Determines whether the graph contains the edge part given.
        /// </summary>
        /// <param name="value">The edge part.</param>
        /// <returns></returns>
        public bool Contains(Labeled<EdgePart> value) => _all.ContainsKey(value);

        /// <summary>
        ///     Determines whether the graph contains an adjacency between the edge parts given.
        /// </summary>
        /// <param name="a">The first adjacent edge part.</param>
        /// <param name="b">The second adjacent edge part.</param>
        /// <returns></returns>
        public bool Contains(Labeled<EdgePart> a, Labeled<EdgePart> b)
        {
            return _adjacencies.Contains(new Adjacency(a, b));
        }
    }
}