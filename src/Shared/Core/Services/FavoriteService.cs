using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LiveScore.Core.Models.Leagues;
using LiveScore.Core.Models.Matches;

namespace LiveScore.Core.Services
{
    public interface IFavoriteService
    {
        IList<IMatch> GetMatches();

        void AddMatch(IMatch match);

        void RemoveMatch(IMatch match);

        bool IsFavoriteMatch(string matchId);

        IList<ILeague> GetLeagues();

        void AddLeague(ILeague league);

        void RemoveLeague(ILeague league);

        bool IsFavoriteLeague(string leagueId);

        Func<Task> OnAddedFunc { get; set; }

        Func<string, Task> OnRemovedFunc { get; set; }

        Func<Task> OnReachedLimit { get; set; }
    }
}