using System;
using JetBrains.Annotations;

namespace EscherTiler
{
    /// <summary>
    ///     Stores a value for a combo box with the display string that should be shown in the box.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    public class ComboBoxValue<T>
    {
        /// <summary>
        ///     The display string.
        /// </summary>
        [NotNull]
        public readonly string DisplayString;

        /// <summary>
        ///     The value.
        /// </summary>
        [NotNull]
        public readonly T Value;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ComboBoxValue{T}" /> class.
        /// </summary>
        /// <param name="displayString">The display string.</param>
        /// <param name="value">The value.</param>
        /// <exception cref="System.ArgumentNullException">
        /// </exception>
        public ComboBoxValue([NotNull] string displayString, [NotNull] T value)
        {
            if (displayString == null) throw new ArgumentNullException(nameof(displayString));
            if (value == null) throw new ArgumentNullException(nameof(value));

            DisplayString = displayString;
            Value = value;
        }

        /// <summary>
        ///     Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        ///     A string that represents the current object.
        /// </returns>
        public override string ToString() => DisplayString;

        /// <summary>
        ///     Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <returns>
        ///     true if the specified object  is equal to the current object; otherwise, false.
        /// </returns>
        /// <param name="obj">The object to compare with the current object. </param>
        public override bool Equals(object obj)
        {
            ComboBoxValue<T> val = obj as ComboBoxValue<T>;
            if (val != null) return Equals(Value, val.Value);
            return Equals(Value, obj);
        }

        /// <summary>
        ///     Serves as the default hash function.
        /// </summary>
        /// <returns>
        ///     A hash code for the current object.
        /// </returns>
        public override int GetHashCode() => Value.GetHashCode();
    }
}