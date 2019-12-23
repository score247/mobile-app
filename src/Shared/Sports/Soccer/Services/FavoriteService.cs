using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LiveScore.Common.Services;
using LiveScore.Core.Models.Leagues;
using LiveScore.Core.Models.Matches;
using LiveScore.Core.Services;

namespace LiveScore.Soccer.Services
{
    public class FavoriteService : IFavoriteService
    {
        private const string MatchKey = "Matches";
        private const string LeagueKey = "Leagues";

        private const int LeagueLimitation = 30;
        private const int MatchLimitation = 99;

        private readonly IUserSettingService userSettingService;

        private IList<IMatch> Matches;
        private IList<FavoriteLeague> Leagues;

        public FavoriteService(IUserSettingService userSettingService)
        {
            this.userSettingService = userSettingService;

            Init();            
        }

        public Func<Task> OnAddedFunc { get; set; }

        public Func<Task> OnRemovedFunc { get; set; }

        public Func<Task> OnReachedLimit { get; set; }

        public void Init()
        {
            Matches = LoadMatchesFromSetting();
            Leagues = LoadLeaguesFromSetting();
        }

        public void AddLeague(FavoriteLeague league)
        {
            if (Leagues.Count() >= LeagueLimitation)
            {
                OnReachedLimit?.Invoke();
            }

            if (!Leagues.Any(m => m.Id == league.Id))
            {
                Leagues.Add(league);
            }
            
            Task.Run(() => userSettingService.AddOrUpdateValue(LeagueKey, Leagues)).ConfigureAwait(false);

            OnAddedFunc?.Invoke();
        }

        public void RemoveLeague(FavoriteLeague league)
        {
            var favoriteLeague = Leagues.FirstOrDefault(l => l.Id == league.Id);
            if (favoriteLeague != null)
            {
                Leagues.Remove(favoriteLeague);
            }
            
            Task.Run(() => userSettingService.AddOrUpdateValue(LeagueKey, Leagues)).ConfigureAwait(false);

            OnRemovedFunc?.Invoke();
        }

        public IList<FavoriteLeague> GetLeagues() => Leagues;

        public bool IsFavoriteLeague(string leagueId) => Leagues.Any(league => league.Id == leagueId);

        public void AddMatch(IMatch match)
        {
            if (!Matches.Any(m => m.Id == match.Id))
            {
                Matches.Add(match);
            }

            Task.Run(() => userSettingService.AddOrUpdateValue(MatchKey, Matches)).ConfigureAwait(false);
        }

        public void RemoveMatch(IMatch match)
        {
            if (Matches.Count() >= MatchLimitation)
            {
                OnReachedLimit?.Invoke();
            }

            if (Matches.Any(m => m.Id == match.Id))
            {
                Matches.Remove(match);
            }

            Task.Run(() => userSettingService.AddOrUpdateValue(MatchKey, Matches)).ConfigureAwait(false);

            OnRemovedFunc?.Invoke();
        }

        private IList<FavoriteLeague> LoadLeaguesFromSetting()
            => userSettingService.GetValueOrDefault(LeagueKey, Enumerable.Empty<FavoriteLeague>()).ToList();

        private IList<IMatch> LoadMatchesFromSetting()
            => userSettingService.GetValueOrDefault(MatchKey, Enumerable.Empty<IMatch>()).ToList();
    }
}
