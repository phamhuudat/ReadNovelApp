using System;
namespace NovelApp.Models.BookGwModels
{
    public class PageChapter
    {
        public string Text { get; set; }
        public int IndexPage { get; set; }
        public double Coordinates { get; set; }
        public double LineHeight { get; set; }
        public int FontSize { get; set; }
        public int MaxLines { get; set; }
        public PageChapter()
        {
            
        }
    }
}
