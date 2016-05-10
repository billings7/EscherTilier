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
    // This part is responsible for drawing to the UI.
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

        /// <summary>
        ///     Initializes the graphics.
        /// </summary>
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

        /// <summary>
        ///     Unloads the graphics.
        /// </summary>
        private void UnloadGraphics()
        {
            Interlocked.Exchange(ref _directXGraphics, null)?.Dispose();
            Interlocked.Exchange(ref _miskResourceManager, null)?.Dispose();
        }

        /// <summary>
        ///     Renders teh current controller to the renderControl.
        /// </summary>
        /// <param name="renderTarget">The render target.</param>
        /// <param name="swapChain">The swap chain.</param>
        /// <exception cref="System.ObjectDisposedException"></exception>
        private void renderControl_Render([NotNull] RenderTarget renderTarget, [NotNull] SwapChain swapChain)
        {
            IGraphics graphics = _directXGraphics;
            Controller controller = _activeController;
            if (graphics == null) throw new ObjectDisposedException(nameof(Main));

            renderTarget.BeginDraw();
            renderTarget.Transform = ViewMatrix.ToRawMatrix3x2();
            renderTarget.Clear(Color.White);

            lock (_lock)
                controller?.Draw(graphics);

            renderTarget.EndDraw();
            swapChain.Present(0, PresentFlags.None);
        }
    }
}