using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using EscherTilier.Styles;
using JetBrains.Annotations;
using SharpDX.Direct2D1;
using SharpDX.WIC;
using Bitmap = SharpDX.Direct2D1.Bitmap;
using BitmapInterpolationMode = SharpDX.Direct2D1.BitmapInterpolationMode;
using FactoryD2D = SharpDX.Direct2D1.Factory;
using FactoryWrite = SharpDX.DirectWrite.Factory;

namespace EscherTilier.Graphics.DirectX
{
    public class DirectXResourceManager : ResourceManager, IResourceManager<IStyle, Brush>,
        IResourceManager<IImage, Bitmap>
    {
        /// <summary>
        ///     The <see cref="FactoryD2D" /> instance.
        /// </summary>
        [NotNull]
        public static readonly FactoryD2D FactoryD2D = new FactoryD2D(FactoryType.MultiThreaded);

        [NotNull]
        private static readonly FactoryWrite _factoryWrite = new FactoryWrite(SharpDX.DirectWrite.FactoryType.Shared);

        [NotNull]
        private static readonly ImagingFactory _imagingFactory = new ImagingFactory();

        /// <summary>
        ///     Creates an empty <see cref="PathGeometry" />.
        /// </summary>
        [NotNull]
        public static PathGeometry CreatePathGeometry() => new PathGeometry(FactoryD2D);

        /// <summary>
        ///     Creates a stroke style.
        /// </summary>
        /// <param name="properties">The properties.</param>
        /// <returns></returns>
        [NotNull]
        public static StrokeStyle CreateStrokeStyle(StrokeStyleProperties properties)
            => new StrokeStyle(FactoryD2D, properties);

        [NotNull]
        private readonly object _lock = new object();

        [NotNull]
        private RenderTarget _renderTarget;

        [CanBeNull]
        private Dictionary<IStyle, Brush> _brushes = new Dictionary<IStyle, Brush>();

        [CanBeNull]
        private Dictionary<IImage, Bitmap> _bitmaps = new Dictionary<IImage, Bitmap>();

        [CanBeNull]
        private Dictionary<IImage, Bitmap> _tempBitmaps = new Dictionary<IImage, Bitmap>();

        /// <summary>
        ///     Initializes a new instance of the <see cref="DirectXResourceManager" /> class.
        /// </summary>
        /// <param name="renderTarget">The render target.</param>
        public DirectXResourceManager([NotNull] RenderTarget renderTarget)
        {
            if (renderTarget == null) throw new ArgumentNullException(nameof(renderTarget));
            _renderTarget = renderTarget;
        }

        /// <summary>
        ///     Gets or sets the render target.
        /// </summary>
        /// <value>
        ///     The render target.
        /// </value>
        public RenderTarget RenderTarget
        {
            get { return _renderTarget; }
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));
                if (_renderTarget == value) return;

                lock (_lock)
                {
                    _renderTarget = value;

                    Dictionary<IStyle, Brush> newBrushes = new Dictionary<IStyle, Brush>();
                    Dictionary<IStyle, Brush> brushes = Interlocked.Exchange(
                        ref _brushes,
                        newBrushes);
                    Debug.Assert(brushes != null, "brushes != null");

                    foreach (KeyValuePair<IStyle, Brush> kvp in brushes)
                    {
                        Debug.Assert(kvp.Value != null, "kvp.Value != null");
                        Debug.Assert(kvp.Key != null, "kvp.Key != null");

                        newBrushes.Add(kvp.Key, CreateBrush(kvp.Key));

                        kvp.Value.Dispose();
                    }
                }
            }
        }

        /// <summary>
        ///     Creates a <see cref="Brush" /> from an <see cref="IStyle" /> using the current render target.
        /// </summary>
        /// <param name="style">The style.</param>
        /// <returns></returns>
        [NotNull]
        private Brush CreateBrush([NotNull] IStyle style)
        {
            lock (_lock)
            {
                SolidColourStyle solidColour = style as SolidColourStyle;
                if (solidColour != null)
                    return new SolidColorBrush(_renderTarget, solidColour.Colour.ToRawColor4());

                LinearGradientStyle linearGradient = style as LinearGradientStyle;
                if (linearGradient != null)
                {
                    return new LinearGradientBrush(
                        _renderTarget,
                        new LinearGradientBrushProperties
                        {
                            StartPoint = linearGradient.Start.ToRawVector2(),
                            EndPoint = linearGradient.End.ToRawVector2()
                        },
                        new GradientStopCollection(
                            _renderTarget,
                            linearGradient.GradientStops.Select(DirectXExtensions.ToGradientStop).ToArray()));
                }

                ImageStyle image = style as ImageStyle;
                if (image != null)
                {
                    Bitmap bitmap = Get(image.Image);
                    return new BitmapBrush(
                        _renderTarget,
                        bitmap,
                        new BitmapBrushProperties
                        {
                            // TODO set from style
                            ExtendModeX = ExtendMode.Clamp,
                            ExtendModeY = ExtendMode.Clamp,
                            InterpolationMode = BitmapInterpolationMode.Linear
                        },
                        new BrushProperties
                        {
                            Transform = image.ImageTransform.ToRawMatrix3x2()
                        });
                }

                throw new NotImplementedException();
            }
        }

        /// <summary>
        ///     Creates a <see cref="Bitmap" /> from an <see cref="IImage" /> using the current render target.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <returns></returns>
        [NotNull]
        private Bitmap CreateBitmap([NotNull] IImage image)
        {
            using (Stream stream = image.GetStream())
            using (BitmapDecoder decoder = new BitmapDecoder(
                _imagingFactory,
                stream,
                DecodeOptions.CacheOnDemand))
            using (BitmapFrameDecode source = decoder.GetFrame(0))
            using (FormatConverter converter = new FormatConverter(_imagingFactory))
            {
                converter.Initialize(source, SharpDX.WIC.PixelFormat.Format32bppPRGBA);

                Bitmap bitmap = Bitmap.FromWicBitmap(_renderTarget, converter);

                Debug.Assert(bitmap != null, "bitmap != null");
                return bitmap;
            }
        }

        /// <summary>
        ///     Adds the styles from the <see cref="StyleManager" /> given to this resource manager.
        /// </summary>
        /// <param name="manager">The manager.</param>
        public void AddFromStyleManager([NotNull] StyleManager manager)
        {
            if (manager == null) throw new ArgumentNullException(nameof(manager));

            foreach (IStyle style in manager.Styles.Select(ts => ts.Style).Distinct())
            {
                Debug.Assert(style != null, "style != null");

                ImageStyle imageStyle = style as ImageStyle;
                if (imageStyle != null)
                    Add(imageStyle.Image, CreateBitmap(imageStyle.Image));

                Add(style, CreateBrush(style));
            }
        }

        /// <summary>
        ///     Gets the brush for the specified style.
        /// </summary>
        /// <param name="style">The style.</param>
        /// <returns>The brush.</returns>
        /// <exception cref="System.Collections.Generic.KeyNotFoundException">
        ///     The manager did not contain a resource with the given
        ///     key.
        /// </exception>
        public Brush Get(IStyle style)
        {
            if (style == null) throw new ArgumentNullException(nameof(style));

            lock (_lock)
            {
                if (_brushes == null) throw new ObjectDisposedException(nameof(DirectXResourceManager));
                Brush brush;
                if (_brushes.TryGetValue(style, out brush))
                {
                    Debug.Assert(brush != null, "brush != null");
                    return brush;
                }
                return CreateBrush(style);
            }
        }

        /// <summary>
        ///     Adds the specified <see cref="IStyle" /> to the manager, creating the brush for it.
        /// </summary>
        /// <param name="style">The style.</param>
        /// <returns>The created brush.</returns>
        public Brush Add(IStyle style)
        {
            if (style == null) throw new ArgumentNullException(nameof(style));

            lock (_lock)
            {
                if (_brushes == null) throw new ObjectDisposedException(nameof(DirectXResourceManager));
                Brush brush = CreateBrush(style);
                _brushes.Add(style, brush);
                return brush;
            }
        }

        /// <summary>
        ///     Adds the specified <see cref="IStyle" /> to <see cref="Brush" /> resource mapping to the manager.
        /// </summary>
        /// <param name="style">The style.</param>
        /// <param name="brush">The brush.</param>
        public void Add(IStyle style, Brush brush)
        {
            if (style == null) throw new ArgumentNullException(nameof(style));
            if (brush == null) throw new ArgumentNullException(nameof(brush));

            lock (_lock)
            {
                if (_brushes == null) throw new ObjectDisposedException(nameof(DirectXResourceManager));
                _brushes.Add(style, brush);
            }
        }

        /// <summary>
        ///     Releases the specified <see cref="Brush" />.
        /// </summary>
        /// <param name="style">The style.</param>
        /// <param name="brush">The brush.</param>
        public void Release(IStyle style, Brush brush)
        {
            if (style == null) throw new ArgumentNullException(nameof(style));
            if (brush == null) throw new ArgumentNullException(nameof(brush));

            lock (_lock)
            {
                if (_brushes == null) throw new ObjectDisposedException(nameof(DirectXResourceManager));
                Debug.Assert(_tempBitmaps != null, "_tempBitmaps != null");

                ImageStyle imageStyle = style as ImageStyle;
                Bitmap bitmap;
                if (imageStyle != null && _tempBitmaps.TryGetValue(imageStyle.Image, out bitmap))
                {
                    Debug.Assert(bitmap != null, "bitmap != null");
                    Release(imageStyle.Image, bitmap);
                }

                if (!_brushes.ContainsValue(brush))
                    brush.Dispose();
            }
        }

        /// <summary>
        ///     Gets the bitmap for the specified image.
        /// </summary>
        /// <returns>The resource.</returns>
        /// <exception cref="System.Collections.Generic.KeyNotFoundException">
        ///     The manager did not contain a resource with the given key.
        /// </exception>
        public Bitmap Get(IImage image)
        {
            if (image == null) throw new ArgumentNullException(nameof(image));

            lock (_lock)
            {
                if (_bitmaps == null) throw new ObjectDisposedException(nameof(DirectXResourceManager));
                Bitmap bitmap;
                if (_bitmaps.TryGetValue(image, out bitmap))
                {
                    Debug.Assert(bitmap != null, "bitmap != null");
                    return bitmap;
                }

                Debug.Assert(_tempBitmaps != null, "_tempBitmaps != null");
                if (_tempBitmaps.TryGetValue(image, out bitmap))
                {
                    Debug.Assert(bitmap != null, "bitmap != null");
                    return bitmap;
                }

                bitmap = CreateBitmap(image);
                _tempBitmaps.Add(image, bitmap);
                return bitmap;
            }
        }

        /// <summary>
        ///     Adds the specified <see cref="IImage" /> to the manager, creating the bitmap for it.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <returns>The created bitmap.</returns>
        [NotNull]
        public Bitmap Add(IImage image)
        {
            if (image == null) throw new ArgumentNullException(nameof(image));

            lock (_lock)
            {
                if (_bitmaps == null) throw new ObjectDisposedException(nameof(DirectXResourceManager));
                Bitmap bitmap = CreateBitmap(image);
                _bitmaps.Add(image, bitmap);
                return bitmap;
            }
        }

        /// <summary>
        ///     Adds the specified <see cref="IImage" /> to <see cref="Bitmap" /> resource mapping to the manager.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="bitmap">The bitmap.</param>
        public void Add(IImage image, Bitmap bitmap)
        {
            if (image == null) throw new ArgumentNullException(nameof(image));
            if (bitmap == null) throw new ArgumentNullException(nameof(bitmap));

            lock (_lock)
            {
                if (_bitmaps == null) throw new ObjectDisposedException(nameof(DirectXResourceManager));
                _bitmaps.Add(image, bitmap);
            }
        }

        /// <summary>
        ///     Releases the specified bitmap.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="bitmap">The bitmap.</param>
        public void Release(IImage image, Bitmap bitmap)
        {
            if (image == null) throw new ArgumentNullException(nameof(image));
            if (bitmap == null) throw new ArgumentNullException(nameof(bitmap));

            lock (_lock)
            {
                if (_bitmaps == null) throw new ObjectDisposedException(nameof(DirectXResourceManager));
                Debug.Assert(_tempBitmaps != null, "_tempBitmaps != null");

                if (_tempBitmaps.ContainsValue(bitmap))
                {
                    foreach (IImage img in _tempBitmaps
                        .Where(kvp => kvp.Value == bitmap)
                        .Select(kvp => kvp.Key).ToArray())
                        _tempBitmaps.Remove(img);
                    Debug.Assert(!_tempBitmaps.ContainsValue(bitmap));
                    Debug.Assert(!_tempBitmaps.ContainsKey(image));
                }

                if (!_bitmaps.ContainsValue(bitmap))
                    bitmap.Dispose();
            }
        }

        /// <summary>
        ///     Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing">
        ///     <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" />
        ///     to release only unmanaged resources.
        /// </param>
        protected override void Dispose(bool disposing)
        {
            if (!disposing) return;

            lock (_lock)
            {
                Dictionary<IStyle, Brush> brushes = Interlocked.Exchange(
                    ref _brushes,
                    null);
                if (brushes != null)
                {
                    foreach (KeyValuePair<IStyle, Brush> kvp in brushes)
                    {
                        Debug.Assert(kvp.Value != null, "kvp.Value != null");
                        Debug.Assert(kvp.Key != null, "kvp.Key != null");

                        kvp.Value.Dispose();
                    }
                }

                Dictionary<IImage, Bitmap> bitmaps = Interlocked.Exchange(
                    ref _bitmaps,
                    null);
                if (bitmaps != null)
                {
                    foreach (KeyValuePair<IImage, Bitmap> kvp in bitmaps)
                    {
                        Debug.Assert(kvp.Value != null, "kvp.Value != null");
                        Debug.Assert(kvp.Key != null, "kvp.Key != null");

                        kvp.Value.Dispose();
                    }
                }

                bitmaps = Interlocked.Exchange(
                    ref _tempBitmaps,
                    null);
                if (bitmaps != null)
                {
                    foreach (KeyValuePair<IImage, Bitmap> kvp in bitmaps)
                    {
                        Debug.Assert(kvp.Value != null, "kvp.Value != null");
                        Debug.Assert(kvp.Key != null, "kvp.Key != null");

                        kvp.Value.Dispose();
                    }
                }
            }
        }
    }
}