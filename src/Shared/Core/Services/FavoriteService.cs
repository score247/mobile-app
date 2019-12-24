using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LiveScore.Common.Services;
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

        Func<Task> OnAddedFunc { get; set; }

        Func<Task> OnRemovedFunc { get; set; }

        Func<Task> OnReachedLimit { get; set; }
    }

    public class FavoriteService : IFavoriteService
    {
        private const string MatchKey = "Matches";
        private const string LeagueKey = "Leagues";

        private const int LeagueLimitation = 30;
        private const int MatchLimitation = 99;

        private readonly IUserSettingService userSettingService;

        private IList<IMatch> Matches;
        private IList<ILeague> Leagues;

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

        public void AddLeague(ILeague league)
        {
            if (Leagues.Count >= LeagueLimitation)
            {
                OnReachedLimit?.Invoke();
            }

            if (Leagues.All(m => m.Id != league.Id))
            {
                Leagues.Add(league);
            }

            Task.Run(() => userSettingService.AddOrUpdateValue(LeagueKey, Leagues)).ConfigureAwait(false);

            OnAddedFunc?.Invoke();
        }

        public void RemoveLeague(ILeague league)
        {
            var favoriteLeague = Leagues.FirstOrDefault(l => l.Id == league.Id);

            if (favoriteLeague != null)
            {
                Leagues.Remove(favoriteLeague);
            }

            Task.Run(() => userSettingService.AddOrUpdateValue(LeagueKey, Leagues)).ConfigureAwait(false);

            OnRemovedFunc?.Invoke();
        }

        public IList<ILeague> GetLeagues() => Leagues;

        public bool IsFavoriteLeague(string leagueId) => Leagues.Any(league => league.Id == leagueId);

        public void AddMatch(IMatch match)
        {
            if (Matches.All(m => m.Id != match.Id))
            {
                Matches.Add(match);
            }

            Task.Run(() => userSettingService.AddOrUpdateValue(MatchKey, Matches)).ConfigureAwait(false);
        }

        public void RemoveMatch(IMatch match)
        {
            if (Matches.Count >= MatchLimitation)
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

        private IList<ILeague> LoadLeaguesFromSetting()
            => userSettingService.GetValueOrDefault(LeagueKey, Enumerable.Empty<ILeague>()).ToList();

        private IList<IMatch> LoadMatchesFromSetting()
            => userSettingService.GetValueOrDefault(MatchKey, Enumerable.Empty<IMatch>()).ToList();
    }
}