using NovelApp.Models.BookGwModels;
using NovelApp.Services.Book;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace NovelApp.ViewModels
{
    public class BookDetailPageViewModel : BaseViewModel
    {
        private readonly IBookService _bookService;
        public Novel Novel { get => novel; set => SetProperty(ref novel, value); }
        private int _novelId;
        private Novel novel;
        private bool isExpand;

        public bool IsExpand { get => isExpand; set => SetProperty(ref isExpand, value); }
        public ICommand ExpandCommand { get; set; }
        public BookDetailPageViewModel(INavigationService navigationService, IBookService bookService) : base(navigationService)
        {
            _bookService = bookService;
            ExpandCommand = new DelegateCommand(() =>
            {
                IsExpand = !IsExpand;
            });
        }
        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters.ContainsKey("ID"))
                _novelId = int.Parse(parameters["ID"].ToString());
            Novel = await _bookService.GetDetailNovel(_novelId);
        }
    }
}
