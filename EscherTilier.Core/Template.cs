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
    ///     Defines the base shapes and tiling definitions that can be used to create a <see cref="Tiling" />.
    /// </summary>
    public class Template
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Template" /> class.
        /// </summary>
        /// <param name="shapeTemplates">The shape templates.</param>
        /// <param name="shapeConstraints">The shape constraints.</param>
        /// <param name="tilings">The tilings.</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="shapeTemplates" />, <paramref name="shapeConstraints" /> or <paramref name="tilings" /> is null.
        /// </exception>
        /// TODO Exceptions
        public Template(
            [NotNull] IReadOnlyList<ShapeTemplate> shapeTemplates,
            [NotNull] IReadOnlyList<IExpression<bool>> shapeConstraints,
            [NotNull] IReadOnlyList<TilingDefinition> tilings)
        {
            if (shapeTemplates == null) throw new ArgumentNullException(nameof(shapeTemplates));
            if (shapeConstraints == null) throw new ArgumentNullException(nameof(shapeConstraints));
            if (tilings == null) throw new ArgumentNullException(nameof(tilings));
            if (shapeTemplates.Count < 1)
                throw new ArgumentException(Strings.Template_Template_OneTemplateRequired, nameof(shapeTemplates));
            if (tilings.Count < 1)
                throw new ArgumentException(Strings.Template_Template_OneTilingRequired, nameof(tilings));
            if (shapeTemplates.Any(t => t == null)) throw new ArgumentNullException(nameof(shapeTemplates));
            if (shapeConstraints.Any(t => t == null)) throw new ArgumentNullException(nameof(shapeConstraints));
            if (tilings.Any(t => t == null)) throw new ArgumentNullException(nameof(tilings));
            if (!shapeTemplates.AreDistinct(t => t.Name, StringComparer.InvariantCulture))
                throw new ArgumentException(Strings.Template_Template_ShapeNamesUnique);
            if (!shapeTemplates.SelectMany(t => t.EdgeNames.Concat(t.VertexNames))
                .AreDistinct(StringComparer.InvariantCulture))
                throw new ArgumentException(Strings.Template_Template_EdgeVertexNamesUnique);
            if (!tilings.AreDistinct(t => t.ID))
                throw new ArgumentException(Strings.Template_Template_TilingIDsUnique);

            HashSet<string> allEdges = new HashSet<string>(
                shapeTemplates.SelectMany(t => t.EdgeNames),
                StringComparer.InvariantCulture);

            if (tilings.Any(t => !allEdges.SetEquals(t.EdgePatterns.Select(p => p.EdgeName))))
                throw new ArgumentException(Strings.Template_Template_UnknownEdges, nameof(tilings));

            ShapeTemplates = shapeTemplates;
            Tilings = tilings;

            ShapeSet shapes = CreateShapes();
            foreach (IExpression<bool> constaint in shapeConstraints)
            {
                Debug.Assert(constaint != null, "constaint != null");
                if (!constaint.Evaluate(shapes))
                    throw new InvalidOperationException(Strings.Template_Template_InitialVertsInvalid);
            }

            ShapeConstraints = shapeConstraints.Select(e => e.Compile()).ToArray();
        }

        /// <summary>
        ///     Gets the templates of the shapes that can be used within this template.
        /// </summary>
        /// <value>
        ///     The shape templates.
        /// </value>
        [NotNull]
        [ItemNotNull]
        public IReadOnlyList<ShapeTemplate> ShapeTemplates { get; }

        /// <summary>
        ///     Gets the constraints, if any, on the angles and verticies in the shapes in the template.
        /// </summary>
        /// <value>
        ///     The shape constraints.
        /// </value>
        [NotNull]
        [ItemNotNull]
        public IReadOnlyList<IExpression<bool>> ShapeConstraints { get; }

        /// <summary>
        ///     Gets the definitions of the tilings that are possible for the shapes in this template.
        /// </summary>
        /// <value>
        ///     The tilings.
        /// </value>
        [NotNull]
        [ItemNotNull]
        public IReadOnlyList<TilingDefinition> Tilings { get; }

        /// <summary>
        ///     Creates shapes from the <see cref="ShapeTemplates" />.
        /// </summary>
        /// <returns></returns>
        [NotNull]
        public ShapeSet CreateShapes() => new ShapeSet(ShapeTemplates.Select(s => new Shape(s)).ToArray());

        [NotNull]
        public Tiling CreateTiling(IEnumerable<Shape> shapes)
        {
            throw new NotImplementedException();
        }
    }
}