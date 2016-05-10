using System;
using System.Drawing;
using System.Windows.Forms;
using EscherTiler.Graphics.GDI;
using EscherTiler.Styles;
using JetBrains.Annotations;

namespace EscherTiler
{
    /// <summary>
    ///     Control for editing a <see cref="IStyle" />.
    /// </summary>
    /// <seealso cref="System.Windows.Forms.UserControl" />
    public partial class StyleControl : UserControl
    {
        [NotNull]
        private readonly GDIResourceManager _resourceManager;

        /// <summary>
        ///     Gets the resource manager.
        /// </summary>
        /// <value>
        ///     The resource manager.
        /// </value>
        [NotNull]
        public GDIResourceManager ResourceManager => _resourceManager;

        [CanBeNull]
        private IStyle _style;

        /// <summary>
        ///     Gets or sets the style.
        /// </summary>
        /// <value>
        ///     The style.
        /// </value>
        [CanBeNull]
        public IStyle Style
        {
            get { return _style; }
            set
            {
                if (value == _style) return;

                if (_style != null)
                    ResourceManager.Release(_style);
                _style = value;
                if (value != null)
                {
                    _brush = ResourceManager.Add(value);

                    // TODO need to work out a transform to make the style fill the preview properly
                }

                _previewPnl?.Invalidate();

                StyleChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        [CanBeNull]
        private Brush _brush;

        /// <summary>
        ///     Occurs when the control style changes.
        /// </summary>
        public new event EventHandler StyleChanged;

        /// <summary>
        ///     Initializes a new instance of the <see cref="StyleControl" /> class.
        /// </summary>
        // ReSharper disable once NotNullMemberIsNotInitialized
        public StyleControl()
            : this(new GDIResourceManager()) { }

        /// <summary>
        ///     Initializes a new instance of the <see cref="StyleControl" /> class.
        /// </summary>
        /// <param name="resourceManager">The resource manager.</param>
        // ReSharper disable once NotNullMemberIsNotInitialized
        public StyleControl([NotNull] GDIResourceManager resourceManager)
            : this(null, resourceManager) { }

        /// <summary>
        ///     Initializes a new instance of the <see cref="StyleControl" /> class.
        /// </summary>
        // ReSharper disable once NotNullMemberIsNotInitialized
        public StyleControl([CanBeNull] IStyle style, [NotNull] GDIResourceManager resourceManager)
        {
            if (resourceManager == null) throw new ArgumentNullException(nameof(resourceManager));
            _resourceManager = resourceManager;
            Style = style;

            InitializeComponent();

            _previewPnl.SetDoubleBuffered();
        }

        /// <summary>
        ///     Handles the Paint event of the _previewPnl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="PaintEventArgs" /> instance containing the event data.</param>
        private void _previewPnl_Paint(object sender, PaintEventArgs e)
        {
            if (_brush != null)
                e.Graphics.FillRectangle(_brush, 0, 0, _previewPnl.Width, _previewPnl.Height);
        }

        /// <summary>
        ///     Handles the DoubleClick event of the _previewPnl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void _previewPnl_DoubleClick(object sender, EventArgs e)
        {
            SolidColourStyle style = Style as SolidColourStyle;
            if (style != null)
                _colourDialog.Color = Color.FromArgb(style.Colour.ToArgb());

            if (_colourDialog.ShowDialog(FindForm()) == DialogResult.OK)
            {
                Color col = _colourDialog.Color;
                Style = new SolidColourStyle(col.R, col.G, col.B, col.A);
            }
        }
    }
}