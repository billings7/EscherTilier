using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using EscherTilier.Numerics;
using EscherTilier.Styles;
using EscherTilier.Utilities;
using JetBrains.Annotations;

namespace EscherTilier
{
    public class Tiling
    {
        [NotNull]
        private readonly Dictionary<EdgePart, Tile> _tileByEdgePart = new Dictionary<EdgePart, Tile>();

        public Tiling(
            [NotNull] Template template,
            [NotNull] TilingDefinition definition,
            [NotNull] IReadOnlyList<Tile> tiles,
            [NotNull] StyleManager styleManager)
        {
            if (template == null) throw new ArgumentNullException(nameof(template));
            if (definition == null) throw new ArgumentNullException(nameof(definition));
            if (tiles == null) throw new ArgumentNullException(nameof(tiles));
            if (styleManager == null) throw new ArgumentNullException(nameof(styleManager));

            Template = template;
            Definition = definition;
            Tiles = tiles;
            StyleManager = styleManager;

            foreach (Tile tile in tiles)
                foreach (EdgePartShape part in tile.PartShapes)
                    _tileByEdgePart.Add(part.Part, tile);
        }

        [NotNull]
        public Template Template { get; }

        [NotNull]
        public TilingDefinition Definition { get; }

        [NotNull]
        [ItemNotNull]
        public IReadOnlyList<Tile> Tiles { get; }

        [NotNull]
        public StyleManager StyleManager { get; set; }

        [NotNull]
        public IEnumerable<TileBase> GetTiles(
            Rectangle bounds,
            [NotNull] StyleManager styleManager,
            [NotNull] IEnumerable<TileBase> existingTiles)
        {
            //startTile = the tile to start tiling from

            //Function GetTiles(Rectangle bounds, StyleManager styleManager, ITile[] existingTiles) : ITile[]
            //Begin

            //    tiles = []
            //    openTiles = []
            TileSet tiles = new TileSet();
            Queue<TileBase> openTiles = new Queue<TileBase>();

            List<TileBase> removeTiles = new List<TileBase>();

            foreach (TileBase tile in existingTiles)
            {
                // if tile is in bounds
                if (bounds.IntersectsWith(tile.GetApproximateBounds()))
                {
                    tiles.Add(tile);

                    if (tile.GetOpenEdgeParts().Any())
                        openTiles.Enqueue(tile);
                }
                else
                    removeTiles.Add(tile);
            }

            foreach (TileBase tile in removeTiles)
                tile.RemoveAdjacent();

            if (tiles.Count < 1)
            {
                //        add startTile to tiles
                tiles.Add(Tiles[0]);

                //        add startTile to end of openTiles
                openTiles.Enqueue(Tiles[0]);
            }

            //    while openTiles is not empty
            while (openTiles.Count > 0)
            {
                //     tile = get and remove tile from start of openTiles
                TileBase tile = openTiles.Dequeue();

                //     for each edgePart with no neighbour
                foreach (EdgePart edgePart in tile.GetOpenEdgeParts())
                {
                    //     newTile = CreateNewTile(tile, edgePart)
                    TileBase newTile = CreateNewTile(tile, edgePart, styleManager);

                    //     set edgePart neighbour to newTile
                    //     add newTile to tiles
                    tiles.Add(newTile);

                    //     if newTile is in bounds
                    if (bounds.IntersectsWith(newTile.GetApproximateBounds()))
                    {
                        //     add newTile to end of openTiles
                        openTiles.Enqueue(newTile);
                    }
                }
                //     end
            }
            // end while

            return tiles.ToArray();
            //    return tiles
            //End
        }

        //Function CreateNewTile(ITile tile, EdgePart part) : ITile
        //Begin
        private TileBase CreateNewTile(TileBase tile, EdgePart part, StyleManager styleManager)
        {
            //    adjacent = get adjacent part from part.AdjacentParts with tile.Label
            Labeled<EdgePart> adjacent;
            if (!Definition.AdjacentParts.TryGetAdjacent(part.WithLabel(tile.Label), out adjacent))
                throw new InvalidOperationException();

            Tile adjTile;
            if (!_tileByEdgePart.TryGetValue(adjacent.Value, out adjTile))
                throw new InvalidOperationException();

            //    newTile = create tile from the shape adjacent is part of
            //    set newTile label to adjacent.AdjacentLabel
            //    transform newTile so adjacent.EdgePart = part
            TileInstance newTile = new TileInstance(
                adjTile,
                adjacent.Label,
                adjTile.GetEdgePartPosition(adjacent.Value)
                    .GetTransformTo(tile.GetEdgePartPosition(part)));

            //     set cell style from styleManager
            newTile.Style = styleManager.GetStyle(newTile);

            //    return newTile
            return newTile;
        }
        //End
    }

    internal class TileSet : IEnumerable<TileBase>
    {
        [NotNull]
        private readonly HashSet<TileBase> _tiles = new HashSet<TileBase>();

        [NotNull]
        private readonly Dictionary<EPPos, AdjTiles> _tilesByPos = new Dictionary<EPPos, AdjTiles>();

        public TileSet()
        {
        }
        public TileSet([NotNull] IEnumerable<TileBase> tiles)
        {
            if (tiles == null) throw new ArgumentNullException(nameof(tiles));

            foreach (TileBase tile in tiles)
                Add(tile);
        }

        public int Count => _tiles.Count;

        public void Add(TileBase item)
        {
            _tiles.Add(item);

            foreach (EdgePartPosition partPos in item.PartShapes.Select(p => item.GetEdgePartPosition(p.Part)))
            {
                Debug.Assert(partPos.Part != null, "partPos.PartShape != null");

                _tilesByPos.AddOrUpdate(
                    new EPPos(partPos.Start, partPos.End),
                    e => new AdjTiles(item, partPos.Part),
                    (adj, _) => adj.Add(item, partPos.Part));
            }
        }

        public bool Contains(TileBase item) => _tiles.Contains(item);

        public IEnumerator<TileBase> GetEnumerator() => _tiles.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _tiles.GetEnumerator();

        private struct EPPos : IEquatable<EPPos>
        {
            public readonly Vector2 Start;
            public readonly Vector2 End;

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
            /// Indicates whether the current object is equal to another object of the same type.
            /// </summary>
            /// <returns>
            /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
            /// </returns>
            /// <param name="other">An object to compare with this object.</param>
            public bool Equals(EPPos other)
            {
                return Start.Equals(other.Start) && End.Equals(other.End);
            }

            /// <summary>
            /// Indicates whether this instance and a specified object are equal.
            /// </summary>
            /// <returns>
            /// true if <paramref name="obj"/> and this instance are the same type and represent the same value; otherwise, false.
            /// </returns>
            /// <param name="obj">The object to compare with the current instance. </param>
            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                return obj is EPPos && Equals((EPPos) obj);
            }

            /// <summary>
            /// Returns the hash code for this instance.
            /// </summary>
            /// <returns>
            /// A 32-bit signed integer that is the hash code for this instance.
            /// </returns>
            public override int GetHashCode()
            {
                unchecked
                {
                    return (Start.GetHashCode() * 397) ^ End.GetHashCode();
                }
            }

            /// <summary>
            /// Implements the operator ==.
            /// </summary>
            /// <param name="left">The left.</param>
            /// <param name="right">The right.</param>
            /// <returns>
            /// The result of the operator.
            /// </returns>
            public static bool operator ==(EPPos left, EPPos right)
            {
                return left.Equals(right);
            }

            /// <summary>
            /// Implements the operator !=.
            /// </summary>
            /// <param name="left">The left.</param>
            /// <param name="right">The right.</param>
            /// <returns>
            /// The result of the operator.
            /// </returns>
            public static bool operator !=(EPPos left, EPPos right)
            {
                return !left.Equals(right);
            }

            /// <summary>
            /// Returns the fully qualified type name of this instance.
            /// </summary>
            /// <returns>
            /// A <see cref="T:System.String"/> containing a fully qualified type name.
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