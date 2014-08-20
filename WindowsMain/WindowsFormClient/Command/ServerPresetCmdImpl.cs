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
    class ServerPresetCmdImpl : BaseImplementer
    {
        private IClient client = null;

        public ServerPresetCmdImpl(IClient client)
        {
            this.client = client;
        }

        public override void ExecuteCommand(string userId, string command)
        {
            ServerPresetsStatus vncData = deserialize.Deserialize<ServerPresetsStatus>(command);
            if (vncData == null)
            {
                return;
            }

            List<PresetModel> presetList = new List<PresetModel>();
            foreach(PresetsEntry entry in vncData.UserPresetList)
            {
                List<ApplicationModel> appList = new List<ApplicationModel>();
                foreach (ApplicationEntry appEntry in entry.ApplicationList)
                {
                    ApplicationModel appModel = new ApplicationModel()
                    {
                        AppliationId = appEntry.Identifier,
                        ApplicationName = appEntry.Name,
                    };
                    appList.Add(appModel);
                }

                PresetModel model = new PresetModel()
                {
                    PresetId = entry.Identifier,
                    PresetName = entry.Name,
                    ApplicationList = appList,
                };

                presetList.Add(model);
            }

            ApplicationSettings.GetInstance().PresetList = vncData.UserPresetList;
            client.RefreshPresetList(presetList);
        }
    }
}
