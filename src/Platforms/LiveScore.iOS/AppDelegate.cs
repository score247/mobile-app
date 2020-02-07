using System;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;
using FFImageLoading.Forms.Platform;
using FFImageLoading.Svg.Forms;
using Foundation;
using LiveScore.Common.Helpers;
using LiveScore.Common.Services;
using LiveScore.Core.Events;
using ObjCRuntime;
using PanCardView.iOS;
using Prism;
using Prism.Events;
using Prism.Ioc;
using UIKit;
using Runtime = ObjCRuntime.Runtime;

namespace LiveScore.iOS
{
    [Register("AppDelegate")]
    public class AppDelegate : Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        private ILoggingService loggingService;
        private IEventAggregator eventAggregator;

        public override bool FinishedLaunching(UIApplication uiApplication, NSDictionary launchOptions)
        {
            Profiler.Start("IOS Application");
            Rg.Plugins.Popup.Popup.Init();
            Xamarin.Forms.Forms.Init();
            CachedImageRenderer.Init();
            SvgCachedImage.Init();
            CardsViewRenderer.Preserve();
            XamEffects.iOS.Effects.Init();

            var application = new App(new iOSInitializer());
            eventAggregator = application.Container.Resolve<IEventAggregator>();

            LoadApplication(application);
            SubscribeEvents();
            HandleGlobalExceptions(application);

            return base.FinishedLaunching(uiApplication, launchOptions);
        }

        private void SubscribeEvents()
        {
            eventAggregator.GetEvent<StartLoadDataEvent>().Subscribe(() =>
            {
                UIApplication.SharedApplication.NetworkActivityIndicatorVisible = true;
            });

            eventAggregator.GetEvent<StopLoadDataEvent>().Subscribe(() => UIApplication.SharedApplication.NetworkActivityIndicatorVisible = false);
        }

        private void HandleGlobalExceptions(App application)
        {
            loggingService = application.Container.Resolve<ILoggingService>();

            Runtime.MarshalManagedException += HandleMarshalException;
            Runtime.MarshalObjectiveCException += HandleMarshalObjectCException;
            AppDomain.CurrentDomain.FirstChanceException += CurrentDomain_FirstChanceException;
            AppDomain.CurrentDomain.UnhandledException += UnhandledException;
            TaskScheduler.UnobservedTaskException += UnobservedTaskException;
        }

        private void CurrentDomain_FirstChanceException(object sender, FirstChanceExceptionEventArgs e)
        {
            loggingService.LogException(e.Exception);
        }

        private void HandleMarshalException(object sender, MarshalManagedExceptionEventArgs args)
        {
            loggingService.LogException(args.Exception);
        }

        private void HandleMarshalObjectCException(object sender, MarshalObjectiveCExceptionEventArgs args)
        {
            loggingService.LogException(new InvalidOperationException($"Marshaling Objective-C exception. {args.Exception.DebugDescription}"));
        }

        private void UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            loggingService.LogException(e.Exception);
        }

        private void UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            loggingService.LogException(e.ExceptionObject as Exception);
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