using Session.Data;
using Session.Data.SubData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WindowsFormClient.Client.Model;

namespace WindowsFormClient.Command
{
    class ServerWndsAttrCmdImpl : BaseImplementer
    {
        private IClient client = null;

        public ServerWndsAttrCmdImpl(IClient client)
        {
            this.client = client;
        }

        public override void ExecuteCommand(string userId, string command)
        {
            ServerWindowsPos wndData = deserialize.Deserialize<ServerWindowsPos>(command);
            if (wndData == null)
            {
                return;
            }

            List<WindowsModel> wndsModelList = new List<WindowsModel>();
            foreach(WndPos wndPos in wndData.WindowsAttributes)
            {
                WindowsModel model = new WindowsModel()
                {
                    WindowsId = wndPos.id,
                    DisplayName = wndPos.name,
                    PosLeft = wndPos.posX,
                    PosTop = wndPos.posY,
                    Width = wndPos.width,
                    Height = wndPos.height,
                    Style = wndPos.style,
                    ZOrder = wndPos.ZOrder
                };

                wndsModelList.Add(model);
            }

            client.RefreshWndList(wndsModelList);
        }
    }
}
