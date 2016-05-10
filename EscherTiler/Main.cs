using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Media;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using EscherTiler.Controllers;
using EscherTiler.Graphics;
using EscherTiler.Graphics.GDI;
using EscherTiler.Properties;
using EscherTiler.Storage;
using EscherTiler.Styles;
using JetBrains.Annotations;
using Action = EscherTiler.Controllers.Action;
using DragAction = EscherTiler.Controllers.DragAction;

namespace EscherTiler
{
    /// <summary>
    ///     The main form for the application.
    /// </summary>
    public partial class Main : Form, IView
    {
        private float _zoom = 100f;
        private Matrix3x2 _scale = Matrix3x2.Identity, _invScale = Matrix3x2.Identity;
        private Matrix3x2 _translate = Matrix3x2.Identity, _invTranslate = Matrix3x2.Identity;
        private Matrix3x2 _centerTranslate, _invCenterTranslate;

        [NotNull]
        private readonly float[] _zoomLevels =
        {
            3200,
            1600,
            1200,
            800,
            700,
            600,
            500,
            400,
            300,
            200,
            150,
            100,
            66.67f,
            50f,
            33.33f,
            25f,
            16.67f,
            12.5f,
            10f
        };

        /// <summary>
        ///     Gets the view matrix.
        /// </summary>
        /// <value>
        ///     The view matrix.
        /// </value>
        public Matrix3x2 ViewMatrix =>
            _centerTranslate
            * _translate
            * _scale;

        /// <summary>
        ///     Gets the inverse view matrix.
        /// </summary>
        /// <value>
        ///     The inverse view matrix.
        /// </value>
        public Matrix3x2 InverseViewMatrix =>
            _invScale
            * _invTranslate
            * _invCenterTranslate;

        /// <summary>
        ///     Gets the view bounds.
        /// </summary>
        /// <value>
        ///     The view bounds.
        /// </value>
        public Numerics.Rectangle ViewBounds => _bounds;

        /// <summary>
        ///     Occurs when the value of the <see cref="IView.ViewBounds" /> property changes.
        /// </summary>
        public event EventHandler ViewBoundsChanged;

        private Numerics.Rectangle _bounds;

        private Size _lastNormalSize = Size.Empty;

        private Point _lastNormalLocation = new Point(0, 0);

        /// <summary>
        ///     Gets or sets the currently active controller.
        /// </summary>
        /// <value>
        ///     The active controller.
        /// </value>
        [CanBeNull]
        private Controller ActiveController
        {
            get { return _activeController; }
            set
            {
                if (_activeController == value) return;

                lock (_lock)
                {
                    if (_activeController == value) return;

                    _activeController = value;
                    UpdateTools();
                    if (value != null && _panTool != null)
                        _panTool.Controller = value;
                }
            }
        }

        [CanBeNull]
        private Controller _activeController;

        [CanBeNull]
        private TilingController _tilingController;

        [CanBeNull]
        private PanTool _panTool;

        [NotNull]
        private SelectTileTool _selectTileTool;

        [CanBeNull]
        private DragAction _dragAction;

        [NotNull]
        private readonly SelectTemplateDialog _selectTemplateDialog;

        [NotNull]
        private readonly Dictionary<Tool, ToolStripButton> _toolBtns = new Dictionary<Tool, ToolStripButton>();

        [NotNull]
        private readonly TilingPrintSettingsDialog _tilingPrintSettingsDialog;

        [NotNull]
        private readonly object _lock = new object();

        /// <summary>
        ///     Gets the name of the current document.
        /// </summary>
        /// <value>
        ///     The name of the document.
        /// </value>
        [NotNull]
        public string DocumentName => _documentName;

        /// <summary>
        ///     Gets or sets the document path.
        /// </summary>
        /// <value>
        ///     The document path.
        /// </value>
        [CanBeNull]
        public string DocumentPath
        {
            get { return _documentPath; }
            set
            {
                _documentPath = value;
                _documentName = _documentPath == null
                    ? "Untitled"
                    : Path.GetFileNameWithoutExtension(_documentPath);
                Text = _documentName + (IsDirty ? "*" : string.Empty) + @" - " + Resources.Main_Title;
            }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether there are any unsaved changes.
        /// </summary>
        /// <value>
        ///     <see langword="true" /> if there are any unsaved changes; otherwise, <see langword="false" />.
        /// </value>
        private bool IsDirty
        {
            get { return _isDirty; }
            set
            {
                if (value == _isDirty) return;
                _isDirty = value;
                Text = _documentName + (IsDirty ? "*" : string.Empty) + @" - " + Resources.Main_Title;
            }
        }

        [NotNull]
        private string _documentName = "Untitled";

        [CanBeNull]
        private string _documentPath;

        private bool _isDirty;

        private bool _layoutSuspended;

        /// <summary>
        ///     Initializes a new instance of the <see cref="Main" /> class.
        /// </summary>
        // ReSharper disable once NotNullMemberIsNotInitialized - they are
        public Main()
        {
            InitializeComponent();
            InitializeGraphics();
            InitializeStyleManager();

            _changeLineTypeCmb.Name = TilingController.EditLineTool.ChangeLineTypeName;
            _changeLineTypeCmb.Items.Add(new ComboBoxValue<Type>("Line", typeof(Line)));
            _changeLineTypeCmb.Items.Add(new ComboBoxValue<Type>("Quadratic Curve", typeof(QuadraticBezierCurve)));
            _changeLineTypeCmb.Items.Add(new ComboBoxValue<Type>("Cubic Curve", typeof(CubicBezierCurve)));

            _centerTranslate = Matrix3x2.CreateTranslation(_renderControl.Width / 2f, _renderControl.Height / 2f);
            _invCenterTranslate = Matrix3x2.CreateTranslation(-_renderControl.Width / 2f, -_renderControl.Height / 2f);
            UpdateScale();

            _statusInfoLabel.Text = string.Empty;

            _renderControl.MouseWheel += renderControl_MouseWheel;

            _selectTemplateDialog = new SelectTemplateDialog();

            _printDocument.SetGetTranformDelegate(GetPrintTransform);

            _printPreviewDialog.StartPosition = FormStartPosition.CenterParent;
            _printPreviewDialog.Size = new Size(1000, 800);

            _tilingPrintSettingsDialog = new TilingPrintSettingsDialog(_printDocument, SelectTileAsync);

            Text = _documentName + @" - " + Resources.Main_Title;
        }

        /// <summary>
        ///     Raises the <see cref="E:System.Windows.Forms.Form.Load" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            Size = Settings.Default.WindowSize;

            if (Settings.Default.WindowLocation.X == int.MinValue &&
                Settings.Default.WindowLocation.Y == int.MinValue)
            {
                Rectangle rectangle = Screen.PrimaryScreen.Bounds;
                Settings.Default.WindowLocation = new Point(
                    rectangle.X + (rectangle.Width - Width) / 2,
                    rectangle.Y + (rectangle.Height - Height) / 2);
            }

            Location = Settings.Default.WindowLocation;
            switch (Settings.Default.WindowState)
            {
                case FormWindowState.Maximized:
                    WindowState = FormWindowState.Maximized;
                    break;
                default:
                    WindowState = FormWindowState.Normal;
                    break;
            }

            /*
            EdgePart pa, pb, pc, pd;

            Template template = new Template(
                new[]
                {
                    new ShapeTemplate(
                    "Square",
                    new[] { "a", "b", "c", "d" },
                    new[] { "A", "B", "C", "D" },
                    new[]
                    {
                        new Vector2(-40, -40),
                        new Vector2(40, -40),
                        new Vector2(40, 40),
                        new Vector2(-40, 40)
                    })
                },
                new IExpression<bool>[0],
                new[]
                {
                    new TilingDefinition(
                    1,
                    null,
                    new[]
                    {
                        new EdgePattern("a", new[] { pa = new EdgePart(1, 1, 1, true) }),
                        new EdgePattern("b", new[] { pb = new EdgePart(2, 2, 1, true) }),
                        new EdgePattern("c", new[] { pc = new EdgePart(3, 1, 1, false) }),
                        new EdgePattern("d", new[] { pd = new EdgePart(4, 2, 1, false) })
                    },
                    new EdgePartAdjacencies
                    {
                        { pa.WithLabel("A"), pc.WithLabel("B") },
                        { pb.WithLabel("A"), pd.WithLabel("B") },
                        { pa.WithLabel("B"), pc.WithLabel("A") },
                        { pb.WithLabel("B"), pd.WithLabel("A") }
                    })
                });


            ShapeController sc = new ShapeController(template, this);

            StyleManager styleManager = CreateDefaultStyleManager(sc.Shapes.ToArray());

            //*/

            UpdateViewBounds();

            _renderControl.Start();
        }

        /// <summary>
        ///     Creates the tiling controller.
        /// </summary>
        /// <param name="tiling">The tiling.</param>
        /// <returns></returns>
        private TilingController CreateTilingController([NotNull] Tiling tiling)
        {
            Debug.Assert(tiling != null, "tiling != null");

            TilingController tc = new TilingController(tiling, this);

            tc.EditLine.OptionsChanged += TilingController_EditLineTool_OptionsChanged;
            tc.EditLine.ChangeLineOption.ValueChanged += v => _changeLineTypeCmb.SelectedItem = v;
            _changeLineTypeCmb.SelectedItem = tc.EditLine.ChangeLineOption.Value;

            tc.StyleManagerChanged += (sender, args) => IsDirty = true;
            tc.CurrentToolChanged += controller_CurrentToolChanged;
            _panTool = new PanTool(tc, this);
            _selectTileTool = new SelectTileTool(tc);

            _toolBtns.Add(_panTool, _panToolBtn);

            UpdateTools();

            return tc;
        }

        /// <summary>
        ///     Handles the OptionsChanged event of the TilingControllers EditLineTool.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void TilingController_EditLineTool_OptionsChanged(object sender, EventArgs e)
        {
            TilingController.EditLineTool tool = (TilingController.EditLineTool) sender;

            _changeLineTypeCmb.Visible = tool.Options.Contains(tool.ChangeLineOption);
        }

        /// <summary>
        ///     Raises the <see cref="E:System.Windows.Forms.Form.Closing" /> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.ComponentModel.CancelEventArgs" /> that contains the event data. </param>
        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            if (!PromptSave())
            {
                e.Cancel = true;
                return;
            }

            Settings.Default.WindowState = WindowState;
            Settings.Default.WindowSize = _lastNormalSize;
            Settings.Default.WindowLocation = _lastNormalLocation;

            Settings.Default.Save();

            _renderControl.Stop();
        }

        /// <summary>
        ///     Raises the <see cref="E:System.Windows.Forms.Form.Closed" /> event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            UnloadGraphics();
            _activeController = null;
            Interlocked.Exchange(ref _tilingController, null)?.Dispose();
            // TODO Dispose shape controller
        }

        /// <summary>
        ///     Raises the <see cref="E:System.Windows.Forms.Control.Layout" /> event.
        /// </summary>
        /// <param name="levent">The event data.</param>
        protected override void OnLayout(LayoutEventArgs levent)
        {
            base.OnLayout(levent);

            if (WindowState != FormWindowState.Maximized)
                _lastNormalSize = Size;

            UpdateScale();
        }

        /// <summary>
        ///     Raises the <see cref="E:System.Windows.Forms.Control.LocationChanged" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
        protected override void OnLocationChanged(EventArgs e)
        {
            base.OnLocationChanged(e);

            if (WindowState != FormWindowState.Maximized)
                _lastNormalLocation = Location;
        }

        /// <summary>
        ///     Checks if the tiling has changes and prompts the user if they want to save if there are.
        /// </summary>
        /// <returns>
        ///     <see langword="true" /> if there arent any changes or the tiling was saved; otherwise <see langword="false" />.
        /// </returns>
        private bool PromptSave()
        {
            if (!IsDirty) return true;

            lock (_lock)
            {
                if (!IsDirty) return true;

                DialogResult dialogResult = MessageBox.Show(
                    this,
                    string.Format(Resources.Main_PromptSave_Text, DocumentName),
                    Resources.Main_Title,
                    MessageBoxButtons.YesNoCancel);

                switch (dialogResult)
                {
                    case DialogResult.Yes:
                        return Save();
                    case DialogResult.No:
                        return true;
                    case DialogResult.Cancel:
                        return false;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        /// <summary>
        ///     Saves the current tiling.
        /// </summary>
        /// <param name="askForPath">
        ///     If set to <see langword="true" /> the user will be prompted for the location to save the tiling.
        /// </param>
        /// <returns><see langword="true" /> if the tiling was saved; otherwise <see langword="false" />.</returns>
        private bool Save(bool askForPath = false)
        {
            if (!IsDirty) return true;

            lock (_lock)
            {
                if (!IsDirty) return true;

                TilingController controller = _tilingController;
                if (controller == null) return false;

                if (DocumentPath == null || askForPath)
                {
                    _saveFileDialog.FileName = DocumentPath ?? DocumentName;
                    if (_saveFileDialog.ShowDialog(this) == DialogResult.OK)
                        DocumentPath = _saveFileDialog.FileName;
                    else
                        return false;
                }

                // TODO Create thumbnail

                Debug.Assert(DocumentPath != null, "DocumentPath != null");
                FileStorage.SaveTiling(controller.Tiling, DocumentPath);
                IsDirty = false;
                return true;
            }
        }

        /// <summary>
        ///     Opens the tiling given for editing.
        /// </summary>
        /// <param name="tiling">The tiling.</param>
        private void OpenTiling([NotNull] Tiling tiling)
        {
            Debug.Assert(tiling != null, "tiling != null");

            lock (_lock)
            {
                try
                {
                    // Dont call the ViewBoundsChanged event handler while changing the tiling
                    _layoutSuspended = true;

                    // Reset view
                    _translate = _invTranslate = Matrix3x2.Identity;
                    _zoom = 100f;
                    UpdateScale();

                    if (_tilingController == null)
                        _tilingController = CreateTilingController(tiling);
                    else
                        _tilingController.SetTiling(tiling);

                    ActiveController = _tilingController;

                    UpdateStyleManager(tiling.StyleManager);

                    IsDirty = false;
                }
                finally
                {
                    _layoutSuspended = false;
                }
            }
        }

        /// <summary>
        ///     Creates the default style manager.
        /// </summary>
        /// <param name="shapes">The shapes.</param>
        /// <returns></returns>
        [NotNull]
        private StyleManager CreateDefaultStyleManager([NotNull] IReadOnlyCollection<Shape> shapes)
        {
            Debug.Assert(shapes != null, "shapes != null");

            return new GreedyStyleManager(
                1,
                1,
                1,
                new LineStyle(0.5f, _blackStyle),
                new TileStyle(new SolidColourStyle(Colour.ForestGreen), shapes),
                new TileStyle(new SolidColourStyle(Colour.CornflowerBlue), shapes));
        }

        #region File menu

        /// <summary>
        ///     Handles the Click event of the newMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void newMenuItem_Click(object sender, EventArgs e)
        {
            if (!PromptSave()) return;

            if (_selectTemplateDialog.ShowDialog(this) == DialogResult.OK)
            {
                Template template = _selectTemplateDialog.Template;
                if (template == null) return;

                ShapeSet shapes = template.CreateShapes();

                Tiling tiling = template.CreateTiling(
                    template.Tilings.Values.First(),
                    shapes,
                    CreateDefaultStyleManager(shapes));

                OpenTiling(tiling);

                IsDirty = true;
                DocumentPath = null;
            }
        }

        /// <summary>
        ///     Handles the Click event of the openMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void openMenuItem_Click(object sender, EventArgs e)
        {
            if (!PromptSave()) return;

            if (_openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string documentPath = _openFileDialog.FileName;
                Debug.Assert(documentPath != null, "documentPath != null");

                try
                {
                    IImage thumbnail;
                    Tiling tiling = FileStorage.LoadTiling(documentPath, out thumbnail);

                    OpenTiling(tiling);
                    DocumentPath = documentPath;
                }
                catch (InvalidDataException ide)
                {
                    MessageBox.Show(
                        this,
                        "Error loading tiling",
                        $"A problem was found with the file '{Path.GetFileName(documentPath)}', could not load the tiling.\r\n\r\n{ide.Message}",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        this,
                        "Error loading tiling",
                        $"A error occured while loading the file '{Path.GetFileName(documentPath)}'.\r\n\r\n{ex.Message}",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        ///     Handles the Click event of the saveMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void saveMenuItem_Click(object sender, EventArgs e)
        {
            Save();
        }

        /// <summary>
        ///     Handles the Click event of the saveAsMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void saveAsMenuItem_Click(object sender, EventArgs e)
        {
            Save(true);
        }

        /// <summary>
        ///     Handles the Click event of the _pageSetupMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void _pageSetupMenuItem_Click(object sender, EventArgs e)
        {
            _pageSetupDialog.ShowDialog(this);
        }

        /// <summary>
        ///     Handles the Click event of the printMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void printMenuItem_Click(object sender, EventArgs e)
        {
            TilingController controller = _tilingController;
            if (controller == null || controller != ActiveController) return;

            _printDocument.DocumentName = DocumentName;
            _printDocument.Tiling = controller?.Tiling;
            if (_tilingPrintSettingsDialog.ShowDialog(this) != DialogResult.OK)
                return;

            if (_printDialog.ShowDialog(this) == DialogResult.OK)
                _printDocument.Print();
        }

        /// <summary>
        ///     Handles the Click event of the printPreviewMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void printPreviewMenuItem_Click(object sender, EventArgs e)
        {
            TilingController controller = _tilingController;
            if (controller == null || controller != ActiveController) return;

            _printDocument.DocumentName = DocumentName;
            if (_printDocument.Tile == null && _printDocument.Tiling == null)
                _printDocument.Tiling = controller?.Tiling;

            if (_tilingPrintSettingsDialog.ShowDialog(this) != DialogResult.OK)
                return;

            _printPreviewDialog.ShowDialog(this);
        }

        /// <summary>
        ///     Handles the Click event of the exitMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        /// <exception cref="System.NotImplementedException">save</exception>
        private void exitMenuItem_Click(object sender, EventArgs e)
        {
            if (!PromptSave()) return;

            Close();
        }

        #endregion

        #region Edit menu

        private void undoMenuItem_Click(object sender, EventArgs e) { }

        private void redoMenuItem_Click(object sender, EventArgs e) { }

        #endregion

        #region Help menu

        private void indexMenuItem_Click(object sender, EventArgs e) { }

        private void searchMenuItem_Click(object sender, EventArgs e) { }

        private void aboutMenuItem_Click(object sender, EventArgs e) { }

        #endregion

        #region Tool Strip

        /// <summary>
        ///     Handles the Click event of the newButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void newButton_Click(object sender, EventArgs e) => newMenuItem_Click(sender, e);

        /// <summary>
        ///     Handles the Click event of the openButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void openButton_Click(object sender, EventArgs e) => openMenuItem_Click(sender, e);

        /// <summary>
        ///     Handles the Click event of the saveButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void saveButton_Click(object sender, EventArgs e) => saveMenuItem_Click(sender, e);

        /// <summary>
        ///     Handles the Click event of the printButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void printButton_Click(object sender, EventArgs e)
        {
            TilingController controller = _tilingController;
            if (controller == null || controller != ActiveController) return;

            _printDocument.DocumentName = DocumentName;
            if (_printDocument.Tile == null && _printDocument.Tiling == null)
            {
                _printDocument.Tiling = controller?.Tiling;

                if (_tilingPrintSettingsDialog.ShowDialog(this) != DialogResult.OK)
                    return;
            }

            _printDocument.Print();
        }

        private void helpButton_Click(object sender, EventArgs e) { }

        #endregion

        /// <summary>
        ///     Updates the scale matricies.
        /// </summary>
        private void UpdateScale()
        {
            int minDim = Math.Min(_renderControl.Width, _renderControl.Height);

            Vector2 center = new Vector2(_renderControl.Width / 2f, _renderControl.Height / 2f);

            _scale = Matrix3x2.CreateScale((minDim * _zoom) / 10000f, center);
            _invScale = Matrix3x2.CreateScale(10000f / (minDim * _zoom), center);

            UpdateViewBounds();
        }

        /// <summary>
        ///     Updates the translation matricies.
        /// </summary>
        /// <param name="dp">The dp.</param>
        private void UpdateTranslation(Vector2 dp)
        {
            int minDim = Math.Min(_renderControl.Width, _renderControl.Height);

            float scale = (minDim * _zoom) / 10000f;

            _translate *= Matrix3x2.CreateTranslation(dp / scale);
            _invTranslate *= Matrix3x2.CreateTranslation(-dp / scale);

            UpdateViewBounds();
        }

        /// <summary>
        ///     Updates the view bounds.
        /// </summary>
        private void UpdateViewBounds()
        {
            Vector2 tl = Vector2.Zero;
            Vector2 br = new Vector2(_renderControl.Width, _renderControl.Height);

            tl = Vector2.Transform(tl, InverseViewMatrix);
            br = Vector2.Transform(br, InverseViewMatrix);

            _bounds = Numerics.Rectangle.ContainingPoints(tl, br);
            if (!_layoutSuspended)
                ViewBoundsChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        ///     Handles the MouseWheel event of the renderControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseEventArgs" /> instance containing the event data.</param>
        private void renderControl_MouseWheel(object sender, [NotNull] MouseEventArgs e)
        {
            int zoomDir = Math.Sign(e.Delta);

            if (zoomDir == -1)
            {
                if (_zoom > _zoomLevels[_zoomLevels.Length - 1])
                    _zoom = _zoomLevels.First(f => f < _zoom);
            }
            else if (zoomDir == 1)
            {
                if (_zoom < _zoomLevels[0])
                    _zoom = _zoomLevels.Last(f => f > _zoom);
            }
            _zoomText.Text = (_zoom / 100f).ToString("0.##%");

            UpdateScale();
        }

        /// <summary>
        ///     Handles the Layout event of the renderControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="LayoutEventArgs" /> instance containing the event data.</param>
        private void renderControl_Layout(object sender, LayoutEventArgs e)
        {
            _centerTranslate = Matrix3x2.CreateTranslation(_renderControl.Width / 2f, _renderControl.Height / 2f);
            _invCenterTranslate = Matrix3x2.CreateTranslation(-_renderControl.Width / 2f, -_renderControl.Height / 2f);

            UpdateViewBounds();
        }

        /// <summary>
        ///     Handles the Leave event of the zoomText control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void zoomText_Leave(object sender, EventArgs e)
        {
            UpdateZoom();
        }

        /// <summary>
        ///     Processes a command key.
        /// </summary>
        /// <returns>
        ///     true if the keystroke was processed and consumed by the control; otherwise, false to allow further processing.
        /// </returns>
        /// <param name="message">
        ///     A <see cref="T:System.Windows.Forms.Message" />, passed by reference, that represents the Win32
        ///     message to process.
        /// </param>
        /// <param name="keyData">One of the <see cref="T:System.Windows.Forms.Keys" /> values that represents the key to process. </param>
        protected override bool ProcessCmdKey(ref Message message, Keys keyData)
        {
            if (_zoomText.Focused)
            {
                switch (keyData)
                {
                    case Keys.Enter:
                        UpdateZoom();
                        return true;
                    case Keys.Escape:
                        _zoomText.Text = (_zoom / 100f).ToString("0.##%");
                        Unfocus();
                        return true;
                }
            }
            return false;
        }

        /// <summary>
        ///     Handles the MouseDown event of the renderControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseEventArgs" /> instance containing the event data.</param>
        private void renderControl_MouseDown(object sender, [NotNull] MouseEventArgs e)
        {
            Controller controller = ActiveController;
            if (controller == null) return;

            if (e.Button != MouseButtons.Left) return;

            Vector2 loc = new Vector2(e.X, e.Y);

            lock (_lock)
            {
                controller = ActiveController;
                if (controller == null) return;

                Action action = controller.CurrentTool?.StartAction(loc);
                _dragAction = action as DragAction;
                if (action != null && action.ChangesData)
                    IsDirty = true;
            }
        }

        /// <summary>
        ///     Handles the MouseMove event of the renderControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseEventArgs" /> instance containing the event data.</param>
        private void renderControl_MouseMove(object sender, [NotNull] MouseEventArgs e)
        {
            Controller controller = ActiveController;
            if (controller == null) return;

            lock (_lock)
            {
                controller = ActiveController;
                if (controller == null) return;

                controller.CurrentTool?.UpdateLocation(new Vector2(e.X, e.Y));
                DragAction dragAction = _dragAction;
                if (dragAction != null)
                {
                    dragAction.Update(new Vector2(e.X, e.Y));
                    if (dragAction.ChangesData)
                        IsDirty = true;
                }
            }
        }

        /// <summary>
        ///     Handles the MouseUp event of the renderControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseEventArgs" /> instance containing the event data.</param>
        private void renderControl_MouseUp(object sender, MouseEventArgs e)
        {
            lock (_lock)
            {
                DragAction dragAction = Interlocked.Exchange(ref _dragAction, null);
                if (dragAction != null)
                {
                    dragAction.Apply();
                    if (dragAction.ChangesData)
                        IsDirty = true;
                }
            }
        }

        /// <summary>
        ///     Handles the MouseLeave event of the renderControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void renderControl_MouseLeave(object sender, EventArgs e)
        {
            lock (_lock)
            {
                DragAction dragAction = Interlocked.Exchange(ref _dragAction, null);
                if (dragAction != null)
                {
                    dragAction.Apply();
                    if (dragAction.ChangesData)
                        IsDirty = true;
                }
            }
        }

        /// <summary>
        ///     Updates the zoom level.
        /// </summary>
        private void UpdateZoom()
        {
            string str = _zoomText.Text.Trim();
            if (str.EndsWith(CultureInfo.CurrentCulture.NumberFormat.PercentSymbol))
                str = str.Substring(0, str.Length - CultureInfo.CurrentCulture.NumberFormat.PercentSymbol.Length);
            float zoom;
            if (!float.TryParse(str, out zoom))
            {
                SystemSounds.Asterisk.Play();
                _zoomText.Focus();
                return;
            }

            zoom = (float) Math.Round(zoom, 2);
            if (zoom < _zoomLevels[_zoomLevels.Length - 1])
                zoom = _zoomLevels[_zoomLevels.Length - 1];
            else if (zoom > _zoomLevels[0])
                zoom = _zoomLevels[0];

            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (_zoom != zoom)
            {
                _zoom = zoom;

                UpdateScale();
            }

            _zoomText.Text = (_zoom / 100f).ToString("0.##%");

            Unfocus();
        }

        /// <summary>
        ///     Unfocuses the currently focused control.
        /// </summary>
        private void Unfocus()
        {
            IContainerControl container = this;
            while (container != null)
            {
                Control control = container.ActiveControl;
                container.ActiveControl = null;
                container = control as IContainerControl;
            }
        }

        /// <summary>
        ///     Handles the CurrentToolChanged event of the _controller object.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="CurrentToolChangedEventArgs" /> instance containing the event data.</param>
        private void controller_CurrentToolChanged(object sender, CurrentToolChangedEventArgs e)
        {
            if (sender != ActiveController) return;

            if (e.OldTool != null)
            {
                ToolStripButton lastBtn;
                if (_toolBtns.TryGetValue(e.OldTool, out lastBtn))
                {
                    Debug.Assert(lastBtn != null, "lastBtn != null");
                    lastBtn.Checked = false;
                }
            }

            if (e.NewTool != null)
            {
                ToolStripButton newBtn;
                if (_toolBtns.TryGetValue(e.NewTool, out newBtn))
                {
                    Debug.Assert(newBtn != null, "newBtn != null");
                    newBtn.Checked = true;
                }
            }
        }

        /// <summary>
        ///     Handles the SelectedIndexChanged event of the _changeLineTypeCmb control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void changeLineTypeCmb_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBoxValue<Type> val = (ComboBoxValue<Type>) _changeLineTypeCmb.SelectedItem;
            if (_tilingController != null)
            {
                _tilingController.EditLine.ChangeLineOption.Value = val.Value;
                IsDirty = true;
            }
        }

        /// <summary>
        ///     Handles the Click event of the tool button controls.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void toolBtn_Click(object sender, EventArgs e)
        {
            Controller controller = ActiveController;
            if (controller == null) return;

            Debug.Assert(sender is ToolStripButton);
            ToolStripButton btn = (ToolStripButton) sender;

            Debug.Assert(btn.Tag is Tool);
            Tool tool = (Tool) btn.Tag;

            if (controller.CurrentTool != null)
            {
                ToolStripButton lastBtn;
                if (_toolBtns.TryGetValue(controller.CurrentTool, out lastBtn))
                {
                    Debug.Assert(lastBtn != null, "lastBtn != null");
                    lastBtn.Checked = false;
                }
            }

            lock (_lock)
                controller.CurrentTool = tool;
            btn.Checked = true;
        }

        /// <summary>
        ///     Updates the buttons for the current controllers tools.
        /// </summary>
        private void UpdateTools()
        {
            Controller controller = ActiveController;

            _panToolBtn.Tag = _panTool;

            foreach (Tool tool in _toolBtns.Keys.Where(t => t != _panTool).ToArray())
            {
                Debug.Assert(tool != null, "tool != null");

                ToolStripButton btn = _toolBtns[tool];
                if (controller == null || !controller.Tools.Contains(tool))
                    _toolBtns.Remove(tool);

                Debug.Assert(btn != null, "btn != null");
                _operationToolStrip.Items.Remove(btn);
            }

            if (controller == null) return;
            foreach (Tool tool in controller.Tools)
            {
                Debug.Assert(tool != null, "tool != null");

                ToolStripButton btn;
                if (!_toolBtns.TryGetValue(tool, out btn))
                {
                    string toolName = Resources.ResourceManager.GetString(tool.Name + ":Name");
                    Image toolImage = (Image) Resources.ResourceManager.GetObject(tool.Name + ":Icon");

                    btn = new ToolStripButton(toolName, toolImage, toolBtn_Click, tool.Name)
                    {
                        AutoSize = false,
                        Tag = tool,
                        DisplayStyle = ToolStripItemDisplayStyle.Image,
                        Size = new Size(20, 20)
                    };
                    _toolBtns.Add(tool, btn);
                }

                Debug.Assert(btn != null, "btn != null");
                _operationToolStrip.Items.Add(btn);
            }

            _panToolBtn.PerformClick();
        }

        /// <summary>
        ///     Selects a tile from the tiling.
        /// </summary>
        /// <param name="initialTile">The initial tile.</param>
        /// <returns></returns>
        /// <exception cref="System.ObjectDisposedException"></exception>
        private Task<TileBase> SelectTileAsync(TileBase initialTile)
        {
            TilingController controller = _tilingController;
            if (controller == null) return Task.FromResult<TileBase>(null);

            _selectTileTool.LastTool = controller.CurrentTool;
            _selectTileTool.TileSelectedTcs = new TaskCompletionSource<TileBase>();
            controller.CurrentTool = _selectTileTool;
            return _selectTileTool.TileSelectedTcs.Task;
        }

        /// <summary>
        ///     Gets the transform matricies used for printing.
        /// </summary>
        /// <param name="bounds">The bounds.</param>
        /// <param name="transform">The transform.</param>
        /// <param name="inverseTransform">The inverse transform.</param>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        private void GetPrintTransform(
            Numerics.Rectangle bounds,
            out Matrix3x2 transform,
            out Matrix3x2 inverseTransform)
        {
            float minDim = Math.Min(bounds.Width, bounds.Height);

            Vector2 center = bounds.Center;

            Matrix3x2 scale = Matrix3x2.CreateScale((minDim * _zoom) / 10000f, center);
            Matrix3x2 invScale = Matrix3x2.CreateScale(10000f / (minDim * _zoom), center);

            Matrix3x2 centerTranslate = Matrix3x2.CreateTranslation(center);
            Matrix3x2 invCenterTranslate = Matrix3x2.CreateTranslation(-center);

            Matrix3x2 translate, invTranslate;

            switch (_printDocument.PrintMode)
            {
                case TilingPrintMode.TilingFull:
                case TilingPrintMode.TilingLines:
                    translate = Matrix3x2.CreateTranslation(_bounds.Center);
                    invTranslate = Matrix3x2.CreateTranslation(-_bounds.Center);
                    break;

                case TilingPrintMode.SingleTileFull:
                case TilingPrintMode.SingleTileLines:
                    Debug.Assert(_printDocument.Tile != null, "_printDocument.Tile != null");

                    Vector2 centroid = _printDocument.Tile.Centroid;

                    translate = Matrix3x2.CreateTranslation(centroid);
                    invTranslate = Matrix3x2.CreateTranslation(-centroid);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            transform = centerTranslate * translate * scale;
            inverseTransform = invScale * invTranslate * invCenterTranslate;
        }
    }
}