using Realms;
using System;
using System.Collections.Generic;
using System.Text;

namespace NovelApp.Models.BookGwModels
{
    public class BookInfo : RealmObject
    {
        [PrimaryKey]
        public int ID { get; set; }
        public string BookName { get; set; }
        public string Author { get; set; }
        public int ReadState { get; set; }
        public int ListType { get; set; }
        public string LastReadTime { get; set; }
        public string Genre { get; set; }
        public string Description { get; set; }
        public string LastChapter { get; set; }
        public string Image { get; set; }
        public string Updtime { get; set; }
        public int LastReadState { get; set; }
    }
}
