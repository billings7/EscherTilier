using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

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
#if DEBUG
            SharpDX.Configuration.EnableObjectTracking = true;
#endif
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.Run(new Main());

            GC.Collect();
            GC.WaitForPendingFinalizers();

            Thread.Sleep(10000);

            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }
}