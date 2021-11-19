using NovelApp.Configurations;
using NovelApp.Helpers;
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
        private int _maxLineInPage = 4;
        private Color textColor;
        public double WidthReadPage { get; set; }
        public double ViewReadHeight { get; set; }
        public string TextFontFamily { get => textFontFamily; set => SetProperty(ref textFontFamily, value); }
        public Color TextColor { get => textColor; set => SetProperty(ref textColor, value); }
        public ReadModelColor SelectBgColor { get => selectBgColor; set => SetProperty(ref selectBgColor, value); }
        public ICommand PrevContentCommand { get; set; }
        public ICommand NextContentCommand { get; set; }

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
        private char[] _arrayCharFilter = new char[] { 'i', 'f', 'l', 'j', 't', 'r', '.', ',', ':', ';', '?', '!', '1' };
        private ReadMode readMode;
        private double _textSizeChange;
        private Chapter contentChapter;
        private string contentChapterTap;
        private ReadModelColor selectBgColor;
        private string textFontFamily;

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
            TextSizeChapter = TextSizeHelper.TextSizeMode[_textSize][CharSize.Normal];
            ShowReadMode = string.IsNullOrEmpty(_cacheService.GetCache(AppConstants.CacheParameter.ReadMode)) ?
               Models.Enums.ReadMode.Scrolling : (ReadMode)int.Parse(_cacheService.GetCache(AppConstants.CacheParameter.ReadMode));
            TextFontFamily = string.IsNullOrEmpty(_cacheService.GetCache(AppConstants.CacheParameter.TextFont)) ?
               AppConstants.FontFamily.ArialFont : _cacheService.GetCache(AppConstants.CacheParameter.TextFont);
            SelectBgColor = string.IsNullOrEmpty(_cacheService.GetCache(AppConstants.CacheParameter.TextColor)) ?
            ReadModelColor.White : (ReadModelColor)int.Parse(_cacheService.GetCache(AppConstants.CacheParameter.TextColor));
            TextColor = SelectBgColor == ReadModelColor.Black ? Color.White : Color.Black;

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
                    _cacheService.SaveCache(AppConstants.CacheParameter.ReadMode, ((int)e[SettingMode.ReadMode]).ToString());
                    ReadMode((ReadMode)e[SettingMode.ReadMode]);
                }
                else if (e.ContainsKey(SettingMode.TextSize))
                {
                    _cacheService.SaveCache(AppConstants.CacheParameter.TextSize, ((int)e[SettingMode.TextSize]).ToString());
                    TextSizeReadMode((TextSize)e[SettingMode.TextSize]);
                }
                else if (e.ContainsKey(SettingMode.ReadModeColor))
                {
                    _cacheService.SaveCache(AppConstants.CacheParameter.TextColor, ((int)e[SettingMode.ReadModeColor]).ToString());
                    SelectBgColor = (ReadModelColor)e[SettingMode.ReadModeColor];
                    if (SelectBgColor == ReadModelColor.Black)
                        TextColor = Color.White;
                    else
                        TextColor = Color.Black;
                }
                else if (e.ContainsKey(SettingMode.Font))
                {
                    TextFontFamily = e[SettingMode.Font].ToString();
                    _cacheService.SaveCache(AppConstants.CacheParameter.TextFont, TextFontFamily);
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
            await NavigationService.NavigateAsync($"{nameof(SettingsPopup)}?{AppConstants.NavigationParameter.NovelId}={_novelId}&{AppConstants.NavigationParameter.NoChapter}={_no}");
        }
        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (_isNavigationSettings)
                UnRegisterMessageSettings();
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
            RaisePropertyChanged(nameof(contentChapterTap));
            ReadMode(ShowReadMode);
        }
        private void SplitPage(TextSize textSize)
        {
            try
            {
                var list = new List<PageChapter>();

                var WidthPage = App.DisplayScreenWidth - 40;
                var HeightPage = App.DisplayScreenHeight - 100;
                double charWidth = .5;
                if (textSize == TextSize.Small )
                {
                    charWidth = .4;
                }
                else if(textSize == TextSize.Smallest)
                {
                    charWidth = .419;
                }
                else if (textSize == TextSize.Large)
                    charWidth = .44;
                else  if (textSize == TextSize.Largest)
                    charWidth = .46;

                var fontSize = TextSizeHelper.TextSizeMode[textSize][CharSize.Normal];
                double lineHeight = Device.RuntimePlatform == Device.iOS ||
                                    Device.RuntimePlatform == Device.Android ? 1.3 : 1.4;
                //số dòng trên một trang
                int lineCount = (int)(HeightPage / (lineHeight * fontSize));
                //số kí tự trên một dòng
                int charsPerLine = (int)(WidthPage / (charWidth * fontSize));

                var text = ContentChapter.Text;
                var rowLine = text.Split('\n');
                var row = rowLine.Length;
                //text hiển thị trong một page
                string textPage = "";
                //row hiển thị trong một page
                int rowInPaging = 0;
                //Số trang
                int indexPage = 0;
                for (int i = 0; i < row; i++)
                {
                    var textLine = rowLine[i];
                    var length = textLine.Length;
                    var rowInTextLine = length / charsPerLine;
                    if (length % charsPerLine > 0)
                    {
                        ++rowInTextLine;
                    }
                    rowInPaging += rowInTextLine;

                    if (rowInPaging == lineCount)
                    {
                        textPage += textLine + "\n";
                        list.Add(new PageChapter() { Text = textPage, IndexPage = ++indexPage });
                        textPage = "";
                        rowInPaging = 0;
                    }
                    else if (rowInPaging < lineCount)
                    {
                        textPage += textLine + "\n";
                    }
                    else
                    {
                        var deltaRow = rowInPaging - lineCount;
                        var deltaRowInText = rowInTextLine - deltaRow;
                        if (deltaRowInText > 0)
                        {
                            var deltaText = textLine.Substring(deltaRowInText * charsPerLine);
                            textPage += textLine.Substring(0, deltaRowInText * charsPerLine);
                            list.Add(new PageChapter() { Text = textPage, IndexPage = ++indexPage });
                            textPage = "";
                            rowInPaging = 0;
                            rowLine[i] = deltaText;
                            --i;
                        }
                    }

                }
                if (!string.IsNullOrEmpty(textPage))
                {
                    list.Add(new PageChapter() { Text = textPage, IndexPage = ++indexPage });
                }
                CountPage = list.Count;
                CarouselItems = new ObservableCollection<PageChapter>(list);
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
                TextSizeReadMode(TextSize.Small);
                SetTapReadMode();
            }
        }
        /// <summary>
        /// Set view Tapping
        /// </summary>
        private void SetTapReadMode()
        {
            var width = ((App.DisplayScreenWidth - 40) *2)/3 ;

            // Height (in pixels)
            var height = ((App.DisplayScreenHeight-50) *2)/ 3;
            //Diện thích hiển thị content
            double charWidth = .4;


            var fontSize = TextSizeHelper.TextSizeMode[TextSize.Small][CharSize.Normal];
            double lineHeight = Device.RuntimePlatform == Device.iOS ||
                                Device.RuntimePlatform == Device.Android ? 1.3 : 1.4;
            //số dòng trên một trang
            lineCount = (int)(height / (lineHeight * fontSize));
            //số kí tự trên một dòng
            charsPerLine = (int)(width / (charWidth * fontSize));
            _rowTextTap = ContentChapter.Text.Split('\n');
            _rowLine = _rowTextTap.Count();
            ContentChapterTap = _rowTextTap[0];
            var length = ContentChapterTap.Length;
            var rowInTextLine = length / charsPerLine;
            if (length % charsPerLine > 0)
            {
                ++rowInTextLine;
            }
            countLineInPage = rowInTextLine;
            _indexNextContentTap++;
            rowInPage++;

        }
        private int countLineInPage = 0;
        private int charsPerLine;
        private int lineCount;
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
            if (_indexNextContentTap < _rowLine)
            {
                rowInPage++;
                var text = _rowTextTap[_indexNextContentTap];
                var length = text.Length;
                var rowInTextLine = length / charsPerLine;
                if (length % charsPerLine > 0)
                {
                    ++rowInTextLine;
                }
                countLineInPage += rowInTextLine;
                if(countLineInPage> lineCount|| rowInPage >= _maxLineInPage)
                {
                    countLineInPage = rowInTextLine;
                    _prevContentChapterTapList.Add(ContentChapterTap);
                    //ContentChapterTap
                    rowInPage = 1;
                    ContentChapterTap = text;
                    RaisePropertyChanged(nameof(ContentChapterTap));
                }
                else
                {
                    ContentChapterTap += "\n" + text;
                    RaisePropertyChanged(nameof(ContentChapterTap));
                }
                _indexNextContentTap++;

            }
        }
        private void TextSizeReadMode(TextSize textSize)
        {
            if (ShowReadMode == Models.Enums.ReadMode.Paging)
            {
                _textSize = textSize;
                TextSizeChapter = TextSizeHelper.TextSizeMode[textSize][CharSize.Normal];
                SplitPage(textSize);
            }
            else
            {
                _textSize = textSize;
                TextSizeChapter = TextSizeHelper.TextSizeMode[textSize][CharSize.Normal];
            }
        }
    }
}
