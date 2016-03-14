using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EscherTilier.Utilities;
using JetBrains.Annotations;

namespace EscherTilier
{
    /// <summary>
    ///     Contains a set of <see cref="Shape" /> instances.
    /// </summary>
    public class ShapeSet : IReadOnlyCollection<Shape>
    {
        [NotNull]
        private readonly IReadOnlyCollection<Shape> _shapes;

        [NotNull]
        private readonly Dictionary<string, Edge> _edgesByName;

        [NotNull]
        private readonly Dictionary<string, Vertex> _verticesByName;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ShapeSet" /> class.
        /// </summary>
        /// <param name="shapes">The shapes.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public ShapeSet([NotNull] IReadOnlyCollection<Shape> shapes)
        {
            if (shapes == null) throw new ArgumentNullException(nameof(shapes));
            if (shapes.Any(s => s == null)) throw new ArgumentNullException(nameof(shapes));
            if (!shapes.Select(s => s.Template).AreDistinct())
                throw new ArgumentException(Strings.ShapeSet_ShapeSet_DuplicateShapeTemplate, nameof(shapes));
            _shapes = shapes;

            _edgesByName = new Dictionary<string, Edge>(StringComparer.InvariantCulture);
            _verticesByName = new Dictionary<string, Vertex>(StringComparer.InvariantCulture);

            foreach (Shape shape in shapes)
            {
                // ReSharper disable once PossibleNullReferenceException
                foreach (Edge edge in shape.Edges)
                    _edgesByName.Add(edge.Name, edge);
                foreach (Vertex vertex in shape.Vertices)
                    _verticesByName.Add(vertex.Name, vertex);
            }
        }

        /// <summary>
        ///     Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        ///     An enumerator that can be used to iterate through the collection.
        /// </returns>
        [ItemNotNull]
        public IEnumerator<Shape> GetEnumerator() => _shapes.GetEnumerator();

        /// <summary>
        ///     Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        ///     An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
        /// </returns>
        [ItemNotNull]
        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable) _shapes).GetEnumerator();

        /// <summary>
        ///     Gets the number of elements in the collection.
        /// </summary>
        /// <returns>
        ///     The number of elements in the collection.
        /// </returns>
        public int Count => _shapes.Count;

        /// <summary>
        ///     Gets the edges in the set.
        /// </summary>
        /// <value>
        ///     The edges.
        /// </value>
        public IReadOnlyCollection<Edge> Edges => _edgesByName.Values;

        /// <summary>
        ///     Gets the vertices in the set.
        /// </summary>
        /// <value>
        ///     The vertices.
        /// </value>
        public IReadOnlyCollection<Vertex> Vertices => _verticesByName.Values;

        /// <summary>
        ///     Gets the edge with the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="name" /> was <see langword="null" />.</exception>
        [NotNull]
        // ReSharper disable once AssignNullToNotNullAttribute
        public Edge GetEdge([NotNull] string name) => _edgesByName[name];

        /// <summary>
        ///     Gets the vertex with the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="name" /> was <see langword="null" />.</exception>
        [NotNull]
        // ReSharper disable once AssignNullToNotNullAttribute
        public Vertex GetVertex([NotNull] string name) => _verticesByName[name];
    }
}