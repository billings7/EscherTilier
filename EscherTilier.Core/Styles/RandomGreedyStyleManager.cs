using System;
using EscherTilier.Graphics.Resources;
using JetBrains.Annotations;

namespace EscherTilier.Styles
{
    public class RandomGreedyStyleManager : RandomStyleManager
    {
        protected override IStyle GetStyle(TileBase tile, IStyle[] styles)
        {
            throw new NotImplementedException();
        }

        public RandomGreedyStyleManager(
            [CanBeNull] IResourceManager resourceManager,
            int seed)
            : base(resourceManager, seed) { }
    }
}