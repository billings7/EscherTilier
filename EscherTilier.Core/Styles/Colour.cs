using System;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using JetBrains.Annotations;

namespace EscherTilier.Styles
{
    /// <summary>
    /// Defines a colour in an RGBA format.
    /// </summary>
    public partial struct Colour : IEquatable<Colour>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Colour"/> struct.
        /// </summary>
        /// <param name="red">The red component.</param>
        /// <param name="green">The green component.</param>
        /// <param name="blue">The blue component.</param>
        /// <param name="alpha">The alpha component.</param>
        public Colour(byte red, byte green, byte blue, byte alpha = 255)
        {
            R = red / 255f;
            G = green / 255f;
            B = blue / 255f;
            A = alpha / 255f;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Colour"/> struct.
        /// </summary>
        /// <param name="red">The red component.</param>
        /// <param name="green">The green component.</param>
        /// <param name="blue">The blue component.</param>
        /// <param name="alpha">The alpha component.</param>
        public Colour(float red, float green, float blue, float alpha = 1f)
        {
            CheckRange(red);
            CheckRange(green);
            CheckRange(blue);
            CheckRange(alpha);
            R = red;
            G = green;
            B = blue;
            A = alpha;
        }

        private static void CheckRange(float val)
        {
            if (val < 0 || val > 1)
                throw new ArgumentOutOfRangeException(nameof(val), Strings.Colour_CheckRange_NotInRange);
        }

        /// <summary>
        ///     Gets the red component of this colour.
        /// </summary>
        /// <value>
        ///     The red component of this colour.
        /// </value>
        public float R { get; }

        /// <summary>
        ///     Gets the green component of this colour.
        /// </summary>
        /// <value>
        ///     The green component of this colour.
        /// </value>
        public float G { get; }

        /// <summary>
        ///     Gets the blue component of this colour.
        /// </summary>
        /// <value>
        ///     The blue component of this colour.
        /// </value>
        public float B { get; }

        /// <summary>
        ///     Gets the alpha component of this colour.
        /// </summary>
        /// <value>
        ///     The alpha component of this colour.
        /// </value>
        public float A { get; }

        /// <summary>
        ///     Gets the name of this colour.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        [NotNull]
        public string Name
        {
            get
            {
                Colour me = this;
                return _knownColours.FirstOrDefault(kvp => kvp.Value == me).Key ??
                       me.ToArgb().ToString("X8", CultureInfo.InvariantCulture);
            }
        }

        /// <summary>
        ///     Gets the ARGB value for this colour.
        /// </summary>
        /// <returns></returns>
        public int ToArgb()
        {
            return ((byte)(A * 255) << 24) |
                   ((byte)(R * 255) << 16) |
                   ((byte)(G * 255) << 8) |
                   ((byte)(A * 255) << 0);
        }

        /// <summary>
        ///     Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        ///     true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(Colour other)
        {
            return R.Equals(other.R) && G.Equals(other.G) && B.Equals(other.B) && A.Equals(other.A);
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
            return obj is Colour && Equals((Colour)obj);
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
                int hashCode = R.GetHashCode();
                hashCode = (hashCode * 397) ^ G.GetHashCode();
                hashCode = (hashCode * 397) ^ B.GetHashCode();
                hashCode = (hashCode * 397) ^ A.GetHashCode();
                return hashCode;
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
        public static bool operator ==(Colour left, Colour right) => left.Equals(right);

        /// <summary>
        ///     Implements the operator !=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>
        ///     The result of the operator.
        /// </returns>
        public static bool operator !=(Colour left, Colour right) => !left.Equals(right);

        /// <summary>
        ///     Returns the fully qualified type name of this instance.
        /// </summary>
        /// <returns>
        ///     A <see cref="T:System.String" /> containing a fully qualified type name.
        /// </returns>
        public override string ToString() => $"{Name} [ R: {R:0.##}, G: {G:0.##}, B: {B:0.##}, A: {A:0.##} ]";
    }
}