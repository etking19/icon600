using Session;
using Session.Connection;
using Session.Data;
using Session.Data.SubData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using WcfServiceLibrary1;

namespace WindowsFormClient.Server
{
    [CallbackBehavior(ConcurrencyMode = ConcurrencyMode.Single, UseSynchronizationContext = false)]
    class WcfCallbackHandler : IServiceCallback
    {
        private ConnectionManager connectionManager;

        public WcfCallbackHandler(ConnectionManager connectionMgr)
        {
            this.connectionManager = connectionMgr;
        }

        public void OnUserDBAdded(DBTypeEnum dbType, int dbIndex)
        {
            switch(dbType)
            {
                case DBTypeEnum.RemoteVnc:
                    sendVncUpdateToConnectedClients();
                    break;
            }
        }

        public void OnUserDBEditing(DBTypeEnum dbType, int dbIndex)
        {
            switch (dbType)
            {
                case DBTypeEnum.RemoteVnc:
                    sendVncUpdateToConnectedClients();
                    break;
            }
        }

        public void OnUserDBRemoving(DBTypeEnum dbType, int dbIndex)
        {
            switch (dbType)
            {
                case DBTypeEnum.RemoteVnc:
                    sendVncUpdateToConnectedClients();
                    break;
            }
        }

        private void sendVncUpdateToConnectedClients()
        {
            // notify all connected clients
            ServerVncStatus vncStatus = new ServerVncStatus();
            List<VncEntry> vncEntries = new List<VncEntry>();
            vncStatus.UserVncList = vncEntries;
            foreach (RemoteVncData vncData in Server.ServerDbHelper.GetInstance().GetRemoteVncList())
            {
                vncEntries.Add(new VncEntry()
                {
                    Identifier = vncData.id,
                    DisplayName = vncData.name,
                    IpAddress = vncData.remoteIp,
                    Port = vncData.remotePort,
                });
            }

            connectionManager.SendData(
                (int)CommandConst.MainCommandServer.UserPriviledge,
                (int)CommandConst.SubCommandServer.VncList,
                vncStatus,
                Server.ConnectedClientHelper.GetInstance().GetAllClientsSocketId());
        }
    }
}
