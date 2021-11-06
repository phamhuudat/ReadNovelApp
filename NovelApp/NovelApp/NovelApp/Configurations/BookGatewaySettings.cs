using System;
using System.Collections.Generic;
using System.Text;

namespace NovelApp.Configurations
{
    public class BookGatewaySettings
    {
        public static BookGatewaySettings Instance { get; } = new BookGatewaySettings();

        public string GatewayBase { get; } = AppSettings.BookGatewayUrl;
        public Controller Controllers { get; } = new Controller();
        public string PathPrefix { get; } = "/api/";
        public class Controller
        {
            public string Comment { get; } = "Comments";
            public string PostComment { get; } = "PostComments";
            public string Chapter { get; } = "Chapter";
            public string User { get; } = "User";
            public string Search { get; } = "Search";
            public string List { get; } = "List";
            public string NovelInfo { get; } = "novelinfo";
            public string Table { get; } = "Table";
        }
    }
}
