using System.Collections.Generic;
using System.Numerics;
using JetBrains.Annotations;

namespace EscherTiler.Styles
{
    /// <summary>
    ///     A style that draws a linear gradient.
    /// </summary>
    public class LinearGradientStyle : GradientStyle
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="LinearGradientStyle" /> class.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <param name="gradientStops">The gradient stops.</param>
        public LinearGradientStyle(Vector2 start, Vector2 end, [NotNull] IReadOnlyList<GradientStop> gradientStops)
            : base(gradientStops)
        {
            Start = start;
            End = end;
        }

        /// <summary>
        ///     Gets the type of the style.
        /// </summary>
        /// <value>
        ///     The type.
        /// </value>
        public override StyleType Type => StyleType.LinearGradient;

        /// <summary>
        ///     Gets the start point of the gradient (position 0).
        /// </summary>
        /// <value>
        ///     The start.
        /// </value>
        public Vector2 Start { get; }

        /// <summary>
        ///     Gets the end point of the gradient (position 1).
        /// </summary>
        /// <value>
        ///     The end point.
        /// </value>
        public Vector2 End { get; }

        /// <summary>
        ///     Returns a copy of this style with the given transform applied.
        /// </summary>
        /// <param name="matrix">The transform matrix.</param>
        /// <returns>
        ///     The transformed style.
        /// </returns>
        public override IStyle Transform(Matrix3x2 matrix)
        {
            if (matrix == Matrix3x2.Identity) return this;

            Vector2 start = Vector2.Transform(Start, matrix);
            Vector2 end = Vector2.Transform(End, matrix);

            if (start == Start && end == End) return this;
            return new LinearGradientStyle(start, end, GradientStops);
        }
    }
}