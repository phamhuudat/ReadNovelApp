using System;
using System.Windows.Input;
using Prism.Commands;
using Prism.Navigation;

namespace NovelApp.ViewModels
{
    public class PostCommentPageViewModel:BaseViewModel
    {
        public ICommand NavigationCmtCommand;
        public PostCommentPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            //NavigationCmtCommand = new DelegateCommand();
        }
        
    }
}
