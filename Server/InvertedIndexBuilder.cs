using System;
using System.Collections.Generic;
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
            throw new NotImplementedException();
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
