using Prism;
using Prism.Ioc;
using LiveScoreApp.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Prism.Modularity;
using LiveScoreApp.Services;

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
            InitializeComponent();

            await NavigationService.NavigateAsync("MasterDetailPage/MenuTabbedPage");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<IMenuService, MenuService>();

            containerRegistry.RegisterForNavigation<MainPage>();
            containerRegistry.RegisterForNavigation<MenuTabbedPage>();
            containerRegistry.RegisterForNavigation<Views.MasterDetailPage>();
            containerRegistry.RegisterForNavigation<NavigationPage>();
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<Tournament.TournamentModule>();
            moduleCatalog.AddModule<Score.ScoreModule>();
            moduleCatalog.AddModule<Setting.SettingModule>();
        }
    }
}