namespace LiveScore.Score
{
    using Prism.Ioc;
    using Prism.Modularity;
    using LiveScore.Score.ViewModels;
    using LiveScore.Score.Views;

    public class ScoreModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            // Method intentionally left empty.
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<ScoresView, ScoresViewModel>();
            containerRegistry.RegisterForNavigation<MatchDetailView, MatchDetailViewModel>();
            containerRegistry.RegisterForNavigation<LiveView, LiveViewModel>();
        }
    }
}