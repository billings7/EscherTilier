using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Threading;
using EscherTiler.Dependencies;
using EscherTiler.Graphics;
using EscherTiler.Graphics.Resources;
using EscherTiler.Styles;
using JetBrains.Annotations;

namespace EscherTiler.Controllers
{
    /// <summary>
    ///     Controls editing of a <see cref="Tiling" />.
    /// </summary>
    public class TilingController : Controller
    {
        // TODO settings?
        private static readonly float _tolerance = 10;

        [NotNull]
        public static readonly SolidColourStyle TransparentBlue =
            new SolidColourStyle(Colour.CornflowerBlue, 0.5f);

        [NotNull]
        private Tiling _tiling;

        [NotNull]
        private IEnumerable<TileBase> _tiles;

        [CanBeNull]
        private IResourceManager _resourceManager;

        [NotNull]
        private readonly EventHandler _styleManagerChangedHandler;

        /// <summary>
        ///     Occurs when the style manager changes.
        /// </summary>
        public event EventHandler StyleManagerChanged;

        /// <summary>
        ///     Initializes a new instance of the <see cref="TilingController" /> class.
        /// </summary>
        /// <param name="tiling">The tiling.</param>
        /// <param name="view">The view.</param>
        /// <exception cref="System.ArgumentNullException">
        /// </exception>
        /// <exception cref="System.InvalidOperationException"></exception>
        public TilingController([NotNull] Tiling tiling, [NotNull] IView view)
            : base(view)
        {
            if (tiling == null) throw new ArgumentNullException(nameof(tiling));

            _tiling = tiling;

            IResourceManager resourceManager = DependencyManger.GetResourceManager(StyleManager);
            _resourceManager = resourceManager;

            resourceManager.Add(SolidColourStyle.Transparent);
            resourceManager.Add(SolidColourStyle.White);
            resourceManager.Add(SolidColourStyle.Black);
            resourceManager.Add(SolidColourStyle.Gray);
            resourceManager.Add(SolidColourStyle.CornflowerBlue);
            resourceManager.Add(TransparentBlue);

            _tiles = _tiling.GetTiles(view.ViewBounds, Enumerable.Empty<TileBase>());

            _styleManagerChangedHandler = _styleManager_Changed;
            StyleManager.StylesChanged += _styleManagerChangedHandler;

            Tools = new Tool[]
            {
                EditLine = new EditLineTool(this, _tolerance),
                SplitLine = new SplitLineTool(this, _tolerance)
            };

            view.ViewBoundsChanged += View_ViewBoundsChanged;
        }

        /// <summary>
        ///     Handles the Changed event of the _styleManager objects.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="args">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void _styleManager_Changed(object sender, EventArgs args)
        {
            _tiling.UpdateStyles(_tiles);
            StyleManagerChanged?.Invoke(sender, args);
        }

        /// <summary>
        ///     Gets the edit line tool.
        /// </summary>
        /// <value>
        ///     The edit line tool.
        /// </value>
        [NotNull]
        public EditLineTool EditLine { get; }

        /// <summary>
        ///     Gets the split line tool.
        /// </summary>
        /// <value>
        ///     The split line tool.
        /// </value>
        [NotNull]
        public SplitLineTool SplitLine { get; }

        /// <summary>
        ///     Gets the tiles.
        /// </summary>
        /// <value>
        ///     The tiles.
        /// </value>
        [NotNull]
        [ItemNotNull]
        public IEnumerable<TileBase> Tiles => _tiles;

        /// <summary>
        ///     Gets the tiling.
        /// </summary>
        /// <value>
        ///     The tiling.
        /// </value>
        [NotNull]
        public Tiling Tiling => _tiling;

        /// <summary>
        ///     Gets the style manager.
        /// </summary>
        /// <value>
        ///     The style manager.
        /// </value>
        [NotNull]
        public StyleManager StyleManager => _tiling.StyleManager;

        /// <summary>
        ///     Sets the tiling.
        /// </summary>
        /// <param name="tiling">The tiling.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="System.InvalidOperationException"></exception>
        public void SetTiling([NotNull] Tiling tiling)
        {
            if (tiling == null) throw new ArgumentNullException(nameof(tiling));

            if (tiling == _tiling) return;

            StyleManager oldStyleManager = StyleManager;
            Debug.Assert(oldStyleManager != null, "oldStyleManager != null");
            oldStyleManager.StylesChanged -= _styleManagerChangedHandler;

            _tiling = tiling;

            StyleManager.StylesChanged += _styleManagerChangedHandler;

            IResourceManager resourceManager = DependencyManger.GetResourceManager(tiling.StyleManager);
            IResourceManager oldResourceManager = Interlocked.Exchange(ref _resourceManager, resourceManager);

            DependencyManger.ReleaseResourceManager(ref oldResourceManager, oldStyleManager);

            resourceManager.Add(SolidColourStyle.Transparent);
            resourceManager.Add(SolidColourStyle.White);
            resourceManager.Add(SolidColourStyle.Black);
            resourceManager.Add(SolidColourStyle.Gray);
            resourceManager.Add(SolidColourStyle.CornflowerBlue);
            resourceManager.Add(TransparentBlue);

            _tiles = _tiling.GetTiles(View.ViewBounds, Enumerable.Empty<TileBase>());
        }

        /// <summary>
        ///     Raises the <see cref="E:ScreenBoundsChanged" /> event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void View_ViewBoundsChanged(object sender, EventArgs e)
        {
            _tiles = _tiling.GetTiles(View.ViewBounds, _tiles);
        }

        /// <summary>
        ///     Draws this object to the <see cref="IGraphics" /> provided.
        /// </summary>
        /// <param name="graphics">The graphics object to use to draw this object.</param>
        public override void Draw(IGraphics graphics)
        {
            if (_resourceManager == null) throw new ObjectDisposedException(nameof(ShapeController));

            graphics.ResourceManager = _resourceManager;

            Tiling.DrawTiling(_tiles, graphics, StyleManager.LineStyle);

            CurrentTool?.Draw(graphics);
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
                StyleManager.StylesChanged -= _styleManagerChangedHandler;
                DependencyManger.ReleaseResourceManager(ref _resourceManager, StyleManager);
            }
        }

        /// <summary>
        ///     Represents a line that has been selected, along with the edge part and tile it belonged to.
        /// </summary>
        private class SelectedLine
        {
            /// <summary>
            ///     The tile the line is on.
            /// </summary>
            [NotNull]
            public readonly TileBase Tile;

            /// <summary>
            ///     The edge part the line is on.
            /// </summary>
            [NotNull]
            public readonly EdgePartShape EdgePart;

            /// <summary>
            ///     The line
            /// </summary>
            [NotNull]
            public readonly ILine Line;

            /// <summary>
            ///     The transform applied to the line to display it in the view.
            /// </summary>
            public readonly Matrix3x2 LineTransform;

            /// <summary>
            ///     Initializes a new instance of the <see cref="SelectedLine" /> class.
            /// </summary>
            /// <param name="tile">The tile.</param>
            /// <param name="edgePart">The edge part.</param>
            /// <param name="line">The line.</param>
            /// <param name="lineTransform">The line transform.</param>
            public SelectedLine(TileBase tile, EdgePartShape edgePart, ILine line, Matrix3x2 lineTransform)
            {
                Debug.Assert(tile != null, "tile != null");
                Debug.Assert(edgePart != null, "edgePart != null");
                Debug.Assert(line != null, "line != null");
                Tile = tile;
                EdgePart = edgePart;
                Line = line;
                LineTransform = lineTransform;
            }
        }

        /// <summary>
        ///     Gets the line selected at the locaiton given.
        /// </summary>
        private SelectedLine GetSelected(Vector2 rawLocation, float tolerance)
        {
            LinePoint loc;
            return GetSelected(rawLocation, tolerance, out loc);
        }

        /// <summary>
        ///     Gets the line selected at the locaiton given.
        /// </summary>
        private SelectedLine GetSelected(Vector2 rawLocation, float tolerance, out LinePoint loc)
        {
            Matrix3x2 viewMatrix = View.ViewMatrix;

            bool checkNext = false;
            SelectedLine last = null;
            LinePoint lastHit = null;

            // Check each line for each edge part for each tile
            foreach (TileBase tile in Tiles)
            {
                foreach (EdgePartShape partShape in tile.PartShapes)
                {
                    Matrix3x2 transform =
                        partShape.GetLineTransform()
                        * tile.Transform
                        * viewMatrix;

                    foreach (ILine line in partShape.Lines)
                    {
                        LinePoint hit = line.HitTest(rawLocation, tolerance, transform);
                        if (hit != null)
                        {
                            if (checkNext)
                            {
                                if (hit.Distance > 0)
                                {
                                    loc = hit;
                                    return new SelectedLine(tile, partShape, line, transform);
                                }

                                loc = lastHit;
                                return last;
                            }

                            // If the hit was just at/past the end of the line, check to see if it was in the next line
                            if (hit.Distance >= 1)
                            {
                                lastHit = hit;
                                last = new SelectedLine(tile, partShape, line, transform);
                                checkNext = true;
                                continue;
                            }

                            loc = hit;
                            return new SelectedLine(tile, partShape, line, transform);
                        }

                        if (checkNext)
                        {
                            loc = lastHit;
                            return last;
                        }
                    }
                }
            }

            if (checkNext)
            {
                loc = lastHit;
                return last;
            }

            loc = null;
            return null;
        }

        /// <summary>
        ///     Tool used for editing the lines of a tile
        /// </summary>
        /// <seealso cref="Tool" />
        public class EditLineTool : Tool
        {
            /// <summary>
            ///     The tool name.
            /// </summary>
            public const string ToolName = "EditLineTool";

            /// <summary>
            ///     The change line type option name.
            /// </summary>
            public const string ChangeLineTypeName = "ChangeLineType";

            [CanBeNull]
            private SelectedLine _selectedLine;

            [CanBeNull]
            private LineVector _selectedVector;

            private float _tolerance;

            [CanBeNull]
            private SelectedLine _hoverLine;

            [NotNull]
            public readonly Option ChangeLineOption;

            /// <summary>
            ///     Gets or sets the tolerance for how close the point has to be to the line to select it.
            /// </summary>
            /// <value>
            ///     The tolerance.
            /// </value>
            /// <exception cref="ArgumentOutOfRangeException"></exception>
            public float Tolerance
            {
                get { return _tolerance; }
                set
                {
                    if (value <= 0) throw new ArgumentOutOfRangeException(nameof(value));
                    _tolerance = value;
                }
            }

            /// <summary>
            ///     Initializes a new instance of the <see cref="EditLineTool" /> class.
            /// </summary>
            /// <param name="controller">The controller.</param>
            /// <param name="tolerance">The tolerance. Must be greater than 0.</param>
            public EditLineTool([NotNull] TilingController controller, float tolerance)
                : base(controller, ToolName)
            {
                if (tolerance <= 0) throw new ArgumentOutOfRangeException(nameof(tolerance));
                _tolerance = tolerance;
                ChangeLineOption = new Option(ChangeLineTypeName, typeof(Line));
                ChangeLineOption.ValueChanged += ChangeLineType;
            }

            /// <summary>
            ///     Gets the controller the tool belongs to.
            /// </summary>
            /// <value>
            ///     The controller.
            /// </value>
            [NotNull]
            public new TilingController Controller => (TilingController) base.Controller;

            /// <summary>
            ///     Called when this tool is selected as the current tool.
            /// </summary>
            public override void Selected()
            {
                base.Selected();

                if (_selectedLine != null)
                {
                    if (!_selectedLine.EdgePart.Lines.Contains(_selectedLine.Line))
                        _selectedLine = null;
                }
            }

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

                _hoverLine = Controller.GetSelected(rawLocation, _tolerance);
            }

            /// <summary>
            ///     Changes the type of the selected line.
            /// </summary>
            /// <param name="value">The value.</param>
            private void ChangeLineType(object value)
            {
                Type type = value as Type;
                if (type == null) throw new ArgumentException();

                SelectedLine selectedLine = _selectedLine;
                if (selectedLine == null) return;

                ILine currLine = selectedLine.Line;
                Type currLineType = currLine.GetType();
                if (type == currLineType) return;

                ILine newLine;

                if (type == typeof(Line))
                    newLine = new Line(currLine.Start, currLine.End);
                else if (type == typeof(QuadraticBezierCurve))
                {
                    if (currLineType == typeof(Line))
                    {
                        newLine = new QuadraticBezierCurve(
                            currLine.Start,
                            new LineVector((currLine.Start.Vector + currLine.End.Vector) / 2),
                            currLine.End);
                    }
                    else if (currLineType == typeof(CubicBezierCurve))
                    {
                        CubicBezierCurve cubic = (CubicBezierCurve) currLine;

                        Vector2 vec = currLine.Start.Vector + (currLine.End.Vector - currLine.Start.Vector) / 2;
                        newLine = new QuadraticBezierCurve(
                            cubic.Start,
                            new LineVector(vec),
                            cubic.End);
                    }
                    else
                    {
                        throw new NotSupportedException(
                            $"Cannot change a line of type '{currLineType.FullName}' to a line of type '{type.FullName}'.");
                    }
                }
                else if (type == typeof(CubicBezierCurve))
                {
                    if (currLineType == typeof(Line))
                    {
                        Vector2 vec = (currLine.End.Vector - currLine.Start.Vector) / 3;
                        newLine = new CubicBezierCurve(
                            currLine.Start,
                            new LineVector(currLine.Start.Vector + vec),
                            new LineVector(currLine.Start.Vector + vec + vec),
                            currLine.End);
                    }
                    else if (currLineType == typeof(QuadraticBezierCurve))
                    {
                        QuadraticBezierCurve quad = (QuadraticBezierCurve) currLine;

                        Vector2 start = quad.Start.Vector;
                        Vector2 end = quad.End.Vector;
                        Vector2 control = quad.ControlPoint.Vector;

                        newLine = new CubicBezierCurve(
                            quad.Start,
                            new LineVector(start + (2f / 3f * (control - start))),
                            new LineVector(end + (2f / 3f * (control - end))),
                            quad.End);
                    }
                    else
                    {
                        throw new NotSupportedException(
                            $"Cannot change a line of type '{currLineType.FullName}' to a line of type '{type.FullName}'.");
                    }
                }
                else
                {
                    throw new NotSupportedException(
                        $"Cannot change a line of type '{currLineType.FullName}' to a line of type '{type.FullName}'.");
                }

                Debug.Assert(newLine.GetType() == type, "newLine.GetType() == type");

                selectedLine.EdgePart.Lines.Replace(currLine, newLine);

                _selectedLine = new SelectedLine(
                    selectedLine.Tile,
                    selectedLine.EdgePart,
                    newLine,
                    selectedLine.LineTransform);
            }

            /// <summary>
            ///     Gets the distance squared from the <paramref name="vector" /> to the <paramref name="loc" />.
            ///     Only used by <see cref="StartAction" />
            /// </summary>
            /// <param name="vector">The vector.</param>
            /// <param name="transform">The transform.</param>
            /// <param name="loc">The loc.</param>
            /// <returns></returns>
            [NotNull]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private static Tuple<LineVector, float> GetDist(LineVector vector, Matrix3x2 transform, Vector2 loc)
                => Tuple.Create(vector, Vector2.DistanceSquared(Vector2.Transform(vector, transform), loc));

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
                Matrix3x2 viewMatrix = Controller.View.ViewMatrix;

                // If there is a line selected, see if one of the control points has been selected
                if (_selectedLine != null)
                {
                    Matrix3x2 transform =
                        _selectedLine.EdgePart.GetLineTransform()
                        * _selectedLine.Tile.Transform
                        * viewMatrix;

                    // Gets the closest point to the location
                    Tuple<LineVector, float> closest =
                        _selectedLine.EdgePart.Lines.SelectMany(l => l.Points)
                            .Where(p => !p.IsFixed)
                            .Select(p => GetDist(p, transform, rawLocation))
                            .OrderBy(t => t.Item2)
                            .FirstOrDefault();

                    // If the closest point is within the toelrance, select it
                    if (closest != null && closest.Item2 < (_tolerance * _tolerance))
                    {
                        Debug.Assert(closest.Item1 != null, "closest.Item1 != null");

                        // Need the inverse transform for updating the line from a raw location
                        Matrix3x2 inverseTransform;
                        if (!Matrix3x2.Invert(_selectedLine.Tile.Transform, out inverseTransform))
                            throw new InvalidOperationException();

                        inverseTransform =
                            Controller.View.InverseViewMatrix
                            * inverseTransform
                            * _selectedLine.EdgePart.GetLineTransform(true);

                        _selectedVector = closest.Item1;
                        return new EditPointAction(closest.Item1, inverseTransform, this);
                    }
                }
                _selectedVector = null;

                _selectedLine = Controller.GetSelected(rawLocation, _tolerance);
                if (_selectedLine == null)
                {
                    RemoveOption(ChangeLineOption);
                    return null;
                }

                ChangeLineOption.Value = _selectedLine.Line.GetType();
                AddOption(ChangeLineOption);
                return InstantAction.PureInstance;
            }

            /// <summary>
            ///     Draws this object to the <see cref="IGraphics" /> provided.
            /// </summary>
            /// <param name="graphics">The graphics object to use to draw this object.</param>
            public override void Draw(IGraphics graphics)
            {
                base.Draw(graphics);

                graphics.LineWidth = Controller.StyleManager.LineStyle.Width;

                SelectedLine selectedLine = _selectedLine;
                SelectedLine hoverLine = _hoverLine;
                if (hoverLine != null)
                {
                    graphics.LineStyle = SolidColourStyle.CornflowerBlue;
                    hoverLine.Line.Draw(
                        graphics,
                        hoverLine.EdgePart.GetLineTransform() * hoverLine.Tile.Transform);
                }

                if (selectedLine != null)
                {
                    float radius = Controller.StyleManager.LineStyle.Width * 2;

                    Matrix3x2 transform = selectedLine.EdgePart.GetLineTransform() * selectedLine.Tile.Transform;

                    graphics.LineStyle = SolidColourStyle.CornflowerBlue;
                    selectedLine.Line.Draw(graphics, transform);

                    graphics.LineStyle = SolidColourStyle.Black;

                    foreach (ILine line in selectedLine.EdgePart.Lines)
                    {
                        QuadraticBezierCurve quadCurve;
                        CubicBezierCurve cubicCurve;
                        if ((quadCurve = line as QuadraticBezierCurve) != null)
                        {
                            graphics.LineStyle = TransparentBlue;
                            graphics.DrawLines(
                                Vector2.Transform(quadCurve.Start, transform),
                                Vector2.Transform(quadCurve.ControlPoint, transform),
                                Vector2.Transform(quadCurve.End, transform));
                            graphics.LineStyle = SolidColourStyle.Black;
                        }
                        else if ((cubicCurve = line as CubicBezierCurve) != null)
                        {
                            graphics.LineStyle = TransparentBlue;
                            graphics.DrawLines(
                                Vector2.Transform(cubicCurve.Start, transform),
                                Vector2.Transform(cubicCurve.ControlPointA, transform),
                                Vector2.Transform(cubicCurve.ControlPointB, transform),
                                Vector2.Transform(cubicCurve.End, transform));
                            graphics.LineStyle = SolidColourStyle.Black;
                        }

                        foreach (LineVector point in line.Points)
                        {
                            if (point.IsFixed) graphics.FillStyle = SolidColourStyle.Gray;
                            else if (point == _selectedVector) graphics.FillStyle = SolidColourStyle.CornflowerBlue;
                            else graphics.FillStyle = SolidColourStyle.White;

                            DrawControl(graphics, Vector2.Transform(point, transform), radius);
                        }
                    }
                }
            }

            /// <summary>
            ///     Draws a control point at the location given.
            /// </summary>
            /// <param name="graphics">The graphics.</param>
            /// <param name="location">The location.</param>
            /// <param name="radius">The radius.</param>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private void DrawControl([NotNull] IGraphics graphics, Vector2 location, float radius)
            {
                graphics.FillCircle(location, radius);
                graphics.DrawCircle(location, radius);
            }

            /// <summary>
            ///     Action for editing a point of a line.
            /// </summary>
            private class EditPointAction : DragAction
            {
                private readonly Vector2 _initial;

                [NotNull]
                private readonly LineVector _lineVector;

                private readonly Matrix3x2 _inverseTransform;

                [NotNull]
                private readonly EditLineTool _tool;

                /// <summary>
                ///     Initializes a new instance of the <see cref="EditPointAction" /> class.
                /// </summary>
                /// <param name="lineVector">The line vector of the point to edit.</param>
                /// <param name="inverseTransform">The inverse transform matrix.</param>
                /// <param name="tool">The tool.</param>
                public EditPointAction(
                    [NotNull] LineVector lineVector,
                    Matrix3x2 inverseTransform,
                    [NotNull] EditLineTool tool)
                {
                    Debug.Assert(lineVector != null, "lineVector != null");
                    Debug.Assert(tool != null, "tool != null");
                    Debug.Assert(!lineVector.IsFixed, "!lineVector.IsFixed");

                    _lineVector = lineVector;
                    _inverseTransform = inverseTransform;
                    _initial = lineVector;
                    _tool = tool;
                }

                /// <summary>
                ///     Gets a value indicating whether this action changes any data that would need to be saved.
                /// </summary>
                /// <value>
                ///     <see langword="true" /> if the action changes data; otherwise, <see langword="false" />.
                /// </value>
                public override bool ChangesData => true;

                /// <summary>
                ///     Updates the location of the action.
                /// </summary>
                /// <param name="rawLocation">
                ///     The raw location that the action has been dragged to.
                ///     Should be transformed by the <see cref="IView.InverseViewMatrix" /> for the
                ///     <see cref="EscherTiler.Controllers.Controller.View" /> to get the location in 1the tiling itself.
                /// </param>
                public override void Update(Vector2 rawLocation)
                {
                    _lineVector.Vector = Vector2.Transform(rawLocation, _inverseTransform);
                }

                /// <summary>
                ///     Cancels this action.
                /// </summary>
                public override void Cancel()
                {
                    _lineVector.Vector = _initial;
                    _tool._selectedVector = null;
                }

                /// <summary>
                ///     Applies this action.
                /// </summary>
                public override void Apply()
                {
                    _tool._selectedVector = null;
                }
            }
        }

        public class SplitLineTool : Tool
        {
            /// <summary>
            ///     The tool name.
            /// </summary>
            public const string ToolName = "SplitLineTool";

            [CanBeNull]
            private SelectedLine _selectedLine, _hoverLine;

            private float _tolerance;

            /// <summary>
            ///     Gets or sets the tolerance for how close the point has to be to the line to select it.
            /// </summary>
            /// <value>
            ///     The tolerance.
            /// </value>
            /// <exception cref="ArgumentOutOfRangeException"></exception>
            public float Tolerance
            {
                get { return _tolerance; }
                set
                {
                    if (value <= 0) throw new ArgumentOutOfRangeException(nameof(value));
                    _tolerance = value;
                }
            }

            private Vector2 _splitLineStart, _splitLineEnd;

            /// <summary>
            ///     Initializes a new instance of the <see cref="SplitLineTool" /> class.
            /// </summary>
            /// <param name="controller">The controller.</param>
            public SplitLineTool([NotNull] TilingController controller, float tolerance)
                : base(controller, ToolName)
            {
                if (tolerance <= 0) throw new ArgumentOutOfRangeException(nameof(tolerance));
                _tolerance = tolerance;
            }

            /// <summary>
            ///     Gets the controller the tool belongs to.
            /// </summary>
            /// <value>
            ///     The controller.
            /// </value>
            [NotNull]
            public new TilingController Controller => (TilingController) base.Controller;

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

                if (_selectedLine != null)
                    return;

                LinePoint loc;
                _hoverLine = Controller.GetSelected(rawLocation, _tolerance, out loc);
                if (_hoverLine == null)
                    return;

                SetSplitLineLoc(_hoverLine, loc);
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
                LinePoint loc;
                _selectedLine = Controller.GetSelected(rawLocation, _tolerance, out loc);
                if (_selectedLine == null)
                    return null;

                return new SplitLineAction(_selectedLine, loc, this);
            }

            /// <summary>
            ///     Draws this object to the <see cref="IGraphics" /> provided.
            /// </summary>
            /// <param name="graphics">The graphics object to use to draw this object.</param>
            public override void Draw(IGraphics graphics)
            {
                base.Draw(graphics);

                float lineWidth = Controller.StyleManager.LineStyle.Width;
                graphics.LineStyle = SolidColourStyle.Black;
                graphics.LineWidth = lineWidth;

                SelectedLine currLine = _selectedLine ?? _hoverLine;
                if (currLine != null)
                {
                    Matrix3x2 transform = currLine.EdgePart.GetLineTransform() * currLine.Tile.Transform;

                    bool first = true;
                    foreach (ILine line in currLine.EdgePart.Lines)
                    {
                        if (first)
                        {
                            first = false;
                            graphics.DrawCircle(Vector2.Transform(line.Start, transform), lineWidth);
                        }

                        graphics.DrawCircle(Vector2.Transform(line.End, transform), lineWidth);
                    }

                    graphics.DrawLine(_splitLineStart, _splitLineEnd);
                }
            }

            /// <summary>
            ///     Sets the location that the line will be split along, for drwaing the preview.
            /// </summary>
            /// <param name="line">The line.</param>
            /// <param name="point">The point.</param>
            private void SetSplitLineLoc(SelectedLine line, LinePoint point)
            {
                Vector2 tangent = Vector2.Normalize(line.Line.GetTangent(point.Distance, line.LineTransform)) * 3;

                Vector2 pos = Vector2.Transform(point.Position, Controller.View.InverseViewMatrix);

                _splitLineStart = pos + new Vector2(tangent.Y, -tangent.X);
                _splitLineEnd = pos + new Vector2(-tangent.Y, tangent.X);
            }

            private class SplitLineAction : DragAction
            {
                [NotNull]
                private readonly SelectedLine _line;

                [NotNull]
                private LinePoint _point;

                [NotNull]
                private readonly SplitLineTool _tool;

                /// <summary>
                ///     Initializes a new instance of the <see cref="SplitLineAction" /> class.
                /// </summary>
                /// <param name="line">The line.</param>
                /// <param name="point">The point.</param>
                /// <param name="tool">The tool.</param>
                public SplitLineAction(SelectedLine line, LinePoint point, [NotNull] SplitLineTool tool)
                {
                    Debug.Assert(line != null, "line != null");
                    Debug.Assert(point != null, "point != null");
                    Debug.Assert(tool != null, "tool != null");
                    _line = line;
                    _point = point;
                    _tool = tool;
                }

                /// <summary>
                ///     Gets a value indicating whether this action changes any data that would need to be saved.
                /// </summary>
                /// <value>
                ///     <see langword="true" /> if the action changes data; otherwise, <see langword="false" />.
                /// </value>
                public override bool ChangesData => true;

                /// <summary>
                ///     Updates the location of the action.
                /// </summary>
                /// <param name="rawLocation">
                ///     The raw location that the action has been dragged to.
                ///     Should be transformed by the <see cref="IView.InverseViewMatrix" /> for the
                ///     <see cref="EscherTiler.Controllers.Controller.View" /> to get the location in 1the tiling itself.
                /// </param>
                public override void Update(Vector2 rawLocation)
                {
                    LinePoint hit = _line.Line.HitTest(rawLocation, float.PositiveInfinity, _line.LineTransform);
                    Debug.Assert(hit != null, "hit != null");

                    if (hit.Distance < 0)
                        _point = new LinePoint(Vector2.Transform(_line.Line.Start, _line.LineTransform), 0);
                    else if (hit.Distance > 1)
                        _point = new LinePoint(Vector2.Transform(_line.Line.End, _line.LineTransform), 1);
                    else _point = hit;

                    _tool.SetSplitLineLoc(_line, _point);
                }

                /// <summary>
                ///     Cancels this action.
                /// </summary>
                public override void Cancel()
                {
                    _tool._selectedLine = null;
                }

                /// <summary>
                ///     Applies this action.
                /// </summary>
                public override void Apply()
                {
                    _tool._selectedLine = null;
                    if (_point.Distance <= 0 || _point.Distance >= 1) return;

                    ILine line1, line2;
                    _line.Line.SplitLine(_point.Distance, out line1, out line2);

                    _line.EdgePart.Lines.Replace(_line.Line, line1, line2);
                }
            }
        }
    }
}