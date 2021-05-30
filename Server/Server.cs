using System;
using System.Net.Sockets;
using System.Text;
using System.Net;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;
using System.Threading;
using System.Linq;

namespace Server
{
    class Server
    {
        private readonly int _port = 8005;
        private readonly Socket _appSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private readonly IPEndPoint _ipEndPoint;

        private readonly Dictionary<string, HashSet<string>> _index;
        public Server()
        {
            _ipEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), _port);
            string jsonString = File.ReadAllText("InvertedIndex.json");
            _index = JsonSerializer.Deserialize<Dictionary<string, HashSet<string>>>(jsonString);
        }
        /// <summary>
        /// Запуск
        /// </summary>
        public void Run()
        {
            try
            {
                _appSocket.Bind(_ipEndPoint);
                _appSocket.Listen(10);
                while (true)
                {
                    Socket handler = _appSocket.Accept();

                    var th = new Thread(Process);
                    th.Start(handler);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        /// <summary>
        /// Метод для виконання у окремому потоці
        /// </summary>
        /// <param name="obj">Параметри методу</param>
        private void Process(object obj)
        {
            Console.WriteLine("Thread ID: {0}", Thread.CurrentThread.ManagedThreadId);
            Socket handler = (Socket)obj;
            string searchRequest = ReceiveRequest(handler);
            Console.WriteLine("ThreadID: {0}\nReceived request: {1}", Thread.CurrentThread.ManagedThreadId, searchRequest);
            List<string> searchResult = Search(searchRequest).ToList();
            StringBuilder builder = new StringBuilder();
            builder.AppendJoin(' ', searchResult);

            SendResult(handler, builder.ToString());
        }
        /// <summary>
        /// Повертає колекцію імен документів у яких міститься запит
        /// </summary>
        /// <param name="searchRequest">Пошуковий запит</param>
        private IEnumerable<string> Search(string searchRequest)
        {
            searchRequest = searchRequest.Replace("<EOF>", "");
            string[] termins = searchRequest.ToLower().Split(' ');
            HashSet<string> searchResult = _index[termins[0]];

            for (int i = 1; i < termins.Length; i++)
                searchResult.IntersectWith(_index[termins[i]]);

            return searchResult;
        }
        /// <summary>
        /// Отримуємо запит (повідомлення) від користувача
        /// </summary>
        /// <param name="handler">Сокет, що обслуговує даного користувача</param>
        /// <returns>Запит, отриманий від користувача</returns>
        private string ReceiveRequest(Socket handler)
        {
            string request = "";
            byte[] dataBuffer = new byte[1024];

            while (true)
            {
                int bytesRec = handler.Receive(dataBuffer);
                request += Encoding.ASCII.GetString(dataBuffer, 0, bytesRec);
                if (request.IndexOf("<EOF>") > -1)
                {
                    break;
                }
            }
            return request;
        }
        /// <summary>
        /// Надислаємо відповідь користувачу
        /// </summary>
        /// <param name="handler">Сокет, що обслуговує даного користувача</param>
        /// <param name="data">Дані, що будуть надіслані користувачу</param>
        private void SendResult(Socket handler, string data)
        {
            Console.WriteLine("ThreadID: {0}\nSearch result: {0}", Thread.CurrentThread.ManagedThreadId, data);
            byte[] msg = Encoding.ASCII.GetBytes(data);

            handler.Send(msg);
        }
    }
}
