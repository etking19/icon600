using Session.Connection;
using Session.Data.SubData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WindowsFormClient.Client.Model;

namespace WindowsFormClient
{
    public interface IClient
    {
        /// <summary>
        /// GUI of server which client needs to follow
        /// </summary>
        /// <param name="user"></param>
        /// <param name="layout"></param>
        void RefreshLayout(UserInfoModel user, ServerLayoutModel layout, WindowsModel viewingArea);

        /// <summary>
        /// Allowed viewing area changed
        /// </summary>
        /// <param name="viewingArea"></param>
        void RefreshViewingArea(WindowsModel viewingArea);

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

        /// <summary>
        /// Vision input list updated by server
        /// </summary>
        /// <param name="inputAttrList"></param>
        void RefreshVisionInputStatus(List<InputAttributes> inputAttrList);

        void CloseApplication();
    }
}
