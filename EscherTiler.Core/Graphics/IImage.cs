using System;
using System.IO;
using JetBrains.Annotations;

namespace EscherTiler.Graphics
{
    /// <summary>
    ///     Interface to image data.
    /// </summary>
    public interface IImage : IDisposable
    {
        /// <summary>
        ///     Gets the format of the image.
        /// </summary>
        /// <value>
        ///     The format.
        /// </value>
        ImageFormat Format { get; }

        /// <summary>
        ///     Gets a stream for reading the image data.
        /// </summary>
        /// <returns>A stream for reading the image.</returns>
        [NotNull]
        Stream GetStream();
    }
}