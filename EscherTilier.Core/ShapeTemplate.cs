using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace EscherTilier
{
    /// <summary>
    ///     Defines the template for an individual shape within a <see cref="Template" />.
    /// </summary>
    public class ShapeTemplate
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ShapeTemplate" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="edgeNames">The edge names.</param>
        /// <param name="vertexNames">The vertex names.</param>
        /// TODO Exceptions
        public ShapeTemplate(
            string name,
            [NotNull] IReadOnlyList<string> edgeNames,
            [NotNull] IReadOnlyList<string> vertexNames)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name), Strings.ShapeTemplate_ShapeTemplate_NameNullOrWhitespace);
            if (edgeNames == null) throw new ArgumentNullException(nameof(edgeNames));
            if (vertexNames == null) throw new ArgumentNullException(nameof(vertexNames));
            if (edgeNames.Count < 3 || vertexNames.Count < 3)
                throw new ArgumentException(Strings.ShapeTemplate_ShapeTemplate_NotEnoughEdgesOrVerts);
            if (edgeNames.Count != vertexNames.Count)
                throw new ArgumentException(Strings.ShapeTemplate_ShapeTemplate_EdgeVertexCountNotSame);
            if (edgeNames.Any(string.IsNullOrWhiteSpace))
                throw new ArgumentException(Strings.ShapeTemplate_ShapeTemplate_EdgeNamesNullOrWhitespace);
            if (vertexNames.Any(string.IsNullOrWhiteSpace))
                throw new ArgumentException(Strings.ShapeTemplate_ShapeTemplate_VertexNamesNullOrWhitespace);
            if (!edgeNames.Concat(vertexNames).AreDistinct(StringComparer.InvariantCulture))
                throw new ArgumentException(Strings.ShapeTemplate_ShapeTemplate_EdgeVertexNamesUniqe);

            Name = name;
            EdgeNames = edgeNames;
            VertexNames = vertexNames;
        }

        /// <summary>
        ///     Gets the name of this shape.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        [NotNull]
        public string Name { get; }

        /// <summary>
        ///     Gets the names of the edges in this shape, in order.
        /// </summary>
        /// <value>
        ///     The edge names.
        /// </value>
        [NotNull]
        [ItemNotNull]
        public IReadOnlyList<string> EdgeNames { get; }

        /// <summary>
        ///     Gets the names of the vertices in this shape, in order.
        /// </summary>
        /// <value>
        ///     The vertex names.
        /// </value>
        /// <remarks>The nth vertex joins the nth and n+1th edge.</remarks>
        [NotNull]
        [ItemNotNull]
        public IReadOnlyList<string> VertexNames { get; }
    }
}