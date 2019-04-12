namespace LiveScore.League
{
    using LiveScore.League.ViewModels;
    using LiveScore.League.Views;
    using Prism.Ioc;
    using Prism.Modularity;

    public class LeagueModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            // Initialize module
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<LeagueView, LeagueViewModel>();
            containerRegistry.RegisterForNavigation<LeagueDetailView, LeagueDetailViewModel>();
        }
    }
}