using System;
using System.Collections.Generic;
using EscherTilier.Numerics;
using EscherTilier.Styles;
using JetBrains.Annotations;

namespace EscherTilier
{
    public class Tiling
    {
        public Tiling(
            [NotNull] Template template,
            [NotNull] TilingDefinition definition,
            [NotNull] IReadOnlyList<Tile> tiles,
            [NotNull] StyleManager styleManager)
        {
            if (template == null) throw new ArgumentNullException(nameof(template));
            if (definition == null) throw new ArgumentNullException(nameof(definition));
            if (tiles == null) throw new ArgumentNullException(nameof(tiles));
            if (styleManager == null) throw new ArgumentNullException(nameof(styleManager));

            Template = template;
            Definition = definition;
            Tiles = tiles;
            StyleManager = styleManager;
        }

        public Template Template { get; }

        public TilingDefinition Definition { get; }

        public IReadOnlyList<Tile> Tiles { get; }

        public StyleManager StyleManager { get; set; }

        public IEnumerable<ITile> GetTiles(
            Rectangle bounds,
            StyleManager styleManager,
            IEnumerable<ITile> existingTiles)
        {
            throw new NotImplementedException();
        }
    }
}