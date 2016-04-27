using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using EscherTiler.Graphics;
using EscherTiler.Numerics;
using EscherTiler.Styles;
using JetBrains.Annotations;

namespace EscherTiler
{
    /// <summary>
    ///     Base class for tiles.
    /// </summary>
    public abstract class TileBase
    {
        [NotNull]
        private readonly Dictionary<EdgePart, TileBase> _adjacentTiles = new Dictionary<EdgePart, TileBase>();

        /// <summary>
        ///     Initializes a new instance of the <see cref="TileBase" /> class.
        /// </summary>
        /// <param name="label">The label.</param>
        /// <param name="shape">The shape.</param>
        /// <param name="transform">The transform.</param>
        /// <exception cref="System.ArgumentNullException">
        ///     <paramref name="label" /> or <paramref name="shape" /> was null.
        /// </exception>
        public TileBase([NotNull] string label, [NotNull] Shape shape, Matrix3x2 transform)
        {
            if (label == null) throw new ArgumentNullException(nameof(label));
            if (shape == null) throw new ArgumentNullException(nameof(shape));

            Shape = shape;
            Transform = transform;
            Label = label;
        }

        /// <summary>
        ///     Gets the tile label.
        /// </summary>
        /// <value>
        ///     The label.
        /// </value>
        [NotNull]
        public string Label { get; }

        /// <summary>
        ///     Gets or sets the style of this tile.
        /// </summary>
        /// <value>
        ///     The style.
        /// </value>
        [CanBeNull]
        public IStyle Style { get; set; }

        /// <summary>
        ///     Gets the matrix transform applied to this tile.
        /// </summary>
        /// <value>
        ///     The transform.
        /// </value>
        public Matrix3x2 Transform { get; }

        /// <summary>
        ///     Gets the shape that this tile has.
        /// </summary>
        /// <value>
        ///     The shape.
        /// </value>
        [NotNull]
        public Shape Shape { get; }

        /// <summary>
        ///     Gets the centroid point of the tile.
        /// </summary>
        /// <value>
        ///     The centroid point.
        /// </value>
        public Vector2 Centroid => Vector2.Transform(Shape.Centroid, Transform);

        /// <summary>
        ///     Gets the tiles adjacent to this tile.
        /// </summary>
        /// <value>
        ///     The adjacent tiles.
        /// </value>
        [NotNull]
        public IReadOnlyDictionary<EdgePart, TileBase> AdjacentTiles => _adjacentTiles;

        /// <summary>
        ///     Gets the shapes of the parts of this tiles edges.
        /// </summary>
        /// <value>
        ///     The part shapes.
        /// </value>
        [NotNull]
        [ItemNotNull]
        public abstract IReadOnlyList<EdgePartShape> PartShapes { get; }

        /// <summary>
        ///     Populates the graphics path given from this tile.
        /// </summary>
        /// <param name="path">The path.</param>
        public void PopulateGraphicsPath([NotNull] IGraphicsPath path)
        {
            if (path == null) throw new ArgumentNullException(nameof(path));

            bool first = true;

            Matrix3x2 lastTransform = Matrix3x2.Identity;
            Edge lastEdge = null;
            foreach (EdgePartShape partShape in PartShapes)
            {
                Matrix3x2 edgeTransform = partShape.Edge == lastEdge
                    ? lastTransform
                    : partShape.GetLineTransform() * Transform;

                lastTransform = edgeTransform;
                lastEdge = partShape.Edge;

                IEnumerable<ILine> lines = partShape.Lines;
                foreach (ILine line in partShape.Part.IsClockwise ? lines : lines.Reverse())
                {
                    if (first)
                    {
                        first = false;
                        path.Start(Vector2.Transform(partShape.Part.IsClockwise ? line.Start : line.End, edgeTransform));
                    }

                    line.AddToPath(path, edgeTransform, !partShape.Part.IsClockwise);
                }
            }

            path.End();
        }

        /// <summary>
        ///     Gets this tiles edge parts that are not adjacent to another tile.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public IEnumerable<EdgePart> GetOpenEdgeParts()
        {
            foreach (EdgePartShape partShape in PartShapes)
            {
                if (!_adjacentTiles.ContainsKey(partShape.Part))
                    yield return partShape.Part;
            }
        }

        /// <summary>
        ///     Gets an approximate bounding rectangle for this tile.
        ///     The rectangle returned should equal or contain the actual bounds.
        /// </summary>
        /// <returns></returns>
        public Rectangle GetApproximateBounds()
        {
            Rectangle rect = default(Rectangle);
            bool first = true;

            Matrix3x2 lastTransform = Matrix3x2.Identity;
            Edge lastEdge = null;

            foreach (EdgePartShape partShape in PartShapes)
            {
                Matrix3x2 edgeTransform = partShape.Edge == lastEdge
                    ? lastTransform
                    : partShape.GetLineTransform() * Transform;

                lastTransform = edgeTransform;
                lastEdge = partShape.Edge;

                foreach (ILine line in partShape.Lines)
                {
                    Rectangle lineBounds = line.GetApproximateBounds(edgeTransform);
                    rect = first ? lineBounds : Rectangle.Union(rect, lineBounds);
                    first = false;
                }
            }

            return rect;
        }

        /// <summary>
        ///     Gets the position of the given edge part for this tile.
        /// </summary>
        /// <param name="edgePart">The edge part.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="edgePart" /> was <see langword="null" />.</exception>
        /// <exception cref="KeyNotFoundException">The given edge part is not part of any edge of this shape.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public EdgePartPosition GetEdgePartPosition([NotNull] EdgePart edgePart)
            => EdgePartPosition.Create(edgePart, Shape, Transform);

        /// <summary>
        ///     Adds an adjacency between two tiles..
        /// </summary>
        /// <param name="tileA">The first tile in the adjacency.</param>
        /// <param name="edgePartA">The part of the first tile that is adjacent to the second tile.</param>
        /// <param name="tileB">The second tile in the adjacency.</param>
        /// <param name="edgePartB">The part of the second tile that is adjacent to the first tile.</param>
        /// <exception cref="System.ArgumentNullException">
        ///     One of the parameters was null.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetAdjacent(
            [NotNull] TileBase tileA,
            [NotNull] EdgePart edgePartA,
            [NotNull] TileBase tileB,
            [NotNull] EdgePart edgePartB)
        {
            if (tileA == null) throw new ArgumentNullException(nameof(tileA));
            if (edgePartA == null) throw new ArgumentNullException(nameof(edgePartA));
            if (tileB == null) throw new ArgumentNullException(nameof(tileB));
            if (edgePartB == null) throw new ArgumentNullException(nameof(edgePartB));

            tileA._adjacentTiles[edgePartA] = tileB;
            tileB._adjacentTiles[edgePartB] = tileA;
        }

        /// <summary>
        ///     Removes all the adjacencies between this and other tiles.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RemoveAdjacent()
        {
            foreach (TileBase tile in _adjacentTiles.Values.Distinct().ToArray())
                RemoveAdjacent(this, tile);
        }

        /// <summary>
        ///     Removes the adjacency between this and another tile.
        /// </summary>
        /// <param name="otherTile">The other tile.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RemoveAdjacent([NotNull] TileBase otherTile) => RemoveAdjacent(this, otherTile);

        /// <summary>
        ///     Removes the adjacency between the two tiles given.
        /// </summary>
        /// <param name="tileA">The tile a.</param>
        /// <param name="tileB">The tile b.</param>
        /// <exception cref="System.ArgumentNullException">
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void RemoveAdjacent([NotNull] TileBase tileA, [NotNull] TileBase tileB)
        {
            if (tileA == null) throw new ArgumentNullException(nameof(tileA));
            if (tileB == null) throw new ArgumentNullException(nameof(tileB));

            tileA.RemoveAdjacentTiles(tileB);
            tileB.RemoveAdjacentTiles(tileA);
        }

        /// <summary>
        ///     Removes the tile given from the adjacent tiles dictionary.
        /// </summary>
        /// <param name="other">The other tile to remove.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void RemoveAdjacentTiles(TileBase other)
        {
            foreach (KeyValuePair<EdgePart, TileBase> kvp in _adjacentTiles.Where(kvp => kvp.Value == other).ToArray())
                _adjacentTiles.Remove(kvp.Key);
        }
    }
}