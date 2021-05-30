using System;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            InvertedIndexBuilder index = new InvertedIndexBuilder(0, 249);
            index.BuildParallel(5, @"datasets\acllmdb\test\neg");
            index.BuildParallel(5, @"datasets\acllmdb\test\pos");
            index.BuildParallel(5, @"datasets\acllmdb\train\neg");
            index.BuildParallel(5, @"datasets\acllmdb\train\pos");
            index.BuildParallel(5, @"datasets\acllmdb\train\unsup");
            index.WriteToJson("InvertedIndex");

            Server server = new Server();
            server.Run();
        }
    }
}
