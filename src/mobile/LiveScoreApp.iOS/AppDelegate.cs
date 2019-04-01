

namespace LiveScoreApp.iOS
{
    using System;
    using CarouselView.FormsPlugin.iOS;
    using Common.Services;
    using Foundation;
    using ObjCRuntime;
    using Prism;
    using Prism.Ioc;
    using UIKit;

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
        public override bool FinishedLaunching(UIApplication uiApplication, NSDictionary launchOptions)
        {
#if ENABLE_TEST_CLOUD
            Xamarin.Calabash.Start();
#endif

            global::Xamarin.Forms.Forms.Init();
            CarouselViewRenderer.Init();
            LoadApplication(new App(new iOSInitializer()));

            Runtime.MarshalManagedException += (sender, args) =>
            {
                LoggingService.LogError(args.Exception);
            };

            Runtime.MarshalObjectiveCException += (sender, args) =>
            {
                LoggingService.LogError(new InvalidOperationException($"Marshaling Objective-C exception. {args.Exception.DebugDescription}"));
            };

            return base.FinishedLaunching(uiApplication, launchOptions);
        }
    }

#pragma warning disable S101 // Types should be named in PascalCase
    public class iOSInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // Register any platform specific implementations
        }
    }
#pragma warning restore S101 // Types should be named in PascalCase

}