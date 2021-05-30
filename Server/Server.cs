using System;
using System.Net.Sockets;
using System.Text;
using System.Net;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;
using System.Threading;

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
        /// <summary>
        /// Запуск
        /// </summary>
        public void Run()
        {
            try
            {
                appSocket.Bind(ipEndPoint);
                appSocket.Listen(10);
                while (true)
                {
                    Socket handler = appSocket.Accept();

                    var th = new Thread(Process);
                    th.Start(handler);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private void Process(object obj)
        {
            throw new NotImplementedException();
        }
        private IEnumerable<string> Search(string searchRequest)
        {
            throw new NotImplementedException();
        }
        private string ReceiveRequest(Socket handler)
        {
            throw new NotImplementedException();
        }
        private void SendResult(Socket handler, string data)
        {
            throw new NotImplementedException();
        }
    }
}
