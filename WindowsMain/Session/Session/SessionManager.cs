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

        public void StartSession()
        {
            _Session.start();
        }

        public void StopSession()
        {
            _Session.stop();
        }

        public bool IsSessionStarted()
        {
            return _Session.isStarted();
        }

        public void SendMessage(string data)
        {
            _Session.sendMessage(Utils.StringEncoding.ConvertStringToBytes(data));
        }
    }
}
