﻿namespace LiveScore.Soccer.Services
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
        Task<IEnumerable<Match>> GetMatches(string sportId, string language, string fromDate, string toDate, string timezone);
    }

    public class MatchService : BaseService, IMatchService
    {        
        private readonly ILocalStorage cacheService;
        private readonly IApiPolicy apiPolicy;
        private readonly IApiService apiService;

        public MatchService(            
            ILocalStorage cacheService,
            ILoggingService loggingService,
            IApiPolicy apiPolicy,
            IApiService apiService
            ) : base(loggingService)
        {
            this.cacheService = cacheService;            
            this.apiPolicy = apiPolicy;

            this.apiService = apiService; 
        }

        public async Task<IEnumerable<IMatch>> GetMatches(UserSettings settings, DateRange dateRange, bool forceFetchNewData = false)
        {
            try
            {
                var cacheExpiration = dateRange.FromDate < DateTime.Now
                   ? cacheService.CacheDuration(CacheDurationTerm.Long)
                   : cacheService.CacheDuration(CacheDurationTerm.Short);

                var fromDateText = dateRange.FromDate.ToApiFormat();
                var toDateText = dateRange.ToDate.ToApiFormat();

                var cacheKey = $"Scores-{settings}-{fromDateText}-{toDateText}";

                return await cacheService.GetAndFetchLatestValue(
                        cacheKey,
                        () => GetMatches(settings, fromDateText, toDateText),
                        forceFetchNewData,
                        cacheExpiration);
            }
            catch (Exception ex)
            {
                HandleException(ex);

                return Enumerable.Empty<IMatch>();
            }
        }

        private async Task<IEnumerable<Match>> GetMatches(UserSettings settings, string fromDateText, string toDateText)
            => await apiPolicy.RetryAndTimeout(
                () => apiService.GetApi<ISoccerMatchApi>().GetMatches(settings.SportId, settings.Language, fromDateText, toDateText, settings.TimeZone));
    }
}