using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using EscherTilier.Expressions;
using EscherTilier.Styles;
using EscherTilier.Utilities;
using JetBrains.Annotations;

namespace EscherTilier
{
    /// <summary>
    ///     Defines the base shapes and tiling definitions that can be used to create a <see cref="Tiling" />.
    /// </summary>
    public class Template
    {
        private readonly Dictionary<string, ShapeTemplate> _shapeTemplateByEdgeName;

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
            [NotNull][ItemNotNull] IReadOnlyList<ShapeTemplate> shapeTemplates,
            [NotNull][ItemNotNull] IReadOnlyList<IExpression<bool>> shapeConstraints,
            [NotNull][ItemNotNull] IReadOnlyList<TilingDefinition> tilings)
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

            _shapeTemplateByEdgeName =
                shapeTemplates.SelectMany(
                    st => st.EdgeNames
                        .Select(e => new KeyValuePair<string, ShapeTemplate>(e, st)))
                    .ToDictionary(kvp => kvp.Key, kvp => kvp.Value, StringComparer.InvariantCulture);

            foreach (TilingDefinition tiling in tilings)
            {
                tiling.Template = this;
                foreach (EdgePattern pattern in tiling.EdgePatterns)
                {
                    // ReSharper disable once AssignNullToNotNullAttribute
                    pattern.ShapeTemplate = _shapeTemplateByEdgeName[pattern.EdgeName];
                }
            }
            foreach (ShapeTemplate template in shapeTemplates)
            {
                template.Template = this;
                template.EdgePatterns = tilings.ToDictionary(t => t, t => t.EdgePatterns);
                template.EdgeParts = tilings.ToDictionary(
                    t => t,
                    t => (IReadOnlyList<EdgePart>) t.EdgePatterns.SelectMany(ep => ep.Parts).ToArray());
            }
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

        /// <summary>
        /// Creates a tiling from this template.
        /// </summary>
        /// <param name="tilingDefinition">The tiling definition.</param>
        /// <param name="shapes">The shapes.</param>
        /// <param name="styleManager">The initial style manager.</param>
        /// <returns></returns>
        [NotNull]
        public Tiling CreateTiling(
            [NotNull] TilingDefinition tilingDefinition,
            [NotNull] ShapeSet shapes,
            [NotNull] StyleManager styleManager)
        {
            if (tilingDefinition == null) throw new ArgumentNullException(nameof(tilingDefinition));
            if (shapes == null) throw new ArgumentNullException(nameof(shapes));
            if (styleManager == null) throw new ArgumentNullException(nameof(styleManager));
            if (!Tilings.Contains(tilingDefinition))
            {
                throw new ArgumentException(
                    Strings.Template_CreateTiling_UnknownTilingDefinition,
                    nameof(tilingDefinition));
            }

            HashSet<ShapeTemplate> templates = new HashSet<ShapeTemplate>(shapes.Select(s => s.Template));

            if (!templates.SetEquals(ShapeTemplates))
                throw new ArgumentException(Strings.Template_CreateTiling_WrongShapes, nameof(shapes));

            Dictionary<int, ShapeLines> shapeLines = new Dictionary<int, ShapeLines>();

            List<Tile> tiles = new List<Tile>(shapes.Count);

            // TODO Order shapes so that the nth shape is adjacent to at least one of the previous shapes
            foreach (Shape shape in shapes)
            {
                Debug.Assert(shape != null, "shape != null");

                string label = null;
                Matrix3x2 transform = default(Matrix3x2);

                if (tiles.Count < 1)
                {
                    label = tilingDefinition.AdjacentParts
                        .Where(l => l.Value.ShapeTemplate == shape.Template)
                        .OrderBy(l => l.Label, StringComparer.InvariantCulture)
                        .First().Label;
                    transform = Matrix3x2.Identity;
                }
                else
                {
                    foreach (Tile t in tiles)
                    {
                        foreach (EdgePartShape partShape in t.PartShapes)
                        {
                            Debug.Assert(partShape != null, "partShape != null");

                            Labeled<EdgePart> adjacent;
                            if (!tilingDefinition.AdjacentParts.TryGetAdjacent(
                                partShape.Part.WithLabel(t.Label),
                                out adjacent))
                                continue;

                            Debug.Assert(adjacent.Value != null, "adjacent.Value != null");
                            if (adjacent.Value.ShapeTemplate != shape.Template)
                                continue;

                            label = adjacent.Label;
                            transform = EdgePartPosition.Create(adjacent.Value, shape)
                                .GetTransformTo(t.GetEdgePartPosition(partShape.Part));
                        }

                        if (label != null) break;
                    }

                    if (label == null) throw new InvalidDataException();
                }

                EdgePartShape[] partShapes = shape.Template.EdgeParts[tilingDefinition]
                    .Select(
                        ep => new EdgePartShape(
                            ep,
                            shapes.GetEdge(ep.EdgePattern.EdgeName),
                            shapeLines.GetOrAdd(ep.ID, _ => ShapeLines.CreateDefault())))
                    .ToArray();

                Tile tile = new Tile(label, shape, transform, partShapes);
                tile.Style = styleManager.GetStyle(tile);
                tiles.Add(tile);
            }

            return new Tiling(this, tilingDefinition, tiles, styleManager);
        }
    }

    public class ShapeLines : List<ILine>
    {
        public static ShapeLines CreateDefault()
        {
            return new ShapeLines
            {
                new Line(Vector2.Zero, new Vector2(1, 0))
            };
        }
    }
}