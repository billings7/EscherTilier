using System;
using System.Collections.Generic;
using System.Numerics;
using EscherTilier.Styles;
using JetBrains.Annotations;

namespace EscherTilier
{
    public class TileInstance : TileBase
    {
        [NotNull]
        public readonly Tile Tile;

        /// <summary>
        /// Initializes a new instance of the <see cref="TileBase" /> class.
        /// </summary>
        /// <param name="tile">The base tile.</param>
        /// <param name="label">The tile label.</param>
        /// <param name="transform">The transform of this tile.</param>
        /// <exception cref="System.ArgumentNullException"><paramref name="label" /> or <paramref name="tile" /> was null.</exception>
        public TileInstance([NotNull] Tile tile, [NotNull] string label, Matrix3x2 transform)
            : base(label, tile.Shape, transform)
        {
            if (tile == null) throw new ArgumentNullException(nameof(tile));
            Tile = tile;
        }

        /// <summary>
        ///     Gets the shapes of the parts of this tiles edges..
        /// </summary>
        /// <value>
        ///     The part shapes.
        /// </value>
        public override IReadOnlyList<EdgePartShape> PartShapes => Tile.PartShapes;
    }
}