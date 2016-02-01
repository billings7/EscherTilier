using System;
using System.Collections.Generic;

namespace EscherTilier.Styles
{
    public abstract class StyleManager
    {
        // TODO Use DI instead?
        public static StyleManager CreateManager()
        {
            throw new NotImplementedException();
        }

        public IReadOnlyList<TileStyle> Styles { get; }

        public abstract IStyle GetStyle(ITile tile);
    }
}