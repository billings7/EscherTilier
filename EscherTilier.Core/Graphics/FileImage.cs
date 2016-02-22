using System;
using System.IO;
using JetBrains.Annotations;

namespace EscherTilier.Graphics
{
    /// <summary>
    ///     An image stored in a file.
    /// </summary>
    public class FileImage : IImage
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="FileImage" /> class.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public FileImage([NotNull] string filePath)
        {
            if (filePath == null) throw new ArgumentNullException(nameof(filePath));
            FilePath = filePath;
        }

        /// <summary>
        ///     Gets the image file path.
        /// </summary>
        /// <value>
        ///     The file path.
        /// </value>
        [NotNull]
        public string FilePath { get; }

        /// <summary>
        ///     Gets a stream for reading the image data.
        /// </summary>
        /// <returns>A stream for reading the image.</returns>
        public Stream GetStream() => File.Open(FilePath, FileMode.Open, FileAccess.Read, FileShare.Read);

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose() { }
    }
}