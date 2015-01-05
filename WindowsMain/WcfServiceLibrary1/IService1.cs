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
        int AddGroup(string groupName, bool shareDesktop, bool allowMaintenace, bool allowRemote, int monitorId, List<int> allowApps);

        [OperationContract]
        bool EditGroup(int groupId, string groupName, bool shareDesktop, bool allowMaintenace, bool allowRemote, int monitorId, List<int> appIds);

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
        void AddPreset(string presetName, int userId, List<KeyValuePair<int, WindowsRect>> appIds, List<KeyValuePair<int, WindowsRect>> vncIds, List<KeyValuePair<int, WindowsRect>> inputIds);

        [OperationContract(IsOneWay=true)]
        void RemovePreset(int presetId);
        
        [OperationContract(IsOneWay = true)]
        void EditPreset(int presetId, string presetName, int userId, List<int> appIds, List<int> vncIds, List<int> inputIds);

        [OperationContract]
        IList<PresetData> GetPresetByUserId(int userId);

        [OperationContract]
        int GetPresetIdByPresetNameUserId(string presetName, int userId);

        [OperationContract]
        IList<PresetData> GetAllPreset();
        
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

        /// <summary>
        /// param int: id of the db table
        /// param string: window
        /// param string: input
        /// param string: osd
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<VisionData> GetAllVisionInputs();

        [OperationContract]
        bool AddVisionInput(string windowObj, string inputObj, string osdObj);

        [OperationContract]
        bool RemoveVisionInput(int id);

        [OperationContract]
        bool EditVisionData(int id, string windowObj, string inputObj, string osdObj);

        [OperationContract]
        int GetSystemSettingsInputCount();

        [OperationContract]
        bool SetSystemSettingsInputCount(int count);

        [OperationContract]
        UserSettingData GetUserSetting(int userId);

        [OperationContract]
        bool EditUserSetting(int userId, int gridX, int gridY, bool isSnap);

        [OperationContract]
        void KeepAlive();
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

        [DataMember]
        public WindowsRect rect { get; set; }
    }

    [DataContract]
    public class VisionData
    {
        [DataMember]
        public int id { get; set; }

        [DataMember]
        public string windowStr { get; set; }

        [DataMember]
        public string inputStr { get; set; }

        [DataMember]
        public string osdStr { get; set; }

        [DataMember]
        public WindowsRect rect { get; set; }
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

    public class UserSettingData
    {
        [DataMember]
        public int id { get; set; }

        [DataMember]
        public int userId { get; set; }

        [DataMember]
        public int gridX { get; set; }

        [DataMember]
        public int gridY { get; set; }

        [DataMember]
        public bool isSnap { get; set; }
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

        [DataMember]
        public bool allow_remote { get; set; }
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

        [DataMember]
        public List<RemoteVncData> VncDataList { get; set; }

        [DataMember]
        public List<VisionData> InputDataList { get; set; }
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

    [DataContract(Name = "DBType")]
    public enum DBTypeEnum
    {
        [EnumMember]
        Group = 1,
        [EnumMember]
        User = 2,
        [EnumMember]
        Application = 3,
        [EnumMember]
        Preset = 4,
        [EnumMember]
        VisionInput = 5,
        [EnumMember]
        RemoteVnc = 6,
        [EnumMember]
        Monitor = 7,
    }

    public interface IServiceCallback
    {
        [OperationContract(IsOneWay = true)]
        void OnUserDBAdded(DBTypeEnum dbType, int dbIndex);

        [OperationContract(IsOneWay = true)]
        void OnUserDBEditing(DBTypeEnum dbType, int dbIndex);

        [OperationContract(IsOneWay = true)]
        void OnUserDBEdited(DBTypeEnum dbType, int dbIndex);

        [OperationContract(IsOneWay = true)]
        void onUserDBRemoving(DBTypeEnum dbType, int dbIndex);

        [OperationContract(IsOneWay = true)]
        void onUserDBRemoved(DBTypeEnum dbType, int dbIndex);
    }
}
