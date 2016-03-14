using System.Numerics;

namespace EscherTilier
{
    public class Line : ILine
    {
        public Line(Vector2 start, Vector2 end)
        {
            Start = start;
            End = end;
        }

        public Vector2 Start { get; }

        public Vector2 End { get; }
    }
}