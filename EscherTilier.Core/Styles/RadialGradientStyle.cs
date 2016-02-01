using System;
using System.Numerics;

namespace EscherTilier.Styles
{
    public class RadialGradientStyle : GradientStyle
    {
        public Vector2 Center { get; }

        public Vector2 OriginOffset { get; }

        public Vector2 Radius { get; }

        public float Angle { get; }

        public override IStyle Transform(Matrix3x2 matrix)
        {
            throw new NotImplementedException();
        }
    }
}