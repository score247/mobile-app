using System;
using Common.Logging;
using Foundation;
using ObjCRuntime;
using Prism;
using Prism.Ioc;
using UIKit;

namespace LiveScoreApp.iOS
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
                #if ENABLE_TEST_CLOUD
                Xamarin.Calabash.Start();
                #endif

            global::Xamarin.Forms.Forms.Init();
            LoadApplication(new App(new iOSInitializer()));

            Runtime.MarshalManagedException += (object sender, MarshalManagedExceptionEventArgs args) =>
            {
                LoggingService.LogError($"{args.Exception.Message} Exception", args.Exception);
            };

            Runtime.MarshalObjectiveCException += (object sender, MarshalObjectiveCExceptionEventArgs args) =>
            {
                LoggingService.LogError($"Marshaling Objective-C exception {args.Exception.Name}", new InvalidOperationException($"Marshaling Objective-C exception. {args.Exception.DebugDescription}"));
            };

            return base.FinishedLaunching(app, options);
        }
    }

    public class iOSInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // Register any platform specific implementations
        }
    }


}