using System;
using System.Numerics;
using EscherTilier.Numerics;

namespace EscherTilier
{
    public interface IView
    {
        Rectangle ViewBounds { get; }
        Matrix3x2 ViewMatrix { get; }
        Matrix3x2 InverseViewMatrix { get; }

        event EventHandler ViewBoundsChanged;
    }
}