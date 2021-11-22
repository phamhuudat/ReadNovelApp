using MLToolkit.Forms.SwipeCardView;
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

        //void Label_SizeChanged_1(System.Object sender, System.EventArgs e)
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