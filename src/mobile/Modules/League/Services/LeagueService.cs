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
        //https://api.sportradar.us/soccer-t3/eu/en/tournaments.json?api_key=vequ6wxdqyt7eg8qzh26dm5u
        Task<LeagueInfo> GetLeagues(string sport, string group, string lang, string key);
    }

    public interface ILeagueService
    {
        IList<League> GetAll();

        Task<IList<League>> GetAllAsync(string group, string sportName, string language);

        IList<Match> GetMatches(string leagueId);
    }

    internal class LeagueService : ILeagueService
    {
        private readonly ILeagueApi leagueApi;

        public LeagueService(ILeagueApi leagueApi)
        {
            this.leagueApi = leagueApi ?? RestService.For<ILeagueApi>(Settings.ApiEndPoint);
        }

        public IList<League> GetAll()
        {


            return new List<League>
            {
                new League { Id = "1", Name = "Champions League" },
                new League { Id = "2", Name = "Europa League"},
                new League { Id = Guid.NewGuid().ToString(), Name = "Premiere League" },
                new League { Id = Guid.NewGuid().ToString(), Name = "Bundesliga"},
                new League { Id = Guid.NewGuid().ToString(), Name = "Laliga" }
             
            };
        }

        public IList<Match> GetMatches(string leagueId)
        {
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

        public async Task<IList<League>> GetAllAsync(string group, string sportName, string language)
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
    }
}