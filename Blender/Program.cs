using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Blender
{
    public static class Program
    {
        public static Main g_Main;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            g_Main = new Main();
            Application.Run(g_Main);
        }

        public static void Log(string msg)
        {
            g_Main.Log(msg);
        }

        public static void Status(string msg)
        {
            g_Main.Status(msg);
        }

        public static void Progress(int count, int max)
        {
            g_Main.Progress(count, max);
        }
    }
}
