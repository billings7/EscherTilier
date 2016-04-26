using System;
using System.Collections.Generic;
using EscherTilier.Graphics;
using JetBrains.Annotations;

namespace EscherTilier.Controllers
{
    /// <summary>
    ///     Base class for controllers.
    /// </summary>
    /// <seealso cref="EscherTilier.Graphics.IDrawable" />
    /// <seealso cref="System.IDisposable" />
    public abstract class Controller : IDrawable, IDisposable
    {
        [NotNull]
        private IReadOnlyList<Tool> _tools;

        private Tool _currentTool;

        /// <summary>
        ///     Initializes a new instance of the <see cref="Controller" /> class.
        /// </summary>
        /// <param name="view">The view.</param>
        protected Controller([NotNull] IView view)
        {
            if (view == null) throw new ArgumentNullException(nameof(view));
            View = view;
            _tools = Array.Empty<Tool>();
        }

        /// <summary>
        ///     Gets the view that this controller uses.
        /// </summary>
        /// <value>
        ///     The view.
        /// </value>
        [NotNull]
        public IView View { get; }

        /// <summary>
        ///     Occurs when the <see cref="Tools" /> property changes.
        /// </summary>
        public event EventHandler ToolsChanged;

        /// <summary>
        ///     Gets the tools for this controller.
        /// </summary>
        /// <value>
        ///     The tools.
        /// </value>
        [NotNull]
        [ItemNotNull]
        public IReadOnlyList<Tool> Tools
        {
            get { return _tools; }
            protected set
            {
                if (ReferenceEquals(value, _tools)) return;
                _tools = value;

                ToolsChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public event EventHandler<CurrentToolChangedEventArgs> CurrentToolChanged;

        /// <summary>
        ///     Gets or sets the currently active tool.
        /// </summary>
        /// <value>
        ///     The current tool.
        /// </value>
        [CanBeNull]
        public Tool CurrentTool
        {
            get { return _currentTool; }
            set
            {
                if (value == _currentTool) return;

                Tool oldTool = _currentTool;

                _currentTool?.Deselected();
                _currentTool = value;
                _currentTool?.Selected();

                CurrentToolChanged?.Invoke(this, new CurrentToolChangedEventArgs(oldTool, value));
            }
        }

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

    public class CurrentToolChangedEventArgs : EventArgs
    {
        public readonly Tool OldTool;
        public readonly Tool NewTool;

        public CurrentToolChangedEventArgs(Tool oldTool, Tool newTool)
        {
            OldTool = oldTool;
            NewTool = newTool;
        }
    }
}