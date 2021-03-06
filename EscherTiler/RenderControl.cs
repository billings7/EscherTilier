﻿using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using EscherTiler.Graphics.DirectX;
using EscherTiler.Utilities;
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

namespace EscherTiler
{
    /// <summary>
    ///     A control for rendering with SharpDX.
    /// </summary>
    public sealed class RenderControl : SharpDX.Windows.RenderControl
    {
        private bool _running;

        [NotNull]
        private readonly object _lock = new object();

        [CanBeNull]
        private Device _device;

        [CanBeNull]
        private SwapChain _swapChain;

        [CanBeNull]
        private RenderTargetContainer _renderTargetContainer;

        [CanBeNull]
        private readonly Reference<RenderTarget> _renderTargetRef;

        [CanBeNull]
        private Surface _backBuffer;

        [CanBeNull]
        private Thread _renderThread;

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

            RenderTarget renderTarget = new RenderTarget(
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
            _renderTargetContainer = RenderTargetContainer.CreateContainer(renderTarget, out _renderTargetRef);

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
                    if (_renderTargetRef == null) throw new ObjectDisposedException(nameof(RenderControl));
                    Debug.Assert(_renderTargetRef.Value != null, "_renderTargetRef.Value != null");
                    return _renderTargetRef.Value;
                }
            }
        }

        /// <summary>
        ///     Gets the render target container.
        /// </summary>
        /// <value>
        ///     The render target container.
        /// </value>
        [NotNull]
        public RenderTargetContainer RenderTargetContainer
        {
            get
            {
                if (_renderTargetContainer == null) throw new ObjectDisposedException(nameof(RenderControl));
                return _renderTargetContainer;
            }
        }

        /// <summary>
        ///     Occurs when the render target changes.
        /// </summary>
        /// <exception cref="System.ObjectDisposedException">
        /// </exception>
        public event Action<RenderTarget> RenderTargetChanged
        {
            add
            {
                if (_renderTargetContainer == null) throw new ObjectDisposedException(nameof(RenderControl));
                _renderTargetContainer.RenderTargetChanged += value;
            }
            remove
            {
                if (_renderTargetContainer == null) throw new ObjectDisposedException(nameof(RenderControl));
                _renderTargetContainer.RenderTargetChanged -= value;
            }
        }

        /// <summary>
        ///     Gets the swap chain.
        /// </summary>
        /// <value>
        ///     The swap chain.
        /// </value>
        [NotNull]
        public SwapChain SwapChain
        {
            get
            {
                if (_swapChain == null) throw new ObjectDisposedException(nameof(RenderControl));
                return _swapChain;
            }
        }

        /// <summary>
        ///     Gets the device.
        /// </summary>
        /// <value>
        ///     The device.
        /// </value>
        [NotNull]
        public Device Device
        {
            get
            {
                if (_device == null) throw new ObjectDisposedException(nameof(RenderControl));
                return _device;
            }
        }

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
                Debug.Assert(_renderTargetRef != null, "_renderTargetRef != null");

                _renderTargetRef.Value = null;
                Interlocked.Exchange(ref _backBuffer, null)?.Dispose();

                Debug.Assert(_swapChain != null, "_swapChain != null");
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

                _renderTargetRef.Value = new RenderTarget(
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
            }
            Thread.Yield();
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
            {
                Debug.Assert(_renderTargetRef != null, "_renderTargetRef != null");
                Debug.Assert(_renderTargetRef.Value != null, "_renderTargetRef.Value != null");
                Debug.Assert(_swapChain != null, "_swapChain != null");

                Render?.Invoke(_renderTargetRef.Value, _swapChain);
            }
        }

        /// <summary>
        ///     Starts the render loop.
        /// </summary>
        public void Start()
        {
            if (_running) return;
            lock (_lock)
            {
                if (_running) return;
                if (_renderThread == null) throw new ObjectDisposedException(nameof(RenderControl));
                _running = true;
                _renderThread.Start();
            }
        }

        /// <summary>
        ///     Stops the render loop.
        /// </summary>
        public void Stop()
        {
            lock (_lock) _running = false;
        }

        /// <summary>
        ///     Runs the render loop.
        /// </summary>
        private void RenderLoop()
        {
            while (_running)
            {
                OnRender();
                Thread.Sleep(1);
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
                lock (_lock)
                {
                    _running = false;
                    _renderThread = null;
                    Interlocked.Exchange(ref _renderTargetContainer, null)?.Dispose();
                    Interlocked.Exchange(ref _backBuffer, null)?.Dispose();
                    Interlocked.Exchange(ref _swapChain, null)?.Dispose();
                    Interlocked.Exchange(ref _device, null)?.Dispose();
                }
            }
        }
    }
}