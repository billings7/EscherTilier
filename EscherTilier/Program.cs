using System;
using System.Diagnostics;
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

            Task.Run(
                async () =>
                {
                    while (true)
                    {
                        Debug.WriteLine(SharpDX.Diagnostics.ObjectTracker.ReportActiveObjects());
                        await Task.Delay(1000);
                    }
                });

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.Run(new Main());
        }
    }
}