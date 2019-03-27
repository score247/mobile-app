namespace Menu
{
    using Prism.Ioc;
    using Prism.Modularity;
    using Menu.ViewModels;
    using Menu.Views;

    public class MenuModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<RefreshView, RefreshViewModel>();
            containerRegistry.RegisterForNavigation<DefaultSportView, DefaultSportViewModel>();
            containerRegistry.RegisterForNavigation<DefaultLanguageView, DefaultLanguageViewModel>();
            containerRegistry.RegisterForNavigation<InfoAlertView, InfoAlertViewModel>();
            containerRegistry.RegisterForNavigation<FAQView, FAQPageViewModel>();
        }
    }
}