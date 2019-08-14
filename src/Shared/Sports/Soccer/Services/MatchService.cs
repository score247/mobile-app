namespace LiveScore.Soccer.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using LiveScore.Common.Extensions;
    using LiveScore.Common.Services;
    using LiveScore.Core.Enumerations;
    using LiveScore.Core.Models.Matches;
    using LiveScore.Core.Models.Settings;
    using LiveScore.Core.Models.Teams;
    using LiveScore.Core.Services;
    using LiveScore.Soccer.Models.Matches;
    using LiveScore.Soccer.Models.Teams;
    using Microsoft.AspNetCore.SignalR.Client;
    using Newtonsoft.Json;
    using Refit;

    public interface ISoccerMatchApi
    {
        [Get("/soccer/{language}/matches?fd={fromDate}&td={toDate}")]
        Task<IEnumerable<Match>> GetMatches(string fromDate, string toDate, string language);

        [Get("/soccer/{language}/matches/{matchId}")]
        Task<Match> GetMatch(string matchId, string language);
    }

    public class MatchService : BaseService, IMatchService
    {
        private const string PushMatchEvent = "MatchEvent";
        private const string PushTeamStatistic = "TeamStatistic";
        private readonly IApiService apiService;
        private readonly ICachingService cacheService;

        public MatchService(
            IApiService apiService,
            ICachingService cacheService,
            ILoggingService loggingService) : base(loggingService)
        {
            this.apiService = apiService;
            this.cacheService = cacheService;
        }

        public async Task<IEnumerable<IMatch>> GetMatches(DateRange dateRange, Language language, bool forceFetchNewData = false)
        {
            try
            {
                var cacheDuration = dateRange.ToDate.Date != DateTime.Today
                   ? (int)CacheDuration.Long
                   : (int)CacheDuration.Short;

                var matchListDataCacheKey = $"GetMatches:{dateRange}:{language.DisplayName}";

                return await cacheService.GetAndFetchLatestValue(
                        matchListDataCacheKey,
                        () => GetMatchesFromApi(dateRange.FromDateString, dateRange.ToDateString, language.DisplayName),
                        cacheService.GetFetchPredicate(forceFetchNewData, cacheDuration));
            }
            catch (Exception ex)
            {
                HandleException(ex);

                return Enumerable.Empty<IMatch>();
            }
        }

        public async Task<IMatch> GetMatch(string matchId, Language language, bool forceFetchNewData = false)
        {
            try
            {
                var matchDataCacheKey = $"GetMatch:{matchId}:{language}";

                var match = await cacheService.GetAndFetchLatestValue(
                        matchDataCacheKey,
                        () => GetMatchFromApi(matchId, language.DisplayName),
                        cacheService.GetFetchPredicate(forceFetchNewData, (int)CacheDuration.Short));

                return match;
            }
            catch (Exception ex)
            {
                HandleException(ex);

                return null;
            }
        }

        public void SubscribeMatchEvent(HubConnection hubConnection, Action<byte, IMatchEvent> handler)
        {
            hubConnection.On<byte, string>(PushMatchEvent, (sportId, payload) =>
            {
                var matchEvent = JsonConvert.DeserializeObject<MatchEvent>(payload);

                handler.Invoke(sportId, matchEvent);
            });
        }

        public void SubscribeTeamStatistic(HubConnection hubConnection, Action<byte, string, bool, ITeamStatistic> handler)
        {
            hubConnection.On<byte, string, bool, string>(PushTeamStatistic, (sportId, matchId, isHome, payload) =>
            {
                var teamStats = JsonConvert.DeserializeObject<TeamStatistic>(payload);

                handler.Invoke(sportId, matchId, isHome, teamStats);
            });
        }

        private async Task<IEnumerable<Match>> GetMatchesFromApi(string fromDateText, string toDateText, string language)
            => await apiService.Execute
            (
                () => apiService.GetApi<ISoccerMatchApi>().GetMatches(fromDateText, toDateText, language)
            );

        private async Task<Match> GetMatchFromApi(string matchId, string language)
           => await apiService.Execute
           (
               () => apiService.GetApi<ISoccerMatchApi>().GetMatch(matchId, language)
           );
    }
}