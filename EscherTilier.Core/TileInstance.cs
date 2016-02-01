using System.Numerics;
using EscherTilier.Styles;

namespace EscherTilier
{
    public class TileInstance : ITile
    {
        public string Label { get; }

        public Shape Shape { get; }

        public IStyle Style { get; }

        public Matrix3x2 Transform { get; }

        private readonly Tile _tile;
    }
}