using System.Collections.Generic;
using System.Numerics;
using JetBrains.Annotations;

namespace EscherTiler.Styles
{
    /// <summary>
    ///     A style that draws a radial gradient.
    /// </summary>
    public class RadialGradientStyle : GradientStyle
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="RadialGradientStyle" /> class.
        /// </summary>
        /// <param name="center">The center of the gradient ellipse.</param>
        /// <param name="originOffset">The offset of the gradient origin relative to the center.</param>
        /// <param name="radius">The radius of the gradient ellipse.</param>
        /// <param name="angle">The angle of the gradient around the center.</param>
        /// <param name="gradientStops">The gradient stops.</param>
        public RadialGradientStyle(
            Vector2 center,
            Vector2 originOffset,
            Vector2 radius,
            float angle,
            [NotNull] IReadOnlyList<GradientStop> gradientStops)
            : base(gradientStops)
        {
            UnitOriginOffset = originOffset / radius;
            GradientTransform = Matrix3x2.CreateScale(radius)
                                * Matrix3x2.CreateRotation(angle)
                                * Matrix3x2.CreateTranslation(center);
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="RadialGradientStyle" /> class.
        /// </summary>
        /// <param name="unitOriginOffset">The unit origin offset.</param>
        /// <param name="transform">The transform.</param>
        /// <param name="gradientStops">The gradient stops.</param>
        private RadialGradientStyle(
            Vector2 unitOriginOffset,
            Matrix3x2 transform,
            [NotNull] IReadOnlyList<GradientStop> gradientStops)
            : base(gradientStops)
        {
            UnitOriginOffset = unitOriginOffset;
            GradientTransform = transform;
        }

        /// <summary>
        ///     Gets the type of the style.
        /// </summary>
        /// <value>
        ///     The type.
        /// </value>
        public override StyleType Type => StyleType.RadialGradient;

        /// <summary>
        ///     Gets the origin offset vector.
        /// </summary>
        /// <value>
        ///     The unit origin offset.
        /// </value>
        /// <remarks>
        ///     The values of this vector are based on the original origin offset and radius,
        ///     where a unit offset of 1 equals the radius.
        /// </remarks>
        public Vector2 UnitOriginOffset { get; }

        /// <summary>
        ///     Gets the gradient transform.
        /// </summary>
        /// <value>
        ///     The gradient transform.
        /// </value>
        public Matrix3x2 GradientTransform { get; }

        /// <summary>
        ///     Returns a copy of this style with the given transform applied.
        /// </summary>
        /// <param name="matrix">The transform matrix.</param>
        /// <returns>
        ///     The transformed style.
        /// </returns>
        public override IStyle Transform(Matrix3x2 matrix)
        {
            if (matrix.IsIdentity) return this;
            return new RadialGradientStyle(UnitOriginOffset, GradientTransform * matrix, GradientStops);
        }
    }
}