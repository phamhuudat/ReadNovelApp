using NovelApp.DependencyServices;
using NovelApp.Droid.DependencyServices;
using NovelApp.Models.Enums;
using NovelApp.Styles;
using System;
using Xamarin.Forms;

[assembly: Dependency(typeof(ThemeService))]
namespace NovelApp.Droid.DependencyServices
{
    public class ThemeService : IAppTheme
    {
        public void SetAppTheme(Theme theme)
        {

            SetTheme(theme);
        }
        void SetTheme(Theme mode)
        {
            if (mode == Theme.Dark)
            {
                if (App.AppTheme == Theme.Dark)
                    return;
                App.Current.Resources = new DarkTheme();
            }
            else
            {
                if (App.AppTheme != Theme.Dark)
                    return;
                App.Current.Resources = new LightTheme();
            }
            App.AppTheme = mode;
        }
    }
}