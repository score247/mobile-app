namespace LiveScore.Soccer.Services
{
    using System;
    using System.Collections.Generic;
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
        [Get("/Match/GetMatches?sportId={sportId}&from={fromDate}&to={toDate}")]
        Task<IEnumerable<MatchDTO>> GetDailyMatches(int sportId, string fromDate, string toDate);
    }

    public class MatchService : BaseService, IMatchService
    {
        private const int CacheHours = 2;
        private const int CachedMonths = 1;
        private readonly ISoccerMatchApi soccerMatchApi;
        private readonly ISettingsService settingsService;
        private readonly ICacheService cacheService;
        private readonly IMapper mapper;

        public MatchService(
            ISoccerMatchApi soccerMatchApi,
            ISettingsService settingsService,
            ICacheService cacheService,
            ILoggingService loggingService,
            IMapper mapper) : base(loggingService)
        {
            this.settingsService = settingsService;
            this.cacheService = cacheService;
            this.soccerMatchApi = soccerMatchApi;
            this.mapper = mapper;
        }

        public async Task<IList<IMatch>> GetDailyMatches(DateTime fromDate, DateTime toDate, bool forceFetchNewData = false)
        {
            var sportId = settingsService.CurrentSportId;
            var matches = new List<IMatch>();
            var cacheExpiration = fromDate < DateTime.Now ? DateTime.Now.AddMonths(CachedMonths) : DateTime.Now.AddHours(CacheHours);
            var fromDateText = fromDate.ToApiFormat();
            var toDateText = toDate.ToApiFormat();

            try
            {
                // TODO Refactor DTO later
                var dtoMatches = await cacheService.GetAndFetchLatestValue(
                        $"DailyMatches{fromDateText}-{toDateText}",
                        () => soccerMatchApi.GetDailyMatches(sportId, fromDateText, toDateText),
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

        public Task<IList<IMatch>> GetMatchesByLeague(string leagueId, string group)
        {
            throw new NotImplementedException();
        }
    }
}
