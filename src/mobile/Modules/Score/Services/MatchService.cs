namespace Score.Services
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    using Common.Extensions;
    using Common.Helpers.Logging.Logging;
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
        Task<IList<Match>> GetDailyMatches(DateTime date);
    }

    public class MatchService : IMatchService
    {
        private readonly IMatchApi matchApi;
        private readonly ISettingsService settingsService;

        public MatchService(IMatchApi matchApi, ISettingsService settingsService)
        {
            this.settingsService = settingsService;
            this.matchApi = matchApi;
        }

        public async Task<IList<Match>> GetDailyMatches(DateTime date)
        {
            var currentSportName = settingsService.CurrentSportName;
            var sportName = settingsService.SportNameMapper[currentSportName];
            var language = settingsService.LanguageMapper["en-US"];
            var eventDate = date.ToSportRadarFormat();
            var matches = new List<Match>();

            var tasks = settingsService.LeagueGroups.Select(async (group) =>
            {
                matches.AddRange(await GetMatchesFromAPI(group, sportName, language, eventDate).ConfigureAwait(false));
            });

            await Task.WhenAll(tasks);

            return matches;
        }

        private async Task<IEnumerable<Match>> GetMatchesFromAPI(string group, string sportName, string language, string eventDate)
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