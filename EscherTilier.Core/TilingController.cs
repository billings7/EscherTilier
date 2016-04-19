using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using EscherTilier.Dependencies;
using EscherTilier.Graphics;
using EscherTilier.Graphics.Resources;
using EscherTilier.Numerics;
using EscherTilier.Styles;
using JetBrains.Annotations;

namespace EscherTilier
{
    public class TilingController : Controller
    {
        private readonly bool _tilingSet;

        [NotNull]
        private readonly Tiling _tiling;

        [NotNull]
        private IEnumerable<TileBase> _tiles;

        [NotNull]
        private StyleManager _styleManager;

        [CanBeNull]
        private IResourceManager _resourceManager;

        public TilingController([NotNull] Tiling tiling, [NotNull] StyleManager styleManager, Rectangle screenBounds)
            : base(screenBounds)
        {
            if (tiling == null) throw new ArgumentNullException(nameof(tiling));
            if (styleManager == null) throw new ArgumentNullException(nameof(styleManager));

            _tiling = tiling;
            _styleManager = styleManager;

            IResourceManager resourceManager = DependencyManger.GetResourceManager(_styleManager);
            if (resourceManager == null) throw new InvalidOperationException();
            _resourceManager = resourceManager;

            _tiles = _tiling.GetTiles(ScreenBounds, _styleManager, Enumerable.Empty<TileBase>());
            _tilingSet = true;
        }

        /// <summary>
        ///     Raises the <see cref="E:ScreenBoundsChanged" /> event.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected override void OnScreenBoundsChanged(EventArgs e)
        {
            base.OnScreenBoundsChanged(e);

            if (_tilingSet)
                _tiles = _tiling.GetTiles(ScreenBounds, _styleManager, _tiles);
        }

        /// <summary>
        ///     Draws this object to the <see cref="IGraphics" /> provided.
        /// </summary>
        /// <param name="graphics">The graphics object to use to draw this object.</param>
        public override void Draw(IGraphics graphics)
        {
            if (_resourceManager == null) throw new ObjectDisposedException(nameof(ShapeController));

            graphics.ResourceManager = _resourceManager;

            Matrix3x2 initialTransform = graphics.Transform;

            graphics.SetLineStyle(_styleManager.LineStyle);

            Dictionary<Tile, IGraphicsPath> tilePaths = new Dictionary<Tile, IGraphicsPath>();

            try
            {
                IEnumerable<TileBase> tiles = _tiles;
                foreach (TileBase tile in tiles)
                {
                    graphics.FillStyle = tile.Style;

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
                foreach (IGraphicsPath path in tilePaths.Values)
                    path.Dispose();
            }
        }

        /// <summary>
        ///     Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing">
        ///     <see langword="true" /> to release both managed and unmanaged resources;
        ///     <see langword="false" /> to release only unmanaged resources.
        /// </param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                DependencyManger.ReleaseResourceManager(ref _resourceManager, _styleManager);
                _styleManager.Dispose();
            }
        }
    }
}