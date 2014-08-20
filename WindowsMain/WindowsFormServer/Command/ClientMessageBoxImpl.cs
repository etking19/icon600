using Session.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormClient.Command
{
    class ClientMessageBoxImpl : BaseImplementer
    {
        private IServer server;
        public ClientMessageBoxImpl(IServer server)
        {
            this.server = server;
        }

        public override void ExecuteCommand(string userId, string command)
        {
            ClientMessageBoxCmd messageBoxData = deserialize.Deserialize<ClientMessageBoxCmd>(command);
            if (messageBoxData == null)
            {
                return;
            }

            this.server.AddMessageBox(
                messageBoxData.Message, 
                messageBoxData.TextFont, 
                messageBoxData.TextColor, 
                messageBoxData.Duration, 
                messageBoxData.Left, 
                messageBoxData.Top, 
                messageBoxData.Width, 
                messageBoxData.Height);
        }
    }
}
