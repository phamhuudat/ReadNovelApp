using System;
using System.Collections.Generic;
using System.Windows.Input;
using NovelApp.Services.DatabaseService;
using Prism.Commands;
using Prism.Navigation;

namespace NovelApp.ViewModels.Popup
{
    public class FilterPopupViewModel : BaseViewModel
    {
        public List<string> ChoiceChapterItems { get; set; } = new List<string>()
        {
            "All",
            "<50",
            "50-100",
            "100-200",
            "200-500",
            "500-1000",
            ">1000"
        };
        public List<string> ChoiceTypeItems { get; set; } = new List<string>()
        {
            "All",
            "Translation",
            "Original",
            "MTL",
        };
        public List<string> ChoiceStatusItems { get; set; } = new List<string>()
        {
            "All",
            "Ongoing",
            "Completed",
        };
        public List<string> ChoiceGenreItems { get; set; } = new List<string>()
        {
            "All",
            "Male",
            "Female",
        };
        private readonly IDatabaseService _databaseService;
        public ICommand GobackCommand { get; set; }
        public ICommand SubmitCommand { get; set; }
        public FilterPopupViewModel(INavigationService navigationService, IDatabaseService databaseService) : base(navigationService)
        {
            _databaseService = databaseService;
            GobackCommand = new DelegateCommand(async () =>
            {
                await NavigationService.GoBackAsync();
            });
            SubmitCommand = new DelegateCommand(SubmitFilter);
        }
        private void SubmitFilter()
        {

        }

    }
}
