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
using WindowsFormClient.Server.Model;

namespace WindowsFormClient.Presenter
{
    public class ApplicationsPresenter
    {
        public ApplicationsPresenter()
        {
        }

        public DataTable GetApplicationTable()
        {
            // get user's info (display name, username, group name)
            DataTable table = new DataTable();
            table.Columns.Add("Application Id", typeof(int)).ReadOnly = true;
            table.Columns.Add("Name", typeof(string)).ReadOnly = true;
            table.Columns.Add("Executable Path", typeof(string)).ReadOnly = true;
            table.Columns.Add("Arguments", typeof(string)).ReadOnly = true;
            table.Columns.Add("Display Area", typeof(string)).ReadOnly = true;

            foreach (ApplicationData data in Server.ServerDbHelper.GetInstance().GetAllApplications())
            {
                table.Rows.Add(data.id, data.name, data.applicationPath, data.arguments, String.Format("{0}, {1}, {2}, {3}", data.rect.Left, data.rect.Top, data.rect.Right, data.rect.Bottom));
            }

            return table;
        }

        public void AddApplication(string appName, string exePath, string arguments, int left, int top, int right, int bottom)
        {
            Server.ServerDbHelper.GetInstance().AddApplication(appName, arguments, exePath, left, top, right, bottom);
        }

        public void RemoveApplication(int appId)
        {
            Server.ServerDbHelper.GetInstance().RemoveApplication(appId);
        }

        public void EditApplication(int appId, string appName, string exePath, string arguments, int left, int top, int right, int bottom)
        {
            Server.ServerDbHelper.GetInstance().EditApplication(appId, appName, exePath, arguments, left, top, right, bottom);
        }
    }
}
