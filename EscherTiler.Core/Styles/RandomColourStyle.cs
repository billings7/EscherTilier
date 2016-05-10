using System;
using System.Numerics;
using EscherTiler.Utilities;

namespace EscherTiler.Styles
{
    /// <summary>
    ///     A style that draws with a random solid colour, interpolated between two fixed colours.
    /// </summary>
    public class RandomColourStyle : IStyle
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="RandomColourStyle" /> class.
        /// </summary>
        /// <param name="from">The first colour to interpolate from.</param>
        /// <param name="to">The second colour to interpolate to.</param>
        /// <param name="position">The position.</param>
        public RandomColourStyle(Colour @from, Colour to, Vector2 position = default(Vector2))
        {
            From = @from;
            To = to;
            Position = position;

            float x = (float) Math.Round(position.X, 3);
            float y = (float) Math.Round(position.Y, 3);

            float rand = TileRandom.Random(From.ToArgb() + to.ToArgb(), x, y);

            PositionColour = Colour.Interpolate(From, To, rand);
        }

        /// <summary>
        ///     Gets the type of the style.
        /// </summary>
        /// <value>
        ///     The type.
        /// </value>
        public StyleType Type => StyleType.RandomColour;

        /// <summary>
        ///     Gets the colour to interpolate from.
        /// </summary>
        /// <value>
        ///     The colour to interpolate from.
        /// </value>
        public Colour From { get; }

        /// <summary>
        ///     Gets the colour to interpolate to.
        /// </summary>
        /// <value>
        ///     The colour to interpolate to.
        /// </value>
        public Colour To { get; }

        /// <summary>
        ///     Gets the position of the colour used to determine the actual colour used.
        /// </summary>
        /// <value>
        ///     The position.
        /// </value>
        public Vector2 Position { get; }

        /// <summary>
        ///     Gets the random colour at the current position.
        /// </summary>
        /// <value>
        ///     The position colour.
        /// </value>
        public Colour PositionColour { get; }

        /// <summary>
        ///     Returns a copy of this style with the given transform applied.
        /// </summary>
        /// <param name="matrix">The transform matrix.</param>
        /// <returns>
        ///     The transformed style.
        /// </returns>
        public IStyle Transform(Matrix3x2 matrix)
        {
            if (matrix.IsIdentity) return this;
            Vector2 pos = Vector2.Transform(Position, matrix);

            // ReSharper disable CompareOfFloatsByEqualityOperator
            if (Math.Round(pos.X, 3) == Math.Round(Position.X, 3) &&
                Math.Round(pos.X, 3) == Math.Round(Position.X, 3))
                // ReSharper restore CompareOfFloatsByEqualityOperator
                return this;

            return new RandomColourStyle(From, To, pos);
        }
    }
}