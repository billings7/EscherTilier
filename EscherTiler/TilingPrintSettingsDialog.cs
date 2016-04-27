using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Forms;
using EscherTiler.Graphics.GDI;
using JetBrains.Annotations;

namespace EscherTiler
{
    public partial class TilingPrintSettingsDialog : Form
    {
        [NotNull]
        private TilerPrintDocument _document;

        private TilingPrintMode _printMode;

        [NotNull]
        private Func<TileBase, Task<TileBase>> _selectTileFunction;

        private bool _selectingTile;
        private bool _selectedTile;

        /// <summary>
        ///     Initializes a new instance of the <see cref="TilingPrintSettingsDialog" /> class.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <param name="selectTileFunction">The select tile function.</param>
        /// <exception cref="System.ArgumentNullException">
        /// </exception>
        // ReSharper disable once NotNullMemberIsNotInitialized
        public TilingPrintSettingsDialog(
            [NotNull] TilerPrintDocument document,
            [NotNull] Func<TileBase, Task<TileBase>> selectTileFunction)
        {
            if (document == null) throw new ArgumentNullException(nameof(document));
            if (selectTileFunction == null) throw new ArgumentNullException(nameof(selectTileFunction));

            _document = document;
            SelectTileFunction = selectTileFunction;
            InitializeComponent();
        }

        /// <summary>
        ///     Raises the <see cref="E:System.Windows.Forms.Form.Shown" /> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.EventArgs" /> that contains the event data. </param>
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            if (_selectingTile)
            {
                _selectingTile = _selectedTile = false;
                return;
            }

            PrintMode = _document.PrintMode;
            Tile = _document.Tile;
        }

        [Category("Data")]
        [DefaultValue(null)]
        [Description("The TilingPrintMode that determines what gets printed.")]
        public TilingPrintMode PrintMode
        {
            get { return _printMode; }
            set
            {
                _printMode = value;

                switch (value)
                {
                    case TilingPrintMode.TilingFull:
                        tilingFullBtn.Checked = true;
                        break;
                    case TilingPrintMode.TilingLines:
                        tilingLinesBtn.Checked = true;
                        break;
                    case TilingPrintMode.SingleTileFull:
                        tileFullBtn.Checked = true;
                        break;
                    case TilingPrintMode.SingleTileLines:
                        tileLinesBtn.Checked = true;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        /// <summary>
        ///     Gets or sets the tile.
        /// </summary>
        /// <value>
        ///     The tile.
        /// </value>
        [Browsable(false)]
        [CanBeNull]
        public TileBase Tile { get; set; }

        /// <summary>
        ///     Gets or sets the document.
        /// </summary>
        /// <value>
        ///     The document.
        /// </value>
        [Category("Data")]
        [DefaultValue(null)]
        [Description("The TilerPrintDocument to get print settings from.")]
        [NotNull]
        public TilerPrintDocument Document
        {
            get { return _document; }
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));
                if (_document == value) return;
                _document = value;

                PrintMode = value.PrintMode;
                Tile = _document.Tile;
            }
        }

        /// <summary>
        ///     Gets or sets the select tile function.
        /// </summary>
        /// <value>
        ///     The select tile function.
        /// </value>
        /// <exception cref="System.ArgumentNullException"></exception>
        [NotNull]
        public Func<TileBase, Task<TileBase>> SelectTileFunction
        {
            get { return _selectTileFunction; }
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));
                _selectTileFunction = value;
            }
        }

        /// <summary>
        ///     Handles the Click event of the selectTileBtn control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private async void selectTileBtn_Click(object sender, EventArgs e)
        {
            _selectingTile = true;
            Hide();

            Tile = await SelectTileFunction(Tile);

            _selectedTile = true;
        }

        /// <summary>
        ///     Handles the CheckedChanged event of the print mode controls.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        /// <exception cref="System.NotSupportedException"></exception>
        private void modeBtn_CheckedChanged(object sender, EventArgs e)
        {
            if (tilingFullBtn.Checked)
            {
                _printMode = TilingPrintMode.TilingFull;
                selectTileBtn.Visible = false;
            }
            else if (tilingLinesBtn.Checked)
            {
                _printMode = TilingPrintMode.TilingLines;
                selectTileBtn.Visible = false;
            }
            else if (tileFullBtn.Checked)
            {
                _printMode = TilingPrintMode.SingleTileFull;
                selectTileBtn.Visible = true;
            }
            else if (tileLinesBtn.Checked)
            {
                _printMode = TilingPrintMode.SingleTileLines;
                selectTileBtn.Visible = true;
            }
            else throw new NotSupportedException();
        }

        /// <summary>
        ///     Shows the form as a modal dialog box with the specified owner.
        /// </summary>
        /// <param name="owner">
        ///     Any object that implements <see cref="T:System.Windows.Forms.IWin32Window" /> that represents the
        ///     top-level window that will own the modal dialog box.
        /// </param>
        /// <returns>
        ///     One of the <see cref="T:System.Windows.Forms.DialogResult" /> values.
        /// </returns>
        public new DialogResult ShowDialog(IWin32Window owner = null)
        {
            while (true)
            {
                DialogResult result = base.ShowDialog(owner);

                if (_selectingTile)
                {
                    while (!_selectedTile)
                        Application.DoEvents();
                }
                else
                {
                    if (result == DialogResult.OK)
                    {
                        _document.PrintMode = PrintMode;
                        _document.Tile = Tile;
                    }

                    return result;
                }
            }
        }
    }
}