using System;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            InvertedIndexBuilder index = new InvertedIndexBuilder(0, 249);
            index.BuildParallel(5);

            Server server = new Server();
            server.Run();
        }
    }
}
