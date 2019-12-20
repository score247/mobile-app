using System.Collections.Generic;
using LiveScore.Core.Models.Leagues;
using LiveScore.Core.Models.Matches;

namespace LiveScore.Core.Services
{
    public interface IFavouriteService
    {
        void AddMatch(IMatch match);

        void RemoveMatch(IMatch match);

        void AddLeague(ILeague league);

        void RemoveLeague(ILeague league);
    }
}
