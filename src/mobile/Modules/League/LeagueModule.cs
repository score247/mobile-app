namespace League
{
    using System;
    using System.Net.Http;
    using Common.Helpers.Logging;
    using Common.Services;
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
            // Initialize module
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            var settingService = new SettingsService();

            containerRegistry.RegisterInstance(
                RestService.For<ILeagueApi>(new HttpClient(new HttpLoggingHandler())
                {
                    BaseAddress = new Uri(settingService.ApiEndPoint)
                }));

            containerRegistry.Register<ILeagueService, LeagueService>();
            containerRegistry.RegisterForNavigation<LeagueView, LeagueViewModel>();
            containerRegistry.RegisterForNavigation<LeagueDetailView, LeagueDetailViewModel>();
        }
    }
}