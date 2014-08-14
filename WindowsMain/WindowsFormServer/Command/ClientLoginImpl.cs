using Database.Data;
using Session.Data;
using Session.Data.SubData;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using WindowsFormClient.Server.Model;

namespace WindowsFormClient.Command
{
    class ClientLoginImpl : BaseImplementer
    {
        private IServer server = null;
        public ClientLoginImpl(IServer server)
        {
            this.server = server;
        }

        public override void ExecuteCommand(string userId, string command)
        {
            ClientLoginCmd data = deserialize.Deserialize<ClientLoginCmd>(command);
            if (data == null)
            {
                return;
            }

            // get the login status matches with database
            string displayName = string.Empty;
            int dbUserId = -1;
            DataTable dataTable = Database.DbHelper.GetInstance().ReadData(new User());
            foreach (DataRow dataRow in dataTable.Rows)
            {
                string username = dataRow[User.USERNAME].ToString();
                string password = dataRow[User.PASSWORD].ToString();

                if (username.CompareTo(data.Username) == 0 &&
                    password.CompareTo(data.Password) == 0)
                {
                    // found matched username and password
                    dbUserId = int.Parse(dataRow[User.USER_ID].ToString());
                    displayName = dataRow[User.LABEL].ToString();
                    break;
                }
            }

            if (displayName.Length == 0)
            {
                // no match found
                return;
            }

            // get the monitor info from client
            List<MonitorModel> monitorList = new List<MonitorModel>() ;
            int count = 1;
            foreach (MonitorInfo monitorInfo in data.MonitorsInfo)
            {
                MonitorModel model = new MonitorModel()
                {
                    Label = String.Format("{0}'s PC: Monitor {1}", displayName, count),
                    WorkAreaLeft = monitorInfo.LeftPos,
                    WorkAreaTop = monitorInfo.TopPos,
                    WorkAreaRight = monitorInfo.RightPos,
                    WorkAreaBottom = monitorInfo.BottomPos,
                };

                count++;

                monitorList.Add(model);
            };

            // notify UI
            ClientInfoModel clientModel = new ClientInfoModel()
            {
                DbUserId = dbUserId,
                SocketUserId = userId,
                Name = displayName,
                VncInfo = new VncModel() { IpAdress = data.VncServerIp, ListeningPort = data.VncServerPort },
                MonitorsInfo = monitorList,
            };

            server.ClientLogin(clientModel);
        }
    }
}
