using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using EscherTiler.Numerics;
using JetBrains.Annotations;

namespace EscherTiler
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

            IsClockwise =
                Edges.Sum(e => (e.End.Location.X - e.Start.Location.X) * (e.End.Location.Y + e.Start.Location.Y)) > 0;
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
        ///     Gets a value indicating whether the verticies in this shape are defined in a clockwise order.
        /// </summary>
        /// <value>
        ///     <see langword="true" /> if the shape is clockwise; otherwise, <see langword="false" />.
        /// </value>
        public bool IsClockwise { get; }

        /// <summary>
        ///     Gets the centroid point of the shape.
        /// </summary>
        /// <value>
        ///     The centroid point.
        /// </value>
        public Vector2 Centroid => Vertices.Aggregate(Vector2.Zero, (sum, vert) => sum + vert.Location)
            / Vertices.Count;

        /// <summary>
        ///     Gets the bounds of the shape.
        /// </summary>
        /// <value>
        ///     The shape bounds.
        /// </value>
        public Rectangle Bounds
        {
            get
            {
                Vector2 min = Vector2.Zero;
                Vector2 max = Vector2.Zero;

                bool first = true;
                foreach (Vector2 vertex in Vertices.Select(v => v.Location))
                {
                    if (first) min = max = vertex;
                    else
                    {
                        if (vertex.X < min.X) min.X = vertex.X;
                        else if (vertex.X > max.X) max.X = vertex.X;
                        if (vertex.Y < min.Y) min.Y = vertex.Y;
                        else if (vertex.Y > max.Y) max.Y = vertex.Y;
                    }

                    first = false;
                }

                return new Rectangle(min, max - min);
            }
        }

        /// <summary>
        ///     Gets the edge with the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="name" /> was <see langword="null" />.</exception>
        /// <exception cref="KeyNotFoundException">This shape does not contain an edge with the given name.</exception>
        [NotNull]
        // ReSharper disable once AssignNullToNotNullAttribute
        public Edge GetEdge([NotNull] string name) => _edgesByName[name];

        /// <summary>
        ///     Gets the vertex with the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="name" /> was <see langword="null" />.</exception>
        /// <exception cref="KeyNotFoundException">This shape does not contain a vertex with the given name.</exception>
        [NotNull]
        // ReSharper disable once AssignNullToNotNullAttribute
        public Vertex GetVertex([NotNull] string name) => _verticesByName[name];
    }
}