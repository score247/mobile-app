namespace LiveScore.Soccer.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using LiveScore.Common.Extensions;
    using LiveScore.Common.Services;
    using LiveScore.Core.Models.Matches;
    using LiveScore.Core.Models.Settings;
    using LiveScore.Core.Services;
    using LiveScore.Soccer.Models.Matches;
    using Microsoft.AspNetCore.SignalR.Client;
    using Newtonsoft.Json;
    using Refit;

    public interface ISoccerMatchApi
    {
        [Get("/soccer/{language}/matches?fd={fromDate}&td={toDate}&tz={timeZone}")]
        Task<IEnumerable<Match>> GetMatches(string sportId, string language, string fromDate, string toDate, string timeZone);

        [Get("/soccer/{language}/matches/{matchId}")]
        Task<Match> GetMatch(string sportId, string matchId, string timeZone, string language);
    }

    public class MatchService : BaseService, IMatchService
    {
        private const string PushMatchEvent = "MatchEvent";
        private readonly ILocalStorage cacheService;
        private readonly IApiService apiService;

        public MatchService(
            ILocalStorage cacheService,
            ILoggingService loggingService,
            IApiService apiService
            ) : base(loggingService)
        {
            this.cacheService = cacheService;
            this.apiService = apiService;
        }

        public async Task<IEnumerable<IMatch>> GetMatches(UserSettings settings, DateRange dateRange, bool forceFetchNewData = false)
        {
            try
            {
                var cacheExpiration = dateRange.ToDate <= DateTime.Today
                   ? cacheService.CacheDuration(CacheDurationTerm.Long)
                   : cacheService.CacheDuration(CacheDurationTerm.Short);
                var fromDateText = dateRange.FromDate.ToApiFormat();
                var toDateText = dateRange.ToDate.ToApiFormat();
                var cacheKey = $"Scores-{settings}-{fromDateText}-{toDateText}";

                return await cacheService.GetAndFetchLatestValue(
                        cacheKey,
                        () => GetMatchesFromApi(settings, fromDateText, toDateText),
                        (offset) =>
                        {
                            if (forceFetchNewData)
                            {
                                return true;
                            }

                            var elapsed = DateTimeOffset.Now - offset;

                            return elapsed > cacheExpiration;
                        });
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
                var cacheKey = $"Match-{settings}-{matchId}";

                var match = await cacheService.GetAndFetchLatestValue(
                        cacheKey,
                        () => GetMatchFromApi(settings, matchId),
                        (offset) =>
                        {
                            if (forceFetchNewData)
                            {
                                return true;
                            }

                            var elapsed = DateTimeOffset.Now - offset;

                            return elapsed > TimeSpan.FromSeconds(30);
                        });

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
                () => apiService.GetApi<ISoccerMatchApi>().GetMatches(settings.SportId, settings.Language, fromDateText, toDateText, settings.TimeZone)
            );

        private async Task<Match> GetMatchFromApi(UserSettings settings, string matchId)
           => await apiService.Execute
           (
               () => apiService.GetApi<ISoccerMatchApi>().GetMatch(settings.SportId, matchId, settings.TimeZone, settings.Language)
           );
    }
}