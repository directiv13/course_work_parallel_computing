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
    }
}
