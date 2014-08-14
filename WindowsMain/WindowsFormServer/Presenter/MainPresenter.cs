using Database.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormClient.Presenter
{
    class MainPresenter
    {
        public int PortMin 
        { 
            get
            {
                return Server.ServerDbHelper.GetInstance().GetSetting().port_start;
            }

            set
            {
                Setting currentSetting = Server.ServerDbHelper.GetInstance().GetSetting();
                Server.ServerDbHelper.GetInstance().AddOrEditSetting(
                    value,
                    currentSetting.port_end,
                    currentSetting.matrix_col,
                    currentSetting.matrix_row,
                    currentSetting.vnc_path);
            }
        }

        public int PortMax 
        {
            get
            {
                return Server.ServerDbHelper.GetInstance().GetSetting().port_end;
            }

            set
            {
                Setting currentSetting = Server.ServerDbHelper.GetInstance().GetSetting();
                Server.ServerDbHelper.GetInstance().AddOrEditSetting(
                    currentSetting.port_start,
                    value,
                    currentSetting.matrix_col,
                    currentSetting.matrix_row,
                    currentSetting.vnc_path);
            }
        }

        public int ScreenRow
        {
            get
            {
                return Server.ServerDbHelper.GetInstance().GetSetting().matrix_row;
            }

            set
            {
                Setting currentSetting = Server.ServerDbHelper.GetInstance().GetSetting();
                Server.ServerDbHelper.GetInstance().AddOrEditSetting(
                    currentSetting.port_start,
                    currentSetting.port_end,
                    currentSetting.matrix_col,
                    value,
                    currentSetting.vnc_path);
            }
        }

        public int ScreenColumn
        {
            get
            {
                return Server.ServerDbHelper.GetInstance().GetSetting().matrix_col;
            }

            set
            {
                Setting currentSetting = Server.ServerDbHelper.GetInstance().GetSetting();
                Server.ServerDbHelper.GetInstance().AddOrEditSetting(
                    currentSetting.port_start,
                    currentSetting.port_end,
                    value,
                    currentSetting.matrix_row,
                    currentSetting.vnc_path);
            }
        }

        public string VncPath
        {
            get
            {
                return Server.ServerDbHelper.GetInstance().GetSetting().vnc_path;
            }

            set
            {
                Setting currentSetting = Server.ServerDbHelper.GetInstance().GetSetting();
                Server.ServerDbHelper.GetInstance().AddOrEditSetting(
                    currentSetting.port_start,
                    currentSetting.port_end,
                    currentSetting.matrix_col,
                    currentSetting.matrix_row,
                    value);
            }
        }
    }
}
