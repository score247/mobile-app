using System.Collections.Generic;
using System.Linq;
using LiveScore.Common.Services;
using LiveScore.Core.Models.Leagues;
using LiveScore.Core.Models.Matches;
using LiveScore.Core.Services;

namespace LiveScore.Soccer.Services
{
    public class FavoriteService : IFavoriteService
    {
        private readonly string MatchKey = "Matches";
        private readonly string LeagueKey = "Leagues";

        private readonly IUserSettingService userSettingService;

        public IList<IMatch> Matches;
        public IList<FavoriteLeague> Leagues;

        public FavoriteService(IUserSettingService userSettingService)
        {
            this.userSettingService = userSettingService;

            Init();
        }
       

        public void Init()
        {
            Matches = LoadMatchesFromSetting();
            Leagues = LoadLeaguesFromSetting();
        }

        public void AddLeague(FavoriteLeague league)
        {
            if (!Leagues.Any(m => m.Id == league.Id))
            {
                Leagues.Add(league);
            }

            userSettingService.AddOrUpdateValue(LeagueKey, Leagues);
        }

        public void RemoveLeague(FavoriteLeague league)
        {
            if (Leagues.Any(m => m.Id == league.Id))
            {
                Leagues.Remove(league);
            }

            userSettingService.AddOrUpdateValue(LeagueKey, Leagues);
        }

        public IList<FavoriteLeague> GetLeagues() => Leagues;

        public bool IsFavoriteLeague(string leagueId) => Leagues.Any(league => league.Id == leagueId);

        public void AddMatch(IMatch match)
        {
            if (!Matches.Any(m => m.Id == match.Id))
            {
                Matches.Add(match);
            }

            userSettingService.AddOrUpdateValue(MatchKey, Matches);
        }

        public void RemoveMatch(IMatch match)
        {            
            if (Matches.Any(m => m.Id == match.Id))
            {
                Matches.Remove(match);
            }

            userSettingService.AddOrUpdateValue(MatchKey, Matches);
        }

        private IList<FavoriteLeague> LoadLeaguesFromSetting()
            => userSettingService.GetValueOrDefault(LeagueKey, Enumerable.Empty<FavoriteLeague>()).ToList();

        private IList<IMatch> LoadMatchesFromSetting()
            => userSettingService.GetValueOrDefault(MatchKey, Enumerable.Empty<IMatch>()).ToList();

        
    }
}
