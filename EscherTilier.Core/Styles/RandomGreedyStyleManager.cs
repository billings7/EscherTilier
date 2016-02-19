using System;
using EscherTilier.Graphics;
using JetBrains.Annotations;

namespace EscherTilier.Styles
{
    public class RandomGreedyStyleManager : RandomStyleManager
    {
        protected override IStyle GetStyle(ITile tile, IStyle[] styles)
        {
            throw new NotImplementedException();
        }

        public RandomGreedyStyleManager(
            [CanBeNull] IResourceManager resourceManager,
            int seed)
            : base(resourceManager, seed) { }
    }
}