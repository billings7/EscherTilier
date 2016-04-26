using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using EscherTilier.Numerics;
using JetBrains.Annotations;

namespace EscherTilier
{
    /// <summary>
    ///     Defines the shape of an <see cref="EdgePart" />.
    /// </summary>
    public class EdgePartShape
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="EdgePartShape" /> class.
        /// </summary>
        /// <param name="part">The part.</param>
        /// <param name="edge">The edge.</param>
        /// <param name="lines">The lines.</param>
        public EdgePartShape([NotNull] EdgePart part, [NotNull] Edge edge, [NotNull] ShapeLines lines)
        {
            if (part == null) throw new ArgumentNullException(nameof(part));
            if (edge == null) throw new ArgumentNullException(nameof(edge));
            if (lines == null) throw new ArgumentNullException(nameof(lines));
            Part = part;
            Edge = edge;
            Lines = lines;
        }

        /// <summary>
        ///     Gets the edge the part belongs to.
        /// </summary>
        /// <value>
        ///     The edge.
        /// </value>
        [NotNull]
        public Edge Edge { get; }

        /// <summary>
        ///     Gets the edge part whose shape this instance defines.
        /// </summary>
        /// <value>
        ///     The part.
        /// </value>
        [NotNull]
        public EdgePart Part { get; }

        /// <summary>
        ///     Gets the lines that set the shape of the edge part.
        /// </summary>
        /// <value>
        ///     The lines.
        /// </value>
        [NotNull]
        [ItemNotNull]
        public ShapeLines Lines { get; }

        /// <summary>
        ///     Gets a transform matrix for transforming a unit X vector to align with the <see cref="Edge" />.
        /// </summary>
        /// <param name="inverse">
        ///     if set to <see langword="true" />, a matrix that transforms the edge to a unit X vector will be
        ///     returned instead.
        /// </param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Matrix3x2 GetLineTransform(bool inverse = false)
        {
            Vector2 start = Edge.Start.Location;
            Vector2 end = Edge.End.Location;
            Vector2 diff = end - start;

            start += diff * Part.StartAmount;
            end = start + diff * Part.Amount;

            if (!Part.IsClockwise)
            {
                Vector2 tmp = start;
                start = end;
                end = tmp;
            }

            return inverse
                ? Matrix.GetTransform(
                    start,
                    end,
                    new Vector2(0, 0),
                    new Vector2(1, 0))
                : Matrix.GetTransform(
                    new Vector2(0, 0),
                    new Vector2(1, 0),
                    start,
                    end);
        }
    }
}