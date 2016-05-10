using System;
using System.Threading;
using EscherTiler.Utilities;
using JetBrains.Annotations;
using SharpDX.Direct2D1;

namespace EscherTiler
{
    /// <summary>
    ///     Container class for handling a <see cref="T:RenderTarget" />.
    /// </summary>
    public sealed class RenderTargetContainer : IDisposable
    {
        [CanBeNull]
        private RenderTarget _renderTarget;

        /// <summary>
        ///     Initializes a new instance of the <see cref="RenderTargetContainer" /> class.
        /// </summary>
        /// <param name="renderTarget">The render target.</param>
        private RenderTargetContainer([NotNull] RenderTarget renderTarget)
        {
            _renderTarget = renderTarget;
        }

        /// <summary>
        ///     Creates a container for the given render target.
        /// </summary>
        /// <param name="renderTarget">The render target.</param>
        /// <param name="setter">The setter.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        public static RenderTargetContainer CreateContainer(
            [NotNull] RenderTarget renderTarget,
            out Reference<RenderTarget> setter)
        {
            if (renderTarget == null) throw new ArgumentNullException(nameof(renderTarget));
            RenderTargetContainer container = new RenderTargetContainer(renderTarget);
            setter = new Reference<RenderTarget>(() => container.RenderTarget, rt => container.RenderTarget = rt);
            return container;
        }

        /// <summary>
        ///     Occurs when the render target changes.
        /// </summary>
        public event Action<RenderTarget> RenderTargetChanged;

        /// <summary>
        ///     Gets the render target.
        /// </summary>
        /// <value>
        ///     The render target.
        /// </value>
        /// <exception cref="System.ObjectDisposedException"></exception>
        [NotNull]
        public RenderTarget RenderTarget
        {
            get
            {
                if (_renderTarget == null) throw new ObjectDisposedException(nameof(RenderTargetContainer));
                return _renderTarget;
            }
            private set
            {
                _renderTarget?.Dispose();
                _renderTarget = value;
                if (_renderTarget != null)
                    OnRenderTargetChanged(value);
            }
        }

        /// <summary>
        ///     Raises the <see cref="RenderTargetChanged" /> event.
        /// </summary>
        /// <param name="obj">The object.</param>
        private void OnRenderTargetChanged(RenderTarget obj) => RenderTargetChanged?.Invoke(obj);

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose() => Interlocked.Exchange(ref _renderTarget, null)?.Dispose();
    }
}