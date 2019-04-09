namespace LiveScore.Soccer
{
    using LiveScore.Common.Services;
    using LiveScore.Core.Factories;
    using LiveScore.Core.Services;
    using LiveScore.Soccer.Factories;
    using Prism.Ioc;
    using Prism.Modularity;

    public class SoccerModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var matchApi = containerProvider.Resolve<IMatchApi>();
            var leagueApi = containerProvider.Resolve<ILeagueApi>();
            var settingsService = containerProvider.Resolve<ISettingsService>();
            var cacheService = containerProvider.Resolve<ICacheService>();
            var loggingService = containerProvider.Resolve<ILoggingService>();
            var apiPolicy = containerProvider.Resolve<IApiPolicy>();
            var globalServiceProvider = containerProvider.Resolve<IGlobalFactoryProvider>();

            var soccerServiceFactory = new SoccerServiceFactory(matchApi, leagueApi, settingsService, cacheService, loggingService, apiPolicy);
            soccerServiceFactory.RegisterTo(globalServiceProvider.SportServiceFactoryProvider);
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //
        }
    }
}
