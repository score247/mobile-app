using Prism.Ioc;
using Prism.Modularity;
using Tournament.Views;
using Tournament.ViewModels;
using Tournament.Services;

namespace Tournament
{
    public class TournamentModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<ITournamentService, TournamentService>();
            containerRegistry.RegisterForNavigation<TournamentPage, TournamentPageViewModel>();
            containerRegistry.RegisterForNavigation<TournamentDetailPage, TournamentDetailPageViewModel>();
        }
    }
}