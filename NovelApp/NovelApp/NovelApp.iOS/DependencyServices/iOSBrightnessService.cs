
using NovelApp.DependencyServices;
using NovelApp.iOS.DependencyServices;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(iOSBrightnessService))]
namespace NovelApp.iOS.DependencyServices
{
    public class iOSBrightnessService : IBrightnessService
    {
        public float GetBrightness()
        {
                //throw new NotImplementedException();
                return (float)UIScreen.MainScreen.Brightness;
        }

        public void SetBrightness(float brightness)
        {
            UIScreen.MainScreen.Brightness = brightness;
        }
    }
}