using System.Numerics;

namespace EscherTilier.Styles
{
    public class RandomColourStyle : IStyle
    {
        public RandomColourStyle(Colour @from, Colour to)
        {
            From = @from;
            To = to;
        }

        public Colour From { get; }

        public Colour To { get; }

        public IStyle Transform(Matrix3x2 matrix) => this;
    }
}