using System;
using System.Collections.Generic;
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

        public RadialGradientStyle(IReadOnlyList<GradientStop> gradientStops, Vector2 center, Vector2 originOffset, Vector2 radius, float angle)
            : base(gradientStops)
        {
            Center = center;
            OriginOffset = originOffset;
            Radius = radius;
            Angle = angle;
        }
    }
}