using System;

namespace EscherTiler.Dependencies
{
    [Flags]
    public enum DependencyCacheFlags : byte
    {
        DontCache = 0,
        CacheGlobal = 1 << 0,
        CachePerArgs = 1 << 1,

        DisposeOnRelease = 1 << 4
    }
}