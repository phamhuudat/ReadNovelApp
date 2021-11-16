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
        public float GetBrightness()
        {
            var brightness = Android.Provider.Settings.System.GetInt(CrossCurrentActivity.Current.Activity.ContentResolver, Android.Provider.Settings.System.ScreenBrightness);
            //MainActivity.thisMainActivity is a isntance from activity
            //The returned brightness is an int type value between 0 and 255.

            return brightness;
        }

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