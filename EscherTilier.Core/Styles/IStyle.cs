using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace EscherTilier.Styles
{
    public interface IStyle
    {
        IStyle Transform(Matrix3x2 matrix);
    }
}
