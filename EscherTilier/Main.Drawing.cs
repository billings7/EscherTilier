using System;
using System.Collections.Concurrent;
using EscherTilier.Graphics;
using EscherTilier.Graphics.DirectX;
using EscherTilier.Numerics;
using EscherTilier.Styles;
using EscherTilier.Utilities;
using JetBrains.Annotations;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.DXGI;
using GradientStop = EscherTilier.Styles.GradientStop;
using Matrix3x2 = System.Numerics.Matrix3x2;
using System.Threading;
using System.Diagnostics;
using Vector2 = System.Numerics.Vector2;

namespace EscherTilier
{
    public partial class Main
    {
        [NotNull]
        private readonly ConcurrentDictionary<StyleManager, DirectXResourceManager> _resourceManagers =
            new ConcurrentDictionary<StyleManager, DirectXResourceManager>();

        [CanBeNull]
        private DirectXResourceManager _miskResourceManager;

        [CanBeNull]
        private DirectXGraphics _directXGraphics;

        [NotNull]
        private readonly SolidColourStyle _grayStyle = new SolidColourStyle(Colour.Gray);

        [NotNull]
        private readonly SolidColourStyle _blackStyle = new SolidColourStyle(Colour.Black);

        private void InitializeGraphics()
        {
            _miskResourceManager = new DirectXResourceManager(renderControl.RenderTarget);
            _miskResourceManager.Add(_grayStyle);
            _miskResourceManager.Add(_blackStyle);
            _directXGraphics = new DirectXGraphics(
                renderControl.RenderTarget,
                _miskResourceManager,
                _grayStyle,
                new LineStyle(2, _blackStyle));
        }

        private void UnloadGraphics()
        {
            Interlocked.Exchange(ref _directXGraphics, null)?.Dispose();
            Interlocked.Exchange(ref _miskResourceManager, null)?.Dispose();

            foreach (var managers in _resourceManagers)
            {
                managers.Key.Dispose();
                managers.Value.Dispose();
            }
        }

        partial void renderControl_Render([NotNull] RenderTarget renderTarget, [NotNull] SwapChain swapChain)
        {
            renderTarget.BeginDraw();
            renderTarget.Transform = ViewMatrix.ToRawMatrix3x2();
            renderTarget.Clear(Color.White);

            if (_shape != null)
            {
                using (IGraphicsPath path = _directXGraphics.CreatePath())
                {
                    bool first = true;
                    foreach (Vertex vertex in _shape.Vertices)
                    {
                        if (first) path.Start(vertex.Location);
                        else path.AddLine(vertex.Location);
                        first = false;
                    }

                    path.End();

                    _directXGraphics.DrawPath(path);
                }

                IStyle s =
                    new LinearGradientStyle(Vector2.Zero,
                        new Vector2(50, 50), new[] { new GradientStop(Colour.Aqua, 0), new GradientStop(Colour.DarkOrange, 1) });

                using (_directXGraphics.TempState(s))
                    _directXGraphics.FillRectangle(new Numerics.Rectangle(0, 0, 50, 50));

                Vertex selectedVertex = _selected as Vertex;
                Edge selectedEdge;
                if (selectedVertex != null)
                {
                    _directXGraphics.FillCircle(selectedVertex.Location, 1.25f);

                    GradientStop[] gradientStops =
                    {
                        new GradientStop(Colour.Gray, 0),
                        new GradientStop(Colour.Black, 1)
                    };

                    LinearGradientStyle style = new LinearGradientStyle(selectedVertex.Location,
                        selectedVertex.In.Start.Location, gradientStops);

                    using (_directXGraphics.TempState(lineStyle: style))
                        _directXGraphics.DrawLine(selectedVertex.Location, selectedVertex.In.Start.Location);

                    style = new LinearGradientStyle(selectedVertex.Location,
                        selectedVertex.Out.End.Location, gradientStops);

                    using (_directXGraphics.TempState(lineStyle: style))
                        _directXGraphics.DrawLine(selectedVertex.Location, selectedVertex.Out.End.Location);
                }
                else if ((selectedEdge = _selected as Edge) != null)
                {
                    using (_directXGraphics.TempState(lineStyle: _grayStyle))
                        _directXGraphics.DrawLine(selectedEdge.Start.Location, selectedEdge.End.Location);
                }
            }

            renderTarget.EndDraw();
            swapChain.Present(0, PresentFlags.None);
        }

        partial void renderControl_RenderTargetChanged([NotNull] RenderTarget renderTarget)
        {
            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            if (_miskResourceManager == null) return;

            _miskResourceManager.RenderTarget = renderTarget;
            foreach (DirectXResourceManager manager in _resourceManagers.Values)
                manager.RenderTarget = renderTarget;
            _directXGraphics = new DirectXGraphics(
                renderTarget,
                _miskResourceManager,
                _grayStyle,
                new LineStyle(2, _blackStyle));
        }
    }
}