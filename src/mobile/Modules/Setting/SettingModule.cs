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
            containerRegistry.RegisterForNavigation<RefreshPage, RefreshPageViewModel>();
            containerRegistry.RegisterForNavigation<DefaultSportPage, DefaultSportPageViewModel>();
            containerRegistry.RegisterForNavigation<DefaultLanguagePage, DefaultLanguagePageViewModel>();
            containerRegistry.RegisterForNavigation<InfoAlertPage, InfoAlertPageViewModel>();
            containerRegistry.RegisterForNavigation<FAQPage, FAQPageViewModel>();
        }
    }
}