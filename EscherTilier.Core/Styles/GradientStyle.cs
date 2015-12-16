using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Generic;
using System.Numerics;

namespace EscherTilier.Styles
{
    public abstract class GradientStyle : IStyle
    {
        public IReadOnlyList<GradientStop> GradientStops { get; }

        public abstract IStyle Transform(Matrix3x2 matrix);
    }
}