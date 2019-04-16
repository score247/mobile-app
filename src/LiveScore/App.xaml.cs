﻿using System;
using System.Reflection;
using LiveScore.Basketball;
using LiveScore.Common.LangResources;
using LiveScore.Common.Services;
using LiveScore.Core.Factories;
using LiveScore.Core.Services;
using LiveScore.Factories;
using LiveScore.Favorites;
using LiveScore.League;
using LiveScore.Menu;
using LiveScore.News;
using LiveScore.Score;
using LiveScore.Services;
using LiveScore.Soccer;
using LiveScore.TVSchedule;
using LiveScore.ViewModels;
using LiveScore.Views;
using Plugin.Multilingual;
using Prism;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Mvvm;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace LiveScore
{
    public partial class App
    {
        /*
         * The Xamarin Forms XAML Previewer in Visual Studio uses System.Activator.CreateInstance.
         * This imposes a limitation in which the App class must have a default constructor.
         * App(IPlatformInitializer initializer = null) cannot be handled by the Activator.
         */

        public App() : this(null)
        {
        }

        public App(IPlatformInitializer initializer) : base(initializer)
        {
        }

        protected override async void OnInitialized()
        {
            AppResources.Culture = CrossMultilingual.Current.DeviceCultureInfo;

#if DEBUG
            HotReloader.Current.Start(this);
#endif

            InitializeComponent();

            var cacheService = Container.Resolve<ICacheService>();
            cacheService.Init();

            var logService = Container.Resolve<ILoggingService>();
            logService.Init("https://a75e3e7b51ea4de8baa2c27b67bbede3@sentry.nexdev.net/34");
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
        }

        private static void RegisterServices(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<ICacheService, CacheService>();
            containerRegistry.RegisterSingleton<ISettingsService, SettingsService>();
            containerRegistry.Register<ISportService, SportService>();
            containerRegistry.Register<IEssentialsService, EssentialsService>();
            containerRegistry.RegisterSingleton<ILoggingService, LoggingService>();
            containerRegistry.Register<IApiPolicy, ApiPolicy>();
            containerRegistry.RegisterSingleton<IGlobalFactoryProvider, GlobalFactoryProvider>();
        }

        private static void RegisterForNavigation(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainView>();
            containerRegistry.RegisterForNavigation<MenuTabbedView>();
            containerRegistry.RegisterForNavigation<MainView, MainViewModel>();
            containerRegistry.RegisterForNavigation<SelectSportView, SelectSportViewModel>();
            containerRegistry.RegisterForNavigation<TabMoreView, TabMoreViewModel>();
            ViewModelLocationProvider.Register<NavigationTitleView, NavigationTitleViewModel>();
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<SoccerModule>();
            moduleCatalog.AddModule<BasketballModule>();
            moduleCatalog.AddModule<LeagueModule>();
            moduleCatalog.AddModule<ScoreModule>();
            moduleCatalog.AddModule<FavoritesModule>();
            moduleCatalog.AddModule<NewsModule>();
            moduleCatalog.AddModule<TVScheduleModule>();
            moduleCatalog.AddModule<MenuModule>();
        }
    }
}