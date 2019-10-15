﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LiveScore.Common.Extensions;
using LiveScore.Common.Services;
using LiveScore.Core.Enumerations;
using LiveScore.Core.Models.Matches;
using LiveScore.Core.Services;
using LiveScore.Soccer.Models.Matches;
using MethodTimer;
using Refit;
using static LiveScore.Soccer.Services.SoccerApi;

namespace LiveScore.Soccer.Services
{
   

    public interface ISoccerMatchService
    {
        Task<MatchInfo> GetMatch(string matchId, Language language, bool forceFetchNewData = false);

        Task<MatchCoverage> GetMatchCoverage(string matchId, Language language, bool forceFetchNewData = false);

        Task<IEnumerable<MatchCommentary>> GetMatchCommentaries(string matchId, Language language, bool forceFetchNewData = false);

        Task<MatchStatistic> GetMatchStatistic(string matchId, Language language, bool forceFetchNewData = false);
    }

    public class MatchService : BaseService, IMatchService, ISoccerMatchService
    {
        private readonly IApiService apiService;
        private readonly ICacheManager cacheManager;
        private readonly MatchApi soccerMatchApi;

        public MatchService(
            IApiService apiService,
            ICacheManager cacheManager,
            ILoggingService loggingService,
            MatchApi soccerMatchApi = null) : base(loggingService)
        {
            this.apiService = apiService;
            this.cacheManager = cacheManager;
            this.soccerMatchApi = soccerMatchApi ?? apiService.GetApi<MatchApi>();
        }

        [Time]
        public async Task<IEnumerable<IMatch>> GetMatchesByDate(DateTime dateTime, Language language, bool getLatestData = false)
        {
            try
            {
                var cacheKey = $"Matches:{dateTime.Date}:{language.DisplayName}";

                var cacheDuration = dateTime.Date == DateTime.Today
                    || dateTime.Date == DateTimeExtension.Yesterday().Date
                    ? CacheDuration.Short
                    : CacheDuration.Long;

                return await cacheManager.GetOrSetAsync(
                    cacheKey,
                    () => apiService.Execute(() => soccerMatchApi.GetMatches(
                        dateTime.BeginningOfDay().ToApiFormat(),
                        dateTime.EndOfDay().ToApiFormat(),
                        language.DisplayName)), cacheDuration,
                    getLatestData).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                HandleException(ex);

                return Enumerable.Empty<IMatch>();
            }
        }

        [Time]
        public async Task<MatchInfo> GetMatch(string matchId, Language language, bool forceFetchNewData = false)
        {
            try
            {
                var cacheKey = $"Match:{matchId}:{language}";

                return await cacheManager.GetOrSetAsync(
                    cacheKey,
                    () => apiService.Execute(() => soccerMatchApi.GetMatchInfo(matchId, language.DisplayName)),
                    CacheDuration.Short, forceFetchNewData)
                    .ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                HandleException(ex);

                return null;
            }
        }

        public async Task<IEnumerable<IMatch>> GetLiveMatches(Language language, bool getLatestData = false)
        {
            try
            {
                var cacheKey = $"LiveMatches::{language.DisplayName}";

                return await cacheManager.GetOrSetAsync(
                        cacheKey,
                        () => apiService.Execute(() => soccerMatchApi.GetLiveMatches(language.DisplayName)),
                        CacheDuration.Short, getLatestData)
                    .ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                HandleException(ex);

                return Enumerable.Empty<IMatch>();
            }
        }

        public async Task<MatchCoverage> GetMatchCoverage(string matchId, Language language, bool forceFetchNewData = false)
        {
            try
            {
                var cacheKey = $"Match:{matchId}:{language}:coverage";

                return await cacheManager.GetOrSetAsync(
                        cacheKey,
                        () => apiService.Execute(() => soccerMatchApi.GetMatchCoverage(matchId, language.DisplayName)),
                        CacheDuration.Short, forceFetchNewData)
                    .ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                HandleException(ex);

                return new MatchCoverage { MatchId = matchId };
            }
        }

        public async Task<IEnumerable<MatchCommentary>> GetMatchCommentaries(string matchId, Language language, bool forceFetchNewData = false)
        {
            try
            {
                var cacheKey = $"Match:{matchId}:{language}:Commentaries";

                return await cacheManager.GetOrSetAsync(
                        cacheKey,
                        () => apiService.Execute(() => soccerMatchApi.GetMatchCommentaries(matchId, language.DisplayName)),
                        CacheDuration.Short, forceFetchNewData)
                    .ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                HandleException(ex);

                return Enumerable.Empty<MatchCommentary>();
            }
        }

        public async Task<MatchStatistic> GetMatchStatistic(string matchId, Language language, bool forceFetchNewData = false)
        {
            try
            {
                var cacheKey = $"Match:{matchId}:statistic";

                return await cacheManager.GetOrSetAsync(
                    cacheKey,
                    () => apiService.Execute(() => soccerMatchApi.GetMatchStatistic(matchId, language.DisplayName)),
                    CacheDuration.Short,
                    forceFetchNewData)
                    .ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                HandleException(ex);

                return new MatchStatistic(matchId);
            }
        }
    }
}