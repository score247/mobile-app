namespace LiveScore.Score
{
    using ViewModels;
    using Views;
    using Prism.Ioc;
    using Prism.Modularity;

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