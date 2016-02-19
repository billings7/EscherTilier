using System;
using System.Numerics;

namespace EscherTilier.Graphics
{
    public interface IGraphicsPath : IDisposable
    {
        IGraphicsPath Start(Vector2 point);

        IGraphicsPath End(bool close = true);

        IGraphicsPath AddLine(Vector2 to);

        IGraphicsPath AddLines(params Vector2[] points);

        IGraphicsPath AddArc(Vector2 to, Vector2 radius, float angle, bool clockwise);

        IGraphicsPath AddQuadraticBezier(Vector2 control, Vector2 to);

        IGraphicsPath AddCubicBezier(Vector2 controlA, Vector2 controlB, Vector2 to);
    }
}