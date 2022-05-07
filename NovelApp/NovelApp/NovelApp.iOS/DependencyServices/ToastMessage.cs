using System;
using Foundation;
using NovelApp.DependencyServices;
using NovelApp.iOS.DependencyServices;
using UIKit;

[assembly: Xamarin.Forms.Dependency(typeof(ToastMessage))]
namespace NovelApp.iOS.DependencyServices
{
    public class ToastMessage: IToastMessage
    {
        const double LONG_DELAY = 3.5;


        NSTimer alertDelay;
        UIAlertController alert;
        public ToastMessage()
        {
        }

        public void Show(string message)
        {
            ShowAlert(message, LONG_DELAY);
        }
        void ShowAlert(string message, double seconds)
        {
            alertDelay = NSTimer.CreateScheduledTimer(seconds, (obj) =>
            {
                dismissMessage();
            });
            alert = UIAlertController.Create(null, message, UIAlertControllerStyle.Alert);
            UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewController(alert, true, null);
        }
        void dismissMessage()
        {
            if (alert != null)
            {
                alert.DismissViewController(true, null);
            }
            if (alertDelay != null)
            {
                alertDelay.Dispose();
            }
        }
    }
}
