using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormClient.Server
{
    class LaunchedWndHelper
    {
        private static LaunchedWndHelper sInstance = null;

        /// <summary>
        /// key: user DB id (unique)
        /// value: list of windows launched by this user in server session 
        /// (key - unique window's identifier, value - application DB id), db id cannot be key as user might launch 2 stored apps one time
        /// </summary>
        private Dictionary<int, Dictionary<int, List<int>>> mLaunchedAppMap = new Dictionary<int, Dictionary<int, List<int>>>();

        private LaunchedWndHelper()
        {
        }

        public static LaunchedWndHelper GetInstance()
        {
            if (sInstance == null)
            {
                sInstance = new LaunchedWndHelper();
            }

            return sInstance;
        }

        public void AddLaunchedApp(int userDBid, int windowUniqueId, int appDBid)
        {
            Dictionary<int, List<int>> launchedAppMap;
            List<int> launchAppList = null;
            if (false == mLaunchedAppMap.TryGetValue(userDBid, out launchedAppMap))
            {
                launchedAppMap = new Dictionary<int, List<int>>();
                mLaunchedAppMap.Add(userDBid, launchedAppMap);
            }

            if(false == launchedAppMap.TryGetValue(windowUniqueId, out launchAppList))
            {
                launchAppList = new List<int>();
                launchedAppMap.Add(windowUniqueId, launchAppList);
            }

            try
            {
                launchAppList.Add(appDBid);
            }
            catch (Exception e)
            {
                if (Properties.Settings.Default.Debug)
                {
                    MessageBox.Show("Unable to add this window to list: " + e.Message);
                }
            }
        }

        public bool RemoveLaunchedApp(int userDBid, int windowUniqueId)
        {
            Dictionary<int, List<int>> launchedAppMap;
            if (mLaunchedAppMap.TryGetValue(userDBid, out launchedAppMap))
            {
                return launchedAppMap.Remove(windowUniqueId);
            }

            return false;
        }

        /// <summary>
        /// key: unique window's identifier
        /// value: app DB id
        /// </summary>
        /// <param name="userDBid"></param>
        /// <returns></returns>
        public Dictionary<int, List<int>> GetLaunchedApps(int userDBid)
        {
            Dictionary<int, List<int>> launchedAppMap;
            if (false == mLaunchedAppMap.TryGetValue(userDBid, out launchedAppMap))
            {
                launchedAppMap = new Dictionary<int, List<int>>();
            }

            return new Dictionary<int, List<int>>(launchedAppMap);
        }

        public void ClearAll(int userDbId)
        {
            mLaunchedAppMap.Remove(userDbId);
        }

        public void Reset()
        {
            mLaunchedAppMap.Clear();
        }
    }
}
