using System;
using System.Reflection;
using Common.LangResources;
using Common.Services;
using Core.Services;
using LiveScoreApp.Services;
using LiveScoreApp.ViewModels;
using LiveScoreApp.Views;
using Plugin.Multilingual;
using Prism;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Mvvm;
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
            containerRegistry.Register<ICacheService, CacheService>();
            containerRegistry.Register<ISettingsService, SettingsService>();
            containerRegistry.Register<IMenuService, MenuService>();
            containerRegistry.Register<ISportService, SportService>();
            containerRegistry.Register<IEssentialsService, EssentialsService>();
            containerRegistry.Register<ILoggingService, SentryLogger>();

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