using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using EscherTilier.Expressions;
using EscherTilier.Utilities;
using JetBrains.Annotations;

namespace EscherTilier
{
    /// <summary>
    ///     Defines how shapes can be tiled.
    /// </summary>
    public class TilingDefinition
    {
        private Template _template;

        [NotNull]
        internal Template Template
        {
            get
            {
                Debug.Assert(_template != null, "_template != null");
                return _template;
            }
            set
            {
                Debug.Assert(_template == null || _template == value, "The tiling is already used by another template.");
                _template = value;
            }
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="TilingDefinition" /> class.
        /// </summary>
        /// <param name="id">The identifier for the definition.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="edgePatterns">The edge patterns.</param>
        /// <param name="adjacentParts">The adjacent parts.</param>
        /// <exception cref="System.ArgumentNullException">
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// </exception>
        public TilingDefinition(
            int id,
            [CanBeNull] IExpression<bool> condition,
            [NotNull] [ItemNotNull] IReadOnlyList<EdgePattern> edgePatterns,
            [NotNull] EdgePartAdjacencies adjacentParts)
        {
            if (edgePatterns == null) throw new ArgumentNullException(nameof(edgePatterns));
            if (adjacentParts == null) throw new ArgumentNullException(nameof(adjacentParts));
            if (edgePatterns.Count < 3)
            {
                throw new ArgumentException(
                    Strings.TilingDefinition_TilingDefinition_ThreePatternsRequired,
                    nameof(edgePatterns));
            }
            if (edgePatterns.Any(t => t == null)) throw new ArgumentNullException(nameof(edgePatterns));
            if (!edgePatterns.AreDistinct(p => p.EdgeName, StringComparer.InvariantCulture))
            {
                throw new ArgumentException(
                    Strings.TilingDefinition_TilingDefinition_PatternsNameUnique,
                    nameof(edgePatterns));
            }
            if (adjacentParts.Any(p => edgePatterns.All(e => !e.Parts.Contains(p.Value))))
            {
                throw new ArgumentException(
                    Strings.TilingDefinition_TilingDefinition_AdjacentPartNotUsed,
                    nameof(adjacentParts));
            }

            foreach (EdgePattern pattern in edgePatterns) pattern.TilingDefinition = this;

            ID = id;
            Condition = condition?.Compile();
            EdgePatterns = edgePatterns;
            AdjacentParts = adjacentParts;
        }

        /// <summary>
        ///     Gets the identifier.
        /// </summary>
        /// <value>
        ///     The identifier.
        /// </value>
        public int ID { get; }

        /// <summary>
        ///     Gets the condition that must be true for this to be a valid tiling.
        /// </summary>
        /// <value>
        ///     The condition.
        /// </value>
        [CanBeNull]
        public IExpression<bool> Condition { get; }

        /// <summary>
        ///     Gets the edge patterns.
        /// </summary>
        /// <value>
        ///     The edge patterns.
        /// </value>
        [NotNull]
        public IReadOnlyList<EdgePattern> EdgePatterns { get; }

        /// <summary>
        ///     Gets the collection of adjacent edge parts.
        /// </summary>
        /// <value>
        ///     The adjacent parts.
        /// </value>
        [NotNull]
        public EdgePartAdjacencies AdjacentParts { get; }
    }
}