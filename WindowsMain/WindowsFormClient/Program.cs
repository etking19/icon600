using Session.Connection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace WindowsFormClient
{
    static class Program
    {
        //static Mutex mutex = new Mutex(true, "00bdd546-a42b-42b4-bc49-48749d88a2e8");

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //if (mutex.WaitOne(TimeSpan.Zero, true))
            //{
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new FormLogin());
               // mutex.ReleaseMutex();
           // }
           // else
           // {
            //    MessageBox.Show("Only one instance of Vistrol application allowed.");
           // }

            
        }
    }
}
