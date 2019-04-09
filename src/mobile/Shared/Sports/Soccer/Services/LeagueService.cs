namespace LiveScore.Soccer.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using LiveScore.Common.Services;
    using LiveScore.Core.Models.Leagues;
    using LiveScore.Core.Services;

    public class LeagueService : BaseService, ILeagueService
    {
        private readonly ILeagueApi leagueApi;
        private readonly ISettingsService settingsService;
        private readonly IApiPolicy apiPolicy;

        public LeagueService(
            ILeagueApi leagueApi,
            ISettingsService settingsService,
            ILoggingService loggingService,
            IApiPolicy apiPolicy) : base(loggingService)
        {
            this.settingsService = settingsService;
            this.apiPolicy = apiPolicy;

            this.leagueApi = leagueApi;
        }

        public async Task<IEnumerable<ILeague>> GetLeagues()
        {
            IEnumerable<ILeague> leagues = Enumerable.Empty<ILeague>();

            try
            {
                leagues = await apiPolicy.RetryAndTimeout
                (
                    () => leagueApi.GetLeagues(settingsService.CurrentSportId, settingsService.CurrentLanguage)
                );

            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return leagues;
        }
    }
}
