using Session.Data;
using Session.Data.SubData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WindowsFormClient.Client.Model;

namespace WindowsFormClient.Command
{
    class ServerVncStatusCmdImpl : BaseImplementer
    {
        private IClient client = null;

        public ServerVncStatusCmdImpl(IClient client)
        {
            this.client = client;
        }

        public override void ExecuteCommand(string userId, string command)
        {
            ServerVncStatus vncData = deserialize.Deserialize<ServerVncStatus>(command);
            if (vncData == null)
            {
                return;
            }

            List<VncModel> vncList = new List<VncModel>();
            foreach(VncEntry entry in vncData.UserVncList)
            {
                VncModel model = new VncModel()
                {
                    DisplayName = entry.DisplayName,
                    VncServerIp = entry.IpAddress,
                    VncServerPort = entry.Port
                };

                vncList.Add(model);
            }

            client.RefreshVncList(vncList);
        }
    }
}
