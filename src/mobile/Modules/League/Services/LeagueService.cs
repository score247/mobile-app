namespace League.Services
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    using Common.Models;
    using Common.Models.MatchInfo;
    using Common.Settings;
    using Refit;

    public interface ILeagueApi
    {
        [Get("/{sport}-t3/{group}/{lang}/tournaments.json?api_key={key}")]
        Task<LeagueInfo> GetLeagues(string sport, string group, string lang, string key);

        [Get("/{sport}-t3/{group}/{lang}/tournaments/{leagueId}/schedule.json?api_key={key}")]
        Task<LeagueSchedule> GetMatches(string sport, string group, string lang, string leagueId, string key);
    }

    public interface ILeagueService
    {
        Task<IList<League>> GetAllAsync();

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

            switch (leagueId)
            {
                case "1":
                    return GetSoccerMatches();

                case "2":
                    return GetTennisMatches();

                case "3":
                    return GetESportMatches();

                case "4":
                    return GetHockeyMatches();

                default:
                    return GetSoccerMatches();
            }
        }

        private IList<Match> GetSoccerMatches()
        {
            return new List<Match>
                {
                   
                };
        }

        private IList<Match> GetTennisMatches()
        {
            return new List<Match>
                {
                   
                };
        }

        private IList<Match> GetESportMatches()
        {
            return new List<Match>
                {
                   
                };
        }

        private IList<Match> GetHockeyMatches()
        {
            return new List<Match>
                {
                    
                };
        }

        public async Task<IList<League>> GetAllAsync()
        {

            var sportNameSetting = Settings.SportNameMapper[Settings.CurrentSportName];
            var languageSetting = Settings.LanguageMapper[Settings.CurrentLanguage];
            var leagues = new List<League>();

            var tasks = Settings.LeagueGroups.Select(async (leagueGroup) =>
            {
                leagues.AddRange(await GetSportLeagues(leagueGroup, sportNameSetting, languageSetting).ConfigureAwait(false));
            });

            await Task.WhenAll(tasks);

            return leagues.OrderBy(x=>x.Name).ToList();
        }

        private async Task<IList<League>> GetSportLeagues(string group, string sportName, string language) 
        {
            var apiKeyByGroup = Settings.ApiKeyMapper[group];
            var leagues = new List<League>();

            try
            {
                var leaguesResult = await leagueApi.GetLeagues(sportName, group, language, apiKeyByGroup).ConfigureAwait(false);

                return leaguesResult.Leagues;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);

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
                Debug.WriteLine(ex.Message);

            }

            return matches;
        }
    }
}