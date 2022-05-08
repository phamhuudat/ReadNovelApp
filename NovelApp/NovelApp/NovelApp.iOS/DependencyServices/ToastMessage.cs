using System;
using System.Threading.Tasks;
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
       async void ShowAlert(string message, double seconds)
        {
            try
            {
                UIView ToastView;
                UILabel messageLabel;
                //if (ToastView == null)
                //{
                UIWindow window = UIApplication.SharedApplication.KeyWindow;
                ToastView = new UIView();
                ToastView.TranslatesAutoresizingMaskIntoConstraints = false;
                ToastView.Frame = window.Frame;
                ToastView.BackgroundColor = UIColor.FromRGB(89, 89, 89);
                ToastView.Layer.CornerRadius = 10;
                window.AddSubview(ToastView);

                messageLabel = new UILabel()
                {
                    TranslatesAutoresizingMaskIntoConstraints = false,
                    Lines = 0,
                };
                ToastView.AddSubview(messageLabel);
                messageLabel.TopAnchor.ConstraintEqualTo(ToastView.TopAnchor, 10).Active = true;
                messageLabel.LeadingAnchor.ConstraintEqualTo(ToastView.LeadingAnchor, 10).Active = true;
                messageLabel.TrailingAnchor.ConstraintEqualTo(ToastView.TrailingAnchor, -10).Active = true;
                messageLabel.BottomAnchor.ConstraintEqualTo(ToastView.BottomAnchor, -10).Active = true;
                messageLabel.TextColor = UIColor.White;
                messageLabel.TextAlignment = UITextAlignment.Center;

                ToastView.LeadingAnchor.ConstraintEqualTo(window.SafeAreaLayoutGuide.LeadingAnchor, 20).Active = true;
                ToastView.TrailingAnchor.ConstraintEqualTo(window.SafeAreaLayoutGuide.TrailingAnchor, -20).Active = true;
                ToastView.BottomAnchor.ConstraintEqualTo(window.SafeAreaLayoutGuide.BottomAnchor, -30).Active = true;

                // }
                //else
                // {
                //UIWindow window = UIApplication.SharedApplication.KeyWindow;

                // }
                messageLabel.Text = message;
                var result = ToastView.Alpha = 1;
                await Task.Delay(2500);

                UIView.Animate(0.5, () =>
                {
                    result = ToastView.Alpha = 0;
                });
                window.WillRemoveSubview(ToastView);

            }
            catch (Exception e)
            {

            }
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
