namespace LiveScore.Features.League
{
    using ViewModels;
    using Views;
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