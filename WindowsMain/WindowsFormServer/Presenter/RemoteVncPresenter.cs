using Session;
using Session.Connection;
using Session.Data;
using Session.Data.SubData;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using WcfServiceLibrary1;

namespace WindowsFormClient.Presenter
{
    public class RemoteVncPresenter
    {
        public RemoteVncPresenter()
        {
        }

        public DataTable GetRemoteVncData()
        {
            /*
             * 1. data id
             * 2. label
             * 3. remote ip address
             * 4. remote port number
             */
            DataTable table = new DataTable();
            table.Columns.Add("Data Id", typeof(int)).ReadOnly = true;
            table.Columns.Add("Name", typeof(string)).ReadOnly = true;
            table.Columns.Add("IP Address", typeof(string)).ReadOnly = true;
            table.Columns.Add("Port Number", typeof(int)).ReadOnly = true;

            // get data from database
            foreach (RemoteVncData data in Server.ServerDbHelper.GetInstance().GetRemoteVncList())
            {
                table.Rows.Add(data.id, data.name, data.remoteIp, data.remotePort);
            }

            return table;
        }

        public void AddVnc(string label, string ipAdd, int port)
        {
            Server.ServerDbHelper.GetInstance().AddRemoteVnc(label, ipAdd, port);
        }

        public void RemoveVnc(int dataId)
        {
            Server.ServerDbHelper.GetInstance().RemoveRemoteVnc(dataId);
        }

        public void EditVnc(int dataId, string label, string ipAdd, int port)
        {
            Server.ServerDbHelper.GetInstance().EditRemoteVnc(dataId, label, ipAdd, port);
        }
    }
}
