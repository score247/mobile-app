﻿namespace LiveScore.Soccer.Services
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
        [Get("/Match/GetMatches?sportId={sportId}&from={fromDate}&to={toDate}&language={language}")]
        Task<IEnumerable<MatchDTO>> GetMatches(int sportId, string language, string fromDate, string toDate);
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
            IApiPolicy apiPolicy) : base(loggingService)
        {
            this.cacheService = cacheService;
            this.soccerMatchApi = soccerMatchApi;
            this.mapper = mapper;
<<<<<<< refs/remotes/origin/master
            this.apiPolicy = apiPolicy;
=======
        }       

        public Task<IList<IMatch>> GetLiveMatches(int sportId, string languge)
        {
            throw new NotImplementedException();
>>>>>>> remove setting service out of matchservice change signature match service interface
        }

        public async Task<IList<IMatch>> GetMatches(
            int sportId, 
            string language, 
            DateTime fromDate, 
            DateTime toDate, 
            bool forceFetchNewData = false)
        {
            var matches = new List<IMatch>();
            var cacheExpiration = fromDate < DateTime.Now
                ? DateTime.Now.AddMinutes(OldMatchExpiration)
                : DateTime.Now.AddMinutes(TodayMatchExpiration);
            var fromDateText = fromDate.ToApiFormat();
            var toDateText = toDate.ToApiFormat();

            try
            {
                // TODO Refactor DTO later
                var dtoMatches = await cacheService.GetAndFetchLatestValue(
<<<<<<< refs/remotes/origin/master
                        $"DailyMatches{fromDateText}-{toDateText}",
                        () => GetMatches(sportId, fromDateText, toDateText),
=======
                        $"DailyMatches-{sportId}-{language}-{fromDateText}-{toDateText}",
                        () => soccerMatchApi.GetMatches(sportId, language, fromDateText, toDateText),
>>>>>>> remove setting service out of matchservice change signature match service interface
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

<<<<<<< refs/remotes/origin/master
        private async Task<IEnumerable<MatchDTO>> GetMatches(int sportId, string fromDateText, string toDateText)
            => await apiPolicy.RetryAndTimeout
                (
                    () => soccerMatchApi.GetDailyMatches(sportId, fromDateText, toDateText)
                );

        public Task<IList<IMatch>> GetMatchesByLeague(string leagueId, string group)
=======
        public Task<IList<IMatch>> GetMatchesByLeague(
            string leagueId, 
            string group)
>>>>>>> remove setting service out of matchservice change signature match service interface
        {
            throw new NotImplementedException();
        }
    }
}
