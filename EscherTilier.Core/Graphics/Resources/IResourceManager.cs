using JetBrains.Annotations;

namespace EscherTilier.Graphics.Resources
{
    /// <summary>
    ///     Readonly interface to a class that manages resources.
    /// </summary>
    public interface IResourceManager : IReadonlyResourceManager
    {
        /// <summary>
        ///     Adds the specified key and resource value to the manager.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TResource">The type of the resource.</typeparam>
        /// <param name="key">The key.</param>
        /// <param name="resource">The resource.</param>
        /// <exception cref="System.NotSupportedException">The type of the key and/or resource is not supported by this manager.</exception>
        /// <exception cref="System.ArgumentNullException"><paramref name="key" /> or <paramref name="resource" /> is null.</exception>
        void Add<TKey, TResource>(
            [NotNull] TKey key,
            [NotNull] TResource resource);

        /// <summary>
        ///     Removes the resource specified by the given key.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <param name="key">The key.</param>
        /// <exception cref="System.NotSupportedException">The type of the key and/or resource is not supported by this manager.</exception>
        /// <exception cref="System.ArgumentNullException"><paramref name="key" /> is null.</exception>
        void Remove<TKey>([NotNull] TKey key);
    }

    /// <summary>
    ///     Interface to a class that manages resources of a type mapped to keys of another type.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TResource">The type of the resource.</typeparam>
    public interface IResourceManager<in TKey, TResource> : IReadonlyResourceManager<TKey, TResource>,
        IResourceRemover<TKey>
    {
        /// <summary>
        ///     Adds the specified key and resource value to the manager.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TResource">The type of the resource.</typeparam>
        /// <param name="key">The key.</param>
        /// <param name="resource">The resource.</param>
        /// <exception cref="System.NotSupportedException">The type of the key and/or resource is not supported by this manager.</exception>
        /// <exception cref="System.ArgumentNullException"><paramref name="key" /> or <paramref name="resource" /> is null.</exception>
        void Add([NotNull] TKey key, [NotNull] TResource resource);
    }
}