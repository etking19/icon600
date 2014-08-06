using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace License
{
    public class LicenseChecker
    {
        public delegate void LicenseStatusDelegate(LicenseChecker checker, bool isValid);
        public event LicenseStatusDelegate EvtLicenseCheckStatus = null;

        private string licenseFilepath;
        private Worker workerObject = null;
        private Thread workerThread = null;

        public LicenseChecker(string licenseFilePath)
        {
            this.licenseFilepath = licenseFilePath;
        }

        public void StartCheck()
        {
            if (IsChecking())
            {
                return;
            }

            // spread a thread to check the internet status
            workerObject = new Worker(this, licenseFilepath);
            workerThread = new Thread(workerObject.DoWork);

            // register the event
            workerObject.EvtLicenseCheckStatus += workerObject_EvtLicenseCheckStatus;

            // Start the worker thread.
            workerThread.Start();
        }

        void workerObject_EvtLicenseCheckStatus(LicenseChecker.Worker worker, bool isValid)
        {
            if (EvtLicenseCheckStatus != null)
            {
                EvtLicenseCheckStatus(this, isValid);
            }
        }

        public bool IsChecking()
        {
            return (workerObject != null &&
                workerThread != null &&
                workerThread.IsAlive);
        }

        public void StopCheck()
        {
            // Request that the worker thread stop itself:
            workerObject.RequestStop();

            // Use the Join method to block the current thread  
            // until the object's thread terminates.
            workerThread.Join();

            workerObject = null;
            workerThread = null;
        }

        internal class Worker
        {
            public delegate void LicenseStatusDelegate(Worker worker, bool isValid);
            public event LicenseStatusDelegate EvtLicenseCheckStatus = null;

            // Volatile is used as hint to the compiler that this data 
            // member will be accessed by multiple threads. 
            private volatile bool _shouldStop;
            
            private LicenseChecker _caller;
            private string _filePath;

            public Worker(LicenseChecker callerClass, string filePath)
            {
                _caller = callerClass;
                _filePath = filePath;
            }

            // This method will be called when the thread is started. 
            public void DoWork()
            {
                while (!_shouldStop)
                {
                    if (EvtLicenseCheckStatus != null)
                    {
                        // use async invoke funtion
                        EvtLicenseCheckStatus.BeginInvoke(this, checkResult(), null, null);
                    }

                    Thread.Sleep(1000);
                }
            }

            public void RequestStop()
            {
                _shouldStop = true;
            }

            private bool checkResult()
            {
                // get the mac address of the machine and decoded contents
                string identifier = Utils.GetMachineIdentifier();
                string licenseData = Encryptor.GetInstance().DecodeContent(Utils.ReadFileByte(_filePath));
                if (licenseData.CompareTo(identifier) == 0)
                {
                    return true;
                }

                return false;
            }
        }

    }
}
