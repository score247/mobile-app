namespace LiveScore.Soccer.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using LiveScore.Common.Services;
    using LiveScore.Core.Models.Leagues;
    using LiveScore.Core.Services;
    using Polly;
    using Refit;

    public class LeagueService : BaseService, ILeagueService
    {
        private readonly ILeagueApi leagueApi;
        private readonly ISettingsService settingsService;
        private readonly INetworkService networkService;

        public LeagueService(
            ILeagueApi leagueApi,
            ISettingsService settingsService,
            ILoggingService loggingService,
            INetworkService networkService) : base(loggingService)
        {
            this.settingsService = settingsService;
            this.networkService = networkService;

            this.leagueApi = leagueApi ?? RestService.For<ILeagueApi>(SettingsService.ApiEndPoint);
        }

        public async Task<IList<LeagueItem>> GetLeagues()
        {
            var leagues = await networkService.WaitAndRetry
                (
                    () => GetLeagueItems(),
                    sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                );

            return leagues;
        }

        private async Task<IList<LeagueItem>> GetLeagueItems()
        {
            try
            {
                var leagues = await leagueApi.GetLeagues(
                     settingsService.CurrentSportId,
                     settingsService.CurrentLanguage).ConfigureAwait(false);

                return leagues;
            }
            catch (Exception ex)
            {
                HandleException(ex);

                return Enumerable.Empty<League>();
            }
        }

    }
}
