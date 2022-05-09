using MLToolkit.Forms.SwipeCardView;
using NovelApp.Models.BookGwModels;
using NovelApp.ViewModels;
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

        void Label_SizeChanged(System.Object sender, System.EventArgs e)
        {
            var view = sender as SwipeCardView;
            
        }
        /// <summary>
        /// khi vuốt cuối danh sách
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ScrollView_Scrolled(object sender, ScrolledEventArgs e)
        {
            var scroll = sender as ScrollView;
            if (!(sender is ScrollView scrollView))
                return;

            var scrollingSpace = scrollView.ContentSize.Height - scrollView.Height;

            if (scrollingSpace > e.ScrollY)
                return;

            // load more content.
        }
        /// <summary>
        /// Item xuất hiện trên listview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listView_ItemAppearing(object sender, Syncfusion.ListView.XForms.ItemAppearingEventArgs e)
        {
            if (e.ItemData != null)
            {
                var obj = e.ItemData as Chapter;
                if (obj != null)
                    NameChapterLabel.Text = obj.Name;
            }
        }

        void bookRight_Swiped(System.Object sender, MLToolkit.Forms.SwipeCardView.Core.SwipedCardEventArgs e)
        {
            //bookRight.IsVisible = false;
            var viewmdel = BindingContext as ReadBookPageViewModel;
            viewmdel.NextRightPageChapter(e.Item as PageChapter);
        }

        void SwipeGestureRecognizer_Swiped(System.Object sender, Xamarin.Forms.SwipedEventArgs e)
        {
        }        //void Label_SizeChanged_1(System.Object sender, System.EventArgs e)

        private void bookLeft_Swiped(object sender, MLToolkit.Forms.SwipeCardView.Core.SwipedCardEventArgs e)
        {
            var viewmdel = BindingContext as ReadBookPageViewModel;
            viewmdel.NextLeftPageChapter(e.Item as PageChapter);
        }
        private async void TapNextPage_Tapped(object sender, EventArgs e)
        {
            try
            {
                await bookLeft.InvokeSwipe(MLToolkit.Forms.SwipeCardView.Core.SwipeCardDirection.Left);
            }
            catch (Exception ex)
            {

            }
          
        }
        
        private async void TapPrevPage_Tapped(object sender, EventArgs e)
        {
            try
            {
                await bookRight.InvokeSwipe(MLToolkit.Forms.SwipeCardView.Core.SwipeCardDirection.Right);
            }
          catch(Exception ex)
            {

            }
        }
        //{
        //    var viewmdel = BindingContext as ReadBookPageViewModel;
        //    var label = sender as Label;

        //    viewmdel.Height = label.Height; 
        //    viewmdel.WidthReadPage = label.Height;
        //    Debug.WriteLine("ddooj cao ta=hay doi" + viewmdel.Height);
        //    //viewmdel.SplitPage(label.Height);
        //}
        ///// <summary>
        ///// Tinhs toans chieu cao cua tung cau
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //void Label_SizeChanged_2(System.Object sender, System.EventArgs e)
        //{
        //    var viewmdel = BindingContext as ReadBookPageViewModel;
        //    var label = sender as Label;

        //    viewmdel.ViewReadHeight = label.Height;
        //    //viewmdel.SplitPage(label.Height);
        //}

        //void Label_PropertyChanged(System.Object sender, System.ComponentModel.PropertyChangedEventArgs e)
        //{
        //    if(e.PropertyName == "Text")
        //    {

        //        var viewmdel = BindingContext as ReadBookPageViewModel;
        //        var label = sender as Label;

        //        viewmdel.Height = label.Height;
        //        Debug.WriteLine("ddooj cao ta=hay doi" + viewmdel.Height);
        //    }
        //}
    }
}