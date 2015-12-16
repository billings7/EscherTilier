using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EscherTilier.Expressions;
using JetBrains.Annotations;
using System.Numerics;
using EscherTilier.Numerics;
using EscherTilier.Styles;

namespace EscherTilier
{
    public class Template
    {
        public IReadOnlyList<ShapeTemplate> Shapes { get; }

        public IReadOnlyList<IExpression<bool>> ShapeConstraints { get; }

        public IReadOnlyList<TilingDefinition> Tilings { get; }

        public IEnumerable<Shape> CreateShape()
        {
            throw new NotImplementedException();
        }

        public Tiling CreateTiling(IEnumerable<Shape> shapes)
        {
            throw new NotImplementedException();
        }
    }

    public class ShapeTemplate
    {
        public Template Template { get; }

        public string Name { get; }
        
        public IReadOnlyList<string> EdgeNames { get; }

        public IReadOnlyList<string> VertexNames { get; }
    }

    public class Shape
    {
        public ShapeTemplate Template { get; }

        public IReadOnlyList<Vertex> Vertices { get; }

        public IReadOnlyList<Edge> Edges { get; }
    }

    public class Vertex
    {
        public Shape Shape { get; }

        public Vector2 Location { get; }

        public float Angle()
        {
            throw new NotImplementedException();
        }
    }

    public class Edge
    {
        public Shape Shape { get; }

        public Vector2 Start { get; }

        public Vector2 End { get; }

        public float Length() => Vector2.Distance(Start, End);
    }

    public class TilingDefinition
    {
        public int ID { get; }

        [CanBeNull]
        public IExpression<bool> Condition { get; }

        public IReadOnlyList<EdgePattern> EdgePatterns { get; }
    }

    public class EdgePattern
    {
        public string EdgeName { get; }

        public IReadOnlyList<EdgePart> Parts { get; }
    }

    public class EdgePart
    {
        public int ID { get; }

        public PartDirection Direction { get; }

        public float Amount { get; }

        public IReadOnlyList<EdgePartAdjacency> AdjacentParts { get; }
    }

    public class EdgePartAdjacency
    {
        public string Label { get; }

        public EdgePart AdjacentPart { get; }

        public string AdjacentLabel { get; }
    }

    public enum PartDirection
    {
        ClockwiseOut,
        ClockwiseIn,
        CounterClockwiseOut,
        CounterClockwiseIn
    }

    public class Tiling
    {
        public Template Template { get; }

        public TilingDefinition Definition { get; }

        public IReadOnlyList<Tile> Tiles { get; }

        public LineStyle LineStyle { get; }

        public StyleManager Styles { get; }

        public IEnumerable<ITile> GetTiles(Rectangle bounds, StyleManager styleManager, IEnumerable<ITile> existingTiles)
        {
            throw new NotImplementedException();
        }
    }

    public interface ITile
    {
        string Label { get; }

        IStyle Style { get; }

        Matrix3x2 Transform { get; }

        Shape Shape { get; }
    }

    public class Tile : ITile
    {
        public string Label { get; }

        public Shape Shape { get; }

        public IStyle Style { get; }

        public Matrix3x2 Transform { get; }

        public IReadOnlyList<EdgePartShape> PartShapes { get; }

        public void PopulateGraphicsPath(IGraphicsPath path)
        {
            throw new NotImplementedException();
        }
    }

    public class TileInstance : ITile
    {
        public string Label { get; }

        public Shape Shape { get; }

        public IStyle Style { get; }

        public Matrix3x2 Transform { get; }

        private readonly Tile _tile;
    }

    public class EdgePartShape
    {
        public EdgePart Part { get; }

        public IReadOnlyList<ILine> Lines { get; }
    }

    public interface ILine
    {
        Vector2 Start { get; }

        Vector2 End { get; }
    }

    public class Line : ILine
    {
        public Vector2 End { get; }

        public Vector2 Start { get; }
    }

    public class QuadraticBezierCurve : ILine
    {
        public Vector2 End { get; }

        public Vector2 ControlPoint { get; }

        public Vector2 Start { get; }
    }

    public class CubicBezierCurve : ILine
    {
        public Vector2 End { get; }

        public Vector2 ControlPointA { get; }

        public Vector2 ControlPointB { get; }

        public Vector2 Start { get; }
    }

    public class Arc : ILine
    {
        public Vector2 End { get; }
        
        public Vector2 Start { get; }

        public Vector2 Radius { get; }

        public float Angle { get; }

        public bool Clockwise { get; }
    }
}
