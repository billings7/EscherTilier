using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Numerics;
using System.Threading;
using EscherTilier.Graphics.Resources;
using EscherTilier.Styles;
using JetBrains.Annotations;
using GDIGraphicsPath = System.Drawing.Drawing2D.GraphicsPath;
using Rectangle = EscherTilier.Numerics.Rectangle;

namespace EscherTilier.Graphics.GDI
{
    public class GDIGraphics : IGraphics
    {
        [NotNull]
        private System.Drawing.Graphics _graphics;

        [CanBeNull]
        private Brush _fillBrush;

        [CanBeNull]
        private Pen _linePen;

        [CanBeNull]
        private IStyle _fillStyle;

        [CanBeNull]
        private LineStyle _lineStyle;

        [NotNull]
        private GDIResourceManager _resourceManager;

        [NotNull]
        private readonly Stack<State> _stateStack = new Stack<State>();

        /// <summary>
        ///     Initializes a new instance of the <see cref="GDIGraphics" /> class.
        /// </summary>
        /// <param name="graphics">The graphics.</param>
        /// <param name="resourceManager">The initial resource manager.</param>
        /// <param name="fillStyle">The initial fill style.</param>
        /// <param name="lineStyle">The initial line style.</param>
        /// <exception cref="System.ArgumentNullException">
        /// </exception>
        public GDIGraphics(
            [NotNull] System.Drawing.Graphics graphics,
            [NotNull] IResourceManager resourceManager,
            [NotNull] IStyle fillStyle,
            [NotNull] LineStyle lineStyle)
        {
            if (graphics == null) throw new ArgumentNullException(nameof(graphics));
            if (resourceManager == null) throw new ArgumentNullException(nameof(resourceManager));
            if (fillStyle == null) throw new ArgumentNullException(nameof(fillStyle));
            if (lineStyle == null) throw new ArgumentNullException(nameof(lineStyle));
            _graphics = graphics;
            ResourceManager = resourceManager;
            FillStyle = fillStyle;
            SetLineStyle(lineStyle);
        }

        /// <summary>
        ///     Gets or sets the render target.
        /// </summary>
        /// <value>
        ///     The render target.
        /// </value>
        [NotNull]
        public System.Drawing.Graphics Graphics
        {
            get { return _graphics; }
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));
                _graphics = value;
            }
        }

        /// <summary>
        ///     Creates an <see cref="IGraphicsPath" /> that can be used to draw a path with this graphics object.
        /// </summary>
        /// <returns></returns>
        public IGraphicsPath CreatePath() => new GraphicsPath();

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

            _graphics.DrawPath(LinePen, gp.PathGeometry);
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

            _graphics.FillPath(FillBrush, gp.PathGeometry);
        }

        /// <summary>
        ///     Draws a line between two points.
        /// </summary>
        /// <param name="from">The point to draw the line from.</param>
        /// <param name="to">The point to draw the line to.</param>
        public void DrawLine(Vector2 @from, Vector2 to)
        {
            _graphics.DrawLine(LinePen, @from.ToPointF(), to.ToPointF());
        }

        /// <summary>
        ///     Draws a set of lines joining the array of points given.
        /// </summary>
        /// <param name="points">The points to draw lines between.</param>
        public void DrawLines(Vector2[] points)
        {
            _graphics.DrawLines(LinePen, points.Select(GDIExtensions.ToPointF).ToArray());
        }

        /// <summary>
        ///     Draws an arc of an elipse.
        /// </summary>
        /// <param name="from">The start point of the arc.</param>
        /// <param name="to">The end point of the arc.</param>
        /// <param name="radius">The radius of the arc.</param>
        /// <param name="angle">The angle of the arc, in radians.</param>
        /// <param name="clockwise">If set to <see langword="true" /> the arc will be drawn clockwise.</param>
        /// <param name="isLarge">Specifies whether the given arc is larger than 180 degrees</param>
        public void DrawArc(Vector2 @from, Vector2 to, Vector2 radius, float angle, bool clockwise, bool isLarge)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Draws a quadratic bezier curve to the end of the line.
        /// </summary>
        /// <param name="from">The start point of the curve.</param>
        /// <param name="control">The control point of the curve.</param>
        /// <param name="to">The end point of the curve.</param>
        /// <returns>This <see cref="IGraphicsPath" />.</returns>
        public void DrawQuadraticBezier(Vector2 @from, Vector2 control, Vector2 to)
        {
            _graphics.DrawBezier(
                LinePen,
                @from.ToPointF(),
                (@from + (2f / 3f * (control - @from))).ToPointF(),
                (to + (2f / 3f * (control - to))).ToPointF(),
                to.ToPointF());
        }

        /// <summary>
        ///     Draws a cubic bezier curve to the end of the line.
        /// </summary>
        /// <param name="from">The start point of the curve.</param>
        /// <param name="controlA">The first control point of the curve.</param>
        /// <param name="controlB">The second control point of the curve.</param>
        /// <param name="to">The end point of the curve.</param>
        /// <returns>This <see cref="IGraphicsPath" />.</returns>
        public void DrawCubicBezier(Vector2 @from, Vector2 controlA, Vector2 controlB, Vector2 to)
        {
            _graphics.DrawBezier(
                LinePen,
                @from.ToPointF(),
                controlA.ToPointF(),
                controlB.ToPointF(),
                to.ToPointF());
        }

        /// <summary>
        ///     Draws a circle.
        /// </summary>
        /// <param name="point">The center point of the circle.</param>
        /// <param name="radius">The radius of the circle.</param>
        public void DrawCircle(Vector2 point, float radius)
        {
            _graphics.DrawEllipse(
                LinePen,
                point.X - radius,
                point.Y - radius,
                radius * 2,
                radius * 2);
        }

        /// <summary>
        ///     Fills the inside of a circle.
        /// </summary>
        /// <param name="point">The center point of the circle.</param>
        /// <param name="radius">The radius of the circle.</param>
        public void FillCircle(Vector2 point, float radius)
        {
            _graphics.FillEllipse(
                FillBrush,
                point.X - radius,
                point.Y - radius,
                radius * 2,
                radius * 2);
        }

        /// <summary>
        ///     Draws an ellipse.
        /// </summary>
        /// <param name="point">The center point of the ellipse.</param>
        /// <param name="radius">The radius of the elipse in each axis.</param>
        public void DrawEllipse(Vector2 point, Vector2 radius)
        {
            _graphics.DrawEllipse(
                LinePen,
                point.X - radius.X,
                point.Y - radius.Y,
                radius.X * 2,
                radius.Y * 2);
        }

        /// <summary>
        ///     Fills the inside of an ellipse.
        /// </summary>
        /// <param name="point">The center point of the ellipse.</param>
        /// <param name="radius">The radius of the elipse in each axis.</param>
        public void FillEllipse(Vector2 point, Vector2 radius)
        {
            _graphics.FillEllipse(
                FillBrush,
                point.X - radius.X,
                point.Y - radius.Y,
                radius.X * 2,
                radius.Y * 2);
        }

        /// <summary>
        ///     Draws a rectangle.
        /// </summary>
        /// <param name="rect">The rectangle to draw.</param>
        public void DrawRectangle(Rectangle rect)
        {
            _graphics.DrawRectangle(LinePen, rect.ToGDIRectangle());
        }

        /// <summary>
        ///     Fills the inside of an rectangle.
        /// </summary>
        /// <param name="rect">The rectangle to fill.</param>
        public void FillRectangle(Rectangle rect)
        {
            _graphics.FillRectangle(FillBrush, rect.ToGDIRectangle());
        }

        /// <summary>
        ///     Gets or sets the transform used when drawing.
        /// </summary>
        /// <value>
        ///     The transform.
        /// </value>
        public Matrix3x2 Transform
        {
            get
            {
                using (Matrix transform = _graphics.Transform)
                    return transform.ToMatrix3x2();
            }
            set { _graphics.Transform = value.ToMatrix(); }
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

                GDIResourceManager manager = value as GDIResourceManager;
                if (manager == null)
                    throw new ArgumentException("Expected resource manager of type GDIResourceManager.");

                if (_fillStyle != null) _resourceManager.Release(_fillStyle);
                if (_lineStyle != null) _resourceManager.Release(_lineStyle);
                _linePen = null;
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
        public SolidColourStyle LineStyle
        {
            get
            {
                Debug.Assert(_lineStyle != null, "_style != null");
                return _lineStyle.Style;
            }
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));
                if (value == _lineStyle?.Style) return;
                if (_resourceManager == null)
                    throw new InvalidOperationException("The ResourceManager must be set before setting the style.");
                if (_lineStyle != null)
                    _resourceManager.Release(_lineStyle);

                LineStyle newStyle = new LineStyle(_lineStyle?.Width ?? 0, value);
                _linePen = _resourceManager.Get<LineStyle, Pen>(newStyle);
                _lineStyle = newStyle;
            }
        }

        [NotNull]
        private Pen LinePen
        {
            get
            {
                if (_linePen != null) return _linePen;
                Debug.Assert(_lineStyle != null, "_lineStyle != null");
                _linePen = _resourceManager.Get<LineStyle, Pen>(_lineStyle);
                Debug.Assert(_linePen != null, "_linePen != null");
                return _linePen;
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
            get { return _lineStyle?.Width ?? 0; }
            set
            {
                if (value <= 0) throw new ArgumentOutOfRangeException(nameof(value));
                // ReSharper disable once CompareOfFloatsByEqualityOperator
                if (value == (_lineStyle?.Width ?? 0)) return;
                if (_resourceManager == null)
                {
                    throw new InvalidOperationException(
                        "The ResourceManager must be set before setting the line width.");
                }
                if (_lineStyle == null)
                    throw new InvalidOperationException("The LineStyle must be set before setting the line width.");
                _resourceManager.Release(_lineStyle);

                LineStyle newStyle = new LineStyle(value, _lineStyle.Style);
                _linePen = _resourceManager.Get<LineStyle, Pen>(newStyle);
                _lineStyle = newStyle;
            }
        }

        /// <summary>
        ///     Sets the line style.
        /// </summary>
        /// <param name="lineStyle">The line style.</param>
        public void SetLineStyle(LineStyle lineStyle)
        {
            if (lineStyle == null) throw new ArgumentNullException(nameof(lineStyle));

            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (lineStyle.Style == LineStyle && lineStyle.Width == LineWidth) return;
            if (_resourceManager == null)
                throw new InvalidOperationException("The ResourceManager must be set before setting the style.");
            if (_lineStyle != null)
                _resourceManager.Release(_lineStyle);

            _linePen = _resourceManager.Get<LineStyle, Pen>(lineStyle);
            _lineStyle = lineStyle;
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
                _linePen = null;
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
            private readonly LineStyle LineStyle;

            private readonly Matrix3x2 Transform;

            [NotNull]
            private readonly GDIResourceManager ResourceManager;

            public State([NotNull] GDIGraphics graphics)
            {
                Debug.Assert(graphics != null, "graphics != null");
                Debug.Assert(graphics._lineStyle != null, "_style != null");
                FillStyle = graphics.FillStyle;
                LineStyle = graphics._lineStyle;
                ResourceManager = graphics._resourceManager;
                Transform = graphics.Transform;
            }

            public void Restore([NotNull] GDIGraphics graphics)
            {
                Debug.Assert(graphics != null, "graphics != null");
                graphics._resourceManager = ResourceManager;
                graphics.FillStyle = FillStyle;
                graphics.SetLineStyle(LineStyle);
                graphics.Transform = Transform;
            }
        }

        /// <summary>
        ///     The <see cref="IGraphicsPath" /> implementation for DirectX.
        /// </summary>
        private class GraphicsPath : IGraphicsPath
        {
            private GDIGraphicsPath _pathGeometry;

            private PointF _lastPoint;

            /// <summary>
            ///     Initializes a new instance of the <see cref="GraphicsPath" /> class.
            /// </summary>
            public GraphicsPath()
            {
                _pathGeometry = new GDIGraphicsPath();
            }

            [NotNull]
            public GDIGraphicsPath PathGeometry
            {
                get
                {
                    if (_pathGeometry == null)
                        throw new ObjectDisposedException(nameof(GraphicsPath));
                    return _pathGeometry;
                }
            }

            /// <summary>
            ///     Starts the path at the point given.
            /// </summary>
            /// <param name="point">The point to start the path at.</param>
            /// <returns>This <see cref="IGraphicsPath" />.</returns>
            public IGraphicsPath Start(Vector2 point)
            {
                _lastPoint = point.ToPointF();
                return this;
            }

            /// <summary>
            ///     Ends the path, optionally closing it.
            /// </summary>
            /// <param name="close">if set to <see langword="true" /> the path will be closed; otherwise it will be left open.</param>
            /// <returns>This <see cref="IGraphicsPath" />.</returns>
            public IGraphicsPath End(bool close = true)
            {
                _pathGeometry?.CloseFigure();
                return this;
            }

            /// <summary>
            ///     Adds a line to the end of the path.
            /// </summary>
            /// <param name="to">The end point of the line.</param>
            /// <returns>This <see cref="IGraphicsPath" />.</returns>
            public IGraphicsPath AddLine(Vector2 to)
            {
                PointF end = to.ToPointF();
                _pathGeometry?.AddLine(_lastPoint, end);
                _lastPoint = end;
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
                PointF[] pointFs = new PointF[points.Length + 1];
                pointFs[0] = _lastPoint;

                for (int i = 1; i <= points.Length; i++)
                    pointFs[i] = _lastPoint = points[i].ToPointF();

                _pathGeometry?.AddLines(pointFs);
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

                PointF[] pointFs = new PointF[points.Count + 1];
                pointFs[0] = _lastPoint;

                for (int i = 1, j = points.Offset; i <= points.Count; i++, j++)
                    pointFs[i] = _lastPoint = points.Array[j].ToPointF();

                _pathGeometry?.AddLines(pointFs);
                return this;
            }

            /// <summary>
            ///     Adds an arc of an elipse to the end of the path.
            /// </summary>
            /// <param name="to">The end point of the arc.</param>
            /// <param name="radius">The radius of the arc.</param>
            /// <param name="angle">The angle of the arc, in radians.</param>
            /// <param name="clockwise">If set to <see langword="true" /> the arc will be drawn clockwise.</param>
            /// <param name="isLarge">Specifies whether the given arc is larger than 180 degrees</param>
            /// <returns>
            ///     This <see cref="IGraphicsPath" />.
            /// </returns>
            public IGraphicsPath AddArc(Vector2 to, Vector2 radius, float angle, bool clockwise, bool isLarge)
            {
                throw new NotImplementedException();
            }

            /// <summary>
            ///     Adds a quadratic bezier curve to the end of the line.
            /// </summary>
            /// <param name="control">The control point of the curve.</param>
            /// <param name="to">The end point of the curve.</param>
            /// <returns>This <see cref="IGraphicsPath" />.</returns>
            public IGraphicsPath AddQuadraticBezier(Vector2 control, Vector2 to)
            {
                Vector2 @from = _lastPoint.ToVector2();
                PointF end = to.ToPointF();

                _pathGeometry?.AddBezier(
                    _lastPoint,
                    (@from + (2f / 3f * (control - @from))).ToPointF(),
                    (to + (2f / 3f * (control - to))).ToPointF(),
                    end);
                _lastPoint = end;

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
                PointF end = to.ToPointF();

                PathGeometry?.AddBezier(
                    _lastPoint,
                    controlA.ToPointF(),
                    controlB.ToPointF(),
                    end);
                _lastPoint = end;

                return this;
            }

            /// <summary>
            ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
            /// </summary>
            public void Dispose() => Interlocked.Exchange(ref _pathGeometry, null)?.Dispose();
        }
    }
}