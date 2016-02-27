using JetBrains.Annotations;

namespace EscherTilier.Graphics
{
    /// <summary>
    ///     Interface to an object that can be drawn.
    /// </summary>
    public interface IDrawable
    {
        /// <summary>
        ///     Draws this object to the <see cref="IGraphics" /> provided.
        /// </summary>
        /// <param name="graphics">The graphics object to use to draw this object.</param>
        void Draw([NotNull] IGraphics graphics);
    }
}