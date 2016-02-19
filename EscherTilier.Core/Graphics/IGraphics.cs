using System;
using System.Numerics;
using EscherTilier.Numerics;
using EscherTilier.Styles;

namespace EscherTilier.Graphics
{
    public interface IGraphics : IDisposable
    {
        IGraphicsPath CreatePath();

        void DrawPath(IGraphicsPath path);
        void FillPath(IGraphicsPath path);

        void DrawLine(Vector2 from, Vector2 to);

        void DrawLines(Vector2[] points);

        void DrawArc(Vector2 to, Vector2 radius, float angle, bool clockwise);

        void DrawCircle(Vector2 point, float radius);
        void FillCircle(Vector2 point, float radius);

        void DrawEllipse(Vector2 point, Vector2 radius);
        void FillEllipse(Vector2 point, Vector2 radius);

        void DrawRectangle(Rectangle rect);
        void FillEllipse(Rectangle rect);

        Matrix3x2 Transform { get; set; }

        IResourceManager ResourceManager { get; set; }

        IStyle Style { get; set; }

        LineStyle LineStyle { get; set; }

        void SaveState();

        void RestoreState();
    }
}