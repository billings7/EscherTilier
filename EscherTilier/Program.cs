using System;
using System.Threading;
using System.Windows.Forms;
using SharpDX.Windows;

namespace EscherTilier
{
    static class Program
    {
        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            Main form = new Main();
            Thread renderThread = new Thread(form.RenderLoop)
            {
                Name = "Render Thread",
                IsBackground = true
            };
            renderThread.Start();
            Application.Run(form);
        }
    }
}