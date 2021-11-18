using MLToolkit.Forms.SwipeCardView;
using NovelApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NovelApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReadBookPage : ContentPage
    {
        public ReadBookPage()
        {
            InitializeComponent();

        }
        
        protected override bool OnBackButtonPressed()
        {
            var viewmdel = BindingContext as ReadBookPageViewModel;
            viewmdel.GoBack();
            return false;
        }

        private void ViewReading_SizeChanged(object sender, EventArgs e)
        {
            

        }

        private void book_SizeChanged(object sender, EventArgs e)
        {
            var view = sender as SwipeCardView;
            var viewmdel = BindingContext as ReadBookPageViewModel;
            viewmdel.ViewReadHeight = view.Height;
            viewmdel.WidthReadPage = view.Width;
        }
    }
}