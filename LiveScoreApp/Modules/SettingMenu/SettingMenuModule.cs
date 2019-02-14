using Prism.Ioc;
using Prism.Modularity;
using SettingMenu.ViewModels;
using SettingMenu.Views;

namespace SettingMenu
{
    public class SettingMenuModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {

        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<AutomaticRefreshPage, AutomaticRefreshPageViewModel>();
        }
    }
}
