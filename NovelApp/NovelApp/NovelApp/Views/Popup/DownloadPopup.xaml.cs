using System;
using System.Collections.Generic;
using NovelApp.ViewModels;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;

namespace NovelApp.Views.Popup
{
    public partial class DownloadPopup : PopupPage
    {
        public DownloadPopup()
        {
            InitializeComponent();
        }

        void TapGestureRecognizer_Tapped(System.Object sender, System.EventArgs e)
        {
            var vm = BindingContext as DownloadPopupViewModel;
            if (vm.GobackCommand.CanExecute(null))
                vm.GobackCommand.Execute(null);
        }
    }
}
