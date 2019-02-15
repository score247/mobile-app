using Prism.Ioc;
using Prism.Modularity;
using Setting.Views;
using Setting.ViewModels;

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
        }
    }
}