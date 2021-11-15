using NovelApp.Configurations;
using NovelApp.DependencyServices;
using NovelApp.Helpers;
using NovelApp.Models.Enums;
using NovelApp.Views;
using Prism.Commands;
using Prism.Navigation;
using Syncfusion.XForms.Buttons;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;
using static NovelApp.Configurations.AppConstants;

namespace NovelApp.ViewModels
{
    public class SettingsPopupViewModel : BaseViewModel
    {
        private ObservableCollection<SfSegmentItem> imageTextCollection;
        public ObservableCollection<SfSegmentItem> ImageTextCollection
        {
            get { return imageTextCollection; }
            set { SetProperty(ref imageTextCollection ,value); }
        }
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
        private int _novelId;
        private int indexReadMode;
        /// <summary>
        /// Font VT, AR, RR
        /// </summary>
        public string SelectFont { get => selectFont; set => SetProperty(ref selectFont, value); }
        private ReadModelColor selectTextColor;
        private string selectFont;

        public ReadModelColor SelectTextColor { get => selectTextColor; set => SetProperty(ref selectTextColor, value); }
        public ICommand NavigationListChapterCommand { get; set; }
        public ICommand NavigationNovelInforCommand { get; set; }
        public ICommand ChangeReadModeCommand { get; set; }
        public ICommand ChangeColorReadModeCommand { get; set; }
        public ICommand ChangeTextSizeReadModeCommand { get; set; }
        public ICommand ChangeTextFontReadModeCommand { get; set; }
        public ICommand GoBackCommand { get; set; }
        public ICommand ChoiceColorCommand { get; set; }
        public ICommand SelectFontCommand { get; set; }
        public SettingsPopupViewModel(INavigationService navigationService) : base(navigationService)
        {
            GoBackCommand = new DelegateCommand(Goback);
            NavigationListChapterCommand = new DelegateCommand(NavigationListChapter);
            NavigationNovelInforCommand = new DelegateCommand(NavigationNovelInfor);
            ChoiceColorCommand = new DelegateCommand<object>(ChoiceColor);
            SelectTextColor = ReadModelColor.White;
            SelectFont = AppConstants.FontFamily.ArialFont;
            SelectFontCommand = new DelegateCommand<object>(ChoiceTextFont);
            ImageTextCollection = new ObservableCollection<SfSegmentItem>
        {
            new SfSegmentItem(){IconFont = FontAwesome.Scroll, 
                Text = "SCROLLING"},
            new SfSegmentItem(){IconFont = FontAwesome.LayerGroup, 
                Text = "PAGING"},
            new SfSegmentItem(){IconFont = FontAwesome.HandPointUp, 
                Text = "TAPPING"} };
        }
        private void ChoiceTextFont(object obj)
        {
            SelectFont = obj.ToString();
            ChangeTextFontReadMode(SelectFont);
        }
        private void ChoiceColor(object textColor)
        {
            SelectTextColor = (ReadModelColor)int.Parse(textColor.ToString());
            ChangeColorReadMode(SelectTextColor);
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
        public void ChangeTextSizeReadMode(TextSize textSize)
        {
            MessagingCenter.Send(this, Message.MessageSettings, new Dictionary<SettingMode, object>() {

                {SettingMode.TextSize,
                   textSize
                }
            });
        }
        private void ChangeTextFontReadMode(string textFont)
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
