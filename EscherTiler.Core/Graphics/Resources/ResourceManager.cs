using System;

namespace EscherTiler.Graphics.Resources
{
    /// <summary>
    ///     Base class for managing resources.
    /// </summary>
    public abstract class ResourceManager : IResourceManager
    {
        /// <summary>
        ///     Adds the specified key to the manager which creates the resource(s) for the key.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <param name="key">The key.</param>
        /// <exception cref="System.NotSupportedException">The type of the key and/or resource is not supported by this manager.</exception>
        /// <exception cref="System.ArgumentNullException"><paramref name="key" /> is null.</exception>
        public void Add<TKey>(TKey key)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));

            IResourceManager<TKey> rm = this as IResourceManager<TKey>;
            if (rm == null)
                throw new NotSupportedException(Strings.ResourceManager_TypesNotSupported);
            rm.Add(key);
        }

        /// <summary>
        ///     Adds the specified key and resource value to the manager.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TResource">The type of the resource.</typeparam>
        /// <param name="key">The key.</param>
        /// <param name="resource">The resource.</param>
        /// <exception cref="System.NotSupportedException">The type of the key and/or resource is not supported by this manager.</exception>
        /// <exception cref="System.ArgumentNullException"><paramref name="key" /> or <paramref name="resource" /> is null.</exception>
        public void Add<TKey, TResource>(TKey key, TResource resource)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            if (resource == null) throw new ArgumentNullException(nameof(resource));

            IResourceManager<TKey, TResource> rm = this as IResourceManager<TKey, TResource>;
            if (rm == null)
                throw new NotSupportedException(Strings.ResourceManager_TypesNotSupported);
            rm.Add(key, resource);
        }

        /// <summary>
        ///     Updates the specified key in the manager which creates the resource(s) for the key.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <param name="key">The key.</param>
        /// <exception cref="System.NotSupportedException">The type of the key and/or resource is not supported by this manager.</exception>
        /// <exception cref="System.ArgumentNullException"><paramref name="key" /> is null.</exception>
        public void Update<TKey>(TKey key)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));

            IResourceManager<TKey> rm = this as IResourceManager<TKey>;
            if (rm == null)
                throw new NotSupportedException(Strings.ResourceManager_TypesNotSupported);
            rm.Update(key);
        }

        /// <summary>
        ///     Updates the specified key and resource value in the manager.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TResource">The type of the resource.</typeparam>
        /// <param name="key">The key.</param>
        /// <param name="resource">The resource.</param>
        /// <exception cref="System.NotSupportedException">The type of the key and/or resource is not supported by this manager.</exception>
        /// <exception cref="System.ArgumentNullException"><paramref name="key" /> or <paramref name="resource" /> is null.</exception>
        public void Update<TKey, TResource>(TKey key, TResource resource)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            if (resource == null) throw new ArgumentNullException(nameof(resource));

            IResourceManager<TKey, TResource> rm = this as IResourceManager<TKey, TResource>;
            if (rm == null)
                throw new NotSupportedException(Strings.ResourceManager_TypesNotSupported);
            rm.Update(key, resource);
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
        /// <exception cref="System.ArgumentNullException"><paramref name="key" /> is null.</exception>
        public TResource Get<TKey, TResource>(TKey key)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));

            IReadonlyResourceManager<TKey, TResource> rm = this as IReadonlyResourceManager<TKey, TResource>;
            if (rm == null)
                throw new NotSupportedException(Strings.ResourceManager_TypesNotSupported);
            return rm.Get(key);
        }

        /// <summary>
        ///     Releases the resource specified by the given key.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <param name="key">The key.</param>
        /// <exception cref="System.NotSupportedException">The type of the key and/or resource is not supported by this manager.</exception>
        /// <exception cref="System.ArgumentNullException"><paramref name="key" /> is null.</exception>
        public void Release<TKey>(TKey key)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));

            IResourceReleaser<TKey> rm = this as IResourceReleaser<TKey>;
            if (rm == null)
                throw new NotSupportedException(Strings.ResourceManager_TypesNotSupported);
            rm.Release(key);
        }

        /// <summary>
        ///     Removes the resource specified by the given key.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <param name="key">The key.</param>
        /// <exception cref="System.NotSupportedException">The type of the key and/or resource is not supported by this manager.</exception>
        /// <exception cref="System.ArgumentNullException"><paramref name="key" /> is null.</exception>
        public void Remove<TKey>(TKey key)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));

            IResourceRemover<TKey> rm = this as IResourceRemover<TKey>;
            if (rm == null)
                throw new NotSupportedException(Strings.ResourceManager_TypesNotSupported);
            rm.Remove(key);
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