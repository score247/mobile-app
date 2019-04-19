namespace LiveScore.Menu
{
    using LiveScore.Menu.ViewModels;
    using LiveScore.Menu.Views;
    using Prism.Ioc;
    using Prism.Modularity;

    public class MenuModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            // OnInitialized
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<RefreshView, RefreshViewModel>();
            containerRegistry.RegisterForNavigation<DefaultSportView, DefaultSportViewModel>();
            containerRegistry.RegisterForNavigation<DefaultLanguageView, DefaultLanguageViewModel>();
            containerRegistry.RegisterForNavigation<InfoAlertView, InfoAlertViewModel>();
            containerRegistry.RegisterForNavigation<FAQView, FaqPageViewModel>();
        }
    }
}