using System.Numerics;

namespace EscherTilier.Styles
{
    public class SolidColourStyle : IStyle
    {
        public Colour Colour { get; }

        public IStyle Transform(Matrix3x2 matrix) => this;
    }
}