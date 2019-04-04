namespace Score
{
    using System;
    using System.Net.Http;
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
                  new HttpClient
                  {
                      BaseAddress = new Uri(settingsService.ApiEndPoint)
                  }));

            containerRegistry.Register<IMatchService, MatchService>();
            containerRegistry.Register<ISettingsService, SettingsService>();
            containerRegistry.Register<IDeviceInfoService, DeviceInfoService>();
            containerRegistry.Register<ILoggingService, SentryLogger>();
            containerRegistry.RegisterForNavigation<ScoresView, ScoresViewModel>();
            containerRegistry.RegisterForNavigation<MatchDetailView, MatchDetailViewModel>();
            containerRegistry.RegisterForNavigation<LiveView, LiveViewModel>();
        }
    }
}