using System;
using Realms;

namespace NovelApp.Models.BookGwModels
{
    public class ChapterInfo: RealmObject
    {
        public int NovelID { get; set; }
        public int No { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public string UpdTime { get; set; }
        /// <summary>
        /// type=1 is vip chapter, type=0 is free
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// No+NovelID
        /// </summary>
        [PrimaryKey]
        public int Id { get; set; }
    }
}
