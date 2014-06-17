using System;
using System.Collections.Generic;
using System.Text;

namespace Session.Session
{
    public class SessionManager
    {
        private ISession _Session;

        public SessionManager(ISession session)
        {
            _Session = session;
        }

        public ISession GetSession()
        {
            return _Session;
        }

        public bool StartSession()
        {
            return _Session.start();
        }

        public void StopSession()
        {
            _Session.stop();
        }

        public bool IsSessionStarted()
        {
            return _Session.isStarted();
        }

        public void BroadcastMessage(string data)
        {
            _Session.broadcastMessage(Utils.StringEncoding.ConvertStringToBytes(data));
        }

        public void SendMessage(string data, List<string> desireReceiver)
        {
            ServerSession serverSession = _Session as ServerSession;

            if (serverSession != null)
            {
                serverSession.sendMessage(Utils.StringEncoding.ConvertStringToBytes(data), desireReceiver);
            }
            else
            {
                _Session.broadcastMessage(Utils.StringEncoding.ConvertStringToBytes(data));
            }
            
        }
    }
}
