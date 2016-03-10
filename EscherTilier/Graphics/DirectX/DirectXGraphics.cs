using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Threading;
using EscherTilier.Graphics.Resources;
using EscherTilier.Numerics;
using EscherTilier.Styles;
using JetBrains.Annotations;
using SharpDX.Direct2D1;
using SharpDX.Mathematics.Interop;

namespace EscherTilier.Graphics.DirectX
{
    /// <summary>
    ///     DirectX graphics renderer.
    /// </summary>
    public class DirectXGraphics : IGraphics
    {
        private static readonly StrokeStyle _strokeStyle = DirectXResourceManager.CreateStrokeStyle(
            new StrokeStyleProperties
            {
                LineJoin = LineJoin.Round,
                StartCap = CapStyle.Round,
                EndCap = CapStyle.Round
            });

        [NotNull]
        private RenderTarget _renderTarget;

        [CanBeNull]
        private Brush _fillBrush;

        [CanBeNull]
        private Brush _lineBrush;

        [CanBeNull]
        private IStyle _fillStyle;

        [CanBeNull]
        private IStyle _lineStyle;

        private float _lineWidth;

        [NotNull]
        private DirectXResourceManager _resourceManager;

        [NotNull]
        private readonly Stack<State> _stateStack = new Stack<State>();

        /// <summary>
        ///     Initializes a new instance of the <see cref="DirectXGraphics" /> class.
        /// </summary>
        /// <param name="renderTarget">The render target.</param>
        /// <param name="resourceManager">The initial resource manager.</param>
        /// <param name="fillStyle">The initial fill style.</param>
        /// <param name="lineStyle">The initial line style.</param>
        /// <param name="lineWidth">Width of the line.</param>
        /// <exception cref="System.ArgumentNullException">
        /// </exception>
        public DirectXGraphics(
            [NotNull] RenderTarget renderTarget,
            [NotNull] IResourceManager resourceManager,
            [NotNull] IStyle fillStyle,
            [NotNull] IStyle lineStyle,
            float lineWidth)
        {
            if (renderTarget == null) throw new ArgumentNullException(nameof(renderTarget));
            if (resourceManager == null) throw new ArgumentNullException(nameof(resourceManager));
            if (fillStyle == null) throw new ArgumentNullException(nameof(fillStyle));
            if (lineStyle == null) throw new ArgumentNullException(nameof(lineStyle));
            _renderTarget = renderTarget;
            ResourceManager = resourceManager;
            FillStyle = fillStyle;
            LineStyle = lineStyle;
            LineWidth = lineWidth;
        }

        /// <summary>
        /// Gets or sets the render target.
        /// </summary>
        /// <value>
        /// The render target.
        /// </value>
        [NotNull]
        public RenderTarget RenderTarget
        {
            get { return _renderTarget; }
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));
                if (_renderTarget == value) return;

                if (_fillStyle != null)
                    _resourceManager.Release(_fillStyle);
                if (_lineStyle != null)
                    _resourceManager.Release(_lineStyle);
                _lineBrush = null;
                _fillBrush = null;

                _resourceManager.RenderTarget = value;
                _renderTarget = value;
            }
        }

        /// <summary>
        ///     Creates an <see cref="IGraphicsPath" /> that can be used to draw a path with this graphics object.
        /// </summary>
        /// <returns></returns>
        public IGraphicsPath CreatePath() => new GraphicsPath(DirectXResourceManager.CreatePathGeometry());

        /// <summary>
        ///     Draws a path.
        /// </summary>
        /// <param name="path">The path to draw.</param>
        public void DrawPath(IGraphicsPath path)
        {
            if (path == null) throw new ArgumentNullException(nameof(path));
            GraphicsPath gp = path as GraphicsPath;
            if (gp == null)
                throw new ArgumentException("The path must be a path returned by CreatePath", nameof(path));

            _renderTarget.DrawGeometry(gp.PathGeometry, LineBrush, _lineWidth, _strokeStyle);
        }

        /// <summary>
        ///     Fills the inside of a path.
        /// </summary>
        /// <param name="path">The path to fill.</param>
        public void FillPath(IGraphicsPath path)
        {
            if (path == null) throw new ArgumentNullException(nameof(path));
            GraphicsPath gp = path as GraphicsPath;
            if (gp == null)
                throw new ArgumentException("The path must be a path returned by CreatePath", nameof(path));

            _renderTarget.FillGeometry(gp.PathGeometry, FillBrush);
        }

        /// <summary>
        ///     Draws a line between two points.
        /// </summary>
        /// <param name="from">The point to draw the line from.</param>
        /// <param name="to">The point to draw the line to.</param>
        public void DrawLine(Vector2 @from, Vector2 to)
        {
            _renderTarget.DrawLine(@from.ToRawVector2(), to.ToRawVector2(), LineBrush, _lineWidth, _strokeStyle);
        }

        /// <summary>
        ///     Draws a set of lines joining the array of points given.
        /// </summary>
        /// <param name="points">The points to draw lines between.</param>
        public void DrawLines(Vector2[] points)
        {
            using (GraphicsPath path = new GraphicsPath(DirectXResourceManager.CreatePathGeometry()))
            {
                path.Start(points[0])
                    .AddLines(new ArraySegment<Vector2>(points, 1, points.Length - 1))
                    .End(false);

                _renderTarget.DrawGeometry(path.PathGeometry, LineBrush, _lineWidth, _strokeStyle);
            }
        }

        /// <summary>
        ///     Draws an arc of an elipse.
        /// </summary>
        /// <param name="from">The start point of the arc.</param>
        /// <param name="to">The end point of the arc.</param>
        /// <param name="radius">The radius of the arc.</param>
        /// <param name="angle">The angle that the arc sweeps.</param>
        /// <param name="clockwise">if set to <see langword="true" /> the arc is drawm clockwise.</param>
        public void DrawArc(Vector2 from, Vector2 to, Vector2 radius, float angle, bool clockwise)
        {
            using (GraphicsPath path = new GraphicsPath(DirectXResourceManager.CreatePathGeometry()))
            {
                path.Start(from)
                    .AddArc(to, radius, angle, clockwise)
                    .End(false);

                _renderTarget.DrawGeometry(path.PathGeometry, LineBrush, _lineWidth, _strokeStyle);
            }
        }

        /// <summary>
        ///     Draws a circle.
        /// </summary>
        /// <param name="point">The center point of the circle.</param>
        /// <param name="radius">The radius of the circle.</param>
        public void DrawCircle(Vector2 point, float radius)
        {
            _renderTarget.DrawEllipse(
                new Ellipse
                {
                    Point = point.ToRawVector2(),
                    RadiusX = radius,
                    RadiusY = radius
                },
                LineBrush,
                _lineWidth,
                _strokeStyle);
        }

        /// <summary>
        ///     Fills the inside of a circle.
        /// </summary>
        /// <param name="point">The center point of the circle.</param>
        /// <param name="radius">The radius of the circle.</param>
        public void FillCircle(Vector2 point, float radius)
        {
            _renderTarget.FillEllipse(
                new Ellipse
                {
                    Point = point.ToRawVector2(),
                    RadiusX = radius,
                    RadiusY = radius
                },
                FillBrush);
        }

        /// <summary>
        ///     Draws an ellipse.
        /// </summary>
        /// <param name="point">The center point of the ellipse.</param>
        /// <param name="radius">The radius of the elipse in each axis.</param>
        public void DrawEllipse(Vector2 point, Vector2 radius)
        {
            _renderTarget.DrawEllipse(
                new Ellipse
                {
                    Point = point.ToRawVector2(),
                    RadiusX = radius.X,
                    RadiusY = radius.Y
                },
                LineBrush,
                _lineWidth,
                _strokeStyle);
        }

        /// <summary>
        ///     Fills the inside of an ellipse.
        /// </summary>
        /// <param name="point">The center point of the ellipse.</param>
        /// <param name="radius">The radius of the elipse in each axis.</param>
        public void FillEllipse(Vector2 point, Vector2 radius)
        {
            _renderTarget.FillEllipse(
                new Ellipse
                {
                    Point = point.ToRawVector2(),
                    RadiusX = radius.X,
                    RadiusY = radius.Y
                },
                FillBrush);
        }

        /// <summary>
        ///     Draws a rectangle.
        /// </summary>
        /// <param name="rect">The rectangle to draw.</param>
        public void DrawRectangle(Rectangle rect)
        {
            _renderTarget.DrawRectangle(rect.ToRawRectangleF(), LineBrush, _lineWidth, _strokeStyle);
        }

        /// <summary>
        ///     Fills the inside of an rectangle.
        /// </summary>
        /// <param name="rect">The rectangle to fill.</param>
        public void FillRectangle(Rectangle rect)
        {
            _renderTarget.FillRectangle(rect.ToRawRectangleF(), FillBrush);
        }

        /// <summary>
        ///     Gets or sets the transform used when drawing.
        /// </summary>
        /// <value>
        ///     The transform.
        /// </value>
        public Matrix3x2 Transform
        {
            get { return _renderTarget.Transform.ToMatrix3x2(); }
            set { _renderTarget.Transform = value.ToRawMatrix3x2(); }
        }

        /// <summary>
        ///     Gets or sets the resource manager for the graphics object.
        /// </summary>
        /// <value>
        ///     The resource manager.
        /// </value>
        public IResourceManager ResourceManager
        {
            get
            {
                Debug.Assert(_resourceManager != null, "_resourceManager != null");
                return _resourceManager;
            }
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));
                if (value == _resourceManager) return;

                DirectXResourceManager manager = value as DirectXResourceManager;
                if (manager == null)
                    throw new ArgumentException("Expected resource manager of type DirectXResourceManager.");

                if (_fillStyle != null) _resourceManager.Release(_fillStyle);
                if (_lineStyle != null) _resourceManager.Release(_lineStyle);
                _lineBrush = null;
                _fillBrush = null;
                _resourceManager = manager;
            }
        }

        /// <summary>
        ///     Gets or sets the style used to fill the inside of shapes.
        /// </summary>
        /// <value>
        ///     The fill style.
        /// </value>
        public IStyle FillStyle
        {
            get
            {
                Debug.Assert(_fillStyle != null, "_fillStyle != null");
                return _fillStyle;
            }
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));
                if (value == _fillStyle) return;
                if (_resourceManager == null)
                    throw new InvalidOperationException("The ResourceManager must be set before setting the style.");
                if (_fillStyle != null)
                    _resourceManager.Release(_fillStyle);
                _fillBrush = _resourceManager.Get<IStyle, Brush>(value);
                _fillStyle = value;
            }
        }

        [NotNull]
        private Brush FillBrush
        {
            get
            {
                if (_fillBrush != null) return _fillBrush;
                Debug.Assert(_fillStyle != null, "_fillStyle != null");
                _fillBrush = _resourceManager.Get<IStyle, Brush>(_fillStyle);
                Debug.Assert(_fillBrush != null, "_fillBrush != null");
                return _fillBrush;
            }
        }

        /// <summary>
        ///     Gets or sets the style used to draw lines.
        /// </summary>
        /// <value>
        ///     The line style.
        /// </value>
        public IStyle LineStyle
        {
            get
            {
                Debug.Assert(_lineStyle != null, "_style != null");
                return _lineStyle;
            }
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));
                if (value == _lineStyle) return;
                if (_resourceManager == null)
                    throw new InvalidOperationException("The ResourceManager must be set before setting the style.");
                if (_lineStyle != null)
                    _resourceManager.Release(_lineStyle);
                _lineBrush = _resourceManager.Get<IStyle, Brush>(value);
                _lineStyle = value;
            }
        }

        [NotNull]
        private Brush LineBrush
        {
            get
            {
                if (_lineBrush != null) return _lineBrush;
                Debug.Assert(_lineStyle != null, "_lineStyle != null");
                _lineBrush = _resourceManager.Get<IStyle, Brush>(_lineStyle);
                Debug.Assert(_lineBrush != null, "_lineBrush != null");
                return _lineBrush;
            }
        }

        /// <summary>
        ///     Gets or sets the width of the drawn line.
        /// </summary>
        /// <value>
        ///     The width of the line.
        /// </value>
        public float LineWidth
        {
            get { return _lineWidth; }
            set
            {
                if (value <= 0) throw new ArgumentOutOfRangeException(nameof(value));
                _lineWidth = value;
            }
        }

        /// <summary>
        ///     Sets the line style.
        /// </summary>
        /// <param name="lineStyle">The line style.</param>
        public void SetLineStyle(LineStyle lineStyle)
        {
            if (lineStyle == null) throw new ArgumentNullException(nameof(lineStyle));
            LineStyle = lineStyle.Style;
            LineWidth = lineStyle.Width;
        }

        /// <summary>
        ///     Saves the current state of the graphics to a stack.
        /// </summary>
        public void SaveState() => _stateStack.Push(new State(this));

        /// <summary>
        ///     Restores the previously saved state from the stack.
        /// </summary>
        public void RestoreState()
        {
            if (_stateStack.Count > 0) _stateStack.Pop().Restore(this);
        }

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (_fillStyle != null)
            {
                _resourceManager.Release(_fillStyle);
                _fillBrush = null;
            }
            if (_lineStyle != null)
            {
                _resourceManager.Release(_lineStyle);
                _lineBrush = null;
            }
        }

        /// <summary>
        ///     Stores the state of this class.
        /// </summary>
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        private class State
        {
            [NotNull]
            private readonly IStyle FillStyle;

            [NotNull]
            private readonly IStyle LineStyle;

            private readonly float LineWidth;

            private readonly Matrix3x2 Transform;

            [NotNull]
            private readonly DirectXResourceManager ResourceManager;

            public State([NotNull] DirectXGraphics graphics)
            {
                Debug.Assert(graphics != null, "graphics != null");
                FillStyle = graphics.FillStyle;
                LineStyle = graphics.LineStyle;
                LineWidth = graphics._lineWidth;
                ResourceManager = graphics._resourceManager;
                Transform = graphics.Transform;
            }

            public void Restore([NotNull] DirectXGraphics graphics)
            {
                Debug.Assert(graphics != null, "graphics != null");
                graphics._resourceManager = ResourceManager;
                graphics.FillStyle = FillStyle;
                graphics.LineStyle = LineStyle;
                graphics._lineWidth = LineWidth;
                graphics.Transform = Transform;
            }
        }

        /// <summary>
        ///     The <see cref="IGraphicsPath" /> implementation for DirectX.
        /// </summary>
        private class GraphicsPath : IGraphicsPath
        {
            private GeometrySink _sink;

            /// <summary>
            ///     The path geometry.
            /// </summary>
            [NotNull]
            public readonly PathGeometry PathGeometry;

            /// <summary>
            ///     Initializes a new instance of the <see cref="GraphicsPath" /> class.
            /// </summary>
            /// <param name="pathGeometry">The path geometry.</param>
            public GraphicsPath([NotNull] PathGeometry pathGeometry)
            {
                Debug.Assert(pathGeometry != null, "pathGeometry != null");
                PathGeometry = pathGeometry;
                // ReSharper disable once AssignNullToNotNullAttribute
                _sink = pathGeometry.Open();
                Debug.Assert(_sink != null);
            }

            /// <summary>
            ///     Starts the path at the point given.
            /// </summary>
            /// <param name="point">The point to start the path at.</param>
            /// <returns>This <see cref="IGraphicsPath" />.</returns>
            public IGraphicsPath Start(Vector2 point)
            {
                _sink?.BeginFigure(point.ToRawVector2(), FigureBegin.Filled);
                return this;
            }

            /// <summary>
            ///     Ends the path, optionally closing it.
            /// </summary>
            /// <param name="close">if set to <see langword="true" /> the path will be closed; otherwise it will be left open.</param>
            /// <returns>This <see cref="IGraphicsPath" />.</returns>
            public IGraphicsPath End(bool close = true)
            {
                _sink?.EndFigure(close ? FigureEnd.Closed : FigureEnd.Open);
                _sink?.Close();
                return this;
            }

            /// <summary>
            ///     Adds a line to the end of the path.
            /// </summary>
            /// <param name="to">The end point of the line.</param>
            /// <returns>This <see cref="IGraphicsPath" />.</returns>
            public IGraphicsPath AddLine(Vector2 to)
            {
                _sink?.AddLine(to.ToRawVector2());
                return this;
            }

            /// <summary>
            ///     Adds a series of lines to the end of the path.
            /// </summary>
            /// <param name="points">The points of the lines to add.</param>
            /// <returns>This <see cref="IGraphicsPath" />.</returns>
            public IGraphicsPath AddLines([NotNull] params Vector2[] points)
            {
                if (points == null) throw new ArgumentNullException(nameof(points));
                RawVector2[] rawVecs = new RawVector2[points.Length];
                for (int i = 0; i < points.Length; i++)
                    rawVecs[i] = points[i].ToRawVector2();
                _sink?.AddLines(rawVecs);
                return this;
            }

            /// <summary>
            ///     Adds a series of lines to the end of the path.
            /// </summary>
            /// <param name="points">The points of the lines to add.</param>
            /// <returns>This <see cref="IGraphicsPath" />.</returns>
            public IGraphicsPath AddLines(ArraySegment<Vector2> points)
            {
                if (points.Array == null) throw new ArgumentNullException(nameof(points));
                RawVector2[] rawVecs = new RawVector2[points.Count];
                for (int i = 0, j = points.Offset; i < points.Count; i++, j++)
                    rawVecs[i] = points.Array[j].ToRawVector2();
                _sink?.AddLines(rawVecs);
                return this;
            }

            /// <summary>
            ///     Adds an arc of an elipse to the end of the path.
            /// </summary>
            /// <param name="to">The end point of the arc.</param>
            /// <param name="radius">The radius of the arc.</param>
            /// <param name="angle">The sweep angle of the arc.</param>
            /// <param name="clockwise">if set to <see langword="true" /> the arc will be drawn clockwise.</param>
            /// <returns>This <see cref="IGraphicsPath" />.</returns>
            public IGraphicsPath AddArc(Vector2 to, Vector2 radius, float angle, bool clockwise)
            {
                _sink?.AddArc(
                    new ArcSegment
                    {
                        Point = to.ToRawVector2(),
                        RotationAngle = angle,
                        Size = radius.ToSize2F(),
                        SweepDirection = clockwise ? SweepDirection.Clockwise : SweepDirection.CounterClockwise
                    });
                return this;
            }

            /// <summary>
            ///     Adds a quadratic bezier curve to the end of the line.
            /// </summary>
            /// <param name="control">The control point of the curve.</param>
            /// <param name="to">The end point of the curve.</param>
            /// <returns>This <see cref="IGraphicsPath" />.</returns>
            public IGraphicsPath AddQuadraticBezier(Vector2 control, Vector2 to)
            {
                _sink?.AddQuadraticBezier(
                    new QuadraticBezierSegment
                    {
                        Point1 = control.ToRawVector2(),
                        Point2 = to.ToRawVector2()
                    });
                return this;
            }

            /// <summary>
            ///     Adds a cubic bezier curve to the end of the line.
            /// </summary>
            /// <param name="controlA">The first control point of the curve.</param>
            /// <param name="controlB">The second control point of the curve.</param>
            /// <param name="to">The end point of the curve.</param>
            /// <returns>This <see cref="IGraphicsPath" />.</returns>
            public IGraphicsPath AddCubicBezier(Vector2 controlA, Vector2 controlB, Vector2 to)
            {
                _sink?.AddBezier(
                    new BezierSegment
                    {
                        Point1 = controlA.ToRawVector2(),
                        Point2 = controlB.ToRawVector2(),
                        Point3 = to.ToRawVector2()
                    });
                return this;
            }

            /// <summary>
            ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
            /// </summary>
            public void Dispose()
            {
                GeometrySink sink = Interlocked.Exchange(ref _sink, null);
                if (sink != null)
                {
                    sink.Dispose();
                    PathGeometry.Dispose();
                }
            }
        }
    }
}