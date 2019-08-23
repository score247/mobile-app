namespace LiveScore.iOS
{
    using System;
    using CarouselView.FormsPlugin.iOS;
    using Common.Services;
    using Foundation;
    using LiveScore.Common.Helpers;
    using ObjCRuntime;
    using PanCardView.iOS;
    using Prism;
    using Prism.Ioc;
    using UIKit;

    [Register("AppDelegate")]
    public class AppDelegate : Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication uiApplication, NSDictionary launchOptions)
        {
            Profiler.Start("IOS Application");
            Xamarin.Forms.Forms.Init();
            CardsViewRenderer.Preserve();

            var application = new App(new iOSInitializer());
            LoadApplication(application);

            var loggingService = application.Container.Resolve<ILoggingService>();

            Runtime.MarshalManagedException += (_, args) => loggingService.LogError(args.Exception);

            Runtime.MarshalObjectiveCException += (_, args)
                => loggingService.LogError(new InvalidOperationException($"Marshaling Objective-C exception. {args.Exception.DebugDescription}"));

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