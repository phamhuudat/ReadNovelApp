using System;
using System.Collections.Generic;
using System.Text;

namespace NovelApp.Models.BookGwModels
{
    public class Novel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public int Views { get; set; }
        public string Image { get; set; }
        public string Genre { get; set; }
        public string Description { get; set; }
        public string LastChapter { get; set; }
        public DateTime UpdTime { get; set; }
        public List<string> Tags { get; set; }
    }
}
