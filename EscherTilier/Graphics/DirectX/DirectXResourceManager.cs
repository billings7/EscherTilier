using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using EscherTilier.Graphics.Resources;
using EscherTilier.Styles;
using JetBrains.Annotations;
using SharpDX.Direct2D1;
using SharpDX.Mathematics.Interop;
using SharpDX.WIC;
using Bitmap = SharpDX.Direct2D1.Bitmap;
using BitmapInterpolationMode = SharpDX.Direct2D1.BitmapInterpolationMode;
using FactoryD2D = SharpDX.Direct2D1.Factory;
using FactoryWrite = SharpDX.DirectWrite.Factory;

namespace EscherTilier.Graphics.DirectX
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
                    Debug.Assert(_brushes != null);

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

            if (_brushes == null)
                throw new ObjectDisposedException(nameof(DirectXResourceManager));
            lock (_lock)
            {
                if (_brushes == null)
                    throw new ObjectDisposedException(nameof(DirectXResourceManager));

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

            if (_bitmaps == null)
                throw new ObjectDisposedException(nameof(DirectXResourceManager));
            lock (_lock)
            {
                if (_bitmaps == null)
                    throw new ObjectDisposedException(nameof(DirectXResourceManager));

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

        /// <summary>
        ///     Stores a resource optionally with extra resources that should be disposed of along with the main one.
        /// </summary>
        /// <typeparam name="T">The type of the resource.</typeparam>
        private class Resource<T> : IDisposable
        {
            private T _value;

            private IDisposable[] _disposables;

            /// <summary>
            ///     Initializes a new instance of the <see cref="Resource{T}" /> class.
            /// </summary>
            /// <param name="value">The value.</param>
            /// <param name="disposables">The disposables.</param>
            public Resource([NotNull] T value, [NotNull] params IDisposable[] disposables)
            {
                Debug.Assert(value != null, "disposables != null");
                Debug.Assert(disposables != null, "disposables != null");
                _value = value;
                _disposables = disposables.Length > 0 ? disposables : null;
            }

            /// <summary>
            ///     Gets the value.
            /// </summary>
            /// <value>
            ///     The value.
            /// </value>
            [NotNull]
            public T Value
            {
                get
                {
                    Debug.Assert(_value != null);
                    return _value;
                }
            }

            /// <summary>
            ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
            /// </summary>
            public void Dispose()
            {
                T val = _value;
                _value = default(T);

                (val as IDisposable)?.Dispose();

                IDisposable[] disps = Interlocked.Exchange(ref _disposables, null);
                if (disps == null) return;

                for (int i = 0; i < disps.Length; i++)
                {
                    disps[i]?.Dispose();
                    disps[i] = null;
                }
            }

            /// <summary>
            ///     Performs an implicit conversion from <see cref="T" /> to <see cref="Resource{T}" />.
            /// </summary>
            /// <param name="value">The value.</param>
            /// <returns>
            ///     The result of the conversion.
            /// </returns>
            public static implicit operator Resource<T>([NotNull] T value) => new Resource<T>(value);
        }

        /// <summary>
        ///     Dictionary for storing resources.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TResource">The type of the resource.</typeparam>
        /// <seealso cref="System.Collections.Generic.IEnumerable{KeyValuePair{TKey, TResource}}" />
        // TODO Make public in .Core
        private class ResourceDictionary<TKey, TResource> : IEnumerable<KeyValuePair<TKey, TResource>>
        {
            [NotNull]
            private readonly Dictionary<TKey, Wrapper> _resources;

            [NotNull]
            private readonly Dictionary<TResource, Count> _resourceCount;

            /// <summary>
            ///     A wrapper around a resource.
            /// </summary>
            /// <seealso cref="IEnumerable{KeyValuePair{TKey, TResource}}" />
            private struct Wrapper
            {
                public readonly bool IsTemporary;

                private readonly TResource _resource;

                /// <summary>
                ///     Gets the resource.
                /// </summary>
                /// <value>
                ///     The resource.
                /// </value>
                [NotNull]
                public TResource Resource
                {
                    get
                    {
                        Debug.Assert(_resource != null, "_resource != null");
                        return _resource;
                    }
                }

                /// <summary>
                ///     Initializes a new instance of the <see cref="Wrapper" /> struct.
                /// </summary>
                /// <param name="resource">The resource.</param>
                /// <param name="isTemp">if set to <see langword="true" /> the resourec is only temporary.</param>
                public Wrapper([NotNull] TResource resource, bool isTemp)
                {
                    Debug.Assert(resource != null, "resource != null");
                    _resource = resource;
                    IsTemporary = isTemp;
                }
            }

            /// <summary>
            ///     Stores a permenant and temporary resource count.
            /// </summary>
            private class Count
            {
                /// <summary>
                ///     The number of 'permenant' (non-temporary) resources.
                /// </summary>
                public int PermCount;

                /// <summary>
                ///     The number of temporary resources.
                /// </summary>
                public int TempCount;

                /// <summary>
                ///     Gets the total number of resources.
                /// </summary>
                public int Total => PermCount + TempCount;

                /// <summary>
                ///     Increments the count.
                /// </summary>
                /// <param name="temp">
                ///     if set to <see langword="true" /> increment the <see cref="TempCount" />;
                ///     otherwise increment the <see cref="PermCount" />.
                /// </param>
                public void Inc(bool temp)
                {
                    if (temp) TempCount++;
                    else PermCount++;
                }

                /// <summary>
                ///     Decrements the count.
                /// </summary>
                /// <param name="temp">
                ///     if set to <see langword="true" /> decrement the <see cref="TempCount" />;
                ///     otherwise decrement the <see cref="PermCount" />.
                /// </param>
                public void Dec(bool temp)
                {
                    if (temp) TempCount--;
                    else PermCount--;
                    Debug.Assert(TempCount >= 0);
                    Debug.Assert(PermCount >= 0);
                }

                /// <summary>
                ///     Determines if <see cref="PermCount" /> or <see cref="TempCount" />, depending on <paramref name="temp" />, is
                ///     greater than zero.
                /// </summary>
                /// <param name="temp">
                ///     if set to <see langword="true" /> check <see cref="TempCount" />; otherwise check
                ///     <see cref="PermCount" />.
                /// </param>
                /// <returns></returns>
                public bool Any(bool temp) => temp ? TempCount > 0 : PermCount > 0;

                /// <summary>
                ///     Performs an implicit conversion from <see cref="Count" /> to <see cref="System.Int32" />.
                /// </summary>
                /// <param name="c">The c.</param>
                /// <returns>
                ///     The result of the conversion.
                /// </returns>
                public static implicit operator int(Count c) => c.Total;
            }

            /// <summary>
            ///     Initializes a new instance of the <see cref="ResourceDictionary{TKey, TResource}" /> class.
            /// </summary>
            /// <param name="keyComparer">The key comparer.</param>
            /// <param name="resourceComparer">The resource comparer.</param>
            public ResourceDictionary(
                IEqualityComparer<TKey> keyComparer = null,
                IEqualityComparer<TResource> resourceComparer = null)
            {
                keyComparer = keyComparer ?? EqualityComparer<TKey>.Default;
                resourceComparer = resourceComparer ?? EqualityComparer<TResource>.Default;

                _resources = new Dictionary<TKey, Wrapper>(keyComparer);
                _resourceCount = new Dictionary<TResource, Count>(resourceComparer);
            }

            /// <summary>
            ///     Adds a resource for a key.
            /// </summary>
            /// <param name="key">The key.</param>
            /// <param name="resource">The resource.</param>
            /// <param name="temp">if set to <see langword="true" /> the resource is only temporary.</param>
            public void Add([NotNull] TKey key, [NotNull] TResource resource, bool temp)
            {
                Debug.Assert(key != null, "key != null");
                Debug.Assert(resource != null, "resource != null");

                _resources.Add(key, new Wrapper(resource, temp));

                Count count;
                bool got = _resourceCount.TryGetValue(resource, out count);
                Debug.Assert(!got || count > 0);

                if (count == null)
                    _resourceCount.Add(resource, count = new Count());

                count.Inc(temp);
            }

            /// <summary>
            ///     Attempts to get the resource for the specified key.
            /// </summary>
            /// <param name="key">The key.</param>
            /// <param name="resource">The resource.</param>
            /// <param name="temp">
            ///     If <see langword="true" /> get only if temporary, if <see langword="false" /> get only if not
            ///     temporary, if <see langword="null" /> get any.
            /// </param>
            /// <returns><see langword="true" /> if the resource was found; otherwise <see langword="false" />.</returns>
            public bool TryGetResource([NotNull] TKey key, [CanBeNull] out TResource resource, bool? temp = null)
            {
                Debug.Assert(key != null, "key != null");

                Wrapper wrapper;
                if (_resources.TryGetValue(key, out wrapper))
                {
                    if (temp == null || temp == wrapper.IsTemporary)
                    {
                        resource = wrapper.Resource;
                        return true;
                    }
                }

                resource = default(TResource);
                return false;
            }

            /// <summary>
            ///     Determines whether the dictionary contains the specified key.
            /// </summary>
            /// <param name="key">The key.</param>
            /// <param name="temp">
            ///     If <see langword="true" /> only count temporary resources, if <see langword="false" /> only count non-readonly
            ///     resources, if <see langword="null" /> count any.
            /// </param>
            /// <returns><see langword="true" /> if the key was found; otherwise <see langword="false" />.</returns>
            public bool ContainsKey(TKey key, bool? temp = null)
            {
                Debug.Assert(key != null, "key != null");

                if (temp == null)
                    return _resources.ContainsKey(key);

                Wrapper wrapper;
                return _resources.TryGetValue(key, out wrapper) && wrapper.IsTemporary == temp;
            }

            /// <summary>
            ///     Determines whether the dictionary contains the specified resource.
            /// </summary>
            /// <param name="resource">The resource.</param>
            /// <param name="temp">
            ///     If <see langword="true" /> only count temporary resources, if <see langword="false" /> only count non-readonly
            ///     resources, if <see langword="null" /> count any.
            /// </param>
            /// <returns><see langword="true" /> if the resource was found; otherwise <see langword="false" />.</returns>
            public bool ContainsResource(TResource resource, bool? temp = null)
            {
                Debug.Assert(resource != null, "resource != null");

                if (temp == null)
                    return _resourceCount.ContainsKey(resource);

                Count count;
                if (!_resourceCount.TryGetValue(resource, out count)) return false;

                Debug.Assert(count != null, "count != null");
                return count.Any(temp.Value);
            }

            /// <summary>
            ///     Removes and returns the resource with the specified key from the dictionary.
            /// </summary>
            /// <param name="key">The key.</param>
            /// <param name="resource">The resource.</param>
            /// <param name="temp">
            ///     If <see langword="true" /> only remove temporary resources, if <see langword="false" /> only remove non-readonly
            ///     resources, if <see langword="null" /> remove any.
            /// </param>
            /// <returns>Whether or not the resource was removed, and if it was the last copy of the resource in the dictionary.</returns>
            public Removed Remove(TKey key, out TResource resource, bool? temp = null)
            {
                Debug.Assert(key != null, "key != null");

                Wrapper wrapper;
                if (!_resources.TryGetValue(key, out wrapper))
                {
                    resource = default(TResource);
                    return Removed.NotFound;
                }

                if (temp != null && temp != wrapper.IsTemporary)
                {
                    resource = default(TResource);
                    return Removed.NotFound;
                }

                _resources.Remove(key);

                resource = wrapper.Resource;

                Count count = _resourceCount[wrapper.Resource];
                Debug.Assert(count != null, "count != null");
                count.Dec(wrapper.IsTemporary);
                if (count.Total == 0)
                {
                    _resourceCount.Remove(wrapper.Resource);
                    return Removed.RemovedLast;
                }

                return Removed.Removed;
            }

            /// <summary>
            ///     Gets the resource for the specified key, adding it if it doesnt yet exist.
            /// </summary>
            /// <param name="key">The key.</param>
            /// <param name="factory">The factory delegate for creating the resource.</param>
            /// <param name="temp">if set to <see langword="true" /> the resource should only be temporary.</param>
            /// <returns>The resource.</returns>
            [NotNull]
            public TResource GetOrAdd(TKey key, Func<TKey, TResource> factory, bool temp)
            {
                Debug.Assert(key != null, "key != null");
                Debug.Assert(factory != null, "factory != null");

                TResource resource;
                if (TryGetResource(key, out resource))
                {
                    Debug.Assert(resource != null, "resource != null");
                    return resource;
                }

                resource = factory(key);
                Debug.Assert(resource != null, "resource != null");

                Add(key, resource, temp);
                return resource;
            }

            /// <summary>
            ///     Returns an enumerator that iterates through the collection.
            /// </summary>
            /// <returns>
            ///     An enumerator that can be used to iterate through the collection.
            /// </returns>
            public IEnumerator<KeyValuePair<TKey, TResource>> GetEnumerator()
            {
                foreach (KeyValuePair<TKey, Wrapper> kvp in _resources)
                    yield return new KeyValuePair<TKey, TResource>(kvp.Key, kvp.Value.Resource);
            }

            /// <summary>
            ///     Returns an enumerator that iterates through a collection.
            /// </summary>
            /// <returns>
            ///     An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
            /// </returns>
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        /// <summary>
        ///     Possible return values from <see cref="ResourceDictionary{TKey,TResource}.Remove" />
        /// </summary>
        private enum Removed : byte
        {
            /// <summary>
            ///     The resource was not found.
            /// </summary>
            NotFound,

            /// <summary>
            ///     The resource was removed for the key, but still exists for other keys.
            /// </summary>
            Removed,

            /// <summary>
            ///     The resource was removed for the key and no other keys map to it.
            /// </summary>
            RemovedLast
        }
    }
}