using System;
using System.Collections.Generic;
using EscherTilier.Expressions;

namespace EscherTilier
{
    public class Template
    {
        public IReadOnlyList<ShapeTemplate> Shapes { get; }

        public IReadOnlyList<IExpression<bool>> ShapeConstraints { get; }

        public IReadOnlyList<TilingDefinition> Tilings { get; }

        public IEnumerable<Shape> CreateShape()
        {
            throw new NotImplementedException();
        }

        public Tiling CreateTiling(IEnumerable<Shape> shapes)
        {
            throw new NotImplementedException();
        }
    }
}