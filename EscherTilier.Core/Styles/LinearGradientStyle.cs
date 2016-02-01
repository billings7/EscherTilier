using System;
using System.Numerics;

namespace EscherTilier.Styles
{
    public class LinearGradientStyle : GradientStyle
    {
        public Vector2 Start { get; }

        public Vector2 End { get; }

        public override IStyle Transform(Matrix3x2 matrix)
        {
            throw new NotImplementedException();
        }
    }
}