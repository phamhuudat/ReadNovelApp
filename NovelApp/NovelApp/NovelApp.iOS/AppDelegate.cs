using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using Syncfusion.SfRotator.XForms.iOS;
using UIKit;

namespace NovelApp.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Rg.Plugins.Popup.Popup.Init();
            global::Xamarin.Forms.Forms.Init();
            Syncfusion.XForms.iOS.TabView.SfTabViewRenderer.Init();
            Syncfusion.XForms.iOS.Buttons.SfChipGroupRenderer.Init();
            Syncfusion.XForms.iOS.Buttons.SfChipRenderer.Init();
            Syncfusion.XForms.iOS.Cards.SfCardViewRenderer.Init();
            Syncfusion.SfCarousel.XForms.iOS.SfCarouselRenderer.Init();
            new SfRotatorRenderer();
            LoadApplication(new App());
            return base.FinishedLaunching(app, options);
        }
    }
}
