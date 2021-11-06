using System;
using System.Collections.Generic;
using System.Text;

namespace NovelApp.Models.BookGwModels
{
    public class Chapter
    {
        public int NovelID { get; set; }
        public int No { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public DateTime UpdTime { get; set; }
        /// <summary>
        /// type=1 is vip chapter, type=0 is free
        /// </summary>
        public int Type { get; set; }
    }
}
