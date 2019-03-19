namespace League
{
    using League.Services;
    using League.ViewModels;
    using League.Views;
    using Prism.Ioc;
    using Prism.Modularity;

    public class LeagueModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<ILeagueService, LeagueService>();
            containerRegistry.RegisterForNavigation<LeagueView, LeagueViewModel>();
            containerRegistry.RegisterForNavigation<LeagueDetailView, LeagueDetailViewModel>();
        }
    }
}