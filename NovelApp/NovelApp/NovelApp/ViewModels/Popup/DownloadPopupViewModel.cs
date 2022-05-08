using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using NovelApp.Bussiness;
using NovelApp.DependencyServices;
using NovelApp.Models.BookGwModels;
using NovelApp.Services.Book;
using NovelApp.Services.DatabaseService;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using Xamarin.Forms;

namespace NovelApp.ViewModels
{
    public class DownloadPopupViewModel : BaseViewModel
    {
        private bool isChecked10;
        private bool isChecked50;
        private bool isChecked100;
        private bool isLocked;
        private int coinBuy;
        private int coinCurrent;

        public bool IsChecked10
        {
            get => isChecked10; set
            {
                if (SetProperty(ref isChecked10, value))
                {
                    if (isChecked10)
                    {
                        IsChecked50 = false;
                        IsChecked100 = false;
                        CoinBuy = -20;
                    }
                }
            }
        }
        public bool IsChecked50
        {
            get => isChecked50; set
            {
                if (SetProperty(ref isChecked50, value))
                {
                    if (isChecked50)
                    {
                        IsChecked10 = false;
                        IsChecked100 = false;
                        CoinBuy = -100;
                    }
                }
            }
        }
        public bool IsChecked100
        {
            get => isChecked100; set
            {
                if (SetProperty(ref isChecked100, value))
                {
                    if (isChecked100)
                    {
                        IsChecked10 = false;
                        IsChecked50 = false;
                        CoinBuy = -200;
                    }
                }
            }
        }
        public bool IsLocked { get => isLocked; set => SetProperty(ref isLocked, value); }
        public ICommand GobackCommand { get; set; }
        public ICommand BacklockedCommand { get; set; }
        public ICommand UnLockChapterCommand { get; set; }
        public ICommand DownloadChapterCommand { get; set; }
        private readonly IBookService _bookService;
        private readonly IDatabaseService _databaseService;
        private readonly IDownloadService _downloadService;
        private readonly IPageDialogService _pageDialogService;
        public DownloadInfo NovelDownloadInfo { get => novelDownloadInfo; set => SetProperty(ref novelDownloadInfo, value); }
        public int CoinBuy { get => coinBuy; set => SetProperty(ref coinBuy, value); }
        public int CoinCurrent { get => coinCurrent; set => SetProperty(ref coinCurrent, value); }
        public DownloadPopupViewModel(INavigationService navigationService, IPageDialogService pageDialog,
            IBookService bookService, IDatabaseService database, IDownloadService downloadService) : base(navigationService)
        {
            _bookService = bookService;
            _databaseService = database;
            _downloadService = downloadService;
            _pageDialogService = pageDialog;
            IsLocked = true;
            GobackCommand = new DelegateCommand(() =>
            {
                navigationService.GoBackAsync();
            });
            BacklockedCommand = new DelegateCommand(() =>
            {
                IsLocked = !IsLocked;
            });
            UnLockChapterCommand = new DelegateCommand(async () =>
            {
                if (CoinBuy + CoinCurrent < 0)
                {
                    await pageDialog.DisplayAlertAsync("Thông báo", "Coin hiện tại không đủ. Vui lòng mua thêm coint", "Ok");
                }
                else
                {
                    // thực hiện thêm thanh toán để unlock 
                }

            });
            DownloadChapterCommand = new DelegateCommand(DownloadChapter);
        }

        private async void DownloadChapter()
        {
            var book = await _databaseService.GetBookInfo(_no);
            bool choice = true;
            if (book!=null)
            {
                if(book.ListType == 3)
                {
                     choice =  await _pageDialogService.DisplayAlertAsync("Thông báo", "Sách đã được tải. Bạn muốn tải lại không?","Ok", "Cancel");
                } 
            }
            if (choice)
            {
                if(NovelDownloadInfo.Status != Models.Enums.StatusDownload.Running)
                {
                    DependencyService.Get<IToastMessage>().Show("Tải sách");
                    _downloadService.Download(_novel, _no);
                    DependencyService.Get<IToastMessage>().Show("Sách đã được tải");
                    GobackCommand.Execute(null);
                }
                else
                {
                    DependencyService.Get<IToastMessage>().Show("Sách đang được được tải");
                }
            }

        }
        private int _no;
        private DownloadInfo novelDownloadInfo;
        private Novel _novel;

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters.ContainsKey("ID"))
            {
                _no = int.Parse(parameters["ID"].ToString());
            }
            if (parameters.ContainsKey("Novel"))
            {
                _novel = (Novel) parameters["Novel"];
            }
            var novelInfo = new DownloadInfo(_no);
            _downloadService.InstanceDownloadInfo(ref novelInfo);
            NovelDownloadInfo = novelInfo;
        }
    }
}
