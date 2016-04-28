using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Numerics;
using System.Threading;
using System.Xml.Linq;
using EscherTiler.Expressions;
using EscherTiler.Graphics;
using EscherTiler.Styles;
using EscherTiler.Utilities;
using JetBrains.Annotations;

namespace EscherTiler.Storage
{
    public static class FileStorage
    {
        private const NumberStyles FloatNumberStyle = NumberStyles.Float | NumberStyles.AllowThousands;

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

        [NotNull]
        private static readonly CultureInfo _culture = CultureInfo.InvariantCulture;

        [NotNull]
        private static readonly ThreadLocal<Dictionary<IImage, string>> _imageFileNames =
            new ThreadLocal<Dictionary<IImage, string>>(() => new Dictionary<IImage, string>());

        [NotNull]
        private static readonly ThreadLocal<Dictionary<string, IImage>> _fileNameImages =
            new ThreadLocal<Dictionary<string, IImage>>(() => new Dictionary<string, IImage>());

        /// <summary>
        ///     Saves the given <see cref="Template" /> to a file.
        /// </summary>
        /// <param name="template">The template to save.</param>
        /// <param name="file">The path of the file to save to.</param>
        /// <param name="thumbnail">The thumbnail.</param>
        /// <exception cref="System.ArgumentNullException">
        ///     <paramref name="template" /> or <paramref name="file" /> is <see langword="null" />.
        /// </exception>
        public static void SaveTemplate(
            [NotNull] Template template,
            [NotNull] string file,
            [CanBeNull] IImage thumbnail = null)
        {
            if (template == null) throw new ArgumentNullException(nameof(template));
            if (file == null) throw new ArgumentNullException(nameof(file));
            using (FileStream stream = File.Create(file))
                SaveTemplate(template, stream, thumbnail);
        }

        /// <summary>
        ///     Saves the given <see cref="Template" /> to a <see cref="Stream" />.
        /// </summary>
        /// <param name="template">The template to save.</param>
        /// <param name="stream">The stream to save the template to.</param>
        /// <param name="thumbnail">The thumbnail.</param>
        /// <exception cref="System.ArgumentNullException">
        ///     <paramref name="template" /> or <paramref name="stream" /> is <see langword="null" />.
        /// </exception>
        public static void SaveTemplate(
            [NotNull] Template template,
            [NotNull] Stream stream,
            [CanBeNull] IImage thumbnail = null)
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
                }

                if (thumbnail != null)
                {
                    string extension = thumbnail.Format.GetFormatExtension();
                    string name = $"Thumbnail.{extension}";

                    ZipArchiveEntry thumbnailEntry = zip.CreateEntry(name, CompressionLevel.Optimal);

                    Debug.Assert(thumbnailEntry != null, "thumbnailEntry != null");
                    using (Stream destStream = thumbnailEntry.Open())
                    using (Stream sourceStream = thumbnail.GetStream())
                    {
                        Debug.Assert(destStream != null, "destStream != null");
                        sourceStream.CopyTo(destStream);
                    }
                }
            }
        }

        /// <summary>
        ///     Loads a <see cref="Template" /> from a file.
        /// </summary>
        /// <param name="file">The path of the file to load the template from.</param>
        /// <param name="thumbnail">The thumbnail.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="file" /> is <see langword="null" />.</exception>
        [NotNull]
        public static Template LoadTemplate([NotNull] string file, out IImage thumbnail)
        {
            if (file == null) throw new ArgumentNullException(nameof(file));
            using (FileStream stream = File.OpenRead(file))
                return LoadTemplate(stream, out thumbnail);
        }

        /// <summary>
        ///     Loads a <see cref="Template" /> from a <see cref="Stream" />.
        /// </summary>
        /// <param name="stream">The stream to load the template from.</param>
        /// <param name="thumbnail">The thumbnail.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="InvalidDataException">
        ///     Template definition missing.
        ///     or
        ///     Template definition empty.
        ///     or
        ///     Invalid version number
        ///     or
        ///     The template was created for an earlier version of the application and cannot be loaded.
        /// </exception>
        [NotNull]
        public static Template LoadTemplate([NotNull] Stream stream, out IImage thumbnail)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));

            using (ZipArchive zip = new ZipArchive(stream, ZipArchiveMode.Read, true))
            {
                // Load the thumbnail
                ZipArchiveEntry thumbnailEntry = zip.Entries.FirstOrDefault(e => e.Name.StartsWith("Thumbnail."));
                if (thumbnailEntry == null)
                    thumbnail = null;
                else
                {
                    using (Stream thumbnailStream = thumbnailEntry.Open())
                    using (BinaryReader reader = new BinaryReader(thumbnailStream))
                    {
                        Debug.Assert(thumbnailEntry.Length < int.MaxValue);

                        byte[] data = reader.ReadBytes((int)thumbnailEntry.Length);

                        thumbnail = new MemoryImage(data);
                    }
                }

                // Load the template
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
        ///     Saves the given <see cref="Tiling" /> to a file.
        /// </summary>
        /// <param name="tiling">The tiling to save.</param>
        /// <param name="file">The path of the file to save to.</param>
        /// <param name="thumbnail">The thumbnail.</param>
        /// <exception cref="System.ArgumentNullException">
        ///     <paramref name="tiling" /> or <paramref name="file" /> is <see langword="null" />.
        /// </exception>
        public static void SaveTiling(
            [NotNull] Tiling tiling,
            [NotNull] string file,
            [CanBeNull] IImage thumbnail = null)
        {
            if (tiling == null) throw new ArgumentNullException(nameof(tiling));
            if (file == null) throw new ArgumentNullException(nameof(file));
            using (FileStream stream = File.Create(file))
                SaveTiling(tiling, stream, thumbnail);
        }

        /// <summary>
        ///     Saves the given <see cref="Tiling" /> to a <see cref="Stream" />.
        /// </summary>
        /// <param name="tiling">The tiling to save.</param>
        /// <param name="stream">The stream to save the tiling to.</param>
        /// <param name="thumbnail">The thumbnail.</param>
        /// <exception cref="System.ArgumentNullException">
        ///     <paramref name="tiling" /> or <paramref name="stream" /> is <see langword="null" />.
        /// </exception>
        public static void SaveTiling([NotNull] Tiling tiling, [NotNull] Stream stream,
            [CanBeNull] IImage thumbnail = null)
        {
            if (tiling == null) throw new ArgumentNullException(nameof(tiling));
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
                    Serialize(tiling.Template, doc.Root);

                    doc.Save(
                        templateStream,
#if DEBUG
                        SaveOptions.None
#else
                        SaveOptions.DisableFormatting
#endif
                        );
                }

                Dictionary<IImage, string> imageFileNames = _imageFileNames.Value;
                Debug.Assert(imageFileNames != null, "imageFileNames != null");

                imageFileNames.Clear();

                int imgId = 0;
                foreach (ImageStyle imageStyle in tiling.StyleManager.Styles.Select(s => s.Style).OfType<ImageStyle>())
                {
                    Debug.Assert(imageStyle != null, "imageStyle != null");

                    string extension = imageStyle.Image.Format.GetFormatExtension();
                    string name = $"Image{imgId++}.{extension}";

                    imageFileNames.Add(imageStyle.Image, name);

                    ZipArchiveEntry imgEntry = zip.CreateEntry(name, CompressionLevel.Optimal);

                    Debug.Assert(imgEntry != null, "imgEntry != null");
                    using (Stream destStream = imgEntry.Open())
                    using (Stream sourceStream = imageStyle.Image.GetStream())
                    {
                        Debug.Assert(destStream != null, "destStream != null");
                        sourceStream.CopyTo(destStream);
                    }
                }

                ZipArchiveEntry tilingEntry = zip.CreateEntry("Tiling.xml", CompressionLevel.Optimal);

                Debug.Assert(tilingEntry != null, "tilingEntry != null");
                using (Stream tilingStream = tilingEntry.Open())
                {
                    Debug.Assert(tilingStream != null, "templateStream != null");

                    XDocument doc = CreateDocument(DocumentType.Tiling);
                    Debug.Assert(doc.Root != null, "doc.Root != null");
                    Serialize(tiling, doc.Root);

                    doc.Save(
                        tilingStream,
#if DEBUG
                        SaveOptions.None
#else
                        SaveOptions.DisableFormatting
#endif
                        );
                }

                if (thumbnail != null)
                {
                    string extension = thumbnail.Format.GetFormatExtension();
                    string name = $"Thumbnail.{extension}";

                    ZipArchiveEntry thumbnailEntry = zip.CreateEntry(name, CompressionLevel.Optimal);

                    Debug.Assert(thumbnailEntry != null, "thumbnailEntry != null");
                    using (Stream destStream = thumbnailEntry.Open())
                    using (Stream sourceStream = thumbnail.GetStream())
                    {
                        Debug.Assert(destStream != null, "destStream != null");
                        sourceStream.CopyTo(destStream);
                    }
                }
            }
        }

        /// <summary>
        ///     Loads a <see cref="Tiling" /> from a file.
        /// </summary>
        /// <param name="file">The path of the file to load the tiling from.</param>
        /// <param name="thumbnail">The thumbnail.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="file" /> is <see langword="null" />.</exception>
        [NotNull]
        public static Tiling LoadTiling([NotNull] string file, out IImage thumbnail)
        {
            if (file == null) throw new ArgumentNullException(nameof(file));
            using (FileStream stream = File.OpenRead(file))
                return LoadTiling(stream, out thumbnail);
        }

        /// <summary>
        ///     Loads a <see cref="Tiling" /> from a <see cref="Stream" />.
        /// </summary>
        /// <param name="stream">The stream to load the tiling from.</param>
        /// <param name="thumbnail">The thumbnail.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="InvalidDataException">
        ///     Template definition missing.
        ///     or
        ///     Template definition empty.
        ///     or
        ///     Invalid version number
        ///     or
        ///     The template was created for an earlier version of the application and cannot be loaded.
        /// </exception>
        [NotNull]
        public static Tiling LoadTiling([NotNull] Stream stream, out IImage thumbnail)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));

            using (ZipArchive zip = new ZipArchive(stream, ZipArchiveMode.Read, true))
            {
                // Load the thumbnail
                ZipArchiveEntry thumbnailEntry = zip.Entries.FirstOrDefault(e => e.Name.StartsWith("Thumbnail."));
                if (thumbnailEntry == null)
                    thumbnail = null;
                else
                {
                    using (Stream thumbnailStream = thumbnailEntry.Open())
                    using (BinaryReader reader = new BinaryReader(thumbnailStream))
                    {
                        Debug.Assert(thumbnailEntry.Length < int.MaxValue);

                        byte[] data = reader.ReadBytes((int)thumbnailEntry.Length);

                        thumbnail = new MemoryImage(data);
                    }
                }

                // Load the template
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

                Template template = DeserializeTemplate(doc.Root);

                Dictionary<string, IImage> fileNameImages = _fileNameImages.Value;
                Debug.Assert(fileNameImages != null, "imageFileNames != null");

                // Load images
                fileNameImages.Clear();

                foreach (ZipArchiveEntry imgEntry in zip.Entries.Where(e => e.Name.StartsWith("Image")))
                {
                    IImage image;
                    using (Stream imgStream = imgEntry.Open())
                    using (BinaryReader reader = new BinaryReader(imgStream))
                    {
                        Debug.Assert(imgEntry.Length < int.MaxValue);

                        byte[] data = reader.ReadBytes((int)imgEntry.Length);

                        image = new MemoryImage(data);
                    }

                    fileNameImages.Add(imgEntry.Name, image);
                }

                // Load the tiling
                ZipArchiveEntry tilingEntry = zip.GetEntry("Tiling.xml");
                if (tilingEntry == null) throw new InvalidDataException("Tiling definition missing.");

                using (Stream tilingStream = tilingEntry.Open())
                    doc = XDocument.Load(tilingStream);
                if (doc.Root == null)
                    throw new InvalidDataException("Tiling definition empty.");

                if (!Version.TryParse(doc.Root.Attribute("version")?.Value, out version))
                    throw new InvalidDataException("Invalid version number");

                if (version > _version)
                {
                    throw new InvalidDataException(
                        "The template was created for an earlier version of the application and cannot be loaded.");
                }
                // Deal with different versions if needed.

                return DeserializeTiling(doc.Root, template);
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
                    template.Tilings.Values.Select(Serialize)),
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
                return new Template(templates, Array.Empty<IExpression<bool>>(), tilings);

            IExpression<bool>[] constraints = consElm
                .Elements()
                .Select(DeserializeExpressionBool)
                .ToArray();

            return new Template(templates, constraints, tilings);
        }

        /// <summary>
        ///     Serializes the specified tiling to an XML element.
        /// </summary>
        /// <param name="tiling">The tiling.</param>
        /// <param name="element">The element.</param>
        private static void Serialize([NotNull] Tiling tiling, [NotNull] XElement element)
        {
            element.Add(
                new XAttribute("definitionId", tiling.Definition.ID.ToString(_culture)),
                new XElement(
                    "StyleManager",
                    Serialize(tiling.StyleManager)),
                new XElement(
                    "Tiles",
                    tiling.Tiles.Select(Serialize)));
        }

        /// <summary>
        ///     Deserializes a tiling from an XML element.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="template">The template.</param>
        /// <returns></returns>
        [NotNull]
        private static Tiling DeserializeTiling([NotNull] XElement element, [NotNull] Template template)
        {
            int definitionId;

            if (!int.TryParse(element.Attribute("definitionId")?.Value, out definitionId))
                throw new InvalidDataException("Tiling definition ID missing or invalid.");

            TilingDefinition definition;
            if (!template.Tilings.TryGetValue(definitionId, out definition))
                throw new InvalidDataException("Tiling definition ID does not exist.");

            XElement styleManagerElm = element.Element("StyleManager");

            if (styleManagerElm == null || styleManagerElm.IsEmpty)
                throw new InvalidDataException("Style manager missing");

            StyleManager styleManager = DeserializeStyleManager(styleManagerElm);
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
                    partIds.Add(
                        pattern.Parts[j],
                        i.ToString(_culture) + ":" + j.ToString(_culture));
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
                : DeserializeExpressionBool(condElm);

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
                    partIds.Add(
                        i.ToString(_culture) + ":" + j.ToString(_culture),
                        pattern.Parts[j]);
                }
            }

            EdgePartAdjacencies adjacency = new EdgePartAdjacencies();

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
                new XAttribute("id", edgePart.ID.ToString(_culture)),
                new XAttribute("clockwise", edgePart.IsClockwise),
                new XAttribute("amount", edgePart.Amount.ToString("R", _culture)));
        }

        /// <summary>
        ///     Deserializes an edge part from an XML element.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns></returns>
        private static EdgePart DeserializeEdgePart([NotNull] XElement element)
        {
            int id;
            bool clockwise;
            float amount;

            if (!int.TryParse(element.Attribute("id")?.Value, NumberStyles.Integer, _culture, out id))
                throw new InvalidDataException("Edge part ID missing or invalid.");
            if (!bool.TryParse(element.Attribute("clockwise")?.Value, out clockwise))
                throw new InvalidDataException("Edge part clockwise flag missing or invalid.");
            if (!float.TryParse(element.Attribute("amount")?.Value, FloatNumberStyle, _culture, out amount))
                throw new InvalidDataException("Edge part amount missing or invalid.");

            return new EdgePart(id, amount, clockwise);
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
                    shapeTemplate.VertexNames
                        .Zip(
                            shapeTemplate.InitialVertices,
                            (name, loc) => Serialize(loc, "Vertex", name))));
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

            XElement vertsElm = element.Element("Vertices");

            if (vertsElm == null) throw new InvalidDataException("Shape vertices missing.");

            List<string> vertNames = new List<string>();
            List<Vector2> vertLocs = new List<Vector2>();

            foreach (XElement e in vertsElm.Elements("Vertex"))
            {
                float x, y;

                if (!float.TryParse(e.Attribute("x")?.Value, FloatNumberStyle, _culture, out x))
                    throw new InvalidDataException("Vertex X coordinate missing or invalid.");
                if (!float.TryParse(e.Attribute("y")?.Value, FloatNumberStyle, _culture, out y))
                    throw new InvalidDataException("Vertex Y coordinate missing or invalid.");

                vertNames.Add(e.Value);
                vertLocs.Add(new Vector2(x, y));
            }

            return new ShapeTemplate(name, edges, vertNames.ToArray(), vertLocs.ToArray());
        }

        /// <summary>
        ///     Serializes the specified style manager to an XML element.
        /// </summary>
        /// <param name="styleManager">The style manager.</param>
        /// <returns></returns>
        [NotNull]
        private static XElement Serialize([NotNull] StyleManager styleManager)
        {
            XElement element = new XElement(
                "StyleManager",
                new XAttribute("type", styleManager.GetType().FullName),
                new XElement(
                    "LineStyle",
                    new XAttribute("width", styleManager.LineStyle.Width),
                    Serialize(styleManager.LineStyle.Style)),
                new XElement(
                    "Styles",
                    styleManager.Styles.Select(Serialize)));

            RandomStyleManager randomManager;
            GreedyStyleManager greedyManager;

            if ((randomManager = styleManager as RandomStyleManager) != null)
                element.Add(new XElement("Seed", randomManager.Seed.ToString(_culture)));
            else if ((greedyManager = styleManager as GreedyStyleManager) != null)
            {
                element.Add(new XElement("ParamA", greedyManager.ParamA.ToString(_culture)));
                element.Add(new XElement("ParamB", greedyManager.ParamB.ToString(_culture)));
                element.Add(new XElement("ParamC", greedyManager.ParamC.ToString(_culture)));
            }

            return element;
        }

        /// <summary>
        ///     Serializes the specified tile style to an XML element.
        /// </summary>
        /// <param name="tileStyle">The tile style.</param>
        /// <returns></returns>
        [NotNull]
        private static XElement Serialize([NotNull] TileStyle tileStyle)
        {
            return new XElement(
                "TileStyle",
                new XElement("Style", Serialize(tileStyle.Style)),
                new XElement(
                    "Shapes",
                    tileStyle.Shapes.Select(
                        s => new XElement("Shape", s.Template.Name))));
        }

        /// <summary>
        ///     Serializes the specified style to an XML element.
        /// </summary>
        /// <param name="style">The style.</param>
        /// <returns></returns>
        [NotNull]
        private static XElement Serialize([NotNull] IStyle style)
        {
            XElement element = new XElement(style.Type.ToString());

            switch (style.Type)
            {
                case StyleType.SolidColour:
                    SolidColourStyle solidColour = (SolidColourStyle) style;
                    element.Add(Serialize(solidColour.Colour, "Colour"));
                    break;
                case StyleType.RandomColour:
                    RandomColourStyle randomColour = (RandomColourStyle) style;
                    element.Add(Serialize(randomColour.From, "From"));
                    element.Add(Serialize(randomColour.To, "To"));
                    break;
                case StyleType.LinearGradient:
                    LinearGradientStyle linearGradient = (LinearGradientStyle) style;
                    element.Add(Serialize(linearGradient.Start, "Start"));
                    element.Add(Serialize(linearGradient.End, "End"));
                    element.Add(new XElement("GradientStops", linearGradient.GradientStops.Select(Serialize)));
                    break;
                case StyleType.RadialGradient:
                    RadialGradientStyle radialGradient = (RadialGradientStyle) style;
                    element.Add(Serialize(radialGradient.UnitOriginOffset, "UnitOriginOffset"));
                    element.Add(Serialize(radialGradient.GradientTransform, "Transform"));
                    element.Add(new XElement("GradientStops", radialGradient.GradientStops.Select(Serialize)));
                    break;
                case StyleType.Image:
                    ImageStyle image = (ImageStyle) style;
                    element.Add(new XAttribute("file", _imageFileNames.Value[image.Image]));
                    element.Add(Serialize(image.ImageTransform, "Transform"));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return element;
        }

        /// <summary>
        ///     Serializes the specified tile to an XML element.
        /// </summary>
        /// <param name="tile">The tile.</param>
        /// <returns></returns>
        [NotNull]
        private static XElement Serialize([NotNull] Tile tile)
        {
            return new XElement(
                "Tile",
                new XAttribute("label", tile.Label),
                new XAttribute("shape", tile.Shape.Template.Name),
                Serialize(tile.Transform, "Transform"),
                new XElement("PartShapes", tile.PartShapes.Select(Serialize)));
        }

        /// <summary>
        ///     Serializes the specified edge part shape to an XML element.
        /// </summary>
        /// <param name="edgePartShape">The edge part shape.</param>
        /// <returns></returns>
        [NotNull]
        private static XElement Serialize([NotNull] EdgePartShape edgePartShape)
        {
            return new XElement(
                "EdgePartShape",
                new XAttribute("part", edgePartShape.Part.ID.ToString(_culture)),
                new XAttribute("edge", edgePartShape.Edge.Name),
                edgePartShape.Lines.Select(Serialize));
        }

        /// <summary>
        ///     Serializes the specified line to an XML element.
        /// </summary>
        /// <param name="line">The line.</param>
        /// <returns></returns>
        [NotNull]
        private static XElement Serialize([NotNull] ILine line)
        {
            XElement element = new XElement(line.Type.ToString());

            element.Add(Serialize(line.Start, "Start"));
            element.Add(Serialize(line.End, "End"));

            switch (line.Type)
            {
                case LineType.Line:
                    break;
                case LineType.QuadraticBezierCurve:
                    QuadraticBezierCurve quadratic = (QuadraticBezierCurve) line;
                    element.Add(Serialize(quadratic.ControlPoint, "ControlPoint"));
                    break;
                case LineType.CubicBezierCurve:
                    CubicBezierCurve cubic = (CubicBezierCurve) line;
                    element.Add(Serialize(cubic.ControlPointA, "ControlPointA"));
                    element.Add(Serialize(cubic.ControlPointB, "ControlPointB"));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return element;
        }

        /// <summary>
        ///     Serializes the specified vector to an XML element.
        /// </summary>
        /// <param name="vector">The vector.</param>
        /// <param name="name">The name of the created element.</param>
        /// <param name="content">Any additional content to store in the element.</param>
        /// <returns></returns>
        private static XElement Serialize(Vector2 vector, [NotNull] string name, [NotNull] params object[] content)
        {
            return new XElement(
                name,
                new XAttribute("x", vector.X.ToString("R", _culture)),
                new XAttribute("y", vector.Y.ToString("R", _culture)),
                content);
        }

        /// <summary>
        ///     Serializes the specified vector to an XML element.
        /// </summary>
        /// <param name="vector">The vector.</param>
        /// <param name="name">The name of the created element.</param>
        /// <param name="content">Any additional content to store in the element.</param>
        /// <returns></returns>
        private static XElement Serialize(
            [NotNull] LineVector vector,
            [NotNull] string name,
            [NotNull] params object[] content)
        {
            return Serialize(vector.Vector, name, new XAttribute("fixed", vector.IsFixed.ToString(_culture)), content);
        }

        /// <summary>
        ///     Serializes the specified colour to an XML element.
        /// </summary>
        /// <param name="colour">The colour.</param>
        /// <param name="name">The name of the created element.</param>
        /// <param name="content">Any additional content to store in the element.</param>
        /// <returns></returns>
        private static XElement Serialize(Colour colour, [NotNull] string name, [NotNull] params object[] content)
        {
            return new XElement(
                name,
                new XAttribute("alpha", colour.A.ToString("R", _culture)),
                new XAttribute("red", colour.R.ToString("R", _culture)),
                new XAttribute("green", colour.G.ToString("R", _culture)),
                new XAttribute("blue", colour.B.ToString("R", _culture)),
                content);
        }

        /// <summary>
        ///     Serializes the specified gradient stop to an XML element.
        /// </summary>
        /// <param name="stop">The gradient stop.</param>
        /// <returns></returns>
        private static XElement Serialize(GradientStop stop)
        {
            return Serialize(stop.Colour, "GradientStop", stop.Position.ToString("R", _culture));
        }

        /// <summary>
        ///     Serializes the specified matrix to an XML element.
        /// </summary>
        /// <param name="matrix">The matrix.</param>
        /// <param name="name">The name of the created element.</param>
        /// <param name="content">Any additional content to store in the element.</param>
        /// <returns></returns>
        private static XElement Serialize(Matrix3x2 matrix, [NotNull] string name, [NotNull] params object[] content)
        {
            return new XElement(
                name,
                new XAttribute("m11", matrix.M11.ToString("R", _culture)),
                new XAttribute("m12", matrix.M12.ToString("R", _culture)),
                new XAttribute("m21", matrix.M21.ToString("R", _culture)),
                new XAttribute("m22", matrix.M22.ToString("R", _culture)),
                new XAttribute("m31", matrix.M31.ToString("R", _culture)),
                new XAttribute("m32", matrix.M32.ToString("R", _culture)),
                content);
        }

        /// <summary>
        ///     Serializes the specified expression to an XML element.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        [NotNull]
        private static XElement Serialize([NotNull] IExpression expression)
        {
            Stack<IExpression, XElement> stack = new Stack<IExpression, XElement>();
            XElement rootElement = new XElement(expression.ExpressionType.ToString());
            stack.Push(expression, rootElement);

            XElement element;
            while (stack.TryPop(out expression, out element))
            {
                Debug.Assert(expression != null, "expression != null");
                Debug.Assert(element != null, "element != null");

                switch (expression.ExpressionType)
                {
                    case ExpressionType.Compiled:
                        ICompiledExpression compiledExpression = (ICompiledExpression) expression;
                        stack.Push(compiledExpression.RawExpression, element);
                        break;
                    case ExpressionType.Number:
                        NumberExpression numberExpression = (NumberExpression) expression;
                        element.Value = numberExpression.Value.ToString("R", _culture);
                        break;
                    case ExpressionType.Edge:
                        EdgeExpression edgeExpression = (EdgeExpression) expression;
                        element.Value = edgeExpression.EdgeName;
                        break;
                    case ExpressionType.Vertex:
                        VertexExpression vertexExpression = (VertexExpression) expression;
                        element.Value = vertexExpression.VertexName;
                        break;
                    case ExpressionType.Add:
                    case ExpressionType.Subtract:
                    case ExpressionType.Multiply:
                    case ExpressionType.Divide:
                        ArithmeticExpression arithmeticExpression = (ArithmeticExpression) expression;
                        foreach (IExpression<float> exp in arithmeticExpression.Expressions)
                        {
                            XElement elm = new XElement(exp.ExpressionType.ToString());
                            stack.Push(expression, elm);
                            element.Add(elm);
                        }
                        break;
                    case ExpressionType.Equal:
                    case ExpressionType.NotEqual:
                    case ExpressionType.GreaterThan:
                    case ExpressionType.LessThan:
                    case ExpressionType.GreaterThanOrEqual:
                    case ExpressionType.LessThanOrEqual:
                        ComparisonExpression comparisonExpression = (ComparisonExpression) expression;
                        foreach (IExpression<float> exp in comparisonExpression.Expressions)
                        {
                            XElement elm = new XElement(exp.ExpressionType.ToString());
                            stack.Push(expression, elm);
                            element.Add(elm);
                        }
                        break;
                    case ExpressionType.And:
                    case ExpressionType.Or:
                    case ExpressionType.BoolEqual:
                    case ExpressionType.Xor:
                    case ExpressionType.Not:
                        LogicalExpression logicalExpression = (LogicalExpression) expression;
                        foreach (IExpression<bool> exp in logicalExpression.Expressions)
                        {
                            XElement elm = new XElement(exp.ExpressionType.ToString());
                            stack.Push(expression, elm);
                            element.Add(elm);
                        }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return rootElement;
        }

        /// <summary>
        ///     Deserializes an expression from an XML element.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns></returns>
        [NotNull]
        private static IExpression<float> DeserializeExpressionFloat([NotNull] XElement element)
        {
            IExpression<float> result = DeserializeExpression(element) as IExpression<float>;
            if (result == null) throw new InvalidDataException("Expression was of an unexpected type.");
            return result;
        }

        /// <summary>
        ///     Deserializes an expression from an XML element.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns></returns>
        [NotNull]
        private static IExpression<bool> DeserializeExpressionBool([NotNull] XElement element)
        {
            IExpression<bool> result = DeserializeExpression(element) as IExpression<bool>;
            if (result == null) throw new InvalidDataException("Expression was of an unexpected type.");
            return result;
        }

        /// <summary>
        ///     Deserializes an expression from an XML element.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns></returns>
        [NotNull]
        private static IExpression DeserializeExpression([NotNull] XElement element)
        {
            ExpressionType type;
            if (!Enum.TryParse(element.Name.LocalName, out type))
                throw new InvalidDataException("Invalid expression type.");

            switch (type)
            {
                case ExpressionType.Number:
                    float number;

                    if (!float.TryParse(element.Value, FloatNumberStyle, _culture, out number))
                        throw new InvalidDataException("Invalid number.");

                    return new NumberExpression(number);
                case ExpressionType.Edge:
                    return new EdgeExpression(element.Value);
                case ExpressionType.Vertex:
                    return new VertexExpression(element.Value);
                case ExpressionType.Add:
                    return ArithmeticExpression.Add(element.Elements().Select(DeserializeExpressionFloat).ToArray());
                case ExpressionType.Subtract:
                    return ArithmeticExpression.Subtract(
                        element.Elements().Select(DeserializeExpressionFloat).ToArray());
                case ExpressionType.Multiply:
                    return ArithmeticExpression.Multiply(
                        element.Elements().Select(DeserializeExpressionFloat).ToArray());
                case ExpressionType.Divide:
                    return ArithmeticExpression.Divide(element.Elements().Select(DeserializeExpressionFloat).ToArray());
                case ExpressionType.Equal:
                case ExpressionType.NotEqual:
                case ExpressionType.GreaterThan:
                case ExpressionType.LessThan:
                case ExpressionType.GreaterThanOrEqual:
                case ExpressionType.LessThanOrEqual:
                    return new ComparisonExpression(
                        type,
                        element.Elements().Select(DeserializeExpressionFloat).ToArray());
                case ExpressionType.And:
                case ExpressionType.Or:
                case ExpressionType.BoolEqual:
                case ExpressionType.Xor:
                case ExpressionType.Not:
                    return new LogicalExpression(type, element.Elements().Select(DeserializeExpressionBool).ToArray());
                default:
                    Debug.Assert(false);
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}