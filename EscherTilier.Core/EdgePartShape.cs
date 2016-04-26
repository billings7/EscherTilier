using EscherTilier.Numerics;
using JetBrains.Annotations;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace EscherTilier
{
    public class EdgePartShape
    {
        public EdgePartShape(EdgePart part, Edge edge, ShapeLines lines)
        {
            Part = part;
            Edge = edge;
            Lines = lines;
        }

        [NotNull]
        public Edge Edge { get; }

        [NotNull]
        public EdgePart Part { get; }

        [NotNull]
        [ItemNotNull]
        public ShapeLines Lines { get; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Matrix3x2 GetLineTransform(bool inverse = false)
        {
            var start = Edge.Start.Location;
            var end = Edge.End.Location;
            var diff = end - start;

            start += diff * Part.StartAmount;
            end = start + diff * Part.Amount;

            if (!Part.IsClockwise)
            {
                var tmp = start;
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