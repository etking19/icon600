﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace WindowsFormClient
{
    static class Program
    {
        private static FormLicense formLicense;
        private static FormServer formServer;

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

            LicenseChecker.LicenseChecker licenceChecker = null;
            DriveInfo[] allDrives = DriveInfo.GetDrives();
            foreach (DriveInfo drive in allDrives)
            {
                string filePath = drive.RootDirectory + LicenseChecker.LicenseChecker.LICENSE_FILE_NAME;
                if (Utils.Files.IsFileExists(filePath))
                {
                    licenceChecker = new LicenseChecker.LicenseChecker(filePath);
                    licenceChecker.EvtLicenseCheckStatus += licenceChecker_EvtLicenseCheckStatus;
                    licenceChecker.StartCheck();
                }
            }

            if (licenceChecker == null)
            {
                MessageBox.Show("No license found. Please plug in dongle and retry.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                return;
            }

            formLicense = new FormLicense();
            formServer = new FormServer();
            Application.Run(formServer);

            licenceChecker.StopCheck();
        }

        static void licenceChecker_EvtLicenseCheckStatus(LicenseChecker.LicenseChecker checker, bool isValid)
        {
            if (formServer.InvokeRequired)
            {
                formServer.Invoke(new DelegateUI(licenceChecker_EvtLicenseCheckStatus), checker, isValid);
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
