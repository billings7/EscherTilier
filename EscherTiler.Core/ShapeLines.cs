using System;
using System.Collections.Generic;
using System.Numerics;
using JetBrains.Annotations;

namespace EscherTiler
{
    /// <summary>
    ///     Defines a list of <see cref="ILine">lines</see>.
    /// </summary>
    /// <seealso cref="System.Collections.Generic.List{ILine}" />
    public class ShapeLines : List<ILine>
    {
        /// <summary>
        ///     Creates a default instance of the <see cref="ShapeLines" /> class containing a single straight line.
        /// </summary>
        public static ShapeLines CreateDefault()
        {
            return new ShapeLines
            {
                new Line(new LineVector(Vector2.Zero, true), new LineVector(new Vector2(1, 0), true))
            };
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ShapeLines" /> class that is empty and has the default initial
        ///     capacity.
        /// </summary>
        public ShapeLines() { }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ShapeLines" /> class that is empty and has the specified initial
        ///     capacity.
        /// </summary>
        /// <param name="capacity">The number of elements that the new list can initially store.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="capacity" /> is less than 0. </exception>
        public ShapeLines(int capacity)
            : base(capacity) { }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ShapeLines" /> class that contains elements copied from the specified
        ///     collection and has sufficient capacity to accommodate the number of elements copied.
        /// </summary>
        /// <param name="collection">The collection whose elements are copied to the new list.</param>
        /// <exception cref="ArgumentNullException"><paramref name="collection" /> is null.</exception>
        public ShapeLines([NotNull] IEnumerable<ILine> collection)
            : base(collection) { }

        /// <summary>
        ///     Replaces the specified line.
        /// </summary>
        /// <param name="line">The line.</param>
        /// <param name="newLines">The new lines.</param>
        /// <returns><see langword="true" /> if the line existed and was replace; otherwise <see langword="false" />.</returns>
        public bool Replace(ILine line, params ILine[] newLines)
        {
            int index = IndexOf(line);
            if (index < 0) return false;

            RemoveAt(index);
            InsertRange(index, newLines);
            return true;
        }
    }
}