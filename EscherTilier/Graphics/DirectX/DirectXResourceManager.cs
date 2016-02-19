using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using EscherTilier.Styles;
using EscherTilier.Utilities;
using JetBrains.Annotations;
using SharpDX.Direct2D1;
using FactoryD2D = SharpDX.Direct2D1.Factory;
using FactoryWrite = SharpDX.DirectWrite.Factory;

namespace EscherTilier.Graphics.DirectX
{
    internal class DirectXResourceManager : IResourceManager
    {
        [NotNull]
        public static readonly FactoryD2D FactoryD2D = new FactoryD2D(FactoryType.MultiThreaded);

        [NotNull]
        private static readonly FactoryWrite _factoryWrite = new FactoryWrite(SharpDX.DirectWrite.FactoryType.Shared);

        /// <summary>
        ///     Creates an empty <see cref="PathGeometry" />.
        /// </summary>
        [NotNull]
        public static PathGeometry CreatePathGeometry() => new PathGeometry(FactoryD2D);

        /// <summary>
        /// Creates a stroke style.
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

        public DirectXResourceManager([NotNull] RenderTarget renderTarget)
        {
            if (renderTarget == null) throw new ArgumentNullException(nameof(renderTarget));
            _renderTarget = renderTarget;
        }

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

                    Dictionary<IStyle, Brush> brushes = Interlocked.Exchange(
                        ref _brushes,
                        new Dictionary<IStyle, Brush>());
                    Debug.Assert(brushes != null, "brushes != null");

                    foreach (KeyValuePair<IStyle, Brush> kvp in brushes)
                    {
                        Debug.Assert(kvp.Value != null, "kvp.Value != null");
                        Debug.Assert(kvp.Key != null, "kvp.Key != null");

                        kvp.Value.Dispose();
                    }
                }
            }
        }

        [NotNull]
        private Brush CreateBrush([NotNull] IStyle style) => CreateBrush(style, _renderTarget);

        [NotNull]
        public static Brush CreateBrush([NotNull] IStyle style, [NotNull] RenderTarget renderTarget)
        {
            SolidColourStyle solidColour = style as SolidColourStyle;
            if (solidColour != null)
            {
                return new SolidColorBrush(renderTarget, solidColour.Colour.ToRawColor4());
            }

            throw new NotImplementedException();
        }

        [NotNull]
        public Brush GetBrush([NotNull] IStyle style)
        {
            if (style == null) throw new ArgumentNullException(nameof(style));

            lock (_lock)
            {
                if (_brushes == null) throw new ObjectDisposedException(nameof(DirectXResourceManager));
                // ReSharper disable once AssignNullToNotNullAttribute
                return _brushes.GetOrAdd(style, CreateBrush);
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            lock (_lock)
            {
                Dictionary<IStyle, Brush> brushes = Interlocked.Exchange(
                    ref _brushes,
                    null);
                Debug.Assert(brushes != null, "brushes != null");

                foreach (KeyValuePair<IStyle, Brush> kvp in brushes)
                {
                    Debug.Assert(kvp.Value != null, "kvp.Value != null");
                    Debug.Assert(kvp.Key != null, "kvp.Key != null");

                    kvp.Value.Dispose();
                }
            }
        }
    }
}