using System;
using System.Reflection;
using System.Threading.Tasks;
using FFImageLoading;
using FFImageLoading.Helpers;
using LiveScore.Common.LangResources;
using LiveScore.Common.Services;
using LiveScore.Configurations;
using LiveScore.Core.Enumerations;
using LiveScore.Core.Events;
using LiveScore.Core.Services;
using LiveScore.Views;
using Microsoft.AppCenter.Analytics;
using Plugin.Multilingual;
using Prism;
using Prism.DryIoc;
using Prism.Events;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Mvvm;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace LiveScore
{
    public partial class App : PrismApplication
    {
        /*
         * The Xamarin Forms XAML Previewer in Visual Studio uses System.Activator.CreateInstance.
         * This imposes a limitation in which the App class must have a default constructor.
         * App(IPlatformInitializer initializer = null) cannot be handled by the Activator.
         */
        private INetworkConnection networkConnectionManager;
        private IHubService soccerHub;
        private DateTime appSleepTime;

        public App() : this(null)
        {
        }

        public App(IPlatformInitializer initializer) : base(initializer)
        {
        }

        protected override void OnInitialized()
        {
            AppResources.Culture = CrossMultilingual.Current.DeviceCultureInfo;
            ImageService
                .Instance
                .Initialize(new FFImageLoading.Config.Configuration { Logger = Container.Resolve<IMiniLogger>() });

            SetMainPage();
            InitializeComponent();

            StartEventHubs();

            StartGlobalTimer();

            networkConnectionManager = Container.Resolve<INetworkConnection>();
            networkConnectionManager.StartListen();
        }

        protected override void ConfigureViewModelLocator()
        {
            base.ConfigureViewModelLocator();

            ViewModelLocationProvider.SetDefaultViewTypeToViewModelTypeResolver((viewType) =>
            {
                var viewName = viewType.FullName?.Replace(".ViewModels.", ".Views.");
                var viewAssemblyName = viewType.GetTypeInfo().Assembly.FullName;
                var viewModelName = $"{viewName?.Replace("View", string.Empty)}ViewModel, {viewAssemblyName}";

                return Type.GetType(viewModelName);
            });
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
            => containerRegistry
                .UseContainerInstance(Container)
                .UseConfiguration(new Configuration())
                .UseSentry()
                .RegisterServices(Container)
                .RegisterNavigation();

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
            => moduleCatalog
                .AddProductModules()
                .AddFeatureModules()
                .Verify();

        private void StartEventHubs()
        {
            soccerHub = Container.Resolve<IHubService>(SportType.Soccer.Value.ToString());

            soccerHub.Start();
        }

        protected override async void OnSleep()
        {
            try
            {
                base.OnSleep();
                appSleepTime = DateTime.Now;

                await soccerHub.Stop();
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent($"OnSleep: {ex}");
            }
        }

        protected override async void OnResume()
        {
            try
            {
                if (soccerHub == null)
                {
                    soccerHub = Container.Resolve<IHubService>(SportType.Soccer.Value.ToString());
                }

                await Task.Run(async () => await soccerHub.ConnectWithRetry());

                if (DateTime.Now - appSleepTime > TimeSpan.FromMinutes(30))
                {
                    SetMainPage();

                    return;
                }

                base.OnResume();
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent($"OnResume: {ex}");
            }
        }

        private void SetMainPage()
        {
            MainPage = new NavigationPage(new SplashScreen());
        }

        private void StartGlobalTimer(int intervalMinutes = 1)
        {
            Device.StartTimer(TimeSpan.FromMinutes(intervalMinutes), () =>
            {
                if (networkConnectionManager.IsSuccessfulConnection())
                {
                    var eventAggregator = Container.Resolve<IEventAggregator>();
                    eventAggregator.GetEvent<OneMinuteTimerCountUpEvent>().Publish();
                }

                return true;
            });
        }
    }
}