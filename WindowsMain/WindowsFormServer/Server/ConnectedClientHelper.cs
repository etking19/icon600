using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using WindowsFormClient.Server.Model;

namespace WindowsFormClient.Server
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

        public IList<ClientInfoModel> GetAllUsers()
        {
            return connectedClientList.Values.ToList().AsReadOnly();
        }

        public void ClearLaunchedData(object identifier)
        {
            ClientInfoModel model = null;
            connectedClientList.TryGetValue(identifier, out model);
            if (model == null)
            {
                Trace.WriteLine("ClearLaunchedData: no such user identifier");
                return;
            }

            model.LaunchedAppList.Clear();
            model.LaunchedSourceList.Clear();
            model.LaunchedVncList.Clear();
        }

        public void AddLaunchedApp(object identifier, int mainWinId, int dbAppIndex)
        {
            ClientInfoModel model = null;
            connectedClientList.TryGetValue(identifier, out model);
            if(model == null)
            {
                Trace.WriteLine("AddLaunchedApp: no such user identifier");
                return;
            }

            model.LaunchedAppList.Add(new KeyValuePair<int, int>(mainWinId, dbAppIndex));
        }

        public void RemoveLaunchedApp(object identifier, int mainWinId)
        {
            ClientInfoModel model = null;
            connectedClientList.TryGetValue(identifier, out model);
            if (model == null)
            {
                return;
            }

            var result = model.LaunchedAppList.FindAll(x => x.Key == mainWinId);
            foreach(var valuePair in result)
            {
                model.LaunchedAppList.Remove(valuePair);
            }
        }

        public void AddLaunchedVnc(object identifier, int mainWinId, int dbAppVnc)
        {
            ClientInfoModel model = null;
            connectedClientList.TryGetValue(identifier, out model);
            if (model == null)
            {
                return;
            }

            model.LaunchedVncList.Add(new KeyValuePair<int, int>(mainWinId, dbAppVnc));            
        }

        public void RemoveLaunchedVnc(object identifier, int mainWinId)
        {
            ClientInfoModel model = null;
            connectedClientList.TryGetValue(identifier, out model);
            if (model == null)
            {
                return;
            }

            var result = model.LaunchedVncList.FindAll(x => x.Key == mainWinId);
            foreach (var valuePair in result)
            {
                model.LaunchedVncList.Remove(valuePair);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="processId"></param>
        /// <param name="dbAppSource"></param>
        public void AddLaunchedInputSource(object identifier, int processId, int dbAppSource)
        {
            ClientInfoModel model = null;
            connectedClientList.TryGetValue(identifier, out model);
            if (model == null)
            {
                return;
            }

            model.LaunchedSourceList.Add(new KeyValuePair<int, int>(processId, dbAppSource));
        }

        public void RemoveLaunchedInputSource(object identifier, int mainWinId)
        {
            ClientInfoModel model = null;
            connectedClientList.TryGetValue(identifier, out model);
            if (model == null)
            {
                return;
            }

            var result = model.LaunchedSourceList.FindAll(x => x.Key == mainWinId);
            foreach (var valuePair in result)
            {
                model.LaunchedSourceList.Remove(valuePair);
            }
        }

        public void UpdateLaunchedList(List<int> currentWndId)
        {
            /*
            foreach(ClientInfoModel model in connectedClientList.Values)
            {
                var result = model.LaunchedAppList
                    .Where(x => currentWndId.Contains(x.Key)).ToList();
                model.LaunchedAppList = result;

                var resultVnc = model.LaunchedVncList
                    .Where(x => currentWndId.Contains(x.Key)).ToList();
                model.LaunchedVncList = resultVnc;

                var resultSources = model.LaunchedSourceList
                    .Where(x => currentWndId.Contains(x.Key)).ToList();
                model.LaunchedSourceList = resultSources;
            }
             */
        }
    }
}
