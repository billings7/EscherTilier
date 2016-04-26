using System;
using System.Numerics;
using EscherTilier.Numerics;

namespace EscherTilier
{
    /// <summary>
    ///     Interface that defines basic information about a view.
    /// </summary>
    public interface IView
    {
        /// <summary>
        ///     Gets the view bounds.
        /// </summary>
        /// <value>
        ///     The view bounds.
        /// </value>
        Rectangle ViewBounds { get; }

        /// <summary>
        ///     Gets the view matrix.
        /// </summary>
        /// <value>
        ///     The view matrix.
        /// </value>
        Matrix3x2 ViewMatrix { get; }

        /// <summary>
        ///     Gets the inverse view matrix.
        /// </summary>
        /// <value>
        ///     The inverse view matrix.
        /// </value>
        Matrix3x2 InverseViewMatrix { get; }

        /// <summary>
        ///     Occurs when the value of the <see cref="ViewBounds" /> property changes.
        /// </summary>
        event EventHandler ViewBoundsChanged;
    }
}