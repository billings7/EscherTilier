using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace EscherTilier.Styles
{
    public class SolidColourStyle : IStyle
    {
        public Colour Colour { get; }

        public IStyle Transform(Matrix3x2 matrix) => this;
    }
}
