using System.Numerics;

namespace EscherTilier
{
    public class QuadraticBezierCurve : ILine
    {
        public Vector2 End { get; }

        public Vector2 ControlPoint { get; }

        public Vector2 Start { get; }
    }
}