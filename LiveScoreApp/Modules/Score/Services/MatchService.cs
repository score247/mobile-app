namespace Score.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Common.Extensions;
    using Common.Models;
    using Common.Settings;
    using Refit;

    public interface IMatchApi
    {
        [Get("/{sport}-t3/eu/{lang}/schedules/{date}/results.json?api_key={key}")]
        Task<DailySchedule> GetDailySchedules(string sport, string lang, string date, string key);
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

            var dailySchedule = await matchApi.GetDailySchedules(sportName, language, eventDate, apiKey).ConfigureAwait(false);
            dailySchedule.Matches.ToList().ForEach(match => match.Event.ShortEventDate = match.Event.EventDate.ToShortDayMonth());

            return dailySchedule.Matches;
        }
    }
}