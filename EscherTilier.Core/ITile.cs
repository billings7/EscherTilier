using System.Numerics;
using EscherTilier.Styles;

namespace EscherTilier
{
    public interface ITile
    {
        string Label { get; }

        IStyle Style { get; }

        Matrix3x2 Transform { get; }

        Shape Shape { get; }
    }
}