using Session;
using Session.Data;
using Session.Data.SubData;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Utils.Windows;
using WcfServiceLibrary1;

namespace WindowsFormClient.Command
{
    class ClientPresetCmdImpl : BaseImplementer
    {
        private int clientId;
        private IServer server;

        public ClientPresetCmdImpl(IServer server, int userId)
        {
            this.clientId = userId;
            this.server = server;
        }

        public override void ExecuteCommand(string userId, string command)
        {
            ClientPresetsCmd presetData = deserialize.Deserialize<ClientPresetsCmd>(command);
            if (presetData == null)
            {
                return;
            }

            bool broadcastChanges = false;
            switch (presetData.ControlType)
            {
                case ClientPresetsCmd.EControlType.Add:
                    AddPreset(clientId, presetData);
                    broadcastChanges = true;
                    break;
                case ClientPresetsCmd.EControlType.Delete:
                    RemovePreset(presetData);
                    broadcastChanges = true;
                    break;
                case ClientPresetsCmd.EControlType.Launch:
                    LaunchPreset(clientId, presetData);
                    break;
                case ClientPresetsCmd.EControlType.Modify:
                    ModifyPreset(clientId, presetData);
                    break;
                default:
                    break;
            }

            if (broadcastChanges)
            {
                // TODO: broadcast changes to user who login using the owner's account
                // get user's preset list
                ServerPresetsStatus serverPresetStatus = new ServerPresetsStatus();
                serverPresetStatus.UserPresetList = new List<PresetsEntry>();
                foreach (PresetData data in Server.ServerDbHelper.GetInstance().GetPresetByUserId(clientId))
                {
                    List<ApplicationEntry> presetAppEntries = new List<ApplicationEntry>();
                    foreach (ApplicationData appData in data.AppDataList)
                    {
                        presetAppEntries.Add(new ApplicationEntry()
                        {
                            Identifier = appData.id,
                            Name = appData.name
                        });
                    }
                    serverPresetStatus.UserPresetList.Add(new PresetsEntry()
                    {
                        Identifier = data.Id,
                        Name = data.Name,
                        ApplicationList = presetAppEntries
                    });
                }

                server.GetConnectionMgr().SendData(
                    (int)CommandConst.MainCommandServer.UserPriviledge,
                    (int)CommandConst.SubCommandServer.PresetList,
                    serverPresetStatus,
                    new List<string>() { userId });
            }
        }

        private void AddPreset(int userId, ClientPresetsCmd presetData)
        {
            // save preset to database
            List<int> applicationIds = new List<int>();
            foreach(ApplicationEntry entry in presetData.PresetEntry.ApplicationList)
            {
                applicationIds.Add(entry.Identifier);
            }

            Server.ServerDbHelper.GetInstance().AddPreset(
                presetData.PresetEntry.Name,
                userId,
                applicationIds);
        }

        private void RemovePreset(ClientPresetsCmd presetData)
        {
            // remove preset from database
            Server.ServerDbHelper.GetInstance().RemovePreset(presetData.PresetEntry.Identifier);
        }

        private void ModifyPreset(int userId, ClientPresetsCmd presetData)
        {
            List<int> applicationIds = new List<int>();
            foreach (ApplicationEntry entry in presetData.PresetEntry.ApplicationList)
            {
                applicationIds.Add(entry.Identifier);
            }

            Server.ServerDbHelper.GetInstance().EditPreset(presetData.PresetEntry.Identifier, presetData.PresetEntry.Name, userId, applicationIds);
        }

        private void LaunchPreset(int userId, ClientPresetsCmd presetData)
        {
            // 1. Close all existing running applications
            foreach(Utils.Windows.WindowsHelper.ApplicationInfo info in Utils.Windows.WindowsHelper.GetRunningApplicationInfo())
            {
                if (false == info.name.Contains("Vistrol Server"))
                {
                    Utils.Windows.NativeMethods.SendMessage(new IntPtr(info.id), Utils.Windows.Constant.WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
                }
            }

            // 2. trigger the apps in the preset by giving preset's id
            PresetData preset = Server.ServerDbHelper.GetInstance().GetPresetByUserId(userId).First(PresetData => PresetData.Id == presetData.PresetEntry.Identifier);
            foreach (ApplicationData appData in preset.AppDataList)
            {
                ProcessStartInfo info = new ProcessStartInfo()
                {
                    FileName = appData.applicationPath,
                    Arguments = appData.arguments
                };
                using(Process process = Process.Start(info))
                {
                    NativeMethods.SetWindowPos(new IntPtr(process.Id), Constant.HWND_TOP, appData.rect.Left, appData.rect.Top, 0, 0, (Int32)(Constant.SWP_NOSIZE));
                    NativeMethods.SetWindowPos(new IntPtr(process.Id), Constant.HWND_TOP, 0, 0, appData.rect.Right - appData.rect.Left, appData.rect.Bottom - appData.rect.Top, (Int32)Constant.SWP_NOMOVE);
                }
            }
        }
    }
}
