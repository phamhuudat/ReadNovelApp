using System;
namespace NovelApp.Models.BookGwModels
{
    public class Comment
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public string Name { get; set; }
        public int NovelID { get; set; }
        public int Star { get; set; }
        public string Content { get; set; }
        public DateTime ModifiedTime { get; set; }
        public int Status { get; set; }

    }
}
