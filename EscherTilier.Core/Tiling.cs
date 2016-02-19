using System;
using System.Collections.Generic;
using EscherTilier.Numerics;
using EscherTilier.Styles;

namespace EscherTilier
{
    public class Tiling
    {
        public Template Template { get; }

        public TilingDefinition Definition { get; }

        public IReadOnlyList<Tile> Tiles { get; }

        public StyleManager Styles { get; }

        public IEnumerable<ITile> GetTiles(
            Rectangle bounds,
            StyleManager styleManager,
            IEnumerable<ITile> existingTiles)
        {
            throw new NotImplementedException();
        }
    }
}