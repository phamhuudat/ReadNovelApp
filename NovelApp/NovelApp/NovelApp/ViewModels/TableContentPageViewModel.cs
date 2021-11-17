using NovelApp.Models.BookGwModels;
using NovelApp.Services.Book;
using NovelApp.Views;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace NovelApp.ViewModels
{
    public class TableContentPageViewModel : BaseViewModel
    {
        private readonly IBookService _bookService;
        private List<ChapInfo> listChapter;
        private int _novelId;
        private int countChapter;
        private bool isSortDown;
       
        public List<ChapInfo> ListChapter { get => listChapter; set => SetProperty(ref listChapter, value); }
        public int CountChapter { get => countChapter; set => SetProperty(ref countChapter, value); }
        public ICommand SortCommand { get; set; }
        public ICommand ItemTappedCommand { get; set; }
        public ICommand GobackCommand { get; set; }
        public bool IsSortDown { get => isSortDown; set => SetProperty(ref isSortDown, value); }
        public TableContentPageViewModel(INavigationService navigationService, IBookService bookService) : base(navigationService)
        {
            _bookService = bookService;
            SortCommand = new DelegateCommand(Sort);
            ItemTappedCommand = new DelegateCommand<object>(ItemTapped);
            GobackCommand = new DelegateCommand(async () =>
            {
                await NavigationService.GoBackAsync();
            });
        }
        private async void ItemTapped(object obj)
        {
            var listView = obj as Syncfusion.ListView.XForms.SfListView;
            if (listView == null) return;
            var item = listView.SelectedItem as ChapInfo;
            listView.SelectedItem = null;
            await NavigationService.NavigateAsync($"{nameof(ReadBookPage)}?ID={_novelId}&NO={item.No}");
        }
        private void Sort()
        {
            IsSortDown = !IsSortDown;
            if (IsSortDown)
                ListChapter = ListChapter.OrderByDescending(x => x.No).ToList();
            else
                ListChapter = ListChapter.OrderBy(x => x.No).ToList();
        }
        public async override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters.ContainsKey("ID"))
                _novelId = int.Parse(parameters["ID"].ToString());
            var list = await _bookService.GetTBC(_novelId);
            if (list != null && list.Chapters.Any())
            {
                ListChapter = list.Chapters;
                CountChapter = ListChapter.Count;
            }

        }
    }
}
