using System;
using System.Numerics;
using JetBrains.Annotations;

namespace EscherTilier.Graphics
{
    /// <summary>
    ///     Interface to an object that represents a path that can be drawn with an instance of <see cref="IGraphics" />.
    /// </summary>
    public interface IGraphicsPath : IDisposable
    {
        /// <summary>
        ///     Starts the path at the point given.
        /// </summary>
        /// <param name="point">The point to start the path at.</param>
        /// <returns>This <see cref="IGraphicsPath" />.</returns>
        /// <remarks>Must be called before any other method is called.</remarks>
        [NotNull]
        IGraphicsPath Start(Vector2 point);

        /// <summary>
        ///     Ends the path, optionally closing it.
        /// </summary>
        /// <param name="close">if set to <see langword="true" /> the path will be closed; otherwise it will be left open.</param>
        /// <returns>This <see cref="IGraphicsPath" />.</returns>
        /// <remarks>No other methods should be called after calling this.</remarks>
        [NotNull]
        IGraphicsPath End(bool close = true);

        /// <summary>
        ///     Adds a line to the end of the path.
        /// </summary>
        /// <param name="to">The end point of the line.</param>
        /// <returns>This <see cref="IGraphicsPath" />.</returns>
        [NotNull]
        IGraphicsPath AddLine(Vector2 to);

        /// <summary>
        ///     Adds a series of lines to the end of the path.
        /// </summary>
        /// <param name="points">The points of the lines to add.</param>
        /// <returns>This <see cref="IGraphicsPath" />.</returns>
        [NotNull]
        IGraphicsPath AddLines(params Vector2[] points);

        /// <summary>
        ///     Adds a series of lines to the end of the path.
        /// </summary>
        /// <param name="points">The points of the lines to add.</param>
        /// <returns>This <see cref="IGraphicsPath" />.</returns>
        [NotNull]
        IGraphicsPath AddLines(ArraySegment<Vector2> points);

        /// <summary>
        ///     Adds an arc of an elipse to the end of the path.
        /// </summary>
        /// <param name="to">The end point of the arc.</param>
        /// <param name="radius">The radius of the arc.</param>
        /// <param name="angle">The sweep angle of the arc.</param>
        /// <param name="clockwise">if set to <see langword="true" /> the arc will be drawn clockwise.</param>
        /// <returns>This <see cref="IGraphicsPath" />.</returns>
        [NotNull]
        IGraphicsPath AddArc(Vector2 to, Vector2 radius, float angle, bool clockwise);

        /// <summary>
        ///     Adds a quadratic bezier curve to the end of the line.
        /// </summary>
        /// <param name="control">The control point of the curve.</param>
        /// <param name="to">The end point of the curve.</param>
        /// <returns>This <see cref="IGraphicsPath" />.</returns>
        [NotNull]
        IGraphicsPath AddQuadraticBezier(Vector2 control, Vector2 to);

        /// <summary>
        ///     Adds a cubic bezier curve to the end of the line.
        /// </summary>
        /// <param name="controlA">The first control point of the curve.</param>
        /// <param name="controlB">The second control point of the curve.</param>
        /// <param name="to">The end point of the curve.</param>
        /// <returns>This <see cref="IGraphicsPath" />.</returns>
        [NotNull]
        IGraphicsPath AddCubicBezier(Vector2 controlA, Vector2 controlB, Vector2 to);
    }
}