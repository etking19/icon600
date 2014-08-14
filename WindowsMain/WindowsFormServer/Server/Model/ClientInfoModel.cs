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
        /// Monitor info, used in vnc connection
        /// </summary>
        public IList<MonitorModel> MonitorsInfo { get; set; }

        /// <summary>
        /// Vnc info
        /// </summary>
        public VncModel VncInfo { get; set; }
    }
}
