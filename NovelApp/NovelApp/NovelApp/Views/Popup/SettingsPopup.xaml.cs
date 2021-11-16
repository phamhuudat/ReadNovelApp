using NovelApp.DependencyServices;
using NovelApp.Models.Enums;
using NovelApp.ViewModels;
using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NovelApp.Views.Popup
{
    public partial class SettingsPopup : PopupPage
    {
        public SettingsPopup()
        {
            InitializeComponent();
        }
        protected override Task OnAppearingAnimationEndAsync()
        {
            if(_viewModel == null)
                _viewModel = BindingContext as SettingsPopupViewModel;
            
            Brightness.Value = _viewModel.IndexBrightness;
            TextSize.Value = _viewModel.IndexTextSize;



            return base.OnAppearingAnimationEndAsync();
        }
        private SettingsPopupViewModel _viewModel;

        private void TextSize_ValueChanging(object sender, Syncfusion.SfRangeSlider.XForms.ValueEventArgs e)
        {
            if (_viewModel == null)
                _viewModel = BindingContext as SettingsPopupViewModel;
            _viewModel.ChangeTextSizeReadMode((TextSize)e.Value);
        }

        private void Brightness_ValueChanging(object sender, Syncfusion.SfRangeSlider.XForms.ValueEventArgs e)
        {
            var brightnessService = DependencyService.Get<IBrightnessService>();
            brightnessService.SetBrightness((float)Brightness.Value);
        }
    }
}