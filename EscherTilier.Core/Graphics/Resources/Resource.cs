using System;
using System.Diagnostics;
using System.Threading;
using JetBrains.Annotations;

namespace EscherTilier.Graphics.Resources
{
    /// <summary>
    ///     Stores a resource optionally with extra resources that should be disposed of along with the main one.
    /// </summary>
    /// <typeparam name="T">The type of the resource.</typeparam>
    public class Resource<T> : IDisposable
    {
        private T _value;

        private IDisposable[] _disposables;

        /// <summary>
        ///     Initializes a new instance of the <see cref="Resource{T}" /> class.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="disposables">The disposables.</param>
        public Resource([NotNull] T value, [NotNull] params IDisposable[] disposables)
        {
            Debug.Assert(value != null, "disposables != null");
            Debug.Assert(disposables != null, "disposables != null");
            _value = value;
            _disposables = disposables.Length > 0 ? disposables : null;
        }

        /// <summary>
        ///     Gets the value.
        /// </summary>
        /// <value>
        ///     The value.
        /// </value>
        [NotNull]
        public T Value
        {
            get
            {
                Debug.Assert(_value != null);
                return _value;
            }
        }

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            T val = _value;
            _value = default(T);

            (val as IDisposable)?.Dispose();

            IDisposable[] disps = Interlocked.Exchange(ref _disposables, null);
            if (disps == null) return;

            for (int i = 0; i < disps.Length; i++)
            {
                disps[i]?.Dispose();
                disps[i] = null;
            }
        }

        /// <summary>
        ///     Performs an implicit conversion from <see cref="T" /> to <see cref="Resource{T}" />.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        ///     The result of the conversion.
        /// </returns>
        public static implicit operator Resource<T>([NotNull] T value) => new Resource<T>(value);
    }
}