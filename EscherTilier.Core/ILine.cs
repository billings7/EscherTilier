using System.Numerics;

namespace EscherTilier
{
    public interface ILine
    {
        Vector2 Start { get; }

        Vector2 End { get; }
    }
}