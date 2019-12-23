using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LiveScore.Core.Models.Leagues;
using LiveScore.Core.Models.Matches;

namespace LiveScore.Core.Services
{
    public interface IFavoriteService
    {
        void AddMatch(IMatch match);

        void RemoveMatch(IMatch match);

        void AddLeague(ILeague league);

        void RemoveLeague(ILeague league);

        IList<ILeague> GetLeagues();

        bool IsFavoriteLeague(string leagueId);

        public Func<Task> OnAddedFunc { get; set; }

        public Func<Task> OnRemovedFunc { get; set; }

        public Func<Task> OnReachedLimit { get; set; }
    }
}
