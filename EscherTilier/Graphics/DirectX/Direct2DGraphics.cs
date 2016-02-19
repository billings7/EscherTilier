using System;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Threading;
using EscherTilier.Numerics;
using EscherTilier.Styles;
using JetBrains.Annotations;
using SharpDX.Direct2D1;

namespace EscherTilier.Graphics.DirectX
{
    internal class DirectXGraphics : IGraphics
    {
        private static StrokeStyle _strokeStyle = DirectXResourceManager.CreateStrokeStyle(
            new StrokeStyleProperties
            {
                LineJoin = LineJoin.Round,
                StartCap = CapStyle.Round,
                EndCap = CapStyle.Round
            });

        [NotNull]
        private readonly RenderTarget _renderTarget;

        private Brush _fillBrush;
        private Brush _lineBrush;

        [NotNull]
        private LineStyle _lineStyle;

        [NotNull]
        private IStyle _style;

        [NotNull]
        private DirectXResourceManager _resourceManager;

        public DirectXGraphics(
            [NotNull] RenderTarget renderTarget,
            [NotNull] DirectXResourceManager resourceManager,
            [NotNull] IStyle style,
            [NotNull] LineStyle lineStyle)
        {
            if (renderTarget == null) throw new ArgumentNullException(nameof(renderTarget));
            if (resourceManager == null) throw new ArgumentNullException(nameof(resourceManager));
            if (style == null) throw new ArgumentNullException(nameof(style));
            if (lineStyle == null) throw new ArgumentNullException(nameof(lineStyle));
            _renderTarget = renderTarget;
            _resourceManager = resourceManager;
            Style = style;
            LineStyle = lineStyle;
        }

        public IGraphicsPath CreatePath() => new GraphicsPath(DirectXResourceManager.CreatePathGeometry());

        public void DrawPath(IGraphicsPath path)
        {
            if (path == null) throw new ArgumentNullException(nameof(path));
            GraphicsPath gp = path as GraphicsPath;
            if (gp == null)
                throw new ArgumentException("The path must be a path returned by CreatePath", nameof(path));

            _renderTarget.DrawGeometry(gp.PathGeometry, _lineBrush, LineStyle.Width, _strokeStyle);
        }

        public void FillPath(IGraphicsPath path)
        {
            throw new NotImplementedException();
        }

        public void DrawLine(Vector2 @from, Vector2 to)
        {
            _renderTarget.DrawLine(@from.ToRawVector2(), to.ToRawVector2(), _lineBrush, LineStyle.Width, _strokeStyle);
        }

        public void DrawLines(Vector2[] points)
        {
            throw new NotImplementedException();
        }

        public void DrawArc(Vector2 to, Vector2 radius, float angle, bool clockwise)
        {
            throw new NotImplementedException();
        }

        public void DrawCircle(Vector2 point, float radius)
        {
            throw new NotImplementedException();
        }

        public void FillCircle(Vector2 point, float radius)
        {
            _renderTarget.FillEllipse(
                new Ellipse
                {
                    Point = point.ToRawVector2(),
                    RadiusX = radius,
                    RadiusY = radius
                },
                _fillBrush);
        }

        public void DrawEllipse(Vector2 point, Vector2 radius)
        {
            throw new NotImplementedException();
        }

        public void FillEllipse(Vector2 point, Vector2 radius)
        {
            _renderTarget.FillEllipse(
                new Ellipse
                {
                    Point = point.ToRawVector2(),
                    RadiusX = radius.X,
                    RadiusY = radius.Y
                },
                _fillBrush);
        }

        public void DrawRectangle(Rectangle rect)
        {
            throw new NotImplementedException();
        }

        public void FillEllipse(Rectangle rect)
        {
            throw new NotImplementedException();
        }

        public Matrix3x2 Transform { get; set; }

        [NotNull]
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

                DirectXResourceManager manager = value as DirectXResourceManager;
                if (manager == null)
                    throw new ArgumentException("Expected resource manager of type DirectXResourceManager.");

                _resourceManager = manager;
            }
        }

        [NotNull]
        public IStyle Style
        {
            get
            {
                Debug.Assert(_style != null, "_style != null");
                return _style;
            }
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));
                if (_resourceManager == null)
                    throw new InvalidOperationException("The ResourceManager must be set before setting the style.");
                _fillBrush = _resourceManager.GetBrush(value);
                _style = value;
            }
        }

        [NotNull]
        public LineStyle LineStyle
        {
            get
            {
                Debug.Assert(_lineStyle != null, "_lineStyle != null");
                return _lineStyle;
            }
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));
                if (_resourceManager == null)
                    throw new InvalidOperationException("The ResourceManager must be set before setting the style.");
                _lineBrush = _resourceManager.GetBrush(value.Style);
                _lineStyle = value;
            }
        }

        public void SaveState()
        {
            throw new NotImplementedException();
        }

        public void RestoreState()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        private class GraphicsPath : IGraphicsPath
        {
            private GeometrySink _sink;

            [NotNull]
            public readonly PathGeometry PathGeometry;

            public GraphicsPath([NotNull] PathGeometry pathGeometry)
            {
                Debug.Assert(pathGeometry != null, "pathGeometry != null");
                PathGeometry = pathGeometry;
                // ReSharper disable once AssignNullToNotNullAttribute
                _sink = pathGeometry.Open();
                Debug.Assert(_sink != null);
            }

            public IGraphicsPath Start(Vector2 point)
            {
                _sink?.BeginFigure(point.ToRawVector2(), FigureBegin.Filled);
                return this;
            }

            public IGraphicsPath End(bool close = true)
            {
                _sink?.EndFigure(close ? FigureEnd.Closed : FigureEnd.Open);
                _sink?.Close();
                return this;
            }

            public IGraphicsPath AddLine(Vector2 to)
            {
                _sink?.AddLine(to.ToRawVector2());
                return this;
            }

            public IGraphicsPath AddLines([NotNull] params Vector2[] points)
            {
                if (points == null) throw new ArgumentNullException(nameof(points));
                _sink?.AddLines(points.Select(DirectXExtensions.ToRawVector2).ToArray());
                return this;
            }

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
                    //sink.Close();
                    sink.Dispose();
                    PathGeometry.Dispose();
                }
            }
        }
    }
}