using NovelApp.Configurations;
using NovelApp.Models.BookGwModels;
using NovelApp.Models.Enums;
using NovelApp.Services.Book;
using NovelApp.Views.Popup;
using Prism.Commands;
using Prism.Navigation;
using Syncfusion.SfCarousel.XForms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using static NovelApp.Configurations.AppConstants;

namespace NovelApp.ViewModels
{
    public class ReadBookPageViewModel : BaseViewModel
    {
       /* public int CardIndex
        {
            get => cardIndex; set
            {
                if (SetProperty(ref cardIndex, value))
                {
                    if (cardIndex == 1)
                    {

                        PagingMargin = new Thickness(-50, -20, -40, -20);
                        PagingPadding = new Thickness(50, 30, 0, 30);
                    }
                    if (cardIndex == 2)
                    {

                        PagingMargin = new Thickness(-40, -20, -80, -20);
                        PagingPadding = new Thickness(30, 20, 0, 20);
                    }
                    else
                    {
                        PagingMargin = new Thickness(-40, -20, -110, -20);
                        PagingPadding = new Thickness(30);
                    }

                }
            }
        }*/

        public PageType PageTypeShow { get => pageTypeShow; set => SetProperty(ref pageTypeShow, value); }

        //public Thickness PagingPadding { get => pagingPadding; set => SetProperty(ref pagingPadding, value); }
        //public Thickness PagingMargin { get => pagingMargin; set => SetProperty(ref pagingMargin, value); }
        private TextSize _textSize;
        public Chapter ContentChapter { get => contentChapter; set => SetProperty(ref contentChapter, value); }
        public double TextSizeChapter { get => _textSizeChange; set => SetProperty(ref _textSizeChange, value); }
        public ReadMode ShowReadMode
        {
            get => readMode; set => SetProperty(ref readMode, value);
        }
        private bool _isNavigationSettings;
        /// <summary>
        /// index novelid
        /// </summary>
        private int _novelId;
        /// <summary>
        /// index chapter
        /// </summary>
        private int _no;
        private int _countPage;
        public int CountPage { get => _countPage; set => SetProperty(ref _countPage, value); }
        private ObservableCollection<PageChapter> carouselItems;
        private readonly IBookService _bookService;
        public ICommand NavigationSettingsCommand { get; set; }
        public ICommand GoBackCommand { get; set; }
        public ObservableCollection<PageChapter> CarouselItems { get => carouselItems; set => SetProperty(ref carouselItems, value); }
        /// <summary>
        /// danh sach cac kí tự có kích thước nhỏ 
        /// </summary>
        private char[] _arrayCharFilter = new char[] { 'i', 'f', 'l', 'j', 't', 'r', '.', ',', ':', ';', '?', '!' };
        private ReadMode readMode;
        private double _textSizeChange;
        private Chapter contentChapter;
        //private Thickness pagingMargin;
        private int cardIndex;
        //private Thickness pagingPadding;
        private PageType pageTypeShow;

        /// <summary>
        /// Định nghĩa size trong ứng dụng
        /// </summary>
        private Dictionary<TextSize, Dictionary<CharSize, int>> TextSizeMode { get; } = new Dictionary<TextSize, Dictionary<CharSize, int>>()
        {
            { TextSize.Smallest, new Dictionary<CharSize, int>(){

                {CharSize.Normal, 15},
                {CharSize.Small, 10}
            }},
            { TextSize.Smaller, new Dictionary<CharSize, int>(){

                {CharSize.Normal, 20},
                {CharSize.Small, 15}
            }},
            { TextSize.Small, new Dictionary<CharSize, int>(){

                {CharSize.Normal, 25},
                {CharSize.Small, 20}
            }},
            { TextSize.Normal, new Dictionary<CharSize, int>(){

                {CharSize.Normal, 30},
                {CharSize.Small, 25}
            }},
            { TextSize.Large, new Dictionary<CharSize, int>(){

                {CharSize.Normal, 35},
                {CharSize.Small, 30}
            }},
            { TextSize.Largest, new Dictionary<CharSize, int>(){

                {CharSize.Normal, 40},
                {CharSize.Small, 35}
            }},
        };

        public ReadBookPageViewModel(INavigationService navigationService, IBookService bookService) : base(navigationService)
        {
            NavigationSettingsCommand = new DelegateCommand(PopupSettings);
            GoBackCommand = new DelegateCommand(GoBack);
            _bookService = bookService;
            TextSizeChapter = TextSizeMode[TextSize.Normal][CharSize.Normal];
            _textSize = TextSize.Normal;
            PageTypeShow = PageType.OnePage;
            //PagingMargin = new Thickness(-50, -20, -20, -20);
            //PagingPadding = new Thickness(40, 20, 40, 20);

        }
        private void CardTapped(object obj)
        {

        }
        private async void GoBack()
        {
            UnRegisterMessageSettings();
            await NavigationService.GoBackAsync();
        }
        private void UnRegisterMessageSettings()
        {
            MessagingCenter.Unsubscribe<SettingsPopupViewModel, Dictionary<SettingMode, object>>(this, Message.MessageSettings);
        }
        private void RegisterMessageSettings()
        {
            MessagingCenter.Subscribe<SettingsPopupViewModel, Dictionary<SettingMode, object>>(this, Message.MessageSettings, (send, e) =>
            {
                if (e.ContainsKey(SettingMode.ReadMode))
                {
                    ReadMode((ReadMode)e[SettingMode.ReadMode]);
                }
                else if (e.ContainsKey(SettingMode.TextSize))
                {
                    TextSizeReadMode((TextSize)e[SettingMode.ReadMode]);
                }
            });
        }
        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);
            if (!_isNavigationSettings)
                UnRegisterMessageSettings();
        }
        private async void PopupSettings()
        {
            RegisterMessageSettings();
            _isNavigationSettings = true;
            await NavigationService.NavigateAsync($"{nameof(SettingsPopup)}?{AppConstants.NavigationParameter.NoChapter}={_novelId}&{AppConstants.NavigationParameter.NoChapter}={_no}");
        }
        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters.ContainsKey(AppConstants.NavigationParameter.GoBack))
                return;
            if (parameters.ContainsKey(AppConstants.NavigationParameter.NovelId))
                _novelId = int.Parse(parameters[AppConstants.NavigationParameter.NovelId].ToString());
            if (parameters.ContainsKey(AppConstants.NavigationParameter.NoChapter))
                _no = int.Parse(parameters[AppConstants.NavigationParameter.NoChapter].ToString());
            await GetContentChapter(_novelId, _no);
        }
        private async Task GetContentChapter(int novelId, int no)
        {
            ContentChapter = await _bookService.GetContentChapter(_novelId, _no);
        }

        /// <summary>
        /// Phân trang hiển thị chế độ Paging, textsize hiển thị
        /// </summary>
        /// <param name="textSize"></param>
        /// <returns></returns>
        private async Task SplitPage(TextSize textSize = TextSize.Smallest)
        {
            var sizeDic = TextSizeMode[textSize];
            int smallSizeChar = sizeDic[CharSize.Small];
            int normlaSizeChar = sizeDic[CharSize.Normal];
            // Get Metrics
            var mainDisplayInfo = DeviceDisplay.MainDisplayInfo;
            var density = mainDisplayInfo.Density;
            // Width (in pixels)
            var width = mainDisplayInfo.Width / density;

            // Height (in pixels)
            var height = mainDisplayInfo.Height / density;


            try
            {
                var content = await _bookService.GetContentChapter(_novelId, _no);
                var text = content.Text;
                var rowheight = ((int)width - 40);
                var columnHeight = ((int)height - 120);
                var maxRowInPage = columnHeight / normlaSizeChar;
                //dieenj tich hien thi content
                var counttext = rowheight * columnHeight;
                var list = new List<PageChapter>();
                var rowLine = text.Split('\n');
                var row = rowLine.Length;
                //text hiển thị trong một page
                string textPage = "";
                //row hiển thị trong một page
                int rowInPage = 0;
                //số pixel trong một page
                int countPixelPage = 0;
                //Số trang
                int indexPage = 0;
                for (int i = 0; i < row; i++)
                {
                    rowInPage++;
                    var texline = rowLine[i];
                    var listChar = texline.ToCharArray();
                    int countminsize = 0;
                    foreach (var textChar in listChar)
                    {
                        if (_arrayCharFilter.Contains(textChar))
                        {
                            countminsize++;
                        }
                    }
                    countPixelPage += countminsize * smallSizeChar + (texline.Length - countminsize) * normlaSizeChar;
                    textPage += texline;
                    if (countPixelPage > counttext)
                    {
                        string deltatext = "";
                        var deltaPixcel = textPage.Length - counttext;
                        int comparePixcel = 0;
                        for (int j = texline.Length; j >= 0; j--)
                        {
                            char t = texline[j];
                            if (_arrayCharFilter.Contains(t))
                            {
                                comparePixcel += smallSizeChar;
                            }
                            else
                                comparePixcel += normlaSizeChar;
                            if (deltaPixcel < comparePixcel)
                            {
                                break;
                            }
                            else
                            {
                                deltatext += texline[j];
                            }

                        }
                        textPage.Replace(deltatext, "");
                        list.Add(new PageChapter() { Text = textPage, IndexPage = ++indexPage });
                        textPage = deltatext;
                        countPixelPage = 0;
                        rowInPage = 0;

                    }
                    else
                    {
                        textPage += "\n";
                        var k = i + 1;
                        if (k == row || rowInPage == maxRowInPage)
                        {
                            countPixelPage = 0;
                            rowInPage = 0;
                            list.Add(new PageChapter() { Text = textPage, IndexPage = ++indexPage });
                            textPage = "";
                        }

                    }

                }
                CountPage = list.Count;

                list = list.OrderByDescending(x => x.IndexPage).ToList();
                CarouselItems = new ObservableCollection<PageChapter>(list);
                if (CountPage == 1)
                    PageTypeShow = PageType.OnePage;
                else if (CountPage == 2)
                    PageTypeShow = PageType.TwoPages;
                else
                    PageTypeShow = PageType.ThreePages;
            }
            catch (Exception e)
            {

            }
        }
        /// <summary>
        /// Tapping, Scrolling, Paging
        /// </summary>
        private async void ReadMode(ReadMode readMode)
        {
            ShowReadMode = readMode;
            if (ShowReadMode == Models.Enums.ReadMode.Paging)
                await SplitPage(_textSize);
        }
        private async void TextSizeReadMode(TextSize textSize)
        {
            if (ShowReadMode == Models.Enums.ReadMode.Paging)
            {
                await SplitPage(textSize);
            }
            else
            {
                _textSize = textSize;
                TextSizeChapter = TextSizeMode[textSize][CharSize.Normal];
            }
        }
    }
}
