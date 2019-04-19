namespace LiveScore.Soccer.Services
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using AutoMapper;
    using LiveScore.Common.Extensions;
    using LiveScore.Common.Services;
    using LiveScore.Core.Models.Matches;
    using LiveScore.Core.Services;
    using LiveScore.Soccer.DTOs.Matches;
    using Refit;

    public interface ISoccerMatchApi
    {
        [Get("/Match/GetMatches?sportId={sportId}&from={fromDate}&to={toDate}&timeZone={timezone}&language={language}")]
        Task<IEnumerable<MatchDTO>> GetMatches(int sportId, string language, string fromDate, string toDate, string timezone);
    }

    public class MatchService : BaseService, IMatchService
    {
        private const int TodayMatchExpiration = 2;
        private const int OldMatchExpiration = 120;
        private readonly ISoccerMatchApi soccerMatchApi;
        private readonly ICacheService cacheService;
        private readonly IMapper mapper;
        private readonly IApiPolicy apiPolicy;

        public MatchService(
            ISoccerMatchApi soccerMatchApi,
            ICacheService cacheService,
            ILoggingService loggingService,
            IMapper mapper,
            IApiPolicy apiPolicy
            ) : base(loggingService)
        {
            this.cacheService = cacheService;
            this.soccerMatchApi = soccerMatchApi;
            this.mapper = mapper;
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

        public async Task<IList<IMatch>> GetMatches(int sportId, string language, DateRange dateRange, string timezone, bool forceFetchNewData = false)
        {
            var matches = new List<IMatch>();
            var cacheExpiration = dateRange.FromDate < DateTime.Now
                ? DateTime.Now.AddMinutes(OldMatchExpiration)
                : DateTime.Now.AddMinutes(TodayMatchExpiration);
            var fromDateText = dateRange.FromDate.ToApiFormat();
            var toDateText = dateRange.ToDate.ToApiFormat();

            try
            {
                // TODO Refactor DTO later
                var dtoMatches = await cacheService.GetAndFetchLatestValue(
                        $"DailyMatches-{sportId}-{language}-{fromDateText}-{toDateText}",
                        () => GetMatches(sportId, language, fromDateText, toDateText, timezone),
                        forceFetchNewData,
                        cacheExpiration);

                foreach (var dtoMatch in dtoMatches)
                {
                    var match = mapper.Map<MatchDTO, Match>(dtoMatch);
                    matches.Add(match);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return matches;
        }

        private async Task<IEnumerable<MatchDTO>> GetMatches(int sportId, string language, string fromDateText, string toDateText, string timezone)
        {
            Debug.WriteLine("GetMatches");

            return await apiPolicy.RetryAndTimeout
                  (
                      () => soccerMatchApi.GetMatches(sportId, language, fromDateText, toDateText, timezone)
                  );
        }
    }
}