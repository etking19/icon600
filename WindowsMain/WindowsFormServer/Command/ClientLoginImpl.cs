using Session.Data;
using Session.Data.SubData;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using WcfServiceLibrary1;
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

            /*
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
            }*/

            List<UserData> userDataList = new List<UserData>(Server.ServerDbHelper.GetInstance().GetAllUsers());
            UserData userData = userDataList.Find(user
                =>
                (user.username.CompareTo(data.Username) == 0 &&
                user.password.CompareTo(data.Password) == 0));
            if (userData == null)
            {
                // no matched 
                return;
            }

            // notify UI
            ClientInfoModel clientModel = new ClientInfoModel()
            {
                DbUserId = userData.id,
                SocketUserId = userId,
                Name = userData.name,
                LaunchedAppList = new Dictionary<int, int>(),
                LaunchedSourceList = new Dictionary<uint, int>(),
                LaunchedVncList = new Dictionary<int, int>(),
            };

            server.ClientLogin(clientModel);
        }
    }
}
