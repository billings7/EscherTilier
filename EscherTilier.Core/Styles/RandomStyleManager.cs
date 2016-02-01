using System;

namespace EscherTilier.Styles
{
    public class RandomStyleManager : StyleManager
    {
        public int Seed { get; }

        public override IStyle GetStyle(ITile tile)
        {
            throw new NotImplementedException();
        }
    }
}