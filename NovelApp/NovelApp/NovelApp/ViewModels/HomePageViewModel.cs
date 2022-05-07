﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using NovelApp.Models.BookGwModels;
using NovelApp.Services.Book;
using NovelApp.Services.DatabaseService;
using NovelApp.ViewModels.BookSelf;
using NovelApp.Views;
using Prism.Commands;
using Prism.Navigation;

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
            BookSelfVM = new BookSelfViewModel(navigationService, _databaseService);
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
