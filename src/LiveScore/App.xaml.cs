﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Fanex.Caching;
using LiveScore.Common;
using LiveScore.Common.Helpers;
using LiveScore.Common.LangResources;
using LiveScore.Common.Services;
using LiveScore.Configurations;
using LiveScore.Core;
using LiveScore.Core.Controls.SearchPage;
using LiveScore.Core.Events;
using LiveScore.Core.Services;
using LiveScore.Core.ViewModels;
using LiveScore.Core.Views;
using LiveScore.Features.Favorites;
using LiveScore.Features.League;
using LiveScore.Features.Menu;
using LiveScore.Features.News;
using LiveScore.Features.Score;
using LiveScore.Features.TVSchedule;
using LiveScore.Soccer;
using LiveScore.Soccer.Services;
using LiveScore.ViewModels;
using LiveScore.Views;
using MessagePack.Resolvers;
using MethodTimer;
using Microsoft.AspNetCore.SignalR.Client;
using Plugin.Multilingual;
using Prism;
using Prism.DryIoc;
using Prism.Events;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Mvvm;
using Refit;
using Xamarin.Essentials;
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
        private INetworkConnectionManager networkConnectionManager;


        public App() : this(null)
        {
        }

        [Time]
        public App(IPlatformInitializer initializer) : base(initializer)
        {
            this.PageAppearing += App_PageAppearing;
        }

        private void App_PageAppearing(object sender, Page e)
        {
            Debug.Write($"Page {e.Title} (id:{e.Id}) appears at {DateTime.Now:HH:mm:ss.fff}");
        }

        [Time]
        protected override void OnInitialized()
        {
            AppResources.Culture = CrossMultilingual.Current.DeviceCultureInfo;
            var logService = Container.Resolve<ILoggingService>();

            MainPage = new NavigationPage(new SplashScreen(logService));

            InitializeComponent();

            logService.Init(Configuration.SentryDsn, Configuration.Environment);

            _ = StartEventHubs();

            StartGlobalTimer();

            networkConnectionManager = Container.Resolve<INetworkConnectionManager>();
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
            containerRegistry.RegisterInstance(Container);
            RegisterServices(containerRegistry);
            RegisterForNavigation(containerRegistry);

            containerRegistry.Register<IHubConnectionBuilder, HubConnectionBuilder>();
        }

        private static void RegisterServices(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterInstance<IHttpService>(new HttpService(new Uri(Configuration.ApiEndPoint)));
            containerRegistry.RegisterSingleton<ICacheManager, CacheManager>();
            containerRegistry.RegisterSingleton<ICacheService, CacheService>();
            containerRegistry.RegisterSingleton<ISettings, Settings>();
            containerRegistry.RegisterSingleton<ISportService, SportService>();
            containerRegistry.RegisterSingleton<IEssential, Essential>();
            containerRegistry.RegisterSingleton<ILoggingService, LoggingService>();
            containerRegistry.RegisterSingleton<IApiPolicy, ApiPolicy>();
            containerRegistry.RegisterSingleton<IApiService, ApiService>();
            containerRegistry.RegisterSingleton<INetworkConnectionManager, NetworkConnectionManager>();
            containerRegistry.RegisterSingleton<IMatchService, MatchService>();
            containerRegistry.RegisterInstance(new RefitSettings
            {
                ContentSerializer = new MessagePackContentSerializer()
            });
            containerRegistry.RegisterSingleton<IDependencyResolver, DependencyResolver>();
            containerRegistry.RegisterInstance<Func<string,string>>((countryCode) 
                => string.IsNullOrWhiteSpace(countryCode) 
                    ? "images/flag_league/default_flag.svg"
                    : $"{Configuration.AssetsEndPoint}flags/{countryCode}.svg", 
                FuncNameConstants.BuildFlagUrlFuncName);

            CompositeResolver.RegisterAndSetAsDefault(
                SoccerModelResolver.Instance,
                CoreModelResolver.Instance,
                BuiltinResolver.Instance,
                PrimitiveObjectResolver.Instance);
        }

        private static void RegisterForNavigation(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<SearchNavigationPage>();
            containerRegistry.RegisterForNavigation<MenuTabbedView, MenuTabbedViewModel>();
            containerRegistry.RegisterForNavigation<MainView, MainViewModel>();
            containerRegistry.RegisterForNavigation<SelectSportView, SelectSportViewModel>();
            containerRegistry.RegisterForNavigation<SearchView, SearchViewModel>();
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
                Container.Resolve<INetworkConnectionManager>());

            await soccerHub.Start();

            eventAggregator
                .GetEvent<ConnectionChangePubSubEvent>()
                .Subscribe(async (isConnected) =>
                {
                    if(isConnected)
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

        private void StartGlobalTimer()
        {
            Device.StartTimer(TimeSpan.FromMinutes(1), () =>
            {
                if (networkConnectionManager.IsConnectionOK())
                {
                    var eventAggregator = Container.Resolve<IEventAggregator>();
                    eventAggregator.GetEvent<OneMinuteTimerCountUpEvent>().Publish();
                }

                return true;
            });
        }
    }
}