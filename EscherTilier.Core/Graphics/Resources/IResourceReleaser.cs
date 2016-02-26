using JetBrains.Annotations;

namespace EscherTilier.Graphics.Resources
{
    /// <summary>
    ///     Interface to a class that allows a resource to be released.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    public interface IResourceReleaser<in TKey>
    {
        /// <summary>
        ///     Releases the resource specified by the given key.
        /// </summary>
        /// <param name="key">The key</param>
        /// <exception cref="System.NotSupportedException">The type of the key and/or resource is not supported by this manager.</exception>
        /// <exception cref="System.ArgumentNullException"><paramref name="key" /> is null.</exception>
        void Release([NotNull] TKey key);
    }
}