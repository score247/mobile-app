namespace Score.Services
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    using Common.Extensions;
    using Common.Models;
    using Common.Services;
    using Refit;

    public interface IMatchApi
    {
        [Get("/{sport}-t3/{group}/{lang}/schedules/{date}/results.json?api_key={key}")]
        Task<DailySchedule> GetDailySchedules(string sport, string group, string lang, string date, string key);
    }

    public interface IMatchService
    {
        Task<IList<Match>> GetDailyMatches(DateTime date, bool forceFetchNewData = false);
    }

    public class MatchService : IMatchService
    {
        private readonly IMatchApi matchApi;
        private readonly ISettingsService settingsService;
        private readonly ICacheService cacheService;

        public MatchService(IMatchApi matchApi, ISettingsService settingsService, ICacheService cacheService)
        {
            this.settingsService = settingsService;
            this.cacheService = cacheService;
            this.matchApi = matchApi;
        }

        public async Task<IList<Match>> GetDailyMatches(DateTime date, bool forceFetchNewData = false)
        {
            var currentSportName = settingsService.CurrentSportName;
            var sportName = settingsService.SportNameMapper[currentSportName];
            var language = settingsService.LanguageMapper["en-US"];
            var eventDate = date.ToSportRadarFormat();

            return await cacheService.GetAndFetchLatestValue(
                $"DailyMatches{eventDate}",
                () => GetMatchesByGroup(sportName, language, eventDate),
                forceFetchNewData,
                DateTime.Now.AddMinutes(5));
        }

        private async Task<IList<Match>> GetMatchesByGroup(string sportName, string language, string eventDate)
        {
            var matches = new List<Match>();

            var tasks = settingsService.LeagueGroups.Select(async (group) =>
            {
                matches.AddRange(await GetMatches(group, sportName, language, eventDate).ConfigureAwait(false));
            });

            await Task.WhenAll(tasks);

            return matches;
        }

        private async Task<IEnumerable<Match>> GetMatches(string group, string sportName, string language, string eventDate)
        {
            var apiKeyByGroup = settingsService.ApiKeyMapper[group];

            try
            {
                var dailySchedule = await matchApi.GetDailySchedules(sportName, group, language, eventDate, apiKeyByGroup).ConfigureAwait(false);
                dailySchedule.Matches.ToList().ForEach(match => match.Event.ShortEventDate = match.Event.EventDate.ToShortDayMonth());

                return dailySchedule.Matches;
            }
            catch (Exception ex)
            {
                LoggingService.LogError(ex);

                Debug.WriteLine(ex.Message);
                return Enumerable.Empty<Match>();
            }
        }
    }
}