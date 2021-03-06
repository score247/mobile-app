using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LiveScore.Common.Services;
using LiveScore.Core.Enumerations;
using LiveScore.Core.Models.Leagues;
using LiveScore.Core.Models.Matches;
using LiveScore.Core.Services;
using static LiveScore.Soccer.Services.SoccerApi;

namespace LiveScore.Soccer.Services
{
    public class LeagueService : BaseService, ILeagueService
    {
        private const int CacheDuration = 86_400;
        private readonly IApiService apiService;
        private readonly ICacheManager cacheManager;
        private readonly LeagueApi leagueApi;

        public LeagueService(
            IApiService apiService,
            ICacheManager cacheManager,
            ILoggingService loggingService,
            LeagueApi leagueApi = null) : base(loggingService)
        {
            this.apiService = apiService;
            this.cacheManager = cacheManager;
            this.leagueApi = leagueApi ?? apiService.GetApi<LeagueApi>();
        }

        public async Task<IEnumerable<ILeague>> GetMajorLeaguesAsync(Language language, bool forceFetchLatestData = false)
        {
            try
            {
                const string cacheKey = "LiveScore.Soccer.Services.LeagueService.MajorLeagues";

                return await cacheManager.GetOrSetAsync(
                            cacheKey,
                            () => apiService.Execute(() => leagueApi.GetMajorLeagues(language.DisplayName)),
                            CacheDuration,
                            forceFetchLatestData);
            }
            catch (Exception ex)
            {
                var properties = new Dictionary<string, string>
                {
                    { "Method", nameof(GetMajorLeaguesAsync)}
                };

                HandleException(ex, properties);

                return Enumerable.Empty<ILeague>();
            }
        }

        public async Task<ILeagueTable> GetTable(string leagueId, string seasonId, string leagueRoundGroup, Language language)
        {
            try
            {
                var leagueTable = await apiService.Execute(()
                    => leagueApi.GetTable(language.DisplayName, leagueId, seasonId, leagueRoundGroup ?? " "));

                return leagueTable;
            }
            catch (Exception ex)
            {
                var properties = new Dictionary<string, string>
                {
                    { "Method", nameof(GetTable)},
                    { "LeagueId", leagueId},
                    { "SeasonId", seasonId},
                    { "LeagueRoundGroup", leagueRoundGroup},
                };

                HandleException(ex, properties);

                return null;
            }
        }

        public async Task<IEnumerable<IMatch>> GetFixtures(string leagueId, string seasonId, string leagueGroupName, Language language)
        {
            try
            {
                var matches = await apiService.Execute(()
                    => leagueApi.GetFixtures(language.DisplayName, leagueId, seasonId, leagueGroupName));

                return matches;
            }
            catch (Exception ex)
            {
                var properties = new Dictionary<string, string>
                {
                    { "Method", nameof(GetFixtures)},
                    { "LeagueId", leagueId},
                    { "SeasonId", seasonId},
                    { "LeagueGroupName", leagueGroupName},
                };

                HandleException(ex, properties);

                return Enumerable.Empty<IMatch>();
            }
        }

        public async Task<IEnumerable<ILeague>> GetCountryLeagues(string countryCode, Language language)
        {
            try
            {
                var countryLeagues = await apiService.Execute(()
                       => leagueApi.GetCountryLeagues(language.DisplayName, countryCode));

                return countryLeagues;
            }
            catch (Exception ex)
            {
                var properties = new Dictionary<string, string>
                {
                    { "Method", nameof(GetCountryLeagues)},
                    { "CountryCode", countryCode}
                };

                HandleException(ex, properties);

                return Enumerable.Empty<ILeague>();
            }
        }

        public async Task<IEnumerable<ILeagueGroupStage>> GetLeagueGroupStages(string leagueId, string seasonId, Language language)
        {
            try
            {
                var countryLeagues = await apiService.Execute(()
                       => leagueApi.GetLeagueGroupStages(language.DisplayName, leagueId, seasonId));

                return countryLeagues;
            }
            catch (Exception ex)
            {
                var properties = new Dictionary<string, string>
                {
                    { "Method", nameof(GetLeagueGroupStages)},
                    { "LeagueId", leagueId},
                    { "SeasonId", seasonId}
                };

                HandleException(ex, properties);

                return Enumerable.Empty<ILeagueGroupStage>();
            }
        }
    }
}