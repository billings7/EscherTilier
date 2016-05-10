using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using EscherTiler.Graphics.Resources;
using EscherTiler.Styles;
using JetBrains.Annotations;
using SharpDX.Direct2D1;
using SharpDX.WIC;
using Bitmap = SharpDX.Direct2D1.Bitmap;
using BitmapInterpolationMode = SharpDX.Direct2D1.BitmapInterpolationMode;
using FactoryD2D = SharpDX.Direct2D1.Factory;
using FactoryWrite = SharpDX.DirectWrite.Factory;

namespace EscherTiler.Graphics.DirectX
{
    /// <summary>
    ///     Resource manager for DirectX related objects.
    /// </summary>
    /// <seealso cref="ResourceManager" />
    /// <seealso cref="IResourceManager{IStyle, Brush}" />
    /// <seealso cref="IResourceManager{IImage, Bitmap}" />
    public class DirectXResourceManager : ResourceManager,
        IResourceManager<IStyle, Brush>,
        IResourceManager<IImage, Bitmap>
    {
        /// <summary>
        ///     The <see cref="FactoryD2D" /> instance.
        /// </summary>
        [NotNull]
        public static readonly FactoryD2D FactoryD2D = new FactoryD2D(FactoryType.MultiThreaded);

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
        private ResourceDictionary<IStyle, Resource<Brush>> _brushes =
            new ResourceDictionary<IStyle, Resource<Brush>>();

        [CanBeNull]
        private ResourceDictionary<IImage, Bitmap> _bitmaps =
            new ResourceDictionary<IImage, Bitmap>();

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
        ///     Initializes a new instance of the <see cref="DirectXResourceManager" /> class.
        /// </summary>
        /// <param name="renderTarget">The render target.</param>
        /// <param name="styleManager">The style manager to initialise the resources with.</param>
        /// <exception cref="System.ArgumentNullException">
        /// </exception>
        public DirectXResourceManager([NotNull] RenderTarget renderTarget, [CanBeNull] StyleManager styleManager)
        {
            if (renderTarget == null) throw new ArgumentNullException(nameof(renderTarget));
            _renderTarget = renderTarget;
            if (styleManager != null)
                AddFromStyleManager(styleManager);
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

                if (_bitmaps == null)
                    throw new ObjectDisposedException(nameof(DirectXResourceManager));

                lock (_lock)
                {
                    if (_bitmaps == null)
                        throw new ObjectDisposedException(nameof(DirectXResourceManager));
                    if (_renderTarget == value) return;

                    _renderTarget = value;

                    ResourceDictionary<IImage, Bitmap> oldBitmaps = _bitmaps;
                    _bitmaps = new ResourceDictionary<IImage, Bitmap>();

                    foreach (KeyValuePair<IImage, Bitmap> kvp in oldBitmaps)
                    {
                        Debug.Assert(kvp.Value != null, "kvp.Value != null");
                        Debug.Assert(kvp.Key != null, "kvp.Key != null");

                        _bitmaps.Add(kvp.Key, CreateBitmap(kvp.Key), false);

                        kvp.Value.Dispose();
                    }

                    Debug.Assert(_brushes != null);
                    ResourceDictionary<IStyle, Resource<Brush>> oldBrushes = _brushes;
                    _brushes = new ResourceDictionary<IStyle, Resource<Brush>>();

                    foreach (KeyValuePair<IStyle, Resource<Brush>> kvp in oldBrushes)
                    {
                        Debug.Assert(kvp.Value != null, "kvp.Value != null");
                        Debug.Assert(kvp.Key != null, "kvp.Key != null");

                        _brushes.Add(kvp.Key, CreateBrush(kvp.Key, false), false);

                        kvp.Value.Dispose();
                    }
                }
            }
        }

        /// <summary>
        ///     Creates a <see cref="Brush" /> from an <see cref="IStyle" /> using the current render target.
        /// </summary>
        /// <param name="style">The style.</param>
        /// <param name="temp">if set to <see langword="true" /> the brush is temporary.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        [NotNull]
        private Resource<Brush> CreateBrush([NotNull] IStyle style, bool temp)
        {
            lock (_lock)
            {
                SolidColourStyle solidColour = style as SolidColourStyle;
                if (solidColour != null)
                    return new SolidColorBrush(_renderTarget, solidColour.Colour.ToRawColor4());

                RandomColourStyle randomColour = style as RandomColourStyle;
                if (randomColour != null)
                    return new SolidColorBrush(_renderTarget, randomColour.PositionColour.ToRawColor4());

                LinearGradientStyle linearGradient = style as LinearGradientStyle;
                if (linearGradient != null)
                {
                    GradientStopCollection gradientStops = new GradientStopCollection(
                        _renderTarget,
                        linearGradient.GradientStops.Select(DirectXExtensions.ToGradientStop).ToArray(),
                        Gamma.Linear);

                    return new Resource<Brush>(
                        new LinearGradientBrush(
                            _renderTarget,
                            new LinearGradientBrushProperties
                            {
                                StartPoint = linearGradient.Start.ToRawVector2(),
                                EndPoint = linearGradient.End.ToRawVector2()
                            },
                            gradientStops),
                        gradientStops);
                }

                RadialGradientStyle radialGradient = style as RadialGradientStyle;
                if (radialGradient != null)
                {
                    GradientStopCollection gradientStops = new GradientStopCollection(
                        _renderTarget,
                        radialGradient.GradientStops.Select(DirectXExtensions.ToGradientStop).ToArray(),
                        Gamma.Linear);

                    return new Resource<Brush>(
                        new RadialGradientBrush(
                            _renderTarget,
                            new RadialGradientBrushProperties
                            {
                                GradientOriginOffset = radialGradient.UnitOriginOffset.ToRawVector2(),
                                RadiusX = 1,
                                RadiusY = 1
                            },
                            new BrushProperties
                            {
                                Opacity = 1,
                                Transform = radialGradient.GradientTransform.ToRawMatrix3x2()
                            },
                            gradientStops),
                        gradientStops);
                }

                ImageStyle image = style as ImageStyle;
                if (image != null)
                {
                    Bitmap bitmap = temp ? Get(image.Image) : Add(image.Image);
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
                            Transform = image.ImageTransform.ToRawMatrix3x2(),
                            Opacity = 1
                        });
                }

                throw new NotSupportedException();
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
                    Add(imageStyle.Image);

                Add(style);
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
                Resource<Brush> brush;
                if (_brushes.TryGetResource(style, out brush))
                {
                    Debug.Assert(brush != null, "brush != null");
                    return brush.Value;
                }

                brush = CreateBrush(style, true);
                _brushes.Add(style, brush, true);
                return brush.Value;
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
                return _brushes.GetOrAdd(style, k => CreateBrush(k, false), false).Value;
            }
        }

        /// <summary>
        ///     Adds the specified <see cref="IStyle" /> to the manager, creating the brush for it.
        /// </summary>
        /// <param name="style">The style.</param>
        void IResourceManager<IStyle>.Add(IStyle style) => Add(style);

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
                _brushes.Add(style, brush, false);
            }
        }

        /// <summary>
        ///     Updates the specified <see cref="IStyle" /> in the manager, creating the <see cref="Brush" /> for it.
        /// </summary>
        /// <param name="style">The style.</param>
        /// <returns>The updated brush.</returns>
        public Brush Update(IStyle style)
        {
            if (style == null) throw new ArgumentNullException(nameof(style));

            lock (_lock)
            {
                if (_brushes == null) throw new ObjectDisposedException(nameof(DirectXResourceManager));
                return _brushes.AddOrUpdate(
                    style,
                    k => CreateBrush(k, false),
                    (k, b) =>
                    {
                        Resource<Brush> newBrush = CreateBrush(k, false);
                        b.Dispose();
                        return newBrush;
                    },
                    false).Value;
            }
        }

        /// <summary>
        ///     Updates the specified <see cref="IStyle" /> in the manager, creating the <see cref="Brush" /> for it.
        /// </summary>
        /// <param name="style">The style.</param>
        void IResourceManager<IStyle>.Update(IStyle style) => Update(style);

        /// <summary>
        ///     Updates the <see cref="Brush" /> for the specified <see cref="IStyle" /> in the manager.
        /// </summary>
        /// <param name="style">The style.</param>
        /// <param name="brush">The brush.</param>
        /// <exception cref="System.NotSupportedException">The type of the key and/or resource is not supported by this manager.</exception>
        /// <exception cref="System.ArgumentNullException"><paramref name="style" /> or <paramref name="brush" /> is null.</exception>
        public void Update(IStyle style, Brush brush)
        {
            if (style == null) throw new ArgumentNullException(nameof(style));
            if (brush == null) throw new ArgumentNullException(nameof(brush));

            lock (_lock)
            {
                if (_brushes == null) throw new ObjectDisposedException(nameof(DirectXResourceManager));
                _brushes.Update(style, brush, false);
            }
        }

        /// <summary>
        ///     Releases the specified <see cref="Brush" />.
        /// </summary>
        /// <param name="style">The style.</param>
        public void Release(IStyle style) => ReleaseRemove(style, true);

        /// <summary>
        ///     Removes the resource specified by the given key.
        /// </summary>
        /// <param name="style">The style.</param>
        public void Remove(IStyle style) => ReleaseRemove(style, false);

        /// <summary>
        ///     Releases or completely removes a resource.
        /// </summary>
        /// <param name="style">The style.</param>
        /// <param name="release">if set to <see langword="true" /> the resource will be released instead of completely removed.</param>
        private bool ReleaseRemove(IStyle style, bool release)
        {
            if (style == null) throw new ArgumentNullException(nameof(style));

            // If we are disposed, then technically we have already released the style
            if (_brushes == null)
                return true;
            lock (_lock)
            {
                if (_brushes == null)
                    return true;

                ImageStyle imageStyle = style as ImageStyle;
                if (imageStyle != null)
                    Release(imageStyle.Image);

                Resource<Brush> brush;
                switch (_brushes.Remove(style, out brush, release ? true : (bool?) null))
                {
                    case Removed.NotFound:
                        return false;
                    case Removed.Removed:
                        return true;
                    case Removed.RemovedLast:
                        Debug.Assert(brush != null, "brush != null");
                        brush.Dispose();
                        return true;
                    default:
                        Debug.Fail("Unexpected value");
                        throw new ArgumentOutOfRangeException();
                }
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
                if (_bitmaps.TryGetResource(image, out bitmap))
                {
                    Debug.Assert(bitmap != null, "bitmap != null");
                    return bitmap;
                }

                bitmap = CreateBitmap(image);
                _bitmaps.Add(image, bitmap, true);
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
                return _bitmaps.GetOrAdd(image, CreateBitmap, false);
            }
        }

        /// <summary>
        ///     Adds the specified <see cref="IImage" /> to the manager, creating the bitmap for it.
        /// </summary>
        /// <param name="image">The image.</param>
        void IResourceManager<IImage>.Add(IImage image) => Add(image);

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
                _bitmaps.Add(image, bitmap, false);
            }
        }

        /// <summary>
        ///     Updates the specified <see cref="IImage" /> in the manager, creating the <see cref="Bitmap" /> for it.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <returns>The updated brush.</returns>
        public Bitmap Update(IImage image)
        {
            if (image == null) throw new ArgumentNullException(nameof(image));

            lock (_lock)
            {
                if (_bitmaps == null) throw new ObjectDisposedException(nameof(DirectXResourceManager));
                return _bitmaps.AddOrUpdate(
                    image,
                    CreateBitmap,
                    (k, b) =>
                    {
                        Bitmap newBitmap = CreateBitmap(k);
                        b.Dispose();
                        return newBitmap;
                    },
                    false);
            }
        }

        /// <summary>
        ///     Updates the specified <see cref="IImage" /> in the manager, creating the <see cref="Bitmap" /> for it.
        /// </summary>
        /// <param name="image">The image.</param>
        void IResourceManager<IImage>.Update(IImage image) => Update(image);

        /// <summary>
        ///     Updates the <see cref="Bitmap" /> for the specified <see cref="IImage" /> in the manager.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="bitmap">The bitmap.</param>
        /// <exception cref="System.NotSupportedException">The type of the key and/or resource is not supported by this manager.</exception>
        /// <exception cref="System.ArgumentNullException"><paramref name="image" /> or <paramref name="bitmap" /> is null.</exception>
        public void Update(IImage image, Bitmap bitmap)
        {
            if (image == null) throw new ArgumentNullException(nameof(image));
            if (bitmap == null) throw new ArgumentNullException(nameof(bitmap));

            lock (_lock)
            {
                if (_bitmaps == null) throw new ObjectDisposedException(nameof(DirectXResourceManager));
                _bitmaps.Update(image, bitmap, false);
            }
        }

        /// <summary>
        ///     Releases the specified bitmap.
        /// </summary>
        /// <param name="image">The image.</param>
        public void Release(IImage image) => ReleaseRemove(image, true);

        /// <summary>
        ///     Removes the resource specified by the given key.
        /// </summary>
        /// <param name="image">The image</param>
        public void Remove(IImage image) => ReleaseRemove(image, false);

        /// <summary>
        ///     Releases or completely removes a resource.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="release">if set to <see langword="true" /> the resource will be released instead of completely removed.</param>
        private bool ReleaseRemove(IImage image, bool release)
        {
            if (image == null) throw new ArgumentNullException(nameof(image));

            // If we are disposed, then technically we have already released the style
            if (_brushes == null)
                return true;
            lock (_lock)
            {
                if (_brushes == null)
                    return true;

                Bitmap bitmap;
                switch (_bitmaps.Remove(image, out bitmap, release ? true : (bool?) null))
                {
                    case Removed.NotFound:
                        return false;
                    case Removed.Removed:
                        return true;
                    case Removed.RemovedLast:
                        Debug.Assert(bitmap != null, "bitmap != null");
                        bitmap.Dispose();
                        return true;
                    default:
                        Debug.Fail("Unexpected value");
                        throw new ArgumentOutOfRangeException();
                }
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
                ResourceDictionary<IStyle, Resource<Brush>> brushes = Interlocked.Exchange(ref _brushes, null);
                if (brushes != null)
                {
                    foreach (KeyValuePair<IStyle, Resource<Brush>> kvp in brushes)
                    {
                        Debug.Assert(kvp.Value != null, "kvp.Value != null");
                        Debug.Assert(kvp.Key != null, "kvp.Key != null");

                        kvp.Value.Dispose();
                    }
                }

                ResourceDictionary<IImage, Bitmap> bitmaps = Interlocked.Exchange(ref _bitmaps, null);
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