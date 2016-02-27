using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace EscherTilier
{
    /// <summary>
    ///     Adds a label to a value.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public struct Labeled<T> : IEquatable<Labeled<T>>
    {
        private readonly string _label;

        /// <summary>
        ///     Initializes a new instance of the <see cref="Labeled{T}" /> struct.
        /// </summary>
        /// <param name="label">The label.</param>
        /// <param name="value">The value.</param>
        public Labeled([NotNull] string label, [CanBeNull] T value)
        {
            _label = label;
            Value = value;
        }

        /// <summary>
        ///     Gets the label.
        /// </summary>
        /// <value>
        ///     The label.
        /// </value>
        [NotNull]
        public string Label => _label ?? string.Empty;

        /// <summary>
        ///     Gets the value.
        /// </summary>
        /// <value>
        ///     The value.
        /// </value>
        [CanBeNull]
        public T Value { get; }

        /// <summary>
        ///     Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        ///     true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(Labeled<T> other)
        {
            return string.Equals(Label, other.Label, StringComparison.InvariantCulture) &&
                   EqualityComparer<T>.Default.Equals(Value, other.Value);
        }

        /// <summary>
        ///     Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <returns>
        ///     true if <paramref name="obj" /> and this instance are the same type and represent the same value; otherwise, false.
        /// </returns>
        /// <param name="obj">The object to compare with the current instance. </param>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Labeled<T> && Equals((Labeled<T>) obj);
        }

        /// <summary>
        ///     Returns the hash code for this instance.
        /// </summary>
        /// <returns>
        ///     A 32-bit signed integer that is the hash code for this instance.
        /// </returns>
        public override int GetHashCode()
        {
            unchecked
            {
                return (StringComparer.InvariantCulture.GetHashCode(Label) * 397) ^
                       EqualityComparer<T>.Default.GetHashCode(Value);
            }
        }

        /// <summary>
        ///     Implements the operator ==.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>
        ///     The result of the operator.
        /// </returns>
        public static bool operator ==(Labeled<T> left, Labeled<T> right) => left.Equals(right);

        /// <summary>
        ///     Implements the operator !=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>
        ///     The result of the operator.
        /// </returns>
        public static bool operator !=(Labeled<T> left, Labeled<T> right) => !left.Equals(right);
    }
}