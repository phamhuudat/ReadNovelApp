using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NovelApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BookDetailPage : ContentPage
    {
        public BookDetailPage()
        {
            InitializeComponent();
        }
        private void ScrollView_Scrolled(object sender, ScrolledEventArgs e)
        {
            var deltaAnimation = (frameDetail.Y - e.ScrollY) / 100;
            if (deltaAnimation > 0)
            {
                header.Opacity = 1 - deltaAnimation;
            }
            else
            {
                header.Opacity = 1;
                deltaAnimation = CommentLs.Y - e.ScrollY;
                if (deltaAnimation <= 0)
                {
                    UnderLineInfo.IsVisible = false;
                    UnderLineReviews.IsVisible = true;
                }
                else
                {
                    UnderLineReviews.IsVisible = false;
                    UnderLineInfo.IsVisible = true;
                }
            }
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
           await ViewScroll.ScrollToAsync(0,frameDetail.Y,true);
        }

        private async void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
        {
            await ViewScroll.ScrollToAsync(0, CommentLs.Y, true);
        }
    }
}