using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WindowsFormClient.Server
{
    public class ThreadClientLogin
    {
        public string Id { get; set; }
        public ManualResetEvent ResetEvt { get; set; }
    }
}
