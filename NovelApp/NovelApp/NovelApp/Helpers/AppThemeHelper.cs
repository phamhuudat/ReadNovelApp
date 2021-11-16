using NovelApp.DependencyServices;
using NovelApp.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace NovelApp.Helpers
{
    public class AppThemeHelper
    {
        public static void SetAppTheme(Theme theme)
        {
            var nav = App.Current.MainPage as Xamarin.Forms.NavigationPage;

            var e = DependencyService.Get<IAppTheme>();
            e.SetAppTheme(theme);
            if (theme == Theme.Dark)
            {
                e?.SetStatusBarColor(Color.FromHex("#0e0d13"), false);
                if (nav != null)
                {
                    nav.BarBackgroundColor = Color.FromHex("#0e0d13");
                    nav.BarTextColor = Color.White;
                }
            }
            else
            {
                e?.SetStatusBarColor(Color.FromHex("#f7f8fd"), true);
                if (nav != null)
                {
                    nav.BarBackgroundColor = Color.FromHex("#f7f8fd");
                    nav.BarTextColor = Color.Black;
                }
            }
        }
    }
}
