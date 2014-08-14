using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Session.Session
{
    class KeepAlive
    {
        public delegate void OnSocketCheck(object sender);
        public event OnSocketCheck EvtSocketCheck;

        private volatile bool _shouldStop = false;

        public void DoWork()
        {
            while(!_shouldStop)
            {
                // check all the connections
                if (EvtSocketCheck != null)
                {
                    EvtSocketCheck(this);
                }

                Thread.Sleep(2000);
            }
        }

        public void RequestStop()
        {
            _shouldStop = true;
        }
    }
}
