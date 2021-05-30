using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Client
{
    class Client
    {
        private readonly int _port = 8005;
        private readonly Socket _sender = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private readonly IPEndPoint _ipEndPoint;

        public Client()
        {
            _ipEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), _port);
        }
        /// <summary>
        /// Запуск
        /// </summary>
        public void Run()
        {
            try
            {
                _sender.Connect(_ipEndPoint);

                Console.WriteLine("Socket connected to {0}", _sender.RemoteEndPoint.ToString());

                Console.WriteLine("Enter search request: ");
                string searchRequest = Console.ReadLine();
                Console.WriteLine();

                byte[] bytes = new byte[1024];

                // Кодуємо дані у байтовий масив.
                byte[] msg = Encoding.ASCII.GetBytes(searchRequest);

                // Надислаємо дані через сокет  
                int bytesSent = _sender.Send(msg);

                // Отримуємо відповідь від віддаленого пристрою
                int bytesRec = _sender.Receive(bytes);
                Console.WriteLine("Result: {0}",
                    Encoding.ASCII.GetString(bytes, 0, bytesRec));

                // Звільняємо сокет
                _sender.Shutdown(SocketShutdown.Both);
                _sender.Close();

            }
            catch (ArgumentNullException ane)
            {
                Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
            }
            catch (SocketException se)
            {
                Console.WriteLine("SocketException : {0}", se.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine("Unexpected exception : {0}", e.ToString());
            }
        }
    }
}
