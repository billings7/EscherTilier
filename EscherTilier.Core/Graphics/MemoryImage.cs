using System;
using System.IO;

namespace EscherTilier.Graphics
{
    /// <summary>
    ///     An image stored in memory.
    /// </summary>
    public class MemoryImage : IImage
    {
        private byte[] _data;

        /// <summary>
        ///     Initializes a new instance of the <see cref="MemoryImage" /> class.
        /// </summary>
        /// <param name="data">The data.</param>
        public MemoryImage(byte[] data)
        {
            _data = data;
        }

        /// <summary>
        ///     Gets a stream for reading the image data.
        /// </summary>
        /// <returns>A stream for reading the image.</returns>
        public Stream GetStream()
        {
            if (_data == null) throw new ObjectDisposedException(nameof(MemoryImage));
            return new MemoryStream(_data, false);
        }

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose() => _data = null;
    }
}