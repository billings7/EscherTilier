using System;
using System.Numerics;
using EscherTilier.Graphics.Resources;
using EscherTilier.Numerics;
using EscherTilier.Styles;
using JetBrains.Annotations;

namespace EscherTilier.Graphics
{
    /// <summary>
    ///     Interface to an object that can render graphics.
    /// </summary>
    public interface IGraphics : IDisposable
    {
        /// <summary>
        ///     Creates an <see cref="IGraphicsPath" /> that can be used to draw a path with this graphics object.
        /// </summary>
        /// <returns></returns>
        [NotNull]
        IGraphicsPath CreatePath();

        /// <summary>
        ///     Draws a path.
        /// </summary>
        /// <param name="path">The path to draw.</param>
        void DrawPath([NotNull] IGraphicsPath path);

        /// <summary>
        ///     Fills the inside of a path.
        /// </summary>
        /// <param name="path">The path to fill.</param>
        void FillPath([NotNull] IGraphicsPath path);

        /// <summary>
        ///     Draws a line between two points.
        /// </summary>
        /// <param name="from">The point to draw the line from.</param>
        /// <param name="to">The point to draw the line to.</param>
        void DrawLine(Vector2 from, Vector2 to);

        /// <summary>
        ///     Draws a set of lines joining the array of points given.
        /// </summary>
        /// <param name="points">The points to draw lines between.</param>
        void DrawLines([NotNull] Vector2[] points);

        /// <summary>
        ///     Draws an arc of an elipse.
        /// </summary>
        /// <param name="from">The start point of the arc.</param>
        /// <param name="to">The end point of the arc.</param>
        /// <param name="radius">The radius of the arc.</param>
        /// <param name="angle">The angle of the arc, in radians.</param>
        /// <param name="clockwise">If set to <see langword="true" /> the arc will be drawn clockwise.</param>
        /// <param name="isLarge">Specifies whether the given arc is larger than 180 degrees</param>
        void DrawArc(Vector2 @from, Vector2 to, Vector2 radius, float angle, bool clockwise, bool isLarge);

        /// <summary>
        ///     Draws a quadratic bezier curve to the end of the line.
        /// </summary>
        /// <param name="from">The start point of the curve.</param>
        /// <param name="control">The control point of the curve.</param>
        /// <param name="to">The end point of the curve.</param>
        /// <returns>This <see cref="IGraphicsPath" />.</returns>
        void DrawQuadraticBezier(Vector2 @from, Vector2 control, Vector2 to);

        /// <summary>
        ///     Draws a cubic bezier curve to the end of the line.
        /// </summary>
        /// <param name="from">The start point of the curve.</param>
        /// <param name="controlA">The first control point of the curve.</param>
        /// <param name="controlB">The second control point of the curve.</param>
        /// <param name="to">The end point of the curve.</param>
        /// <returns>This <see cref="IGraphicsPath" />.</returns>
        void DrawCubicBezier(Vector2 @from, Vector2 controlA, Vector2 controlB, Vector2 to);

        /// <summary>
        ///     Draws a circle.
        /// </summary>
        /// <param name="point">The center point of the circle.</param>
        /// <param name="radius">The radius of the circle.</param>
        void DrawCircle(Vector2 point, float radius);

        /// <summary>
        ///     Fills the inside of a circle.
        /// </summary>
        /// <param name="point">The center point of the circle.</param>
        /// <param name="radius">The radius of the circle.</param>
        void FillCircle(Vector2 point, float radius);

        /// <summary>
        ///     Draws an ellipse.
        /// </summary>
        /// <param name="point">The center point of the ellipse.</param>
        /// <param name="radius">The radius of the elipse in each axis.</param>
        void DrawEllipse(Vector2 point, Vector2 radius);

        /// <summary>
        ///     Fills the inside of an ellipse.
        /// </summary>
        /// <param name="point">The center point of the ellipse.</param>
        /// <param name="radius">The radius of the elipse in each axis.</param>
        void FillEllipse(Vector2 point, Vector2 radius);

        /// <summary>
        ///     Draws a rectangle.
        /// </summary>
        /// <param name="rect">The rectangle to draw.</param>
        void DrawRectangle(Rectangle rect);

        /// <summary>
        ///     Fills the inside of an ellipse.
        /// </summary>
        /// <param name="rect">The rectangle to fill.</param>
        void FillRectangle(Rectangle rect);

        /// <summary>
        ///     Gets or sets the transform used when drawing.
        /// </summary>
        /// <value>
        ///     The transform.
        /// </value>
        Matrix3x2 Transform { get; set; }

        /// <summary>
        ///     Gets or sets the resource manager for the graphics object.
        /// </summary>
        /// <value>
        ///     The resource manager.
        /// </value>
        [NotNull]
        IResourceManager ResourceManager { get; set; }

        /// <summary>
        ///     Gets or sets the style used to fill the inside of shapes.
        /// </summary>
        /// <value>
        ///     The fill style.
        /// </value>
        [NotNull]
        IStyle FillStyle { get; set; }

        /// <summary>
        ///     Gets or sets the style used to draw lines.
        /// </summary>
        /// <value>
        ///     The line style.
        /// </value>
        [NotNull]
        IStyle LineStyle { get; set; }

        /// <summary>
        ///     Gets or sets the width of the drawn line.
        /// </summary>
        /// <value>
        ///     The width of the line.
        /// </value>
        float LineWidth { get; set; }

        /// <summary>
        ///     Sets the line style.
        /// </summary>
        /// <param name="lineStyle">The line style.</param>
        void SetLineStyle([NotNull] LineStyle lineStyle);

        /// <summary>
        ///     Saves the current state of the graphics to a stack.
        /// </summary>
        void SaveState();

        /// <summary>
        ///     Restores the previously saved state from the stack.
        /// </summary>
        void RestoreState();
    }
}