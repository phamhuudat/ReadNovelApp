using System;
using System.Windows.Input;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;

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
        public int CoinBuy { get => coinBuy; set => SetProperty(ref coinBuy, value); }
        public int CoinCurrent { get => coinCurrent; set => SetProperty(ref coinCurrent, value); }
        public DownloadPopupViewModel(INavigationService navigationService, IPageDialogService pageDialog) : base(navigationService)
        {
            IsLocked = true;
            GobackCommand = new DelegateCommand(() =>
            {
                navigationService.GoBackAsync();
            });
            BacklockedCommand = new DelegateCommand(() =>
            {
                IsLocked = !IsLocked;
            });
            UnLockChapterCommand = new DelegateCommand(async()=> {
                if (CoinBuy + CoinCurrent < 0)
                {
                   await pageDialog.DisplayAlertAsync("Thông báo", "Coin hiện tại không đủ. Vui lòng mua thêm coint", "Ok");
                }
                else
                {

                }

            });
        }
    }
}
