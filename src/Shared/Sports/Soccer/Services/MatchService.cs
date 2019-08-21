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
    using LiveScore.Core.Models.Teams;
    using LiveScore.Core.Services;
    using LiveScore.Soccer.Models.Matches;
    using LiveScore.Soccer.Models.Teams;
    using MethodTimer;
    using Microsoft.AspNetCore.SignalR.Client;
    using Newtonsoft.Json;
    using Refit;

    public interface ISoccerMatchApi
    {
        [Get("/soccer/{language}/matches?fd={fromDate}&td={toDate}")]
        Task<IEnumerable<Match>> GetMatches(string fromDate, string toDate, string language);

        [Get("/soccer/{language}/matches/{matchId}")]
        Task<MatchOld> GetMatch(string matchId, string language);
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

        [Time]
        public async Task<IEnumerable<IMatch>> GetMatches(DateRange dateRange, Language language, bool forceFetchNewData = false)
        {
            try
            {
                var dataFromDate = await GetMatchesByDate(dateRange.From, language, forceFetchNewData);

                if (dateRange.IsOneDay)
                {
                    return dataFromDate;
                }
                else
                {
                    var dataToDate = await GetMatchesByDate(dateRange.To, language, forceFetchNewData);

                    return dataFromDate.Concat(dataToDate);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);

                return Enumerable.Empty<IMatch>();
            }
        }

        [Time]
        private async Task<IEnumerable<IMatch>> GetMatchesByDate(DateTime dateTime, Language language, bool forceFetchNewData = false)
        {
            try
            {
                var cacheKey = $"Matches:{dateTime.Date}:{language.DisplayName}";

                if (dateTime.Date == DateTime.Today
                    || dateTime.Date == DateTimeExtension.Yesterday().Date)
                {
                    return await cacheService.GetAndFetchLatestValue(
                        cacheKey,
                        () => GetMatchesFromApi(
                            dateTime.BeginningOfDay().ToApiFormat(),
                            dateTime.EndOfDay().ToApiFormat(),
                            language.DisplayName),
                        cacheService.GetFetchPredicate(forceFetchNewData, (int)CacheDuration.Short));
                }

                return await cacheService.GetOrFetchValue(
                       cacheKey,
                       () => GetMatchesFromApi(
                           dateTime.BeginningOfDay().ToApiFormat(),
                           dateTime.EndOfDay().ToApiFormat(),
                           language.DisplayName), DateTime.Now.AddSeconds((int)CacheDuration.Long));
            }
            catch (Exception ex)
            {
                HandleException(ex);

                return Enumerable.Empty<IMatch>();
            }
        }

        [Time]
        public async Task<IMatchOld> GetMatch(string matchId, Language language, bool forceFetchNewData = false)
        {
            try
            {
                var cacheKey = $"Match:{matchId}:{language}";

                var match = await cacheService.GetAndFetchLatestValue(
                        cacheKey,
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

        [Time]
        public void SubscribeMatchEvent(HubConnection hubConnection, Action<byte, IMatchEvent> handler)
        {
            hubConnection.On<byte, string>(PushMatchEvent, (sportId, payload) =>
            {
                var matchEvent = JsonConvert.DeserializeObject<MatchEvent>(payload);

                handler.Invoke(sportId, matchEvent);
            });
        }

        [Time]
        public void SubscribeTeamStatistic(HubConnection hubConnection, Action<byte, string, bool, ITeamStatistic> handler)
        {
            hubConnection.On<byte, string, bool, string>(PushTeamStatistic, (sportId, matchId, isHome, payload) =>
            {
                var teamStats = JsonConvert.DeserializeObject<TeamStatistic>(payload);

                handler.Invoke(sportId, matchId, isHome, teamStats);
            });
        }

        [Time]
        private async Task<IEnumerable<Match>> GetMatchesFromApi(string fromDateText, string toDateText, string language)
            => await apiService.Execute
            (
                () => apiService.GetApi<ISoccerMatchApi>().GetMatches(fromDateText, toDateText, language)
            );

        [Time]
        private async Task<MatchOld> GetMatchFromApi(string matchId, string language)
           => await apiService.Execute
           (
               () => apiService.GetApi<ISoccerMatchApi>().GetMatch(matchId, language)
           );
    }
}