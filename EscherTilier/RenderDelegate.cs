using JetBrains.Annotations;
using SharpDX.Direct2D1;
using SharpDX.DXGI;

namespace EscherTilier
{
    /// <summary>
    ///     Delegate for rendering.
    /// </summary>
    /// <param name="renderTarget">The render target.</param>
    /// <param name="swapChain">The swap chain.</param>
    public delegate void RenderDelegate([NotNull] RenderTarget renderTarget, [NotNull] SwapChain swapChain);
}