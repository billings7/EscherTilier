using System.Numerics;

namespace EscherTilier.Numerics
{
    /// <summary>
    ///     Defines a rectangle.
    /// </summary>
    public struct Rectangle
    {
        /// <summary>
        ///     Gets the location of the rectangle.
        /// </summary>
        /// <value>
        ///     The location.
        /// </value>
        public readonly Vector2 Location;

        /// <summary>
        ///     Gets the size of the rectangle.
        /// </summary>
        /// <value>
        ///     The size.
        /// </value>
        public readonly Vector2 Size;

        /// <summary>
        ///     Initializes a new instance of the <see cref="Rectangle" /> struct.
        /// </summary>
        /// <param name="location">The location.</param>
        /// <param name="size">The size.</param>
        public Rectangle(Vector2 location, Vector2 size)
        {
            Location = location;
            Size = size;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Rectangle"/> struct.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public Rectangle(float x, float y, float width, float height)
        {
            Location = new Vector2(x, y);
            Size = new Vector2(width, height);
        }

        /// <summary>
        ///     Gets the position of the left side of the rectangle.
        /// </summary>
        public float Left => Location.X;

        /// <summary>
        ///     Gets the position of the top side of the rectangle.
        /// </summary>
        public float Top => Location.Y + Size.Y;

        /// <summary>
        ///     Gets the position of the right side of the rectangle.
        /// </summary>
        public float Right => Location.X + Size.X;

        /// <summary>
        ///     Gets the position of the bottom side of the rectangle.
        /// </summary>
        public float Bottom => Location.Y;
    }
}