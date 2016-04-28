using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Printing;
using System.Linq;
using System.Numerics;
using System.Threading;
using EscherTiler.Numerics;
using EscherTiler.Styles;
using JetBrains.Annotations;

namespace EscherTiler.Graphics.GDI
{
    /// <summary>
    ///     Class for printing a tiling.
    /// </summary>
    /// <seealso cref="System.Drawing.Printing.PrintDocument" />
    public class TilerPrintDocument : PrintDocument
    {
        /// <summary>
        /// Delegate for getting the transform matricies for a page with the given bounds.
        /// </summary>
        /// <param name="bounds">The bounds.</param>
        /// <param name="transform">The transform.</param>
        /// <param name="inverseTransform">The inverse transform.</param>
        public delegate void GetTranformDelegate(
            Rectangle bounds,
            out Matrix3x2 transform,
            out Matrix3x2 inverseTransform);

        [NotNull]
        private static readonly LineStyle _lineStyle = new LineStyle(1, SolidColourStyle.Black);

        [CanBeNull]
        private GDIResourceManager _resourceManager = new GDIResourceManager();

        [NotNull]
        private GetTranformDelegate _getTranform = DefaultGetTranform;

        /// <summary>
        /// Initializes a new instance of the <see cref="TilerPrintDocument"/> class.
        /// </summary>
        public TilerPrintDocument()
        {
            Debug.Assert(_resourceManager != null, "_resourceManager != null");

            _resourceManager.Add(SolidColourStyle.White);
            _resourceManager.Add(_lineStyle);
        }

        /// <summary>
        ///     Gets or sets the print mode.
        /// </summary>
        /// <value>
        ///     The print mode.
        /// </value>
        [Description("The print mode for the tiling.")]
        public TilingPrintMode PrintMode { get; set; }

        /// <summary>
        ///     Gets or sets the tiling to print.
        /// </summary>
        /// <value>
        ///     The tiling.
        /// </value>
        [Browsable(false)]
        [CanBeNull]
        public Tiling Tiling { get; set; }

        /// <summary>
        ///     Gets or sets the tile to print.
        /// </summary>
        /// <value>
        ///     The tile.
        /// </value>
        [Browsable(false)]
        [CanBeNull]
        public TileBase Tile { get; set; }

        /// <summary>
        /// Gets or sets the get tranform fucntion.
        /// </summary>
        /// <value>
        /// The get tranform function.
        /// </value>
        [NotNull]
        public GetTranformDelegate GetTranform
        {
            get { return _getTranform; }
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));
                _getTranform = value;
            }
        }

        /// <summary>
        ///     The default function for the <see cref="GetTranform"/> property.
        /// </summary>
        /// <param name="bounds">The bounds.</param>
        /// <param name="transform">The transform.</param>
        /// <param name="inverseTransform">The inverse transform.</param>
        private static void DefaultGetTranform(
            Rectangle bounds,
            out Matrix3x2 transform,
            out Matrix3x2 inverseTransform)
        {
            transform = inverseTransform = Matrix3x2.Identity;
        }

        /// <summary>
        ///     Raises the <see cref="E:System.Drawing.Printing.PrintDocument.PrintPage" /> event. It is called before a page
        ///     prints.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Drawing.Printing.PrintPageEventArgs" /> that contains the event data. </param>
        protected override void OnPrintPage(PrintPageEventArgs e)
        {
            base.OnPrintPage(e);

            if (_resourceManager == null) return;

            e.Graphics.SetClip(e.MarginBounds);

            using (GDIGraphics graphics = new GDIGraphics(
                e.Graphics,
                _resourceManager,
                SolidColourStyle.White,
                _lineStyle))
            {
                Matrix3x2 transform, inverseTransform;
                _getTranform(
                    e.MarginBounds.ToRectangle(),
                    out transform,
                    out inverseTransform);

                graphics.Transform = transform;

                Vector2 tl = new Vector2(e.MarginBounds.Left, e.MarginBounds.Top);
                Vector2 br = new Vector2(e.MarginBounds.Right, e.MarginBounds.Bottom);

                tl = Vector2.Transform(tl, inverseTransform);
                br = Vector2.Transform(br, inverseTransform);

                Rectangle bounds = Rectangle.ContainingPoints(tl, br);

                switch (PrintMode)
                {
                    case TilingPrintMode.TilingFull:
                        if (Tiling == null) throw new InvalidOperationException("The Tiling is not set");

                        Tiling.DrawTiling(GetTiles(bounds), graphics, Tiling.StyleManager.LineStyle);
                        break;
                    case TilingPrintMode.TilingLines:
                        if (Tiling == null) throw new InvalidOperationException("The Tiling is not set");

                        Tiling.DrawTiling(GetTiles(bounds), graphics, _lineStyle, SolidColourStyle.White);
                        break;
                    case TilingPrintMode.SingleTileFull:
                        if (Tile == null) throw new InvalidOperationException("The Tile is not set");

                        DrawTile(graphics, Tiling?.StyleManager.LineStyle ?? _lineStyle);
                        break;
                    case TilingPrintMode.SingleTileLines:
                        if (Tile == null) throw new InvalidOperationException("The Tile is not set");

                        DrawTile(graphics, _lineStyle, SolidColourStyle.White);
                        break;
                    default:
                        Debug.Fail($"Unexpected print mode {PrintMode}");
                        throw new NotSupportedException();
                }
            }
        }

        /// <summary>
        /// Gets the tiles from the <see cref="Tiling"/>.
        /// </summary>
        /// <returns></returns>
        [NotNull]
        private IEnumerable<TileBase> GetTiles(Rectangle bounds)
        {
            Debug.Assert(Tiling != null, "Tiling != null");

            Tile origTile = Tiling.Tiles[0];
            Debug.Assert(origTile != null, "origTile != null");

            TileInstance tile = new TileInstance(origTile, origTile.Label, origTile.Transform)
            {
                Style = origTile.Style
            };

            return Tiling.GetTiles(bounds, new TileBase[] { tile });
        }

        /// <summary>
        ///     Draws the <see cref="Tile" />.
        /// </summary>
        /// <param name="graphics">The graphics.</param>
        /// <param name="lineStyle">The line style.</param>
        /// <param name="fillStyle">The fill style.</param>
        private void DrawTile([NotNull] IGraphics graphics, [NotNull] LineStyle lineStyle, IStyle fillStyle = null)
        {
            Debug.Assert(graphics != null, "graphics != null");
            Debug.Assert(lineStyle != null, "lineStyle != null");
            Debug.Assert(Tile != null, "Tile != null");

            graphics.SetLineStyle(lineStyle);
            graphics.FillStyle = fillStyle ?? Tile.Style ?? SolidColourStyle.Transparent;

            using (IGraphicsPath path = graphics.CreatePath())
            {
                Tile.PopulateGraphicsPath(path);
                graphics.FillPath(path);
                graphics.DrawPath(path);
            }
        }

        /// <summary>
        ///     Releases the unmanaged resources used by the <see cref="T:System.ComponentModel.Component" /> and optionally
        ///     releases the managed resources.
        /// </summary>
        /// <param name="disposing">
        ///     true to release both managed and unmanaged resources; false to release only unmanaged
        ///     resources.
        /// </param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
                Interlocked.Exchange(ref _resourceManager, null)?.Dispose();
        }
    }
}