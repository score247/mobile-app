namespace Score
{
    using System;
    using System.Net.Http;
    using Common.Helpers;
    using Common.Settings;
    using Prism.Ioc;
    using Prism.Modularity;
    using Refit;
    using Score.Services;
    using Score.ViewModels;
    using Score.Views;

    public class ScoreModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterInstance(
              RestService.For<IMatchApi>(
                  new HttpClient(new HttpLoggingHandler())
                  {
                      BaseAddress = new Uri(Settings.BaseSportRadarEndPoint)
                  }));
            containerRegistry.Register<IMatchService, MatchService>();

            containerRegistry.RegisterForNavigation<ScorePage, ScorePageViewModel>();
            containerRegistry.RegisterForNavigation<MatchInfoPage, MatchInfoPageViewModel>();
            containerRegistry.RegisterForNavigation<MatchTrackerPage, MatchTrackerPageViewModel>();
            containerRegistry.RegisterForNavigation<LivePage, LivePageViewModel>();
            containerRegistry.RegisterForNavigation<CalendarPage, CalendarPageViewModel>();


        }
    }
}