using System;
using EscherTiler.Graphics.Resources;
using JetBrains.Annotations;

namespace EscherTiler.Styles
{
    public class RandomGreedyStyleManager : RandomStyleManager
    {
        public RandomGreedyStyleManager(int seed) : base(seed) { }

        protected override IStyle GetStyle(TileBase tile, IStyle[] styles)
        {
            throw new NotImplementedException();
        }
    }
}