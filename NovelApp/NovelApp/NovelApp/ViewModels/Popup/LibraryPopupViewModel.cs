using System;
using System.Windows.Input;
using NovelApp.Bussiness;
using NovelApp.Configurations;
using NovelApp.DependencyServices;
using NovelApp.Models.BookGwModels;
using NovelApp.Services.Book;
using NovelApp.Services.DatabaseService;
using NovelApp.Views;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using Xamarin.Forms;

namespace NovelApp.ViewModels
{
    public class LibraryPopupViewModel: BaseViewModel
    {
        private int _no;
        private DownloadInfo novelDownloadInfo;
        private Novel _novel;
        private readonly IBookService _bookService;
        private readonly IDatabaseService _databaseService;
        private readonly IDownloadService _downloadService;
        private readonly IPageDialogService _pageDialogService;
        public DownloadInfo NovelDownloadInfo { get => novelDownloadInfo; set => SetProperty(ref novelDownloadInfo, value); }
        public ICommand GobackCommand { get; set; }
        public ICommand RemoveBookCommand { get; set; }
        public ICommand DownloadChapterCommand { get; set; }
        public ICommand NavigationNovelInforCommand { get; set; }
        public LibraryPopupViewModel(INavigationService navigationService, IPageDialogService pageDialog,
            IBookService bookService, IDatabaseService database, IDownloadService downloadService) : base(navigationService)
        {
            _bookService = bookService;
            _databaseService = database;
            _downloadService = downloadService;
            _pageDialogService = pageDialog;
            RemoveBookCommand = new DelegateCommand(RemoveBook);
            DownloadChapterCommand = new DelegateCommand(DownloadBook);
            NavigationNovelInforCommand = new DelegateCommand(NavigationNovelInfor);
            GobackCommand = new DelegateCommand(async() =>
            {
               await NavigationService.GoBackAsync();
            });
        }
        private async void NavigationNovelInfor()
        {
            await NavigationService.NavigateAsync($"{nameof(BookDetailPage)}?{AppConstants.NavigationParameter.NovelId}={_no}");
        }
        private async  void RemoveBook()
        {
           var choice = await _pageDialogService.DisplayAlertAsync("Thông báo", $"Bạn có nuốn xóa \"{_novel.Name}\" khỏi bộ nhớ", "Ok", "Cancel");
            if (choice)
            {
               await _databaseService.RemoveBook(_no);
               DependencyService.Get<IToastMessage>().Show("Sách đã được xóa");
               GobackCommand.Execute(null);
            }
            
        }
        private async void DownloadBook()
        {
            var book = await _databaseService.GetBookInfo(_no);
            bool choice = true;
            if (book != null)
            {
                if (book.ListType == 3)
                {
                    choice = await _pageDialogService.DisplayAlertAsync("Thông báo", "Sách đã được tải. Bạn muốn tải lại không?", "Ok", "Cancel");
                }
            }
            if (choice)
            {
                if (NovelDownloadInfo.Status != Models.Enums.StatusDownload.Running)
                {
                    DependencyService.Get<IToastMessage>().Show("Tải sách");
                    _downloadService.Download(_novel, _no);
                    DependencyService.Get<IToastMessage>().Show("Sách đã được tải");
                    //GobackCommand.Execute(null);
                }
                else
                {
                    DependencyService.Get<IToastMessage>().Show("Sách đang được tải");
                }
            }
        }
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters.ContainsKey("ID"))
            {
                _no = int.Parse(parameters["ID"].ToString());
            }
            if (parameters.ContainsKey("Novel"))
            {
                _novel = (Novel)parameters["Novel"];
            }
            var novelInfo = new DownloadInfo(_no);
            _downloadService.InstanceDownloadInfo(ref novelInfo);
            NovelDownloadInfo = novelInfo;
        }
    }
}
