using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WcfServiceLibrary1;

namespace WindowsFormClient.Presenter
{
    class MainPresenter
    {
        public int PortMin 
        { 
            get
            {
                return Server.ServerDbHelper.GetInstance().GetSetting().PortStart;
            }

            set
            {
                SettingData currentSetting = Server.ServerDbHelper.GetInstance().GetSetting();
                Server.ServerDbHelper.GetInstance().AddOrEditSetting(
                    value,
                    currentSetting.PortStart,
                    currentSetting.MatrixColumn,
                    currentSetting.MatrixRow,
                    currentSetting.VncPath);
            }
        }

        public int PortMax 
        {
            get
            {
                return Server.ServerDbHelper.GetInstance().GetSetting().PortEnd;
            }

            set
            {
                SettingData currentSetting = Server.ServerDbHelper.GetInstance().GetSetting();
                Server.ServerDbHelper.GetInstance().AddOrEditSetting(
                    currentSetting.PortStart,
                    value,
                    currentSetting.MatrixColumn,
                    currentSetting.MatrixRow,
                    currentSetting.VncPath);
            }
        }

        public int ScreenRow
        {
            get
            {
                return Server.ServerDbHelper.GetInstance().GetSetting().MatrixRow;
            }

            set
            {
                SettingData currentSetting = Server.ServerDbHelper.GetInstance().GetSetting();
                Server.ServerDbHelper.GetInstance().AddOrEditSetting(
                    currentSetting.PortStart,
                    currentSetting.PortEnd,
                    currentSetting.MatrixColumn,
                    value,
                    currentSetting.VncPath);
            }
        }

        public int ScreenColumn
        {
            get
            {
                return Server.ServerDbHelper.GetInstance().GetSetting().MatrixColumn;
            }

            set
            {
                SettingData currentSetting = Server.ServerDbHelper.GetInstance().GetSetting();
                Server.ServerDbHelper.GetInstance().AddOrEditSetting(
                    currentSetting.PortStart,
                    currentSetting.PortEnd,
                    value,
                    currentSetting.MatrixRow,
                    currentSetting.VncPath);
            }
        }

        public string VncPath
        {
            get
            {
                return Server.ServerDbHelper.GetInstance().GetSetting().VncPath;
            }

            set
            {
                SettingData currentSetting = Server.ServerDbHelper.GetInstance().GetSetting();
                Server.ServerDbHelper.GetInstance().AddOrEditSetting(
                    currentSetting.PortStart,
                    currentSetting.PortEnd,
                    currentSetting.MatrixColumn,
                    currentSetting.MatrixRow,
                    value);
            }
        }
    }
}
