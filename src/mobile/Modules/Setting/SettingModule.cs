using Prism.Ioc;
using Prism.Modularity;
using Setting.ViewModels;
using Setting.Views;

namespace Setting
{
    public class SettingModule : IModule
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