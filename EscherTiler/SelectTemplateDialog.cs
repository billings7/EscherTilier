using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Windows.Forms;
using EscherTiler.Graphics;
using EscherTiler.Graphics.GDI;
using EscherTiler.Templates;
using EscherTiler.Utilities;
using JetBrains.Annotations;
using GdiPGraphics = System.Drawing.Graphics;

namespace EscherTiler
{
    /// <summary>
    ///     Dialog box for selecting a built in <see cref="Template" />.
    /// </summary>
    /// <seealso cref="System.Windows.Forms.Form" />
    public partial class SelectTemplateDialog : Form
    {
        [NotNull]
        private readonly Dictionary<int, ListViewGroup> _groups = new Dictionary<int, ListViewGroup>();

        /// <summary>
        ///     Initializes a new instance of the <see cref="SelectTemplateDialog" /> class.
        /// </summary>
        // ReSharper disable once NotNullMemberIsNotInitialized
        public SelectTemplateDialog()
        {
            InitializeComponent();

            int index = 0;
            foreach (Tuple<Template, IImage> tuple in TemplateManager.Templates)
            {
                Image image;
                using (Stream stream = tuple.Item2.GetStream())
                {
                    image = Image.FromStream(stream);
                    if (image.Size != _thumbnailList.ImageSize)
                    {
                        Vector2 scale = _thumbnailList.ImageSize.ToVector2() / image.Size.ToVector2();
                        float minScale = Math.Min(scale.X, scale.Y);
                        Vector2 size = minScale * image.Size.ToVector2();
                        Vector2 pos = (_thumbnailList.ImageSize.ToVector2() - size) / 2;

                        Bitmap bit = new Bitmap(
                            _thumbnailList.ImageSize.Width,
                            _thumbnailList.ImageSize.Height,
                            PixelFormat.Format32bppArgb);

                        using (GdiPGraphics g = GdiPGraphics.FromImage(bit))
                        {
                            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                            g.SmoothingMode = SmoothingMode.AntiAlias;
                            g.PixelOffsetMode = PixelOffsetMode.HighQuality;

                            g.DrawImage(image, pos.X, pos.Y, size.X, size.Y);
                        }
                        image.Dispose();
                        image = bit;
                    }
                }

                _thumbnailList.Images.Add(image);

                // TODO Just temporary. need to add a name to Template
                ShapeTemplate shapeTemplate = tuple.Item1.ShapeTemplates.First().Value;
                string name = shapeTemplate.Name;
                int sideCount = shapeTemplate.VertexNames.Count;

                ListViewItem item = new ListViewItem(name, index++, GetGroup(sideCount))
                {
                    Tag = tuple.Item1
                };

                _templateList.Items.Add(item);
            }
        }

        /// <summary>
        ///     Gets the selected template.
        /// </summary>
        /// <value>
        ///     The template.
        /// </value>
        [CanBeNull]
        public Template Template => _templateList.SelectedItems.Count < 1
            ? null
            : (Template) _templateList.SelectedItems[0].Tag;

        /// <summary>
        ///     Gets the list view group for templates with the given number of sides.
        /// </summary>
        /// <param name="sideCount">The side count.</param>
        /// <returns></returns>
        [NotNull]
        private ListViewGroup GetGroup(int sideCount) => _groups.GetOrAdd(sideCount, CreateGroup);

        /// <summary>
        ///     Creates the list view group for templates with the given number of sides.
        /// </summary>
        /// <param name="sideCount">The side count.</param>
        /// <returns></returns>
        [NotNull]
        private ListViewGroup CreateGroup(int sideCount)
        {
            ListViewGroup listViewGroup;
            switch (sideCount)
            {
                case 3:
                    listViewGroup = new ListViewGroup("Triangles") { Tag = sideCount };
                    break;
                case 4:
                    listViewGroup = new ListViewGroup("Quadrilaterals") { Tag = sideCount };
                    break;
                case 5:
                    listViewGroup = new ListViewGroup("Pentagons") { Tag = sideCount };
                    break;
                case 6:
                    listViewGroup = new ListViewGroup("Hexagons") { Tag = sideCount };
                    break;
                default:
                    listViewGroup = new ListViewGroup($"{sideCount} sided") { Tag = sideCount };
                    break;
            }

            bool added = false;
            for (int i = 0; i < _templateList.Groups.Count; i++)
            {
                int tag = (int) _templateList.Groups[i].Tag;
                if (tag < sideCount)
                {
                    _templateList.Groups.Insert(i + 1, listViewGroup);
                    added = true;
                    break;
                }
            }

            if (!added)
                _templateList.Groups.Add(listViewGroup);

            return listViewGroup;
        }

        /// <summary>
        ///     Handles the SelectedIndexChanged event of the _templateList control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void _templateList_SelectedIndexChanged(object sender, EventArgs e)
        {
            _okBtn.Enabled = _templateList.SelectedItems.Count > 0;
        }
    }
}