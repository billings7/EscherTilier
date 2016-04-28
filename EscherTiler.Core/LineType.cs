namespace EscherTiler
{
    /// <summary>
    ///     Defines the possible types of an <see cref="ILine" />.
    /// </summary>
    public enum LineType : byte
    {
        /// <summary>
        ///     A straight line.
        /// </summary>
        Line,

        /// <summary>
        ///     A quadratic bezier curve.
        /// </summary>
        QuadraticBezierCurve,

        /// <summary>
        ///     A cubic bezier curve.
        /// </summary>
        CubicBezierCurve
    }
}