namespace EscherTiler.Styles
{
    /// <summary>
    ///     Describes the location and color of a transition point in a gradient.
    /// </summary>
    public struct GradientStop
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="GradientStop" /> struct.
        /// </summary>
        /// <param name="colour">The colour.</param>
        /// <param name="position">The position.</param>
        public GradientStop(Colour colour, float position)
        {
            Colour = colour;
            Position = position;
        }

        /// <summary>
        ///     Gets the colour.
        /// </summary>
        /// <value>
        ///     The colour.
        /// </value>
        public Colour Colour { get; }

        /// <summary>
        ///     Gets the position.
        /// </summary>
        /// <value>
        ///     The position.
        /// </value>
        public float Position { get; }
    }
}