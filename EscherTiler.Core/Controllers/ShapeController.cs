using System;
using EscherTiler.Dependencies;
using EscherTiler.Graphics;
using EscherTiler.Graphics.Resources;
using JetBrains.Annotations;

namespace EscherTiler.Controllers
{
    /// <summary>
    ///     Controller for editing a <see cref="Shape" />.
    /// </summary>
    /// <seealso cref="EscherTiler.Controllers.Controller" />
    public class ShapeController : Controller
    {
        [NotNull]
        private readonly Template _template;

        [NotNull]
        public readonly ShapeSet Shapes;

        [CanBeNull]
        private IResourceManager _resourceManager;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ShapeController" /> class.
        /// </summary>
        /// <param name="template">The template.</param>
        /// <param name="view">The view.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="System.InvalidOperationException"></exception>
        public ShapeController([NotNull] Template template, [NotNull] IView view)
            : base(view)
        {
            if (template == null) throw new ArgumentNullException(nameof(template));

            _template = template;
            Shapes = template.CreateShapes();

            IResourceManager resourceManager = DependencyManger.GetResourceManager();
            if (resourceManager == null) throw new InvalidOperationException();
            _resourceManager = resourceManager;
        }

        /// <summary>
        ///     Draws this object to the <see cref="T:EscherTiler.Graphics.IGraphics" /> provided.
        /// </summary>
        /// <param name="graphics">The graphics object to use to draw this object.</param>
        public override void Draw(IGraphics graphics)
        {
            if (_resourceManager == null) throw new ObjectDisposedException(nameof(ShapeController));

            graphics.ResourceManager = _resourceManager;

            foreach (Shape shape in Shapes)
            {
                using (IGraphicsPath path = graphics.CreatePath())
                {
                    bool first = true;
                    foreach (Vertex vertex in shape.Vertices)
                    {
                        if (first) path.Start(vertex.Location);
                        else path.AddLine(vertex.Location);
                        first = false;
                    }

                    path.End();

                    graphics.DrawPath(path);
                }
            }
        }

        /// <summary>
        ///     Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing">
        ///     <see langword="true" /> to release both managed and unmanaged resources;
        ///     <see langword="false" /> to release only unmanaged resources.
        /// </param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
                DependencyManger.ReleaseResourceManager(ref _resourceManager, null);
        }
    }
}