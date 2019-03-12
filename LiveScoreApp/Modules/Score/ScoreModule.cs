namespace Score
{
    using Prism.Ioc;
    using Prism.Modularity;
    using Score.Views;
    using Score.ViewModels;

    public class ScoreModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<ScorePage, ScorePageViewModel>();
            containerRegistry.RegisterForNavigation<MatchInfoPage, MatchInfoPageViewModel>();
            containerRegistry.RegisterForNavigation<MatchTrackerPage, MatchTrackerPageViewModel>();
            containerRegistry.RegisterForNavigation<LivePage, LivePageViewModel>();
            containerRegistry.RegisterForNavigation<CalendarPage, CalendarPageViewModel>();
        }
    }
}