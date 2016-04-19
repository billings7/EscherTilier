using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using EscherTilier.Graphics.Resources;
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
        public IList<TileStyle> Styles => _styles;

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

        public void Add(TileStyle style)
        {
            _styles.Add(style);
        }

        [CanBeNull]
        public IResourceManager ResourceManager => _resourceManager;

        /// <summary>
        /// Gets the style for the given tile.
        /// </summary>
        /// <param name="tile">The tile.</param>
        /// <returns>The style for the tile.</returns>
        [CanBeNull]
        public IStyle GetStyle([NotNull] TileBase tile)
        {
            if (tile == null) throw new ArgumentNullException(nameof(tile));
            return GetStyle(tile, _styles.Where(s => s.Shapes.Contains(tile.Shape)).Select(s => s.Style).ToArray());
        }

        /// <summary>
        /// Gets the style for the given tile.
        /// </summary>
        /// <param name="tile">The tile.</param>
        /// <param name="styles">The styles to choose from.</param>
        /// <returns></returns>
        [CanBeNull]
        protected abstract IStyle GetStyle([NotNull] TileBase tile, [NotNull] IStyle[] styles);

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
            GC.SuppressFinalize(this);
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