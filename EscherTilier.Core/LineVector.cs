using System;
using System.Numerics;
using JetBrains.Annotations;

namespace EscherTilier
{
    /// <summary>
    ///     Defines a vector which is used to define a <see cref="ILine" />.
    /// </summary>
    public class LineVector
    {
        private Vector2 _vector;
        private readonly bool _isFixed;

        /// <summary>
        ///     Initializes a new instance of the <see cref="LineVector" /> class.
        /// </summary>
        /// <param name="vector">The vector.</param>
        /// <param name="isFixed">
        ///     if set to <see langword="true" /> the vector is fixed and any attempt to change the value will
        ///     fail.
        /// </param>
        public LineVector(Vector2 vector, bool isFixed = false)
        {
            Vector = vector;
            _isFixed = isFixed;
        }

        /// <summary>
        ///     Gets a value indicating whether this vector is fixed.
        /// </summary>
        /// <value>
        ///     <see langword="true" /> if this vector is fixed; otherwise, <see langword="false" />.
        /// </value>
        public bool IsFixed => _isFixed;

        /// <summary>
        ///     Gets or sets the vector value of this <see cref="LineVector" />.
        /// </summary>
        /// <value>
        ///     The vector.
        /// </value>
        /// <exception cref="InvalidOperationException">The joint is fixed</exception>
        public Vector2 Vector
        {
            get { return _vector; }
            set
            {
                if (_isFixed) throw new InvalidOperationException("The joint is fixed");
                _vector = value;
            }
        }

        /// <summary>
        ///     Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        ///     A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString() => Vector + (_isFixed ? " (fixed)" : string.Empty);

        /// <summary>
        ///     Performs an implicit conversion from <see cref="LineVector" /> to <see cref="Vector2" />.
        /// </summary>
        /// <param name="vector">The vector.</param>
        /// <returns>
        ///     The result of the conversion.
        /// </returns>
        public static implicit operator Vector2([CanBeNull] LineVector vector) => vector?._vector ?? Vector2.Zero;
    }
}