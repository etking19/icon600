using Database;
using Database.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WcfServiceLibrary1
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Single)]
    public class Service1 : IService1
    {
        private const string DB_FOLDER = @"\Vistrol";
        private const string DB_NAME = @"\VistrolDB.sqlite";
        private List<IServiceCallback> callbackList = new List<IServiceCallback>();

        Service1()
        {
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData, Environment.SpecialFolderOption.Create);
            string folderPath = appDataPath + DB_FOLDER;

            bool result = false;

            // create the directory
            DirectoryInfo info = Directory.CreateDirectory(folderPath);

            // create the db
            if ((result = Database.DbHelper.GetInstance().Initialize(folderPath + DB_NAME)) == true)
            {
                Database.DbHelper.GetInstance().CreateTable(new Monitor());
                Database.DbHelper.GetInstance().CreateTable(new Group());
                Database.DbHelper.GetInstance().CreateTable(new GroupMonitor());
                Database.DbHelper.GetInstance().CreateTable(new User());
                Database.DbHelper.GetInstance().CreateTable(new Application());
                Database.DbHelper.GetInstance().CreateTable(new GroupApplications());
                Database.DbHelper.GetInstance().CreateTable(new PresetName());
                Database.DbHelper.GetInstance().CreateTable(new PresetApplications());
                Database.DbHelper.GetInstance().CreateTable(new PresetVnc());
                Database.DbHelper.GetInstance().CreateTable(new PresetVisionInput());
                Database.DbHelper.GetInstance().CreateTable(new Setting());
                Database.DbHelper.GetInstance().CreateTable(new RemoteVnc());
                Database.DbHelper.GetInstance().CreateTable(new VisionInput());
                Database.DbHelper.GetInstance().CreateTable(new SystemSettings());
            }
        }

        public void RegisterCallback()
        {
            callbackList.Add(OperationContext.Current.GetCallbackChannel<IServiceCallback>());
        }

        private void NotifyUserEditing(DBTypeEnum dbType, int dbIndex)
        {
            List<IServiceCallback> removeList = new List<IServiceCallback>();
            foreach(IServiceCallback callback in callbackList)
            {
                try
                {
                    callback.OnUserDBEditing(dbType, dbIndex);
                }
                catch (Exception)
                {
                    removeList.Add(callback);
                }
            }

            foreach (IServiceCallback callback in removeList)
            {
                callbackList.Remove(callback);
            }
        }

        private void NotifyUserEdited(DBTypeEnum dbType, int dbIndex)
        {
            List<IServiceCallback> removeList = new List<IServiceCallback>();
            foreach (IServiceCallback callback in callbackList)
            {
                try
                {
                    callback.OnUserDBEdited(dbType, dbIndex);
                }
                catch (Exception)
                {
                    removeList.Add(callback);
                }
            }

            foreach (IServiceCallback callback in removeList)
            {
                callbackList.Remove(callback);
            }
        }

        private void NotifyUserAdded(DBTypeEnum dbType, int dbIndex)
        {
            List<IServiceCallback> removeList = new List<IServiceCallback>();
            foreach (IServiceCallback callback in callbackList)
            {
                try
                {
                    callback.OnUserDBAdded(dbType, dbIndex);
                }
                catch (Exception)
                {
                    removeList.Add(callback);
                }
            }

            foreach (IServiceCallback callback in removeList)
            {
                callbackList.Remove(callback);
            }
        }

        private void NotifyUserRemoving(DBTypeEnum dbType, int dbIndex)
        {
            List<IServiceCallback> removeList = new List<IServiceCallback>();
            foreach (IServiceCallback callback in callbackList)
            {
                try
                {
                    callback.onUserDBRemoving(dbType, dbIndex);
                }
                catch (Exception)
                {
                    removeList.Add(callback);
                }
            }

            foreach (IServiceCallback callback in removeList)
            {
                callbackList.Remove(callback);
            }
        }

        private void NotifyUserRemoved(DBTypeEnum dbType, int dbIndex)
        {
            List<IServiceCallback> removeList = new List<IServiceCallback>();
            foreach (IServiceCallback callback in callbackList)
            {
                try
                {
                    callback.onUserDBRemoved(dbType, dbIndex);
                }
                catch (Exception)
                {
                    removeList.Add(callback);
                }
            }

            foreach (IServiceCallback callback in removeList)
            {
                callbackList.Remove(callback);
            }
        }
        
        public int AddUser(string name, string userName, string password, int groupId)
        {
            User dbUser = new User() { label = name, password = password, username = userName, group = groupId };
            int userId = -1;
            if (DbHelper.GetInstance().AddData(dbUser))
            {
                // get the added index number
                foreach (UserData data in GetAllUsers())
                {
                    if (data.username.CompareTo(userName) == 0)
                    {
                        userId = data.id;
                        break;
                    }
                }
            }

            NotifyUserAdded(DBTypeEnum.User, userId);
            return userId;
        }

        
        public bool EditUser(int userId, string name, string userName, string password, int groupId)
        {
            NotifyUserEditing(DBTypeEnum.User, userId);

            User dbUser = new User() { id = userId, label = name, password = password, username = userName, group = groupId };
            bool result = DbHelper.GetInstance().UpdateData(dbUser);

            NotifyUserEdited(DBTypeEnum.User, userId);
            return result;
        }

        
        public bool RemoveUser(int userId)
        {
            NotifyUserRemoving(DBTypeEnum.User, userId);

            User dbUser = new User() { id = userId };
            bool result = DbHelper.GetInstance().RemoveData(dbUser);

            NotifyUserRemoved(DBTypeEnum.User, userId);
            return result;
        }

        
        public List<UserData> GetAllUsers()
        {
            List<UserData> usersList = new List<UserData>();

            User dbUser = new User();
            DataTable dataTable = DbHelper.GetInstance().ReadData(dbUser);
            foreach (DataRow dataRow in dataTable.Rows)
            {
                UserData userData = new UserData()
                {
                    id = int.Parse(dataRow[User.USER_ID].ToString()),
                    name = dataRow[User.LABEL].ToString(),
                    username = dataRow[User.USERNAME].ToString(),
                    password = dataRow[User.PASSWORD].ToString(),
                    group = int.Parse(dataRow[User.GROUP_ID].ToString())
                };

                usersList.Add(userData);
            }

            return usersList;
        }

        
        public UserData GetUser(int userId)
        {
            UserData userData = new UserData();

            User dbUser = new User();
            DataTable dataTable = DbHelper.GetInstance().ReadData(dbUser);
            foreach (DataRow dataRow in dataTable.Rows)
            {
                if (int.Parse(dataRow[User.USER_ID].ToString()) == userId)
                {
                    userData.id = int.Parse(dataRow[User.USER_ID].ToString());
                    userData.name = dataRow[User.LABEL].ToString();
                    userData.username = dataRow[User.USERNAME].ToString();
                    userData.password = dataRow[User.PASSWORD].ToString();
                    userData.group = int.Parse(dataRow[User.GROUP_ID].ToString());

                    break;
                }
            }

            return userData;
        }

        
        public int AddGroup(string groupName, bool shareDesktop, bool allowMaintenace, int monitorId, List<int> allowApps)
        {
            int groupId = -1;

            Group dbGroup = new Group { label = groupName, share_full_desktop = shareDesktop, allow_maintenance = allowMaintenace };
            if (DbHelper.GetInstance().AddData(dbGroup))
            {
                // get the added index number
                foreach (GroupData data in GetAllGroups())
                {
                    if (data.name.CompareTo(groupName) == 0)
                    {
                        groupId = data.id;
                        break;
                    }
                }

                // add the application list to this group
                foreach (int appId in allowApps)
                {
                    GroupApplications groupApp = new GroupApplications()
                    {
                        application_id = appId,
                        group_id = groupId,
                    };

                    DbHelper.GetInstance().AddData(groupApp);
                }

                // add the monitor id to link with this group
                GroupMonitor groupMonitor = new GroupMonitor() { group_id = groupId, monitor_id = monitorId };
                DbHelper.GetInstance().AddData(groupMonitor);
            }

            NotifyUserAdded(DBTypeEnum.Group, groupId);
            return groupId;
        }

        
        public bool EditGroup(int groupId, string groupName, bool shareDesktop, bool allowMaintenace, int monitorId, List<int> appIds)
        {
            NotifyUserEditing(DBTypeEnum.Group, groupId);

            // modify the group info
            Group dbGroup = new Group { id = groupId, label = groupName, share_full_desktop = shareDesktop, allow_maintenance = allowMaintenace };
            bool result = DbHelper.GetInstance().UpdateData(dbGroup);

            // remove all from group-monitor
            GroupMonitor groupMonitor = new GroupMonitor() { group_id = groupId };
            DbHelper.GetInstance().RemoveData(groupMonitor);

            if (shareDesktop == false)
            {
                // add from group-monitor
                groupMonitor = new GroupMonitor() { group_id = groupId, monitor_id = monitorId };
                result &= DbHelper.GetInstance().AddData(groupMonitor);
            }

            // remove all from the group-app
            GroupApplications groupApps = new GroupApplications() { group_id = groupId };
            result &= DbHelper.GetInstance().RemoveData(groupApps);

            // add back the group-app
            foreach (int appId in appIds)
            {
                groupApps.application_id = appId;
                result &= DbHelper.GetInstance().AddData(groupApps);
            }

            NotifyUserEdited(DBTypeEnum.Group, groupId);
            return result;
        }

        
        public bool RemoveGroup(int groupId)
        {
            NotifyUserRemoving(DBTypeEnum.Group, groupId);

            Group dbGroup = new Group { id = groupId };
            bool result = DbHelper.GetInstance().RemoveData(dbGroup);

            NotifyUserRemoved(DBTypeEnum.Group, groupId);
            return result;
        }

        
        public IList<GroupData> GetAllGroups()
        {
            List<GroupData> groupsList = new List<GroupData>();

            Group dbGroup = new Group();
            DataTable dataTable = DbHelper.GetInstance().ReadData(dbGroup);
            foreach (DataRow dataRow in dataTable.Rows)
            {
                int groupId = int.Parse(dataRow[Group.GROUP_ID].ToString());

                GroupData groupData = new GroupData()
                {
                    id = groupId,
                    name = dataRow[Group.NAME].ToString(),
                    share_full_desktop = int.Parse(dataRow[Group.SHARE_FULL].ToString()) == 0 ? false : true,
                    allow_maintenance = int.Parse(dataRow[Group.MAINTENANCE].ToString()) == 0 ? false : true,
                };

                groupsList.Add(groupData);
            }

            return groupsList.AsReadOnly();
        }

        
        public GroupData GetGroupByUserId(int userId)
        {
            UserData userData = GetUser(userId);

            //appData = appsList.First(ApplicationData => ApplicationData.id == int.Parse(presetDataRow[PresetName.APPLICATION_ID].ToString()))
            GroupData groupData = GetAllGroups().First(GroupData => GroupData.id == userData.group);
            return groupData;
        }

        
        public List<UserData> GetUsersInGroup(int groupId)
        {
            return GetAllUsers().Where(userData => userData.group == groupId).ToList();
        }

        
        public int AddApplication(string appName, string extraArguments, string exePath, int left, int top, int right, int bottom)
        {
            Application dbApp = new Application()
            {
                label = appName,
                path = exePath,
                arguments = extraArguments,
                pos_left = left,
                pos_top = top,
                pos_right = right,
                pos_bottom = bottom,
            };

            int groupId = -1;
            if (DbHelper.GetInstance().AddData(dbApp))
            {
                foreach (ApplicationData data in GetAllApplications())
                {
                    if (data.name.CompareTo(appName) == 0)
                    {
                        groupId = data.id;
                    }
                }
            }

            NotifyUserAdded(DBTypeEnum.Application, groupId);
            return groupId;
        }

        
        public bool EditApplication(int appId, string appName, string exePath, string extraArguments, int left, int top, int right, int bottom)
        {
            NotifyUserEditing(DBTypeEnum.Application, appId);

            Application dbApp = new Application()
            {
                id = appId,
                label = appName,
                path = exePath,
                arguments = extraArguments,
                pos_left = left,
                pos_top = top,
                pos_right = right,
                pos_bottom = bottom,
            };

            bool result = DbHelper.GetInstance().UpdateData(dbApp);
            NotifyUserEdited(DBTypeEnum.Application, appId);
            
            return result;
        }

        
        public bool RemoveApplication(int appId)
        {
            NotifyUserRemoving(DBTypeEnum.Application, appId);

            Application dbApp = new Application()
            {
                id = appId
            };

            bool result = DbHelper.GetInstance().RemoveData(dbApp);
            NotifyUserRemoved(DBTypeEnum.Application, appId);

            return result;
        }

        
        public IList<ApplicationData> GetAllApplications()
        {
            List<ApplicationData> appsList = new List<ApplicationData>();

            Application dbApp = new Application();
            DataTable dataTable = DbHelper.GetInstance().ReadData(dbApp);
            foreach (DataRow dataRow in dataTable.Rows)
            {
                ApplicationData appData = new ApplicationData()
                {
                    id = int.Parse(dataRow[Application.APPLICATION_ID].ToString()),
                    name = dataRow[Application.LABEL].ToString(),
                    arguments = dataRow[Application.ARGUMENTS].ToString(),
                    applicationPath = dataRow[Application.PATH].ToString(),
                    rect = new WindowsRect
                    {
                        Left = int.Parse(dataRow[Application.SHOWING_LEFT].ToString()),
                        Top = int.Parse(dataRow[Application.SHOWING_TOP].ToString()),
                        Right = int.Parse(dataRow[Application.SHOWING_RIGHT].ToString()),
                        Bottom = int.Parse(dataRow[Application.SHOWING_BOTTOM].ToString()),
                    }
                };

                appsList.Add(appData);
            }

            return appsList.AsReadOnly();
        }

        
        public List<ApplicationData> GetAppsWithGroupId(int groupId)
        {
            List<ApplicationData> appsList = new List<ApplicationData>();

            // get all applications
            IList<ApplicationData> allAppsList = GetAllApplications();

            // get the group-application list
            GroupApplications groupApp = new GroupApplications() { group_id = groupId };
            DataTable groupAppDataTable = DbHelper.GetInstance().ReadData(groupApp);
            foreach (DataRow groupAppRow in groupAppDataTable.Rows)
            {
                int appId = int.Parse(groupAppRow[GroupApplications.APPLICATION_ID].ToString());
                foreach (ApplicationData data in allAppsList)
                {
                    if (data.id == appId)
                    {
                        appsList.Add(data);
                    }
                }
            }

            return appsList;
        }

        
        public List<ApplicationData> GetAppsWithUserId(int userId)
        {
            foreach (UserData userData in GetAllUsers())
            {
                if (userData.id == userId)
                {
                    return GetAppsWithGroupId(userData.group);
                }
            }

            return null;
        }


        public void AddPreset(string presetName, int userId, Dictionary<int, WindowsRect> appIds, Dictionary<int, WindowsRect> vncIds, Dictionary<int, WindowsRect> inputIds)
        {
            PresetName dbPreset = new PresetName()
            {
                preset_name = presetName,
                user_id = userId,
            };

            if (DbHelper.GetInstance().AddData(dbPreset))
            {
                // get the preset id just added
                int presetId = GetPresetIdByPresetNameUserId(presetName, userId);

                foreach (KeyValuePair<int, WindowsRect> appId in appIds)
                {
                    PresetApplications dbPresetApp = new PresetApplications()
                    {
                        preset_name_id = presetId,
                        preset_application_id = appId.Key,
                        app_latest_pos_left = appId.Value.Left,
                        app_latest_pos_top = appId.Value.Top,
                        app_latest_pos_right = appId.Value.Right,
                        app_latest_pos_bottom = appId.Value.Bottom,
                    };

                    DbHelper.GetInstance().AddData(dbPresetApp);
                }

                foreach (KeyValuePair<int, WindowsRect> vncId in vncIds)
                {
                    PresetVnc dbPresetVnc = new PresetVnc()
                    {
                        preset_name_id = presetId,
                        preset_vnc_id = vncId.Key,
                        vnc_latest_pos_left = vncId.Value.Left,
                        vnc_latest_pos_top = vncId.Value.Top,
                        vnc_latest_pos_right = vncId.Value.Right,
                        vnc_latest_pos_bottom = vncId.Value.Bottom,
                    };

                    DbHelper.GetInstance().AddData(dbPresetVnc);
                }

                foreach (KeyValuePair<int, WindowsRect> inputId in inputIds)
                {
                    PresetVisionInput dbPresetInput = new PresetVisionInput()
                    {
                        preset_name_id = presetId,
                        preset_vision_id = inputId.Key,
                        vision_latest_pos_left = inputId.Value.Left,
                        vision_latest_pos_top = inputId.Value.Top,
                        vision_latest_pos_right = inputId.Value.Right,
                        vision_latest_pos_bottom = inputId.Value.Bottom,
                    };

                    DbHelper.GetInstance().AddData(dbPresetInput);
                }
            }

            NotifyUserAdded(DBTypeEnum.Preset, -1);
        }

        
        public void RemovePreset(int presetId)
        {
            NotifyUserRemoving(DBTypeEnum.Preset, presetId);

            PresetName dbPreset = new PresetName()
            {
                preset_id = presetId,
            };

            DbHelper.GetInstance().RemoveData(dbPreset);

            NotifyUserRemoved(DBTypeEnum.Preset, presetId);
        }


        public void EditPreset(int presetId, string presetName, int userId, List<int> appIds, List<int> vncIds, List<int> inputIds)
        {
            NotifyUserEditing(DBTypeEnum.Preset, presetId);

            // update the possible name change
            PresetName dbPreset = new PresetName()
            {
                preset_id = presetId,
                preset_name = presetName,
                user_id = userId,
            };

            DbHelper.GetInstance().UpdateData(dbPreset);

            // update all the applications
            // 1. first delete existing applications with preset id - because we wont know if there is any app id deleted
            PresetApplications dbPresetApp = new PresetApplications()
            {
                preset_name_id = presetId,
            };
            DbHelper.GetInstance().RemoveData(dbPresetApp);

            // 2. add back all the application
            foreach (int appId in appIds)
            {
                PresetApplications dbPresetAppAdd = new PresetApplications()
                {
                    preset_name_id = presetId,
                    preset_application_id = appId,
                };
                DbHelper.GetInstance().AddData(dbPresetAppAdd);
            }

            PresetVnc dbPresetVncPre = new PresetVnc()
            {
                preset_name_id = presetId,
            };
            DbHelper.GetInstance().RemoveData(dbPresetVncPre);

            foreach (int vncId in vncIds)
            {
                PresetVnc dbPresetVnc = new PresetVnc()
                {
                    preset_name_id = presetId,
                    preset_vnc_id = vncId,
                };

                DbHelper.GetInstance().AddData(dbPresetVnc);
            }

            PresetVisionInput dbPresetInputPre = new PresetVisionInput()
            {
                preset_name_id = presetId,
            };
            DbHelper.GetInstance().RemoveData(dbPresetInputPre);

            foreach (int inputId in inputIds)
            {
                PresetVisionInput dbPresetInput = new PresetVisionInput()
                {
                    preset_name_id = presetId,
                    preset_vision_id = inputId,
                };

                DbHelper.GetInstance().AddData(dbPresetInput);
            }

            NotifyUserEdited(DBTypeEnum.Preset, presetId);
        }

        
        public IList<PresetData> GetPresetByUserId(int userId)
        {
            List<PresetData> presetList = new List<PresetData>();

            // get all applications
            IList<ApplicationData> applicationList = GetAllApplications();

            // get the preset name id list
            PresetName dbPresetName = new PresetName() { user_id = userId };
            DataTable dataTablePresetName = DbHelper.GetInstance().ReadData(dbPresetName);
            foreach (DataRow presetNameDataRow in dataTablePresetName.Rows)
            {
                // fill the preset name
                PresetData presetData = new PresetData();
                presetData.Name = presetNameDataRow[PresetName.PRESET_NAME].ToString();

                // preset name id
                presetData.Id = int.Parse(presetNameDataRow[PresetName.PRESET_ID].ToString());

                // get the application ids releated to this preset
                PresetApplications dbPresetApp = new PresetApplications()
                {
                    // preset id
                    preset_name_id = presetData.Id,
                };

                
                List<ApplicationData> presetAppList = new List<ApplicationData>();
                DataTable dataTablePresetApp = DbHelper.GetInstance().ReadData(dbPresetApp);
                foreach (DataRow presetAppDataRow in dataTablePresetApp.Rows)
                {
                    // get the application id from table
                    int appId = int.Parse(presetAppDataRow[PresetApplications.APPLICATION_ID].ToString());

                    // get the application data from app full list with match appId
                    ApplicationData appData = applicationList.First(ApplicationData => ApplicationData.id == appId);

                    // modify the data according to latest position
                    appData.rect.Left = int.Parse(presetAppDataRow[PresetApplications.APPLICATION_LATEST_LEFT].ToString());
                    appData.rect.Top = int.Parse(presetAppDataRow[PresetApplications.APPLICATION_LATEST_TOP].ToString());
                    appData.rect.Right = int.Parse(presetAppDataRow[PresetApplications.APPLICATION_LATEST_RIGHT].ToString());
                    appData.rect.Bottom = int.Parse(presetAppDataRow[PresetApplications.APPLICATION_LATEST_BOTTOM].ToString());

                    // add to the app list
                    presetAppList.Add(appData);
                }

                // get all the VNC
                IList<RemoteVncData> vncList = GetRemoteVncList();

                // get the vnc ids releated to this preset
                PresetVnc dbPresetVnc = new PresetVnc()
                {
                    // preset id
                    preset_name_id = presetData.Id,
                };


                List<RemoteVncData> presetVncList = new List<RemoteVncData>();
                DataTable dataTablePresetVnc = DbHelper.GetInstance().ReadData(dbPresetVnc);
                foreach (DataRow presetVncDataRow in dataTablePresetVnc.Rows)
                {
                    // get the application id from table
                    int vncId = int.Parse(presetVncDataRow[PresetVnc.VNC_ID].ToString());

                    // get the application data from app full list with match appId
                    RemoteVncData remoteVnc = vncList.First(RemoteVncData => RemoteVncData.id == vncId);

                    // modify based on latest input
                    remoteVnc.rect = new WindowsRect()
                    {
                        Left = int.Parse(presetVncDataRow[PresetVnc.VNC_LATEST_LEFT].ToString()),
                        Top = int.Parse(presetVncDataRow[PresetVnc.VNC_LATEST_TOP].ToString()),
                        Right = int.Parse(presetVncDataRow[PresetVnc.VNC_LATEST_RIGHT].ToString()),
                        Bottom = int.Parse(presetVncDataRow[PresetVnc.VNC_LATEST_BOTTOM].ToString()),
                    };

                    // add to the app list
                    presetVncList.Add(remoteVnc);
                }

                // get all the Vision input
                IList<VisionData> visionList = GetAllVisionInputs();

                // get the vnc ids releated to this preset
                PresetVisionInput dbPresetVision = new PresetVisionInput()
                {
                    // preset id
                    preset_name_id = presetData.Id,
                };


                List<VisionData> presetInputList = new List<VisionData>();
                DataTable dataTablePresetInput = DbHelper.GetInstance().ReadData(dbPresetVision);
                foreach (DataRow presetVisionDataRow in dataTablePresetInput.Rows)
                {
                    // get the application id from table
                    int visionId = int.Parse(presetVisionDataRow[PresetVisionInput.VISION_ID].ToString());

                    // get the application data from app full list with match appId
                    VisionData visionVnc = visionList.First(VisionData => VisionData.id == visionId);

                    // get the updated rect
                    visionVnc.rect = new WindowsRect()
                    {
                        Left = int.Parse(presetVisionDataRow[PresetVisionInput.VISION_LATEST_LEFT].ToString()),
                        Top = int.Parse(presetVisionDataRow[PresetVisionInput.VISION_LATEST_TOP].ToString()),
                        Right = int.Parse(presetVisionDataRow[PresetVisionInput.VISION_LATEST_RIGHT].ToString()),
                        Bottom = int.Parse(presetVisionDataRow[PresetVisionInput.VISION_LATEST_BOTTOM].ToString()),
                    };

                    // add to the app list
                    presetInputList.Add(visionVnc);
                }

                presetData.AppDataList = presetAppList;
                presetData.VncDataList = presetVncList;
                presetData.InputDataList = presetInputList;
                presetList.Add(presetData);
            }


            //// get the allowed list by a user
            //IList<ApplicationData> appsList = GetAppsWithUserId(userId);

            //PresetName dbPreset = new PresetName() {user_id = userId};
            //DataTable dataTable = DbHelper.GetInstance().ReadData(dbPreset);
            //foreach(DataRow presetDataRow in dataTable.Rows)
            //{
            //    PresetData presetData = new PresetData()
            //    {
            //        name = presetDataRow[PresetName.PRESET_NAME].ToString(),
            //        appData = appsList.First(ApplicationData => ApplicationData.id == int.Parse(presetDataRow[PresetName.APPLICATION_ID].ToString()))
            //    };

            //    presetList.Add(presetData);
            //}

            return presetList.AsReadOnly();
        }

        
        public int GetPresetIdByPresetNameUserId(string presetName, int userId)
        {
            int presetId = -1;

            PresetName dbPreset = new PresetName() { user_id = userId };
            DataTable dataTable = DbHelper.GetInstance().ReadData(dbPreset);
            foreach (DataRow presetDataRow in dataTable.Rows)
            {
                if (presetDataRow[PresetName.PRESET_NAME].ToString().CompareTo(presetName) == 0)
                {
                    presetId = int.Parse(presetDataRow[PresetName.PRESET_ID].ToString());
                    break;
                }
            }

            return presetId;
        }

        
        public void AddOrEditSetting(int portStart, int portEnd, int matrixCol, int matrixRow, string vncPath)
        {
            Setting dbSetting = new Setting()
            {
                port_start = portStart,
                port_end = portEnd,
                matrix_col = matrixCol,
                matrix_row = matrixRow,
                vnc_path = vncPath
            };

            DbHelper.GetInstance().AddData(dbSetting);
        }

        
        public void RemoveSetting()
        {
            DbHelper.GetInstance().RemoveData(new Setting());
        }

        
        public bool IsSettingAdded()
        {
            return (DbHelper.GetInstance().ReadData(new Setting()).Rows.Count != 0);
        }

        
        public SettingData GetSetting()
        {
            SettingData userSetting = new SettingData();

            DataTable dataTableSetting = DbHelper.GetInstance().ReadData(new Setting());
            foreach (DataRow settingDataRow in dataTableSetting.Rows)
            {
                userSetting.PortStart = int.Parse(settingDataRow[Setting.PORT_START].ToString());
                userSetting.PortEnd = int.Parse(settingDataRow[Setting.PORT_END].ToString());
                userSetting.MatrixRow = int.Parse(settingDataRow[Setting.ROW].ToString());
                userSetting.MatrixColumn = int.Parse(settingDataRow[Setting.COL].ToString());
                userSetting.VncPath = settingDataRow[Setting.VNC_PATH].ToString();
            }

            return userSetting;
        }

        
        public List<MonitorData> GetMonitorsList()
        {
            List<MonitorData> monitorList = new List<MonitorData>();

            DataTable dataTableMonitor = DbHelper.GetInstance().ReadData(new Monitor());
            foreach (DataRow monitorDataRow in dataTableMonitor.Rows)
            {
                MonitorData monitorData = new MonitorData()
                {
                    MonitorId = int.Parse(monitorDataRow[Monitor.MONITOR_ID].ToString()),
                    Name = monitorDataRow[Monitor.NAME].ToString(),
                    Left = int.Parse(monitorDataRow[Monitor.SHARE_LEFT].ToString()),
                    Top = int.Parse(monitorDataRow[Monitor.SHARE_TOP].ToString()),
                    Right = int.Parse(monitorDataRow[Monitor.SHARE_RIGHT].ToString()),
                    Bottom = int.Parse(monitorDataRow[Monitor.SHARE_BOTTOM].ToString()),
                };

                monitorList.Add(monitorData);
            }

            return monitorList;
        }

        
        public bool AddMonitor(string name, int left, int top, int right, int bottom)
        {
            Monitor dbMonitor = new Monitor()
            {
                label = name,
                monitor_left = left,
                monitor_top = top,
                monitor_right = right,
                monitor_bottom = bottom
            };

            bool result = DbHelper.GetInstance().AddData(dbMonitor);
            NotifyUserAdded(DBTypeEnum.Monitor, -1);

            return result;
        }

        
        public bool RemoveMonitor(int monitorId)
        {
            NotifyUserRemoving(DBTypeEnum.Monitor, monitorId);

            Monitor dbMonitor = new Monitor()
            {
                id = monitorId
            };

            bool result = DbHelper.GetInstance().RemoveData(dbMonitor);
            NotifyUserRemoved(DBTypeEnum.Monitor, monitorId);

            return result;
        }

        
        public bool EditMonitor(int monitorId, string name, int left, int top, int right, int bottom)
        {
            NotifyUserEditing(DBTypeEnum.Monitor, monitorId);

            Monitor dbMonitor = new Monitor()
            {
                id = monitorId,
                label = name,
                monitor_left = left,
                monitor_top = top,
                monitor_right = right,
                monitor_bottom = bottom
            };

            bool result = DbHelper.GetInstance().UpdateData(dbMonitor);
            NotifyUserEdited(DBTypeEnum.Monitor, monitorId);

            return result;
        }

        
        public MonitorData GetMonitorByGroupId(int groupId)
        {
            MonitorData monitorData = new MonitorData();

            // get the monitor from group-monitor table
            GroupMonitor groupMonitor = new GroupMonitor() { group_id = groupId };
            DataTable dbMonitorTable = DbHelper.GetInstance().ReadData(groupMonitor);

            int monitorId = -1;
            foreach (DataRow row in dbMonitorTable.Rows)
            {
                monitorId = int.Parse(row[GroupMonitor.MONITOR_ID].ToString());
            }
            monitorData.MonitorId = monitorId;

            if (monitorId != -1)
            {
                monitorData = GetMonitorsList().First(MonitorData => MonitorData.MonitorId == monitorId);
            }

            return monitorData;
        }

        
        public MonitorData GetMonitorDataByUserId(int userId)
        {
            MonitorData monitorData = new MonitorData();

            // get the monitor from group-monitor table
            GroupMonitor groupMonitor = new GroupMonitor() { group_id = GetGroupByUserId(userId).id };
            DataTable dbMonitorTable = DbHelper.GetInstance().ReadData(groupMonitor);

            int monitorId = -1;
            foreach (DataRow row in dbMonitorTable.Rows)
            {
                monitorId = int.Parse(row[GroupMonitor.MONITOR_ID].ToString());
            }

            if (monitorId != -1)
            {
                monitorData = GetMonitorsList().First(MonitorData => MonitorData.MonitorId == monitorId);
            }

            return monitorData;
        }

        
        public List<RemoteVncData> GetRemoteVncList()
        {
            List<RemoteVncData> remoteVncList = new List<RemoteVncData>();

            DataTable dataTableVnc = DbHelper.GetInstance().ReadData(new RemoteVnc());
            foreach (DataRow vncDataRow in dataTableVnc.Rows)
            {
                RemoteVncData vncData = new RemoteVncData()
                {
                    id = int.Parse(vncDataRow[RemoteVnc.REMOTEVNC_ID].ToString()),
                    name = vncDataRow[RemoteVnc.NAME].ToString(),
                    remoteIp = vncDataRow[RemoteVnc.REMOTE_IP].ToString(),
                    remotePort = int.Parse(vncDataRow[RemoteVnc.REMOTE_PORT].ToString()),
                };

                remoteVncList.Add(vncData);
            }

            return remoteVncList;
        }

        
        public bool AddRemoteVnc(string name, string ipAdd, int port)
        {
            RemoteVnc remoteVnc = new RemoteVnc()
            {
                name = name,
                remoteIp = ipAdd,
                remotePort = port
            };

            bool result = DbHelper.GetInstance().AddData(remoteVnc);
            NotifyUserAdded(DBTypeEnum.RemoteVnc, -1);

            return result;
        }

        
        public bool RemoveRemoteVnc(int dataId)
        {
            NotifyUserRemoving(DBTypeEnum.RemoteVnc, dataId);

            RemoteVnc remoteVnc = new RemoteVnc()
            {
                id = dataId
            };

            bool result = DbHelper.GetInstance().RemoveData(remoteVnc);
            NotifyUserRemoved(DBTypeEnum.RemoteVnc, dataId);

            return result;
        }

        
        public bool EditRemoteVnc(int dataId, string name, string ipAdd, int port)
        {
            NotifyUserEditing(DBTypeEnum.RemoteVnc, dataId);

            RemoteVnc remoteVnc = new RemoteVnc()
            {
                id = dataId,
                name = name,
                remoteIp = ipAdd,
                remotePort = port
            };

            bool result = DbHelper.GetInstance().UpdateData(remoteVnc);

            NotifyUserEdited(DBTypeEnum.RemoteVnc, dataId);

            return result;
        }

        
        public bool AddVisionInput(string windowObj, string inputObj, string osdObj)
        {
            VisionInput visionInput = new VisionInput()
            {
                Window = windowObj,
                Input = inputObj,
                OSD = osdObj,
            };

            bool result = DbHelper.GetInstance().AddData(visionInput);
            NotifyUserAdded(DBTypeEnum.VisionInput, -1);

            return result;
        }

        
        public bool RemoveVisionInput(int id)
        {
            NotifyUserRemoving(DBTypeEnum.VisionInput, id);

            VisionInput visionInput = new VisionInput()
            {
                Id = id,
            };

            bool result = DbHelper.GetInstance().RemoveData(visionInput);
            NotifyUserRemoved(DBTypeEnum.VisionInput, id);
            return result;
        }

        
        public bool EditVisionData(int id, string windowObj, string inputObj, string osdObj)
        {
            NotifyUserEditing(DBTypeEnum.VisionInput, id);

            VisionInput visionInput = new VisionInput()
            {
                Id = id,
                Window = windowObj,
                Input = inputObj,
                OSD = osdObj,
            };

            bool result = DbHelper.GetInstance().UpdateData(visionInput);
            NotifyUserEdited(DBTypeEnum.VisionInput, id);

            return result;
        }



        public List<VisionData> GetAllVisionInputs()
        {
            List<VisionData> result = new List<VisionData>();

            DataTable dataTableVision = DbHelper.GetInstance().ReadData(new VisionInput());
            foreach (DataRow visionDataRow in dataTableVision.Rows)
            {
                VisionData tuple = new VisionData()
                    {
                        id = int.Parse(visionDataRow[VisionInput.VISION_TABLE_ID].ToString()),
                        windowStr = visionDataRow[VisionInput.VISION_WINDOW].ToString(),
                        inputStr = visionDataRow[VisionInput.VISION_INPUT].ToString(),
                        osdStr = visionDataRow[VisionInput.VISION_OSD].ToString()
                    };
                result.Add(tuple);
            }

            return result;
        }


        public int GetSystemSettingsInputCount()
        {
            int inputCount = 0;
            DataTable dataTableSettings = DbHelper.GetInstance().ReadData(new SystemSettings());
            foreach (DataRow settingDataRow in dataTableSettings.Rows)
            {
                inputCount = int.Parse(settingDataRow[SystemSettings.INPUT].ToString());
            }

            return inputCount;
        }

        public bool SetSystemSettingsInputCount(int count)
        {
            return DbHelper.GetInstance().AddData(new SystemSettings()
                    {
                        InputCount = count,
                    });
        }
    }
}
