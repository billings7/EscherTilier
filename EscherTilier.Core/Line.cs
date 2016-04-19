using System.Numerics;
using EscherTilier.Graphics;
using EscherTilier.Numerics;

namespace EscherTilier
{
    /// <summary>
    /// Defines a straight line between two points.
    /// </summary>
    public class Line : ILine
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Line"/> class.
        /// </summary>
        /// <param name="start">The start point.</param>
        /// <param name="end">The end point.</param>
        public Line(Vector2 start, Vector2 end)
        {
            Start = start;
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
        /// Gets the approximate bounds for this line after it has been transformed by the given <paramref name="transform" />.
        /// The rectangle returned should equal or contain the actual bounds.
        /// </summary>
        /// <param name="transform">The transform to apply to the line.</param>
        /// <returns></returns>
        public Rectangle GetApproximateBounds(Matrix3x2 transform)
        {
            return new Rectangle(Vector2.Transform(Start, transform), Vector2.Zero)
                .Expand(Vector2.Transform(End, transform));
        }

        /// <summary>
        /// Adds the line to the given <paramref name="path" /> after transforming it by the given <paramref name="transform" />.
        /// </summary>
        /// <param name="path">The path to add the line to.</param>
        /// <param name="transform">The transform.</param>
        public void AddToPath(IGraphicsPath path, Matrix3x2 transform)
        {
            path.AddLine(Vector2.Transform(End, transform));
        }

        /// <summary>
        /// Draws the line to the given <paramref name="graphics" /> after transforming it by the given <paramref name="transform" />.
        /// </summary>
        /// <param name="graphics">The graphics to draw to.</param>
        /// <param name="transform">The transform.</param>
        public void Draw(IGraphics graphics, Matrix3x2 transform)
        {
            graphics.DrawLine(
                Vector2.Transform(Start, transform),
                Vector2.Transform(End, transform));
        }
    }
}