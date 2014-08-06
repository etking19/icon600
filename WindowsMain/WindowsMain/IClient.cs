﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WindowsMain.Client.Model;

namespace WindowsMain
{
    public interface IClient
    {
        // GUI

        /// <summary>
        /// GUI of server which client needs to follow
        /// </summary>
        /// <param name="user"></param>
        /// <param name="layout"></param>
        void RefreshLayout(UserInfoModel user, ServerLayoutModel layout);

        /// <summary>
        /// Allowed application list set by server during user management
        /// </summary>
        /// <param name="appList"></param>
        void RefreshAppList(IList<ApplicationModel> appList);

        /// <summary>
        /// Runtime application windows send by server and show as mimic window in client
        /// </summary>
        /// <param name="wndsList"></param>
        void RefreshWndList(IList<WindowsModel> wndsList);

        /// <summary>
        /// All connected user's vnc server info
        /// </summary>
        /// <param name="vncList"></param>
        void RefreshVncList(IList<VncModel> vncList);

        /// <summary>
        /// Presets stores/saved by user
        /// </summary>
        /// <param name="presetList"></param>
        void RefreshPresetList(IList<PresetModel> presetList);

        /// <summary>
        /// User priviledge setting by server during user management
        /// </summary>
        /// <param name="privilegde"></param>
        void RefreshMaintenanceStatus(UserPriviledgeModel privilegde);
    }
}
