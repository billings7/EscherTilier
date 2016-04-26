using System.Diagnostics;
using JetBrains.Annotations;

namespace EscherTilier
{
    /// <summary>
    ///     Defines one part of an edge.
    /// </summary>
    public class EdgePart
    {
        private EdgePattern _edgePattern;

        [NotNull]
        internal EdgePattern EdgePattern
        {
            get
            {
                Debug.Assert(_edgePattern != null, "_edgePattern != null");
                return _edgePattern;
            }
            set
            {
                Debug.Assert(
                    _edgePattern == null || _edgePattern == value,
                    "The part is already used in another pattern.");
                _edgePattern = value;
            }
        }

        internal ShapeTemplate ShapeTemplate => EdgePattern.ShapeTemplate;

        /// <summary>
        ///     Gets the distance along the edge that the part starts, if this part has been added to a pattern.
        /// </summary>
        /// <value>
        ///     The start amount.
        /// </value>
        internal float StartAmount { get; set; }

        /// <summary>
        ///     Initializes a new instance of the <see cref="EdgePart" /> class.
        /// </summary>
        /// <param name="id">The part identifier.</param>
        /// <param name="amount">The amount.</param>
        /// <param name="isClockwise">If set to <see langword="true" /> the part is clockwise; otherwise counter-clockwise.</param>
        public EdgePart(int id, float amount, bool isClockwise)
        {
            ID = id;
            Amount = amount;
            IsClockwise = isClockwise;
        }

        /// <summary>
        ///     Gets the identifier for the edge part.
        ///     All parts with the same ID will have the same shape in the final tiling.
        /// </summary>
        /// <value>
        ///     The identifier.
        /// </value>
        public int ID { get; }

        /// <summary>
        ///     Gets a value indicating whether this part is oriented in a clockwise direction.
        /// </summary>
        /// <value>
        ///     <see langword="true" /> if this part is clockwise; otherwise, <see langword="false" />.
        /// </value>
        public bool IsClockwise { get; }

        /// <summary>
        ///     Gets the amount of the edge that the part takes up.
        /// </summary>
        /// <value>
        ///     The amount of the edge that the part takes up, in the range 0-1.
        /// </value>
        public float Amount { get; }
    }
}