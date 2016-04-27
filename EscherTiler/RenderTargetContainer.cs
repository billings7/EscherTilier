using System;
using System.Threading;
using EscherTiler.Utilities;
using JetBrains.Annotations;
using SharpDX.Direct2D1;

namespace EscherTiler
{
    public sealed class RenderTargetContainer : IDisposable
    {
        [CanBeNull]
        private RenderTarget _renderTarget;

        private RenderTargetContainer([NotNull] RenderTarget renderTarget)
        {
            _renderTarget = renderTarget;
        }

        public static RenderTargetContainer CreateContainer(
            [NotNull] RenderTarget renderTarget,
            out Reference<RenderTarget> setter)
        {
            if (renderTarget == null) throw new ArgumentNullException(nameof(renderTarget));
            RenderTargetContainer container = new RenderTargetContainer(renderTarget);
            setter = new Reference<RenderTarget>(() => container.RenderTarget, rt => container.RenderTarget = rt);
            return container;
        }

        public event Action<RenderTarget> RenderTargetChanged;

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

        private void OnRenderTargetChanged(RenderTarget obj) => RenderTargetChanged?.Invoke(obj);

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Interlocked.Exchange(ref _renderTarget, null)?.Dispose();
        }
    }
}