using System;
using System.Collections.Generic;
using System.Text;


namespace NovelApp.Configurations
{
    public class AppConstants
    {
        public class AppParameters
        {
            internal const string DatabaseNovel = "Novel.realm";
        }
        public class NavigationParameter
        {
            internal const string GoBack = "GoBack";
            internal const string PageFromParameter = "PageFrom";
            internal const string NovelId = "ID";
            internal const string NoChapter = "NO";
        }
        public class FontFamily
        {
            internal const string VnTimeFont = "VT";
            internal const string RobotoFont = "RR";
            internal const string ArialFont = "AR";

        }
        public class FilterParameter
        {
            internal const string Status = "stat";
            internal const string Genre = "gen";
            internal const string Chapters = "chno";
            internal const string Type = "tag";
        }
        public class CacheParameter
        {
            internal const string DatabaseLocalRealm = "RealmDatabase.realm";
            internal const string TextSize = "TextSize";
            internal const string TextColor = "TextColor";
            internal const string TextFont = "TextFont";
            internal const string ReadMode = "ReadMode";
            internal const string ThemeMode = "ThemeMode";
            internal const string FilterMode = "FilterMode";

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
