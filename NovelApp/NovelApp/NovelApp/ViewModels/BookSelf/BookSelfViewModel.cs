using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using NovelApp.Configurations;
using NovelApp.Helpers;
using NovelApp.Models.BookGwModels;
using NovelApp.Services.CacheService;
using NovelApp.Services.DatabaseService;
using NovelApp.Views;
using NovelApp.Views.Popup;
using Prism.Commands;
using Prism.Navigation;

namespace NovelApp.ViewModels.BookSelf
{
    public class BookSelfViewModel : BaseViewModel
    {
        public bool IsCheckedSortAZ { get => isCheckedSortA; set => SetProperty(ref isCheckedSortA, value); }
        public bool IsCheckedSortRecent { get => isCheckedSortRecent; set => SetProperty(ref isCheckedSortRecent, value); }
        public bool IsShowSortPopup { get => isShowSortPopup; set => SetProperty(ref isShowSortPopup, value); }

        public List<Novel> RecentList { get => recentList; set => SetProperty(ref recentList, value); }
        public List<Novel> FollowingList { get => followingList; set => SetProperty(ref followingList, value); }
        public List<Novel> DownloadingList { get => downloadingList; set => SetProperty(ref downloadingList, value); }
        private readonly IDatabaseService _databaseService;
        private readonly ICacheService _cacheService;
        private List<Novel> recentList;
        private List<Novel> followingList;
        private List<Novel> downloadingList;
        private bool isShowSortPopup;
        private bool isCheckedSortA;
        private bool isCheckedSortRecent;

        public ICommand NavigationLibraryCommand { get; set; }
        public ICommand NavigationReadDetailCommand { get; set; }
        public ICommand DisposeSortCommand { get; set; }
        public BookSelfViewModel(INavigationService navigationService, IDatabaseService database, ICacheService cacheService) : base(navigationService)
        {
            _databaseService = database;
            _cacheService = cacheService;
            NavigationLibraryCommand = new DelegateCommand<Novel>(NavigationLibraryPopup);
            NavigationReadDetailCommand = new DelegateCommand<object>(NavigationReadDetail);
            DisposeSortCommand = new DelegateCommand(DisposeSort);
        }
        private async void DisposeSort()
        {
            IsShowSortPopup = !IsShowSortPopup;
            if (!IsShowSortPopup)
            {
                if (IsCheckedSortAZ) _cacheService.SaveCache(AppConstants.CacheParameter.FilterMode, "1");
                else _cacheService.SaveCache(AppConstants.CacheParameter.FilterMode, "2");
                await GetRecentList();
                await GetFollowingList();
                await GetDownloadingList();
            }
            else
            {
                InstanceFilterMode();
            }
        }
        public void InstanceFilterMode()
        {
            var value = _cacheService.GetCache(AppConstants.CacheParameter.FilterMode);
            var filterMode = 1;
            if (!string.IsNullOrEmpty(value))
            {
                filterMode = int.Parse(value);
            }
            if (filterMode == 1) IsCheckedSortAZ = true;
            else IsCheckedSortRecent = true;
        }
        private async void NavigationReadDetail(object obj)
        {
            var listView = obj as Syncfusion.ListView.XForms.SfListView;
            if (listView == null) return;
            var item = listView.SelectedItem as Novel;
            listView.SelectedItem = null;
            await NavigationService.NavigateAsync($"{nameof(ReadBookPage)}?ID={item.ID}&NO={item.ReadState}");
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
            if (IsCheckedSortAZ)
                RecentList = bookInfoDto.OrderBy(x => x.Name).ToList();
            else
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
            if (IsCheckedSortAZ)
                FollowingList = bookInfoDto.OrderBy(x => x.Name).ToList();
            else
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
            if (IsCheckedSortAZ)
                DownloadingList = bookInfoDto.OrderBy(x => x.Name).ToList();
            else
                DownloadingList = bookInfoDto;
            return true;
        }
    }
}
