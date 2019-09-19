using LiveScore.Core.Events;
using Prism.Events;
using System;
using LiveScore.Common.Helpers;
using LiveScore.Common.Services;
using FFImageLoading.Forms.Platform;
using Foundation;
using Lottie.Forms.iOS.Renderers;
using ObjCRuntime;
using PanCardView.iOS;
using Prism;
using Prism.Ioc;
using UIKit;

namespace LiveScore.iOS
{
    [Register("AppDelegate")]
    public class AppDelegate : Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication uiApplication, NSDictionary launchOptions)
        {
            Profiler.Start("IOS Application");
            Xamarin.Forms.Forms.Init();
            AnimationViewRenderer.Init();
            CachedImageRenderer.Init();
            CardsViewRenderer.Preserve();

            var application = new App(new iOSInitializer());
            LoadApplication(application);

            var loggingService = application.Container.Resolve<ILoggingService>();

            Runtime.MarshalManagedException += (_, args) => loggingService.LogError(args.Exception);
            Runtime.MarshalObjectiveCException += (_, args)
                => loggingService.LogError(new InvalidOperationException($"Marshaling Objective-C exception. {args.Exception.DebugDescription}"));

            var eventAggregator = application.Container.Resolve<IEventAggregator>();
            eventAggregator.GetEvent<StartLoadDataEvent>().Subscribe(() =>
            {
                UIApplication.SharedApplication.NetworkActivityIndicatorVisible = true;
            });

            eventAggregator.GetEvent<StopLoadDataEvent>().Subscribe(() =>
            {
                UIApplication.SharedApplication.NetworkActivityIndicatorVisible = false;
            });

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