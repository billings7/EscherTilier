using System;
using EscherTilier.Graphics;
using EscherTilier.Numerics;

namespace EscherTilier.Controllers
{
    /// <summary>
    ///     Base class for controllers.
    /// </summary>
    /// <seealso cref="EscherTilier.Graphics.IDrawable" />
    /// <seealso cref="System.IDisposable" />
    public abstract class Controller : IDrawable, IDisposable
    {
        private Rectangle _screenBounds;

        protected Controller(Rectangle screenBounds)
        {
            ScreenBounds = screenBounds;
        }

        /// <summary>
        /// Occurs when the value of the <see cref="ScreenBounds"/> property changes.
        /// </summary>
        public event EventHandler ScreenBoundsChanged;

        /// <summary>
        /// Gets or sets the screen bounds.
        /// </summary>
        /// <value>
        /// The screen bounds.
        /// </value>
        public Rectangle ScreenBounds
        {
            get { return _screenBounds; }
            set
            {
                if (_screenBounds == value)
                    return;

                _screenBounds = value;
                OnScreenBoundsChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        ///     Draws this object to the <see cref="IGraphics" /> provided.
        /// </summary>
        /// <param name="graphics">The graphics object to use to draw this object.</param>
        public abstract void Draw(IGraphics graphics);

        /// <summary>
        /// Raises the <see cref="E:ScreenBoundsChanged" /> event.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected virtual void OnScreenBoundsChanged(EventArgs e)
        {
            ScreenBoundsChanged?.Invoke(this, e);
        }

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