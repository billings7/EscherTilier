using System.Numerics;

namespace EscherTilier
{
    /// <summary>
    ///     Defines a point on a line.
    /// </summary>
    public class LinePoint
    {
        /// <summary>
        ///     The position of the point.
        /// </summary>
        public readonly Vector2 Position;

        /// <summary>
        ///     The distance along the line that the point lies, in the range 0 - 1.
        /// </summary>
        public readonly float Distance;

        /// <summary>
        ///     Initializes a new instance of the <see cref="LinePoint" /> class.
        /// </summary>
        /// <param name="pos">The position.</param>
        /// <param name="distance">The distance along the line that the point lies, in the range 0 - 1.</param>
        public LinePoint(Vector2 pos, float distance)
        {
            Position = pos;
            Distance = distance;
        }

        /// <summary>
        ///     Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        ///     A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString() => $"{Distance:P1} - {Position}";
    }
}