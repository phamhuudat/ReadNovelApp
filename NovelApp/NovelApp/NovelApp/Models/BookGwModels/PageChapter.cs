using System;
namespace NovelApp.Models.BookGwModels
{
    public class PageChapter
    {
        public int IndexPageInChapter { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public int IndexPage { get; set; }
        public int CountPage { get; set; }
        public double Coordinates { get; set; }
        public double LineHeight { get; set; }
        public int FontSize { get; set; }
        public int MaxLines { get; set; }
        public bool ShowSwipToNextChapter { get; set; }
        public PageChapter()
        {
            
        }
    }
}
