using System;
using JetBrains.Annotations;

namespace EscherTiler.Graphics.Resources
{
    /// <summary>
    ///     Readonly interface to a class that manages resources of a type mapped to keys of another type.
    /// </summary>
    public interface IReadonlyResourceManager : IDisposable
    {
        /// <summary>
        ///     Gets the resource value for the specified key.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TResource">The type of the resource.</typeparam>
        /// <param name="key">The key.</param>
        /// <returns>The resource.</returns>
        /// <exception cref="System.NotSupportedException">The type of the key and/or resource is not supported by this manager.</exception>
        /// <exception cref="System.ArgumentNullException"><paramref name="key" /> is null.</exception>
        /// <exception cref="System.Collections.Generic.KeyNotFoundException">
        ///     The manager did not contain a resource with the given key.
        /// </exception>
        [NotNull]
        TResource Get<TKey, TResource>([NotNull] TKey key);

        /// <summary>
        ///     Releases the resource specified by the given key.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <param name="key">The key.</param>
        /// <exception cref="System.NotSupportedException">The type of the key and/or resource is not supported by this manager.</exception>
        /// <exception cref="System.ArgumentNullException"><paramref name="key" /> is null.</exception>
        void Release<TKey>([NotNull] TKey key);
    }

    /// <summary>
    ///     Readonly interface to a class that manages resources of a type mapped to keys of another type.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TResource">The type of the resource.</typeparam>
    public interface IReadonlyResourceManager<in TKey, out TResource> : IDisposable, IResourceReleaser<TKey>
    {
        /// <summary>
        ///     Gets the resource value for the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>The resource.</returns>
        /// <exception cref="System.NotSupportedException">The type of the key and/or resource is not supported by this manager.</exception>
        /// <exception cref="System.ArgumentNullException"><paramref name="key" /> is null.</exception>
        /// <exception cref="System.Collections.Generic.KeyNotFoundException">
        ///     The manager did not contain a resource with the given key.
        /// </exception>
        [NotNull]
        TResource Get([NotNull] TKey key);
    }
}