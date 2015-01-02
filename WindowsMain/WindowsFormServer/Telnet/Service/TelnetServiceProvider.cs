using SocketCommand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormClient.Telnet.Service
{
    public class TelnetServiceProvider : TcpServiceProvider
    {
        private string _receivedStr;
        private CommandParser _parser;

        public TelnetServiceProvider(CommandParser parser)
        {
            _parser = parser;
        }

        public override object Clone()
        {
            return new TelnetServiceProvider(new CommandParser());
        }

        public override void OnAcceptConnection(ConnectionState state)
        {
            _receivedStr = "";

            byte[] str = Encoding.UTF8.GetBytes("Welcome to Vistrol Telnet Service!\r\n");
            if (!state.Write(str, 0, str.Length))
            {
                state.EndConnection();
            }
        }

        public override void OnReceiveData(ConnectionState state)
        {
            byte[] buffer = new byte[1024];
            while (state.AvailableData > 0)
            {
                int readBytes = state.Read(buffer, 0, 1024);
                if (readBytes > 0)
                {
                    _receivedStr += Encoding.UTF8.GetString(buffer, 0, readBytes);
                    if (_receivedStr.IndexOf("\r\n") >= 0)
                    {
                        string reply = _parser.parseCommand(_receivedStr.Replace("\r\n", ""));
                        state.Write(Encoding.UTF8.GetBytes(reply), 0, reply.Length);
                        
                        _receivedStr = "";
                    }
                }
                else
                {
                    state.EndConnection();
                }
            }
        }

        public override void OnDropConnection(ConnectionState state)
        {
            // no implementation here
        }
    }
}
