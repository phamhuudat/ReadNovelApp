using Realms;
using System;
using System.Collections.Generic;
using System.Text;

namespace NovelApp.Models.BookGwModels
{
    public class BookInfo : RealmObject
    {
        public int ID { get; set; }
        public string BookName { get; set; }
        public string Author { get; set; }
        public int ReadState { get; set; }
        public int ListType { get; set; }
        public string LastReadTime { get; set; }
    }
}
