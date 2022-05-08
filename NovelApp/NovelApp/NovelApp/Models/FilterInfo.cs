using System;
using Realms;

namespace NovelApp.Models
{
    public class FilterInfo: RealmObject
    {
        public int ID { get; set; }
        [PrimaryKey]
        public int Type { get; set; }
        public string Name { get; set; }
        public int IsDelete { get; set; }
    }
}
