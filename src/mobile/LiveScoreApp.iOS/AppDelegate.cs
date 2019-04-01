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

    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication uiApplication, NSDictionary launchOptions)
        {
#if ENABLE_TEST_CLOUD
            Xamarin.Calabash.Start();
#endif
            Xamarin.Forms.Forms.Init();
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