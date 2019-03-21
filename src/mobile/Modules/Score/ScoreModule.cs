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
            // Method intentionally left empty.
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterInstance(
              RestService.For<IMatchApi>(
                  new HttpClient(new HttpLoggingHandler())
                  {
                      BaseAddress = new Uri(Settings.ApiEndPoint)
                  }));
            containerRegistry.Register<IMatchService, MatchService>();

            containerRegistry.RegisterForNavigation<ScoresView, ScoresViewModel>();
            containerRegistry.RegisterForNavigation<MatchDetailView, MatchDetailViewModel>();
            containerRegistry.RegisterForNavigation<LiveView, LiveViewModel>();
        }
    }
}