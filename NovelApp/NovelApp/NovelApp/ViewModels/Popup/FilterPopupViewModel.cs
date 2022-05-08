using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using NovelApp.DependencyServices;
using NovelApp.Models;
using NovelApp.Services.DatabaseService;
using Prism.Commands;
using Prism.Navigation;
using Xamarin.Forms;

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
        private string selectedChapterItem;
        private string selectedypeItem;
        private string selectedStatusItem;
        private string selectedGenreItem;
        private FilterInfo _chapterFiler;
        private FilterInfo _typeFiler;
        private FilterInfo _statusFiler;
        private FilterInfo _genreFiler;
        private bool isEnableSubmit;

        public ICommand GobackCommand { get; set; }
        public ICommand SubmitCommand { get; set; }
        public string SelectedChapterItem
        {
            get => selectedChapterItem; set
            {
                if (SetProperty(ref selectedChapterItem, value))
                    CheckDiffFilter();
            }
        }
        public string SelectedTypeItem
        {
            get => selectedypeItem; set
            {
                if (SetProperty(ref selectedypeItem, value))
                    CheckDiffFilter();
            }
        }
        public string SelectedStatusItem
        {
            get => selectedStatusItem; set
            {
                if (SetProperty(ref selectedStatusItem, value))
                    CheckDiffFilter();
            }
        }
        public string SelectedGenreItem
        {
            get => selectedGenreItem; set
            {
                if (SetProperty(ref selectedGenreItem, value))
                    CheckDiffFilter();
            }
        }
        public bool IsEnableSubmit { get => isEnableSubmit; set => SetProperty(ref isEnableSubmit, value); }
        public FilterPopupViewModel(INavigationService navigationService, IDatabaseService databaseService) : base(navigationService)
        {
            _databaseService = databaseService;
            GobackCommand = new DelegateCommand(async () =>
            {
                await NavigationService.GoBackAsync();
            });
            SubmitCommand = new DelegateCommand(SubmitFilter);
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            var listFilters = await _databaseService.GetFilters();
            if (listFilters!=null&& listFilters.Any())
            {
                foreach (var filter in listFilters)
                {
                    if (filter.Type == 1)
                    {
                        _statusFiler = new FilterInfo {
                            ID = filter.ID,
                            Type = filter.Type,
                            Name = filter.Name,
                            IsDelete = filter.IsDelete
                        };
                    }
                    else if (filter.Type == 2)
                    {
                        _genreFiler = new FilterInfo
                        {
                            ID = filter.ID,
                            Type = filter.Type,
                            Name = filter.Name,
                            IsDelete = filter.IsDelete
                        };
                    }
                    else if (filter.Type == 3)
                    {
                        _typeFiler = new FilterInfo
                        {
                            ID = filter.ID,
                            Type = filter.Type,
                            Name = filter.Name,
                            IsDelete = filter.IsDelete
                        };
                    }
                    else
                        _chapterFiler = new FilterInfo
                        {
                            ID = filter.ID,
                            Type = filter.Type,
                            Name = filter.Name,
                            IsDelete = filter.IsDelete
                        };
                }

            }
            else
            {
                _statusFiler = new FilterInfo { ID = 0, Type = 1, Name = ChoiceStatusItems[0], IsDelete = 0 };
                _genreFiler = new FilterInfo { ID = 0, Type = 2, Name = ChoiceGenreItems[0], IsDelete = 0 };
                _typeFiler = new FilterInfo { ID = 0, Type = 3, Name = ChoiceStatusItems[0], IsDelete = 0 };
                _chapterFiler = new FilterInfo { ID = 0, Type = 4, Name = ChoiceChapterItems[0], IsDelete = 0 };
            }
            _selectedChapterItemPrv = SelectedChapterItem = ChoiceChapterItems[_chapterFiler.ID];
            _selectedTypeItemPrv = SelectedTypeItem = ChoiceTypeItems[_typeFiler.ID]; ;
            _selectedStatusItemPrv = SelectedStatusItem = ChoiceStatusItems[_statusFiler.ID]; ;
            _selectedGenreItemPrv = SelectedGenreItem = ChoiceGenreItems[_genreFiler.ID];
        }
        private string _selectedChapterItemPrv;
        private string _selectedTypeItemPrv;
        private string _selectedStatusItemPrv;
        private string _selectedGenreItemPrv;
        private bool CheckDiffFilter()
        {
            if (SelectedChapterItem != _selectedChapterItemPrv || SelectedTypeItem != _selectedTypeItemPrv
                || SelectedStatusItem != _selectedStatusItemPrv || SelectedGenreItem != _selectedGenreItemPrv)
            {
                IsEnableSubmit = true;
                return true;
            }
            IsEnableSubmit = false;
            return false;
        }
        private async void SubmitFilter()
        {
            if (IsEnableSubmit)
            {
                IsEnableSubmit = false;
                _statusFiler.ID = ChoiceStatusItems.IndexOf(SelectedStatusItem);
                _selectedStatusItemPrv = _statusFiler.Name = SelectedStatusItem;
                _genreFiler.ID = ChoiceGenreItems.IndexOf(SelectedGenreItem);
                _selectedGenreItemPrv =  _genreFiler.Name = SelectedGenreItem;
                _typeFiler.ID = ChoiceTypeItems.IndexOf(SelectedTypeItem);
                _selectedTypeItemPrv =  _typeFiler.Name = SelectedTypeItem;
                _chapterFiler.ID = ChoiceChapterItems.IndexOf(SelectedChapterItem);
                _selectedChapterItemPrv = _chapterFiler.Name = SelectedChapterItem;

                var list = new List<FilterInfo>()
                {
                    _statusFiler,_genreFiler,_typeFiler,_chapterFiler
                };
               var result = await _databaseService.SaveFilters(list);
                if (result)
                {
                    var listFilters = await _databaseService.GetFilters();
                    if (listFilters != null && listFilters.Any())
                    {
                        foreach (var filter in listFilters)
                        {
                            if (filter.Type == 1)
                            {
                                _statusFiler = new FilterInfo
                                {
                                    ID = filter.ID,
                                    Type = filter.Type,
                                    Name = filter.Name,
                                    IsDelete = filter.IsDelete
                                };
                            }
                            else if (filter.Type == 2)
                            {
                                _genreFiler = new FilterInfo
                                {
                                    ID = filter.ID,
                                    Type = filter.Type,
                                    Name = filter.Name,
                                    IsDelete = filter.IsDelete
                                };
                            }
                            else if (filter.Type == 3)
                            {
                                _typeFiler = new FilterInfo
                                {
                                    ID = filter.ID,
                                    Type = filter.Type,
                                    Name = filter.Name,
                                    IsDelete = filter.IsDelete
                                };
                            }
                            else
                                _chapterFiler = new FilterInfo
                                {
                                    ID = filter.ID,
                                    Type = filter.Type,
                                    Name = filter.Name,
                                    IsDelete = filter.IsDelete
                                };
                        }
                        DependencyService.Get<IToastMessage>().Show("Submit thành công");
                    }
                }
                else
                {
                    DependencyService.Get<IToastMessage>().Show("Submit thất bại");
                    IsEnableSubmit = true;
                }
            }
        }
    }
    
}
