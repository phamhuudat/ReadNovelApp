using Android.Views;
using NovelApp.DependencyServices;
using NovelApp.Droid.DependencyServices;
using Plugin.CurrentActivity;
using Xamarin.Forms;

[assembly: Dependency(typeof(AndroidBrightnessService))]
namespace NovelApp.Droid.DependencyServices
{
    public class AndroidBrightnessService : IBrightnessService
    {
        public void SetBrightness(float brightness)
        {
            var window = CrossCurrentActivity.Current.Activity.Window;
            var attributesWindow = new WindowManagerLayoutParams();

            attributesWindow.CopyFrom(window.Attributes);
            attributesWindow.ScreenBrightness = brightness;

            window.Attributes = attributesWindow;
        }
    }
}