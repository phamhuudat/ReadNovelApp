
using NovelApp.Services.Book;
using NovelApp.Services.RequestProvider;
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
            #region RegisterService SingleTon
            containerRegistry.RegisterSingleton<IRequestProvider, RequestProvider>();
            containerRegistry.RegisterSingleton<IBookService, BookService>();
            #endregion
        }
        protected override async void OnInitialized()
        {
            InitializeComponent();
            await NavigationService.NavigateAsync("NavigationPage/MainPage");
           
        }
        public App()
        {

        }
        public App(IPlatformInitializer platformInitializer) : base(platformInitializer)
        {
        }
    }
}
