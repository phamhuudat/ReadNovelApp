using NovelApp.Configurations;
using NovelApp.Helpers;
using NovelApp.Models.BookGwModels;
using NovelApp.Models.Enums;
using NovelApp.Services.Book;
using NovelApp.Services.CacheService;
using NovelApp.Views.Popup;
using Prism.Commands;
using Prism.Navigation;
using Syncfusion.ListView.XForms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
        //private double _areaTextTap;
        private List<string> _rowTextTap;
        private int _rowLine;
        private int _indexPrevContentTap = -1;
        private int _indexNextContentTap = 0;
        private int rowInPage = 0;
        //private int countPixelInPage;
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
        public ObservableCollection<Chapter> ListChaptersScroll { get; set; }
        private readonly IBookService _bookService;
        private readonly ICacheService _cacheService;
        public bool IsIsLoading { get => isIsLoading; set => SetProperty(ref isIsLoading, value); }
        public ICommand NavigationSettingsCommand { get; set; }
        public ICommand GoBackCommand { get; set; }
        /// <summary>
        /// Load khi ấn footer
        /// </summary>
        public ICommand LoadNextChapterCommand { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public ICommand LoadMoreCommand { get; set; }
        public ObservableCollection<PageChapter> CarouselItems { get => carouselItems; set => SetProperty(ref carouselItems, value); }
        /// <summary>
        /// danh sach cac kí tự có kích thước nhỏ 
        /// </summary>
        private char[] _arrayCharFilter = new char[] { '.', ',', ':', ';', '!', '\'', '\"', 'i', 'l', 'j', 't', 'r' };
        private ReadMode readMode;
        private double _textSizeChange;
        private Chapter contentChapter;
        private string contentChapterTap;
        private ReadModelColor selectBgColor;
        private string textFontFamily;
        public double Height
        {
            get => height; set
            {
                SetProperty(ref height, value);
            }
        }
        public string TextCal
        {
            get => textCal;
            set => SetProperty(ref textCal, value);

        }
        /// <summary>
        /// dùng trong tính toán phân trang
        /// </summary>
        private TextFont TextFont
        {
            get
            {

                if (TextFontFamily == AppConstants.FontFamily.ArialFont)
                    return TextFont.Arial;
                else if (TextFontFamily == AppConstants.FontFamily.RobotoFont)
                    return TextFont.Roboto;
                else
                    return TextFont.VnTime;
            }
        }
        public ReadBookPageViewModel(INavigationService navigationService, IBookService bookService,
            ICacheService cacheService
            ) : base(navigationService)
        {
            _bookService = bookService;
            _cacheService = cacheService;
            NavigationSettingsCommand = new DelegateCommand(PopupSettings);
            GoBackCommand = new DelegateCommand(GoBack);
            PrevContentCommand = new DelegateCommand(PrevContent);
            NextContentCommand = new DelegateCommand(() => NextContent());
            LoadNextChapterCommand = new DelegateCommand<object>(LoadMore);
            ListChaptersScroll = new ObservableCollection<Chapter>();
            LoadMoreCommand = new DelegateCommand<object>(LoadMore);
            GetCache();
        }
        /// <summary>
        /// Load more scrollview
        /// </summary>
        /// <param name="obj"></param>
        private async void LoadMore(object obj)
        {
            var listView = obj as Syncfusion.ListView.XForms.SfListView;
            if (listView == null) return;
            try
            {
                if (ListChaptersScroll == null || !ListChaptersScroll.Any())
                    return;
                listView.IsBusy = true;
                await Task.Delay(1000);
                await GetContentChapter(_novelId, ++_no);
                (listView.LayoutManager as LinearLayout).ScrollToRowIndex(ListChaptersScroll.Count - 1);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
            }
            listView.IsBusy = false;
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
            ReadMode(ShowReadMode);

        }
        /// <summary>
        /// Load more data tapping
        /// </summary>
        /// <returns></returns>
        private async Task<bool> GetContentChapter()
        {
            ContentChapter = await _bookService.GetContentChapter(_novelId, ++_no);
            return ContentChapter != null;
        }
        private async Task<bool> GetContentChapter(int novelId, int no)
        {
            var isSuccess = false;
            ContentChapter = await _bookService.GetContentChapter(_novelId, _no);
            if (ContentChapter != null)
            {
                isSuccess = true;
                ListChaptersScroll.Add(ContentChapter);
            }
            return isSuccess;
        }
        public void SplitPage(double height)
        {
            if (ContentChapter == null || string.IsNullOrWhiteSpace(ContentChapter.Text))
                return;
            var fontSize = TextSizeHelper.TextSizeMode[_textSize][CharSize.Normal];
            var text = ContentChapter.Text;
            var list = new List<PageChapter>();
            //659.428571428571
            //var widthPage = App.DisplayScreenWidth - 40;
            //Chiều cao của page
            var heightPage = App.DisplayScreenHeight - 80;
            double lineHeightInPage = Device.RuntimePlatform == Device.iOS ||
                                    Device.RuntimePlatform == Device.Android ? 1.35 : 1.4;
            //số dòng trên một trang
            double lineCountInPage = heightPage / (lineHeightInPage * fontSize);
            //Tổng số line count in novel
            double lineCount = Math.Ceiling(height / (lineHeightInPage * fontSize));

            var countPage = Math.Ceiling(lineCount / lineCountInPage);
            var indexPage = 0;
            double prev = 0;
            for (int i = 0; i < countPage; i++)
            {
                list.Add(new PageChapter()
                {
                    Text = text,
                    IndexPage = ++indexPage,
                    MaxLines = (i + 1) * (int)lineCountInPage
                    ,
                    Coordinates = i == 0 ? 5 : i * ((int)lineCountInPage) * lineHeightInPage * fontSize + (i >= 2 ? 10 : 5)
                });
            }
            CountPage = list.Count;
            CarouselItems = new ObservableCollection<PageChapter>(list);
        }
        private void SplitPage()
        {
            try
            {
                //Phân trang chapter
                var list = new List<PageChapter>();

                var WidthPage = App.DisplayScreenWidth - 50;
                var HeightPage = App.DisplayScreenHeight - 100;
                var fontSize = TextSizeHelper.TextSizeMode[_textSize][CharSize.Normal];
                //tỉ lệ độ cao tương ứng
                double ratioHeight = TextSizeHelper.TextHeightRatio[TextFont];

                //số dòng trên một trang
                int lineCount = (int)(HeightPage / (ratioHeight * fontSize));

                var text = ContentChapter.Text;
                var rowLine = text.Split('\n');
                var row = rowLine.Length;
                //text hiển thị trong một page
                string textPage = "";
                //row hiển thị trong một page
                int rowInPaging = 0;
                //Số trang
                int indexPage = 0;

                double widthSpace = TextSizeHelper.TextWidthRatio[TextFont][TextSizeHelper.CharDownWidthSmallest] * fontSize;
                for (int i = 0; i < row; i++)
                {
                    var textLine = rowLine[i];
                    var words = textLine.Split(' ');
                    var countWord = words.Length;
                    string wordInRow = "";
                    //số độ rộng tính từng dòng
                    double countWidthInRow = 0;
                    for (int j = 0; j < countWord; j++)
                    {
                        var word = words[j].Trim();
                        countWidthInRow += TextSizeHelper.GetWidthWord($"{word}", _textSize, TextFont);
                        if (countWidthInRow < WidthPage && countWidthInRow + widthSpace <= WidthPage)
                        {
                            wordInRow += $"{word} ";
                            countWidthInRow += widthSpace;
                        }
                        else
                        {
                            j--;
                            rowInPaging++;
                            // Debug.WriteLine($"Page {indexPage} \n row {rowInPaging} {wordInRow}\n");
                            countWidthInRow = 0;
                            textPage += wordInRow;
                            if (rowInPaging == lineCount)
                            {
                                list.Add(new PageChapter()
                                {
                                    Text = textPage,
                                    IndexPage = ++indexPage,
                                });
                                textPage = "";
                                wordInRow = "";
                                rowInPaging = 0;
                            }
                            else
                            {
                                //Debug.WriteLine($"Row {rowInPaging} page {indexPage +1} \n {wordInRow} \n");
                                wordInRow = "";
                            }
                        }
                    }
                    textPage += wordInRow + "\n";
                    if (!string.IsNullOrEmpty(wordInRow))
                    {
                        rowInPaging++;
                        if (rowInPaging == lineCount)
                        {
                            list.Add(new PageChapter()
                            {
                                Text = textPage,
                                IndexPage = ++indexPage,
                            });
                            textPage = "";
                            wordInRow = "";
                            rowInPaging = 0;
                        }
                    }


                    if ((i + 1) == row)
                    {
                        if (!string.IsNullOrEmpty(textPage))
                        {
                            list.Add(new PageChapter()
                            {
                                Text = textPage,
                                IndexPage = ++indexPage,
                            });
                        }
                    }

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
            TextCal = ContentChapter.Text;
            if (ShowReadMode == Models.Enums.ReadMode.Paging)
                SplitPage();
            else
            if (ShowReadMode == Models.Enums.ReadMode.Tapping)
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
            var width = (App.DisplayScreenWidth - 40) * 2 / 3;

            // Height (in pixels)
            var height = (App.DisplayScreenHeight - 50) * 2 / 3;
            //Diện thích hiển thị content
            double charWidth = .4;
            var fontSize = TextSizeHelper.TextSizeMode[TextSize.Small][CharSize.Normal];
            double lineHeight = Device.RuntimePlatform == Device.iOS ||
                                Device.RuntimePlatform == Device.Android ? 1.3 : 1.4;
            //số dòng trên một trang
            lineCount = (int)(height / (lineHeight * fontSize));
            //số kí tự trên một dòng
            charsPerLine = (int)(width / (charWidth * fontSize));
            _rowTextTap = new List<string>(ContentChapter.Text.Split('\n').ToList());
            _rowLine = _rowTextTap.Count();
            ContentChapterTap = ContentChapter.Name;
            var length = ContentChapterTap.Length;
            var rowInTextLine = length / charsPerLine;
            if (length % charsPerLine > 0)
            {
                ++rowInTextLine;
            }
            countLineInPage = rowInTextLine;
            _indexNextContentTap = 0;
            rowInPage++;
        }
        private async Task<bool> SetTapLoadMoreReadMode()
        {
            var result = await GetContentChapter();
            if (result)
            {
                _rowTextTap.Add(ContentChapter.Name);
                _rowTextTap.AddRange(ContentChapter.Text.Split('\n').ToList());
                _rowLine = _rowTextTap.Count();
            }
            return result;
        }
        private int countLineInPage = 0;
        private int charsPerLine;
        private int lineCount;
        private double height;
        private string textCal;
        private bool isIsLoading;

        /// <summary>
        /// Xử lý back content
        /// </summary>
        private void PrevContent()
        {
            if (_prevContentChapterTapList.Any())
            {
                if (isNextcontent)
                {
                    _prevContentChapterTapList.Add(ContentChapterTap);
                }
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
        bool isNextcontent = false;
        /// <summary>
        /// Xử lý next content
        /// </summary>
        private async void NextContent(bool isAutoNext = false)
        {
            
            if (_indexPrevContentTap >= 0 && _prevContentChapterTapList.Any() && _indexPrevContentTap < _prevContentChapterTapList.Count - 1)
            {
                ContentChapterTap = _prevContentChapterTapList[++_indexPrevContentTap];
                return;
            }
            isNextcontent = true;
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
                if (countLineInPage > lineCount || rowInPage >= _maxLineInPage || isAutoNext)
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
            else
            {
                var isSuccess = await SetTapLoadMoreReadMode();
                if (isSuccess) NextContent(true);
            }
        }
        private void TextSizeReadMode(TextSize textSize)
        {
            if (ShowReadMode == Models.Enums.ReadMode.Paging)
            {
                _textSize = textSize;
                TextSizeChapter = TextSizeHelper.TextSizeMode[textSize][CharSize.Normal];
                SplitPage();
                RaisePropertyChanged(nameof(TextColor));
            }
            else
            {
                _textSize = textSize;
                TextSizeChapter = TextSizeHelper.TextSizeMode[textSize][CharSize.Normal];
            }
        }
    }
}
