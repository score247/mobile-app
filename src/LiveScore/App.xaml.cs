using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using Akavache;
using JsonNet.ContractResolvers;
using LiveScore.Common.Configuration;
using LiveScore.Common.LangResources;
using LiveScore.Common.Services;
using LiveScore.Core;
using LiveScore.Core.Controls.SearchPage;
using LiveScore.Core.Services;
using LiveScore.Core.ViewModels;
using LiveScore.Core.Views;
using LiveScore.Features.Favorites;
using LiveScore.Features.League;
using LiveScore.Features.Menu;
using LiveScore.Features.News;
using LiveScore.Features.TVSchedule;
using LiveScore.Score;
using LiveScore.Soccer;
using LiveScore.Soccer.Services;
using LiveScore.ViewModels;
using LiveScore.Views;
using MethodTimer;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
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
        private readonly List<IHubService> hubServices = new List<IHubService>();

        public App() : this(null)
        {
        }

        [Time]
        public App(IPlatformInitializer initializer) : base(initializer)
        {
        }

        [Time]
        protected override async void OnInitialized()
        {
            Registrations.Start("Score247.App");

            var jsonSettings = new JsonSerializerSettings
            {
                DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                ContractResolver = new PrivateSetterContractResolver()
            };

            Splat.Locator.CurrentMutable.Register(() => jsonSettings, typeof(JsonSerializerSettings));

            AppResources.Culture = CrossMultilingual.Current.DeviceCultureInfo;

            InitializeComponent();

            var logService = Container.Resolve<ILoggingService>();
            logService.Init(Configuration.SentryDsn);

            var settingsService = Container.Resolve<ISettingsService>();
            settingsService.ApiEndpoint = Configuration.LocalEndPoint;
            settingsService.HubEndpoint = Configuration.LocalHubEndPoint;

            RegisterAndStartEventHubs(Container);

            await NavigationService.NavigateAsync(nameof(MainView) + "/" + nameof(MenuTabbedView));
        }

        protected override void ConfigureViewModelLocator()
        {
            base.ConfigureViewModelLocator();

            ViewModelLocationProvider.SetDefaultViewTypeToViewModelTypeResolver((viewType) =>
            {
                var viewName = viewType.FullName.Replace(".ViewModels.", ".Views.");
                var viewAssemblyName = viewType.GetTypeInfo().Assembly.FullName;
                var viewModelName = $"{viewName.Replace("View", string.Empty)}ViewModel, {viewAssemblyName}";

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
            containerRegistry.RegisterSingleton<ICachingService, CachingService>();
            containerRegistry.RegisterSingleton<ISettingsService, SettingsService>();
            containerRegistry.RegisterSingleton<ISportService, SportService>();
            containerRegistry.RegisterSingleton<IEssential, Essential>();
            containerRegistry.RegisterSingleton<ILoggingService, LoggingService>();
            containerRegistry.RegisterSingleton<IApiPolicy, ApiPolicy>();
            containerRegistry.RegisterSingleton<IApiService, ApiService>();
            containerRegistry.RegisterSingleton<IDependencyResolver, DependencyResolver>();
        }

        private static void RegisterForNavigation(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<SearchNavigationPage>();
            containerRegistry.RegisterForNavigation<MenuTabbedView, MenuTabbedViewModel>();
            containerRegistry.RegisterForNavigation<MainView, MainViewModel>();
            containerRegistry.RegisterForNavigation<SelectSportView, SelectSportViewModel>();
            containerRegistry.RegisterForNavigation<TabMoreView, TabMoreViewModel>();
            containerRegistry.RegisterForNavigation<SearchView, SearchViewModel>();
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<SoccerModule>();
            moduleCatalog.AddModule<LeagueModule>();
            moduleCatalog.AddModule<ScoreModule>();
            moduleCatalog.AddModule<FavoritesModule>();
            moduleCatalog.AddModule<NewsModule>();
            moduleCatalog.AddModule<TVScheduleModule>();
            moduleCatalog.AddModule<MenuModule>();
        }

        private async void RegisterAndStartEventHubs(IContainerProvider container)
        {
            var soccerHubService = new HubService(
                container.Resolve<IHubConnectionBuilder>(),
                container.Resolve<ILoggingService>(),
                container.Resolve<ISettingsService>(),
                container.Resolve<IEventAggregator>());

            hubServices.Add(soccerHubService);

            // TODO: Ricky: temporary comment here
            ////foreach (var hubService in hubServices)
            ////{
            ////    await hubService.Start();
            ////}
        }

        protected override void OnSleep()
        {
            Debug.WriteLine("OnSleep");

            var localStorage = Container.Resolve<ICachingService>();
            localStorage.Shutdown();

            base.OnSleep();
        }

        protected override async void OnResume()
        {
            Debug.WriteLine("OnResume");

            // TODO: Ricky: temporary comment here
            ////foreach (var hubService in hubServices)
            ////{
            ////    await hubService.Reconnect();
            ////}

            base.OnResume();
        }
    }
}