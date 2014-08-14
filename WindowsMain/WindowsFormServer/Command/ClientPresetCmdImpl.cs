using Session.Data;
using Session.Data.SubData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormClient.Command
{
    class ClientPresetCmdImpl : BaseImplementer
    {
        private int clientId;
        public ClientPresetCmdImpl(int userId)
        {
            this.clientId = userId;
        }

        public override void ExecuteCommand(string userId, string command)
        {
            ClientPresetsControl presetData = deserialize.Deserialize<ClientPresetsControl>(command);
            if (presetData == null)
            {
                return;
            }

            bool broadcastChanges = false;
            switch (presetData.ControlType)
            {
                case ClientPresetsControl.EControlType.Add:
                    AddPreset(clientId, presetData);
                    broadcastChanges = true;
                    break;
                case ClientPresetsControl.EControlType.Delete:
                    RemovePreset(presetData);
                    broadcastChanges = true;
                    break;
                case ClientPresetsControl.EControlType.Launch:
                    LaunchPreset(presetData);
                    break;
                case ClientPresetsControl.EControlType.Modify:
                    ModifyPreset(clientId, presetData);
                    break;
                default:
                    break;
            }

            if (broadcastChanges)
            {
                // TODO: broadcast changes to user who login using the owner's account
            }
        }

        private void AddPreset(int userId, ClientPresetsControl presetData)
        {
            // save preset to database
            List<int> applicationIds = new List<int>();
            foreach(ApplicationEntry entry in presetData.ApplicationList)
            {
                applicationIds.Add(entry.Identifier);
            }

            Server.ServerDbHelper.GetInstance().AddPreset(
                presetData.PresetEntry.Name,
                userId,
                applicationIds);
        }

        private void RemovePreset(ClientPresetsControl presetData)
        {
            // remove preset from database
            Server.ServerDbHelper.GetInstance().RemovePreset(presetData.PresetEntry.Identifier);
        }

        private void ModifyPreset(int userId, ClientPresetsControl presetData)
        {
            List<int> applicationIds = new List<int>();
            foreach (ApplicationEntry entry in presetData.ApplicationList)
            {
                applicationIds.Add(entry.Identifier);
            }

            Server.ServerDbHelper.GetInstance().EditPreset(presetData.PresetEntry.Identifier, presetData.PresetEntry.Name, userId, applicationIds);
        }

        private void LaunchPreset(ClientPresetsControl presetData)
        {
            // TODO:
            // 1. Close all existing running applications
            // 2. trigger the apps in the preset by giving preset's id
        }
    }
}
