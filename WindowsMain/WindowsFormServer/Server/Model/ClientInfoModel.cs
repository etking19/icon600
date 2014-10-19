using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormClient.Server.Model
{
    public class ClientInfoModel
    {
        /// <summary>
        /// identifier from socket class
        /// </summary>
        public string SocketUserId { get; set; }

        /// <summary>
        /// primary key for user table
        /// </summary>
        public int DbUserId { get; set; }

        /// <summary>
        /// Display name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// key: main window handle int32
        /// value pair:
        ///  - key: db user id
        ///  - value: db index application
        /// </summary>
        public Dictionary<int, int> LaunchedAppList { get; set; }

        /// <summary>
        /// key: main window handle int32
        /// value pair:
        ///  - key: db user id
        ///  - value: db index Vnc
        /// </summary>
        public Dictionary<int, int> LaunchedVncList { get; set; }

        /// <summary>
        /// key: main window handle int32
        /// value pair:
        ///  - key: db user id
        ///  - value: db index VisionInput
        /// </summary>
        public Dictionary<uint, int> LaunchedSourceList { get; set; }
    }
}
