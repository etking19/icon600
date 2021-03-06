﻿using SocketCommand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormClient.Telnet.Service
{
    public class TelnetServiceProvider : TcpServiceProvider
    {
        private string _receivedStr;

        public TelnetServiceProvider()
        {
        }

        public override object Clone()
        {
            return new TelnetServiceProvider();
        }

        public override void OnAcceptConnection(ConnectionState state)
        {
            try
            {
                _receivedStr = "";

                byte[] str = Encoding.UTF8.GetBytes("Welcome to Vistrol Telnet Service!\r\n");
                if (!state.Write(str, 0, str.Length))
                {
                    state.EndConnection();
                }
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message, "OnAcceptConnection");
            }
            
        }

        public override void OnReceiveData(ConnectionState state)
        {
            try
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
                            string reply = CommandParser.GetInstance().parseCommand(_receivedStr.Replace("\r\n", ""));
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
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "OnReceiveData");
            }
        }

        public override void OnDropConnection(ConnectionState state)
        {
            // no implementation here
        }
    }
}
