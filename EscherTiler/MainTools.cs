using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using System.Threading.Tasks;
using System.Windows.Forms;
using EscherTiler.Controllers;
using EscherTiler.Graphics;
using EscherTiler.Graphics.GDI;
using JetBrains.Annotations;
using DragAction = EscherTiler.Controllers.DragAction;

namespace EscherTiler
{
    // This part of the class defines the tools specific to the UI.
    public partial class Main
    {
        /// <summary>
        ///     Tool used to pan around the tiling.
        /// </summary>
        /// <seealso cref="EscherTiler.Controllers.Tool" />
        private class PanTool : Tool
        {
            [NotNull]
            private readonly Main _form;

            private Cursor _lastCursor;

            /// <summary>
            ///     Initializes a new instance of the <see cref="PanTool" /> class.
            /// </summary>
            /// <param name="controller">The controller.</param>
            /// <param name="form">The form.</param>
            public PanTool([NotNull] Controller controller, [NotNull] Main form)
                : base(controller)
            {
                _form = form;
            }

            /// <summary>
            ///     Called when this tool is selected as the current tool.
            /// </summary>
            public override void Selected()
            {
                base.Selected();
                _lastCursor = _form._renderControl.Cursor;
                _form._renderControl.Cursor = Cursors.SizeAll;
            }

            /// <summary>
            ///     Called when this tool is deselected as the current tool.
            /// </summary>
            public override void Deselected()
            {
                base.Deselected();
                _form._renderControl.Cursor = _lastCursor;
            }

            /// <summary>
            ///     Starts the action associated with this tool at the location given.
            /// </summary>
            /// <param name="rawLocation">
            ///     The raw location to start the action.
            ///     Should be transformed by the <see cref="IView.InverseViewMatrix" /> for the
            ///     <see cref="Controller">Controllers</see> <see cref="EscherTiler.Controllers.Controller.View" /> to get the
            ///     location in the tiling itself.
            /// </param>
            /// <returns>
            ///     The action that was performed, or null if no action was performed.
            /// </returns>
            public override Action StartAction(Vector2 rawLocation)
            {
                return new PanAction(rawLocation, _form);
            }

            /// <summary>
            ///     Action used for panning.
            /// </summary>
            private class PanAction : DragAction
            {
                private Vector2 _last;

                [NotNull]
                private readonly Main _form;

                private readonly Matrix3x2 _translate;
                private readonly Matrix3x2 _invTranslate;

                /// <summary>
                ///     Initializes a new instance of the <see cref="PanAction" /> class.
                /// </summary>
                /// <param name="start">The start point of the action.</param>
                /// <param name="form">The form.</param>
                public PanAction(Vector2 start, [NotNull] Main form)
                {
                    _last = start;
                    _form = form;

                    _translate = form._translate;
                    _invTranslate = form._invTranslate;
                }

                /// <summary>
                ///     Gets a value indicating whether this action changes any data that would need to be saved.
                /// </summary>
                /// <value>
                ///     <see langword="true" /> if the action changes data; otherwise, <see langword="false" />.
                /// </value>
                public override bool ChangesData => false;

                /// <summary>
                ///     Updates the location of the action.
                /// </summary>
                /// <param name="rawLocation">
                ///     The raw location that the action has been dragged to.
                ///     Should be transformed by the <see cref="P:EscherTiler.IView.InverseViewMatrix" /> for the
                ///     <see cref="P:EscherTiler.Controllers.Controller.View" /> to get the location in 1the tiling itself.
                /// </param>
                public override void Update(Vector2 rawLocation)
                {
                    _form.UpdateTranslation(rawLocation - _last);
                    _last = rawLocation;
                }

                /// <summary>
                ///     Cancels this action.
                /// </summary>
                public override void Cancel()
                {
                    _form._translate = _translate;
                    _form._invTranslate = _invTranslate;
                    _form.UpdateViewBounds();
                }

                /// <summary>
                ///     Applies this action.
                /// </summary>
                public override void Apply() { }
            }
        }

        /// <summary>
        ///     Tool used to select a tile.
        /// </summary>
        /// <seealso cref="EscherTiler.Controllers.Tool" />
        private class SelectTileTool : Tool
        {
            private TileBase _hoverTile;

            /// <summary>
            ///     Initializes a new instance of the <see cref="SelectTileTool" /> class.
            /// </summary>
            /// <param name="controller">The controller.</param>
            public SelectTileTool([NotNull] TilingController controller)
                : base(controller) { }

            /// <summary>
            ///     Gets or sets the last tool.
            /// </summary>
            /// <value>
            ///     The last tool.
            /// </value>
            [CanBeNull]
            public Tool LastTool { get; set; }

            /// <summary>
            ///     Gets the controller the tool belongs to.
            /// </summary>
            /// <value>
            ///     The controller.
            /// </value>
            [NotNull]
            public new TilingController Controller => (TilingController) base.Controller;

            /// <summary>
            ///     Gets or sets the tile selected <see cref="TaskCompletionSource{TResult}" />.
            /// </summary>
            /// <value>
            ///     The tile selected <see cref="TaskCompletionSource{TileBase}" />.
            /// </value>
            [NotNull]
            public TaskCompletionSource<TileBase> TileSelectedTcs { get; set; } = new TaskCompletionSource<TileBase>();

            /// <summary>
            ///     Called when the highlighted location (ie the cursor location) changes.
            /// </summary>
            /// <param name="rawLocation">
            ///     The raw location.
            ///     Should be transformed by the <see cref="IView.InverseViewMatrix" /> for the
            ///     <see cref="Controller">Controllers</see> <see cref="EscherTiler.Controllers.Controller.View" /> to get the
            ///     location in the tiling itself.
            /// </param>
            public override void UpdateLocation(Vector2 rawLocation)
            {
                base.UpdateLocation(rawLocation);

                _hoverTile = GetTileOver(rawLocation);
            }

            /// <summary>
            ///     Starts the action associated with this tool at the location given.
            /// </summary>
            /// <param name="rawLocation">
            ///     The raw location to start the action.
            ///     Should be transformed by the <see cref="IView.InverseViewMatrix" /> for the
            ///     <see cref="Controller">Controllers</see> <see cref="EscherTiler.Controllers.Controller.View" /> to get the
            ///     location in the tiling itself.
            /// </param>
            /// <returns>
            ///     The action that was performed, or null if no action was performed.
            /// </returns>
            public override Action StartAction(Vector2 rawLocation)
            {
                _hoverTile = GetTileOver(rawLocation);
                TileSelectedTcs.TrySetResult(_hoverTile);
                Controller.CurrentTool = LastTool;
                return InstantAction.PureInstance;
            }

            /// <summary>
            ///     Gets the tile at the location given.
            /// </summary>
            /// <param name="location">The location.</param>
            /// <returns></returns>
            private TileBase GetTileOver(Vector2 location)
            {
                Dictionary<Tile, IGraphicsPath> tilePaths = new Dictionary<Tile, IGraphicsPath>();
                try
                {
                    Matrix3x2 inverseViewMatrix = Controller.View.InverseViewMatrix;

                    IEnumerable<TileBase> tiles = Controller.Tiles;
                    foreach (TileBase tile in tiles)
                    {
                        IGraphicsPath path;
                        bool disposePath = false;

                        TileInstance tileInstance;
                        Tile rawTile = tile as Tile;
                        if (rawTile != null)
                        {
                            path = new GDIGraphics.GraphicsPath();
                            rawTile.PopulateGraphicsPath(path);
                            tilePaths.Add(rawTile, path);
                        }
                        else if ((tileInstance = tile as TileInstance) != null)
                        {
                            if (!tilePaths.TryGetValue(tileInstance.Tile, out path))
                            {
                                path = new GDIGraphics.GraphicsPath();
                                tileInstance.Tile.PopulateGraphicsPath(path);
                                tilePaths.Add(tileInstance.Tile, path);
                            }
                            Debug.Assert(path != null, "path != null");
                        }
                        else
                        {
                            disposePath = true;
                            path = new GDIGraphics.GraphicsPath();
                            tile.PopulateGraphicsPath(path);
                        }

                        using (disposePath ? path : null)
                        {
                            Matrix3x2 tranform;
                            if (Matrix3x2.Invert(tile.Transform, out tranform) &&
                                path.ContainsPoint(location, inverseViewMatrix * tranform))
                                return tile;
                        }
                    }
                }
                finally
                {
                    foreach (IGraphicsPath path in tilePaths.Values)
                    {
                        Debug.Assert(path != null, "path != null");
                        path.Dispose();
                    }
                }

                Debug.Fail("There should be a tile");
                return null;
            }

            /// <summary>
            ///     Draws this object to the <see cref="IGraphics" /> provided.
            /// </summary>
            /// <param name="graphics">The graphics object to use to draw this object.</param>
            public override void Draw(IGraphics graphics)
            {
                base.Draw(graphics);

                if (_hoverTile != null)
                {
                    graphics.FillStyle = TilingController.TransparentBlue;

                    using (IGraphicsPath path = graphics.CreatePath())
                    {
                        _hoverTile.PopulateGraphicsPath(path);
                        graphics.FillPath(path);
                    }
                }
            }
        }
    }
}