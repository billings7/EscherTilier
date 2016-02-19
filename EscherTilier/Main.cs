using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using EscherTilier.Graphics.DirectX;
using EscherTilier.Properties;
using EscherTilier.Styles;
using System.Numerics;
using EscherTilier.Expressions;

namespace EscherTilier
{
    public partial class Main : Form
    {
        private float _zoom = 300f;
        private Matrix3x2 _scale = Matrix3x2.Identity;
        private Matrix3x2 _translate = Matrix3x2.Identity;

        private Matrix3x2 ViewMatrix => Matrix3x2.CreateScale(1, -1) * _scale *
                                        Matrix3x2.CreateTranslation(renderControl.Width / 2f, renderControl.Height / 2f) * _translate;

        public Main()
        {
            InitializeComponent();

            statusInfoLabel.Text = string.Empty;
            _miskResourceManager = new DirectXResourceManager(renderControl.RenderTarget);
            _directXGraphics = new DirectXGraphics(
                renderControl.RenderTarget,
                _miskResourceManager,
                _greyStyle,
                new LineStyle(2, _blackStyle));

            renderControl.MouseWheel += renderControl_MouseWheel;

            //using (Factory factory = new Factory())
            //{
            //    _textFormat = new TextFormat(factory, "Calibri", 24.0f);
            //}
        }

        public void RenderLoop() => renderControl.RenderLoop();

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


            Template template = new Template(
                new[]
                {
                    new ShapeTemplate(
                    "Triangle",
                    new[] { "a", "b", "c" },
                    new[] { "A", "B", "C" },
                    new[] { new System.Numerics.Vector2(-100, -86.60254f), new System.Numerics.Vector2(100, -86.60254f), new System.Numerics.Vector2(0, 86.60254f) })
                },
                new IExpression<bool>[0],
                new[]
                {
                    new TilingDefinition(
                    1,
                    null,
                    new[]
                    {
                        new EdgePattern("a", new[] { new EdgePart(1, PartDirection.ClockwiseIn, 1) }),
                        new EdgePattern("b", new[] { new EdgePart(2, PartDirection.ClockwiseIn, 1) }),
                        new EdgePattern("c", new[] { new EdgePart(1, PartDirection.ClockwiseIn, 1) }),
                    },
                    new AdjacencyGraph<Labeled<EdgePart>>())
                });

            shape = template.CreateShapes().First();
        }

        private Shape shape;

        /// <summary>
        ///     Raises the <see cref="E:System.Windows.Forms.Form.Closing" /> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.ComponentModel.CancelEventArgs" /> that contains the event data. </param>
        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            Settings.Default.WindowState = WindowState;
            Settings.Default.WindowSize = _lastNormalSize;
            Settings.Default.WindowLocation = _lastNormalLocation;

            Settings.Default.Save();
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

            AdjustScale();
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


        private void AdjustScale()
        {
            int minDim = Math.Min(renderControl.Width, renderControl.Height);

            _scale = Matrix3x2.CreateScale(minDim / _zoom);
        }

        private void AdjustTranslation(float dx, float dy)
        {
            _translate *= Matrix3x2.CreateTranslation(dx, dy);
        }

        private void renderControl_MouseWheel(object sender, MouseEventArgs e)
        {
            _zoom *= 1 - e.Delta / 1200f;
            AdjustScale();
        }

        partial void renderControl_RenderTargetChanged(SharpDX.Direct2D1.RenderTarget obj);

        partial void renderControl_Render(SharpDX.Direct2D1.RenderTarget renderTarget, SharpDX.DXGI.SwapChain swapChain);

        private object _selected;

        private void renderControl_MouseMove(object sender, MouseEventArgs e)
        {
            Matrix3x2 matrix;
            if (!Matrix3x2.Invert(ViewMatrix, out matrix))
                throw new InvalidOperationException();

            Vector2 loc = Vector2.Transform(new Vector2(e.X, e.Y), matrix);

            Vertex vertex = shape.Vertices.OrderBy(v => Vector2.DistanceSquared(v.Location, loc)).First();
            if (Vector2.DistanceSquared(vertex.Location, loc) < 25)
            {
                _selected = vertex;
                return; ;
            }

            Edge edge = shape.Edges.OrderBy(d => d.DistanceTo(loc)).First();
            if (edge.DistanceTo(loc) < 5)
            {
                _selected = edge;
                return; ;
            }
            _selected = null;
        }
    }
}