using System;

namespace EscherTilier.Graphics
{
    /// <summary>
    ///     Base class for managing resources.
    /// </summary>
    public abstract class ResourceManager : IResourceManager
    {
        /// <summary>
        ///     Adds the specified key and resource value to the manager.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TResource">The type of the resource.</typeparam>
        /// <param name="key">The key.</param>
        /// <param name="resoruce">The resoruce.</param>
        /// <exception cref="System.NotSupportedException">The type of the key and/or resource is not supported by this manager.</exception>
        public void Add<TKey, TResource>(TKey key, TResource resoruce)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            if (resoruce == null) throw new ArgumentNullException(nameof(resoruce));

            IResourceManager<TKey, TResource> rm = this as IResourceManager<TKey, TResource>;
            if (rm == null)
                throw new NotSupportedException(Strings.ResourceManager_TypesNotSupported);
            rm.Add(key, resoruce);
        }

        /// <summary>
        ///     Gets the resource value for the specified key.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TResource">The type of the resource.</typeparam>
        /// <param name="key">The key.</param>
        /// <returns>The resource.</returns>
        /// <exception cref="System.NotSupportedException">The type of the key and/or resource is not supported by this manager.</exception>
        /// <exception cref="System.Collections.Generic.KeyNotFoundException">
        ///     The manager did not contain a resource with the given
        ///     key.
        /// </exception>
        public TResource Get<TKey, TResource>(TKey key)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));

            IReadonlyResourceManager<TKey, TResource> rm = this as IReadonlyResourceManager<TKey, TResource>;
            if (rm == null)
                throw new NotSupportedException(Strings.ResourceManager_TypesNotSupported);
            return rm.Get(key);
        }

        /// <summary>
        ///     Releases the specified resource.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TResource">The type of the resource.</typeparam>
        /// <param name="key">The key.</param>
        /// <param name="resource">The resource.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="System.NotSupportedException">The type of the key and/or resource is not supported by this manager.</exception>
        public void Release<TKey, TResource>(TKey key, TResource resource)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            if (resource == null) throw new ArgumentNullException(nameof(resource));

            IResourceReleaser<TKey, TResource> rm = this as IResourceReleaser<TKey, TResource>;
            if (rm == null)
                throw new NotSupportedException(Strings.ResourceManager_TypesNotSupported);
            rm.Release(key, resource);
        }

        /// <summary>
        ///     Finalizes an instance of the <see cref="ResourceManager" /> class.
        /// </summary>
        ~ResourceManager()
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
        ///     <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" />
        ///     to release only unmanaged resources.
        /// </param>
        protected virtual void Dispose(bool disposing) { }
    }
}