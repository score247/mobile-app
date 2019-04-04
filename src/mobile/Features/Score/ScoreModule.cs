namespace Score
{
    using System;
    using System.Net.Http;
    using Common.Helpers.Logging;
    using Core.Services;
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
            var settingsService = new SettingsService();

            containerRegistry.RegisterInstance(
              RestService.For<IMatchApi>(
                  new HttpClient(new HttpLoggingHandler())
                  {
                      BaseAddress = new Uri(settingsService.ApiEndPoint)
                  }));

            containerRegistry.Register<Score.Services.IMatchService, MatchService>();
            containerRegistry.RegisterForNavigation<ScoresView, ScoresViewModel>();
            containerRegistry.RegisterForNavigation<MatchDetailView, MatchDetailViewModel>();
            containerRegistry.RegisterForNavigation<LiveView, LiveViewModel>();
        }
    }
}