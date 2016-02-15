using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace EscherTilier
{
    /// <summary>
    /// Defines the parts that make up a given edge for a tiling.
    /// </summary>
    public class EdgePattern
    {
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
            [NotNull] IReadOnlyList<EdgePart> parts)
        {
            if (edgeName == null) throw new ArgumentNullException(nameof(edgeName));
            if (parts == null) throw new ArgumentNullException(nameof(parts));
            if (parts.Count < 1)
                throw new ArgumentException(Strings.EdgePattern_EdgePattern_OnePartRequired, nameof(parts));
            if (Math.Abs(parts.Sum(p => p.Amount) - 1) > 0.001f)
                throw new ArgumentException(Strings.EdgePattern_EdgePattern_PartAmountEqual1, nameof(parts));

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