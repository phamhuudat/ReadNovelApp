using System;
using System.Collections.ObjectModel;
using System.Linq;
using NovelApp.Models.BookGwModels;
using NovelApp.Services.Book;
using Prism.Navigation;

namespace NovelApp.ViewModels
{
    public class HomePageViewModel : BaseViewModel
    {
        public ObservableCollection<Novel> ListNovel { get => listNovel; set => SetProperty(ref listNovel, value); }
        private readonly IBookService _bookService;
        private ObservableCollection<Novel> listNovel;

        public HomePageViewModel(INavigationService navigationService, IBookService bookService) : base(navigationService)
        {
            _bookService = bookService;
        }
        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            var list = await _bookService.GetNovelList(0);
            if (list != null && list.Any())
                ListNovel = new ObservableCollection<Novel>(list);
        }
    }
}
