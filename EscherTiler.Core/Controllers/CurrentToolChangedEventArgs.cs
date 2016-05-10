using System;
using JetBrains.Annotations;

namespace EscherTiler.Controllers
{
    /// <summary>
    ///     Event arguments for the <see cref="Controller.CurrentToolChanged" /> event.
    /// </summary>
    public class CurrentToolChangedEventArgs : EventArgs
    {
        /// <summary>
        ///     The old tool.
        /// </summary>
        [CanBeNull]
        public readonly Tool OldTool;

        /// <summary>
        ///     The new tool.
        /// </summary>
        [CanBeNull]
        public readonly Tool NewTool;

        /// <summary>
        ///     Initializes a new instance of the <see cref="CurrentToolChangedEventArgs" /> class.
        /// </summary>
        /// <param name="oldTool">The old tool.</param>
        /// <param name="newTool">The new tool.</param>
        public CurrentToolChangedEventArgs([CanBeNull] Tool oldTool, [CanBeNull] Tool newTool)
        {
            OldTool = oldTool;
            NewTool = newTool;
        }
    }
}