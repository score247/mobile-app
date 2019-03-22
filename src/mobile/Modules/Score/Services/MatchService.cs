namespace Score.Services
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    using Common.Extensions;
    using Common.Helpers.Logging;
    using Common.Models;
    using Common.Settings;
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

        public MatchService(IMatchApi matchApi)
        {
            this.matchApi = matchApi ?? RestService.For<IMatchApi>(Settings.ApiEndPoint);
        }

        public async Task<IList<Match>> GetDailyMatches(DateTime date)
        {
            var sportName = Settings.SportNameMapper[Settings.CurrentSportName];
            var language = Settings.LanguageMapper[Settings.CurrentLanguage];
            var eventDate = date.ToSportRadarFormat();
            var matches = new List<Match>();

            var tasks = Settings.LeagueGroups.Select(async (group) =>
            {
                matches.AddRange(await GetMatchesFromAPI(group, sportName, language, eventDate).ConfigureAwait(false));
            });

            await Task.WhenAll(tasks);

            return matches;
        }

        private async Task<IEnumerable<Match>> GetMatchesFromAPI(string group, string sportName, string language, string eventDate)
        {
            var apiKeyByGroup = Settings.ApiKeyMapper[group];

            try
            {
                var dailySchedule = await matchApi.GetDailySchedules(sportName, group, language, eventDate, apiKeyByGroup).ConfigureAwait(false);
                dailySchedule.Matches.ToList().ForEach(match => match.Event.ShortEventDate = match.Event.EventDate.ToShortDayMonth());

                return dailySchedule.Matches;
            }
            catch (Exception ex)
            {
                if (ex is ApiException)
                {
                    LoggingService.LogError($"LeagueService request data for {((ApiException)ex).Uri.ToString()} occurs error", ex);
                }
                else
                {
                    LoggingService.LogError("LeagueService request data error", ex);
                }

                Debug.WriteLine(ex.Message);
                return Enumerable.Empty<Match>();
            }
        }
    }
}