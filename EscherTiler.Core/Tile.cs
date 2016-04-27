using System;
using System.Collections.Generic;
using System.Numerics;
using JetBrains.Annotations;

namespace EscherTiler
{
    /// <summary>
    ///     Defines the shape of a tile.
    /// </summary>
    public class Tile : TileBase
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Tile" /> class.
        /// </summary>
        /// <param name="label">The label.</param>
        /// <param name="shape">The shape.</param>
        /// <param name="transform">The transform.</param>
        /// <param name="partShapes">The part shapes.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public Tile(
            [NotNull] string label,
            [NotNull] Shape shape,
            Matrix3x2 transform,
            [NotNull] IReadOnlyList<EdgePartShape> partShapes)
            : base(label, shape, transform)
        {
            if (partShapes == null) throw new ArgumentNullException(nameof(partShapes));
            PartShapes = partShapes;
        }

        /// <summary>
        ///     Gets the shapes of the parts of this tiles edges.
        /// </summary>
        /// <value>
        ///     The part shapes.
        /// </value>
        public override IReadOnlyList<EdgePartShape> PartShapes { get; }
    }
}