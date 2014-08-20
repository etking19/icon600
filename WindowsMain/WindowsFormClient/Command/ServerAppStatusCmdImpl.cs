using Session.Data;
using Session.Data.SubData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WindowsFormClient.Client.Model;
using WindowsFormClient.Settings;

namespace WindowsFormClient.Command
{
    class ServerAppStatusCmdImpl : BaseImplementer
    {
        private IClient client = null;
        public ServerAppStatusCmdImpl(IClient client)
        {
            this.client = client;
        }

        public override void ExecuteCommand(string userId, string command)
        {
            ServerApplicationStatus appStatusData = deserialize.Deserialize<ServerApplicationStatus>(command);
            if (appStatusData == null)
            {
                return;
            }

            List<ApplicationModel> appModelList = new List<ApplicationModel>();
            foreach (ApplicationEntry entry in appStatusData.UserApplicationList)
            {
                ApplicationModel model = new ApplicationModel()
                {
                    AppliationId = entry.Identifier,
                    ApplicationName = entry.Name,
                };

                appModelList.Add(model);
            }

            ApplicationSettings.GetInstance().ApplicationList = appStatusData.UserApplicationList;
            client.RefreshAppList(appModelList);
        }
    }
}
