using System;
using System.Collections.Generic;
using System.Numerics;
using EscherTilier.Graphics;
using EscherTilier.Styles;

namespace EscherTilier
{
    public class Tile : ITile
    {
        public Tile(string label, Shape shape, Matrix3x2 transform, IReadOnlyList<EdgePartShape> partShapes)
        {
            Shape = shape;
            Transform = transform;
            PartShapes = partShapes;
            Label = label;
        }

        public string Label { get; }

        public Shape Shape { get; }

        public IStyle Style { get; set; }

        public Matrix3x2 Transform { get; }

        public IReadOnlyList<EdgePartShape> PartShapes { get; }

        public void PopulateGraphicsPath(IGraphicsPath path)
        {
            throw new NotImplementedException();
        }
    }
}