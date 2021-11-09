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
    public class BookDetailPageViewModel : BaseViewModel
    {
        private readonly IBookService _bookService;
        public Novel Novel { get => novel; set => SetProperty(ref novel, value); }
        private int _novelId;
        private Novel novel;
        private bool isExpand;
        private List<Comment> listComment;
        private int countReview;

        public bool IsExpand { get => isExpand; set => SetProperty(ref isExpand, value); }
        public ICommand ExpandCommand { get; set; }
        public ICommand SearchChapterCommand { get; set; }
        public ICommand NavigationCmtCommand { get; set; }
        public List<Comment> ListComment { get => listComment; set => SetProperty(ref listComment, value); }
        public int CountReview { get => countReview; set => SetProperty(ref countReview, value); }
        public BookDetailPageViewModel(INavigationService navigationService, IBookService bookService) : base(navigationService)
        {
            _bookService = bookService;
            ExpandCommand = new DelegateCommand(() =>
            {
                IsExpand = !IsExpand;
            });
            SearchChapterCommand = new DelegateCommand(NavigationSearchChapter);
            NavigationCmtCommand = new DelegateCommand(NavigationCommentPage);
        }
        private async void NavigationCommentPage()
        {
            await NavigationService.NavigateAsync($"{nameof(PostCommentPage)}?ID={_novelId}");
        }
        private async void NavigationSearchChapter()
        {
            await NavigationService.NavigateAsync($"{nameof(TableContentPage)}?ID={_novelId}");
        }
        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters.ContainsKey("ID"))
                _novelId = int.Parse(parameters["ID"].ToString());
            Novel = await _bookService.GetDetailNovel(_novelId);
            var list = await _bookService.GetCommentList(_novelId);
            ListComment = list?.ToList();
            CountReview = ListComment.Count();
        }
    }
}
