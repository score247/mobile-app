namespace LiveScore.Basketball.Services
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using AutoMapper;
    using LiveScore.Basketball.DTOs.Matches;
    using LiveScore.Common.Extensions;
    using LiveScore.Common.Services;
    using LiveScore.Core.Models.Matches;
    using LiveScore.Core.Models.Settings;
    using LiveScore.Core.Services;
    using Refit;

    public interface IBasketballMatchApi
    {
        [Get("/Match/GetMatches?sportId={sportId}&from={fromDate}&to={toDate}&timeZone={timezone}&language={language}")]
        Task<IEnumerable<MatchDto>> GetMatches(int sportId, string language, string fromDate, string toDate, string timezone);
    }

    public class MatchService : BaseService, IMatchService
    {
        private readonly IBasketballMatchApi basketballMatchApi;
        private readonly ICacheService cacheService;
        private readonly IMapper mapper;
        private readonly IApiPolicy apiPolicy;

        public MatchService(
            IBasketballMatchApi basketballMatchApi,
            ICacheService cacheService,
            ILoggingService loggingService,
            IMapper mapper,
            IApiPolicy apiPolicy
            ) : base(loggingService)
        {
            this.cacheService = cacheService;
            this.basketballMatchApi = basketballMatchApi;
            this.mapper = mapper;
            this.apiPolicy = apiPolicy;
        }

        public async Task<IList<IMatch>> GetMatches(UserSettings settings, DateRange dateRange, bool forceFetchNewData = false)
        {
            var fromDateText = dateRange.FromDate.ToApiFormat();
            var toDateText = dateRange.ToDate.ToApiFormat();
            var matches = new List<IMatch>();
            var cacheKey = $"Scores-{settings.ToString()}-{fromDateText}-{toDateText}";
            var cacheExpiration = dateRange.FromDate < DateTime.Now
               ? cacheService.CacheDuration(CacheDurationTerm.Long)
               : cacheService.CacheDuration(CacheDurationTerm.Short);

            // TODO Refactor DTO later
            var dtoMatches = await cacheService.GetAndFetchLatestValue(
                    cacheKey,
                    () => GetMatches(settings, fromDateText, toDateText),
                    forceFetchNewData,
                    cacheExpiration);

            foreach (var dtoMatch in dtoMatches)
            {
                var match = mapper.Map<MatchDto, Match>(dtoMatch);
                matches.Add(match);
            }

            return matches;
        }

        private async Task<IEnumerable<MatchDto>> GetMatches(UserSettings settings, string fromDateText, string toDateText)
        {
            Debug.WriteLine($"GetMatches for {fromDateText} - {toDateText}");

            return await apiPolicy.RetryAndTimeout
                  (
                      () =>
                      {
                          Debug.WriteLine($"GetMatchesApi for {fromDateText} - {toDateText}");

                          return basketballMatchApi.GetMatches(settings.SportId, settings.Language, fromDateText, toDateText, settings.TimeZone);
                      }
                  );
        }

        public Task<IList<IMatch>> GetMatchesByLeague(string leagueId, string group)
        {
            throw new NotImplementedException();
        }

        public Task<IList<IMatch>> GetLiveMatches(int sportId, string language)
        {
            throw new NotImplementedException();
        }
    }
}
