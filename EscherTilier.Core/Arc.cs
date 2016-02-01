using System.Numerics;

namespace EscherTilier
{
    public class Arc : ILine
    {
        public Vector2 End { get; }

        public Vector2 Start { get; }

        public Vector2 Radius { get; }

        public float Angle { get; }

        public bool Clockwise { get; }
    }
}