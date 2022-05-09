using NovelApp.Helpers;
using NovelApp.Models.BookGwModels;
using NovelApp.Services.Book;
using NovelApp.Services.DatabaseService;
using NovelApp.Views;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace NovelApp.ViewModels
{
    public class TableContentPageViewModel : BaseViewModel
    {
        private readonly IDatabaseService _databaseService;
        private readonly IPageDialogService _dialogService;
        private readonly IBookService _bookService;
        private List<ChapInfo> listChapter;
        private int _novelId;
        private int countChapter;
        private bool isSortDown;
        /// <summary>
        /// Lưu danh sách chapter ở local
        /// </summary>
        private List<ChapInfo> _staticListChapter;
        public List<ChapInfo> ListChapter { get => listChapter; set => SetProperty(ref listChapter, value); }
        public int CountChapter { get => countChapter; set => SetProperty(ref countChapter, value); }
        public ICommand SortCommand { get; set; }
        public ICommand ItemTappedCommand { get; set; }
        public ICommand GobackCommand { get; set; }
        public ICommand SearchCommand { get; set; }
        public bool IsSortDown { get => isSortDown; set => SetProperty(ref isSortDown, value); }

        public TableContentPageViewModel(INavigationService navigationService, IBookService bookService, IPageDialogService dialogService,
            IDatabaseService databaseService) : base(navigationService)
        {
            _bookService = bookService;
            _dialogService = dialogService;
            _databaseService = databaseService;
            SortCommand = new DelegateCommand(Sort);
            ItemTappedCommand = new DelegateCommand<object>(ItemTapped);
            SearchCommand = new DelegateCommand<string>(SearchNovel);
            GobackCommand = new DelegateCommand(async () =>
            {
                await NavigationService.GoBackAsync();
            });
        }
        private void SearchNovel(string search)
        {
            try
            {
                if (string.IsNullOrEmpty(search))
                {
                    ListChapter = new List<ChapInfo>(_staticListChapter);
                }
                else if (_staticListChapter != null && _staticListChapter.Any())
                {
                    var list = new List<ChapInfo>();
                    search = search.ToUpper();
                    if (isSortDown)
                    {
                        var listBuffDown = _staticListChapter.Where(x => x.Name.ToUpper().Contains(search) || x.No.ToString().Contains(search)).OrderByDescending(x => x.No);
                        if (listBuffDown != null && listBuffDown.Any())
                            list = listBuffDown.ToList();
                    }
                    else
                    {
                        var listBuffUp = _staticListChapter.Where(x => x.Name.ToUpper().Contains(search) || x.No.ToString().Contains(search)).OrderBy(x => x.No);
                        if (listBuffUp != null && listBuffUp.Any())
                            list = listBuffUp.ToList();
                    }
                    ListChapter = new List<ChapInfo>(list);
                }
            }
            catch (Exception e)
            {
                ListChapter = new List<ChapInfo>(0);
            }

        }
        private async void ItemTapped(object obj)
        {
            var listView = obj as Syncfusion.ListView.XForms.SfListView;
            if (listView == null) return;
            var item = listView.SelectedItem as ChapInfo;
            listView.SelectedItem = null;
            try
            {
                if (item.Type == 1)
                {
                    var result = await _bookService.UnlockChapter(_novelId, item.No, "test@email.com");
                    if (result != null && result.Any())
                    {
                        var errorcode = int.Parse(result[0].ToString());
                        if (errorcode == 0)
                        {
                            await _dialogService.DisplayAlertAsync("Notification", result[1].ToString(), "OK");
                            return;
                        }
                    }
                    else
                    {
                        await _dialogService.DisplayAlertAsync("Notification", "Error network!!!", "OK");
                        return;
                    }

                }
                BookInfo bookInfo;
                if (_novel == null)
                {
                    bookInfo = await _databaseService.GetBookInfo(_novelId);
                }
                else
                    bookInfo = NovelConverterHelper.NovelToConverterBook(_novel, 1);
                await _databaseService.SaveBookInfo(bookInfo);
                await NavigationService.NavigateAsync($"{nameof(ReadBookPage)}?ID={_novelId}&NO={item.No}");
            }
            catch (Exception ex)
            {

            }

        }

        private void Sort()
        {
            IsSortDown = !IsSortDown;
            if (ListChapter != null && ListChapter.Any())
            {
                if (IsSortDown)
                    ListChapter = ListChapter.OrderByDescending(x => x.No).ToList();
                else
                    ListChapter = ListChapter.OrderBy(x => x.No).ToList();
            }
        }
        private Novel _novel;
        public async override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters.ContainsKey("ID"))
                _novelId = int.Parse(parameters["ID"].ToString());
            if (parameters.ContainsKey("Novel"))
                _novel = (Novel)parameters["Novel"];
            var chapterInfo = await _bookService.GetTBC(_novelId);
            var list = new List<ChapInfo>();
            if (chapterInfo != null && chapterInfo.Chapters.Any())
            {
                list = chapterInfo.Chapters;
            }
            IsSortDown = false;
            ListChapter = list;
            _staticListChapter = new List<ChapInfo>(list);
            CountChapter = ListChapter.Count;
        }
    }
}
