using Common.LangResources;
using LiveScoreApp.Services;
using LiveScoreApp.ViewModels;
using LiveScoreApp.Views;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Plugin.Multilingual;
using Prism;
using Prism.Ioc;
using Prism.Logging;
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
            await NavigationService.NavigateAsync("MasterDetailPage/MenuTabbedPage");

            AppCenter.Start("ios=78840378-a040-416b-84c6-91390b3edb55;", typeof(Analytics), typeof(Crashes));
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<ILoggerFacade, LogService>();
            containerRegistry.Register<IMenuService, MenuService>();
            containerRegistry.Register<ISportService, SportService>();

            containerRegistry.RegisterForNavigation<MainPage>();
            containerRegistry.RegisterForNavigation<MenuTabbedPage>();
            containerRegistry.RegisterForNavigation<Views.MasterDetailPage>();
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<SelectSportPage, SelectSportPageViewModel>();
            ViewModelLocationProvider.Register<NavigationTitleView, NavigationTitleViewViewModel>();
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<Common.CommonModule>();
            moduleCatalog.AddModule<League.LeagueModule>();
            moduleCatalog.AddModule<Score.ScoreModule>();
            moduleCatalog.AddModule<Favorites.FavoritesModule>();
            moduleCatalog.AddModule<News.NewsModule>();
            moduleCatalog.AddModule<Setting.SettingModule>();
        }
    }
}