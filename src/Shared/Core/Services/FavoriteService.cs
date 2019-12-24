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
        IList<IMatch> GetMatches();

        void AddMatch(IMatch match);

        void RemoveMatch(IMatch match);

        bool IsFavoriteMatch(string matchId);

        IList<ILeague> GetLeagues();

        void AddLeague(ILeague league);

        void RemoveLeague(ILeague league);

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

        public IList<IMatch> GetMatches() => Matches;

        public void AddMatch(IMatch match)
        {
            if (Matches.Count >= MatchLimitation)
            {
                OnReachedLimit?.Invoke();
            }

            if (Matches.All(m => m.Id != match.Id))
            {
                Matches.Add(match);
            }

            Task.Run(() => userSettingService.AddOrUpdateValue(MatchKey, Matches)).ConfigureAwait(false);

            OnAddedFunc?.Invoke();
        }

        public void RemoveMatch(IMatch match)
        {
            if (Matches.Any(m => m.Id == match.Id))
            {
                Matches.Remove(match);
            }

            Task.Run(() => userSettingService.AddOrUpdateValue(MatchKey, Matches)).ConfigureAwait(false);

            OnRemovedFunc?.Invoke();
        }

        public bool IsFavoriteMatch(string matchId) => Matches.Any(match => match.Id == matchId);

        private IList<ILeague> LoadLeaguesFromSetting()
            => userSettingService.GetValueOrDefault(LeagueKey, Enumerable.Empty<ILeague>()).ToList();

        private IList<IMatch> LoadMatchesFromSetting()
            => userSettingService.GetValueOrDefault(MatchKey, Enumerable.Empty<IMatch>()).ToList();
    }
}