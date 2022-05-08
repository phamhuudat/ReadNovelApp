using System;
using System.Collections.Generic;
using System.Linq;
using FFImageLoading.Svg.Forms;
using Foundation;
using Syncfusion.ListView.XForms.iOS;
using Syncfusion.SfRotator.XForms.iOS;
using Syncfusion.XForms.iOS.EffectsView;
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
            Syncfusion.SfRangeSlider.XForms.iOS.SfRangeSliderRenderer.Init();
            Syncfusion.SfRating.XForms.iOS.SfRatingRenderer.Init();
            Syncfusion.XForms.iOS.ProgressBar.SfCircularProgressBarRenderer.Init();
            SfListViewRenderer.Init();
            SfEffectsViewRenderer.Init();
            new SfRotatorRenderer();
            FFImageLoading.Forms.Platform.CachedImageRenderer.Init();
            var ignore = typeof(SvgCachedImage);
            App.DisplayScreenWidth = (double)UIScreen.MainScreen.Bounds.Width;
            App.DisplayScreenHeight = (double)UIScreen.MainScreen.Bounds.Height;
            
            LoadApplication(new App());
            return base.FinishedLaunching(app, options);
        }
    }
}
