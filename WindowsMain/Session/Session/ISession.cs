using System;
using System.Collections.Generic;
using System.Text;

namespace Session.Session
{
    public abstract class ISession
    {
        public abstract void start();
        public abstract void stop();

        public abstract bool isStarted();

        public abstract int getPortNumber();

        public abstract void sendMessage(byte[] data);
    }
}
