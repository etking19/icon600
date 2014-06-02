using System;
using System.Collections.Generic;
using System.Text;

namespace Socket
{
    public class StateObject
    {
        // Client  socket.
        public System.Net.Sockets.Socket workSocket = null;
        // Size of receive buffer.
        public const int BufferSize = 1024;
        // Receive buffer.
        public byte[] buffer = new byte[BufferSize];
        // Received data string.
        public StringBuilder sb = new StringBuilder();
    }
}
