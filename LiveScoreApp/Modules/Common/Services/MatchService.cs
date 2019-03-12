namespace Common.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Common.Extensions;
    using Common.Models;
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
            this.matchApi = matchApi ?? RestService.For<IMatchApi>(Settings.Settings.BaseSportRadarEndPoint);
        }

        public async Task<IList<Match>> GetDailyMatches(DateTime date)
        {
            var sportName = Settings.Settings.SportNameMapper[Settings.Settings.CurrentSportName];
            var language = Settings.Settings.LanguageMapper[Settings.Settings.CurrentLanguage];
            var eventDate = date.ToSportRadarFormat();
            var apiKey = Settings.Settings.SportRadarApiKey;

            var dailySchedule = await matchApi.GetDailySchedules(sportName, language, eventDate, apiKey).ConfigureAwait(false);

            return dailySchedule.Matches;
        }
    }
}