using System;
using System.Collections.Generic;
using System.Text;


namespace NovelApp.Configurations
{
    public class AppConstants
    {
        public class NavigationParameter
        {
            internal const string GoBack = "GoBack";
            internal const string PageFromParameter = "PageFrom";
            internal const string NovelId = "ID";
            internal const string NoChapter = "NO";
        }
        public class Message
        {
            internal const string MessageSettings = "Settings";
        }
        public class UrlConstants_Development
        {
            internal const string BookGatewayUrl = "http://book2.somee.com";
        }
        public class UrlConstants_Production
        {
            internal const string BookGatewayUrl = "http://book2.somee.com";
        }
    }
}
