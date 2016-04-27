using System;
using System.IO;

namespace EscherTiler.Graphics
{
    /// <summary>
    ///     Interface to image data.
    /// </summary>
    public interface IImage : IDisposable
    {
        /// <summary>
        ///     Gets a stream for reading the image data.
        /// </summary>
        /// <returns>A stream for reading the image.</returns>
        Stream GetStream();
    }
}