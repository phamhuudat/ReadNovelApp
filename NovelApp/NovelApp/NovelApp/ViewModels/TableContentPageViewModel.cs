using NovelApp.Models.BookGwModels;
using NovelApp.Services.Book;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;

namespace NovelApp.ViewModels
{
    public class TableContentPageViewModel : BaseViewModel
    {
        private readonly IBookService _bookService;
        private List<ChapInfo> listChapter;
        private int _novelId;
        public List<ChapInfo> ListChapter { get => listChapter; set => SetProperty(ref listChapter, value); }
        public TableContentPageViewModel(INavigationService navigationService, IBookService bookService) : base(navigationService)
        {
            _bookService = bookService;
        }
        public async override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters.ContainsKey("ID"))
                _novelId = int.Parse(parameters["ID"].ToString());
            var list = await _bookService.GetTBC(_novelId);
            ListChapter = list.Chapters;
        }
    }
}
