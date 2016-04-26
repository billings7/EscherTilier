using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using EscherTilier.Numerics;
using JetBrains.Annotations;

namespace EscherTilier
{
    /// <summary>
    ///     Defines the position of an edge part.
    /// </summary>
    public struct EdgePartPosition
    {
        /// <summary>
        ///     Creates a new <see cref="EdgePartPosition" /> from an edge part and shape.
        /// </summary>
        /// <param name="edgePart">The edge part.</param>
        /// <param name="shape">The shape.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">
        ///     <paramref name="edgePart" /> or <paramref name="shape" /> was <see langword="null" />.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static EdgePartPosition Create([NotNull] EdgePart edgePart, [NotNull] Shape shape)
            => Create(edgePart, shape, Matrix3x2.Identity);

        /// <summary>
        ///     Creates a new <see cref="EdgePartPosition" /> from an edge part, shape and transform.
        /// </summary>
        /// <param name="edgePart">The edge part.</param>
        /// <param name="shape">The shape.</param>
        /// <param name="transform">The transform.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">
        ///     <paramref name="edgePart" /> or <paramref name="shape" /> was <see langword="null" />.
        /// </exception>
        public static EdgePartPosition Create([NotNull] EdgePart edgePart, [NotNull] Shape shape, Matrix3x2 transform)
        {
            if (edgePart == null) throw new ArgumentNullException(nameof(edgePart));
            if (shape == null) throw new ArgumentNullException(nameof(shape));

            Edge edge = shape.GetEdge(edgePart.EdgePattern.EdgeName);

            Vector2 startPoint = edge.GetPointOnEdge(edgePart.StartAmount);
            Vector2 endPoint = edge.GetPointOnEdge(edgePart.StartAmount + edgePart.Amount);

            if (!transform.IsIdentity)
            {
                startPoint = Vector2.Transform(startPoint, transform);
                endPoint = Vector2.Transform(endPoint, transform);
            }

            return new EdgePartPosition(edgePart, startPoint, endPoint);
        }

        /// <summary>
        ///     The edge part shape.
        /// </summary>
        public readonly EdgePart Part;

        /// <summary>
        ///     The start position.
        /// </summary>
        public readonly Vector2 Start;

        /// <summary>
        ///     The end position.
        /// </summary>
        public readonly Vector2 End;

        /// <summary>
        ///     Initializes a new instance of the <see cref="EdgePartPosition" /> struct.
        /// </summary>
        /// <param name="part">The edge part.</param>
        /// <param name="start">The part start position.</param>
        /// <param name="end">The pert end position.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public EdgePartPosition([NotNull] EdgePart part, Vector2 start, Vector2 end)
        {
            if (part == null) throw new ArgumentNullException(nameof(part));
            Part = part;
            Start = start;
            End = end;
        }

        /// <summary>
        ///     Gets the matrix that transforms the part at this position to another position.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns></returns>
        /// <exception cref="System.InvalidOperationException">
        ///     The method has been called on a default instance of the <see cref="EdgePartPosition" /> struct.
        /// </exception>
        /// <exception cref="System.ArgumentException">The other edge part has not been set.</exception>
        public Matrix3x2 GetTransformTo(EdgePartPosition other)
        {
            if (Part == null) throw new InvalidOperationException("Edge part not set.");
            if (other.Part == null) throw new ArgumentException("Other edge part not set.", nameof(other));

            bool isClockwise = Part.IsClockwise;
            bool otherIsClockwise = other.Part.IsClockwise;

            Vector2 fromStart = isClockwise ? Start : End;
            Vector2 fromEnd = isClockwise ? End : Start;
            Vector2 toStart = otherIsClockwise ? other.Start : other.End;
            Vector2 toEnd = otherIsClockwise ? other.End : other.Start;

            Matrix3x2 transform = Matrix.GetTransform(fromStart, fromEnd, toStart, toEnd);

            if (isClockwise == otherIsClockwise)
                transform *= Matrix.CreateReflection(toStart, toEnd);

            return transform;
        }
    }
}