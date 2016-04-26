using EscherTilier.Graphics.Resources;
using JetBrains.Annotations;

namespace EscherTilier.Styles
{
    public class GreedyStyleManager : StyleManager
    {
        public int ParamA { get; }

        public int ParamB { get; }

        public int ParamC { get; }

        protected override IStyle GetStyle(TileBase tile, IStyle[] styles)
        {
            throw new System.NotImplementedException();
        }

        public GreedyStyleManager(
            [CanBeNull] IResourceManager resourceManager,
            int paramA,
            int paramB,
            int paramC)
            : base(resourceManager)
        {
            ParamA = paramA;
            ParamB = paramB;
            ParamC = paramC;
        }
    }
}