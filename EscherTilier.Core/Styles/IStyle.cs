using System.Numerics;

namespace EscherTilier.Styles
{
    public interface IStyle
    {
        IStyle Transform(Matrix3x2 matrix);
    }
}