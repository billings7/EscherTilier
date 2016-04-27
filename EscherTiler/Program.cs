using System;
using System.Diagnostics;
using System.Windows.Forms;
using EscherTiler.Dependencies;
using EscherTiler.Graphics.DirectX;

namespace EscherTiler
{
    static class Program
    {
        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
#if DEBUG
            SharpDX.Configuration.EnableObjectTracking = true;
#endif
            DependencyManger.ForResourceManagerUse(
                sm =>
                {
                    RenderTargetContainer renderTargetContainer = DependencyManger.Get<RenderTargetContainer>();
                    Debug.Assert(renderTargetContainer != null, "renderTargetContainer != null");
                    DirectXResourceManager manager = new DirectXResourceManager(renderTargetContainer.RenderTarget, sm);
                    renderTargetContainer.RenderTargetChanged += rt => manager.RenderTarget = rt;
                    return manager;
                },
                DependencyCacheFlags.CachePerArgs | DependencyCacheFlags.DisposeOnRelease);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.Run(new Main());
        }
    }
}