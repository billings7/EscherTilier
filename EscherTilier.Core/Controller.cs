using System;
using EscherTilier.Graphics;

namespace EscherTilier
{
    /// <summary>
    ///     Base class for controllers.
    /// </summary>
    /// <seealso cref="EscherTilier.Graphics.IDrawable" />
    /// <seealso cref="System.IDisposable" />
    public abstract class Controller : IDrawable, IDisposable
    {
        /// <summary>
        ///     Draws this object to the <see cref="IGraphics" /> provided.
        /// </summary>
        /// <param name="graphics">The graphics object to use to draw this object.</param>
        public abstract void Draw(IGraphics graphics);

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///     Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing">
        ///     <see langword="true" /> to release both managed and unmanaged resources;
        ///     <see langword="false" /> to release only unmanaged resources.
        /// </param>
        protected virtual void Dispose(bool disposing) { }
    }
}