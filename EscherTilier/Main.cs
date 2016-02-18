using System;
using System.Windows.Forms;
using EscherTilier.Properties;
using JetBrains.Annotations;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.DXGI;
using FactoryD2D = SharpDX.Direct2D1.Factory;
using FactoryDXGI = SharpDX.DXGI.Factory1;

namespace EscherTilier
{
    public partial class Main : Form
    {
        // private TextFormat _textFormat;

        public Main()
        {
            InitializeComponent();

            statusInfoLabel.Text = string.Empty;

            //using (Factory factory = new Factory())
            //{
            //    _textFormat = new TextFormat(factory, "Calibri", 24.0f);
            //}
        }

        public void RenderLoop() => renderControl.RenderLoop();

        private void Main_Load(object sender, EventArgs e)
        {
            switch (Settings.Default.WindowState)
            {
                case FormWindowState.Maximized:
                    WindowState = FormWindowState.Maximized;
                    break;
                default:
                    WindowState = FormWindowState.Normal;
                    break;
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            Settings.Default.WindowState = WindowState;

            if (WindowState != FormWindowState.Maximized)
                Settings.Default.WindowSize = Size;

            Settings.Default.Save();
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

        private void renderControl_Render([NotNull] RenderTarget renderTarget, [NotNull] SwapChain swapChain)
        {
            renderTarget.BeginDraw();
            renderTarget.Transform = Matrix3x2.Identity;
            renderTarget.Clear(Color.White);

            using (SolidColorBrush brush = new SolidColorBrush(renderTarget, Color.LightSlateGray))
            {
                for (int x = 0; x < renderTarget.Size.Width; x += 10)
                    renderTarget.DrawLine(new Vector2(x, 0), new Vector2(x, renderTarget.Size.Height), brush, 0.5f);

                for (int y = 0; y < renderTarget.Size.Height; y += 10)
                    renderTarget.DrawLine(new Vector2(0, y), new Vector2(renderTarget.Size.Width, y), brush, 0.5f);

                renderTarget.FillRectangle(
                    new RectangleF(renderTarget.Size.Width / 2 - 50, renderTarget.Size.Height / 2 - 50, 100, 100),
                    brush);
            }

            renderTarget.DrawRectangle(
                new RectangleF(renderTarget.Size.Width / 2 - 100, renderTarget.Size.Height / 2 - 100, 200, 200),
                new SolidColorBrush(renderTarget, Color.CornflowerBlue));

            //Vector2? mouseLoc = _mouseLoc;
            //if (mouseLoc.HasValue)
            //{
            //    renderTarget.FillEllipse(
            //        new Ellipse(mouseLoc.Value, 2, 2),
            //        new SolidColorBrush(renderTarget, Color.Black));
            //}
            //
            //int f = Interlocked.Increment(ref _frame);
            //renderTarget.DrawText(
            //    f.ToString(),
            //    textFormat,
            //    new RectangleF(10, 10, 400, 100),
            //    new SolidColorBrush(renderTarget, Color.Black));

            renderTarget.EndDraw();
            swapChain.Present(0, PresentFlags.None);
        }
    }
}