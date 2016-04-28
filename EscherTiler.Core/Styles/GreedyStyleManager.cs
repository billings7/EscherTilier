using EscherTiler.Graphics.Resources;
using JetBrains.Annotations;

namespace EscherTiler.Styles
{
    public class GreedyStyleManager : StyleManager
    {
        public GreedyStyleManager(
            int paramA,
            int paramB,
            int paramC)
        {
            ParamA = paramA;
            ParamB = paramB;
            ParamC = paramC;
        }

        public int ParamA { get; }

        public int ParamB { get; }

        public int ParamC { get; }

        protected override IStyle GetStyle(TileBase tile, IStyle[] styles)
        {
            throw new System.NotImplementedException();
        }
    }
}