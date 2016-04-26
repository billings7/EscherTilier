using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using EscherTilier.Utilities;
using JetBrains.Annotations;

namespace EscherTilier
{
    /// <summary>
    ///     Defines the template for an individual shape within a <see cref="Template" />.
    /// </summary>
    public class ShapeTemplate
    {
        private Template _template;

        [NotNull]
        internal Template Template
        {
            get
            {
                Debug.Assert(_template != null, "_template != null");
                return _template;
            }
            set
            {
                Debug.Assert(
                    _template == null || _template == value,
                    "The shape template is already used by another template.");
                _template = value;
            }
        }

        private IReadOnlyDictionary<TilingDefinition, IReadOnlyList<EdgePattern>> _edgePatterns;

        [NotNull]
        internal IReadOnlyDictionary<TilingDefinition, IReadOnlyList<EdgePattern>> EdgePatterns
        {
            get
            {
                Debug.Assert(_edgePatterns != null, "_edgePatterns != null");
                return _edgePatterns;
            }
            set
            {
                Debug.Assert(
                    _edgePatterns == null || _edgePatterns == value,
                    "The shape template is already used by a set of edge patterns.");
                _edgePatterns = value;
            }
        }

        private IReadOnlyDictionary<TilingDefinition, IReadOnlyList<EdgePart>> _edgeParts;

        [NotNull]
        internal IReadOnlyDictionary<TilingDefinition, IReadOnlyList<EdgePart>> EdgeParts
        {
            get
            {
                Debug.Assert(_edgeParts != null, "_edgeParts != null");
                return _edgeParts;
            }
            set
            {
                Debug.Assert(
                    _edgeParts == null || _edgeParts == value,
                    "The shape template is already used by a set of edge parts.");
                _edgeParts = value;
            }
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ShapeTemplate" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="edgeNames">The edge names.</param>
        /// <param name="vertexNames">The vertex names.</param>
        /// <param name="initialVertices">The initial vertex locations.</param>
        /// TODO Exceptions
        public ShapeTemplate(
            string name,
            [NotNull] IReadOnlyList<string> edgeNames,
            [NotNull] IReadOnlyList<string> vertexNames,
            [NotNull] IReadOnlyList<Vector2> initialVertices)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name), Strings.ShapeTemplate_ShapeTemplate_NameNullOrWhitespace);
            if (edgeNames == null) throw new ArgumentNullException(nameof(edgeNames));
            if (vertexNames == null) throw new ArgumentNullException(nameof(vertexNames));
            if (initialVertices == null) throw new ArgumentNullException(nameof(initialVertices));
            if (edgeNames.Count < 3 || vertexNames.Count < 3 || initialVertices.Count < 3)
                throw new ArgumentException(Strings.ShapeTemplate_ShapeTemplate_NotEnoughEdgesOrVerts);
            if (edgeNames.Count != vertexNames.Count || vertexNames.Count != initialVertices.Count)
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
            InitialVertices = initialVertices;
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

        /// <summary>
        ///     Gets the initial locaiton of each of the verticies.
        /// </summary>
        /// <value>
        ///     The initial vertices.
        /// </value>
        [NotNull]
        public IReadOnlyList<Vector2> InitialVertices { get; }
    }
}