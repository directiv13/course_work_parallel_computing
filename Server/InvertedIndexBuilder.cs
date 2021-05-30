using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    class InvertedIndexBuilder
    {
        private object _locker = new object();
        private SortedDictionary<string, HashSet<string>> InvertedIndex { get; set; }
        public int StartFileIndex { get; set; }
        public int EndFileIndex { get; set; }

        public InvertedIndexBuilder(int startFileIndex, int endFileIndex)
        {
            StartFileIndex = startFileIndex;
            EndFileIndex = endFileIndex;
            InvertedIndex = new SortedDictionary<string, HashSet<string>>();
        }

        /// <summary>
        /// Метод для послідовної побудови індексу
        /// </summary>
        public void Build(string directory)
        {
            string[] fileNames = Directory.GetFiles(directory).Where(x => int.Parse(Path.GetFileName(x).Split('_')[0]) >= StartFileIndex && int.Parse(Path.GetFileName(x).Split('_')[0]) <= EndFileIndex).ToArray();

            foreach (string fileName in fileNames)
            {
                List<string> content = ReadFile(fileName).ToList();
                foreach (var token in content)
                {
                    if (!InvertedIndex.ContainsKey(token))
                    {
                        InvertedIndex.Add(token, new HashSet<string> { directory + @"\" + Path.GetFileName(fileName) });
                    }
                    else
                    {
                        InvertedIndex[token].Add(directory + @"\" + Path.GetFileName(fileName));
                    }

                }
            }
        }
        /// <summary>
        /// Метод для паралельної побудови індексу
        /// </summary>
        /// <param name="threadNumb">Кількість потоків</param>
        public void BuildParallel(int threadNumb, string directory)
        {
            string[] fileNames = Directory.GetFiles(directory).Where(x => int.Parse(Path.GetFileName(x).Split('_')[0]) >= StartFileIndex && int.Parse(Path.GetFileName(x).Split('_')[0]) <= EndFileIndex).ToArray();

            int filesAmount = EndFileIndex - StartFileIndex + 1;
            int fileThread = filesAmount / threadNumb;
            Thread[] threads = new Thread[threadNumb];

            for (int i = 0; i < threadNumb; i++)
            {
                int startFileIndex = StartFileIndex + fileThread * i;
                int endFileIndex = EndFileIndex - (threadNumb - i - 1) * fileThread;
                threads[i] = new Thread(new ParameterizedThreadStart(IndexBuildParallel));
                threads[i].Start(new IndexParameters()
                {
                    StartFileIndex = startFileIndex,
                    EndFileIndex = endFileIndex,
                    Directory = directory
                });
            }

            for (int i = 0; i < threadNumb; i++)
            {
                threads[i].Join();
            }
        }
        /// <summary>
        /// Метод для обробки документів у окремому потоці
        /// </summary>
        /// <param name="obj">Параметри методу</param>
        private void IndexBuildParallel(object obj)
        {
            Console.WriteLine("Index. Thread {0} started.", Thread.CurrentThread.ManagedThreadId);
            IndexParameters parameters = (IndexParameters)obj;
            int startFileIndex = parameters.StartFileIndex;
            int endFileIndex = parameters.EndFileIndex;
            string directory = parameters.Directory;

            string[] fileNames = Directory.GetFiles(directory).Where(x => int.Parse(Path.GetFileName(x).Split('_')[0]) >= startFileIndex && int.Parse(Path.GetFileName(x).Split('_')[0]) <= endFileIndex).ToArray();

            foreach (string fileName in fileNames)
            {
                List<string> content = ReadFile(fileName).ToList();
                foreach (var token in content)
                {
                    lock (_locker)
                    {
                        if (!InvertedIndex.ContainsKey(token))
                        {
                            InvertedIndex.Add(token, new HashSet<string> { directory + @"\" + Path.GetFileName(fileName) });
                        }
                        else
                        {
                            InvertedIndex[token].Add(directory + @"\" + Path.GetFileName(fileName));
                        }
                    }
                }
            }
            Console.WriteLine("Index. Thread {0} ended.", Thread.CurrentThread.ManagedThreadId);
        }
        /// <summary>
        /// Зчитування файлу
        /// </summary>
        /// <param name="fileName">Назва файлу</param>
        /// <returns>Колекція лексем</returns>
        private IEnumerable<string> ReadFile(string fileName)
        {
            string pattern = @"\W*";
            Regex regex = new Regex(pattern);
            List<string> result = File.ReadAllText(fileName).ToLower().Split(' ').ToList();

            for (int i = 0; i < result.Count(); i++)
            {
                result[i] = regex.Replace(result[i], "");
            }
            result.RemoveAll(x => x == "");
            return result;
        }
        /// <summary>
        /// Запис у файл з розширенням .json
        /// </summary>
        /// <param name="fileName">Назва файлу</param>
        public void WriteToJson(string fileName)
        {
            string jsonString = JsonSerializer.Serialize<SortedDictionary<string, HashSet<string>>>(InvertedIndex);
            FileStream file = File.Create(fileName + ".json");

            var writer = new StreamWriter(file);
            writer.WriteLine(jsonString);
            writer.Close();
        }
    }
}
