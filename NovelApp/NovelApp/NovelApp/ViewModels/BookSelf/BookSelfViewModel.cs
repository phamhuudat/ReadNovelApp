using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using NovelApp.Helpers;
using NovelApp.Models.BookGwModels;
using NovelApp.Services.DatabaseService;
using NovelApp.Views;
using NovelApp.Views.Popup;
using Prism.Commands;
using Prism.Navigation;

namespace NovelApp.ViewModels.BookSelf
{
    public class BookSelfViewModel : BaseViewModel
    {
        public bool IsCheckedSortA { get => isCheckedSortA; set => SetProperty(ref isCheckedSortA, value); }
        public bool IsCheckedSortRecent { get => isCheckedSortRecent; set => SetProperty(ref isCheckedSortRecent, value); }
        public bool IsShowSortPopup { get => isShowSortPopup; set => SetProperty(ref isShowSortPopup, value); }

        public List<Novel> RecentList { get => recentList; set => SetProperty(ref recentList, value); }
        public List<Novel> FollowingList { get => followingList; set => SetProperty(ref followingList, value); }
        public List<Novel> DownloadingList { get => downloadingList; set => SetProperty(ref downloadingList, value); }
        private readonly IDatabaseService _databaseService;
        private List<Novel> recentList;
        private List<Novel> followingList;
        private List<Novel> downloadingList;
        private bool isShowSortPopup;
        private bool isCheckedSortA;
        private bool isCheckedSortRecent;

        public ICommand NavigationLibraryCommand { get; set; }
        public ICommand NavigationReadDetailCommand { get; set; }
        public ICommand SortListCommand { get; set; }
        public ICommand DisposeSortCommand { get; set; }
        public BookSelfViewModel(INavigationService navigationService, IDatabaseService database) : base(navigationService)
        {
            _databaseService = database;
            NavigationLibraryCommand = new DelegateCommand<Novel>(NavigationLibraryPopup);
            NavigationReadDetailCommand = new DelegateCommand<object>(NavigationReadDetail);
            SortListCommand = new DelegateCommand(NavigationSortList);
            DisposeSortCommand = new DelegateCommand(DisposeSort);
        }
        private void DisposeSort()
        {
            IsShowSortPopup = !IsShowSortPopup;
        }
        private async void NavigationSortList()
        {
            //await NavigationService.NavigateAsync($"{nameof(SortReadingPopup)}");
        }
        private async void NavigationReadDetail(object obj)
        {
            var listView = obj as Syncfusion.ListView.XForms.SfListView;
            if (listView == null) return;
            var item = listView.SelectedItem as Novel;
            listView.SelectedItem = null;
            await NavigationService.NavigateAsync($"{nameof(ReadBookPage)}?ID={item.ID}&NO={item.ReadState}");
        }
        public void SaveSortMode()
        {
           
        }
        public async void NavigationLibraryPopup(Novel novel)
        {
            var param = new NavigationParameters();
            param.Add("Novel", novel);
            param.Add("ID", novel.ID);
            await NavigationService.NavigateAsync($"{nameof(LibraryPopup)}", param);
        }
        public async Task<bool> GetRecentList()
        {
            var bookInfoDto = new List<Novel>();
            var bookInfos = await _databaseService.GetRecentlyBookInfos();
            foreach (var obj in bookInfos)
            {
                bookInfoDto.Add(NovelConverterHelper.BookToConvertNovel(obj));
            }
            RecentList = bookInfoDto;
            return true;
        }
        public async Task<bool> GetFollowingList()
        {
            var bookInfoDto = new List<Novel>();
            var bookInfos = await _databaseService.GetFollowingBookInfos();
            foreach (var obj in bookInfos)
            {
                bookInfoDto.Add(NovelConverterHelper.BookToConvertNovel(obj));
            }
            FollowingList = bookInfoDto;
            return true;
        }
        public async Task<bool> GetDownloadingList()
        {
            var bookInfoDto = new List<Novel>();
            var bookInfos = await _databaseService.GetDownloadBookInfos();
            foreach (var obj in bookInfos)
            {
                bookInfoDto.Add(NovelConverterHelper.BookToConvertNovel(obj));
            }
            DownloadingList = bookInfoDto;
            return true;
        }
    }
}
