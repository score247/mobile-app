namespace League.Services
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    using Common.Logging;
    using Common.Models;
    using Common.Models.MatchInfo;
    using Common.Settings;
    using League.Models;
    using Refit;

    public interface ILeagueApi
    {
        [Get("/{sport}-t3/{group}/{lang}/tournaments.json?api_key={key}")]
        Task<LeagueInfo> GetLeaguesByGroup(string sport, string group, string lang, string key);

        [Get("/{sport}-t3/{group}/{lang}/tournaments/{leagueId}/schedule.json?api_key={key}")]
        Task<LeagueSchedule> GetMatches(string sport, string group, string lang, string leagueId, string key);
    }

    public interface ILeagueService
    {
        Task<IList<Category>> GetCategories();

        Task<IList<Match>> GetMatches(string leagueId);
    }

    internal class LeagueService : ILeagueService
    {
        private readonly ILeagueApi leagueApi;

        public LeagueService(ILeagueApi leagueApi)
        {
            this.leagueApi = leagueApi ?? RestService.For<ILeagueApi>(Settings.ApiEndPoint);
        }
               
        public async Task<IList<Match>>  GetMatches(string leagueId)
        {
            var matchEvents = await GetMatchEvents("eu", "soccer", "en", leagueId);

            var matches = matchEvents.Select(x => new Match { Event = x }).ToList();

            return matches;
        }

        public async Task<IList<Category>> GetCategories()
        {

            var sportNameSetting = Settings.SportNameMapper[Settings.CurrentSportName];
            var languageSetting = Settings.LanguageMapper[Settings.CurrentLanguage];
            var leagues = new List<League>();

            var tasks = Settings.LeagueGroups.Select(async (leagueGroup) =>
            {
                leagues.AddRange(await GetLeaguesByGroup(leagueGroup, sportNameSetting, languageSetting).ConfigureAwait(false));
            });

            await Task.WhenAll(tasks);

            var categories = leagues.GroupBy(x => x.Category.Id).Select(g => g.First().Category).ToList();

            return categories;
        }

        private async Task<IList<League>> GetLeaguesByGroup(string group, string sportName, string language) 
        {
            var apiKeyByGroup = Settings.ApiKeyMapper[group];
            var leagues = new List<League>();

            try
            {
                var leaguesResult = await leagueApi.GetLeaguesByGroup(sportName, group, language, apiKeyByGroup).ConfigureAwait(false);

                return leaguesResult.Leagues;
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return leagues;
        }

        private async Task<IList<MatchEvent>> GetMatchEvents(string group, string sportName, string language, string leagueId)
        {
            var apiKeyByGroup = Settings.ApiKeyMapper[group];
            var matches = new List<MatchEvent>();

            try
            {
                var result = await leagueApi.GetMatches(sportName, group, language, leagueId, apiKeyByGroup).ConfigureAwait(false);

                return result.SportEvents;
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return matches;
        }

        private void HandleException(Exception ex) 
        {
            LoggingService.LogError("LeagueService request data error", ex);
            Debug.WriteLine(ex.Message);
        }
    }
}