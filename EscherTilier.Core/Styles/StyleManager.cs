using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using EscherTilier.Graphics;
using JetBrains.Annotations;

namespace EscherTilier.Styles
{
    public abstract class StyleManager : IDisposable
    {
        // TODO Use DI instead?
        public static StyleManager CreateManager()
        {
            throw new NotImplementedException();
        }

        [NotNull]
        [ItemNotNull]
        public IReadOnlyList<TileStyle> Styles
        {
            get { return _styles; }
        }

        [NotNull]
        public LineStyle LineStyle { get; set; }

        [CanBeNull]
        private IResourceManager _resourceManager;

        [NotNull]
        private readonly List<TileStyle> _styles = new List<TileStyle>();

        protected StyleManager([CanBeNull] IResourceManager resourceManager)
        {
            _resourceManager = resourceManager;
        }

        public void AddStyle(TileStyle style)
        {
            _styles.Add(style);
        }

        [CanBeNull]
        public T ResourceManager<T>() where T : IResourceManager => (T)_resourceManager;
        
        public IStyle GetStyle(ITile tile)
            => GetStyle(tile, Styles.Where(s => s.Shapes.Contains(tile.Shape)).Select(s => s.Style).ToArray());

        protected abstract IStyle GetStyle(ITile tile, IStyle[] styles);

        /// <summary>
        ///     Finalizes an instance of the <see cref="StyleManager" /> class.
        /// </summary>
        ~StyleManager()
        {
            Dispose(false);
        }

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        ///     Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing">
        ///     <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only
        ///     unmanaged resources.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
                Interlocked.Exchange(ref _resourceManager, null)?.Dispose();
        }
    }
}