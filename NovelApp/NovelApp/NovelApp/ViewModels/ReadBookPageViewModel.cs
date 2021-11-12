using NovelApp.Services.Book;
using NovelApp.Views.Popup;
using Prism.Commands;
using Prism.Navigation;
using Syncfusion.SfCarousel.XForms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Windows.Input;

namespace NovelApp.ViewModels
{
    public class ReadBookPageViewModel : BaseViewModel
    {
        /// <summary>
        /// index novelid
        /// </summary>
        private int _novelId;
        /// <summary>
        /// index chapter
        /// </summary>
        private int _no;
        private ObservableCollection<string> carouselItems;
        private readonly IBookService _bookService;
        public ICommand NavigationSettingsCommand { get; set; }
        public ObservableCollection<string> CarouselItems { get => carouselItems; set => SetProperty(ref carouselItems, value); }
        public ReadBookPageViewModel(INavigationService navigationService, IBookService bookService) : base(navigationService)
        {
            NavigationSettingsCommand = new DelegateCommand(PopupSettings);
            _bookService = bookService;
        }
        private async void PopupSettings()
        {
            await NavigationService.NavigateAsync(nameof(SettingsPopup));
        }
        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters.ContainsKey("ID"))
                _novelId = int.Parse(parameters["ID"].ToString());
            if (parameters.ContainsKey("NO"))
                _no = int.Parse(parameters["NO"].ToString());

            var content = await _bookService.GetContentChapter(_novelId, _no);
            CarouselItems = new ObservableCollection<string>();
            CarouselItems.Add(content.Text);
            CarouselItems.Add(content.Text);
            CarouselItems.Add(content.Text);
            CarouselItems.Add(content.Text);
            CarouselItems.Add(content.Text);
            CarouselItems.Add(content.Text);
        }
    }
}
