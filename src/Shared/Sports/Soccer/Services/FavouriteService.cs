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
        }

        public void AddLeague(ILeague league)
        {
            var leagues = GetLeagues();

            if (!leagues.Any(m => m.Id == league.Id))
            {
                leagues.Append(league);
            }

            userSettingService.AddOrUpdateValue(LeagueKey, leagues);
        }

        public void RemoveLeague(ILeague league)
        {
            var leagues = GetLeagues();

            if (leagues.Any(m => m.Id == league.Id))
            {
                leagues.Remove(league);
            }

            userSettingService.AddOrUpdateValue(LeagueKey, leagues);
        }

        public IList<ILeague> GetLeagues()
            => userSettingService.GetValueOrDefault(LeagueKey, Enumerable.Empty<ILeague>()).ToList();

        public void AddMatch(IMatch match)
        {
            var matches = GetMatches();

            if (!matches.Any(m => m.Id == match.Id))
            {
                matches.Append(match);
            }

            userSettingService.AddOrUpdateValue(MatchKey, matches);
        }

        public void RemoveMatch(IMatch match)
        {
            var matches = GetMatches();

            if (matches.Any(m => m.Id == match.Id))
            {
                matches.Remove(match);
            }

            userSettingService.AddOrUpdateValue(MatchKey, matches);
        }

        public IList<IMatch> GetMatches()
            => userSettingService.GetValueOrDefault(MatchKey, Enumerable.Empty<IMatch>()).ToList();
    }
}
