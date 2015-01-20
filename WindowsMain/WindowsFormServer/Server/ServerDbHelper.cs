using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading;
using WcfServiceLibrary1;

namespace WindowsFormClient.Server
{
    public class ServerDbHelper
    {
        private static ServerDbHelper sInstance = null;
        private DuplexChannelFactory<IService1> dupFactory;
        private IService1 wcfService;
        private IService1Callback callbackHandler;
        private volatile bool _shouldStop = false;

        private ServerDbHelper()
        {

        }

        public static ServerDbHelper GetInstance()
        {
            if(sInstance == null)
            {
                sInstance = new ServerDbHelper();
            }

            return sInstance;
        }

        public bool Initialize(IService1Callback callbackHandler)
        {
            try
            {

                this.callbackHandler = callbackHandler;
                InstanceContext instanceContext = new InstanceContext(callbackHandler);
                EndpointAddress address = new EndpointAddress(new Uri(Properties.Settings.Default.RemoteIP));
                
                NetTcpBinding tcpBinding = new NetTcpBinding(SecurityMode.None);
                ReliableSessionBindingElement reliableBe = new ReliableSessionBindingElement();
                reliableBe.Ordered = true;
                tcpBinding.OpenTimeout = new TimeSpan(24, 20, 31, 23);
                tcpBinding.CloseTimeout = new TimeSpan(24, 20, 31, 23);
                tcpBinding.ReceiveTimeout = new TimeSpan(24, 20, 31, 23);
                tcpBinding.SendTimeout = new TimeSpan(24, 20, 31, 23);
                tcpBinding.MaxBufferSize = 2147483647;
                tcpBinding.MaxReceivedMessageSize = 2147483647;

                OptionalReliableSession reliableSession = new OptionalReliableSession(reliableBe);
                tcpBinding.ReliableSession = reliableSession;
                tcpBinding.ReceiveTimeout = new TimeSpan(24, 20, 31, 23);

                dupFactory = new DuplexChannelFactory<IService1>(instanceContext, tcpBinding, address);
                dupFactory.Open();

                wcfService = dupFactory.CreateChannel();
                wcfService.RegisterCallback();

                Thread workerThread = new Thread(DoWork);
                _shouldStop = true;
                workerThread.Start();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public void Initialize(IService1 wcfService)
        {
            this.wcfService = wcfService;
            Thread workerThread = new Thread(DoWork);
            _shouldStop = true;
            workerThread.Start();
        }

        private void keepAlive()
        {
            Thread workerThread = new Thread(DoWork);
            _shouldStop = true;
            workerThread.Start();
        }

        private void DoWork()
        {
            while (_shouldStop)
            {
                this.wcfService.KeepAlive();
                Thread.Sleep(1000);
            }
        }

        public void Shutdown()
        {
            try
            {
                _shouldStop = false;
                dupFactory.Close();
                wcfService = null;
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.Message);
            }
        }

        #region User
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="groupId"></param>
        /// <returns>id of the user added</returns>
        public int AddUser(string name, string userName, string password, int groupId)
        {
            return wcfService.AddUser(name, userName, password, groupId);
        }

        public bool EditUser(int userId, string name, string userName, string password, int groupId)
        {
            return wcfService.EditUser(userId, name, userName, password, groupId);
        }

        public bool RemoveUser(int userId)
        {
            return wcfService.RemoveUser(userId);
        }

        public UserData[] GetAllUsers()
        {
            return wcfService.GetAllUsers();
        }

        public UserData GetUser(int userId)
        {
            return wcfService.GetUser(userId);
        }

        #endregion

        #region group
        public int AddGroup(string groupName, bool shareDesktop, bool allowMaintenace, bool allowRemote, int monitorId, List<int> allowApps)
        {
            return wcfService.AddGroup(groupName, shareDesktop, allowMaintenace, allowRemote, monitorId, allowApps.ToArray());
        }

        public bool EditGroup(int groupId, string groupName, bool shareDesktop, bool allowMaintenace, bool allowRemote, int monitorId, List<int> appIds)
        {
            return wcfService.EditGroup(groupId, groupName, shareDesktop, allowMaintenace, allowRemote, monitorId, appIds.ToArray());
        }

        public bool RemoveGroup(int groupId)
        {
            return wcfService.RemoveGroup(groupId);
        }

        public IList<GroupData> GetAllGroups()
        {
            return wcfService.GetAllGroups();
        }

        public GroupData GetGroupByUserId(int userId)
        {
            return wcfService.GetGroupByUserId(userId);
        }

        public UserData[] GetUsersInGroup(int groupId)
        {
            return wcfService.GetUsersInGroup(groupId);
        }

        #endregion

        #region Applications

        public int AddApplication(string appName, string extraArguments, string exePath, int left, int top, int right, int bottom)
        {
            return wcfService.AddApplication(appName, extraArguments, exePath, left, top, right, bottom);
        }

        public bool EditApplication(int appId, string appName, string exePath, string extraArguments, int left, int top, int right, int bottom)
        {
            return wcfService.EditApplication(appId, appName, exePath, extraArguments, left, top, right, bottom);
        }

        public bool RemoveApplication(int appId)
        {
            return wcfService.RemoveApplication(appId);
        }

        public IList<ApplicationData> GetAllApplications()
        {
            return wcfService.GetAllApplications();
        }

        public ApplicationData[] GetAppsWithGroupId(int groupId)
        {
            return wcfService.GetAppsWithGroupId(groupId);
        }

        public ApplicationData[] GetAppsWithUserId(int userId)
        {
            return wcfService.GetAppsWithUserId(userId);
        }

        #endregion

        #region User preset

        /// <summary>
        /// 
        /// </summary>
        /// <param name="presetName"></param>
        /// <param name="userId"></param>
        /// <param name="appIds"></param>
        /// <returns>number of data added</returns>
        public void AddPreset(string presetName, int userId, List<KeyValuePair<int, WindowsRect>> appIds, List<KeyValuePair<int, WindowsRect>> vncIds, List<KeyValuePair<int, WindowsRect>> inputIds)
        {
            wcfService.AddPreset(presetName, userId, appIds.ToArray(), vncIds.ToArray(), inputIds.ToArray());
        }

        public void RemovePreset(int presetId)
        {
            wcfService.RemovePreset(presetId);
        }

        public void EditPreset(int presetId, string presetName, int userId, List<int> appIds, List<int> vncIds, List<int> inputIds)
        {
            wcfService.EditPreset(presetId, presetName, userId, appIds.ToArray(), vncIds.ToArray(), inputIds.ToArray());
        }

        public IList<PresetData> GetAllPreset()
        {
            return wcfService.GetAllPreset();
        }

        public IList<PresetData> GetPresetByUserId(int userId)
        {
            return wcfService.GetPresetByUserId(userId);
        }

        public int GetPresetIdByPresetNameUserId(string presetName, int userId)
        {
            return wcfService.GetPresetIdByPresetNameUserId(presetName, userId);
        }

        #endregion

        #region settings

        public void AddOrEditSetting(int portStart, int portEnd, int matrixCol, int matrixRow, string vncPath)
        {
            wcfService.AddOrEditSetting(portStart, portEnd, matrixCol, matrixRow, vncPath);
        }

        public void RemoveSetting()
        {
            wcfService.RemoveSetting();
        }

        public bool IsSettingAdded()
        {
            return wcfService.IsSettingAdded();
        }

        public SettingData GetSetting()
        {
            return wcfService.GetSetting();
        }

        #endregion

        #region Monitor


        public MonitorData[] GetMonitorsList()
        {
            return wcfService.GetMonitorsList();
        }

        public bool AddMonitor(string name, int left, int top, int right, int bottom)
        {
            return wcfService.AddMonitor(name, left, top, right, bottom);
        }

        public bool RemoveMonitor(int monitorId)
        {
            return wcfService.RemoveMonitor(monitorId);
        }

        public bool EditMonitor(int monitorId, string name, int left, int top, int right, int bottom)
        {
            return wcfService.EditMonitor(monitorId, name, left, top, right, bottom);
        }

        public MonitorData GetMonitorByGroupId(int groupId)
        {
            return wcfService.GetMonitorByGroupId(groupId);
        }

        public MonitorData GetMonitorDataByUserId(int userId)
        {
            return wcfService.GetMonitorDataByUserId(userId);
        }

        #endregion

        public RemoteVncData[] GetRemoteVncList()
        {
            return wcfService.GetRemoteVncList();
        }

        public bool AddRemoteVnc(string name, string ipAdd, int port)
        {
            return wcfService.AddRemoteVnc(name, ipAdd, port);
        }

        public bool RemoveRemoteVnc(int dataId)
        {
            return wcfService.RemoveRemoteVnc(dataId);
        }

        public bool EditRemoteVnc(int dataId, string name, string ipAdd, int port)
        {
            return wcfService.EditRemoteVnc(dataId, name, ipAdd, port);
        }

        /// <summary>
        /// string: window
        /// string: input
        /// string: osd
        /// </summary>
        /// <returns></returns>
        public VisionData[] GetAllVisionInputs()
        {
            return wcfService.GetAllVisionInputs();
        }

        public bool AddVisionInput(string window, string input, string osd)
        {
            return wcfService.AddVisionInput(window, input, osd);
        }

        public bool EditVisionInput(int id, string window, string input, string osd)
        {
            return wcfService.EditVisionData(id, window, input, osd);
        }

        public bool RemoveVisionInput(int id)
        {
            return wcfService.RemoveVisionInput(id);
        }

        public bool AddSystemInputCount(int count)
        {
            return wcfService.SetSystemSettingsInputCount(count);
        }

        public int GetSystemInputCount()
        {
            return wcfService.GetSystemSettingsInputCount();
        }

        public UserSettingData GetUserSetting(int userId)
        {
            return wcfService.GetUserSetting(userId);
        }

        public bool EditUserSetting(int userid, int gridX, int gridY, bool isSnap)
        {
            return wcfService.EditUserSetting(userid, gridX, gridY, isSnap);
        }
    }
}
