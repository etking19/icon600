using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WindowsFormServer.Server.Model;

namespace WindowsFormServer.Server
{
    class ConnectedClientHelper
    {
        private static ConnectedClientHelper sInstance = null;

        private Dictionary<object, ClientInfoModel> connectedClientList;

        private ConnectedClientHelper()
        {
            connectedClientList = new Dictionary<object, ClientInfoModel>();
        }

        public static ConnectedClientHelper GetInstance()
        {
            if(sInstance == null)
            {
                sInstance = new ConnectedClientHelper();
            }

            return sInstance;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="model"></param>
        public void AddClient(object identifier, Server.Model.ClientInfoModel model)
        {
            connectedClientList.Add(identifier, model);
        }

        public bool RemoveClient(object identifier)
        {
            return connectedClientList.Remove(identifier);
        }

        public void RemoveAllClients()
        {
            connectedClientList.Clear();
        }

        public ClientInfoModel GetClientInfo(object identifier)
        {
            ClientInfoModel model = null;
            connectedClientList.TryGetValue(identifier, out model);
            return model;
        }

        public int GetClientsCount()
        {
            return connectedClientList.Count;
        }

        public List<string> GetAllClientsSocketId()
        {
            List<string> socketList = new List<string>();
            foreach (ClientInfoModel model in connectedClientList.Values)
            {
                socketList.Add(model.SocketUserId);
            }

            return socketList;
        }
    }
}
