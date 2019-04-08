namespace LiveScore.Soccer.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using LiveScore.Common.Services;
    using LiveScore.Core.Models.Leagues;
    using LiveScore.Core.Services;
    using Refit;

    public class LeagueService : BaseService, ILeagueService
    {
        private readonly ILeagueApi leagueApi;
        private readonly ISettingsService settingsService;

        public LeagueService(
            ILeagueApi leagueApi,
            ISettingsService settingsService,
            ILoggingService loggingService) : base(loggingService)
        {
            this.settingsService = settingsService;
            this.leagueApi = leagueApi;
        }

        public async Task<IEnumerable<League>> GetLeagues()
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
