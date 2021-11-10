using NovelApp.Services.Book;
using NovelApp.Services.RequestProvider;
using System;

namespace IntergationTest
{
    public class Program
    {
        static void Main(string[] args)
        {
            var requestProvider = new RequestProvider();
            var bookservice = new BookService(requestProvider);
            //var list = bookservice.GetNovelList(0).Result;
            //var novel = bookservice.GetDetailNovel(1).Result;
            //var TBC = bookservice.GetTBC(1).Result;
            //var chapter = bookservice.GetContentChapter(1,1).Result;
            //var comment = bookservice.GetCommentList(1).Result;
            //var listSearch = bookservice.SearchNovelList("This",1).Result;
            var result = bookservice.PostComment(1,"comment 1",3, "test@email.com");
            //var resultunlock = bookservice.UnlockChapter(1, 21, "test@email.com");
            //var user = bookservice.GetDetailUser("test@email.com");
            Console.ReadKey();
        }
    }
}
