using System;
using System.Diagnostics;
using System.Numerics;
using JetBrains.Annotations;

namespace EscherTilier
{
    /// <summary>
    ///     Defines a vertex of a <see cref="Shape" />.
    /// </summary>
    public class Vertex
    {
        private const double RadToDeg = 180.0 / Math.PI;

        private Edge _in;
        private Edge _out;

        /// <summary>
        ///     Initializes a new instance of the <see cref="Vertex" /> class.
        /// </summary>
        /// <param name="name">The name of the vertex.</param>
        /// <param name="shape">The shape the vertex belongs to.</param>
        /// <param name="location">The initial location of the vertex.</param>
        internal Vertex([NotNull] string name, [NotNull] Shape shape, Vector2 location)
        {
            Debug.Assert(name != null, "name != null");
            Debug.Assert(shape != null, "shape != null");
            Name = name;
            Shape = shape;
            Location = location;
        }

        /// <summary>
        ///     Gets the name of the vertex.
        /// </summary>
        /// <value>
        ///     The vertex name.
        /// </value>
        [NotNull]
        public string Name { get; }

        /// <summary>
        ///     Gets the shape the vertex belongs to.
        /// </summary>
        /// <value>
        ///     The shape.
        /// </value>
        [NotNull]
        public Shape Shape { get; }

        /// <summary>
        ///     Gets the location of the vertex.
        /// </summary>
        /// <value>
        ///     The location.
        /// </value>
        public Vector2 Location { get; set; }

        /// <summary>
        ///     Gets the edge that goes into the vertex.
        /// </summary>
        /// <value>
        ///     The edge that goes into the vertex.
        /// </value>
        [NotNull]
        public Edge In
        {
            get
            {
                Debug.Assert(_in != null, "_in != null");
                return _in;
            }
            internal set
            {
                if (_in != null) throw new InvalidOperationException();
                _in = value;
            }
        }

        /// <summary>
        ///     Gets the edge that goes out of the vertex.
        /// </summary>
        /// <value>
        ///     The edge that goes out of the vertex.
        /// </value>
        [NotNull]
        public Edge Out
        {
            get
            {
                Debug.Assert(_out != null, "_out != null");
                return _out;
            }
            internal set
            {
                if (_out != null) throw new InvalidOperationException();
                _out = value;
            }
        }

        /// <summary>
        ///     Gets the angle of the vertex.
        /// </summary>
        /// <value>
        ///     The vertex angle.
        /// </value>
        public float Angle
        {
            get
            {
                Vector2 a = In.Start.Location;
                Vector2 b = Location;
                Vector2 c = Out.End.Location;

                float angle =
                    (float)
                        (RadToDeg *
                         Math.Acos(Vector2.Dot(b - a, b - c) / (Vector2.Distance(a, b) * Vector2.Distance(b, c))));

                float dir = (b.X - a.X) * (b.Y + a.Y) + (c.X - b.X) * (c.Y + b.Y);

                if (dir > 0 != Shape.IsClockwise)
                    angle = 360 - angle;

                return angle;
            }
        }
    }
}