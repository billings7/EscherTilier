using System;
using EscherTiler.Graphics.Resources;
using JetBrains.Annotations;

namespace EscherTiler.Styles
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