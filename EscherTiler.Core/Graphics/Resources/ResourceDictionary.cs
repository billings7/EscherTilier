using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using JetBrains.Annotations;

namespace EscherTiler.Graphics.Resources
{
    /// <summary>
    ///     Dictionary for storing resources.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TResource">The type of the resource.</typeparam>
    /// <seealso cref="System.Collections.Generic.IEnumerable{KeyValuePair{TKey, TResource}}" />
    public class ResourceDictionary<TKey, TResource> : IEnumerable<KeyValuePair<TKey, TResource>>
    {
        [NotNull]
        private readonly Dictionary<TKey, Wrapper> _resources;

        [NotNull]
        private readonly Dictionary<TResource, Count> _resourceCount;

        /// <summary>
        ///     A wrapper around a resource.
        /// </summary>
        /// <seealso cref="IEnumerable{KeyValuePair{TKey, TResource}}" />
        private struct Wrapper
        {
            public readonly bool IsTemporary;

            private readonly TResource _resource;

            /// <summary>
            ///     Gets the resource.
            /// </summary>
            /// <value>
            ///     The resource.
            /// </value>
            [NotNull]
            public TResource Resource
            {
                get
                {
                    Debug.Assert(_resource != null, "_resource != null");
                    return _resource;
                }
            }

            /// <summary>
            ///     Initializes a new instance of the <see cref="Wrapper" /> struct.
            /// </summary>
            /// <param name="resource">The resource.</param>
            /// <param name="isTemp">if set to <see langword="true" /> the resourec is only temporary.</param>
            public Wrapper([NotNull] TResource resource, bool isTemp)
            {
                Debug.Assert(resource != null, "resource != null");
                _resource = resource;
                IsTemporary = isTemp;
            }
        }

        /// <summary>
        ///     Stores a permenant and temporary resource count.
        /// </summary>
        private class Count
        {
            /// <summary>
            ///     The number of 'permenant' (non-temporary) resources.
            /// </summary>
            public int PermCount;

            /// <summary>
            ///     The number of temporary resources.
            /// </summary>
            public int TempCount;

            /// <summary>
            ///     Gets the total number of resources.
            /// </summary>
            public int Total => PermCount + TempCount;

            /// <summary>
            ///     Increments the count.
            /// </summary>
            /// <param name="temp">
            ///     if set to <see langword="true" /> increment the <see cref="TempCount" />;
            ///     otherwise increment the <see cref="PermCount" />.
            /// </param>
            public void Inc(bool temp)
            {
                if (temp) TempCount++;
                else PermCount++;
            }

            /// <summary>
            ///     Decrements the count.
            /// </summary>
            /// <param name="temp">
            ///     if set to <see langword="true" /> decrement the <see cref="TempCount" />;
            ///     otherwise decrement the <see cref="PermCount" />.
            /// </param>
            public void Dec(bool temp)
            {
                if (temp) TempCount--;
                else PermCount--;
                Debug.Assert(TempCount >= 0);
                Debug.Assert(PermCount >= 0);
            }

            /// <summary>
            ///     Determines if <see cref="PermCount" /> or <see cref="TempCount" />, depending on <paramref name="temp" />, is
            ///     greater than zero.
            /// </summary>
            /// <param name="temp">
            ///     if set to <see langword="true" /> check <see cref="TempCount" />; otherwise check
            ///     <see cref="PermCount" />.
            /// </param>
            /// <returns></returns>
            public bool Any(bool temp) => temp ? TempCount > 0 : PermCount > 0;

            /// <summary>
            ///     Performs an implicit conversion from <see cref="Count" /> to <see cref="System.Int32" />.
            /// </summary>
            /// <param name="c">The c.</param>
            /// <returns>
            ///     The result of the conversion.
            /// </returns>
            public static implicit operator int(Count c) => c.Total;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ResourceDictionary{TKey, TResource}" /> class.
        /// </summary>
        /// <param name="keyComparer">The key comparer.</param>
        /// <param name="resourceComparer">The resource comparer.</param>
        public ResourceDictionary(
            IEqualityComparer<TKey> keyComparer = null,
            IEqualityComparer<TResource> resourceComparer = null)
        {
            keyComparer = keyComparer ?? EqualityComparer<TKey>.Default;
            resourceComparer = resourceComparer ?? EqualityComparer<TResource>.Default;

            _resources = new Dictionary<TKey, Wrapper>(keyComparer);
            _resourceCount = new Dictionary<TResource, Count>(resourceComparer);
        }

        /// <summary>
        ///     Adds a resource for a key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="resource">The resource.</param>
        /// <param name="temp">if set to <see langword="true" /> the resource is only temporary.</param>
        public void Add([NotNull] TKey key, [NotNull] TResource resource, bool temp)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            if (resource == null) throw new ArgumentNullException(nameof(resource));

            _resources.Add(key, new Wrapper(resource, temp));

            Count count;
            bool got = _resourceCount.TryGetValue(resource, out count);
            Debug.Assert(!got || count > 0);

            if (count == null)
                _resourceCount.Add(resource, count = new Count());

            count.Inc(temp);
        }

        /// <summary>
        ///     Updates a resource for a key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="resource">The resource.</param>
        /// <param name="temp">if set to <see langword="true" /> the resource is only temporary.</param>
        public void Update([NotNull] TKey key, [NotNull] TResource resource, bool temp)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            if (resource == null) throw new ArgumentNullException(nameof(resource));

            if (!_resources.ContainsKey(key))
                throw new InvalidOperationException("Cannot update a resource that has not yet been added");

            _resources[key] = new Wrapper(resource, temp);
        }

        /// <summary>
        ///     Attempts to get the resource for the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="resource">The resource.</param>
        /// <param name="temp">
        ///     If <see langword="true" /> get only if temporary, if <see langword="false" /> get only if not
        ///     temporary, if <see langword="null" /> get any.
        /// </param>
        /// <returns><see langword="true" /> if the resource was found; otherwise <see langword="false" />.</returns>
        public bool TryGetResource([NotNull] TKey key, [CanBeNull] out TResource resource, bool? temp = null)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));

            Wrapper wrapper;
            if (_resources.TryGetValue(key, out wrapper))
            {
                if (temp == null || temp == wrapper.IsTemporary)
                {
                    resource = wrapper.Resource;
                    return true;
                }
            }

            resource = default(TResource);
            return false;
        }

        /// <summary>
        ///     Determines whether the dictionary contains the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="temp">
        ///     If <see langword="true" /> only count temporary resources, if <see langword="false" /> only count non-readonly
        ///     resources, if <see langword="null" /> count any.
        /// </param>
        /// <returns><see langword="true" /> if the key was found; otherwise <see langword="false" />.</returns>
        public bool ContainsKey([NotNull] TKey key, bool? temp = null)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));

            if (temp == null)
                return _resources.ContainsKey(key);

            Wrapper wrapper;
            return _resources.TryGetValue(key, out wrapper) && wrapper.IsTemporary == temp;
        }

        /// <summary>
        ///     Determines whether the dictionary contains the specified resource.
        /// </summary>
        /// <param name="resource">The resource.</param>
        /// <param name="temp">
        ///     If <see langword="true" /> only count temporary resources, if <see langword="false" /> only count non-readonly
        ///     resources, if <see langword="null" /> count any.
        /// </param>
        /// <returns><see langword="true" /> if the resource was found; otherwise <see langword="false" />.</returns>
        public bool ContainsResource([NotNull] TResource resource, bool? temp = null)
        {
            if (resource == null) throw new ArgumentNullException(nameof(resource));

            if (temp == null)
                return _resourceCount.ContainsKey(resource);

            Count count;
            if (!_resourceCount.TryGetValue(resource, out count)) return false;

            Debug.Assert(count != null, "count != null");
            return count.Any(temp.Value);
        }

        /// <summary>
        ///     Removes and returns the resource with the specified key from the dictionary.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="resource">The resource.</param>
        /// <param name="temp">
        ///     If <see langword="true" /> only remove temporary resources, if <see langword="false" /> only remove non-readonly
        ///     resources, if <see langword="null" /> remove any.
        /// </param>
        /// <returns>Whether or not the resource was removed, and if it was the last copy of the resource in the dictionary.</returns>
        public Removed Remove([NotNull] TKey key, out TResource resource, bool? temp = null)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));

            Wrapper wrapper;
            if (!_resources.TryGetValue(key, out wrapper))
            {
                resource = default(TResource);
                return Removed.NotFound;
            }

            if (temp != null && temp != wrapper.IsTemporary)
            {
                resource = default(TResource);
                return Removed.NotFound;
            }

            _resources.Remove(key);

            resource = wrapper.Resource;

            Count count = _resourceCount[wrapper.Resource];
            Debug.Assert(count != null, "count != null");
            count.Dec(wrapper.IsTemporary);
            if (count.Total == 0)
            {
                _resourceCount.Remove(wrapper.Resource);
                return Removed.RemovedLast;
            }

            return Removed.Removed;
        }

        /// <summary>
        ///     Gets the resource for the specified key, adding it if it doesnt yet exist.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="factory">The factory delegate for creating the resource.</param>
        /// <param name="temp">if set to <see langword="true" /> the resource should only be temporary.</param>
        /// <returns>The resource.</returns>
        [NotNull]
        public TResource GetOrAdd([NotNull] TKey key, [NotNull] Func<TKey, TResource> factory, bool temp)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            if (factory == null) throw new ArgumentNullException(nameof(factory));

            TResource resource;
            if (TryGetResource(key, out resource))
            {
                Debug.Assert(resource != null, "resource != null");
                return resource;
            }

            resource = factory(key);
            if (resource == null) throw new InvalidOperationException("The factory returned a null resource");

            Add(key, resource, temp);
            return resource;
        }

        /// <summary>
        ///     Updates the resource for the specified key, adding it if it doesnt yet exist.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="factory">The factory delegate for creating the resource.</param>
        /// <param name="updater">The updater delegate for updating an existing resource.</param>
        /// <param name="temp">if set to <see langword="true" /> the resource should only be temporary.</param>
        /// <returns>The resource.</returns>
        /// <exception cref="System.InvalidOperationException">
        /// The updater function returned a null resource
        /// or
        /// The factory returned a null resource
        /// </exception>
        [NotNull]
        public TResource AddOrUpdate(
            [NotNull] TKey key,
            [NotNull] Func<TKey, TResource> factory,
            [NotNull] Func<TKey, TResource, TResource> updater,
            bool temp)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            if (factory == null) throw new ArgumentNullException(nameof(factory));
            if (updater == null) throw new ArgumentNullException(nameof(updater));

            TResource resource;
            if (TryGetResource(key, out resource))
            {
                resource = updater(key, resource);
                if (resource == null)
                    throw new InvalidOperationException("The updater function returned a null resource");

                Update(key, resource, temp);
                return resource;
            }

            resource = factory(key);
            if (resource == null) throw new InvalidOperationException("The factory returned a null resource");

            Add(key, resource, temp);
            return resource;
        }

        /// <summary>
        ///     Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        ///     An enumerator that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<KeyValuePair<TKey, TResource>> GetEnumerator()
        {
            foreach (KeyValuePair<TKey, Wrapper> kvp in _resources)
                yield return new KeyValuePair<TKey, TResource>(kvp.Key, kvp.Value.Resource);
        }

        /// <summary>
        ///     Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        ///     An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}