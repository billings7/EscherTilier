using JetBrains.Annotations;
using SharpDX.Direct2D1;
using SharpDX.DXGI;

namespace EscherTiler
{
    /// <summary>
    ///     Delegate for rendering.
    /// </summary>
    /// <param name="renderTarget">The render target.</param>
    /// <param name="swapChain">The swap chain.</param>
    public delegate void RenderDelegate([NotNull] RenderTarget renderTarget, [NotNull] SwapChain swapChain);
}