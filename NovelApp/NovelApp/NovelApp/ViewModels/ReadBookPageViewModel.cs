using NovelApp.Views.Popup;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace NovelApp.ViewModels
{
    public class ReadBookPageViewModel : BaseViewModel
    {
        public ICommand NavigationSettingsCommand { get; set; }
        public ReadBookPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            NavigationSettingsCommand = new DelegateCommand(PopupSettings);
        }
        private async void PopupSettings()
        {
            await NavigationService.NavigateAsync(nameof(SettingsPopup));
        }
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            
        }
    }
}
