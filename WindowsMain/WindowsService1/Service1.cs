using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceProcess;
using System.Text;
using WcfServiceLibrary1;

namespace WindowsService1
{
    public partial class Service1 : ServiceBase
    {
        internal static ServiceHost myServiceHost = null; 

        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            if (myServiceHost != null)
            {
                myServiceHost.Close();
            }

            string strAdrTCP = "net.tcp://localhost:45100/Service1";

            Uri[] adrbase = { new Uri(strAdrTCP) };
            myServiceHost = new ServiceHost(typeof(WcfServiceLibrary1.Service1), adrbase);

            ServiceMetadataBehavior smb = myServiceHost.Description.Behaviors.Find<ServiceMetadataBehavior>();
            // If not, add one
            if (smb == null)
                smb = new ServiceMetadataBehavior();

            smb.HttpGetEnabled = false;
            myServiceHost.Description.Behaviors.Add(smb);

            myServiceHost.AddServiceEndpoint(
                  ServiceMetadataBehavior.MexContractName,
                  MetadataExchangeBindings.CreateMexTcpBinding(),
                  "mex"
                );

            myServiceHost.AddServiceEndpoint(typeof(WcfServiceLibrary1.IService1), new NetTcpBinding(SecurityMode.None), strAdrTCP);

            myServiceHost.Open();
        }

        protected override void OnStop()
        {
            if (myServiceHost != null)
            {
                myServiceHost.Close();
                myServiceHost = null;
            }
        }
    }
}
