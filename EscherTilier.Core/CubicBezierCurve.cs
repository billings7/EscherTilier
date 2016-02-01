using System.Numerics;

namespace EscherTilier
{
    public class CubicBezierCurve : ILine
    {
        public Vector2 End { get; }

        public Vector2 ControlPointA { get; }

        public Vector2 ControlPointB { get; }

        public Vector2 Start { get; }
    }
}