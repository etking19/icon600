﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        private Dictionary<int, Dictionary<int, int>> mLaunchedAppMap = new Dictionary<int, Dictionary<int, int>>();

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
            Dictionary<int, int> launchedAppMap;
            if (false == mLaunchedAppMap.TryGetValue(userDBid, out launchedAppMap))
            {
                launchedAppMap = new Dictionary<int, int>();
                mLaunchedAppMap.Add(userDBid, launchedAppMap);
            }

            launchedAppMap.Add(windowUniqueId, appDBid);
        }

        public bool RemoveLaunchedApp(int userDBid, int windowUniqueId)
        {
            Dictionary<int, int> launchedAppMap;
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
        public Dictionary<int, int> GetLaunchedApps(int userDBid)
        {
            Dictionary<int, int> launchedAppMap;
            if (false == mLaunchedAppMap.TryGetValue(userDBid, out launchedAppMap))
            {
                launchedAppMap = new Dictionary<int, int>();
            }

            return new Dictionary<int, int>(launchedAppMap);
        }

        public void ClearAll()
        {
            mLaunchedAppMap.Clear();
        }
    }
}