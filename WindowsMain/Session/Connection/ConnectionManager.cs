using System;
using Session.Session;
using System.Diagnostics;
using Session.Data;
using System.Runtime.CompilerServices;
using Utils;
using System.Collections.Generic;

namespace Session.Connection
{
    public class ConnectionManager
    {
        public delegate void OnClientConnectedEvt(string userId);
        public event OnClientConnectedEvt EvtClientConnected;

        public delegate void OnClientDisconnectedEvt(string userId);
        public event OnClientConnectedEvt EvtClientDisconnected;

        public delegate void OnClientDataReceived(string userId, int mainId, int subId, string commandData);
        public event OnClientDataReceived EvtClientDataReceived;

        public delegate void OnConnected();
        public event OnConnected EvtConnected;

        public delegate void OnDisconnected();
        public event OnDisconnected EvtDisconnected;

        public delegate void OnServerDataReceived(int mainId, int subId, string commandData);
        public event OnServerDataReceived EvtServerDataReceived;

        private System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();

        private SessionManager _SessionMgr;

        public ConnectionManager()
        {
        }

        public int StartServer(int portStart, int portEnd)
        {
            if (_SessionMgr != null &&
                IsStarted())
            {
                return -1;
            }

            int port = Socket.getUnusedPort(portStart, portEnd);
            ServerSession serverSession = new ServerSession(port);
            serverSession.OnClientConnection += new ServerSession.ClientConnectionEventHandler(serverSession_OnClientConnection);
            serverSession.LostConnection += new ServerSession.LostConnectionEventHandler(serverSession_LostConnection);
            serverSession.DataReceived += new ServerSession.DataReceivedEventHandler(serverSession_DataReceived);

            _SessionMgr = new SessionManager(serverSession);

            if(_SessionMgr.StartSession())
            {
                return port;
            }

            return -1;
        }

        void serverSession_DataReceived(string ID, byte[] Data)
        {
            if (EvtClientDataReceived != null)
            {
                string data = Utils.StringEncoding.ConvertBytesToString(Data);

                System.Web.Script.Serialization.JavaScriptSerializer deserialize = new System.Web.Script.Serialization.JavaScriptSerializer();

                try
                {
                    Data.MainCommand dataObj = deserialize.Deserialize<Data.MainCommand>(data);
                    EvtClientDataReceived(ID, dataObj.mainCommandId, dataObj.subCommandId, dataObj.data);
                }
                catch (Exception e)
                {
                    Trace.WriteLine(e.Message);
                }
            }
        }

        void serverSession_LostConnection(string id)
        {
            if (EvtClientDisconnected != null)
            {
                EvtClientDisconnected(id);
            }
        }

        void serverSession_OnClientConnection(string id)
        {
            if(EvtClientConnected != null)
            {
                EvtClientConnected(id);
            }
        }

        public void StopServer()
        {
            if (IsStarted() == false)
            {
                return;
            }

            ServerSession serverSession = (ServerSession)_SessionMgr.GetSession();
            serverSession.OnClientConnection -= serverSession_OnClientConnection;
            serverSession.LostConnection -= serverSession_LostConnection;
            serverSession.DataReceived -= serverSession_DataReceived;

            _SessionMgr.StopSession();
            _SessionMgr = null;
        }

        public void RemoveClient(string userId)
        {
            if (IsStarted() == false)
            {
                return;
            }

            ServerSession serverSession = (ServerSession)_SessionMgr.GetSession();
            serverSession.RemoveClient(userId);
        }

        public bool StartClient(string hostIP, int hostPort)
        {
            if (_SessionMgr != null &&
                IsStarted())
            {
                return false;
            }

            ClientSession clientSession = new ClientSession(hostIP, hostPort, Utils.StringEncoding.RandomString(10));
            clientSession.OnConnection += new ClientSession.OnConnectionEventHandler(clientSession_OnConnection);
            clientSession.ConnectionClosed += new ClientSession.ConnectionClosedEventHandler(clientSession_ConnectionClosed);
            clientSession.DataReceived += new ClientSession.DataReceivedEventHandler(clientSession_DataReceived);
            
            _SessionMgr = new SessionManager(clientSession);
            return _SessionMgr.StartSession();
        }


        void clientSession_DataReceived(string ID, byte[] Data)
        {
            if (EvtServerDataReceived != null)
            {
                string data = Utils.StringEncoding.ConvertBytesToString(Data);

                try
                {
                    System.Web.Script.Serialization.JavaScriptSerializer deserialize = new System.Web.Script.Serialization.JavaScriptSerializer();
                    Data.MainCommand dataObj = deserialize.Deserialize<Data.MainCommand>(data);

                    EvtServerDataReceived(dataObj.mainCommandId, dataObj.subCommandId, dataObj.data);
                }
                catch (Exception e)
                {
                    Trace.WriteLine(e.Message);
                }
                
            }
        }

        void clientSession_ConnectionClosed()
        {
            StopClient();
            if(EvtDisconnected != null)
            {
                EvtDisconnected();
            }
        }

        void clientSession_OnConnection(string id)
        {
            if(EvtConnected != null)
            {
                EvtConnected();
            }
        }

        public void StopClient()
        {
            if (IsStarted() == false)
            {
                return;
            }

            ClientSession clientSession = (ClientSession)_SessionMgr.GetSession();
            clientSession.OnConnection -= clientSession_OnConnection;
            clientSession.ConnectionClosed -= clientSession_ConnectionClosed;
            clientSession.DataReceived -= clientSession_DataReceived;

            _SessionMgr.StopSession();
            _SessionMgr = null;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void BroadcastMessage(int mainId, int subId, BaseCmd cmdObj)
        {
            if (IsStarted() == false)
            {
                return;
            }

            MainCommand command = new MainCommand();
            command.mainCommandId = mainId;
            command.subCommandId = subId;
            command.data = cmdObj.getCommandString();

            string message = serializer.Serialize(command);
            _SessionMgr.BroadcastMessage(message);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void SendData(int mainId, int subId, BaseCmd cmdObj, List<string> desireReceiver)
        {
            if (IsStarted() == false)
            {
                return;
            }

            MainCommand command = new MainCommand();
            command.mainCommandId = mainId;
            command.subCommandId = subId;
            command.data = cmdObj.getCommandString();

            string message = serializer.Serialize(command);
            _SessionMgr.SendMessage(message, desireReceiver);
        }

        public bool IsStarted()
        {
            if (_SessionMgr == null)
            {
                return false;
            }

            return _SessionMgr.IsSessionStarted();
        }
    }
}
