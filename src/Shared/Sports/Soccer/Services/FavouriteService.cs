using System.Collections.Generic;
using System.Linq;
using LiveScore.Common.Services;
using LiveScore.Core.Models.Leagues;
using LiveScore.Core.Models.Matches;
using LiveScore.Core.Services;

namespace LiveScore.Soccer.Services
{
    public class FavouriteService : IFavouriteService
    {
        private readonly string MatchKey = "Matches";
        private readonly string LeagueKey = "Leagues";

        private readonly IUserSettingService userSettingService;
        

        public FavouriteService(IUserSettingService userSettingService)
        {
            this.userSettingService = userSettingService;

            Init();
        }

        public IList<IMatch> Matches { get; private set; }
        
        public IList<ILeague> Leagues { get; private set; }

        public void Init()
        {
            Matches = GetMatches();
            Leagues = GetLeagues();
        }

        public void AddLeague(ILeague league)
        {
            if (!Leagues.Any(m => m.Id == league.Id))
            {
                Leagues.Append(league);
            }

            userSettingService.AddOrUpdateValue(LeagueKey, Leagues);
        }

        public void RemoveLeague(ILeague league)
        {
            if (Leagues.Any(m => m.Id == league.Id))
            {
                Leagues.Remove(league);
            }

            userSettingService.AddOrUpdateValue(LeagueKey, Leagues);
        }

        public void AddMatch(IMatch match)
        {
            if (!Matches.Any(m => m.Id == match.Id))
            {
                Matches.Append(match);
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

        private IList<ILeague> GetLeagues()
            => userSettingService.GetValueOrDefault(LeagueKey, Enumerable.Empty<ILeague>()).ToList();

        private IList<IMatch> GetMatches()
            => userSettingService.GetValueOrDefault(MatchKey, Enumerable.Empty<IMatch>()).ToList();
    }
}
