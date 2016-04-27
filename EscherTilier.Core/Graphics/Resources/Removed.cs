namespace EscherTilier.Graphics.Resources
{
    /// <summary>
    ///     Possible return values from <see cref="ResourceDictionary{TKey,TResource}.Remove" />
    /// </summary>
    public enum Removed : byte
    {
        /// <summary>
        ///     The resource was not found.
        /// </summary>
        NotFound,

        /// <summary>
        ///     The resource was removed for the key, but still exists for other keys.
        /// </summary>
        Removed,

        /// <summary>
        ///     The resource was removed for the key and no other keys map to it.
        /// </summary>
        RemovedLast
    }
}