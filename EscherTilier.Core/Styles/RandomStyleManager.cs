using System;
using EscherTilier.Graphics;
using JetBrains.Annotations;

namespace EscherTilier.Styles
{
    public class RandomStyleManager : StyleManager
    {
        public int Seed { get; }

        protected override IStyle GetStyle(ITile tile, IStyle[] styles)
        {
            throw new NotImplementedException();
        }

        public RandomStyleManager(
            [CanBeNull] IResourceManager resourceManager,
            int seed)
            : base(resourceManager)
        {
            Seed = seed;
        }
    }
}