using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NovelApp.Helpers;
using NovelApp.Models.BookGwModels;
using NovelApp.Services.DatabaseService;
using Prism.Navigation;

namespace NovelApp.ViewModels.BookSelf
{
    public class BookSelfViewModel : BaseViewModel
    {
        public List<Novel> RecentList { get => recentList; set => SetProperty(ref recentList, value); }
        public List<Novel> FollowingList { get => followingList; set => SetProperty(ref followingList, value); }
        public List<Novel> DownloadingList { get => downloadingList; set => SetProperty(ref downloadingList, value); }
        private readonly IDatabaseService _databaseService;
        private List<Novel> recentList;
        private List<Novel> followingList;
        private List<Novel> downloadingList;

        public BookSelfViewModel(INavigationService navigationService, IDatabaseService database) : base(navigationService)
        {
            _databaseService = database;
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
