using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Session.Data.SubData;
using WindowsFormClient.Client.Model;
using WindowsFormClient.Settings;
using Session.Data;

namespace WindowsFormClient.Command
{
    class ServerViewingAreaCmdImpl : BaseImplementer
    {
        private IClient client;

        public ServerViewingAreaCmdImpl(IClient client)
        {
            this.client = client;
        }

        public override void ExecuteCommand(string userId, string command)
        {
            ServerViewingAreaStatus viewingData = deserialize.Deserialize<ServerViewingAreaStatus>(command);
            if (viewingData == null)
            {
                return;
            }

            WindowsModel viewingArea = new WindowsModel()
            {
                PosLeft = viewingData.ViewingArea.LeftPos,
                PosTop = viewingData.ViewingArea.TopPos,
                Width = viewingData.ViewingArea.RightPos - viewingData.ViewingArea.LeftPos,
                Height = viewingData.ViewingArea.BottomPos - viewingData.ViewingArea.TopPos
            };

            // save to settings
            ServerSettings.GetInstance().ViewingAreaLeft = viewingArea.PosLeft;
            ServerSettings.GetInstance().ViewingAreaTop = viewingArea.PosTop;
            ServerSettings.GetInstance().ViewingAreaWidth = viewingArea.Width;
            ServerSettings.GetInstance().ViewingAreaHeight = viewingArea.Height;

            client.RefreshViewingArea(viewingArea);
        }
    }
}
