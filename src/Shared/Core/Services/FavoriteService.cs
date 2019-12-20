using System.Collections.Generic;
using LiveScore.Core.Models.Leagues;
using LiveScore.Core.Models.Matches;

namespace LiveScore.Core.Services
{
    public interface IFavoriteService
    {
        void AddMatch(IMatch match);

        void RemoveMatch(IMatch match);

        void AddLeague(FavoriteLeague league);

        void RemoveLeague(FavoriteLeague league);

        IList<FavoriteLeague> GetLeagues();

        bool IsFavoriteLeague(string leagueId);
    }
}
