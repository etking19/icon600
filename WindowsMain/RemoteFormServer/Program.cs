using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using WindowsFormClient;

namespace RemoteFormServer
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            FormConnect formConnect = new FormConnect("username", "password");
            Application.Run(formConnect);
        }
    }
}
