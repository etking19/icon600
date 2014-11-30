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

            model.LaunchedAppList.Add(mainWinId, dbAppIndex);
        }

        public void RemoveLaunchedApp(object identifier, int mainWinId)
        {
            ClientInfoModel model = null;
            connectedClientList.TryGetValue(identifier, out model);
            if (model == null)
            {
                return;
            }

            model.LaunchedAppList.Remove(mainWinId);
        }

        public void AddLaunchedVnc(object identifier, int mainWinId, int dbAppVnc)
        {
            ClientInfoModel model = null;
            connectedClientList.TryGetValue(identifier, out model);
            if (model == null)
            {
                return;
            }

            try
            {
                model.LaunchedVncList.Add(mainWinId, dbAppVnc);
            }
            catch
            {
            }
            
        }

        public void RemoveLaunchedVnc(object identifier, int mainWinId)
        {
            ClientInfoModel model = null;
            connectedClientList.TryGetValue(identifier, out model);
            if (model == null)
            {
                return;
            }

            try
            {
                model.LaunchedVncList.Remove(mainWinId);
            }
            catch
            {
            }
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="processId"></param>
        /// <param name="dbAppSource"></param>
        public void AddLaunchedInputSource(object identifier, uint processId, int dbAppSource)
        {
            ClientInfoModel model = null;
            connectedClientList.TryGetValue(identifier, out model);
            if (model == null)
            {
                return;
            }

            try
            {
                model.LaunchedSourceList.Add(processId, dbAppSource);
            }
            catch
            {
            }
            
        }

        public void RemoveLaunchedInputSource(object identifier, uint mainWinId)
        {
            ClientInfoModel model = null;
            connectedClientList.TryGetValue(identifier, out model);
            if (model == null)
            {
                return;
            }

            model.LaunchedSourceList.Remove(mainWinId);
        }

        public void UpdateLaunchedList(List<int> currentWndId)
        {
            foreach(ClientInfoModel model in connectedClientList.Values)
            {
                List<int> removedAppList = model.LaunchedAppList.Keys.Except(currentWndId).ToList();
                foreach (int wndId in removedAppList)
                {
                    model.LaunchedAppList.Remove(wndId);
                }

                List<int> removedVncList = model.LaunchedVncList.Keys.Except(currentWndId).ToList();
                foreach (int wndId in removedVncList)
                {
                    model.LaunchedVncList.Remove(wndId);
                }
            }
        }

        public void UpdateLaunchedSourceList(List<uint> currentProcessId)
        {
            foreach (ClientInfoModel model in connectedClientList.Values)
            {
                List<uint> removedSourceList = model.LaunchedSourceList.Keys.Except(currentProcessId).ToList();
                foreach (uint processId in removedSourceList)
                {
                    model.LaunchedSourceList.Remove(processId);
                }
            }
        }
    }
}
