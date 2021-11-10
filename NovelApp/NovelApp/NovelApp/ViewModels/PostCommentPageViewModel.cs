using System;
using System.Windows.Input;
using NovelApp.Services.Book;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;

namespace NovelApp.ViewModels
{
    public class PostCommentPageViewModel : BaseViewModel
    {
        public ICommand PostCmtCommand { get; set; }
        private string comment;
        private int star;
        private bool isCanPost;

        public bool IsCanPost { get => isCanPost; set => SetProperty(ref isCanPost, value); }
        public string Comment
        {
            get => comment; set
            {
                if (SetProperty(ref comment, value))
                {
                    CheckCanPostComment();
                }
            }
        }
        public int Star
        {
            get => star; set
            {
                if (SetProperty(ref star, value))
                {
                    CheckCanPostComment();
                }
            }
        }
        private int _novelId;
        private IBookService _bookService;
        private IPageDialogService _dialogService;
        public PostCommentPageViewModel(INavigationService navigationService, IBookService bookService, IPageDialogService dialogService) : base(navigationService)
        {
            _bookService = bookService;
            _dialogService = dialogService;
            PostCmtCommand = new DelegateCommand(PostComment);
        }
        private async void PostComment()
        {
            if (!IsCanPost)
                return;
            try
            {
                var result = await _bookService.PostComment(_novelId, Comment, Star, "test@email.com");
                var success = int.Parse(result[0].ToString());
                if (success == 1)
                {
                    await _dialogService.DisplayAlertAsync("Notification", result[1].ToString(), "OK");
                    await NavigationService.GoBackAsync();
                }
                else
                {
                    await _dialogService.DisplayAlertAsync("Notification", result[1].ToString(), "OK");
                }
            }
            catch (Exception e)
            {

            }
        }
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters.ContainsKey("ID"))
                _novelId = int.Parse(parameters["ID"].ToString());
        }
        private void CheckCanPostComment()
        {
            if (string.IsNullOrEmpty(Comment) || Star <= 0)
                IsCanPost = false;
            else
                IsCanPost = true;
        }

    }
}
