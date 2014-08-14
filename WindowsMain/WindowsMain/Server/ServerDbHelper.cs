using Database;
using Database.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace WindowsFormServer.Server
{
    public class ServerDbHelper
    {
        private static ServerDbHelper sInstance = null;

        private const string DB_FOLDER = @"\Vostrol";
        private const string DB_NAME = @"\VostrolDB.sqlite";

        public struct UserData
        {
            public int id { get; set; }
            public string name { get; set; }
            public string username { get; set; }
            public string password { get; set; }
            public int group { get; set; }
        }

        public struct GroupData
        {
            public int id { get; set; }
            public string name { get; set; }
            public bool share_full_desktop { get; set; }
            public Utils.Windows.NativeMethods.Rect rect { get; set; } 
            public bool allow_maintenance { get; set; }
        }

        public struct ApplicationData
        {
            public int id { get; set; }
            public string name { get; set; }
            public string applicationPath { get; set; }
            public string arguments { get; set; }
            public Utils.Windows.NativeMethods.Rect rect { get; set; } 
        }

        public struct PresetData
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public List<ApplicationData> AppDataList { get; set; }
        }

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

        public bool Initialize()
        {
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData, Environment.SpecialFolderOption.Create);
            string folderPath = appDataPath + DB_FOLDER;
            
            bool result = false;

            // create the directory
            DirectoryInfo info = Directory.CreateDirectory(folderPath);

            // create the db
            if((result = Database.DbHelper.GetInstance().Initialize(folderPath + DB_NAME)) == true)
            {
                Database.DbHelper.GetInstance().CreateTable(new Group());
                Database.DbHelper.GetInstance().CreateTable(new User());
                Database.DbHelper.GetInstance().CreateTable(new Application());
                Database.DbHelper.GetInstance().CreateTable(new GroupApplications());
                Database.DbHelper.GetInstance().CreateTable(new PresetName());
            }

            return result;
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
            // add the new user to database
            User dbUser = new User() { label = name, password = password, username = userName, group = groupId };
            if(DbHelper.GetInstance().AddData(dbUser))
            {
                // get the added index number
                foreach (UserData data in GetAllUsers())
                {
                    if (data.username.CompareTo(userName) == 0)
                    {
                        return data.id;
                    }
                }
            }

            return -1;
        }

        public bool EditUser(int userId, string name, string userName, string password, int groupId)
        {
            User dbUser = new User() { id = userId, label = name, password = password, username = userName, group = groupId };
            return DbHelper.GetInstance().UpdateData(dbUser);
        }

        public bool RemoveUser(int userId)
        {
            User dbUser = new User() { id = userId };
            return DbHelper.GetInstance().RemoveData(dbUser);
        }

        public IList<UserData> GetAllUsers()
        {
            List<UserData> usersList = new List<UserData>();

            User dbUser = new User();
            DataTable dataTable = DbHelper.GetInstance().ReadData(dbUser);
            foreach(DataRow dataRow in dataTable.Rows)
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

            return usersList.AsReadOnly();
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

        #endregion

        #region group
        public int AddGroup(string groupName, bool shareDesktop, bool allowMaintenace, int left, int top, int right, int bottom, List<int> allowApps)
        {
            int groupId = -1;

            Group dbGroup = new Group { label = groupName, share_full_desktop = shareDesktop, allow_maintenance = allowMaintenace, monitor_left = left, monitor_top=top, monitor_right=right, monitor_bottom = bottom};
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
            }

            return groupId;
        }

        public bool EditGroup(int groupId, string groupName, bool shareDesktop, bool allowMaintenace, int left, int top, int right, int bottom)
        {
            Group dbGroup = new Group { id=groupId, label = groupName, share_full_desktop = shareDesktop, allow_maintenance = allowMaintenace, monitor_left = left, monitor_top = top, monitor_right = right, monitor_bottom = bottom };
            return DbHelper.GetInstance().UpdateData(dbGroup);
        }

        public bool RemoveGroup(int groupId)
        {
            Group dbGroup = new Group { id = groupId };
            return DbHelper.GetInstance().RemoveData(dbGroup);
        }

        public IList<GroupData> GetAllGroups()
        {
            List<GroupData> groupsList = new List<GroupData>();

            Group dbGroup = new Group();
            DataTable dataTable = DbHelper.GetInstance().ReadData(dbGroup);
            foreach (DataRow dataRow in dataTable.Rows)
            {
                GroupData groupData = new GroupData()
                {
                    id = int.Parse(dataRow[Group.GROUP_ID].ToString()),
                    name = dataRow[Group.NAME].ToString(),
                    share_full_desktop = int.Parse(dataRow[Group.SHARE_FULL].ToString()) == 0 ? false:true,
                    allow_maintenance = int.Parse(dataRow[Group.MAINTENANCE].ToString()) == 0 ? false:true,
                    rect = new Utils.Windows.NativeMethods.Rect
                    {
                        Left = int.Parse(dataRow[Group.SHARE_LEFT].ToString()),
                        Top = int.Parse(dataRow[Group.SHARE_TOP].ToString()),
                        Right = int.Parse(dataRow[Group.SHARE_RIGHT].ToString()),
                        Bottom = int.Parse(dataRow[Group.SHARE_BOTTOM].ToString()),
                    }
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

        #endregion

        #region Applications

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

            if(DbHelper.GetInstance().AddData(dbApp))
            {
                foreach (ApplicationData data in GetAllApplications())
                {
                    if(data.name.CompareTo(appName) == 0)
                    {
                        return data.id;
                    }
                }
            }

            return -1;
        }

        public bool EditApplication(int appId, string appName, string extraArguments, string exePath, int left, int top, int right, int bottom)
        {
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

            return DbHelper.GetInstance().UpdateData(dbApp);
        }

        public bool RemoveApplication(int appId)
        {
            Application dbApp = new Application()
            {
                id = appId
            };

            return DbHelper.GetInstance().RemoveData(dbApp);
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
                    rect = new Utils.Windows.NativeMethods.Rect
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

        public IList<ApplicationData> GetAppsWithGroupId(int groupId)
        {
            List<ApplicationData> appsList = new List<ApplicationData>();

            // get all applications
            IList<ApplicationData> allAppsList = GetAllApplications();

            // get the group-application list
            GroupApplications groupApp = new GroupApplications() { group_id = groupId};
            DataTable groupAppDataTable = DbHelper.GetInstance().ReadData(groupApp);
            foreach (DataRow groupAppRow in groupAppDataTable.Rows)
            {
                int appId = int.Parse(groupAppRow[GroupApplications.GROUP_ID].ToString());
                foreach (ApplicationData data in allAppsList)
                {
                    if(data.id == appId)
                    {
                        appsList.Add(data);
                        break;
                    }
                }
            }

            return appsList.AsReadOnly();
        }

        public IList<ApplicationData> GetAppsWithUserId(int userId)
        {
            foreach(UserData userData in GetAllUsers())
            {
                if(userData.id == userId)
                {
                    return GetAppsWithGroupId(userData.group);
                }
            }

            return null;
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
        public int AddPreset(string presetName, int userId, List<int> appIds)
        {
            int count = 0;
            PresetName dbPreset = new PresetName()
            {
                preset_name = presetName,
                user_id = userId,
            };

            if (DbHelper.GetInstance().AddData(dbPreset))
            {
                // get the preset id just added
                int presetId = GetPresetIdByPresetNameUserId(presetName, userId);

                foreach (int appId in appIds)
                {
                    PresetApplications dbPresetApp = new PresetApplications()
                    {
                        preset_name_id = presetId,
                        preset_application_id = appId,
                    };

                    if (DbHelper.GetInstance().AddData(dbPresetApp))
                    {
                        count++;
                    }
                }
            }
            

            return count;
        }

        public void RemovePreset(int presetId)
        {
            PresetName dbPreset = new PresetName()
            {
                preset_id = presetId,
            };

            DbHelper.GetInstance().RemoveData(dbPreset);
        }

        public void EditPreset(int presetId, string presetName, int userId, List<int> appIds)
        {
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
            foreach(int appId in appIds)
            {
                PresetApplications dbPresetAppAdd = new PresetApplications()
                {
                    preset_name_id = presetId,
                    preset_application_id = appId,
                };
                DbHelper.GetInstance().AddData(dbPresetAppAdd);
            }
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
                List<ApplicationData> presetAppList = new List<ApplicationData>();
                PresetApplications dbPresetApp = new PresetApplications() 
                { 
                    // preset id
                    preset_name_id = presetData.Id,
                };
                DataTable dataTablePresetApp = DbHelper.GetInstance().ReadData(dbPresetApp);
                foreach (DataRow presetAppDataRow in dataTablePresetApp.Rows)
                {
                    // get the application id from table
                    int appId = int.Parse(presetAppDataRow[PresetApplications.APPLICATION_ID].ToString());

                    // get the application data from app full list with match appId
                    ApplicationData appData = applicationList.First(ApplicationData => ApplicationData.id == appId);

                    // add to the app list
                    presetAppList.Add(appData);
                }

                presetData.AppDataList = presetAppList;
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

            PresetName dbPreset = new PresetName() {user_id = userId};
            DataTable dataTable = DbHelper.GetInstance().ReadData(dbPreset);
            foreach(DataRow presetDataRow in dataTable.Rows)
            {
                if (presetDataRow[PresetName.PRESET_NAME].ToString().CompareTo(presetName) == 0)
                {
                    presetId = int.Parse(presetDataRow[PresetName.PRESET_ID].ToString());
                    break;
                }
            }

            return presetId;
        }

        #endregion

        #region settings

        public void AddOrEditSetting(int portStart, int portEnd, int matrixCol, int matrixRow)
        {
            Setting dbSetting = new Setting()
            {
                port_start = portStart,
                port_end = portEnd,
                matrix_col = matrixCol,
                matrix_row = matrixRow,
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

        #endregion
    }
}
