using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using EscherTilier.Graphics.DirectX;
using JetBrains.Annotations;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
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
        private bool _running;

        [NotNull]
        private readonly object _lock = new object();

        [NotNull]
        private Device _device;

        [NotNull]
        private SwapChain _swapChain;

        [NotNull]
        private RenderTarget _renderTarget;

        [NotNull]
        private Surface _backBuffer;

        [NotNull]
        private readonly Thread _renderThread;

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

            Size2F dpi = DirectXResourceManager.FactoryD2D.DesktopDpi;

            _renderTarget = new RenderTarget(
                DirectXResourceManager.FactoryD2D,
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

            using (FactoryDXGI factory = _swapChain.GetParent<FactoryDXGI>())
            {
                Debug.Assert(factory != null, "factory != null");
                factory.MakeWindowAssociation(Handle, WindowAssociationFlags.IgnoreAltEnter);
            }

            _renderThread = new Thread(RenderLoop)
            {
                Name = "Render Thread",
                IsBackground = true
            };
        }

        /// <summary>
        ///     Gets the render target.
        /// </summary>
        /// <value>
        ///     The render target.
        /// </value>
        [NotNull]
        public RenderTarget RenderTarget
        {
            get
            {
                lock (_lock)
                {
                    return _renderTarget;
                }
            }
        }

        public event Action<RenderTarget> RenderTargetChanged;

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
        ///     Raises the <see cref="E:System.Windows.Forms.Control.Layout" /> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.LayoutEventArgs" /> that contains the event data. </param>
        protected override void OnLayout(LayoutEventArgs e)
        {
            base.OnLayout(e);

            if (!_running) return;

            lock (_lock)
            {
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

                Size2F dpi = DirectXResourceManager.FactoryD2D.DesktopDpi;

                _renderTarget = new RenderTarget(
                    DirectXResourceManager.FactoryD2D,
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

                NeedsRender = true;
                OnRenderTargetChanged(_renderTarget);
            }
            Thread.Yield();
        }

        private void OnRenderTargetChanged(RenderTarget obj)
        {
            RenderTargetChanged?.Invoke(obj);
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
            lock (_lock)
                Render?.Invoke(_renderTarget, _swapChain);
        }

        /// <summary>
        /// Starts the render loop.
        /// </summary>
        public void Start()
        {
            if (_running) return;
            lock (_lock)
            {
                if (_running) return;
                _running = true;
                _renderThread.Start();
            }
        }

        /// <summary>
        /// Stops the render loop.
        /// </summary>
        public void Stop()
        {
            lock (_lock) _running = false;
        }

        /// <summary>
        ///     Runs the render loop.
        /// </summary>
        public void RenderLoop()
        {
            while (_running)
            {
                OnRender();
                Thread.Sleep(1);
                //if (NeedsRender)
                //{
                //    OnRender();
                //    Thread.Yield();
                //}
                //else
                //    Thread.Sleep(1);
            }
        }

        /// <param name="disposing">
        ///     true to release both managed and unmanaged resources; false to release only unmanaged
        ///     resources.
        /// </param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                _running = true;
                Interlocked.Exchange(ref _renderTarget, null)?.Dispose();
                Interlocked.Exchange(ref _backBuffer, null)?.Dispose();
                Interlocked.Exchange(ref _swapChain, null)?.Dispose();
                Interlocked.Exchange(ref _device, null)?.Dispose();
            }
        }
    }
}