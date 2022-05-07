using System;
using Android.Widget;
using NovelApp.DependencyServices;
using NovelApp.Droid.DependencyServices;

[assembly: Xamarin.Forms.Dependency(typeof(ToastMessage))]
namespace NovelApp.Droid.DependencyServices
{
    public class ToastMessage:IToastMessage
    {
        public ToastMessage()
        {
        }

        public void Show(string message)
        {
            Toast.MakeText(Android.App.Application.Context, message, ToastLength.Long).Show();
        }
    }
}
