using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using NovelApp.DependencyServices;
using NovelApp.Helpers;
using NovelApp.Models.BookGwModels;
using NovelApp.Services.Book;
using NovelApp.Services.DatabaseService;
using NovelApp.ViewModels.BookSelf;
using NovelApp.Views;
using NovelApp.Views.Popup;
using Prism.Commands;
using Prism.Navigation;
using Xamarin.Forms;

namespace NovelApp.ViewModels
{
    public class HomePageViewModel : BaseViewModel
    {
        public ObservableCollection<Novel> ListNovel { get => listNovel; set => SetProperty(ref listNovel, value); }
        private readonly IBookService _bookService;
        private readonly IDatabaseService _databaseService;
        private ObservableCollection<Novel> listNovel;
        public ICommand LoadMoreCommand { get; set; }
        public ICommand SearchCommand { get; set; }
        public ICommand ItemTappedCommand { get; set; }
        public ICommand FilterCommand { get; set; }
        public ICommand FollowBookCommand { get; set; }
        private string _nameNovel;
        private BookSelfViewModel bookSelfVM;

        public BookSelfViewModel BookSelfVM { get => bookSelfVM; set => SetProperty(ref bookSelfVM, value); }
        public HomePageViewModel(INavigationService navigationService, IBookService bookService,
            IDatabaseService databaseService) : base(navigationService)
        {
            _bookService = bookService;
            _databaseService = databaseService;
            LoadMoreCommand = new DelegateCommand<object>(LoadMore);
            SearchCommand = new DelegateCommand<string>(SearchNovel);
            ItemTappedCommand = new DelegateCommand<object>(ItemTapped);
            FilterCommand = new DelegateCommand(NavigationFilterPopup);
            BookSelfVM = new BookSelfViewModel(navigationService, _databaseService);
            FollowBookCommand = new DelegateCommand<Novel>(FollowBook);
        }
        private async void FollowBook(Novel novel)
        {
            var result = await _databaseService.SaveBookInfo(NovelConverterHelper.NovelToConverterBook(novel, 2));
            if (result == Models.Enums.StatusEnum.Success)
            {
                DependencyService.Get<IToastMessage>().Show("Đã lưu thông tin sách");
            }
            else if (result == Models.Enums.StatusEnum.Exist)
            {
                DependencyService.Get<IToastMessage>().Show("Sách đã tồn tại trong bộ nhớ");
            }
            else
                DependencyService.Get<IToastMessage>().Show("Lỗi trong quá trình lưu thông tin sách");

        }
        private async void NavigationFilterPopup()
        {
            await NavigationService.NavigateAsync($"{nameof(FilterPopup)}");
        }
        private async void ItemTapped(object obj)
        {
            var listView = obj as Syncfusion.ListView.XForms.SfListView;
            if (listView == null) return;
            var item = listView.SelectedItem as Novel;
            listView.SelectedItem = null;
            await NavigationService.NavigateAsync($"{nameof(BookDetailPage)}?ID={item.ID}");
        }

        private async void SearchNovel(string name = "")
        {
            _nameNovel = name;
            if (string.IsNullOrEmpty(name))
            {
                LoadNovel();
            }
            else
            {
                var list = await _bookService.SearchNovelList(name, 0);
                if (list == null || !list.Any())
                {
                    list = new List<Novel>();
                }
                ListNovel = new ObservableCollection<Novel>(list);
            }

        }
        private async void LoadMore(object obj)
        {
            var listView = obj as Syncfusion.ListView.XForms.SfListView;
            if (listView == null) return;
            try
            {
                if (ListNovel == null || !ListNovel.Any())
                    return;
                listView.IsBusy = true;
                await Task.Delay(1000);
                var lastItem = ListNovel.Last();
                IEnumerable<Novel> list = new List<Novel>();
                if (string.IsNullOrEmpty(_nameNovel))
                    list = await _bookService.GetNovelList(lastItem.ID);
                else
                    list = await _bookService.SearchNovelList(_nameNovel, lastItem.ID);
                foreach (var item in list)
                    ListNovel.Add(item);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
            }
            listView.IsBusy = false;
        }
        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            SearchNovel(_nameNovel);
            await BookSelfVM.GetRecentList();
            await BookSelfVM.GetFollowingList();
            await BookSelfVM.GetDownloadingList();
        }

        private async void LoadNovel()
        {
            var list = await _bookService.GetNovelList(0);
            if (list == null || !list.Any())
            {
                list = new List<Novel>();
            }
            ListNovel = new ObservableCollection<Novel>(list);
        }
    }
}
