using Session.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormClient.Command
{
    public interface ICmdImplementer
    {
        /// <summary>
        /// execute command string received
        /// </summary>
        /// <param name="userId">specify client's identifier when server receive command, server will have value "unspecify" when client receive this</param>
        /// <param name="command">command string</param>
        void ExecuteCommand(string userId, string command);
    }
}
