using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LiveScore.Common.Services;
using LiveScore.Core.Models.Leagues;
using LiveScore.Core.Models.Matches;
using LiveScore.Core.Services;
using LiveScore.Soccer.Models.Leagues;
using LiveScore.Soccer.Models.Matches;

namespace LiveScore.Soccer.Services
{
    public class FavoriteService : IFavoriteService
    {
        private const string MatchKey = "Matches";
        private const string LeagueKey = "Leagues";

        private const int LeagueLimitation = 30;
        private const int MatchLimitation = 99;

        private readonly IUserSettingService userSettingService;

        private IList<SoccerMatch> Matches;
        private IList<League> Leagues;

        public FavoriteService(IUserSettingService userSettingService)
        {
            this.userSettingService = userSettingService;

            Init();
        }

        public Func<Task> OnAddedFunc { get; set; }

        public Func<string,Task> OnRemovedFunc { get; set; }

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
                Leagues.Add(league as League);
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

            OnRemovedFunc?.Invoke(league.Id);
        }

        public IList<ILeague> GetLeagues() => Leagues.Select(league => league as ILeague).ToList();

        public bool IsFavoriteLeague(string leagueId) => Leagues.Any(league => league.Id == leagueId);

        public IList<IMatch> GetMatches() => Matches.Select(league => league as IMatch).ToList();

        public void AddMatch(IMatch match)
        {
            if (Matches.Count >= MatchLimitation)
            {
                OnReachedLimit?.Invoke();
            }

            if (Matches.All(m => m.Id != match.Id))
            {
                Matches.Add(match as SoccerMatch);
            }

            Task.Run(() => userSettingService.AddOrUpdateValue(MatchKey, Matches)).ConfigureAwait(false);

            OnAddedFunc?.Invoke();
        }

        public void RemoveMatch(IMatch match)
        {
            if (Matches.Any(m => m.Id == match.Id))
            {
                Matches.Remove(match as SoccerMatch);
            }

            Task.Run(() => userSettingService.AddOrUpdateValue(MatchKey, Matches)).ConfigureAwait(false);

            OnRemovedFunc?.Invoke(match.Id);
        }

        public bool IsFavoriteMatch(string matchId) => Matches.Any(match => match.Id == matchId);

        private IList<League> LoadLeaguesFromSetting()
            => userSettingService.GetValueOrDefault(LeagueKey, Enumerable.Empty<League>()).ToList();

        private IList<SoccerMatch> LoadMatchesFromSetting()
            => userSettingService.GetValueOrDefault(MatchKey, Enumerable.Empty<SoccerMatch>()).ToList();
    }
}
