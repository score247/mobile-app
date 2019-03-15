namespace League
{
    using Prism.Ioc;
    using Prism.Modularity;
    using League.Views;
    using League.ViewModels;
    using League.Services;

    public class LeagueModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<ILeagueService, LeagueService>();
            containerRegistry.RegisterForNavigation<LeaguePage, LeaguePageViewModel>();
            containerRegistry.RegisterForNavigation<LeagueDetailPage, LeagueDetailPageViewModel>();
        }
    }
}