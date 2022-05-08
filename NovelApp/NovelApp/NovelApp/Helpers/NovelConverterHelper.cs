using System;
using NovelApp.Models.BookGwModels;

namespace NovelApp.Helpers
{
    public class NovelConverterHelper
    {
        public NovelConverterHelper()
        {
        }
        public static Novel BookToConvertNovel(BookInfo obj)
        {
            return new Novel()
            {
                ID = obj.ID,
                Name = obj.BookName,
                Author = obj.Author,
                LastChapter = obj.LastChapter,
                Description = obj.Description,
                Genre = obj.Genre,
                Image = obj.Image,
                UpdTime = DateTime.Parse(obj.Updtime),
                LastReadState = obj.LastReadState,
                ReadState = obj.ReadState
            };
        }
        public static BookInfo NovelToConverterBook(Novel obj, int listType)
        {
            return new BookInfo()
            {
                ID = obj.ID,
                BookName = obj.Name,
                Author = obj.Author,
                LastChapter = obj.LastChapter,
                Description = obj.Description,
                Genre = obj.Genre,
                Image = obj.Image,
                Updtime = obj.UpdTime.ToString(),
                ReadState = 0,
                LastReadTime = DateTime.Now.ToString(),
                ListType = listType,
                LastReadState = int.Parse(obj.LastChapter.Replace("Chapter ", ""))
            };
        }
        public static ChapterInfo ChapterToConverterChapterInfo(Chapter chapter)
        {
            return new ChapterInfo
            {
                NovelID = chapter.NovelID,
                No = chapter.No,
                Name = chapter.Name,
                Text = chapter.Text,
                UpdTime = chapter.UpdTime.ToString(),
                Type = chapter.Type,
                Id = chapter.NovelID + chapter.No
            };
        }
        public static Chapter ChapterInfoToConverterChapter(ChapterInfo chapter)
        {
            return new Chapter
            {
                NovelID = chapter.NovelID,
                No = chapter.No,
                Name = chapter.Name,
                Text = chapter.Text,
                UpdTime =DateTime.Parse(chapter.UpdTime),
                Type = chapter.Type
            };
        }
    }
}
