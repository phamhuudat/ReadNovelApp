using NovelApp.Bussiness;
using NovelApp.Configurations;
using NovelApp.DependencyServices;
using NovelApp.Helpers;
using NovelApp.Models.BookGwModels;
using NovelApp.Services.Book;
using NovelApp.Services.DatabaseService;
using NovelApp.Views;
using NovelApp.Views.Popup;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace NovelApp.ViewModels
{
    public class BookDetailPageViewModel : BaseViewModel
    {
        private readonly IBookService _bookService;
        private readonly IDatabaseService _databaseService;
        private readonly IDownloadService _downloadService;
        public Novel Novel { get => novel; set => SetProperty(ref novel, value); }
        private int _novelId;
        private Novel novel;
        private bool isExpand;
        private List<Comment> listComment;
        private int countReview;
        private DownloadInfo novelDownloadInfo;

        public bool IsExpand { get => isExpand; set => SetProperty(ref isExpand, value); }
        public ICommand ExpandCommand { get; set; }
        public ICommand SearchChapterCommand { get; set; }
        public ICommand NavigationCmtCommand { get; set; }
        public ICommand NavigationReadCommand { get; set; }
        public ICommand GobackCommand { get; set; }
        public ICommand FollowBookCommand { get; set; }
        public ICommand DownloadBookCommand { get; set; }
        public DownloadInfo NovelDownloadInfo { get => novelDownloadInfo; set => SetProperty(ref novelDownloadInfo, value); }
        public List<Comment> ListComment { get => listComment; set => SetProperty(ref listComment, value); }
        public int CountReview { get => countReview; set => SetProperty(ref countReview, value); }
        public BookDetailPageViewModel(INavigationService navigationService, IBookService bookService,
            IDatabaseService databaseService, IDownloadService downloadService) : base(navigationService)
        {
            _bookService = bookService;
            _databaseService = databaseService;
            _downloadService = downloadService;
            ExpandCommand = new DelegateCommand(() =>
            {
                IsExpand = !IsExpand;
            });
            SearchChapterCommand = new DelegateCommand(NavigationSearchChapter);
            NavigationCmtCommand = new DelegateCommand(NavigationCommentPage);
            NavigationReadCommand = new DelegateCommand(NavigationReadNow);
            DownloadBookCommand = new DelegateCommand(DownLoadBook);
            GobackCommand = new DelegateCommand(async () =>
            {
                var param = new NavigationParameters();
                param.Add("BookDetail", 1);
                await NavigationService.GoBackAsync(param);
            });
            FollowBookCommand = new DelegateCommand(FollowBook);
        }
        private async void DownLoadBook()
        {
            var param = new NavigationParameters();
            param.Add("Novel", Novel);
            param.Add("ID", _novelId);
            await NavigationService.NavigateAsync($"{nameof(DownloadPopup)}", param);
        }
        private async void FollowBook()
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
        private async void NavigationReadNow()
        {
            var bookInfo = await _databaseService.GetBookInfo(_novelId);
            var no = 0;
            if (bookInfo != null)
                no = bookInfo.ReadState;
            await NavigationService.NavigateAsync($"{nameof(ReadBookPage)}?{AppConstants.NavigationParameter.NovelId}={_novelId}&{AppConstants.NavigationParameter.NoChapter}={no}");
            var result = await _databaseService.SaveBookInfo(NovelConverterHelper.NovelToConverterBook(novel, 1));
        }
        private async void NavigationCommentPage()
        {
            await NavigationService.NavigateAsync($"{nameof(PostCommentPage)}?ID={_novelId}");
        }
        private async void NavigationSearchChapter()
        {
            var param = new NavigationParameters();
            param.Add("Novel", Novel);
            param.Add("ID", _novelId);
            await NavigationService.NavigateAsync($"{nameof(TableContentPage)}", param);
        }
        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters.ContainsKey("ID"))
            {
                _novelId = int.Parse(parameters["ID"].ToString());
               
            }
            if (_novelId != 0)
            {
                Novel = await _bookService.GetDetailNovel(_novelId);
                var list = await _bookService.GetCommentList(_novelId);
                ListComment = list?.ToList();
                CountReview = ListComment.Count();
                var novelInfo = new DownloadInfo(_novelId);
                _downloadService.InstanceDownloadInfo(ref novelInfo);
                NovelDownloadInfo = novelInfo;
            }
           
        }
    }
}
