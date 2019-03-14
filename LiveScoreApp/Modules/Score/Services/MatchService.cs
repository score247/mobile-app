namespace Score.Services
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    using Common.Extensions;
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
            this.matchApi = matchApi ?? RestService.For<IMatchApi>(Settings.BaseSportRadarEndPoint);
        }

        public async Task<IList<Match>> GetDailyMatches(DateTime date)
        {
            var sportName = Settings.SportNameMapper[Settings.CurrentSportName];
            var language = Settings.LanguageMapper[Settings.CurrentLanguage];
            var eventDate = date.ToSportRadarFormat();
            var apiKey = Settings.SportRadarApiKey;
            var matches = new List<Match>();

            var tasks = Settings.SportRadarLeagueGroup.Select(async (group) =>
            {
                matches.AddRange(await GetMatchesFromAPI(group, sportName, language, eventDate));
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
                Debug.WriteLine(ex.Message);
                return Enumerable.Empty<Match>();
            }
        }
    }
}