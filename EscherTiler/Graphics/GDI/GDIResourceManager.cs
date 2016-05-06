using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Threading;
using EscherTiler.Graphics.Resources;
using EscherTiler.Styles;
using JetBrains.Annotations;

namespace EscherTiler.Graphics.GDI
{
    /// <summary>
    ///     Resource manager for GDI+ related objects.
    /// </summary>
    /// <seealso cref="ResourceManager" />
    /// <seealso cref="IResourceManager{IStyle, Brush}" />
    /// <seealso cref="IResourceManager{IImage, Bitmap}" />
    public class GDIResourceManager : ResourceManager,
        IResourceManager<IStyle, Brush>,
        IResourceManager<LineStyle, Pen>,
        IResourceManager<IImage, Bitmap>
    {
        [NotNull]
        private readonly object _lock = new object();

        [CanBeNull]
        private ResourceDictionary<IStyle, Resource<Brush>> _brushes =
            new ResourceDictionary<IStyle, Resource<Brush>>();

        [CanBeNull]
        private ResourceDictionary<LineStyle, Resource<Pen>> _pens =
            new ResourceDictionary<LineStyle, Resource<Pen>>();

        [CanBeNull]
        private ResourceDictionary<IImage, Bitmap> _bitmaps =
            new ResourceDictionary<IImage, Bitmap>();

        /// <summary>
        ///     Initializes a new instance of the <see cref="GDIResourceManager" /> class.
        /// </summary>
        /// <param name="styleManager">The style manager to initialise the resources with.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public GDIResourceManager([CanBeNull] StyleManager styleManager = null)
        {
            if (styleManager != null)
                AddFromStyleManager(styleManager);
        }

        /// <summary>
        ///     Creates a <see cref="Brush" /> from an <see cref="IStyle" />.
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
                    return new SolidBrush(solidColour.Colour.ToColor());

                RandomColourStyle randomColour = style as RandomColourStyle;
                if (randomColour != null)
                    return new SolidBrush(randomColour.PositionColour.ToColor());

                LinearGradientStyle linearGradient = style as LinearGradientStyle;
                if (linearGradient != null)
                {
                    ColorBlend blend = new ColorBlend(linearGradient.GradientStops.Count);

                    Debug.Assert(blend.Colors != null, "blend.Colors != null");
                    Debug.Assert(blend.Positions != null, "blend.Positions != null");

                    for (int i = 0; i < linearGradient.GradientStops.Count; i++)
                    {
                        GradientStop stop = linearGradient.GradientStops[i];

                        blend.Colors[i] = stop.Colour.ToColor();
                        blend.Positions[i] = stop.Position;
                    }

                    return new LinearGradientBrush(
                        linearGradient.Start.ToPointF(),
                        linearGradient.End.ToPointF(),
                        Color.Black,
                        Color.White)
                    {
                        InterpolationColors = blend,
                        WrapMode = WrapMode.Clamp
                    };
                }

                RadialGradientStyle radialGradient = style as RadialGradientStyle;
                if (radialGradient != null)
                {
                    ColorBlend blend = new ColorBlend(radialGradient.GradientStops.Count);

                    Debug.Assert(blend.Colors != null, "blend.Colors != null");
                    Debug.Assert(blend.Positions != null, "blend.Positions != null");

                    for (int i = 0; i < radialGradient.GradientStops.Count; i++)
                    {
                        GradientStop stop = radialGradient.GradientStops[i];

                        blend.Colors[i] = stop.Colour.ToColor();
                        blend.Positions[i] = stop.Position;
                    }

                    GraphicsPath path = new GraphicsPath();
                    path.AddEllipse(-1, -1, 1, 1);
                    return new Resource<Brush>(
                        new PathGradientBrush(path)
                        {
                            WrapMode = WrapMode.Clamp,
                            Transform = radialGradient.GradientTransform.ToMatrix(),
                            InterpolationColors = blend,
                            CenterPoint = radialGradient.UnitOriginOffset.ToPointF()
                        },
                        path);
                }

                ImageStyle image = style as ImageStyle;
                if (image != null)
                {
                    Bitmap bitmap = temp ? Get(image.Image) : Add(image.Image);
                    return new TextureBrush(bitmap, WrapMode.Clamp)
                    {
                        Transform = image.ImageTransform.ToMatrix()
                    };
                }

                throw new NotSupportedException();
            }
        }

        /// <summary>
        ///     Creates a <see cref="Pen" /> from a <see cref="LineStyle" />.
        /// </summary>
        /// <param name="style">The style.</param>
        /// <param name="temp">if set to <see langword="true" /> the brush is temporary.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        [NotNull]
        private Resource<Pen> CreatePen([NotNull] LineStyle style, bool temp)
        {
            lock (_lock)
            {
                Brush brush = temp ? Get(style.Style) : Add(style.Style);
                return new Pen(brush, style.Width)
                {
                    LineJoin = LineJoin.Round,
                    StartCap = LineCap.Round,
                    EndCap = LineCap.Round
                };
            }
        }

        /// <summary>
        ///     Creates a <see cref="Bitmap" /> from an <see cref="IImage" />.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <returns></returns>
        [NotNull]
        private Bitmap CreateBitmap([NotNull] IImage image)
        {
            using (Stream stream = image.GetStream())
                return (Bitmap) Image.FromStream(stream);
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

        #region Brushes

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
                if (_brushes == null) throw new ObjectDisposedException(nameof(GDIResourceManager));
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
        ///     Adds the specified <see cref="IStyle" /> to the manager, creating the <see cref="Brush" /> for it.
        /// </summary>
        /// <param name="style">The style.</param>
        /// <returns>The created brush.</returns>
        [NotNull]
        public Brush Add([NotNull] IStyle style)
        {
            if (style == null) throw new ArgumentNullException(nameof(style));

            lock (_lock)
            {
                if (_brushes == null) throw new ObjectDisposedException(nameof(GDIResourceManager));
                return _brushes.GetOrAdd(style, k => CreateBrush(k, false), false).Value;
            }
        }

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
                if (_brushes == null) throw new ObjectDisposedException(nameof(GDIResourceManager));
                _brushes.Add(style, brush, false);
            }
        }

        /// <summary>
        ///     Updates the specified <see cref="IStyle" /> in the manager, creating the <see cref="Brush" /> for it.
        /// </summary>
        /// <param name="style">The style.</param>
        /// <returns>The updated brush.</returns>
        [NotNull]
        public Brush Update([NotNull] IStyle style)
        {
            if (style == null) throw new ArgumentNullException(nameof(style));

            lock (_lock)
            {
                if (_brushes == null) throw new ObjectDisposedException(nameof(GDIResourceManager));
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

        void IResourceManager<IStyle>.Update(IStyle key) => Update(key);

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
                if (_brushes == null) throw new ObjectDisposedException(nameof(GDIResourceManager));
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

            if (_brushes == null)
                throw new ObjectDisposedException(nameof(GDIResourceManager));
            lock (_lock)
            {
                if (_brushes == null)
                    throw new ObjectDisposedException(nameof(GDIResourceManager));

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

        #endregion

        #region Pens

        /// <summary>
        ///     Gets the pen for the specified style.
        /// </summary>
        /// <param name="style">The style.</param>
        /// <returns>The pen.</returns>
        /// <exception cref="System.Collections.Generic.KeyNotFoundException">
        ///     The manager did not contain a resource with the given
        ///     key.
        /// </exception>
        public Pen Get(LineStyle style)
        {
            if (style == null) throw new ArgumentNullException(nameof(style));

            lock (_lock)
            {
                if (_pens == null) throw new ObjectDisposedException(nameof(GDIResourceManager));
                Resource<Pen> pen;
                if (_pens.TryGetResource(style, out pen))
                {
                    Debug.Assert(pen != null, "pen != null");
                    return pen.Value;
                }

                pen = CreatePen(style, true);
                _pens.Add(style, pen, true);
                return pen.Value;
            }
        }

        /// <summary>
        ///     Adds the specified <see cref="LineStyle" /> to the manager, creating the <see cref="Pen" /> for it.
        /// </summary>
        /// <param name="style">The style.</param>
        /// <returns>The created pen.</returns>
        [NotNull]
        public Pen Add([NotNull] LineStyle style)
        {
            if (style == null) throw new ArgumentNullException(nameof(style));

            lock (_lock)
            {
                if (_pens == null) throw new ObjectDisposedException(nameof(GDIResourceManager));
                return _pens.GetOrAdd(style, k => CreatePen(k, false), false).Value;
            }
        }

        void IResourceManager<LineStyle>.Add(LineStyle style) => Add(style);

        /// <summary>
        ///     Adds the specified <see cref="LineStyle" /> to <see cref="Pen" /> resource mapping to the manager.
        /// </summary>
        /// <param name="style">The style.</param>
        /// <param name="pen">The pen.</param>
        public void Add(LineStyle style, Pen pen)
        {
            if (style == null) throw new ArgumentNullException(nameof(style));
            if (pen == null) throw new ArgumentNullException(nameof(pen));

            lock (_lock)
            {
                if (_pens == null) throw new ObjectDisposedException(nameof(GDIResourceManager));
                _pens.Add(style, pen, false);
            }
        }

        /// <summary>
        ///     Updates the specified <see cref="LineStyle" /> in the manager, creating the <see cref="Pen" /> for it.
        /// </summary>
        /// <param name="style">The style.</param>
        /// <returns>The updated brush.</returns>
        [NotNull]
        public Pen Update([NotNull] LineStyle style)
        {
            if (style == null) throw new ArgumentNullException(nameof(style));

            lock (_lock)
            {
                if (_pens == null) throw new ObjectDisposedException(nameof(GDIResourceManager));
                return _pens.AddOrUpdate(
                    style,
                    k => CreatePen(k, false),
                    (k, b) =>
                    {
                        Resource<Pen> newPen = CreatePen(k, false);
                        b.Dispose();
                        return newPen;
                    },
                    false).Value;
            }
        }

        void IResourceManager<LineStyle>.Update(LineStyle key) => Update(key);

        /// <summary>
        ///     Updates the <see cref="Pen" /> for the specified <see cref="LineStyle" /> in the manager.
        /// </summary>
        /// <param name="style">The key.</param>
        /// <param name="pen">The pen.</param>
        /// <exception cref="System.NotSupportedException">The type of the key and/or resource is not supported by this manager.</exception>
        /// <exception cref="System.ArgumentNullException"><paramref name="style" /> or <paramref name="pen" /> is null.</exception>
        public void Update(LineStyle style, Pen pen)
        {
            if (style == null) throw new ArgumentNullException(nameof(style));
            if (pen == null) throw new ArgumentNullException(nameof(pen));

            lock (_lock)
            {
                if (_pens == null) throw new ObjectDisposedException(nameof(GDIResourceManager));
                _pens.Update(style, pen, false);
            }
        }

        /// <summary>
        ///     Releases the specified <see cref="LineStyle" />.
        /// </summary>
        /// <param name="style">The style.</param>
        public void Release(LineStyle style) => ReleaseRemove(style, true);

        /// <summary>
        ///     Removes the resource specified by the given key.
        /// </summary>
        /// <param name="style">The style.</param>
        public void Remove(LineStyle style) => ReleaseRemove(style, false);

        /// <summary>
        ///     Releases or completely removes a resource.
        /// </summary>
        /// <param name="style">The style.</param>
        /// <param name="release">if set to <see langword="true" /> the resource will be released instead of completely removed.</param>
        private bool ReleaseRemove(LineStyle style, bool release)
        {
            if (style == null) throw new ArgumentNullException(nameof(style));

            if (_pens == null)
                throw new ObjectDisposedException(nameof(GDIResourceManager));
            lock (_lock)
            {
                if (_pens == null)
                    throw new ObjectDisposedException(nameof(GDIResourceManager));

                Release(style.Style);

                Resource<Pen> pen;
                switch (_pens.Remove(style, out pen, release ? true : (bool?) null))
                {
                    case Removed.NotFound:
                        return false;
                    case Removed.Removed:
                        return true;
                    case Removed.RemovedLast:
                        Debug.Assert(pen != null, "pen != null");
                        pen.Dispose();
                        return true;
                    default:
                        Debug.Fail("Unexpected value");
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        #endregion

        #region Images

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
                if (_bitmaps == null) throw new ObjectDisposedException(nameof(GDIResourceManager));
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
        ///     Adds the specified <see cref="IImage" /> to the manager, creating the <see cref="Bitmap" /> for it.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <returns>The created bitmap.</returns>
        [NotNull]
        public Bitmap Add([NotNull] IImage image)
        {
            if (image == null) throw new ArgumentNullException(nameof(image));

            lock (_lock)
            {
                if (_bitmaps == null) throw new ObjectDisposedException(nameof(GDIResourceManager));
                return _bitmaps.GetOrAdd(image, CreateBitmap, false);
            }
        }

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
                if (_bitmaps == null) throw new ObjectDisposedException(nameof(GDIResourceManager));
                _bitmaps.Add(image, bitmap, false);
            }
        }

        /// <summary>
        ///     Updates the specified <see cref="IImage" /> in the manager, creating the <see cref="Bitmap" /> for it.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <returns>The updated brush.</returns>
        [NotNull]
        public Bitmap Update([NotNull] IImage image)
        {
            if (image == null) throw new ArgumentNullException(nameof(image));

            lock (_lock)
            {
                if (_bitmaps == null) throw new ObjectDisposedException(nameof(GDIResourceManager));
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

        void IResourceManager<IImage>.Update(IImage key) => Update(key);

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
                if (_bitmaps == null) throw new ObjectDisposedException(nameof(GDIResourceManager));
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

            if (_bitmaps == null)
                throw new ObjectDisposedException(nameof(GDIResourceManager));
            lock (_lock)
            {
                if (_bitmaps == null)
                    throw new ObjectDisposedException(nameof(GDIResourceManager));

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

        #endregion

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

                ResourceDictionary<LineStyle, Resource<Pen>> pens = Interlocked.Exchange(ref _pens, null);
                if (pens != null)
                {
                    foreach (KeyValuePair<LineStyle, Resource<Pen>> kvp in pens)
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