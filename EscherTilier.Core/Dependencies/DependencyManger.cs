using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using EscherTilier.Graphics;
using EscherTilier.Graphics.Resources;
using EscherTilier.Styles;
using JetBrains.Annotations;

namespace EscherTilier.Dependencies
{
    /// <summary>
    ///     Handles dependencies.
    /// </summary>
    public class DependencyManger
    {
        [NotNull]
        private static readonly ConcurrentDictionary<Tuple<Type, Type>, object> _caches =
            new ConcurrentDictionary<Tuple<Type, Type>, object>();

        [NotNull]
        private static DependencyCache<TDependancy, TArgs> GetCache<TDependancy, TArgs>()
        {
            object tmp;
            if (!_caches.TryGetValue(Tuple.Create(typeof(TDependancy), typeof(TArgs)), out tmp))
                throw new ArgumentException($"There are no dependencies for the the type {typeof(TDependancy).Name}.");

            Debug.Assert(tmp is DependencyCache<TDependancy, TArgs>, "tmp is DependencyCache<TDependancy, TArgs>");
            return (DependencyCache<TDependancy, TArgs>) tmp;
        }

        private static DependencyCache<TDependancy, TArgs> AddOrUpdateCache<TDependancy, TArgs>(
            [NotNull] Func<TArgs, TDependancy> factory,
            DependencyCacheFlags flags)
        {
            Debug.Assert(factory != null, "factory != null");

            return (DependencyCache<TDependancy, TArgs>) _caches.AddOrUpdate(
                new Tuple<Type, Type>(typeof(TDependancy), typeof(TArgs)),
                _ => new DependencyCache<TDependancy, TArgs>(factory, flags),
                (_, old) =>
                {
                    Debug.Assert(old is DependencyCache<TDependancy, TArgs>, "old is DependencyCache<TDependancy, TArgs>");
                    ((DependencyCache<TDependancy, TArgs>) old).Dispose();
                    return new DependencyCache<TDependancy, TArgs>(factory, flags);
                });
        }
        
        /// <summary>
        ///     Sets the factory that should be used for creating instances of type <see cref="TDependancy" />.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="flags">The flags.</param>
        /// <exception cref="System.ArgumentNullException"><paramref name="factory" /> was <see langword="null" />.</exception>
        public static void ForTypeUse<TDependancy>(
            [NotNull] Func<TDependancy> factory,
            DependencyCacheFlags flags)
        {
            if (factory == null) throw new ArgumentNullException(nameof(factory));

            AddOrUpdateCache((NoArgs _) => factory(), flags);
        }
        
        /// <summary>
        ///     Sets the factory that should be used for creating instances of type <see cref="TDependancy" />.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="flags">The flags.</param>
        /// <exception cref="System.ArgumentNullException"><paramref name="factory" /> was <see langword="null" />.</exception>
        public static void ForTypeUse<TDependancy, TArgs>(
            [NotNull] Func<TArgs, TDependancy> factory,
            DependencyCacheFlags flags)
        {
            if (factory == null) throw new ArgumentNullException(nameof(factory));

            AddOrUpdateCache(factory, flags);
        }
        
        /// <summary>
        ///     Sets the factory that should be used for creating instances of <see cref="IResourceManager" />.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="flags">The flags.</param>
        /// <exception cref="System.ArgumentNullException"><paramref name="factory" /> was <see langword="null" />.</exception>
        public static void ForResourceManagerUse(
            [NotNull] Func<StyleManager, IResourceManager> factory,
            DependencyCacheFlags flags)
        {
            if (factory == null) throw new ArgumentNullException(nameof(factory));

            AddOrUpdateCache(factory, flags);
        }
        
        /// <summary>
        ///     Gets a resource manager.
        /// </summary>
        /// <returns></returns>
        [NotNull]
        public static IResourceManager GetResourceManager()
        {
            IResourceManager resourceManager = GetCache<IResourceManager, StyleManager>().Get();
            if (resourceManager == null)
                throw new InvalidOperationException("The factory returned a null resource manager.");
            return resourceManager;
        }

        /// <summary>
        ///     Gets a resource manager for a style manager.
        /// </summary>
        /// <param name="styleManager">The style manager.</param>
        /// <returns></returns>
        [NotNull]
        public static IResourceManager GetResourceManager(StyleManager styleManager)
        {
            IResourceManager resourceManager = GetCache<IResourceManager, StyleManager>().Get(styleManager);
            if (resourceManager == null)
                throw new InvalidOperationException("The factory returned a null resource manager.");
            return resourceManager;
        }
        
        /// <summary>
        /// Releases the resource manager given.
        /// </summary>
        /// <param name="resourceManager">The resource manager.</param>
        /// <param name="styleManager">The style manager that was used to get the resource manager. Can be null.</param>
        public static void ReleaseResourceManager(ref IResourceManager resourceManager, StyleManager styleManager)
        {
            GetCache<IResourceManager, StyleManager>().Release(Interlocked.Exchange(ref resourceManager, null), styleManager);
        }

        [NotNull]
        private static readonly MethodInfo _getMiscMethodInfo = typeof(DependencyManger)
            .GetMethods(BindingFlags.Public | BindingFlags.Static)
            .Single(m => m.Name == nameof(Get) && m.GetGenericArguments().Length == 2);

        [NotNull]
        private static readonly object[] _defaultArgs = { null };

        /// <summary>
        /// Gets a dependency of a paticular type with no arguments specified.
        /// </summary>
        /// <typeparam name="TDependancy">The type of the dependancy.</typeparam>
        /// <returns></returns>
        /// <exception cref="System.InvalidOperationException">There are multiple argument types for the resource type given.</exception>
        /// <exception cref="System.ArgumentException">There are no dependencies for the resource type given.</exception>
        public static TDependancy Get<TDependancy>()
        {
            Tuple<Type, Type> types = null;
            foreach (KeyValuePair<Tuple<Type, Type>, object> kvp in _caches.Where(k => k.Key.Item1 == typeof(TDependancy))
                )
            {
                if (types != null)
                {
                    throw new InvalidOperationException(
                        "There are multiple argument types for the resource type given.");
                }
                types = kvp.Key;
            }
            if (types == null)
                throw new ArgumentException("There are no dependencies for the resource type given.");

            return (TDependancy) _getMiscMethodInfo.MakeGenericMethod(types.Item1, types.Item2).Invoke(null, _defaultArgs);
        }

        /// <summary>
        /// Gets a dependency of a paticular type with specific arguments.
        /// </summary>
        /// <typeparam name="TDependancy">The type of the dependancy.</typeparam>
        /// <typeparam name="TArgs">The type of the arguments.</typeparam>
        /// <param name="args">The arguments.</param>
        /// <returns></returns>
        public static TDependancy Get<TDependancy, TArgs>(TArgs args = default(TArgs))
        {
            return GetCache<TDependancy, TArgs>().Get(args);
        }

        /// <summary>
        /// Structure type used to represent that no argument type was specified
        /// </summary>
        private struct NoArgs
        {
        }

        private class DependencyCache<TDependancy, TArgs> : IDisposable
        {
            // TODO Probably need some kind of reference counting

            private const DependencyCacheFlags CacheTypeMask = (DependencyCacheFlags) 7;

            [NotNull]
            public readonly Func<TArgs, TDependancy> Factory;

            public readonly DependencyCacheFlags Flags;

            private bool _globalCreated;
            private readonly object _globalLock;
            private TDependancy _global;
            private readonly ConcurrentDictionary<TArgs, TDependancy> _perArgs;

            public DependencyCache([NotNull] Func<TArgs, TDependancy> factory, DependencyCacheFlags flags)
            {
                Debug.Assert(factory != null, "factory != null");
                Factory = factory;
                Flags = flags;
                switch (flags & CacheTypeMask)
                {
                    case DependencyCacheFlags.CachePerArgs:
                        _perArgs = new ConcurrentDictionary<TArgs, TDependancy>();
                        _globalLock = new object();
                        break;
                    case DependencyCacheFlags.CacheGlobal:
                        _globalLock = new object();
                        break;
                }
            }

            public TDependancy Get(TArgs args = default(TArgs))
            {
                switch (Flags & CacheTypeMask)
                {
                    case DependencyCacheFlags.DontCache:
                        return Factory(args);
                    case DependencyCacheFlags.CachePerArgs:
                        Debug.Assert(_perArgs != null, "_perArgs != null");
                        if (args == null)
                            goto case DependencyCacheFlags.CacheGlobal;

                        return _perArgs.GetOrAdd(args, a => Factory(a));
                    case DependencyCacheFlags.CacheGlobal:
                        Debug.Assert(_globalLock != null, "_globalLock != null");
                        lock (_globalLock)
                        {
                            if (_globalCreated) return _global;
                            _global = Factory(args);
                            _globalCreated = true;
                            return _global;
                        }
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            public void Release(TDependancy dependancy, TArgs args = default(TArgs))
            {
                switch (Flags & CacheTypeMask)
                {
                    case DependencyCacheFlags.DontCache:
                        break;
                    case DependencyCacheFlags.CachePerArgs:
                        Debug.Assert(_perArgs != null, "_perArgs != null");
                        if (args == null)
                            goto case DependencyCacheFlags.CacheGlobal;

                        TDependancy temp;
                        if (!_perArgs.TryGetValue(args, out temp))
                            throw new ArgumentException("Could not release the specified dependancy.");
                        if (!Equals(dependancy, temp))
                        {
                            throw new ArgumentException(
                                "The given dependancy does not match the cached dependancy for the given arguments.");
                        }
                        _perArgs.TryRemove(args, out temp);

                        break;
                    case DependencyCacheFlags.CacheGlobal:
                        Debug.Assert(_globalLock != null, "_globalLock != null");
                        lock (_globalLock)
                        {
                            if (!_globalCreated)
                                throw new ArgumentException("Could not release the specified dependancy.");
                            if (!Equals(dependancy, _global))
                            {
                                throw new ArgumentException(
                                    "The given dependancy does not match the cached dependancy for the given arguments.");
                            }

                            _globalCreated = false;
                            _global = default(TDependancy);
                        }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                if ((Flags & DependencyCacheFlags.DisposeOnRelease) != 0)
                    (dependancy as IDisposable)?.Dispose();
            }

            /// <summary>
            ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
            /// </summary>
            public void Dispose()
            {
                if ((Flags & DependencyCacheFlags.DisposeOnRelease) == 0) return;

                Debug.Assert(_globalLock != null, "_globalLock != null");
                lock (_globalLock)
                {
                    if (_globalCreated)
                    {
                        (_global as IDisposable)?.Dispose();
                        _global = default(TDependancy);
                        _globalCreated = false;
                    }
                }

                if (_perArgs == null) return;

                foreach (TArgs key in _perArgs.Keys.ToArray())
                {
                    TDependancy temp;
                    _perArgs.TryRemove(key, out temp);
                    (temp as IDisposable)?.Dispose();
                }
            }
        }
    }
}