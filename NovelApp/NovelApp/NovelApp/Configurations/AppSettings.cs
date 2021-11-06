using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Environment = NovelApp.Models.Enums.Environment;
using Xamarin.Forms;
using static NovelApp.Configurations.AppConstants;

namespace NovelApp.Configurations
{
    public class AppSettings
    {
        public const Environment AppEnvironment = Environment.Development;
        public static string BookGatewayUrl = AppEnvironment == Environment.Development ?
            UrlConstants_Development.BookGatewayUrl :
            UrlConstants_Production.BookGatewayUrl;
    }
}