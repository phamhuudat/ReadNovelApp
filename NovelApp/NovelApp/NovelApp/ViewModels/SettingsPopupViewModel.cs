using NovelApp.Configurations;
using NovelApp.Models.Enums;
using NovelApp.Views;
using Prism.Commands;
using Prism.Navigation;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;
using static NovelApp.Configurations.AppConstants;

namespace NovelApp.ViewModels
{
    public class SettingsPopupViewModel : BaseViewModel
    {
        public int IndexReadMode
        {
            get => indexReadMode; set
            {
                if (SetProperty(ref indexReadMode, value))
                {
                    ChangeReadMode((ReadMode)indexReadMode);
                };
            }
        }
        private int Brightness
        {
            get => brightness; set
            {
               if(SetProperty(ref brightness, value))
                {

                }
            }
        } 
        private int _novelId;
        private int indexReadMode;
        private int brightness;

        public ICommand NavigationListChapterCommand { get; set; }
        public ICommand NavigationNovelInforCommand { get; set; }
        public ICommand ChangeReadModeCommand { get; set; }
        public ICommand ChangeColorReadModeCommand { get; set; }
        public ICommand ChangeTextSizeReadModeCommand { get; set; }
        public ICommand ChangeTextFontReadModeCommand { get; set; }
        public ICommand GoBackCommand { get; set; }
        public SettingsPopupViewModel(INavigationService navigationService) : base(navigationService)
        {
            GoBackCommand = new DelegateCommand(Goback);
            NavigationListChapterCommand = new DelegateCommand(NavigationListChapter);
            NavigationNovelInforCommand = new DelegateCommand(NavigationNovelInfor);
            //ChangeReadModeCommand = new DelegateCommand<ReadMode>(ChangeReadMode);
            //ChangeColorReadModeCommand = new DelegateCommand<ReadModelColor>(ChangeColorReadMode);
            //ChangeTextSizeReadModeCommand = new DelegateCommand<TextSize>(ChangeTextSizeReadMode);
            //ChangeTextFontReadModeCommand = new DelegateCommand<TextFont>(ChangeTextFontReadMode);
        }
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters.ContainsKey(AppConstants.NavigationParameter.NovelId))
            {
                _novelId = int.Parse(parameters[AppConstants.NavigationParameter.NovelId].ToString());
            }
        }
        private async void NavigationListChapter()
        {
            await NavigationService.NavigateAsync($"{nameof(TableContentPage)}?{AppConstants.NavigationParameter.NovelId}={_novelId}");
        }
        private async void NavigationNovelInfor()
        {
            await NavigationService.NavigateAsync($"{nameof(BookDetailPage)}?{AppConstants.NavigationParameter.NovelId}={_novelId}");
        }
        private void ChangeReadMode(ReadMode readMode)
        {
            MessagingCenter.Send(this, Message.MessageSettings, new Dictionary<SettingMode, object>() {

                {SettingMode.ReadMode,
                   readMode
                }
            });
        }
        private void ChangeColorReadMode(ReadModelColor readModel)
        {
            MessagingCenter.Send(this, Message.MessageSettings, new Dictionary<SettingMode, object>() {

                {SettingMode.ReadModeColor,
                  readModel
                }
            });
        }
        private void ChangeTextSizeReadMode(TextSize textSize)
        {
            MessagingCenter.Send(this, Message.MessageSettings, new Dictionary<SettingMode, object>() {

                {SettingMode.TextSize,
                  textSize
                }
            });
        }
        private void ChangeTextFontReadMode(TextFont textFont)
        {
            MessagingCenter.Send(this, Message.MessageSettings, new Dictionary<SettingMode, object>() {

                {SettingMode.Font,
                  textFont
                }
            });
        }
        private async void Goback()
        {
            var param = new NavigationParameters();
            param.Add(AppConstants.NavigationParameter.GoBack, 1);
            await NavigationService.GoBackAsync(param);
        }

    }
}
