using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WcfServiceLibrary1
{
    [ServiceContract(SessionMode = SessionMode.Required, CallbackContract = typeof(IServiceCallback))]
    public interface IService1
    {
        [OperationContract(IsOneWay = true)]
        void RegisterCallback();

        [OperationContract]
        int AddUser(string name, string userName, string password, int groupId);

        [OperationContract]
        bool EditUser(int userId, string name, string userName, string password, int groupId);

        [OperationContract]
        bool RemoveUser(int userId);

        [OperationContract]
        List<UserData> GetAllUsers();

        [OperationContract]
        UserData GetUser(int userId);

        [OperationContract]
        int AddGroup(string groupName, bool shareDesktop, bool allowMaintenace, int monitorId, List<int> allowApps);

        [OperationContract]
        bool EditGroup(int groupId, string groupName, bool shareDesktop, bool allowMaintenace, int monitorId, List<int> appIds);

        [OperationContract]
        bool RemoveGroup(int groupId);

        [OperationContract]
        IList<GroupData> GetAllGroups();

        [OperationContract]
        GroupData GetGroupByUserId(int userId);

        [OperationContract]
        List<UserData> GetUsersInGroup(int groupId);

        [OperationContract]
        int AddApplication(string appName, string extraArguments, string exePath, int left, int top, int right, int bottom);

        [OperationContract]
        bool EditApplication(int appId, string appName, string exePath, string extraArguments, int left, int top, int right, int bottom);

        [OperationContract]
        bool RemoveApplication(int appId);

        [OperationContract]
        IList<ApplicationData> GetAllApplications();

        [OperationContract]
        List<ApplicationData> GetAppsWithGroupId(int groupId);

        [OperationContract]
        List<ApplicationData> GetAppsWithUserId(int userId);

        [OperationContract]
        int AddPreset(string presetName, int userId, List<int> appIds);

        [OperationContract(IsOneWay=true)]
        void RemovePreset(int presetId);
        
        [OperationContract(IsOneWay = true)]
        void EditPreset(int presetId, string presetName, int userId, List<int> appIds);

        [OperationContract]
        IList<PresetData> GetPresetByUserId(int userId);

        [OperationContract]
        int GetPresetIdByPresetNameUserId(string presetName, int userId);
        
        [OperationContract(IsOneWay = true)]
        void AddOrEditSetting(int portStart, int portEnd, int matrixCol, int matrixRow, string vncPath);
        
        [OperationContract(IsOneWay = true)]
        void RemoveSetting();

        [OperationContract]
        bool IsSettingAdded();

        [OperationContract]
        SettingData GetSetting();

        [OperationContract]
        List<MonitorData> GetMonitorsList();

        [OperationContract]
        bool AddMonitor(string name, int left, int top, int right, int bottom);

        [OperationContract]
        bool RemoveMonitor(int monitorId);

        [OperationContract]
        bool EditMonitor(int monitorId, string name, int left, int top, int right, int bottom);

        [OperationContract]
        MonitorData GetMonitorByGroupId(int groupId);

        [OperationContract]
        MonitorData GetMonitorDataByUserId(int userId);

        [OperationContract]
        List<RemoteVncData> GetRemoteVncList();

        [OperationContract]
        bool AddRemoteVnc(string name, string ipAdd, int port);

        [OperationContract]
        bool RemoveRemoteVnc(int dataId);

        [OperationContract]
        bool EditRemoteVnc(int dataId, string name, string ipAdd, int port);
    }

    [DataContract]
    public class RemoteVncData
    {
        [DataMember]
        public int id { get; set; }

        [DataMember]
        public string name { get; set; }

        [DataMember]
        public string remoteIp { get; set; }

        [DataMember]
        public int remotePort { get; set; }
    }

    [DataContract]
    public class UserData
    {
        [DataMember]
        public int id { get; set; }

        [DataMember]
        public string name { get; set; }

        [DataMember]
        public string username { get; set; }

        [DataMember]
        public string password { get; set; }

        [DataMember]
        public int group { get; set; }
    }

    [DataContract]
    public class GroupData
    {
        [DataMember]
        public int id { get; set; }

        [DataMember]
        public string name { get; set; }

        [DataMember]
        public bool share_full_desktop { get; set; }

        [DataMember]
        public bool allow_maintenance { get; set; }
    }

    [DataContract]
    public class ApplicationData
    {
        [DataMember]
        public int id { get; set; }

        [DataMember]
        public string name { get; set; }

        [DataMember]
        public string applicationPath { get; set; }

        [DataMember]
        public string arguments { get; set; }

        [DataMember]
        public WindowsRect rect { get; set; }
    }

    [DataContract]
    public class WindowsRect
    {
        [DataMember]
        public int Left { get; set; }

        [DataMember]
        public int Top { get; set; }

        [DataMember]
        public int Right { get; set; }

        [DataMember]
        public int Bottom { get; set; }
    }

    [DataContract]
    public class PresetData
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public List<ApplicationData> AppDataList { get; set; }
    }

    [DataContract]
    public class MonitorData
    {
        [DataMember]
        public int MonitorId { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public int Left { get; set; }

        [DataMember]
        public int Top { get; set; }

        [DataMember]
        public int Right { get; set; }

        [DataMember]
        public int Bottom { get; set; }
    }

    [DataContract]
    public class SettingData
    {
        [DataMember]
        public int PortStart { get; set; }

        [DataMember]
        public int PortEnd { get; set; }

        [DataMember]
        public int MatrixColumn { get; set; }

        [DataMember]
        public int MatrixRow { get; set; }

        [DataMember]
        public string VncPath { get; set; }
    }

    public interface IServiceCallback
    {
        [OperationContract(IsOneWay = true)]
        void OnUserDBChanged();
    }
}
