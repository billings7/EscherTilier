using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using JetBrains.Annotations;

namespace EscherTilier
{
    /// <summary>
    ///     Defines the verticies and edges of a shape.
    /// </summary>
    public class Shape
    {
        [NotNull]
        private readonly IReadOnlyDictionary<string, Edge> _edgesByName;

        [NotNull]
        private readonly IReadOnlyDictionary<string, Vertex> _verticesByName;

        /// <summary>
        ///     Initializes a new instance of the <see cref="Shape" /> class.
        /// </summary>
        /// <param name="template">The template to create the shape from.</param>
        /// <exception cref="System.ArgumentNullException"><paramref name="template" /> was <see langword="null" /></exception>
        public Shape([NotNull] ShapeTemplate template)
        {
            if (template == null) throw new ArgumentNullException(nameof(template));

            Template = template;

            Dictionary<string, Edge> edgesByName = new Dictionary<string, Edge>(StringComparer.InvariantCulture);
            Dictionary<string, Vertex> verticesByName = new Dictionary<string, Vertex>(StringComparer.InvariantCulture);

            Edge[] edges = new Edge[template.EdgeNames.Count];
            Vertex[] vertices = new Vertex[template.VertexNames.Count];

            Edge firstEdge = null;
            Vertex lastVertex = null;

            for (int i = 0; i < template.EdgeNames.Count; i++)
            {
                string edgeName = template.EdgeNames[i];
                string vertName = template.VertexNames[i];
                Vector2 vertLoc = template.InitialVertices[i];

                Debug.Assert(edgeName != null, "edgeName != null");
                Debug.Assert(vertName != null, "vertName != null");

                Edge edge = new Edge(edgeName, this);
                Vertex vert = new Vertex(vertName, this, vertLoc);

                edge.End = vert;
                vert.In = edge;

                if (lastVertex == null)
                    firstEdge = edge;
                else
                {
                    edge.Start = lastVertex;
                    lastVertex.Out = edge;
                }

                lastVertex = vert;

                edgesByName.Add(edgeName, edge);
                verticesByName.Add(vertName, vert);

                edges[i] = edge;
                vertices[i] = vert;
            }
            Debug.Assert(firstEdge != null, "firstEdge != null");

            firstEdge.Start = lastVertex;
            lastVertex.Out = firstEdge;

            _edgesByName = edgesByName;
            _verticesByName = verticesByName;

            Vertices = vertices;
            Edges = edges;
        }

        /// <summary>
        ///     Gets the template this shape was created from.
        /// </summary>
        /// <value>
        ///     The template.
        /// </value>
        [NotNull]
        public ShapeTemplate Template { get; }

        /// <summary>
        ///     Gets the vertices in this shape.
        /// </summary>
        /// <value>
        ///     The vertices.
        /// </value>
        [NotNull]
        [ItemNotNull]
        public IReadOnlyList<Vertex> Vertices { get; }

        /// <summary>
        ///     Gets the edges of this shape.
        /// </summary>
        /// <value>
        ///     The edges.
        /// </value>
        [NotNull]
        [ItemNotNull]
        public IReadOnlyList<Edge> Edges { get; }

        /// <summary>
        ///     Gets the edge with the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="name" /> was <see langword="null" />.</exception>
        [CanBeNull]
        public Edge GetEdge([NotNull] string name) => _edgesByName[name];

        /// <summary>
        ///     Gets the vertex with the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="name" /> was <see langword="null" />.</exception>
        [CanBeNull]
        public Vertex GetVertex([NotNull] string name) => _verticesByName[name];
    }
}