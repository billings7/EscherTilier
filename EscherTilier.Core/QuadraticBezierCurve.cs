using System.Numerics;
using EscherTilier.Graphics;
using EscherTilier.Numerics;

namespace EscherTilier
{
    /// <summary>
    /// Defines a quadratic bezier curve.
    /// </summary>
    public class QuadraticBezierCurve : ILine
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QuadraticBezierCurve"/> class.
        /// </summary>
        /// <param name="start">The start point.</param>
        /// <param name="controlPoint">The control point.</param>
        /// <param name="end">The end point.</param>
        public QuadraticBezierCurve(Vector2 start, Vector2 controlPoint, Vector2 end)
        {
            Start = start;
            ControlPoint = controlPoint;
            End = end;
        }

        /// <summary>
        /// Gets the start point of the line.
        /// </summary>
        /// <value>
        /// The start point.
        /// </value>
        public Vector2 Start { get; }

        /// <summary>
        /// Gets the ens point of the line.
        /// </summary>
        /// <value>
        /// The ens point.
        /// </value>
        public Vector2 End { get; }

        /// <summary>
        /// Gets the control point.
        /// </summary>
        /// <value>
        /// The control point.
        /// </value>
        public Vector2 ControlPoint { get; }

        /// <summary>
        /// Gets the approximate bounds for this line after it has been transformed by the given <paramref name="transform" />.
        /// The rectangle returned should equal or contain the actual bounds.
        /// </summary>
        /// <param name="transform">The transform to apply to the line.</param>
        /// <returns></returns>
        public Rectangle GetApproximateBounds(Matrix3x2 transform)
        {
            return new Rectangle(Vector2.Transform(Start, transform), Vector2.Zero)
                .Expand(Vector2.Transform(End, transform))
                .Expand(Vector2.Transform(ControlPoint, transform));
        }

        /// <summary>
        /// Adds the line to the given <paramref name="path" /> after transforming it by the given <paramref name="transform" />.
        /// </summary>
        /// <param name="path">The path to add the line to.</param>
        /// <param name="transform">The transform.</param>
        public void AddToPath(IGraphicsPath path, Matrix3x2 transform)
        {
            path.AddQuadraticBezier(
                Vector2.Transform(ControlPoint, transform),
                Vector2.Transform(End, transform));
        }

        /// <summary>
        /// Draws the line to the given <paramref name="graphics" /> after transforming it by the given <paramref name="transform" />.
        /// </summary>
        /// <param name="graphics">The graphics to draw to.</param>
        /// <param name="transform">The transform.</param>
        public void Draw(IGraphics graphics, Matrix3x2 transform)
        {
            graphics.DrawQuadraticBezier(
                Vector2.Transform(Start, transform),
                Vector2.Transform(ControlPoint, transform),
                Vector2.Transform(End, transform));
        }
    }
}