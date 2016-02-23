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
        private float _zoom = 100f;
        private Matrix3x2 _scale = Matrix3x2.Identity, _invScale = Matrix3x2.Identity;
        private Matrix3x2 _centerTranslate = Matrix3x2.Identity, _invCenterTranslate = Matrix3x2.Identity;
        private Matrix3x2 _translate = Matrix3x2.Identity, _invTranslate = Matrix3x2.Identity;

        private Matrix3x2 ViewMatrix =>
            _scale
            * _centerTranslate
            * _translate;

        private Matrix3x2 InverseViewMatrix =>
            _invTranslate
            * _invCenterTranslate
            * _invScale;

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


            Template template = new Template(
                new[]
                {
                    new ShapeTemplate(
                    "Triangle",
                    new[] { "a", "b", "c" },
                    new[] { "A", "B", "C" },
                    new[] { new Vector2(-100/2, 86.60254f / 2), new Vector2(100 / 2, 86.60254f / 2), new Vector2(0, -86.60254f / 2) })
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

            _shape = template.CreateShapes().First();

            renderControl.Start();
        }

        private Shape _shape;

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
            _invScale = Matrix3x2.CreateScale(_zoom / minDim);
        }

        private void AdjustTranslation(float dx, float dy)
        {
            _translate *= Matrix3x2.CreateTranslation(dx, dy);
            _invTranslate *= Matrix3x2.CreateTranslation(-dx, -dy);
        }

        private void renderControl_MouseWheel(object sender, MouseEventArgs e)
        {
            if (_zoom > 1)
                _zoom *= 1 - e.Delta / 1200f;
            
            AdjustScale();

            UpdateSelected();
        }

        partial void renderControl_RenderTargetChanged(SharpDX.Direct2D1.RenderTarget obj);

        partial void renderControl_Render(SharpDX.Direct2D1.RenderTarget renderTarget, SharpDX.DXGI.SwapChain swapChain);

        private void renderControl_Layout(object sender, LayoutEventArgs e)
        {
            _centerTranslate = Matrix3x2.CreateTranslation(renderControl.Width / 2f, renderControl.Height / 2f);
            _invCenterTranslate = Matrix3x2.CreateTranslation(-renderControl.Width / 2f, -renderControl.Height / 2f);

            UpdateSelected();
        }

        private object _selected;

        private Point _mouseLocation;

        private void renderControl_MouseMove(object sender, MouseEventArgs e)
        {
            _mouseLocation = e.Location;

            UpdateSelected();
        }

        private void UpdateSelected()
        {
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
        }
    }
}