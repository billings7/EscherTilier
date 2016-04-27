using System;
using System.Threading;
using EscherTiler.Controllers;
using EscherTiler.Dependencies;
using EscherTiler.Graphics;
using EscherTiler.Graphics.DirectX;
using EscherTiler.Graphics.Resources;
using EscherTiler.Styles;
using JetBrains.Annotations;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.DXGI;

namespace EscherTiler
{
    public partial class Main
    {
        [CanBeNull]
        private IResourceManager _miskResourceManager;

        [CanBeNull]
        private IGraphics _directXGraphics;

        [NotNull]
        private readonly SolidColourStyle _whiteStyle = new SolidColourStyle(Colour.White);

        [NotNull]
        private readonly SolidColourStyle _grayStyle = new SolidColourStyle(Colour.Gray);

        [NotNull]
        private readonly SolidColourStyle _blackStyle = new SolidColourStyle(Colour.Black);

        private void InitializeGraphics()
        {
            DependencyManger.ForTypeUse(() => _renderControl.RenderTargetContainer, DependencyCacheFlags.CacheGlobal);

            _miskResourceManager = DependencyManger.GetResourceManager();
            _miskResourceManager.Add(_whiteStyle);
            _miskResourceManager.Add(_grayStyle);
            _miskResourceManager.Add(_blackStyle);

            RenderTargetContainer renderTargetContainer = _renderControl.RenderTargetContainer;
            DirectXGraphics graphics = new DirectXGraphics(
                renderTargetContainer.RenderTarget,
                _miskResourceManager,
                _whiteStyle,
                _blackStyle,
                1f);
            renderTargetContainer.RenderTargetChanged += rt => graphics.RenderTarget = rt;

            _directXGraphics = graphics;
        }

        private void UnloadGraphics()
        {
            Interlocked.Exchange(ref _directXGraphics, null)?.Dispose();
            Interlocked.Exchange(ref _miskResourceManager, null)?.Dispose();
            Interlocked.Exchange(ref _controller, null)?.Dispose();
        }

        private void renderControl_Render([NotNull] RenderTarget renderTarget, [NotNull] SwapChain swapChain)
        {
            IGraphics graphics = _directXGraphics;
            Controller controller = _controller;
            if (graphics == null || controller == null) throw new ObjectDisposedException(nameof(Main));

            renderTarget.BeginDraw();
            renderTarget.Transform = ViewMatrix.ToRawMatrix3x2();
            renderTarget.Clear(Color.White);
            /*
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

                IStyle s = new RandomColourStyle(
                    Colour.Red,
                    Colour.Blue,
                    new Vector2(_mouseLocation.X, _mouseLocation.Y));

                using (_directXGraphics.TempState(s))
                    _directXGraphics.FillRectangle(new Numerics.Rectangle(-55, 0, 50, 50));

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

                    LinearGradientStyle style = new LinearGradientStyle(
                        selectedVertex.Location,
                        selectedVertex.In.Start.Location,
                        gradientStops);

                    using (_directXGraphics.TempState(lineStyle: style))
                        _directXGraphics.DrawLine(selectedVertex.Location, selectedVertex.In.Start.Location);

                    style = new LinearGradientStyle(
                        selectedVertex.Location,
                        selectedVertex.Out.End.Location,
                        gradientStops);

                    using (_directXGraphics.TempState(lineStyle: style))
                        _directXGraphics.DrawLine(selectedVertex.Location, selectedVertex.Out.End.Location);
                }
                else if ((selectedEdge = _selected as Edge) != null)
                {
                    using (_directXGraphics.TempState(lineStyle: _grayStyle))
                        _directXGraphics.DrawLine(selectedEdge.Start.Location, selectedEdge.End.Location);
                }
            }
            //*/

            lock (_drawLock)
                controller.Draw(graphics);

            renderTarget.EndDraw();
            swapChain.Present(0, PresentFlags.None);
        }
    }
}