using LiveScore.Features.League.ViewModels;
using LiveScore.Features.League.Views;
using Prism.Ioc;
using Prism.Modularity;

namespace LiveScore.Features.League
{
    public class LeagueModule : IModule
    {
        public const string ModuleName = nameof(LeaguesView);

        public void OnInitialized(IContainerProvider containerProvider)
        {
            // Initialize module
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<LeaguesView, LeaguesViewModel>();
            containerRegistry.RegisterForNavigation<LeaguesOfCountryView, LeaguesOfCountryViewModel>();
            containerRegistry.RegisterForNavigation<LeagueGroupStagesView, LeagueGroupStagesViewModel>();
        }
    }
}