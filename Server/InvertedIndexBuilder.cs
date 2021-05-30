using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

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
        public void BuildParallel(int threadNumb)
        {
            throw new NotImplementedException();
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
