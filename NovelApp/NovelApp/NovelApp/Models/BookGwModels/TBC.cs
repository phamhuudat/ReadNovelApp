using System;
using System.Collections.Generic;
using System.Text;

namespace NovelApp.Models.BookGwModels
{
    public class TBC
    {
        public int NovelID { get; set; }
        public List<ChapInfo> Chapters { get; set; }
    }
    public class ChapInfo
    {
        public int No { get; set; }
        public string Name { get; set; }
        public DateTime UpdTime { get; set; }
        public int Type { get; set; }

    }
}
