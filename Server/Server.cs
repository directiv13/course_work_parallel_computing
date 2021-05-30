using System;
using System.Net.Sockets;
using System.Text;
using System.Net;

namespace Server
{
    class Server
    {
        private readonly int port = 8005;
        private readonly Socket sender = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private readonly IPEndPoint ipEndPoint;
        public Server()
        {
            ipEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);
        }
    }
}
