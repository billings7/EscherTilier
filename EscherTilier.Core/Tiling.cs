using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
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

        [NotNull]
        private StyleManager _styleManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="Tiling"/> class.
        /// </summary>
        /// <param name="template">The template.</param>
        /// <param name="definition">The definition.</param>
        /// <param name="tiles">The tiles.</param>
        /// <param name="styleManager">The style manager.</param>
        /// <exception cref="System.ArgumentNullException">
        /// </exception>
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

            _styleManager = styleManager;
            Template = template;
            Definition = definition;
            Tiles = tiles;

            foreach (Tile tile in tiles)
                foreach (EdgePartShape part in tile.PartShapes)
                    _tileByEdgePart.Add(part.Part, tile);
        }

        /// <summary>
        /// Gets the template this tiling is created from.
        /// </summary>
        /// <value>
        /// The template.
        /// </value>
        [NotNull]
        public Template Template { get; }

        /// <summary>
        /// Gets the tiling definition this tiling is created from.
        /// </summary>
        /// <value>
        /// The definition.
        /// </value>
        [NotNull]
        public TilingDefinition Definition { get; }

        /// <summary>
        /// Gets the root tiles that all other tiles are based on.
        /// </summary>
        /// <value>
        /// The tiles.
        /// </value>
        [NotNull]
        [ItemNotNull]
        public IReadOnlyList<Tile> Tiles { get; }

        /// <summary>
        ///     Gets or sets the style manager this tiling uses.
        /// </summary>
        /// <value>
        ///     The style manager.
        /// </value>
        [NotNull]
        public StyleManager StyleManager => _styleManager;

        /// <summary>
        /// Sets the style manager for the tiling and updates the styles for the given tiles.
        /// </summary>
        /// <param name="manager">The new style manager manager.</param>
        /// <param name="tiles">The tiles.</param>
        public void SetStyleManager([NotNull] StyleManager manager, [NotNull] [ItemNotNull] IEnumerable<TileBase> tiles)
        {
            if (manager == null) throw new ArgumentNullException(nameof(manager));
            if (tiles == null) throw new ArgumentNullException(nameof(tiles));

            // ReSharper disable once PossibleNullReferenceException - ReSharper thinks its always null...
            Interlocked.Exchange(ref _styleManager, manager).Dispose();

            foreach (TileBase tile in tiles)
                tile.Style = manager.GetStyle(tile);
        }

        /// <summary>
        ///     Gets the tiles that fill the bounds given.
        /// </summary>
        /// <param name="bounds">The bounds.</param>
        /// <param name="existingTiles">The existing tiles.</param>
        /// <returns></returns>
        [NotNull]
        public IEnumerable<TileBase> GetTiles(
            Rectangle bounds,
            [NotNull] IEnumerable<TileBase> existingTiles)
        {
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
                // add initial tile to tiles
                tiles.Add(Tiles[0]);

                // add initial tile to end of openTiles
                openTiles.Enqueue(Tiles[0]);
            }

            // while there are tiles with no neighbour
            while (openTiles.Count > 0)
            {
                TileBase tile = openTiles.Dequeue();

                // for each edgePart with no neighbour
                foreach (EdgePart edgePart in tile.GetOpenEdgeParts())
                {
                    TileBase newTile = CreateNewTile(tile, edgePart);

                    tiles.Add(newTile);

                    // if newTile is in bounds
                    if (bounds.IntersectsWith(newTile.GetApproximateBounds()))
                    {
                        // add newTile to end of openTiles
                        openTiles.Enqueue(newTile);
                    }
                }
            }

            return tiles.ToArray();
        }

        /// <summary>
        ///     Creates a new tile that is adjacent to the given part of the given tile.
        /// </summary>
        /// <param name="tile">The tile.</param>
        /// <param name="part">The part.</param>
        /// <returns></returns>
        private TileBase CreateNewTile([NotNull] TileBase tile, [NotNull] EdgePart part)
        {
            Debug.Assert(tile != null, "tile != null");
            Debug.Assert(part != null, "part != null");

            // get adjacent part for the part with the tiles label
            Labeled<EdgePart> adjacent;
            if (!Definition.AdjacentParts.TryGetAdjacent(part.WithLabel(tile.Label), out adjacent))
                throw new InvalidOperationException();
            Debug.Assert(adjacent.Value != null, "adjacent.Value != null");

            Tile adjTile;
            if (!_tileByEdgePart.TryGetValue(adjacent.Value, out adjTile))
                throw new InvalidOperationException();

            // Create transformed tile
            TileInstance newTile = new TileInstance(
                adjTile,
                adjacent.Label,
                adjTile.GetEdgePartPosition(adjacent.Value)
                    .GetTransformTo(tile.GetEdgePartPosition(part)));

            // set cell style from styleManager
            newTile.Style = StyleManager.GetStyle(newTile);

            return newTile;
        }
    }
}