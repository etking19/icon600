using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using WcfServiceLibrary1;

namespace RemoteFormServer
{
    public partial class Form1 : Form
    {
        private System.Threading.SynchronizationContext syncContext = AsyncOperationManager.SynchronizationContext;
        private EventHandler _broadcastorCallBackHandler;

        public void SetHandler(EventHandler handler)
        {
            this._broadcastorCallBackHandler = handler;
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            InstanceContext instanceContext = new InstanceContext(new CallbackHandler());
            EndpointAddress address = new EndpointAddress(new Uri("net.tcp://localhost:8080/Service1"));
            DuplexChannelFactory<IService1> dupFactory = new DuplexChannelFactory<IService1>(instanceContext, new NetTcpBinding(), address);
            dupFactory.Open();

            IService1 patientSvc = dupFactory.CreateChannel();
            MessageBox.Show("" + patientSvc.AddGroup("test", true, false, 0, new List<int>()));
        }
    }

    [CallbackBehavior(ConcurrencyMode = ConcurrencyMode.Single, UseSynchronizationContext = false)]
    public class CallbackHandler : IServiceCallback
    {

        public void OnUserDBChanged()
        {
            int a = 0;
        }
    }
}
