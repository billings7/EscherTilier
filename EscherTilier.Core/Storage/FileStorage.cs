﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Xml.Linq;
using EscherTilier.Expressions;
using JetBrains.Annotations;

namespace EscherTilier.Storage
{
    public static class FileStorage
    {
        /// <summary>
        ///     The file storage version.
        /// </summary>
        [NotNull]
        private static readonly Version _version = new Version(1, 0);

        /// <summary>
        ///     The <see cref="_version" /> as a string.
        /// </summary>
        [NotNull]
        private static readonly string _versionString = _version.ToString();

        /// <summary>
        ///     Saves the given <see cref="Template" /> to a file.
        /// </summary>
        /// <param name="template">The template to save.</param>
        /// <param name="file">The path of the file to save to.</param>
        /// <exception cref="System.ArgumentNullException">
        ///     <paramref name="template" /> or <paramref name="file" /> is <see langword="null" />.
        /// </exception>
        public static void SaveTemplate([NotNull] Template template, [NotNull] string file)
        {
            if (template == null) throw new ArgumentNullException(nameof(template));
            if (file == null) throw new ArgumentNullException(nameof(file));
            using (FileStream stream = File.Create(file))
                SaveTemplate(template, stream);
        }

        /// <summary>
        ///     Saves the given <see cref="Template" /> to a <see cref="Stream" />.
        /// </summary>
        /// <param name="template">The template to save.</param>
        /// <param name="stream">The stream to save the template to.</param>
        /// <exception cref="System.ArgumentNullException">
        ///     <paramref name="template" /> or <paramref name="stream" /> is <see langword="null" />.
        /// </exception>
        public static void SaveTemplate([NotNull] Template template, [NotNull] Stream stream)
        {
            if (template == null) throw new ArgumentNullException(nameof(template));
            if (stream == null) throw new ArgumentNullException(nameof(stream));

            using (ZipArchive zip = new ZipArchive(stream, ZipArchiveMode.Create, true))
            {
                ZipArchiveEntry templateEntry = zip.CreateEntry("Template.xml", CompressionLevel.Optimal);

                Debug.Assert(templateEntry != null, "templateEntry != null");
                using (Stream templateStream = templateEntry.Open())
                {
                    Debug.Assert(templateStream != null, "templateStream != null");

                    XDocument doc = CreateDocument(DocumentType.Template);
                    Debug.Assert(doc.Root != null, "doc.Root != null");
                    Serialize(template, doc.Root);

                    doc.Save(
                        templateStream,
#if DEBUG
                        SaveOptions.None
#else
                    SaveOptions.DisableFormatting
#endif
                        );

                    templateStream.Flush();
                }

                // TODO Generate and save thumbnail to zip
            }
        }

        /// <summary>
        ///     Loads a <see cref="Template" /> from a file.
        /// </summary>
        /// <param name="file">The path of the file to load the template from.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">
        ///     <paramref name="file" /> is <see langword="null" />.
        /// </exception>
        [NotNull]
        public static Template LoadTemplate([NotNull] string file)
        {
            if (file == null) throw new ArgumentNullException(nameof(file));
            using (FileStream stream = File.OpenRead(file))
                return LoadTemplate(stream);
        }

        /// <summary>
        ///     Loads a <see cref="Template" /> from a <see cref="Stream" />.
        /// </summary>
        /// <param name="stream">The stream to load the template from.</param>
        /// <returns></returns>
        [NotNull]
        public static Template LoadTemplate([NotNull] Stream stream)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));

            using (ZipArchive zip = new ZipArchive(stream, ZipArchiveMode.Read, true))
            {
                ZipArchiveEntry templateEntry = zip.GetEntry("Template.xml");
                if (templateEntry == null) throw new InvalidDataException("Template definition missing.");

                XDocument doc;
                using (Stream templateStream = templateEntry.Open())
                    doc = XDocument.Load(templateStream);
                if (doc.Root == null)
                    throw new InvalidDataException("Template definition empty.");

                Version version;
                if (!Version.TryParse(doc.Root.Attribute("version")?.Value, out version))
                    throw new InvalidDataException("Invalid version number");

                if (version > _version)
                {
                    throw new InvalidDataException(
                        "The template was created for an earlier version of the application and cannot be loaded.");
                }
                // Deal with different versions if needed.

                return DeserializeTemplate(doc.Root);
            }
        }

        /// <summary>
        ///     The different types of document that can be saved.
        /// </summary>
        private enum DocumentType
        {
            Template,
            Tiling
        }

        /// <summary>
        ///     Creates an <see cref="XDocument" /> of a given document type.
        /// </summary>
        /// <param name="type">The type of document.</param>
        /// <returns></returns>
        [NotNull]
        private static XDocument CreateDocument(DocumentType type)
        {
            return new XDocument(
                new XElement(
                    type.ToString(),
                    new XAttribute("version", _versionString)));
        }

        /// <summary>
        ///     Serializes the specified template to an XML element.
        /// </summary>
        /// <param name="template">The template.</param>
        /// <param name="element">The element.</param>
        private static void Serialize([NotNull] Template template, [NotNull] XElement element)
        {
            element.Add(
                new XElement(
                    "Tilings",
                    template.Tilings.Select(Serialize)),
                new XElement(
                    "ShapeTemplates",
                    template.ShapeTemplates.Select(Serialize)),
                new XElement(
                    "ShapeConstraints",
                    template.ShapeConstraints.Select(Serialize)));
        }

        /// <summary>
        ///     Deserializes a template from an XML element.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns></returns>
        [NotNull]
        private static Template DeserializeTemplate([NotNull] XElement element)
        {
            TilingDefinition[] tilings = element
                .Element("Tilings")
                ?.Elements("Tiling")
                .Select(DeserializeTilingDefinition)
                .ToArray();

            if (tilings == null || tilings.Length < 1)
                throw new InvalidDataException("Tilings missing.");

            ShapeTemplate[] templates = element
                .Element("ShapeTemplates")
                ?.Elements("ShapeTemplate")
                .Select(DeserializeShapeTemplate)
                .ToArray();

            if (templates == null || templates.Length < 1)
                throw new InvalidDataException("Shape templates missing.");

            XElement consElm = element.Element("ShapeConstraints");
            if (consElm == null || consElm.IsEmpty)
                return new Template(templates, Array<IExpression<bool>>.Empty, tilings);

            Func<XElement, IExpression<bool>> selector = DeserializeExpression<bool>;

            IExpression<bool>[] constraints = consElm
                .Elements()
                .Select(selector)
                .ToArray();

            return new Template(templates, constraints, tilings);
        }

        /// <summary>
        ///     Serializes the specified tiling definition to an XML element.
        /// </summary>
        /// <param name="tilingDefinition">The tiling definition.</param>
        /// <returns></returns>
        [NotNull]
        private static XElement Serialize([NotNull] TilingDefinition tilingDefinition)
        {
            Dictionary<EdgePart, string> partIds = new Dictionary<EdgePart, string>();

            for (int i = 0; i < tilingDefinition.EdgePatterns.Count; i++)
            {
                EdgePattern pattern = tilingDefinition.EdgePatterns[i];

                Debug.Assert(pattern != null, "pattern != null");
                for (int j = 0; j < pattern.Parts.Count; j++)
                {
                    partIds.Add(pattern.Parts[j], i + ":" + j);
                }
            }

            return new XElement(
                "Tiling",
                new XAttribute("id", tilingDefinition.ID),
                new XElement(
                    "Condition",
                    tilingDefinition.Condition == null
                        ? null
                        : Serialize(tilingDefinition.Condition)),
                new XElement(
                    "EdgePatterns",
                    tilingDefinition.EdgePatterns.Select(Serialize)),
                new XElement(
                    "Adjacencies",
                    tilingDefinition.AdjacentParts.AllAdjacencies.Select(
                        t =>
                        {
                            string indsA = partIds[t.Item1.Value];
                            string indsB = partIds[t.Item2.Value];

                            Debug.Assert(indsA != null, "indsA != null");
                            Debug.Assert(indsB != null, "indsB != null");

                            return new XElement(
                                "Adjacency",
                                new XElement(
                                    "PartRef",
                                    new XAttribute("label", t.Item1.Label),
                                    new XAttribute("index", indsA)),
                                new XElement(
                                    "PartRef",
                                    new XAttribute("label", t.Item2.Label),
                                    new XAttribute("index", indsB)));
                        })));
        }

        /// <summary>
        ///     Deserializes a tiling definition from an XML element.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns></returns>
        [NotNull]
        private static TilingDefinition DeserializeTilingDefinition([NotNull] XElement element)
        {
            int id;

            if (!int.TryParse(element.Attribute("id")?.Value, out id))
                throw new InvalidDataException("Tiling definition ID missing or invalid.");

            XElement condElm = element.Element("Condition");

            IExpression<bool> condition = condElm == null || condElm.IsEmpty
                ? null
                : DeserializeExpression<bool>(condElm);

            XElement patternElm = element.Element("EdgePatterns");

            if (patternElm == null || patternElm.IsEmpty)
                throw new InvalidDataException("Edge patterns missing");

            EdgePattern[] patterns = patternElm
                .Elements("EdgePattern")
                .Select(DeserializeEdgePattern)
                .ToArray();

            Dictionary<string, EdgePart> partIds = new Dictionary<string, EdgePart>();

            for (int i = 0; i < patterns.Length; i++)
            {
                EdgePattern pattern = patterns[i];

                Debug.Assert(pattern != null, "pattern != null");
                for (int j = 0; j < pattern.Parts.Count; j++)
                {
                    partIds.Add(i + ":" + j, pattern.Parts[j]);
                }
            }

            AdjacencyGraph<Labeled<EdgePart>> adjacency = new AdjacencyGraph<Labeled<EdgePart>>();

            XElement adjsElm = element.Element("Adjacencies");
            if (adjsElm == null || adjsElm.IsEmpty)

                throw new InvalidDataException("Tiling definition edge adjacency list missing.");

            foreach (XElement adjElm in adjsElm.Elements("Adjacency"))
            {
                Debug.Assert(adjElm != null, "adjElm != null");

                XElement[] refElms = adjElm.Elements("PartRef").ToArray();

                if (refElms.Length != 2)
                    throw new InvalidDataException("Wrong number of parts to an adjacency.");

                Debug.Assert(refElms[0] != null, "refElms[0] != null");
                Debug.Assert(refElms[1] != null, "refElms[1] != null");

                adjacency.Add(
                    DeserializeLabeledEdgePart(refElms[0], partIds),
                    DeserializeLabeledEdgePart(refElms[1], partIds));
            }

            if (adjacency.Count < 1)
                throw new InvalidDataException("Edge pattern edge adjacency list missing.");

            return new TilingDefinition(id, condition, patterns, adjacency);
        }

        /// <summary>
        ///     Serializes the specified edge pattern to an XML element.
        /// </summary>
        /// <param name="edgePattern">The edge pattern.</param>
        /// <returns></returns>
        [NotNull]
        private static XElement Serialize([NotNull] EdgePattern edgePattern)
        {
            return new XElement(
                "EdgePattern",
                new XAttribute("edge", edgePattern.EdgeName),
                new XElement(
                    "Parts",
                    edgePattern.Parts.Select(Serialize)));
        }

        /// <summary>
        ///     Deserializes an edge pattern from an XML element.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns></returns>
        private static EdgePattern DeserializeEdgePattern([NotNull] XElement element)
        {
            string edge = element.Attribute("edge")?.Value;
            if (string.IsNullOrWhiteSpace(edge))
                throw new InvalidDataException("Edge pattern edge name missing.");

            EdgePart[] parts = element
                .Element("Parts")?
                .Elements("Part")
                .Select(DeserializeEdgePart)
                .ToArray();

            if (parts == null) throw new InvalidDataException("Edge pattern parts missing.");

            return new EdgePattern(edge, parts);
        }

        /// <summary>
        ///     Deserializes a labeled edge part from an XML element.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="parts">The parts.</param>
        /// <returns></returns>
        private static Labeled<EdgePart> DeserializeLabeledEdgePart(
            [NotNull] XElement element,
            [NotNull] Dictionary<string, EdgePart> parts)
        {
            string lbl = element.Attribute("label")?.Value;
            EdgePart part;

            if (lbl == null)
                throw new InvalidDataException("Adjacency label missing.");
            if (!parts.TryGetValue(element.Attribute("index")?.Value ?? string.Empty, out part))
                throw new InvalidDataException("Adjancency ID missing or invalid.");

            return new Labeled<EdgePart>(lbl, part);
        }

        /// <summary>
        ///     Serializes the specified edge part to an XML element.
        /// </summary>
        /// <param name="edgePart">The edge part.</param>
        /// <returns></returns>
        [NotNull]
        private static XElement Serialize([NotNull] EdgePart edgePart)
        {
            return new XElement(
                "Part",
                new XAttribute("id", edgePart.ID),
                new XAttribute("direction", edgePart.Direction),
                new XAttribute("amount", edgePart.Amount.ToString("R")));
        }

        /// <summary>
        ///     Deserializes an edge part from an XML element.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns></returns>
        [NotNull]
        private static EdgePart DeserializeEdgePart([NotNull] XElement element)
        {
            int id;
            PartDirection dir;
            float amount;

            if (!int.TryParse(element.Attribute("id")?.Value, out id))
                throw new InvalidDataException("Edge part ID missing or invalid.");
            if (!Enum.TryParse(element.Attribute("direction")?.Value, out dir))
                throw new InvalidDataException("Edge part direction missing or invalid.");
            if (!float.TryParse(element.Attribute("amount")?.Value, out amount))
                throw new InvalidDataException("Edge part amount missing or invalid.");

            return new EdgePart(id, dir, amount);
        }

        /// <summary>
        ///     Serializes the specified shape template to an XML element.
        /// </summary>
        /// <param name="shapeTemplate">The shape template.</param>
        /// <returns></returns>
        [NotNull]
        private static XElement Serialize([NotNull] ShapeTemplate shapeTemplate)
        {
            return new XElement(
                "ShapeTemplate",
                new XAttribute("name", shapeTemplate.Name),
                new XElement(
                    "Edges",
                    shapeTemplate.EdgeNames.Select(n => new XElement("Edge", n))),
                new XElement(
                    "Vertices",
                    shapeTemplate.VertexNames.Select(n => new XElement("Vertex", n))));
        }

        /// <summary>
        ///     Deserializes a shape template from an XML element.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns></returns>
        [NotNull]
        private static ShapeTemplate DeserializeShapeTemplate([NotNull] XElement element)
        {
            string name = element.Attribute("name")?.Value;

            if (name == null)
                throw new InvalidDataException("Shape template name missing.");

            string[] edges = element.Element("Edges")
                ?.Elements("Edge")
                .Select(e => e.Value)
                .ToArray();

            if (edges == null) throw new InvalidDataException("Shape edges missing.");

            string[] vertices = element.Element("Vertices")
                ?.Elements("Vertex")
                .Select(e => e.Value)
                .ToArray();

            if (vertices == null) throw new InvalidDataException("Shape vertices missing.");

            return new ShapeTemplate(name, edges, vertices);
        }

        [NotNull]
        private static XElement Serialize<T>([NotNull] IExpression<T> expression) { }

        [NotNull]
        private static IExpression<T> DeserializeExpression<T>([NotNull] XElement element) { }
    }
}