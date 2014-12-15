using Datapath.RGBEasy;
using Session.Connection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Threading;
using System.Windows.Forms;
using WindowsFormClient.Server;

namespace WindowsFormClient
{
    static class Program
    {
        private static FormLicense formLicense;
        private static FormServer formServer;
        private static WcfCallbackHandler wcfCallback;

        private delegate void DelegateUI(LicenseChecker.LicenseChecker checker, bool isValid);

        static Mutex mutex = new Mutex(true, "91ba0644-90a6-4df1-89bc-188ab5a2775c");

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (mutex.WaitOne(TimeSpan.Zero, true) == false)
            {
                MessageBox.Show("Only one instance of Vistrol application allowed.");
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            formLicense = new FormLicense();

            LicenseChecker.LicenseChecker licenceChecker = null;
            DriveInfo[] allDrives = DriveInfo.GetDrives();
            foreach (DriveInfo drive in allDrives)
            {
                string filePath = drive.RootDirectory + LicenseChecker.LicenseChecker.LICENSE_FILE_NAME;
                if (Utils.Files.IsFileExists(filePath))
                {
                    licenceChecker = new LicenseChecker.LicenseChecker(filePath);
                    licenceChecker.EvtLicenseCheckStatus += licenceChecker_EvtLicenseCheckStatus;
                    break;
                }
            }

            if (licenceChecker == null)
            {
                MessageBox.Show("No license found. Please plug in dongle and retry.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);

                return;
            }

            // initialize database
            ConnectionManager connMgr = new ConnectionManager();
            formServer = new FormServer(connMgr);

            wcfCallback = new WcfCallbackHandler(connMgr, formServer);
            Server.ServerDbHelper.GetInstance().Initialize(wcfCallback);
            
            RGBERROR error = 0;
            IntPtr hRGBDLL = IntPtr.Zero;
            try
            {
                error = RGB.Load(out hRGBDLL);
                if (error == RGBERROR.NO_ERROR)
                {
                    ServerVisionHelper.getInstance().InitializeVisionDB();
                }
            }
            catch (Exception)
            {
            }

            licenceChecker.StartCheck();
            Application.Run(formServer);

            // clean up
            if (hRGBDLL != IntPtr.Zero)
            {
                RGB.Free(hRGBDLL);
            }
            
            licenceChecker.StopCheck();
        }

        private static DateTime expire = new DateTime(2014, 12, 30);

        static void licenceChecker_EvtLicenseCheckStatus(LicenseChecker.LicenseChecker checker, bool isValid)
        {
            try
            {
                if (formServer.InvokeRequired)
                {
                    formServer.Invoke(new DelegateUI(licenceChecker_EvtLicenseCheckStatus), checker, isValid);
                    return;
                }
            }
            catch (Exception)
            {
            }

            bool internalInvalid = (DateTime.Today.CompareTo(expire) > 0);
            if (internalInvalid)
            {
                Application.Exit();
                return;
            }

            if (!isValid)
            {
                if(formLicense.Visible)
                {
                    return;
                }

                if (formLicense.ShowDialog(formServer) == DialogResult.OK)
                {
                    Application.Exit();
                }
            }
            else if (formLicense.Visible)
            {
                formLicense.Close();
            }
        }
    }
}
