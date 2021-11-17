
using NovelApp.Models.Enums;
using NovelApp.Services.Book;
using NovelApp.Services.CacheService;
using NovelApp.Services.RequestProvider;
using NovelApp.ViewModels;
using NovelApp.Views;
using NovelApp.Views.Popup;
using Prism;
using Prism.Ioc;
using Prism.Plugin.Popups;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NovelApp
{
    public partial class App
    {
        public static Theme AppTheme { get; set; }
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterPopupNavigationService();
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage>();
            containerRegistry.RegisterForNavigation<HomePage, HomePageViewModel>();
            containerRegistry.RegisterForNavigation<BookDetailPage>();
            containerRegistry.RegisterForNavigation<TableContentPage, TableContentPageViewModel>();
            containerRegistry.RegisterForNavigation<PostCommentPage, PostCommentPageViewModel>();
            containerRegistry.RegisterForNavigation<ReadBookPage, ReadBookPageViewModel>();
            containerRegistry.RegisterForNavigation<SettingsPopup, SettingsPopupViewModel>();
            #region RegisterService SingleTon
            containerRegistry.RegisterSingleton<IRequestProvider, RequestProvider>();
            containerRegistry.RegisterSingleton<IBookService, BookService>();
            containerRegistry.RegisterSingleton<ICacheService, CacheService>();
            #endregion
        }
        protected override async void OnInitialized()
        {
            InitializeComponent();
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NTMwNTY3QDMxMzkyZTMzMmUzMEx2R2dSSFRkdVRDMGFrU1h3d1d3L04ycEJhS2IwdThOY2hIQnNkemwyRXc9");
            App.Current.UserAppTheme = OSAppTheme.Dark;
            Helpers.AppThemeHelper.SetAppTheme(Theme.Dark);
            await NavigationService.NavigateAsync("NavigationPage/HomePage");

        }
        public App()
        {

        }
        public App(IPlatformInitializer platformInitializer) : base(platformInitializer)
        {
        }
    }
}
