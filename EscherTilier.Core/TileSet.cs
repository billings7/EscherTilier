using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using EscherTilier.Utilities;
using JetBrains.Annotations;

namespace EscherTilier
{
    internal class TileSet : IReadOnlyCollection<TileBase>
    {
        [NotNull]
        private readonly HashSet<TileBase> _tiles = new HashSet<TileBase>();

        [NotNull]
        private readonly Dictionary<EPPos, AdjTiles> _tilesByPos = new Dictionary<EPPos, AdjTiles>();

        /// <summary>
        ///     Initializes a new instance of the <see cref="TileSet" /> class.
        /// </summary>
        public TileSet() { }

        /// <summary>
        ///     Initializes a new instance of the <see cref="TileSet" /> class.
        /// </summary>
        /// <param name="tiles">The tiles.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public TileSet([NotNull] IEnumerable<TileBase> tiles)
        {
            if (tiles == null) throw new ArgumentNullException(nameof(tiles));

            foreach (TileBase tile in tiles)
                Add(tile);
        }

        public int Count => _tiles.Count;

        /// <summary>
        ///     Adds the specified tile.
        /// </summary>
        /// <param name="tile">The tile.</param>
        public void Add(TileBase tile)
        {
            _tiles.Add(tile);

            foreach (EdgePartPosition partPos in tile.PartShapes.Select(p => tile.GetEdgePartPosition(p.Part)))
            {
                Debug.Assert(partPos.Part != null, "partPos.PartShape != null");

                _tilesByPos.AddOrUpdate(
                    new EPPos(partPos.Start, partPos.End),
                    e => new AdjTiles(tile, partPos.Part),
                    (adj, _) => adj.Add(tile, partPos.Part));
            }
        }

        /// <summary>
        ///     Determines whether this set contains the tile.
        /// </summary>
        /// <param name="tile">The tile.</param>
        /// <returns></returns>
        public bool Contains(TileBase tile) => _tiles.Contains(tile);

        /// <summary>
        ///     Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        ///     An enumerator that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<TileBase> GetEnumerator() => _tiles.GetEnumerator();

        /// <summary>
        ///     Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        ///     An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator() => _tiles.GetEnumerator();

        private struct EPPos : IEquatable<EPPos>
        {
            /// <summary>
            ///     The start
            /// </summary>
            public readonly Vector2 Start;

            /// <summary>
            ///     The end
            /// </summary>
            public readonly Vector2 End;

            /// <summary>
            ///     Initializes a new instance of the <see cref="EPPos" /> struct.
            /// </summary>
            /// <param name="start">The start.</param>
            /// <param name="end">The end.</param>
            public EPPos(Vector2 start, Vector2 end)
            {
                start = start.Round(3);
                end = end.Round(3);

                if (start.X < end.X)
                {
                    Start = start;
                    End = end;
                }
                else if (start.X > end.X)
                {
                    Start = end;
                    End = start;
                }
                else if (start.Y <= end.Y)
                {
                    Start = start;
                    End = end;
                }
                else
                {
                    Start = end;
                    End = start;
                }
            }

            /// <summary>
            ///     Indicates whether the current object is equal to another object of the same type.
            /// </summary>
            /// <returns>
            ///     true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.
            /// </returns>
            /// <param name="other">An object to compare with this object.</param>
            public bool Equals(EPPos other)
            {
                return Start.Equals(other.Start) && End.Equals(other.End);
            }

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
                return obj is EPPos && Equals((EPPos) obj);
            }

            /// <summary>
            ///     Returns the hash code for this instance.
            /// </summary>
            /// <returns>
            ///     A 32-bit signed integer that is the hash code for this instance.
            /// </returns>
            public override int GetHashCode()
            {
                unchecked
                {
                    return (Start.GetHashCode() * 397) ^ End.GetHashCode();
                }
            }

            /// <summary>
            ///     Implements the operator ==.
            /// </summary>
            /// <param name="left">The left.</param>
            /// <param name="right">The right.</param>
            /// <returns>
            ///     The result of the operator.
            /// </returns>
            public static bool operator ==(EPPos left, EPPos right)
            {
                return left.Equals(right);
            }

            /// <summary>
            ///     Implements the operator !=.
            /// </summary>
            /// <param name="left">The left.</param>
            /// <param name="right">The right.</param>
            /// <returns>
            ///     The result of the operator.
            /// </returns>
            public static bool operator !=(EPPos left, EPPos right)
            {
                return !left.Equals(right);
            }

            /// <summary>
            ///     Returns the fully qualified type name of this instance.
            /// </summary>
            /// <returns>
            ///     A <see cref="T:System.String" /> containing a fully qualified type name.
            /// </returns>
            public override string ToString() => $"{Start} - {End}";
        }

        private class AdjTiles
        {
            [NotNull]
            public TileBase TileA { get; }

            [NotNull]
            public EdgePart PartA { get; }

            [CanBeNull]
            public TileBase TileB { get; private set; }

            [CanBeNull]
            public EdgePart PartB { get; private set; }

            public AdjTiles([NotNull] TileBase tileA, [NotNull] EdgePart partA)
            {
                Debug.Assert(tileA != null, "tileA != null");
                Debug.Assert(partA != null, "partA != null");

                TileA = tileA;
                PartA = partA;
            }

            [NotNull]
            public AdjTiles Add([NotNull] TileBase tile, [NotNull] EdgePart part)
            {
                if (TileB != null) throw new InvalidOperationException("The tile has already been set.");

                Debug.Assert(tile != null, "tile != null");
                Debug.Assert(part != null, "part != null");

                TileB = tile;
                PartB = part;

                TileBase.SetAdjacent(TileA, PartA, TileB, PartB);

                return this;
            }
        }
    }
}