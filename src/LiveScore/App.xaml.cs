using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using LiveScore.Common.LangResources;
using LiveScore.Common.Services;
using LiveScore.Configurations;
using LiveScore.Core.Events;
using LiveScore.Core.Services;
using LiveScore.Features.Favorites;
using LiveScore.Features.League;
using LiveScore.Features.Menu;
using LiveScore.Features.News;
using LiveScore.Features.Score;
using LiveScore.Features.TVSchedule;
using LiveScore.Soccer;
using LiveScore.Soccer.Services;
using LiveScore.Views;
using MethodTimer;
using Microsoft.AspNetCore.SignalR.Client;
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
        private IHubService soccerHub;
        private INetworkConnection networkConnectionManager;

        public App() : this(null)
        {
        }

        [Time]
        public App(IPlatformInitializer initializer) : base(initializer)
        {
        }

        [Time]
        protected override void OnInitialized()
        {
            AppResources.Culture = CrossMultilingual.Current.DeviceCultureInfo;

            MainPage = new NavigationPage(new SplashScreen());

            InitializeComponent();

            _ = StartEventHubs();

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
        {
            containerRegistry
                .UseContainerInstance(Container)
                .RegisterServices()
                .RegisterNavigation();
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<SoccerModule>();

            moduleCatalog.AddModule<ScoreModule>();
            moduleCatalog.AddModule<LeagueModule>(InitializationMode.OnDemand);
            moduleCatalog.AddModule<FavoritesModule>(InitializationMode.OnDemand);
            moduleCatalog.AddModule<NewsModule>();
            moduleCatalog.AddModule<TVScheduleModule>(InitializationMode.OnDemand);
            moduleCatalog.AddModule<MenuModule>();
        }

        private async Task StartEventHubs()
        {
            var eventAggregator = Container.Resolve<IEventAggregator>();

            soccerHub = new SoccerHubService(
                Container.Resolve<IHubConnectionBuilder>(),
                Configuration.SignalRHubEndPoint,
                Container.Resolve<ILoggingService>(),
                eventAggregator,
                Container.Resolve<INetworkConnection>());

            await soccerHub.Start();

            eventAggregator
                .GetEvent<ConnectionChangePubSubEvent>()
                .Subscribe(async (isConnected) =>
                {
                    if (isConnected)
                    {
                        await soccerHub.ReConnect();
                    }
                });
        }

        protected override void OnSleep()
        {
            Debug.WriteLine("OnSleep");

            base.OnSleep();
        }

        protected override async void OnResume()
        {
            Debug.WriteLine("OnResume");

            await Task.Run(async () => await soccerHub.ReConnect());

            base.OnResume();
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