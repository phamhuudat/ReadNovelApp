using NovelApp.Configurations;
using NovelApp.DependencyServices;
using NovelApp.Helpers;
using NovelApp.Models.Enums;
using NovelApp.Services.CacheService;
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
        private readonly ICacheService _cacheService;
        private ObservableCollection<SfSegmentItem> imageTextCollection;
        private bool _isFirstIndexReadMode;

        private int _novelId;
        private int indexReadMode;

        private ReadModelColor selectTextColor;
        private string selectFont;
        private int indexTextSize;
        private Color textColor;
        private float indexBrightness;

        public float IndexBrightness { get => indexBrightness; set => SetProperty(ref indexBrightness, value); }
        public ObservableCollection<SfSegmentItem> ImageTextCollection
        {
            get { return imageTextCollection; }
            set { SetProperty(ref imageTextCollection, value); }
        }
        public Color TextColor { get => textColor; set => SetProperty(ref textColor, value); }
        public int IndexTextSize { get => indexTextSize; set => SetProperty(ref indexTextSize, value); }
        /// <summary>
        /// scrolling, paging, tappings
        /// </summary>
        public int IndexReadMode
        {
            get => indexReadMode; set
            {
                if (SetProperty(ref indexReadMode, value))
                {
                    if (!_isFirstIndexReadMode)
                    {
                        _isFirstIndexReadMode = true;
                        return;
                    }
                    ChangeReadMode((ReadMode)indexReadMode);
                };
            }
        }
        /// <summary>
        /// Font VT, AR, RR
        /// </summary>
        public string SelectFont { get => selectFont; set => SetProperty(ref selectFont, value); }
        /// <summary>
        /// Color bg read book
        /// </summary>
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
        public SettingsPopupViewModel(INavigationService navigationService, ICacheService cacheService) : base(navigationService)
        {
            _cacheService = cacheService;
            GoBackCommand = new DelegateCommand(Goback);
            NavigationListChapterCommand = new DelegateCommand(NavigationListChapter);
            NavigationNovelInforCommand = new DelegateCommand(NavigationNovelInfor);
            ChoiceColorCommand = new DelegateCommand<object>(ChoiceColor);
            SelectFontCommand = new DelegateCommand<object>(ChoiceTextFont);
            ImageTextCollection = new ObservableCollection<SfSegmentItem>
        {
            new SfSegmentItem(){IconFont = FontAwesome.Scroll,
                Text = "SCROLLING"},
            new SfSegmentItem(){IconFont = FontAwesome.LayerGroup,
                Text = "PAGING", },
            new SfSegmentItem(){IconFont = FontAwesome.HandPointUp,
                Text = "TAPPING"} };
            Initial();
        }
        private void Initial()
        {
            IndexBrightness = DependencyService.Get<IBrightnessService>().GetBrightness();
            var textSize = string.IsNullOrEmpty(_cacheService.GetCache(AppConstants.CacheParameter.TextSize)) ?
               TextSize.Small : (TextSize)int.Parse(_cacheService.GetCache(AppConstants.CacheParameter.TextSize));
            IndexTextSize = (int)textSize;
            IndexReadMode = string.IsNullOrEmpty(_cacheService.GetCache(AppConstants.CacheParameter.ReadMode)) ?
               (int)ReadMode.Scrolling : int.Parse(_cacheService.GetCache(AppConstants.CacheParameter.ReadMode));
            SelectFont = string.IsNullOrEmpty(_cacheService.GetCache(AppConstants.CacheParameter.TextFont)) ?
               AppConstants.FontFamily.ArialFont : _cacheService.GetCache(AppConstants.CacheParameter.TextFont);
            SelectTextColor = string.IsNullOrEmpty(_cacheService.GetCache(AppConstants.CacheParameter.TextColor)) ?
            ReadModelColor.White : (ReadModelColor)int.Parse(_cacheService.GetCache(AppConstants.CacheParameter.TextColor));
            TextColor = SelectTextColor == ReadModelColor.Black ? Color.White : Color.Black;
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
