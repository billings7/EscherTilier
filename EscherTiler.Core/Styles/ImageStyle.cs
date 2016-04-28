using System;
using System.Numerics;
using EscherTiler.Graphics;
using JetBrains.Annotations;

namespace EscherTiler.Styles
{
    /// <summary>
    ///     A style which draws an image.
    /// </summary>
    public class ImageStyle : IStyle
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ImageStyle" /> class.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="location">The location.</param>
        /// <param name="scale">The scale.</param>
        /// <param name="angle">The angle of the image, in radians.</param>
        public ImageStyle([NotNull] IImage image, Vector2 location, Vector2 scale, float angle = 0f)
        {
            if (image == null) throw new ArgumentNullException(nameof(image));
            Image = image;
            ImageTransform =
                Matrix3x2.CreateScale(scale.X, scale.Y) *
                Matrix3x2.CreateRotation(angle) *
                Matrix3x2.CreateTranslation(location);
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ImageStyle" /> class.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="transform">The transform.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public ImageStyle([NotNull] IImage image, Matrix3x2 transform)
        {
            if (image == null) throw new ArgumentNullException(nameof(image));
            Image = image;
            ImageTransform = transform;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ImageStyle" /> class.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public ImageStyle([NotNull] IImage image)
        {
            if (image == null) throw new ArgumentNullException(nameof(image));
            Image = image;
            ImageTransform = Matrix3x2.Identity;
        }

        /// <summary>
        ///     Gets the type of the style.
        /// </summary>
        /// <value>
        ///     The type.
        /// </value>
        public StyleType Type => StyleType.Image;

        /// <summary>
        ///     Gets the image.
        /// </summary>
        /// <value>
        ///     The image.
        /// </value>
        [NotNull]
        public IImage Image { get; }

        /// <summary>
        ///     Gets the image transform.
        /// </summary>
        /// <value>
        ///     The image transform.
        /// </value>
        public Matrix3x2 ImageTransform { get; }

        // TODO extend mode

        /// <summary>
        ///     Returns a copy of this style with the given transform applied.
        /// </summary>
        /// <param name="matrix">The transform matrix.</param>
        /// <returns>
        ///     The transformed style.
        /// </returns>
        public IStyle Transform(Matrix3x2 matrix)
        {
            if (matrix.IsIdentity) return this;
            return new ImageStyle(Image, ImageTransform * matrix);
        }
    }
}