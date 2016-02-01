using System.Numerics;

namespace EscherTilier
{
    public class Edge
    {
        public Shape Shape { get; }

        public Vector2 Start { get; }

        public Vector2 End { get; }

        public float Length() => Vector2.Distance(Start, End);
    }
}