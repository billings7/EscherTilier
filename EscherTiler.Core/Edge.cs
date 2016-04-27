using System;
using System.Diagnostics;
using System.Numerics;
using JetBrains.Annotations;

namespace EscherTiler
{
    /// <summary>
    ///     Defines an edge of a <see cref="Shape" />.
    /// </summary>
    public class Edge
    {
        private Vertex _end;
        private Vertex _start;

        /// <summary>
        ///     Initializes a new instance of the <see cref="Edge" /> class.
        /// </summary>
        /// <param name="name">The name of the edge.</param>
        /// <param name="shape">The shape the edge belongs to.</param>
        internal Edge([NotNull] string name, [NotNull] Shape shape)
        {
            Debug.Assert(name != null, "name != null");
            Debug.Assert(shape != null, "shape != null");
            Name = name;
            Shape = shape;
        }

        /// <summary>
        ///     Gets the name of the edge.
        /// </summary>
        /// <value>
        ///     The edge name.
        /// </value>
        [NotNull]
        public string Name { get; }

        /// <summary>
        ///     Gets the shape this edge belongs to.
        /// </summary>
        /// <value>
        ///     The shape.
        /// </value>
        [NotNull]
        public Shape Shape { get; }

        /// <summary>
        ///     Gets the vertex at the start of the edge.
        /// </summary>
        /// <value>
        ///     The start vertex.
        /// </value>
        [NotNull]
        public Vertex Start
        {
            get
            {
                Debug.Assert(_start != null, "_start != null");
                return _start;
            }
            internal set
            {
                if (_start != null) throw new InvalidOperationException();
                _start = value;
            }
        }

        /// <summary>
        ///     Gets the vertex at the end of the edge.
        /// </summary>
        /// <value>
        ///     The end vertex.
        /// </value>
        [NotNull]
        public Vertex End
        {
            get
            {
                Debug.Assert(_end != null, "_end != null");
                return _end;
            }
            internal set
            {
                if (_end != null) throw new InvalidOperationException();
                _end = value;
            }
        }

        /// <summary>
        ///     Gets the vector from <see cref="Start" /> to <see cref="End" />.
        /// </summary>
        /// <value>
        ///     The vector.
        /// </value>
        public Vector2 Vector => End.Location - Start.Location;

        /// <summary>
        ///     Gets the length of the edge.
        /// </summary>
        /// <value>
        ///     The length of the edge.
        /// </value>
        public float Length => Vector2.Distance(Start.Location, End.Location);

        /// <summary>
        ///     Gets the center point of the edge.
        /// </summary>
        /// <value>
        ///     The center point of the edge.
        /// </value>
        public Vector2 Center => (Start.Location + End.Location) / 2;

        /// <summary>
        ///     Gets the distance from a point to this edge.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <returns></returns>
        public float DistanceTo(Vector2 point)
        {
            Vector2 p1 = Start.Location;
            Vector2 p2 = End.Location;

            float l2 = Vector2.DistanceSquared(p1, p2);

            float t = Vector2.Dot(point - p1, p2 - p1) / l2;

            if (t < 0) return Vector2.Distance(point, p1);
            if (t > 1) return Vector2.Distance(point, p2);

            Vector2 proj = p1 + t * (p2 - p1);
            return Vector2.Distance(point, proj);
        }

        /// <summary>
        ///     Gets the point the specified amount along the edge.
        /// </summary>
        /// <param name="amount">
        ///     The amount, in the range 0-1. 0 returns the <see cref="Start" /> location, 1 returns the
        ///     <see cref="End" /> location.
        /// </param>
        /// <remarks>The <paramref name="amount" /> will be clamped to the range 0-1.</remarks>
        /// <returns></returns>
        public Vector2 GetPointOnEdge(float amount)
        {
            if (amount <= 0) return Start.Location;
            if (amount >= 1) return End.Location;
            return Start.Location + Vector * amount;
        }
    }
}