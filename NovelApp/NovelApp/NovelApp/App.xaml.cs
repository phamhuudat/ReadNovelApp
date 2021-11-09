﻿
using NovelApp.Services.Book;
using NovelApp.Services.RequestProvider;
using NovelApp.ViewModels;
using NovelApp.Views;
using Prism;
using Prism.Ioc;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NovelApp
{
    public partial class App
    {
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage>();
            containerRegistry.RegisterForNavigation<HomePage,HomePageViewModel>();
            containerRegistry.RegisterForNavigation<BookDetailPage>();
            containerRegistry.RegisterForNavigation<TableContentPage,TableContentPageViewModel>();
            #region RegisterService SingleTon
            containerRegistry.RegisterSingleton<IRequestProvider, RequestProvider>();
            containerRegistry.RegisterSingleton<IBookService, BookService>();
            #endregion
        }
        protected override async void OnInitialized()
        {
            InitializeComponent();
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NTMwNTY3QDMxMzkyZTMzMmUzMEx2R2dSSFRkdVRDMGFrU1h3d1d3L04ycEJhS2IwdThOY2hIQnNkemwyRXc9");
            App.Current.UserAppTheme = OSAppTheme.Dark;
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
