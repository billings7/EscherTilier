using System;
using System.Drawing;
using System.Numerics;

namespace EscherTilier.Styles
{
    public class ImageStyle : IStyle
    {
        public ImageStyle(Bitmap image, Vector2 location, Vector2 size, float angle)
        {
            Image = image;
            Location = location;
            Size = size;
            Angle = angle;
        }

        public Bitmap Image { get; }

        public Vector2 Location { get; }

        public Vector2 Size { get; }

        public float Angle { get; }

        public IStyle Transform(Matrix3x2 matrix)
        {
            throw new NotImplementedException();
        }
    }
}