
namespace Session.Data
{
    public abstract class BaseCmd
    {
        protected System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();

        public abstract string getCommandString();
    }
}
