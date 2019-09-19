using Prism.Ioc;
using Prism.Modularity;
using LiveScore.Features.Score.ViewModels;
using LiveScore.Features.Score.Views;

namespace LiveScore.Features.Score
{
    public class ScoreModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            // Method intentionally left empty.
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<ScoresView, ScoresViewModel>();
            containerRegistry.RegisterForNavigation<LiveView, LiveViewModel>();
        }
    }
}