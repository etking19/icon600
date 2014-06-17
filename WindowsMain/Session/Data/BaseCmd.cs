
namespace Session.Data
{
    public class BaseCmd : ICommand
    {
        protected System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();

        public override string getCommandString()
        {
            return serializer.Serialize(this);
        }
    }
}
