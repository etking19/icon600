using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsMain.Command
{
    class BaseImplementer : ICmdImplementer
    {
        protected System.Web.Script.Serialization.JavaScriptSerializer deserialize = new System.Web.Script.Serialization.JavaScriptSerializer();

        public virtual void ExecuteCommand(string userId, string command)
        {
            throw new NotImplementedException();
        }
    }
}
