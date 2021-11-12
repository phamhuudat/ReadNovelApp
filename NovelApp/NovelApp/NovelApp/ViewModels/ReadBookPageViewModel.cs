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
using Xamarin.Essentials;

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
            // Get Metrics
            var mainDisplayInfo = DeviceDisplay.MainDisplayInfo;
            var density = mainDisplayInfo.Density;
            // Width (in pixels)
            var width = mainDisplayInfo.Width / density;

            // Height (in pixels)
            var height = mainDisplayInfo.Height / density;

            Debug.WriteLine("height: " + height);
            Debug.WriteLine("width: " + width);
            try
            {
                var content = await _bookService.GetContentChapter(_novelId, _no);
                var text = content.Text;
                var length = content.Text.Length;
                var rowheight = (int)width / 20;
                var columnHeight = (int)height / 20;
                var counttext = rowheight * columnHeight;
                var pages = length / counttext;
                var div = length % counttext;
                CarouselItems = new ObservableCollection<string>();
                int start = 0;

                for (int i = 1; i <= pages; i++)
                {
                    int end = i * counttext - 1;
                    Debug.WriteLine($"end:{start} {end} {i} {counttext} {length}");
                    var textsub = text.Substring(start, end);
                    start = end + 1;
                }
                if (div > 0)
                    CarouselItems.Add(text.Substring(start, length - 1));
            }
            catch (Exception e)
            {

            }


        }
    }
}
