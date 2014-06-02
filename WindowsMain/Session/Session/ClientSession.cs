using BasicClientServerLib.Client;
using BasicClientServerLib.Message;
using System;
using System.Diagnostics;

namespace Session.Session
{
    public class ClientSession : ISession
    {
        public delegate void ConnectionClosedEventHandler();
        public delegate void DataReceivedEventHandler(string ID, byte[] Data);
        public delegate void OnConnectionEventHandler(string id);

        public event OnConnectionEventHandler OnConnection;
        public event ConnectionClosedEventHandler ConnectionClosed;
        public event DataReceivedEventHandler DataReceived;

        private BasicSocketClient _Client;

        private string _HostIP = "";
        private int _HostPort = 0;
        private string _ID = "";
        private Guid _Guid;

        public ClientSession(string hostIP, int hostPort, string userId)
        {
            _HostIP = hostIP;
            _HostPort = hostPort;
            _ID = userId;
            _Guid = Guid.NewGuid();
            _Client = new BasicSocketClient();
        }

        public override void start()
        {
            //Adding event handling methods for the client
            _Client.ReceiveMessageEvent += new SocketServerLib.SocketHandler.ReceiveMessageDelegate(_Client_DataReceived);
            _Client.ConnectionEvent += new SocketServerLib.SocketHandler.SocketConnectionDelegate(_Client_Connected);
            _Client.CloseConnectionEvent += new SocketServerLib.SocketHandler.SocketConnectionDelegate(_Client_Disconnected);

            System.Net.IPAddress targetIP;
            if(System.Net.IPAddress.TryParse(_HostIP, out targetIP))
            {
                _Client.Connect(new System.Net.IPEndPoint(targetIP, _HostPort));
            }
        }

        void _Client_DataReceived(SocketServerLib.SocketHandler.AbstractTcpSocketClientHandler handler, SocketServerLib.Message.AbstractMessage message)
        {
            if(DataReceived != null)
            {
                BasicMessage receivedMessage = (BasicMessage)message;
                DataReceived(receivedMessage.ClientUID, receivedMessage.GetBuffer());
            }
        }

        void _Client_Disconnected(SocketServerLib.SocketHandler.AbstractTcpSocketClientHandler handler)
        {
            if (ConnectionClosed != null)
            {
                ConnectionClosed();
            }
        }

        void _Client_Connected(SocketServerLib.SocketHandler.AbstractTcpSocketClientHandler handler)
        {
            if (OnConnection != null)
            {
                OnConnection(_ID);
            }
        }

        public override void stop()
        {
            _Client.Close();
        }

        public override bool isStarted()
        {
            return _Client.IsConnected;
        }

        public override int getPortNumber()
        {
            return _HostPort;
        }

        public override void sendMessage(byte[] data)
        {
            BasicMessage message = new BasicMessage(this._Guid, data);
            _Client.SendAsync(message);
        }
    }
}
