using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Media;
using System.Numerics;
using System.Threading;
using System.Windows.Forms;
using EscherTilier.Controllers;
using EscherTilier.Expressions;
using EscherTilier.Properties;
using EscherTilier.Styles;
using EscherTilier.Utilities;
using JetBrains.Annotations;
using Action = EscherTilier.Controllers.Action;
using DragAction = EscherTilier.Controllers.DragAction;

namespace EscherTilier
{
    public partial class Main : Form, IView
    {
        private float _zoom = 100f;
        private Matrix3x2 _scale = Matrix3x2.Identity, _invScale = Matrix3x2.Identity;
        private Matrix3x2 _centerTranslate = Matrix3x2.Identity, _invCenterTranslate = Matrix3x2.Identity;
        private Matrix3x2 _translate = Matrix3x2.Identity, _invTranslate = Matrix3x2.Identity;

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

        public Matrix3x2 ViewMatrix =>
            _centerTranslate
            * _translate
            * _scale;

        public Matrix3x2 InverseViewMatrix =>
            _invScale
            * _invTranslate
            * _invCenterTranslate;

        public Numerics.Rectangle ViewBounds => _bounds;

        public event EventHandler ViewBoundsChanged;

        private Numerics.Rectangle _bounds;

        [CanBeNull]
        private Controller _controller;

        [NotNull]
        private PanTool _panTool;

        [NotNull]
        private readonly Dictionary<Tool, ToolStripButton> _toolBtns = new Dictionary<Tool, ToolStripButton>();

        #region Controls

        [NotNull]
        private MenuStrip _menuStrip;

        [NotNull]
        private ToolStripMenuItem _fileMenuItem;

        [NotNull]
        private ToolStripMenuItem _newMenuItem;

        [NotNull]
        private ToolStripMenuItem _openMenuItem;

        [NotNull]
        private ToolStripMenuItem _saveMenuItem;

        [NotNull]
        private ToolStripMenuItem _saveAsMenuItem;

        [NotNull]
        private ToolStripMenuItem _printMenuItem;

        [NotNull]
        private ToolStripMenuItem _printPreviewMenuItem;

        [NotNull]
        private ToolStripMenuItem _exitMenuItem;

        [NotNull]
        private ToolStripMenuItem _undoMenuItem;

        [NotNull]
        private ToolStripMenuItem _redoMenuItem;

        [NotNull]
        private ToolStripMenuItem _toolsMenuItem;

        [NotNull]
        private ToolStripMenuItem _customizeMenuItem;

        [NotNull]
        private ToolStripMenuItem _optionsMenuItem;

        [NotNull]
        private ToolStripMenuItem _helpMenuItem;

        [NotNull]
        private ToolStripMenuItem _indexMenuItem;

        [NotNull]
        private ToolStripMenuItem _searchMenuItem;

        [NotNull]
        private ToolStripMenuItem _aboutMenuItem;

        [NotNull]
        private ToolStripButton _newButton;

        [NotNull]
        private ToolStripButton _openButton;

        [NotNull]
        private ToolStripButton _saveButton;

        [NotNull]
        private ToolStripButton _printButton;

        [NotNull]
        private ToolStripButton _helpButton;

        [NotNull]
        private ToolStrip _toolStrip;

        [NotNull]
        private ToolStrip _operationToolStrip;

        [NotNull]
        private ToolStripButton _panToolBtn;

        [NotNull]
        private StatusStrip _statusStrip;

        [NotNull]
        private ToolStripStatusLabel _statusInfoLabel;

        [NotNull]
        private ToolStrip _contextToolStrip;

        [NotNull]
        private RenderControl _renderControl;

        [NotNull]
        private ToolStripTextBox _zoomText;

        #endregion

        // ReSharper disable once NotNullMemberIsNotInitialized - they are
        public Main()
        {
            InitializeComponent();
            InitializeGraphics();

            _centerTranslate = Matrix3x2.CreateTranslation(_renderControl.Width / 2f, _renderControl.Height / 2f);
            _invCenterTranslate = Matrix3x2.CreateTranslation(-_renderControl.Width / 2f, -_renderControl.Height / 2f);
            UpdateScale();

            _statusInfoLabel.Text = string.Empty;

            _renderControl.MouseWheel += renderControl_MouseWheel;

            //using (Factory factory = new Factory())
            //{
            //    _textFormat = new TextFormat(factory, "Calibri", 24.0f);
            //}
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
                        new EdgePattern("a", new[] { pa = new EdgePart(1, 1, true) }),
                        new EdgePattern("b", new[] { pb = new EdgePart(1, 1, true) }),
                        new EdgePattern("c", new[] { pc = new EdgePart(1, 1, false) }),
                        new EdgePattern("d", new[] { pd = new EdgePart(1, 1, false) })
                    },
                    new EdgePartAdjacencies
                    {
                        { pa.WithLabel("A"), pc.WithLabel("B") },
                        { pb.WithLabel("A"), pd.WithLabel("B") },
                        { pa.WithLabel("B"), pc.WithLabel("A") },
                        { pb.WithLabel("B"), pd.WithLabel("A") }
                    })
                });

            UpdateViewBounds();

            ShapeController sc = new ShapeController(template, this);

            RandomStyleManager randomStyleManager = new RandomStyleManager(null, 0)
            {
                LineStyle = new LineStyle(1, _blackStyle),
                Styles =
                {
                    new TileStyle(new SolidColourStyle(Colour.Red), sc.Shapes.ToArray()),
                    new TileStyle(new SolidColourStyle(Colour.White), sc.Shapes.ToArray()),
                    new TileStyle(new SolidColourStyle(Colour.Yellow), sc.Shapes.ToArray()),
                    new TileStyle(new SolidColourStyle(Colour.Orange), sc.Shapes.ToArray())
                }
            };

            _controller = new TilingController(
                template.CreateTiling(template.Tilings[0], sc.Shapes, randomStyleManager),
                randomStyleManager,
                this);
            _controller.CurrentToolChanged += _controller_CurrentToolChanged;
            _panTool = new PanTool(_controller, this);

            _toolBtns.Add(_panTool, _panToolBtn);

            UpdateTools();
            
            _renderControl.Start();
        }

        /// <summary>
        ///     Raises the <see cref="E:System.Windows.Forms.Form.Closing" /> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.ComponentModel.CancelEventArgs" /> that contains the event data. </param>
        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            // TODO prompt to save if not

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
        }

        private Size _lastNormalSize = Size.Empty;
        private Point _lastNormalLocation = new Point(0, 0);

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

        #region File menu

        private void newMenuItem_Click(object sender, EventArgs e) { }

        private void openMenuItem_Click(object sender, EventArgs e) { }

        private void saveMenuItem_Click(object sender, EventArgs e) { }

        private void saveAsMenuItem_Click(object sender, EventArgs e) { }

        private void printMenuItem_Click(object sender, EventArgs e) { }

        private void printPreviewMenuItem_Click(object sender, EventArgs e) { }

        private void exitMenuItem_Click(object sender, EventArgs e)
        {
            // TODO Prompt for save if unsaved

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

        private void newButton_Click(object sender, EventArgs e) { }

        private void openButton_Click(object sender, EventArgs e) { }

        private void saveButton_Click(object sender, EventArgs e) { }

        private void printButton_Click(object sender, EventArgs e) { }

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

        partial void renderControl_Render(SharpDX.Direct2D1.RenderTarget renderTarget, SharpDX.DXGI.SwapChain swapChain);

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

        private DragAction _dragAction;

        /// <summary>
        ///     Handles the MouseDown event of the renderControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseEventArgs" /> instance containing the event data.</param>
        private void renderControl_MouseDown(object sender, [NotNull] MouseEventArgs e)
        {
            if (_controller == null) return;

            Vector2 loc = new Vector2(e.X, e.Y);

            Action action = _controller.CurrentTool?.StartAction(loc);
            _dragAction = action as DragAction;
        }

        /// <summary>
        ///     Handles the MouseMove event of the renderControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseEventArgs" /> instance containing the event data.</param>
        private void renderControl_MouseMove(object sender, [NotNull] MouseEventArgs e)
        {
            _controller.CurrentTool?.UpdateLocation(new Vector2(e.X, e.Y));
            _dragAction?.Update(new Vector2(e.X, e.Y));
        }
        
        /// <summary>
        ///     Handles the MouseUp event of the renderControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseEventArgs" /> instance containing the event data.</param>
        private void renderControl_MouseUp(object sender, MouseEventArgs e)
            => Interlocked.Exchange(ref _dragAction, null)?.Apply();

        /// <summary>
        ///     Handles the MouseLeave event of the renderControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void renderControl_MouseLeave(object sender, EventArgs e)
            => Interlocked.Exchange(ref _dragAction, null)?.Apply();

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
        
        private void _controller_CurrentToolChanged(object sender, CurrentToolChangedEventArgs e)
        {
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
        ///     Handles the Click event of the tool button controls.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void toolBtn_Click(object sender, EventArgs e)
        {
            if (_controller == null) return;

            Debug.Assert(sender is ToolStripButton);
            ToolStripButton btn = (ToolStripButton) sender;

            Debug.Assert(btn.Tag is Tool);
            Tool tool = (Tool) btn.Tag;

            if (_controller.CurrentTool != null)
            {
                ToolStripButton lastBtn;
                if (_toolBtns.TryGetValue(_controller.CurrentTool, out lastBtn))
                {
                    Debug.Assert(lastBtn != null, "lastBtn != null");
                    lastBtn.Checked = false;
                }
            }

            _controller.CurrentTool = tool;
            btn.Checked = true;
        }

        /// <summary>
        ///     Updates the buttons for the current controllers tools.
        /// </summary>
        private void UpdateTools()
        {
            if (_controller == null) return;

            _panToolBtn.Tag = _panTool;

            foreach (Tool tool in _toolBtns.Keys.Where(t => t != _panTool).ToArray())
            {
                Debug.Assert(tool != null, "tool != null");

                ToolStripButton btn = _toolBtns[tool];
                if (!_controller.Tools.Contains(tool))
                    _toolBtns.Remove(tool);

                Debug.Assert(btn != null, "btn != null");
                _operationToolStrip.Items.Remove(btn);
            }

            foreach (Tool tool in _controller.Tools.Except(_toolBtns.Keys))
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
        ///     Tool used to pan around the tiling.
        /// </summary>
        /// <seealso cref="EscherTilier.Controllers.Tool" />
        private class PanTool : Tool
        {
            [NotNull]
            private readonly Main _form;

            private Cursor _lastCursor;

            /// <summary>
            ///     Initializes a new instance of the <see cref="PanTool" /> class.
            /// </summary>
            /// <param name="controller">The controller.</param>
            /// <param name="form">The form.</param>
            public PanTool([NotNull] Controller controller, [NotNull] Main form)
                : base(controller)
            {
                _form = form;
            }

            /// <summary>
            ///     Called when this tool is selected as the current tool.
            /// </summary>
            public override void Selected()
            {
                base.Selected();
                _lastCursor = _form._renderControl.Cursor;
                _form._renderControl.Cursor = Cursors.SizeAll;
            }

            /// <summary>
            ///     Called when this tool is deselected as the current tool.
            /// </summary>
            public override void Deselected()
            {
                base.Deselected();
                _form._renderControl.Cursor = _lastCursor;
            }

            /// <summary>
            ///     Starts the action associated with this tool at the location given.
            /// </summary>
            /// <param name="rawLocation">
            ///     The raw location to start the action.
            ///     Should be transformed by the <see cref="IView.InverseViewMatrix" /> for the
            ///     <see cref="Controller">Controllers</see> <see cref="EscherTilier.Controllers.Controller.View" /> to get the
            ///     location in the tiling itself.
            /// </param>
            /// <returns>
            ///     The action that was performed, or null if no action was performed.
            /// </returns>
            public override Action StartAction(Vector2 rawLocation)
            {
                return new PanAction(rawLocation, _form);
            }

            /// <summary>
            ///     Action used for panning.
            /// </summary>
            private class PanAction : DragAction
            {
                private Vector2 _last;

                [NotNull]
                private readonly Main _form;

                private readonly Matrix3x2 _translate;
                private readonly Matrix3x2 _invTranslate;

                /// <summary>
                ///     Initializes a new instance of the <see cref="PanAction" /> class.
                /// </summary>
                /// <param name="start">The start point of the action.</param>
                /// <param name="form">The form.</param>
                public PanAction(Vector2 start, [NotNull] Main form)
                {
                    _last = start;
                    _form = form;

                    _translate = form._translate;
                    _invTranslate = form._invTranslate;
                }

                /// <summary>
                ///     Updates the location of the action.
                /// </summary>
                /// <param name="rawLocation">
                ///     The raw location that the action has been dragged to.
                ///     Should be transformed by the <see cref="P:EscherTilier.IView.InverseViewMatrix" /> for the
                ///     <see cref="P:EscherTilier.Controllers.Controller.View" /> to get the location in 1the tiling itself.
                /// </param>
                public override void Update(Vector2 rawLocation)
                {
                    _form.UpdateTranslation(rawLocation - _last);
                    _last = rawLocation;
                }

                /// <summary>
                ///     Cancels this action.
                /// </summary>
                public override void Cancel()
                {
                    _form._translate = _translate;
                    _form._invTranslate = _invTranslate;
                    _form.UpdateViewBounds();
                }

                /// <summary>
                ///     Applies this action.
                /// </summary>
                public override void Apply() { }
            }
        }
    }
}