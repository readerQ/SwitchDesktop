using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace HotKeyAgent
{
    internal static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Process current = Process.GetCurrentProcess();
            string debugger = (Debugger.IsAttached ? "-debug" : "");
            string currentProcessName = $"{current.ProcessName}{debugger}";

            // run only one insatace
            using (Mutex mutex = new Mutex(true, currentProcessName, out bool createdNew))
            {
                if (createdNew)
                {

                    RunApp();
                }
            }
        }

        private static void RunApp()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MyCustomApplicationContext());
        }
    }
}
