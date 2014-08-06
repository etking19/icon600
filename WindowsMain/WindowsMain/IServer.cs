using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WindowsMain.Server.Model;

namespace WindowsMain
{
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
    }
}
