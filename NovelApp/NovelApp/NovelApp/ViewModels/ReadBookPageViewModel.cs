using NovelApp.Configurations;
using NovelApp.Models.BookGwModels;
using NovelApp.Models.Enums;
using NovelApp.Services.Book;
using NovelApp.Services.CacheService;
using NovelApp.Views.Popup;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using static NovelApp.Configurations.AppConstants;

namespace NovelApp.ViewModels
{
    public class ReadBookPageViewModel : BaseViewModel
    {
        //Tapping-------------------------------------
        private List<string> _prevContentChapterTapList = new List<string>();
        /// <summary>
        /// content hieen thi trong mod tap
        /// </summary>
        public string ContentChapterTap { get => contentChapterTap; set => SetProperty(ref contentChapterTap, value); }
        private double _areaTextTap;
        private string[] _rowTextTap;
        private int _rowLine;
        private int _indexPrevContentTap = -1;
        private int _indexNextContentTap = 0;
        private int rowInPage = 0;
        private int countPixelInPage;
        private int _maxLineInPage = 3;
        private Color textColor;
        public string TextFontFamily { get => textFontFamily; set => SetProperty(ref textFontFamily, value); }
        public Color TextColor { get => textColor; set => SetProperty(ref textColor, value); }
        public ReadModelColor SelectBgColor { get => selectBgColor; set => SetProperty(ref selectBgColor, value); }
        public ICommand PrevContentCommand { get; set; }
        public ICommand NextContentCommand { get; set; }

        /// <summary>
        /// margin/padding in paging
        /// </summary>
        ///
        public PageType PageTypeShow { get => pageTypeShow; set => SetProperty(ref pageTypeShow, value); }
        private TextSize _textSize;
        public Chapter ContentChapter { get => contentChapter; set => SetProperty(ref contentChapter, value); }
        public double TextSizeChapter { get => _textSizeChange; set => SetProperty(ref _textSizeChange, value); }
        /// <summary>
        /// Mode show scrolling, paging, tapping
        /// </summary>
        public ReadMode ShowReadMode
        {
            get => readMode; set => SetProperty(ref readMode, value);
        }
        /// <summary>
        /// key register message settings
        /// </summary>
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
        private readonly ICacheService _cacheService;
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
        private PageType pageTypeShow;
        private string contentChapterTap;
        private ReadModelColor selectBgColor;
        private string textFontFamily;

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

        public ReadBookPageViewModel(INavigationService navigationService, IBookService bookService,
            ICacheService cacheService
            ) : base(navigationService)
        {
            _bookService = bookService;
            _cacheService = cacheService;
            NavigationSettingsCommand = new DelegateCommand(PopupSettings);
            GoBackCommand = new DelegateCommand(GoBack);
            PrevContentCommand = new DelegateCommand(PrevContent);
            NextContentCommand = new DelegateCommand(NextContent);
            GetCache();
        }
        private void GetCache()
        {
            _textSize = string.IsNullOrEmpty(_cacheService.GetCache(AppConstants.CacheParameter.TextSize)) ?
               TextSize.Small : (TextSize)int.Parse(_cacheService.GetCache(AppConstants.CacheParameter.TextSize));
            TextSizeChapter = TextSizeMode[_textSize][CharSize.Normal];
            PageTypeShow = string.IsNullOrEmpty(_cacheService.GetCache(AppConstants.CacheParameter.PageType)) ?
               PageType.OnePage : (PageType)int.Parse(_cacheService.GetCache(AppConstants.CacheParameter.PageType));
            TextFontFamily = string.IsNullOrEmpty(_cacheService.GetCache(AppConstants.CacheParameter.TextFont)) ?
               AppConstants.FontFamily.ArialFont : _cacheService.GetCache(AppConstants.CacheParameter.TextFont);
            //SelectBgColor = string.IsNullOrEmpty(_cacheService.GetCache(AppConstants.CacheParameter.TextColor)) ?
            //   Color.Black :Color.FromHex(_cacheService.GetCache(AppConstants.CacheParameter.TextColor));
        }
        public async void GoBack()
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
                    _cacheService.SaveCache(AppConstants.CacheParameter.ReadMode,( (int)(ReadMode)e[SettingMode.ReadMode]).ToString());
                    ReadMode((ReadMode)e[SettingMode.ReadMode]);
                }
                else if (e.ContainsKey(SettingMode.TextSize))
                {
                    _cacheService.SaveCache(AppConstants.CacheParameter.TextSize, ((int)(TextSize)e[SettingMode.TextSize]).ToString());
                    TextSizeReadMode((TextSize)e[SettingMode.TextSize]);
                }
                else if (e.ContainsKey(SettingMode.ReadModeColor))
                {
                    _cacheService.SaveCache(AppConstants.CacheParameter.TextSize, ((int)(TextSize)e[SettingMode.TextSize]).ToString());
                    SelectBgColor = (ReadModelColor)e[SettingMode.ReadModeColor];
                    if (SelectBgColor == ReadModelColor.Black)
                        TextColor = Color.White;
                    else
                        TextColor = Color.Black;
                }
                else if (e.ContainsKey(SettingMode.Font))
                {
                    TextFontFamily = e[SettingMode.Font].ToString();
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
        private void SplitPage(TextSize textSize = TextSize.Smallest)
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
                var text = ContentChapter.Text;
                var rowheight = ((int)width - 60);
                var columnHeight = ((int)height - 100);
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
        private void ReadMode(ReadMode readMode)
        {
            ShowReadMode = readMode;
            if (ShowReadMode == Models.Enums.ReadMode.Paging)
                SplitPage(_textSize);
            else if (ShowReadMode == Models.Enums.ReadMode.Tapping)
            {
                SetTapReadMode();
            }
        }
        /// <summary>
        /// Set view Tapping
        /// </summary>
        private void SetTapReadMode()
        {
            //tinhs do rong view hien thi
            // Get Metrics
            var mainDisplayInfo = DeviceDisplay.MainDisplayInfo;
            var density = mainDisplayInfo.Density;
            // Width (in pixels)
            var width = (mainDisplayInfo.Width / density) / (2 / 3);

            // Height (in pixels)
            var height = (mainDisplayInfo.Height / density) / (2 / 3);
            //Diện thích hiển thị content
            _areaTextTap = width * height;

            _rowTextTap = ContentChapter.Text.Split('\n');
            _rowLine = _rowTextTap.Count();
            ContentChapterTap = _rowTextTap[0];
            _indexNextContentTap++;
            rowInPage++;

        }

        /// <summary>
        /// Xử lý back content
        /// </summary>
        private void PrevContent()
        {
            if (_prevContentChapterTapList.Any())
            {
                var leng = _prevContentChapterTapList.Count;
                if (_indexPrevContentTap == -1)
                {
                    ContentChapterTap = _prevContentChapterTapList.Last();
                    _indexPrevContentTap = leng - 1;
                }
                else if (_indexPrevContentTap > 0)
                {
                    ContentChapterTap = _prevContentChapterTapList[--_indexPrevContentTap];
                }
            }

        }
        /// <summary>
        /// Xử lý next content
        /// </summary>
        private void NextContent()
        {
            if (_indexPrevContentTap > 0 && _prevContentChapterTapList.Any() && _indexPrevContentTap < _prevContentChapterTapList.Count - 1)
            {
                ContentChapterTap = _prevContentChapterTapList[++_indexPrevContentTap];
                return;
            }
            var sizeDic = TextSizeMode[TextSize.Normal];
            int smallSizeChar = sizeDic[CharSize.Small];
            int normlaSizeChar = sizeDic[CharSize.Normal];
            if (_indexNextContentTap < _rowLine)
            {
                rowInPage++;
                var text = _rowTextTap[_indexNextContentTap];
                var listChar = text.ToCharArray();
                int countminsize = 0;
                foreach (var textChar in listChar)
                {
                    if (_arrayCharFilter.Contains(textChar))
                    {
                        countminsize++;
                    }
                }
                var buffPixelInPage = countminsize * smallSizeChar + (text.Length - countminsize) * normlaSizeChar;
                countPixelInPage += buffPixelInPage;
                if (countPixelInPage > _areaTextTap)
                {
                    countPixelInPage = 0;
                    _prevContentChapterTapList.Add(ContentChapterTap);
                    ContentChapterTap = text;
                }
                else
                {
                    if (rowInPage == _maxLineInPage)
                    {
                        ContentChapterTap += "\n" + text;
                        RaisePropertyChanged(nameof(ContentChapterTap));
                        countPixelInPage = 0;
                    }
                    else if (rowInPage > _maxLineInPage)
                    {
                        rowInPage = 1;
                        countPixelInPage = buffPixelInPage;
                        _prevContentChapterTapList.Add(ContentChapterTap);
                        ContentChapterTap = text;
                    }
                    else
                    {
                        ContentChapterTap += "\n" + text;
                        RaisePropertyChanged(nameof(ContentChapterTap));
                    }
                    _indexNextContentTap++;
                }
            }
        }
        private void TextSizeReadMode(TextSize textSize)
        {
            if (ShowReadMode == Models.Enums.ReadMode.Paging)
            {
                _textSize = textSize;
                TextSizeChapter = TextSizeMode[textSize][CharSize.Normal];
                SplitPage(textSize);
            }
            else
            {
                _textSize = textSize;
                TextSizeChapter = TextSizeMode[textSize][CharSize.Normal];
            }
        }
    }
}
