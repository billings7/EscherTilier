using System;
using System.Diagnostics;
using System.Threading;
using JetBrains.Annotations;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using SharpDX.Windows;
using AlphaMode = SharpDX.Direct2D1.AlphaMode;
using Device = SharpDX.Direct3D11.Device;
using FactoryD2D = SharpDX.Direct2D1.Factory;
using FactoryDXGI = SharpDX.DXGI.Factory1;

namespace EscherTilier
{
    /// <summary>
    ///     A control for rendering with SharpDX.
    /// </summary>
    public sealed class RenderControl : SharpDX.Windows.RenderControl
    {
        [NotNull]
        private readonly Device _device;

        [NotNull]
        private readonly SwapChain _swapChain;

        [NotNull]
        private RenderTarget _renderTarget;

        [NotNull]
        private Surface _backBuffer;

        /// <summary>
        ///     Initializes a new instance of the <see cref="RenderControl" /> class.
        /// </summary>
        public RenderControl()
        {
            SwapChainDescription swapCHainDesc = new SwapChainDescription
            {
                BufferCount = 2,
                Usage = Usage.RenderTargetOutput,
                OutputHandle = Handle,
                IsWindowed = true,
                ModeDescription =
                    new ModeDescription(
                        Width,
                        Height,
                        new Rational(60, 1),
                        Format.R8G8B8A8_UNorm),
                SampleDescription = new SampleDescription(1, 0),
                Flags = SwapChainFlags.AllowModeSwitch,
                SwapEffect = SwapEffect.Discard
            };

            Device.CreateWithSwapChain(
                DriverType.Hardware,
                DeviceCreationFlags.BgraSupport,
                swapCHainDesc,
                out _device,
                out _swapChain);

            Debug.Assert(_swapChain != null, "_swapChain != null");

            // ReSharper disable once AssignNullToNotNullAttribute
            _backBuffer = Surface.FromSwapChain(_swapChain, 0);
            Debug.Assert(_backBuffer != null, "_backBuffer != null");

            using (FactoryD2D factory = new FactoryD2D())
            {
                Size2F dpi = factory.DesktopDpi;

                _renderTarget = new RenderTarget(
                    factory,
                    _backBuffer,
                    new RenderTargetProperties
                    {
                        DpiX = dpi.Width,
                        DpiY = dpi.Height,
                        MinLevel = SharpDX.Direct2D1.FeatureLevel.Level_DEFAULT,
                        PixelFormat = new PixelFormat(Format.Unknown, AlphaMode.Ignore),
                        Type = RenderTargetType.Default,
                        Usage = RenderTargetUsage.None
                    });
            }
            using (FactoryDXGI factory = _swapChain.GetParent<FactoryDXGI>())
            {
                Debug.Assert(factory != null, "factory != null");
                factory.MakeWindowAssociation(Handle, WindowAssociationFlags.IgnoreAltEnter);
            }
        }

        /// <summary>
        ///     Gets the render target.
        /// </summary>
        /// <value>
        ///     The render target.
        /// </value>
        [NotNull]
        public RenderTarget RenderTarget => _renderTarget;

        /// <summary>
        ///     Gets the swap chain.
        /// </summary>
        /// <value>
        ///     The swap chain.
        /// </value>
        [NotNull]
        public SwapChain SwapChain => _swapChain;

        /// <summary>
        ///     Gets the device.
        /// </summary>
        /// <value>
        ///     The device.
        /// </value>
        [NotNull]
        public Device Device => _device;

        /// <summary>
        ///     Occurs when the control needs to render.
        /// </summary>
        public event RenderDelegate Render;

        /// <summary>
        ///     Gets a value indicating whether the control needs rendering.
        /// </summary>
        /// <value>
        ///     <see langword="true" /> if the control needs rendering; otherwise, <see langword="false" />.
        /// </value>
        public bool NeedsRender { get; private set; }

        /// <summary>
        ///     Raises the <see cref="E:System.Windows.Forms.Control.SizeChanged" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);

            Interlocked.Exchange(ref _renderTarget, null)?.Dispose();
            Interlocked.Exchange(ref _backBuffer, null)?.Dispose();

            _swapChain.ResizeBuffers(
                2,
                Width,
                Height,
                Format.R8G8B8A8_UNorm,
                SwapChainFlags.AllowModeSwitch);

            // ReSharper disable once AssignNullToNotNullAttribute
            _backBuffer = Surface.FromSwapChain(_swapChain, 0);
            Debug.Assert(_backBuffer != null, "_backBuffer != null");

            using (FactoryD2D factory = new FactoryD2D())
            {
                Size2F dpi = factory.DesktopDpi;

                _renderTarget = new RenderTarget(
                    factory,
                    _backBuffer,
                    new RenderTargetProperties
                    {
                        DpiX = dpi.Width,
                        DpiY = dpi.Height,
                        MinLevel = SharpDX.Direct2D1.FeatureLevel.Level_DEFAULT,
                        PixelFormat = new PixelFormat(Format.Unknown, AlphaMode.Ignore),
                        Type = RenderTargetType.Default,
                        Usage = RenderTargetUsage.None
                    });
            }

            NeedsRender = true;
        }

        /// <summary>
        ///     Forces the control to invalidate its client area and immediately redraw itself and any child controls.
        /// </summary>
        public override void Refresh() => NeedsRender = true;

        /// <summary>
        ///     Called to render the control.
        /// </summary>
        private void OnRender()
        {
            NeedsRender = false;
            Render?.Invoke(_renderTarget, _swapChain);
        }

        /// <summary>
        ///     Runs the render loop.
        /// </summary>
        public void RenderLoop()
        {
            using (RenderLoop renderLoop = new RenderLoop(this)
            {
                UseApplicationDoEvents = false
            })
            {
                while (renderLoop.NextFrame())
                {
                    if (NeedsRender)
                        OnRender();
                    Thread.Yield();
                }
            }
        }
    }
}