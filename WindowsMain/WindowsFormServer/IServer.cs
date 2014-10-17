using Session.Connection;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WcfServiceLibrary1;
using WindowsFormClient.Server.Model;

namespace WindowsFormClient
{
    public enum ServerCommandType
    {
        Added,
        Edited,
        Removed,
    }

    public interface IServer
    {
        /// <summary>
        /// Notify client login details
        /// </summary>
        /// <param name="model"></param>
        void ClientLogin(ClientInfoModel model);

        /// <summary>
        /// get the current connected client's vnc details
        /// </summary>
        /// <returns></returns>
        VncMarshall.Client GetVncClient();

        /// <summary>
        /// get the client info from userId passed in by socket connection
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        ClientInfoModel GetClientInfo(string userId);

        ConnectionManager GetConnectionMgr();

        void AddMessageBox(string message, Font font, Color color, Color backgroundColor, int duration, int left, int top, int width, int height, bool animation);

        void OnGridDataUpdateRequest(ServerCommandType command, DBTypeEnum dbType);
        
    }
}
