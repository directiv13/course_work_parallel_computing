using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    class InvertedIndexBuilder
    {
        SortedDictionary<string, HashSet<string>> InvertedIndex { get; set; }
        public int StartFileIndex { get; set; }
        public int EndFileIndex { get; set; }

        public InvertedIndexBuilder(int startFileIndex, int endFileIndex)
        {
            StartFileIndex = startFileIndex;
            EndFileIndex = endFileIndex;
            InvertedIndex = new SortedDictionary<string, HashSet<string>>();
        }

        /// <summary>
        /// Метод для послідовнох побудови індексу
        /// </summary>
        public void Build()
        {
            string[] fileNames = Directory.GetFiles(@"datasets\acllmdb\test\neg").Where(x => int.Parse(Path.GetFileName(x).Split('_')[0]) >= StartFileIndex && int.Parse(Path.GetFileName(x).Split('_')[0]) <= EndFileIndex).ToArray();

            foreach (string fileName in fileNames)
            {
                List<string> content = ReadFile(fileName).ToList();
                foreach (var token in content)
                {
                    if (!InvertedIndex.ContainsKey(token))
                    {
                        InvertedIndex.Add(token, new HashSet<string> { Path.GetFileName(fileName) });
                    }
                    else
                    {
                        InvertedIndex[token].Add(Path.GetFileName(fileName));
                    }

                }
            }
            WriteToJson("InvertedIndex");
        }
        /// <summary>
        /// Метод для паралельної побудови індексу
        /// </summary>
        /// <param name="threadNumb">Кількість потоків</param>
        public void BuildParallel(int threadNumb)
        {
            string[] fileNames = Directory.GetFiles(@"datasets\acllmdb\test\neg").Where(x => int.Parse(Path.GetFileName(x).Split('_')[0]) >= StartFileIndex && int.Parse(Path.GetFileName(x).Split('_')[0]) <= EndFileIndex).ToArray();

            int filesAmount = EndFileIndex - StartFileIndex + 1;
            int fileThread = filesAmount / threadNumb;
            Thread[] threads = new Thread[threadNumb];

            for (int i = 0; i < threadNumb; i++)
            {
                int startFileIndex = StartFileIndex + fileThread * i;
                int endFileIndex = EndFileIndex - (threadNumb - i - 1) * fileThread;
                threads[i] = new Thread(new ParameterizedThreadStart(IndexBuildParallel));
                threads[i].Start(new[] { startFileIndex, endFileIndex });
            }

            for (int i = 0; i < threadNumb; i++)
            {
                threads[i].Join();
            }
            Task.Run(() => WriteToJson("InvertedIndex"));
        }
        private void IndexBuildParallel(object obj)
        {
            throw new NotImplementedException();
        }
        private IEnumerable<string> ReadFile(string fileName)
        {
            throw new NotImplementedException();
        }
        private async void WriteToJson(string fileName)
        {
            throw new NotImplementedException();
        }
    }
}
