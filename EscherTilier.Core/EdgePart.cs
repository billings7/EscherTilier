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
                Debug.Assert(_edgePattern == null || _edgePattern == value, "The part is already used in another pattern.");
                _edgePattern = value;
            }
        }

        internal ShapeTemplate ShapeTemplate => EdgePattern.ShapeTemplate;

        /// <summary>
        ///     Initializes a new instance of the <see cref="EdgePart" /> class.
        /// </summary>
        /// <param name="id">The part identifier.</param>
        /// <param name="direction">The direction.</param>
        /// <param name="amount">The amount.</param>
        public EdgePart(int id, PartDirection direction, float amount)
        {
            ID = id;
            Direction = direction;
            Amount = amount;
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
        ///     Gets the direction the part is oriented.
        /// </summary>
        /// <value>
        ///     The direction.
        /// </value>
        public PartDirection Direction { get; }

        /// <summary>
        ///     Gets the amount of the edge that the part takes up.
        /// </summary>
        /// <value>
        ///     The amount.
        /// </value>
        public float Amount { get; }
    }
}