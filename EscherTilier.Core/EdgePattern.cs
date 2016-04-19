using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using JetBrains.Annotations;

namespace EscherTilier
{
    /// <summary>
    /// Defines the parts that make up a given edge for a tiling.
    /// </summary>
    public class EdgePattern
    {
        private TilingDefinition _tilingDefinition;

        [NotNull]
        internal TilingDefinition TilingDefinition
        {
            get
            {
                Debug.Assert(_tilingDefinition != null, "_tilingDefinition != null");
                return _tilingDefinition;
            }
            set
            {
                Debug.Assert(_tilingDefinition == null || _tilingDefinition == value, "The pattern is already used by another definition.");
                _tilingDefinition = value;
            }
        }

        private ShapeTemplate _shapeTemplate;

        [NotNull]
        internal ShapeTemplate ShapeTemplate
        {
            get
            {
                Debug.Assert(_shapeTemplate != null, "_shapeTemplate != null");
                return _shapeTemplate;
            }
            set
            {
                Debug.Assert(_shapeTemplate == null || _shapeTemplate == value, "The pattern is already used by another shape template.");
                _shapeTemplate = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EdgePattern"/> class.
        /// </summary>
        /// <param name="edgeName">Name of the edge.</param>
        /// <param name="parts">The parts.</param>
        /// <exception cref="System.ArgumentNullException">
        /// </exception>
        /// <exception cref="System.ArgumentException"></exception>
        public EdgePattern(
            [NotNull] string edgeName,
            [NotNull][ItemNotNull] IReadOnlyList<EdgePart> parts)
        {
            if (edgeName == null) throw new ArgumentNullException(nameof(edgeName));
            if (parts == null) throw new ArgumentNullException(nameof(parts));
            if (parts.Count < 1)
                throw new ArgumentException(Strings.EdgePattern_EdgePattern_OnePartRequired, nameof(parts));
            if (parts.Any(p => p == null)) throw new ArgumentNullException(nameof(parts));
            if (Math.Abs(parts.Sum(p => p.Amount) - 1) > 0.001f)
                throw new ArgumentException(Strings.EdgePattern_EdgePattern_PartAmountEqual1, nameof(parts));

            float amount = 0;
            foreach (EdgePart part in parts)
            {
                part.EdgePattern = this;
                part.StartAmount = amount;
                amount += part.StartAmount;
            }

            EdgeName = edgeName;
            Parts = parts;
        }

        /// <summary>
        /// Gets the name of the edge that this is the pattern for.
        /// </summary>
        /// <value>
        /// The name of the edge.
        /// </value>
        [NotNull]
        public string EdgeName { get; }

        /// <summary>
        /// Gets the parts that make up the pattern.
        /// </summary>
        /// <value>
        /// The parts.
        /// </value>
        [NotNull]
        public IReadOnlyList<EdgePart> Parts { get; }
    }
}