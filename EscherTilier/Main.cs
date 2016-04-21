using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Media;
using System.Numerics;
using System.Windows.Forms;
using EscherTilier.Controllers;
using EscherTilier.Expressions;
using EscherTilier.Properties;
using EscherTilier.Styles;
using EscherTilier.Utilities;
using JetBrains.Annotations;

namespace EscherTilier
{
    public partial class Main : Form
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
            //8.33f,
            //6.25f,
            //5f,
            //4f,
            //3f,
            //2f,
            //1.5f,
            //1f,
            //0.7f,
            //0.5f,
            //0.4f,
            //0.3f,
            //0.2f,
            //0.17f,
        };

        private Matrix3x2 ViewMatrix =>
            _scale
            * _centerTranslate
            * _translate;

        private Matrix3x2 InverseViewMatrix =>
            _invTranslate
            * _invCenterTranslate
            * _invScale;

        private Numerics.Rectangle _bounds;

        private Controller _controller;

        public Main()
        {
            InitializeComponent();
            InitializeGraphics();

            statusInfoLabel.Text = string.Empty;

            renderControl.MouseWheel += renderControl_MouseWheel;

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
                            new Vector2(-40, 40),
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
                            new EdgePattern("b", new[] { pb = new EdgePart(2, 1, true) }),
                            new EdgePattern("c", new[] { pc = new EdgePart(1, 1, false) }),
                            new EdgePattern("d", new[] { pd = new EdgePart(2, 1, false) })
                        },
                        new EdgePartAdjacencies
                        {
                            { pa.WithLabel("A"), pc.WithLabel("B") },
                            { pb.WithLabel("A"), pd.WithLabel("B") },
                            { pa.WithLabel("B"), pc.WithLabel("A") },
                            { pb.WithLabel("B"), pd.WithLabel("A") },
                        })
                });

            UpdateViewBounds();

            ShapeController sc = new ShapeController(template, _bounds);

            RandomStyleManager randomStyleManager = new RandomStyleManager(null, 0)
            {
                LineStyle = new LineStyle(1, _blackStyle),
                Styles =
                {
                    new TileStyle(new SolidColourStyle(Colour.Red), sc.Shapes.ToArray()),
                    new TileStyle(new SolidColourStyle(Colour.White), sc.Shapes.ToArray()),
                    new TileStyle(new SolidColourStyle(Colour.Yellow), sc.Shapes.ToArray()),
                    new TileStyle(new SolidColourStyle(Colour.Orange), sc.Shapes.ToArray()),
                },
            };

            _controller = new TilingController(
                template.CreateTiling(template.Tilings[0], sc.Shapes, randomStyleManager),
                randomStyleManager,
                _bounds);

            renderControl.Start();
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

            renderControl.Stop();
        }

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

        private void UpdateScale()
        {
            int minDim = Math.Min(renderControl.Width, renderControl.Height);

            _scale = Matrix3x2.CreateScale((minDim * _zoom) / 10000f);
            _invScale = Matrix3x2.CreateScale(10000f / (minDim * _zoom));

            UpdateViewBounds();
        }

        private void UpdateTranslation(float dx, float dy)
        {
            _translate *= Matrix3x2.CreateTranslation(dx, dy);
            _invTranslate *= Matrix3x2.CreateTranslation(-dx, -dy);

            UpdateViewBounds();
        }

        private void UpdateViewBounds()
        {
            Vector2 tl = Vector2.Zero;
            Vector2 br = new Vector2(renderControl.Width, renderControl.Height);

            tl = Vector2.Transform(tl, InverseViewMatrix);
            br = Vector2.Transform(br, InverseViewMatrix);

            _bounds = Numerics.Rectangle.ContainingPoints(tl, br);
            if (_controller != null)
                _controller.ScreenBounds = _bounds;
        }

        private void renderControl_MouseWheel(object sender, MouseEventArgs e)
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
            zoomText.Text = (_zoom / 100f).ToString("0.##%");

            UpdateScale();

            UpdateSelected();
        }

        partial void renderControl_Render(SharpDX.Direct2D1.RenderTarget renderTarget, SharpDX.DXGI.SwapChain swapChain);

        private void renderControl_Layout(object sender, LayoutEventArgs e)
        {
            _centerTranslate = Matrix3x2.CreateTranslation(renderControl.Width / 2f, renderControl.Height / 2f);
            _invCenterTranslate = Matrix3x2.CreateTranslation(-renderControl.Width / 2f, -renderControl.Height / 2f);

            UpdateViewBounds();
            UpdateSelected();
        }

        private void zoomText_Leave(object sender, EventArgs e)
        {
            UpdateZoom();
        }

        /// <summary>
        /// Processes a command key.
        /// </summary>
        /// <returns>
        /// true if the keystroke was processed and consumed by the control; otherwise, false to allow further processing.
        /// </returns>
        /// <param name="msg">A <see cref="T:System.Windows.Forms.Message"/>, passed by reference, that represents the Win32 message to process. </param><param name="keyData">One of the <see cref="T:System.Windows.Forms.Keys"/> values that represents the key to process. </param>
        protected override bool ProcessCmdKey(ref Message message, Keys keyData)
        {
            if (zoomText.Focused)
                switch (keyData)
                {
                    case Keys.Enter:
                        UpdateZoom();
                        return true;
                    case Keys.Escape:
                        zoomText.Text = (_zoom / 100f).ToString("0.##%");
                        Unfocus();
                        return true;
                }
            return false;
        }

        private void renderControl_MouseMove(object sender, MouseEventArgs e)
        {
            UpdateSelected();
        }

        private void UpdateZoom()
        {
            string str = zoomText.Text.Trim();
            if (str.EndsWith(CultureInfo.CurrentCulture.NumberFormat.PercentSymbol))
                str = str.Substring(0, str.Length - CultureInfo.CurrentCulture.NumberFormat.PercentSymbol.Length);
            float zoom;
            if (!float.TryParse(str, out zoom))
            {
                SystemSounds.Asterisk.Play();
                zoomText.Focus();
                return;
            }

            zoom = (float)Math.Round(zoom, 2);
            if (zoom < _zoomLevels[_zoomLevels.Length - 1])
                zoom = _zoomLevels[_zoomLevels.Length - 1];
            else if (zoom > _zoomLevels[0])
                zoom = _zoomLevels[0];

            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (_zoom != zoom)
            {
                _zoom = zoom;

                UpdateScale();

                UpdateSelected();
            }

            zoomText.Text = (_zoom / 100f).ToString("0.##%");

            Unfocus();
        }

        private void UpdateSelected()
        {/*
            if (_shape == null) return;

            Matrix3x2 matrix = InverseViewMatrix;

            Vector2 loc = Vector2.Transform(new Vector2(_mouseLocation.X, _mouseLocation.Y), matrix);

            Vertex vertex = _shape.Vertices.OrderBy(v => Vector2.DistanceSquared(v.Location, loc)).First();
            if (Vector2.DistanceSquared(vertex.Location, loc) < 25)
            {
                _selected = vertex;
                return;
            }

            Edge edge = _shape.Edges.OrderBy(d => d.DistanceTo(loc)).First();
            if (edge.DistanceTo(loc) < 5)
            {
                _selected = edge;
                return;
            }
            _selected = null;
            //*/
        }

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
    }
}