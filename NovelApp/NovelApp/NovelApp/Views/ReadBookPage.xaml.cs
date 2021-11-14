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
        private void book_VisibleCardIndexChanged(object sender, Syncfusion.XForms.Cards.VisibleCardIndexChangedEventArgs e)
        {
            // handle event action.
        }
    }
}