using System.Diagnostics;
using EscherTiler.Graphics.Resources;
using JetBrains.Annotations;

namespace EscherTiler.Styles
{
    public class SimpleStyleManager : StyleManager
    {
        protected override IStyle GetStyle(TileBase tile, IStyle[] styles)
        {
            Debug.Assert(styles.Length == 1);
            return styles[0];
        }
    }
}