using System;
using JetBrains.Annotations;

namespace EscherTilier.Graphics
{
    /// <summary>
    ///     Readonly interface to a class that manages resources.
    /// </summary>
    public interface IResourceManager : IDisposable
    {
        /// <summary>
        ///     Gets the resource value for the specified key.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TResource">The type of the resource.</typeparam>
        /// <param name="key">The key.</param>
        /// <returns>The source.</returns>
        /// <exception cref="System.NotSupportedException">The type of the key and/or resource is not supported by this manager.</exception>
        /// <exception cref="System.Collections.Generic.KeyNotFoundException">
        ///     The manager did not contain a resource with the given
        ///     key.
        /// </exception>
        [NotNull]
        TResource Get<TKey, TResource>([NotNull] TKey key);

        /// <summary>
        ///     Adds the specified key and resource value to the manager.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TResource">The type of the resource.</typeparam>
        /// <param name="key">The key.</param>
        /// <param name="resoruce">The resoruce.</param>
        /// <exception cref="System.NotSupportedException">The type of the key and/or resource is not supported by this manager.</exception>
        void Add<TKey, TResource>(
            [NotNull] TKey key,
            [NotNull] TResource resoruce);

        /// <summary>
        ///     Releases the specified resource.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TResource">The type of the resource.</typeparam>
        /// <param name="key">The key.</param>
        /// <param name="resource">The resource.</param>
        /// <exception cref="System.NotSupportedException">The type of the key and/or resource is not supported by this manager.</exception>
        void Release<TKey, TResource>([NotNull] TKey key, [NotNull] TResource resource);
    }

    /// <summary>
    ///     Interface to a class that manages resources of a type mapped to keys of another type.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TResource">The type of the resource.</typeparam>
    public interface IResourceManager<in TKey, TResource> : IReadonlyResourceManager<TKey, TResource>,
        IResourceReleaser<TKey, TResource>
    {
        /// <summary>
        ///     Adds the specified key and resource value to the manager.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TResource">The type of the resource.</typeparam>
        /// <param name="key">The key.</param>
        /// <param name="resource">The resoruce.</param>
        void Add([NotNull] TKey key, [NotNull] TResource resource);
    }

    /// <summary>
    ///     Readonly interface to a class that manages resources of a type mapped to keys of another type.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TResource">The type of the resource.</typeparam>
    public interface IReadonlyResourceManager<in TKey, out TResource> : IDisposable
    {
        /// <summary>
        ///     Gets the resource value for the specified key.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <param name="key">The key.</param>
        /// <returns>The resource.</returns>
        /// <exception cref="System.Collections.Generic.KeyNotFoundException">
        ///     The manager did not contain a resource with the given
        ///     key.
        /// </exception>
        [NotNull]
        TResource Get(TKey key);
    }

    /// <summary>
    ///     Interface to a class that allows a resource to be released.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TResource">The type of the resource.</typeparam>
    public interface IResourceReleaser<in TKey, in TResource>
    {
        /// <summary>
        ///     Releases the specified resource.
        /// </summary>
        /// <param name="key">The key</param>
        /// <param name="resource">The resource.</param>
        void Release([NotNull] TKey key, [NotNull] TResource resource);
    }
}