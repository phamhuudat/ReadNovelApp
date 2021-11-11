using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace NovelApp.ViewModels
{
    public class SettingsPopupViewModel : BaseViewModel
    {
        public ICommand GoBackCommand { get; set; }
        public SettingsPopupViewModel(INavigationService navigationService) : base(navigationService)
        {
            GoBackCommand = new DelegateCommand(Goback);
        }
        private async void Goback()
        {
           await NavigationService.GoBackAsync();
        }
    }
}
