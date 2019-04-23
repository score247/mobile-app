namespace LiveScore.Soccer.Services
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    using LiveScore.Common.Extensions;
    using LiveScore.Common.Services;
    using LiveScore.Core.Models.Matches;
    using LiveScore.Core.Models.Settings;
    using LiveScore.Core.Services;
    using Refit;

    public interface ISoccerMatchApi
    {
        [Get("/Match/GetMatches?sportId={sportId}&from={fromDate}&to={toDate}&timeZone={timezone}&language={language}")]
        Task<IEnumerable<Match>> GetMatches(int sportId, string language, string fromDate, string toDate, string timezone);
    }

    public class MatchService : BaseService, IMatchService
    {
        private readonly ISoccerMatchApi soccerMatchApi;
        private readonly ILocalStorage cacheService;
        private readonly IApiPolicy apiPolicy;

        public MatchService(
            ISoccerMatchApi soccerMatchApi,
            ILocalStorage cacheService,
            ILoggingService loggingService,
            IApiPolicy apiPolicy
            ) : base(loggingService)
        {
            this.cacheService = cacheService;
            this.soccerMatchApi = soccerMatchApi;
            this.apiPolicy = apiPolicy;
        }

        public Task<IList<IMatch>> GetLiveMatches(int sportId, string language)
        {
            throw new NotImplementedException();
        }

        public Task<IList<IMatch>> GetMatchesByLeague(
            string leagueId,
            string group)
        {
            throw new NotImplementedException();
        }

        public async Task<IList<IMatch>> GetMatches(UserSettings settings, DateRange dateRange, bool forceFetchNewData = false)
        {
            try
            {
                var cacheExpiration = dateRange.FromDate < DateTime.Now
                   ? cacheService.CacheDuration(CacheDurationTerm.Long)
                   : cacheService.CacheDuration(CacheDurationTerm.Short);

                var fromDateText = dateRange.FromDate.ToApiFormat();
                var toDateText = dateRange.ToDate.ToApiFormat();

                var cacheKey = $"Scores-{settings.ToString()}-{fromDateText}-{toDateText}";

                Debug.WriteLine($"cacheKey {cacheKey}");

                var matches = await cacheService.GetAndFetchLatestValue(
                        cacheKey,
                        () => GetMatches(settings, fromDateText, toDateText),
                        forceFetchNewData,
                        cacheExpiration) as IEnumerable<IMatch>;

                return matches.ToList();

            }
            catch (Exception ex)
            {
                HandleException(ex);

                return Enumerable.Empty<IMatch>().ToList();
            }
        }

        private async Task<IEnumerable<Match>> GetMatches(UserSettings settings, string fromDateText, string toDateText)
            => await apiPolicy.RetryAndTimeout(
                () => soccerMatchApi.GetMatches(settings.SportId, settings.Language, fromDateText, toDateText, settings.TimeZone));
    }
}