using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Threading;
using EscherTiler.Graphics;
using EscherTiler.Numerics;
using EscherTiler.Styles;
using EscherTiler.Utilities;
using JetBrains.Annotations;

namespace EscherTiler
{
    /// <summary>
    ///     Class that manages a tiling of a number of tiles.
    /// </summary>
    public class Tiling
    {
        [NotNull]
        private readonly Dictionary<EdgePart, Tile> _tileByEdgePart = new Dictionary<EdgePart, Tile>();

        [NotNull]
        private StyleManager _styleManager;

        /// <summary>
        ///     Initializes a new instance of the <see cref="Tiling" /> class.
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
            {
                foreach (EdgePartShape part in tile.PartShapes)
                    _tileByEdgePart.Add(part.Part, tile);
            }
        }

        /// <summary>
        ///     Gets the template this tiling is created from.
        /// </summary>
        /// <value>
        ///     The template.
        /// </value>
        [NotNull]
        public Template Template { get; }

        /// <summary>
        ///     Gets the tiling definition this tiling is created from.
        /// </summary>
        /// <value>
        ///     The definition.
        /// </value>
        [NotNull]
        public TilingDefinition Definition { get; }

        /// <summary>
        ///     Gets the root tiles that all other tiles are based on.
        /// </summary>
        /// <value>
        ///     The tiles.
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
        ///     Sets the style manager for the tiling and updates the styles for the given tiles.
        /// </summary>
        /// <param name="manager">The new style manager manager.</param>
        /// <param name="tiles">The tiles.</param>
        public void SetStyleManager([NotNull] StyleManager manager, [NotNull] [ItemNotNull] IEnumerable<TileBase> tiles)
        {
            if (manager == null) throw new ArgumentNullException(nameof(manager));
            if (tiles == null) throw new ArgumentNullException(nameof(tiles));

            Interlocked.Exchange(ref _styleManager, manager);

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
                TileBase tile = Tiles[0];
                Debug.Assert(tile != null, "tile != null");

                Rectangle approximateBounds = tile.GetApproximateBounds();
                if (!bounds.IntersectsWith(approximateBounds))
                {
                    tile = new TileInstance(
                        (Tile) tile,
                        tile.Label,
                        tile.Transform * Matrix3x2.CreateTranslation(bounds.Center - approximateBounds.Center));
                }

                // add initial tile to tiles
                tiles.Add(tile);

                // add initial tile to end of openTiles
                openTiles.Enqueue(tile);
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

            // Set the style for the new cells
            foreach (TileBase tile in tiles.Where(t => t.Style == null))
            {
                // set cell style from styleManager
                tile.Style = StyleManager.GetStyle(tile);
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

            return newTile;
        }

        /// <summary>
        ///     Draws a tiling.
        /// </summary>
        /// <param name="tiles">The tiles to draw.</param>
        /// <param name="graphics">The graphics to draw to.</param>
        /// <param name="lineStyle">The line style.</param>
        /// <param name="fillStyle">If not null, overwrites the <see cref="TileBase.Style" /> of the tiles.</param>
        /// <exception cref="ArgumentNullException">
        /// </exception>
        public static void DrawTiling(
            [NotNull] IEnumerable<TileBase> tiles,
            [NotNull] IGraphics graphics,
            [NotNull] LineStyle lineStyle,
            [CanBeNull] IStyle fillStyle = null)
        {
            if (tiles == null) throw new ArgumentNullException(nameof(tiles));
            if (graphics == null) throw new ArgumentNullException(nameof(graphics));
            if (lineStyle == null) throw new ArgumentNullException(nameof(lineStyle));

            Matrix3x2 initialTransform = graphics.Transform;

            graphics.SetLineStyle(lineStyle);
            if (fillStyle != null)
                graphics.FillStyle = fillStyle;

            Dictionary<Tile, IGraphicsPath> tilePaths = new Dictionary<Tile, IGraphicsPath>();
            try
            {
                foreach (TileBase tile in tiles)
                {
                    if (tile == null) throw new ArgumentNullException();

                    if (fillStyle == null)
                        graphics.FillStyle = tile.Style ?? SolidColourStyle.Transparent;

                    IGraphicsPath path;
                    bool disposePath = false;

                    TileInstance tileInstance;
                    Tile rawTile = tile as Tile;
                    if (rawTile != null)
                    {
                        path = graphics.CreatePath();
                        rawTile.PopulateGraphicsPath(path);
                        tilePaths.Add(rawTile, path);

                        graphics.Transform = initialTransform;
                    }
                    else if ((tileInstance = tile as TileInstance) != null)
                    {
                        if (!tilePaths.TryGetValue(tileInstance.Tile, out path))
                        {
                            path = graphics.CreatePath();
                            tileInstance.Tile.PopulateGraphicsPath(path);
                            tilePaths.Add(tileInstance.Tile, path);
                        }
                        Debug.Assert(path != null, "path != null");

                        graphics.Transform = tile.Transform * initialTransform;
                    }
                    else
                    {
                        disposePath = true;
                        path = graphics.CreatePath();
                        tile.PopulateGraphicsPath(path);

                        graphics.Transform = initialTransform;
                    }

                    using (disposePath ? path : null)
                    {
                        graphics.FillPath(path);
                        graphics.DrawPath(path);
                    }
                }
            }
            finally
            {
                graphics.Transform = initialTransform;
                foreach (IGraphicsPath path in tilePaths.Values)
                {
                    Debug.Assert(path != null, "path != null");
                    path.Dispose();
                }
            }
        }
    }
}