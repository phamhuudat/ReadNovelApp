
using NovelApp.DependencyServices;
using NovelApp.iOS.DependencyServices;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(iOSBrightnessService))]
namespace NovelApp.iOS.DependencyServices
{
    public class iOSBrightnessService : IBrightnessService
    {
        public void SetBrightness(float brightness)
        {
            UIScreen.MainScreen.Brightness = brightness;
        }
    }
}