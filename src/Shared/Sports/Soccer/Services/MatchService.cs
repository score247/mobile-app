namespace LiveScore.Soccer.Services
{
    using LiveScore.Common.Extensions;
    using LiveScore.Common.Services;
    using LiveScore.Core.Models.Matches;
    using LiveScore.Core.Models.Settings;
    using LiveScore.Core.Services;
    using LiveScore.Soccer.Models.Matches;
    using Microsoft.AspNetCore.SignalR.Client;
    using Newtonsoft.Json;
    using Refit;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public interface ISoccerMatchApi
    {
        [Get("/soccer/{language}/matches?fd={fromDate}&td={toDate}&tz={timeZone}")]
        Task<IEnumerable<Match>> GetMatches(string language, string fromDate, string toDate, string timeZone);

        [Get("/soccer/{language}/matches/{matchId}")]
        Task<Match> GetMatch(string matchId, string language);
    }

    public class MatchService : BaseService, IMatchService
    {
        private const string PushMatchEvent = "MatchEvent";
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

        public async Task<IEnumerable<IMatch>> GetMatches(UserSettings settings, DateRange dateRange, bool forceFetchNewData = false)
        {
            try
            {
                var cacheDuration = dateRange.ToDate < DateTime.Today
                   ? (int)CacheDuration.Long
                   : (int)CacheDuration.Short;

                var matchListDataCacheKey = $"Matches-{settings}-{dateRange}";

                return await cacheService.GetAndFetchLatestValue(
                        matchListDataCacheKey,
                        () => GetMatchesFromApi(settings, dateRange.FromDateString, dateRange.ToDateString), 
                        cacheService.GetFetchPredicate(forceFetchNewData, cacheDuration));
            }
            catch (Exception ex)
            {
                HandleException(ex);

                return Enumerable.Empty<IMatch>();
            }
        }

        public async Task<IMatch> GetMatch(UserSettings settings, string matchId, bool forceFetchNewData = false)
        {
            try
            {
                var matchDataCacheKey = $"Match-{settings}-{matchId}";

                var match = await cacheService.GetAndFetchLatestValue(
                        matchDataCacheKey,
                        () => GetMatchFromApi(settings, matchId),
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

        private async Task<IEnumerable<Match>> GetMatchesFromApi(UserSettings settings, string fromDateText, string toDateText)
            => await apiService.Execute
            (
                () => apiService.GetApi<ISoccerMatchApi>().GetMatches(settings.Language, fromDateText, toDateText, settings.TimeZone)
            );

        private async Task<Match> GetMatchFromApi(UserSettings settings, string matchId)
           => await apiService.Execute
           (
               () => apiService.GetApi<ISoccerMatchApi>().GetMatch(matchId, settings.Language)
           );
    }
}