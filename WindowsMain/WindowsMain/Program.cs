using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsMain
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

            Action action = new Action(StartClient);

            for (int a = 0; a < 2; a++ )
            {
                Task.Factory.StartNew(action);
            }
            

            Application.Run(new FormMain());
        }

        static void StartClient()
        {
            FormClient formClient = new FormClient();
            formClient.ShowDialog();
        }
    }
}
