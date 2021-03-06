﻿using BasicClientServerLib.Message;
using BasicClientServerLib.Server;
using SocketServerLib.Server;
using SocketServerLib.SocketHandler;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Threading;

namespace Session.Session
{
    public class ServerSession : ISession
    {
        public delegate void DataReceivedEventHandler(string ID, byte[] Data);
        public delegate void LostConnectionEventHandler(string id);
        public delegate void ClientConnectionEventHandler(string id);

        public event DataReceivedEventHandler DataReceived;
        public event LostConnectionEventHandler LostConnection;
        public event ClientConnectionEventHandler OnClientConnection;

        private BasicSocketServer _Server;
        private int _ListeningPort = 0;
        private Guid _ServerGuid = Guid.NewGuid();
        private bool isServerStarted = false;

       // private KeepAlive keepAliveWorker = new KeepAlive();
       // private Thread workerThread = null;

        private BackgroundWorker worker;

        public ServerSession(int listeningPort)
        {
            _ListeningPort = listeningPort;
            _Server = new BasicSocketServer();

            //keepAliveWorker.EvtSocketCheck += keepAliveWorker_EvtSocketCheck;
            //workerThread = new Thread(keepAliveWorker.DoWork);
            //workerThread.Start();
            //while (!workerThread.IsAlive) ;
        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            while(true)
            {
                if (_Server == null)
                {
                    continue;
                }

                if (isServerStarted == false)
                {
                    continue;
                }

                ClientInfo[] clientList = _Server.GetClientList();
                foreach (ClientInfo client in clientList)
                {
                    AbstractTcpSocketClientHandler clientHandler = client.TcpSocketClientHandler;

                    if (clientHandler.Connected == false)
                    {
                        Trace.WriteLine(String.Format("Disconnected: {0}", clientHandler.GetHashCode().ToString()));
                        clientHandler.Close();
                    }
                }

                Thread.Sleep(5000);
            }
        }

        //void keepAliveWorker_EvtSocketCheck(object sender)
        //{
           
        //}

        public override bool start()
        {
            //Adding event handling methods, to handle the server messages
            _Server.ReceiveMessageEvent += new SocketServerLib.SocketHandler.ReceiveMessageDelegate(Server_DataReceived);
            _Server.ConnectionEvent += new SocketConnectionDelegate(Server_onConnection);
            _Server.CloseConnectionEvent += new SocketConnectionDelegate(Server_lostConnection);

            try
            {
                _Server.Init(new IPEndPoint(IPAddress.Any, _ListeningPort));
                _Server.StartUp();
                isServerStarted = true;

                worker = new BackgroundWorker();
                worker.WorkerSupportsCancellation = true;
                worker.DoWork += worker_DoWork;
                worker.RunWorkerAsync();
            }
            catch (Exception e)
            {
                Trace.WriteLine(e);
                return false;
            }
            
            return true;
        }

        void Server_DataReceived(SocketServerLib.SocketHandler.AbstractTcpSocketClientHandler handler, SocketServerLib.Message.AbstractMessage message)
        {
            if (DataReceived != null)
            {
                DataReceived(handler.GetHashCode().ToString(), message.GetBuffer());
            }
        }

        void Server_lostConnection(AbstractTcpSocketClientHandler handler)
        {
            if(LostConnection != null)
            {
                LostConnection(handler.GetHashCode().ToString());
            }
        }

        void Server_onConnection(AbstractTcpSocketClientHandler handler)
        {
            handler.KeepAlive = true;
            if(OnClientConnection != null)
            {
                OnClientConnection(handler.GetHashCode().ToString());
            }
        }

        public override void stop()
        {
            worker.CancelAsync();

            _Server.Shutdown();
            isServerStarted = false;
        }

        public override bool isStarted()
        {
            return isServerStarted;
        }

        public override int getPortNumber()
        {
            return _ListeningPort;
        }

        public override void broadcastMessage(byte[] data)
        {
            ClientInfo[] clientList = _Server.GetClientList();
            foreach (ClientInfo client in clientList)
            {
                AbstractTcpSocketClientHandler clientHandler = client.TcpSocketClientHandler;
                BasicMessage message = new BasicMessage(_ServerGuid, data);
                try
                {
                    clientHandler.SendAsync(message);
                }
                catch (Exception)
                {
                    Trace.WriteLine(String.Format("failed to send data :{0}", clientHandler.GetHashCode().ToString()));
                }
            }
        }

        public override void sendMessage(byte[] data, List<string> desireReceiver)
        {
            ClientInfo[] clientList = _Server.GetClientList();
            foreach (ClientInfo client in clientList)
            {
                AbstractTcpSocketClientHandler clientHandler = client.TcpSocketClientHandler;
                foreach (string receiver in desireReceiver)
                {
                    if (clientHandler.GetHashCode().ToString().CompareTo(receiver) == 0)
                    {
                        BasicMessage message = new BasicMessage(_ServerGuid, data);
                        try
                        {
                            clientHandler.SendAsync(message);
                        }
                        catch (Exception)
                        {
                            Trace.WriteLine(String.Format("failed to send data :{0}", receiver));
                        }
                        
                    }
                }
            }
        }

        public void RemoveClient(string userId)
        {
            ClientInfo[] clientList = _Server.GetClientList();
            foreach (ClientInfo client in clientList)
            {
                if (client.TcpSocketClientHandler.GetHashCode().ToString() == userId)
                {
                    client.TcpSocketClientHandler.Close();
                    break;
                }
            }
        }
    }
}
