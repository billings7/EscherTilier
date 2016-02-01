using System;
using System.Collections.Generic;
using System.Numerics;
using EscherTilier.Graphics;
using EscherTilier.Styles;

namespace EscherTilier
{
    public class Tile : ITile
    {
        public string Label { get; }

        public Shape Shape { get; }

        public IStyle Style { get; }

        public Matrix3x2 Transform { get; }

        public IReadOnlyList<EdgePartShape> PartShapes { get; }

        public void PopulateGraphicsPath(IGraphicsPath path)
        {
            throw new NotImplementedException();
        }
    }
}