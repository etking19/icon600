
using Session.Data.SubData;
using System.Collections.Generic;
namespace Session.Data
{
    public class ClientLoginCmd : BaseCmd
    {
        public string Username { get; set; }

        public string Password { get; set; }
    }
}
