using System;
using System.Net.Http;
using System.Reflection;
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
using LiveScore.ViewModels;
using LiveScore.Views;
using Plugin.Multilingual;
using Prism;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Mvvm;
using Refit;
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
            Akavache.Registrations.Start("LiveScore.Storage");

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
            RegisterServices(containerRegistry);
            RegisterForNavigation(containerRegistry);
        }

        private static void RegisterServices(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<ICacheService, CacheService>();
            containerRegistry.Register<ISettingsService, SettingsService>();
            containerRegistry.Register<IMenuService, MenuService>();
            containerRegistry.Register<ISportService, SportService>();
            containerRegistry.Register<IEssentialsService, EssentialsService>();
            containerRegistry.Register<ILoggingService, LoggingService>();
            containerRegistry.RegisterInstance(
                RestService.For<IMatchApi>(new HttpClient
                {
                    BaseAddress = new Uri(SettingsService.ApiEndPoint)
                }));
            containerRegistry.RegisterInstance(
               RestService.For<ILeagueApi>(new HttpClient
               {
                   BaseAddress = new Uri(SettingsService.ApiEndPoint)
               }));
            containerRegistry.Register<IGlobalFactory, GlobalFactory>();
        }

        private static void RegisterForNavigation(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainView>();
            containerRegistry.RegisterForNavigation<MenuTabbedView>();
            containerRegistry.RegisterForNavigation<MainView, MainViewModel>();
            containerRegistry.RegisterForNavigation<SelectSportView, SelectSportViewModel>();
            ViewModelLocationProvider.Register<NavigationTitleView, NavigationTitleViewModel>();
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<LeagueModule>();
            moduleCatalog.AddModule<ScoreModule>();
            moduleCatalog.AddModule<FavoritesModule>();
            moduleCatalog.AddModule<NewsModule>();
            moduleCatalog.AddModule<MenuModule>();
        }

    }
}