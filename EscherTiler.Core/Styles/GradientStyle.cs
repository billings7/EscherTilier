using System;
using System.Collections.Generic;
using System.Numerics;
using JetBrains.Annotations;

namespace EscherTiler.Styles
{
    /// <summary>
    ///     Base class for styles which have a gradient.
    /// </summary>
    public abstract class GradientStyle : IStyle
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="GradientStyle" /> class.
        /// </summary>
        /// <param name="gradientStops">The gradient stops.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        protected GradientStyle([NotNull] IReadOnlyList<GradientStop> gradientStops)
        {
            if (gradientStops == null) throw new ArgumentNullException(nameof(gradientStops));
            GradientStops = gradientStops;
        }

        /// <summary>
        ///     Gets the gradient stops for this style.
        /// </summary>
        /// <value>
        ///     The gradient stops.
        /// </value>
        [NotNull]
        public IReadOnlyList<GradientStop> GradientStops { get; }

        /// <summary>
        ///     Returns a copy of this style with the given transform applied.
        /// </summary>
        /// <param name="matrix">The transform matrix.</param>
        /// <returns>The transformed style.</returns>
        public abstract IStyle Transform(Matrix3x2 matrix);
    }
}