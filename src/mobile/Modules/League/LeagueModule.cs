namespace League
{
    using System;
    using System.Net.Http;
    using Common.Helpers;
    using Common.Settings;
    using League.Services;
    using League.ViewModels;
    using League.Views;
    using Prism.Ioc;
    using Prism.Modularity;
    using Refit;

    public class LeagueModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterInstance(
              RestService.For<ILeagueApi>(
                  new HttpClient(new HttpLoggingHandler())
                  {
                      BaseAddress = new Uri(Settings.ApiEndPoint)
                  }));

            containerRegistry.Register<ILeagueService, LeagueService>();
            containerRegistry.RegisterForNavigation<LeagueView, LeagueViewModel>();
            containerRegistry.RegisterForNavigation<LeagueDetailView, LeagueDetailViewModel>();
        }
    }
}