using System.Collections.Concurrent;
using EscherTilier.Graphics;
using EscherTilier.Graphics.DirectX;
using EscherTilier.Styles;
using JetBrains.Annotations;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.DXGI;

namespace EscherTilier
{
    public partial class Main
    {
        private readonly ConcurrentDictionary<StyleManager, DirectXResourceManager> _resourceManagers =
            new ConcurrentDictionary<StyleManager, DirectXResourceManager>();

        [NotNull]
        private readonly DirectXResourceManager _miskResourceManager;

        [NotNull]
        private readonly SolidColourStyle _greyStyle = new SolidColourStyle(new Colour(0.5f, 0.5f, 0.5f));

        [NotNull]
        private readonly SolidColourStyle _blackStyle = new SolidColourStyle(new Colour(0, 0, 0));

        [NotNull]
        private DirectXGraphics _directXGraphics;

        partial void renderControl_Render([NotNull] RenderTarget renderTarget, [NotNull] SwapChain swapChain)
        {
            renderTarget.BeginDraw();
            renderTarget.Transform = ViewMatrix.ToMatrix3x2();
            renderTarget.Clear(Color.White);

            if (shape != null)
            {
                using (IGraphicsPath path = _directXGraphics.CreatePath())
                {
                    bool first = true;
                    foreach (Vertex vertex in shape.Vertices)
                    {
                        if (first) path.Start(vertex.Location);
                        else path.AddLine(vertex.Location);
                        first = false;
                    }

                    path.End();

                    _directXGraphics.DrawPath(path);
                }

                Vertex selectedVertex = _selected as Vertex;
                Edge selectedEdge;
                if (selectedVertex != null)
                    _directXGraphics.FillCircle(selectedVertex.Location, 3);
                else if ((selectedEdge = _selected as Edge) != null)
                {
                    LineStyle tmp = _directXGraphics.LineStyle;
                    _directXGraphics.LineStyle = new LineStyle(2, _greyStyle);
                    _directXGraphics.DrawLine(selectedEdge.Start.Location, selectedEdge.End.Location);
                    _directXGraphics.LineStyle = tmp;
                }
            }

            renderTarget.EndDraw();
            swapChain.Present(0, PresentFlags.None);
        }

        partial void renderControl_RenderTargetChanged([NotNull] RenderTarget renderTarget)
        {
            if (_miskResourceManager == null) return;

            _miskResourceManager.RenderTarget = renderTarget;
            foreach (DirectXResourceManager manager in _resourceManagers.Values)
                manager.RenderTarget = renderTarget;
            _directXGraphics = new DirectXGraphics(
                renderTarget,
                _miskResourceManager,
                _greyStyle,
                new LineStyle(2, _blackStyle));
        }
    }
}