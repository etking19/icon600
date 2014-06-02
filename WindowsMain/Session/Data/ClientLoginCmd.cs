
namespace Session.Data
{
    public class ClientLoginCmd : BaseCmd
    {
        public string username { get; set; }

        public string password { get; set; }

        public override string getCommandString()
        {
            return serializer.Serialize(this);
        }
    }
}
