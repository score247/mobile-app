using System;
using System.Net.Http;
using System.Reflection;
using Common.LangResources;
using Common.Services;
using Core.Factories;
using Core.Services;
using LiveScoreApp.Factories;
using LiveScoreApp.Services;
using LiveScoreApp.ViewModels;
using LiveScoreApp.Views;
using Plugin.Multilingual;
using Prism;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Mvvm;
using Refit;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace LiveScoreApp
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
            Akavache.Registrations.Start("LiveScoreAppStorage");

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
            containerRegistry.Register<ILoggingService, SentryLogger>();
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
            containerRegistry.Register<ILoggingService, LoggingService>();

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
            moduleCatalog.AddModule<League.LeagueModule>();
            moduleCatalog.AddModule<Score.ScoreModule>();
            moduleCatalog.AddModule<Favorites.FavoritesModule>();
            moduleCatalog.AddModule<News.NewsModule>();
            moduleCatalog.AddModule<Menu.MenuModule>();
        }

    }
}