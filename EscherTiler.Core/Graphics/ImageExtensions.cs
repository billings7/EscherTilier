#region © Copyright Web Applications (UK) Ltd, 2015.  All rights reserved.

// Copyright (c) 2015, Web Applications UK Ltd
// All rights reserved.
//
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions are met:
//     * Redistributions of source code must retain the above copyright
//       notice, this list of conditions and the following disclaimer.
//     * Redistributions in binary form must reproduce the above copyright
//       notice, this list of conditions and the following disclaimer in the
//       documentation and/or other materials provided with the distribution.
//     * Neither the name of Web Applications UK Ltd nor the
//       names of its contributors may be used to endorse or promote products
//       derived from this software without specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
// WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
// DISCLAIMED. IN NO EVENT SHALL WEB APPLICATIONS UK LTD BE LIABLE FOR ANY
// DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
// (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
// LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
// ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
// SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

#endregion

using System;
using System.IO;
using JetBrains.Annotations;

namespace EscherTiler.Graphics
{
    /// <summary>
    ///     Extension methods for Graphics.
    /// </summary>
    [PublicAPI]
    public static class ImageExtensions
    {
        /// <summary>
        ///     Gets the image format from the header of the image data.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <returns>ImageFormat.</returns>
        /// <exception cref="System.InvalidOperationException"></exception>
        /// <remarks>
        ///     See (amongst others) http://www.garykessler.net/library/file_sigs.html
        /// </remarks>
        public static ImageFormat GetImageFormat([NotNull] this byte[] bytes)
        {
            int length = bytes.Length;
            if (length > 2)
            {
                switch (bytes[0])
                {
                    case 0x00:
                        if ((length > 4) &&
                            (bytes[1] == 0x00) &&
                            (bytes[2] == 0x01) &&
                            (bytes[3] == 0x00))
                            return ImageFormat.Icon;
                        break;
                    case 0x01:
                        if ((length > 4) &&
                            (bytes[1] == 0x00) &&
                            (bytes[3] == 0x00))
                        {
                            if ((length > 6) &&
                                (bytes[2] == 0x09) &&
                                (bytes[5] == 0x03))
                                return ImageFormat.Wmf;
                            if (bytes[2] == 0x00)
                                return ImageFormat.Emf;
                        }
                        break;
                    case 0x42:
                        if ((bytes[1] == 0x4D))
                            return ImageFormat.Bmp;
                        break;
                    case 0x47:
                        if ((length > 3) &&
                            (bytes[1] == 0x49) &&
                            (bytes[2] == 0x46))
                            return ImageFormat.Gif;
                        break;
                    case 0x49:
                        if ((length > 4) &&
                            (bytes[1] == 0x49) &&
                            (bytes[2] == 0x2A) &&
                            (bytes[3] == 0x00))
                            return ImageFormat.Tiff;
                        if ((length > 3) &&
                            (bytes[1] == 0x20) &&
                            (bytes[2] == 0x49))
                            return ImageFormat.Tiff;
                        break;
                    case 0x4D:
                        if ((length > 4) &&
                            (bytes[1] == 0x4D) &&
                            (bytes[2] == 0x00) &&
                            ((bytes[3] == 0x2A) || (bytes[3] == 0x2B)))
                            return ImageFormat.Tiff;
                        break;
                    case 0x89:
                        if ((length > 4) &&
                            (bytes[1] == 0x50) &&
                            (bytes[2] == 0x4E) &&
                            (bytes[3] == 0x47))
                            return ImageFormat.Png;
                        break;
                    case 0xFF:
                        if ((length > 4) &&
                            (bytes[1] == 0xD8) &&
                            (bytes[2] == 0xFF) &&
                            ((bytes[3] >= 0xE0) && (bytes[3] <= 0xEF)))
                            return ImageFormat.Jpeg;
                        break;
                }
            }
            throw new ArgumentOutOfRangeException(nameof(bytes));
        }

        /// <summary>
        ///     Gets the image format from the header of the image data.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns>
        ///     ImageFormat.
        /// </returns>
        /// <exception cref="System.InvalidOperationException"></exception>
        /// <remarks>
        ///     See (amongst others) http://www.garykessler.net/library/file_sigs.html
        /// </remarks>
        public static ImageFormat GetImageFormat([NotNull] this Stream stream)
        {
            byte[] bytes = new byte[6];
            int read = stream.Read(bytes, 0, bytes.Length);
            Array.Resize(ref bytes, read);

            return bytes.GetImageFormat();
        }

        /// <summary>
        ///     Gets the file extension (without the dot) for the image format given.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentOutOfRangeException">null</exception>
        public static string GetFormatExtension(this ImageFormat format)
        {
            switch (format)
            {
                case ImageFormat.Bmp:
                    return "bmp";
                case ImageFormat.Emf:
                    return "emf";
                case ImageFormat.Wmf:
                    return "wmf";
                case ImageFormat.Gif:
                    return "gif";
                case ImageFormat.Jpeg:
                    return "jpg";
                case ImageFormat.Png:
                    return "png";
                case ImageFormat.Tiff:
                    return "tiff";
                case ImageFormat.Icon:
                    return "ico";
                default:
                    throw new ArgumentOutOfRangeException(nameof(format), format, null);
            }
        }
    }
}