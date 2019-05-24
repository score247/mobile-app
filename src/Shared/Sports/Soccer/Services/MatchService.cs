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
    using Microsoft.AspNetCore.SignalR.Client;
    using Refit;

    public interface ISoccerMatchApi
    {
        [Get("/Match/GetMatches?sportId={sportId}&from={fromDate}&to={toDate}&timeZone={timezone}&language={language}")]
        Task<IEnumerable<Match>> GetMatches(string sportId, string language, string fromDate, string toDate, string timezone);
    }

    public class MatchService : BaseService, IMatchService
    {
        private const string PushMatchesMethod = "PushMatchEvent";
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
                        () => GetMatches(settings, fromDateText, toDateText),
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

        public void SubscribeMatches(
            HubConnection hubConnection,
            Action<string, Dictionary<string, MatchPayload>> handler)
        {
            hubConnection.On<int, Dictionary<string, MatchPayload>>(PushMatchesMethod, (sportId, payload) =>
            {
                handler.Invoke(sportId.ToString(), payload);
            });
        }

        private async Task<IEnumerable<Match>> GetMatches(UserSettings settings, string fromDateText, string toDateText)
            => await apiService.Execute
            (
                () => apiService.GetApi<ISoccerMatchApi>().GetMatches(settings.SportId, settings.Language, fromDateText, toDateText, settings.TimeZone)
            );
    }
}