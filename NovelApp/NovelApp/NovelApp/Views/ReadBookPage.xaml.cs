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
        int topItem;
        void book_Swiped(System.Object sender, MLToolkit.Forms.SwipeCardView.Core.SwipedCardEventArgs e)
        {
            if(e.Direction == MLToolkit.Forms.SwipeCardView.Core.SwipeCardDirection.Left)
            {
                topItem++;
            }
            else
            {
                topItem--;
                book.TopItem = topItem < 0 ? 0 : topItem ;
            }

        }
    }
}