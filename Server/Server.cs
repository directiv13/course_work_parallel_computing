using System;
using System.Net.Sockets;
using System.Text;
using System.Net;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;

namespace Server
{
    class Server
    {
        private readonly int port = 8005;
        private readonly Socket appSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private readonly IPEndPoint ipEndPoint;

        private readonly Dictionary<string, HashSet<string>> index;
        public Server()
        {
            ipEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);
            string jsonString = File.ReadAllText("InvertedIndex.json");
            index = JsonSerializer.Deserialize<Dictionary<string, HashSet<string>>>(jsonString);
        }
        
    }
}
